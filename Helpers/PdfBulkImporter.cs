using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using AiNetStudio.models; // use your existing PdfType & PdfEntry model

public static class PdfBulkImporter
{
    private static readonly Regex RxUsGrant = new(@"US\s?\d{6,}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex RxKind = new(@"\b(A\d|B\d)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex RxDoi = new(@"10\.\d{4,9}/\S+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex RxYear = new(@"\b(19|20)\d{2}\b", RegexOptions.Compiled);

    public static async Task<int> ImportFolderAsync(
        string sourceFolder,
        string appDataRoot,                 // e.g., pm.GetWritableFolder("")
        string dbPath,                      // e.g., Path.Combine(appDataRoot, "TechArchive.aidb")
        IProgress<(int done, int total, string file)>? progress = null)
    {
        if (!Directory.Exists(sourceFolder)) throw new DirectoryNotFoundException(sourceFolder);

        // PDFs destination folder
        string pdfDir = Path.Combine(appDataRoot, "Library");
        Directory.CreateDirectory(pdfDir);

        // Open DB
        var cs = new SqliteConnectionStringBuilder
        {
            DataSource = dbPath,
            Cache = SqliteCacheMode.Shared,
            // IMPORTANT: do not create/overwrite DB here; require it to exist
            Mode = SqliteOpenMode.ReadWrite
        }.ToString();

        // Guard: refuse to create a new/empty DB; importer must not overwrite seed DBs
        if (!File.Exists(dbPath))
            throw new FileNotFoundException("Database not found for import. Importer will not create or overwrite.", dbPath);
        try
        {
            var sz = new FileInfo(dbPath).Length;
            if (sz < 1024) // treat tiny/empty DBs as invalid to avoid populating a placeholder
                throw new InvalidDataException($"Database appears empty or uninitialized: {dbPath} (size {sz} bytes)");
        }
        catch (IOException) { /* if we can't read size, let open attempt surface the real error */ }

        using var con = new SqliteConnection(cs);
        await con.OpenAsync();
        await EnsurePdfsTableAsync(con);

        var files = Directory.EnumerateFiles(sourceFolder, "*.pdf", SearchOption.TopDirectoryOnly).ToList();
        int total = files.Count;
        int count = 0;

        using var tx = con.BeginTransaction();

        foreach (var src in files)
        {
            progress?.Report((count, total, src));

            var guid = Guid.NewGuid();
            string guidName = guid.ToString("N").ToUpperInvariant() + ".pdf";
            string dest = Path.Combine(pdfDir, guidName);
            File.Copy(src, dest, overwrite: false);

            string relPath = Path.Combine("Library", guidName).Replace('\\', '/');
            string fileName = Path.GetFileName(src);

            var title = Path.GetFileNameWithoutExtension(src);
            var (type, country, kind, patNum, doi) = GuessTypeAndIdsFromName(title);

            // Optional: sniff a year from filename tokens to seed PublicationDate
            string? pubDateStr = null;
            var mYear = RxYear.Match(title);
            if (mYear.Success && int.TryParse(mYear.Value, out int y))
            {
                // store ISO date string; your model uses DateTime? so you can parse on read if desired
                pubDateStr = new DateTime(y, 1, 1).ToString("yyyy-MM-dd");
            }

            using var cmd = con.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = @"
INSERT INTO Pdfs (
  PdfGuid, PdfPath, FileName, Type, Title, Description, Authors,
  Category, SubCategory, Tags, Notes, Rank,
  PublicationDate, DOI, CountryCode, KindCode, PatentNumber, CPC, IPC, PriorityDataJson,
  AddedUtc, LastViewedUtc
) VALUES (
  $PdfGuid, $PdfPath, $FileName, $Type, $Title, $Description, $Authors,
  $Category, $SubCategory, $Tags, $Notes, $Rank,
  $PublicationDate, $DOI, $CountryCode, $KindCode, $PatentNumber, $CPC, $IPC, $PriorityDataJson,
  $AddedUtc, $LastViewedUtc
);";

            cmd.Parameters.AddWithValue("$PdfGuid", guid.ToByteArray());
            cmd.Parameters.AddWithValue("$PdfPath", relPath);
            cmd.Parameters.AddWithValue("$FileName", fileName);
            cmd.Parameters.AddWithValue("$Type", (int)type);
            cmd.Parameters.AddWithValue("$Title", title);
            cmd.Parameters.AddWithValue("$Description", "");                // matches PdfEntry.Description
            cmd.Parameters.AddWithValue("$Authors", (object)DBNull.Value);  // matches PdfEntry.Authors
            cmd.Parameters.AddWithValue("$Category", "Uncategorized");
            cmd.Parameters.AddWithValue("$SubCategory", (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$Tags", (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$Notes", (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$Rank", 0);
            cmd.Parameters.AddWithValue("$PublicationDate", (object?)pubDateStr ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$DOI", (object?)doi ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$CountryCode", (object?)country ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$KindCode", (object?)kind ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$PatentNumber", (object?)patNum ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$CPC", (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$IPC", (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$PriorityDataJson", (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$AddedUtc", DateTime.UtcNow.ToString("o"));
            cmd.Parameters.AddWithValue("$LastViewedUtc", (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
            count++;
            progress?.Report((count, total, src));
        }

        tx.Commit();
        return count;
    }

    private static (PdfType Type, string? Country, string? Kind, string? PatentNumber, string? Doi)
        GuessTypeAndIdsFromName(string name)
    {
        var type = PdfType.Other;
        string? country = null;
        string? kind = null;
        string? patent = null;
        string? doi = null;

        var md = RxDoi.Match(name);
        if (md.Success) { doi = md.Value; type = PdfType.Article; }

        var mp = RxUsGrant.Match(name);
        if (mp.Success)
        {
            type = PdfType.Patent;
            country = "US";
            patent = mp.Value.Replace(" ", "");
        }

        var mk = RxKind.Match(name);
        if (mk.Success) kind = mk.Value.ToUpperInvariant();

        return (type, country, kind, patent, doi);
    }

    private static async Task EnsurePdfsTableAsync(SqliteConnection con)
    {
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Pdfs (
  PdfGuid          BLOB PRIMARY KEY,
  PdfPath          TEXT NOT NULL,
  FileName         TEXT NOT NULL,
  Type             INTEGER NOT NULL,
  Title            TEXT NOT NULL,
  Description      TEXT,
  Authors          TEXT,
  Category         TEXT NOT NULL DEFAULT 'Uncategorized',
  SubCategory      TEXT,
  Tags             TEXT,
  Notes            TEXT,
  Rank             INTEGER DEFAULT 0,
  PublicationDate  TEXT,
  DOI              TEXT,
  CountryCode      TEXT,
  KindCode         TEXT,
  PatentNumber     TEXT,
  CPC              TEXT,
  IPC              TEXT,
  PriorityDataJson TEXT,
  AddedUtc         TEXT NOT NULL,
  LastViewedUtc    TEXT
);
";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = @"
CREATE INDEX IF NOT EXISTS idx_pdfs_type ON Pdfs(Type);
CREATE INDEX IF NOT EXISTS idx_pdfs_category ON Pdfs(Category, SubCategory);
CREATE INDEX IF NOT EXISTS idx_pdfs_patent ON Pdfs(PatentNumber);
CREATE INDEX IF NOT EXISTS idx_pdfs_doi ON Pdfs(DOI);
";
        await cmd.ExecuteNonQueryAsync();

        // Optional: FTS for search across key text fields
        // Uncomment if/when you want full-text search
        /*
        cmd.CommandText = @"
CREATE VIRTUAL TABLE IF NOT EXISTS Pdfs_fts
USING fts5(
  Title, Description, Tags, Notes, Category, SubCategory,
  content='',
  tokenize='unicode61 remove_diacritics 1'
);";
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = @"
CREATE TRIGGER IF NOT EXISTS Pdfs_ai AFTER INSERT ON Pdfs BEGIN
  INSERT INTO Pdfs_fts(rowid, Title, Description, Tags, Notes, Category, SubCategory)
  VALUES (new.rowid, new.Title, new.Description, new.Tags, new.Notes, new.Category, new.SubCategory);
END;
CREATE TRIGGER IF NOT EXISTS Pdfs_ad AFTER DELETE ON Pdfs BEGIN
  INSERT INTO Pdfs_fts(Pdfs_fts, rowid, Title, Description, Tags, Notes, Category, SubCategory)
  VALUES ('delete', old.rowid, old.Title, old.Description, old.Tags, old.Notes, old.Category, old.SubCategory);
END;
CREATE TRIGGER IF NOT EXISTS Pdfs_au AFTER UPDATE ON Pdfs BEGIN
  INSERT INTO Pdfs_fts(Pdfs_fts, rowid, Title, Description, Tags, Notes, Category, SubCategory)
  VALUES ('delete', old.rowid, old.Title, old.Description, old.Tags, old.Notes, old.Category, old.SubCategory);
  INSERT INTO Pdfs_fts(rowid, Title, Description, Tags, Notes, Category, SubCategory)
  VALUES (new.rowid, new.Title, new.Description, new.Tags, new.Notes, new.Category, new.SubCategory);
END;";
        await cmd.ExecuteNonQueryAsync();
        */
    }
}
