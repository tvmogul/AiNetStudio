using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AiNetStudio.DataAccess
{

    public static class DbInitializer
    {
        // Bump when schema changes (v1 = base PDFs schema)
        private const int SchemaVersion = 1;

        public static void EnsurePatentsSchema(string dbPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

            var cs = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                Cache = SqliteCacheMode.Shared,
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();

            using var con = new SqliteConnection(cs);
            con.Open();

            using var enableFk = con.CreateCommand();
            enableFk.CommandText = "PRAGMA foreign_keys=ON;";
            enableFk.ExecuteNonQuery();

            int currentVersion;
            using (var getVer = con.CreateCommand())
            {
                getVer.CommandText = "PRAGMA user_version;";
                currentVersion = Convert.ToInt32(getVer.ExecuteScalar());
            }

            using var tx = con.BeginTransaction();

            // v1: initial schema (PDFs table matching PdfEntry, plus FTS over text fields)
            if (currentVersion < 1)
            {
                Exec(con, tx, @"
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
                    ");

                // FTS for search (includes Category/SubCategory)
                Exec(con, tx, @"
                    CREATE VIRTUAL TABLE IF NOT EXISTS Pdfs_fts
                    USING fts5(
                      Title, Description, Tags, Notes, Category, SubCategory,
                      content='',
                      tokenize='unicode61 remove_diacritics 1'
                    );");

                // Triggers to keep FTS in sync
                Exec(con, tx, @"
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
                    END;
                    ");

                // Helpful indexes
                Exec(con, tx, @"CREATE INDEX IF NOT EXISTS idx_pdfs_type ON Pdfs(Type);");
                Exec(con, tx, @"CREATE INDEX IF NOT EXISTS idx_pdfs_category ON Pdfs(Category, SubCategory);");
                Exec(con, tx, @"CREATE INDEX IF NOT EXISTS idx_pdfs_patent ON Pdfs(PatentNumber);");
                Exec(con, tx, @"CREATE INDEX IF NOT EXISTS idx_pdfs_doi ON Pdfs(DOI);");

                Exec(con, tx, "PRAGMA user_version=1;");
                currentVersion = 1;
            }

            tx.Commit();
        }

        private static void Exec(SqliteConnection con, SqliteTransaction tx, string sql)
        {
            using var cmd = con.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private static void TryExec(SqliteConnection con, SqliteTransaction tx, string sql)
        {
            try { Exec(con, tx, sql); }
            catch (SqliteException) { /* ignore when column/table already exists */ }
        }
    }
}






