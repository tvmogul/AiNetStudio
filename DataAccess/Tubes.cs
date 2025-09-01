using AiNetStudio.Models;
using CustomControls;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;

namespace AiNetStudio.DataAccess
{
    public sealed class Tubes
    {
        private static readonly object _initLock = new();
        private static bool _initialized;
        private static Dictionary<string, int>? _headerIndexCache;

        // --------------------------------------------------------------------
        // Public API
        // --------------------------------------------------------------------

        /// <summary>
        /// Ensures the database file and Feeds table exist. Safe to call multiple times.
        /// </summary>
        public static void EnsureDatabase(bool seedIfEmpty = true)
        {
            if (_initialized) return;

            lock (_initLock)
            {
                if (_initialized) return;

                try
                {
                    // Uses PathManager (separate class) to resolve folder.
                    var dbPath = GetDbPath(); 
                    var dir = Path.GetDirectoryName(dbPath)!;
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (var con = OpenConnection())
                    {
                        try
                        {
                            using (var pragma = con.CreateCommand())
                            {
                                // Keep everything in a single database file; avoid extra -wal/-shm or temp files
                                pragma.CommandText = "PRAGMA journal_mode=DELETE; PRAGMA foreign_keys=ON; PRAGMA temp_store=MEMORY;";
                                pragma.ExecuteNonQuery();
                            }
                        }
                        catch (SqliteException ex) { ReportError("EnsureDatabase/PRAGMA", ex); throw; }
                        catch (Exception ex) { ReportError("EnsureDatabase/PRAGMA", ex); throw; }

                        try
                        {
                            using (var cmd = con.CreateCommand())
                            {
                                // Create Feeds table (SQLite-friendly mapping of your MSSQL schema)
                                cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Feeds (
                            FeedId           TEXT    NOT NULL PRIMARY KEY,  -- uniqueidentifier as GUID string (TEXT)
                            category         TEXT    NOT NULL,
                            subcategory      TEXT,
                            catsub           TEXT,
                            groupcategory    TEXT,
                            moviecategory    TEXT    NOT NULL,
                            rank             INTEGER NOT NULL,
                            title            TEXT,
                            author           TEXT,
                            link             TEXT,
                            linkType         TEXT,
                            linkValue        TEXT,
                            shortDescription TEXT,
                            description      TEXT,
                            bodyLinks        TEXT,
                            image            TEXT,
                            publishedDate    TEXT,                          -- ISO-8601 string (optional)
                            duration         TEXT,
                            tags             TEXT
                        );
                        CREATE INDEX IF NOT EXISTS IX_Feeds_Group ON Feeds(groupcategory);
                        CREATE INDEX IF NOT EXISTS IX_Feeds_Rank ON Feeds(rank);
                        CREATE INDEX IF NOT EXISTS IX_Feeds_Title ON Feeds(title COLLATE NOCASE);
                        CREATE INDEX IF NOT EXISTS IX_Feeds_Category ON Feeds(category, subcategory);
                        ";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (SqliteException ex) { ReportError("EnsureDatabase/CreateTable", ex); throw; }
                        catch (Exception ex) { ReportError("EnsureDatabase/CreateTable", ex); throw; }

                        if (seedIfEmpty)
                        {
                            try
                            {
                                // If empty, import from CSV only.
                                if (GetRowCount(con) == 0)
                                {
                                    // Try common locations: alongside the
                                    // DB first, then fallback paths.
                                    // exeDir + Libs
                                    var dbDir = Path.GetDirectoryName(dbPath)!;

                                    PathManager pmx = new PathManager();

                                    //SOURCE
                                    //Path.Combine(pmx.GetInstallationFolder("Libs"), "feeds_subset.csv")
                                    //Path.Combine(pmx.GetInstallationFolder("Libs"), "feeds.csv")

                                    //DESTINATION
                                    //Path.Combine(pmx.GetWritableFolder("Libs"), "feeds_subset.csv")
                                    //Path.Combine(pmx.GetWritableFolder("Libs"), "feeds.csv")

                                    string installLibs = pmx.GetInstallationFolder("Libs");
                                    string writableLibs = pmx.GetWritableFolder("Libs");
                                    Directory.CreateDirectory(writableLibs);

                                    // Candidates only from installation Libs
                                    string[] sourceCandidates =
                                    {
                                        Path.Combine(installLibs, "feeds_subset.csv"),
                                        Path.Combine(installLibs, "feeds.csv")
                                    };

                                    string? csvPath = null;

                                    // If a source exists in the installation folder, copy it to writable and use that
                                    var firstSource = sourceCandidates.FirstOrDefault(File.Exists);
                                    if (!string.IsNullOrEmpty(firstSource))
                                    {
                                        var destPath = Path.Combine(writableLibs, Path.GetFileName(firstSource));
                                        File.Copy(firstSource, destPath, overwrite: true);
                                        csvPath = destPath;
                                    }

                                    if (!string.IsNullOrEmpty(csvPath))
                                    {
                                        ImportCsvIfEmpty(con, csvPath);
                                    }
                                }
                            }
                            catch (SqliteException ex) { ReportError("EnsureDatabase/ImportCsvIfEmpty", ex); throw; }
                            catch (Exception ex) { ReportError("EnsureDatabase/ImportCsvIfEmpty", ex); throw; }

                        }
                    }

                    _initialized = true;
                }
                catch (SqliteException ex) { ReportError("EnsureDatabase", ex); throw; }
                catch (Exception ex) { ReportError("EnsureDatabase", ex); throw; }
            }
        }

        /// <summary>
        /// Scans all feeds with YouTube links and removes those that are no longer available.
        /// Uses the YouTube oEmbed endpoint for validation.
        /// </summary>
        public static void CleanUnavailableYouTubeVideos()
        {
            try
            {
                //EnsureDatabase();

                using var con = OpenConnection();
                using var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT FeedId, link FROM Feeds WHERE link LIKE 'https://%youtube.com/watch%'";
                using var rdr = cmd.ExecuteReader();

                var badIds = new List<string>();
                using var http = new HttpClient();

                while (rdr.Read())
                {
                    var feedId = rdr.GetString(0);
                    var link = rdr.IsDBNull(1) ? null : rdr.GetString(1);
                    if (string.IsNullOrWhiteSpace(link)) continue;

                    // Extract the videoId from link (basic parsing for v=VIDEOID)
                    var uri = new Uri(link);
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    var videoId = query.Get("v");
                    if (string.IsNullOrWhiteSpace(videoId)) continue;

                    var url = $"https://www.youtube.com/oembed?url=http://www.youtube.com/watch?v={videoId}&format=json";
                    try
                    {
                        var resp = http.GetAsync(url).Result;
                        if (resp.StatusCode != HttpStatusCode.OK)
                        {
                            badIds.Add(feedId);
                        }
                    }
                    catch
                    {
                        badIds.Add(feedId);
                    }
                }

                rdr.Close();

                if (badIds.Count > 0)
                {
                    using var tx = con.BeginTransaction();
                    using var del = con.CreateCommand();
                    del.Transaction = tx;
                    del.CommandText = "DELETE FROM Feeds WHERE FeedId = $id";

                    foreach (var id in badIds)
                    {
                        del.Parameters.Clear();
                        del.Parameters.AddWithValue("$id", id);
                        del.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                ReportError("CleanUnavailableYouTubeVideos", ex);
                throw;
            }
        }

        /// <summary>
        /// Distinct top-level categories.
        /// </summary>
        public static List<string> GetCategories()
        {
            try
            {
                //EnsureDatabase();
                var list = new List<string>();
                using var con = OpenConnection();
                using var cmd = con.CreateCommand();
                //cmd.CommandText = @"SELECT DISTINCT category FROM Feeds WHERE IFNULL(category,'') <> '' ORDER BY category COLLATE NOCASE;";
                cmd.CommandText = @"
                    SELECT DISTINCT category
                    FROM Feeds
                    WHERE IFNULL(category,'') <> ''
                      AND category <> 'All Watch'
                    ORDER BY category COLLATE NOCASE;";
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read()) list.Add(rdr.GetString(0));
                return list;
            }
            catch (SqliteException ex) { ReportError("GetCategories", ex); throw; }
            catch (Exception ex) { ReportError("GetCategories", ex); throw; }
        }

        /// <summary>
        /// Distinct subcategories for a given category.
        /// </summary>
        public static List<string> GetSubcategories(string category)
        {
            try
            {
                //EnsureDatabase();
                var list = new List<string>();
                using var con = OpenConnection();
                using var cmd = con.CreateCommand();
                cmd.CommandText = @"
            SELECT DISTINCT COALESCE(subcategory,'') AS sc
            FROM Feeds
            WHERE category = $c
            ORDER BY sc COLLATE NOCASE;";
                cmd.Parameters.AddWithValue("$c", category ?? "");
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read()) list.Add(rdr.GetString(0));
                return list;
            }
            catch (SqliteException ex) { ReportError("GetSubcategories", ex); throw; }
            catch (Exception ex) { ReportError("GetSubcategories", ex); throw; }
        }

        /// <summary>
        /// Get distinct groupcategory values for a category, 
        /// optionally filtered by a list of subcategories.
        /// Returns an empty list if none found.
        /// </summary>
        public static List<string> GetGroupCategories(string category, List<string> subcategory)
        {
            var results = new List<string>();
            var cat = category ?? string.Empty;

            // normalize subcategory list: trim, drop null/empty
            var subs = (subcategory ?? new List<string>())
                       .Where(s => !string.IsNullOrWhiteSpace(s))
                       .Select(s => s.Trim())
                       .Distinct(StringComparer.OrdinalIgnoreCase)
                       .ToList();

            try
            {
                using var con = OpenConnection();
                using var cmd = con.CreateCommand();

                var sql = @"
            SELECT DISTINCT COALESCE(groupcategory,'') AS gc
            FROM Feeds
            WHERE category = $c
              AND groupcategory IS NOT NULL
              AND TRIM(groupcategory) <> ''";

                if (subs.Count == 1)
                {
                    sql += " AND subcategory = $s0";
                    cmd.Parameters.AddWithValue("$s0", subs[0]);
                }
                else if (subs.Count > 1)
                {
                    // Build IN ($s0, $s1, ...)
                    var placeholders = string.Join(", ", subs.Select((_, i) => $"$s{i}"));
                    sql += $" AND subcategory IN ({placeholders})";
                    for (int i = 0; i < subs.Count; i++)
                        cmd.Parameters.AddWithValue($"$s{i}", subs[i]);
                }

                sql += " ORDER BY gc COLLATE NOCASE;";

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("$c", cat);

                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    results.Add(rdr.GetString(0));

                return results;
            }
            catch (SqliteException)
            {
                return results;
            }
            catch (Exception)
            {
                return results;
            }
        }


        /// <summary>
        /// Returns feeds filtered by category and optional subcategory.
        /// </summary>
        public static List<FeedItem> GetFeedsByCategory(string category, string? subcategory = null, int take = 500, int skip = 0)
        {
            try
            {
                //EnsureDatabase();
                if (string.IsNullOrWhiteSpace(category)) category = "";

                var items = new List<FeedItem>();
                using var con = OpenConnection();
                using var cmd = con.CreateCommand();

                if (string.IsNullOrWhiteSpace(subcategory))
                {
                    cmd.CommandText = @"
                SELECT FeedId, category, subcategory, catsub, groupcategory, moviecategory, rank, title, author, link, linkType, linkValue, shortDescription, description, bodyLinks, image, publishedDate, duration, tags
                FROM Feeds
                WHERE category = $c
                ORDER BY rank ASC, title COLLATE NOCASE
                LIMIT $take OFFSET $skip;";
                    cmd.Parameters.AddWithValue("$c", category);
                }
                else
                {
                    cmd.CommandText = @"
                SELECT FeedId, category, subcategory, catsub, groupcategory, moviecategory, rank, title, author, link, linkType, linkValue, shortDescription, description, bodyLinks, image, publishedDate, duration, tags
                FROM Feeds
                WHERE category = $c AND COALESCE(subcategory,'') = $sc
                ORDER BY rank ASC, title COLLATE NOCASE
                LIMIT $take OFFSET $skip;";
                    cmd.Parameters.AddWithValue("$c", category);
                    cmd.Parameters.AddWithValue("$sc", subcategory);
                }

                cmd.Parameters.AddWithValue("$take", Math.Clamp(take, 1, 5000));
                cmd.Parameters.AddWithValue("$skip", Math.Max(0, skip));

                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    items.Add(new FeedItem(
                        FeedId: rdr.GetString(0),
                        Category: rdr.IsDBNull(1) ? null : rdr.GetString(1),
                        Subcategory: rdr.IsDBNull(2) ? null : rdr.GetString(2),
                        CatSub: rdr.IsDBNull(3) ? null : rdr.GetString(3),
                        GroupCategory: rdr.IsDBNull(4) ? null : rdr.GetString(4),
                        MovieCategory: rdr.IsDBNull(5) ? null : rdr.GetString(5),
                        Rank: rdr.IsDBNull(6) ? 0 : rdr.GetInt32(6),
                        Title: rdr.IsDBNull(7) ? null : rdr.GetString(7),
                        Author: rdr.IsDBNull(8) ? null : rdr.GetString(8),
                        Link: rdr.IsDBNull(9) ? null : rdr.GetString(9),
                        LinkType: rdr.IsDBNull(10) ? null : rdr.GetString(10),
                        LinkValue: rdr.IsDBNull(11) ? null : rdr.GetString(11),
                        ShortDescription: rdr.IsDBNull(12) ? null : rdr.GetString(12),
                        Description: rdr.IsDBNull(13) ? null : rdr.GetString(13),
                        BodyLinks: rdr.IsDBNull(14) ? null : rdr.GetString(14),
                        Image: rdr.IsDBNull(15) ? null : rdr.GetString(15),
                        PublishedDate: rdr.IsDBNull(16) ? null : rdr.GetString(16),
                        Duration: rdr.IsDBNull(17) ? null : rdr.GetString(17),
                        Tags: rdr.IsDBNull(18) ? null : rdr.GetString(18)
                    ));
                }

                return items;
            }
            catch (SqliteException ex) { ReportError("GetFeedsByCategory", ex); throw; }
            catch (Exception ex) { ReportError("GetFeedsByCategory", ex); throw; }
        }

        public static string BuildFeedsHtml(string category, string? subcategory, IEnumerable<FeedItem> feeds)
        {
            var sb = new StringBuilder();
            sb.Append("""
                <!doctype html>
                <html lang="en">
                <head>
                <meta charset="utf-8">
                <title>Feeds</title>
                <meta name="viewport" content="width=device-width, initial-scale=1">
                <style>
                  :root{
                    --bg:#ffffff; --fg:#000000; --muted:#444444; --accent:#007bff; --card:#f7f7f7; --chip:#e0e0e0;
                  }
                  html,body{margin:0;padding:0;background:var(--bg);color:var(--fg);font:14px/1.5 -apple-system, Segoe UI, Roboto, Helvetica, Arial, sans-serif;}
                  header{position:sticky;top:0;background:linear-gradient(180deg,var(--bg),rgba(255,255,255,.9));backdrop-filter:saturate(140%) blur(6px);padding:12px 16px;border-bottom:1px solid #ccc;z-index:2}
                  h1{font-size:16px;margin:0}
                  .muted{color:var(--muted)}
                  .list{padding:10px 8px 32px;max-width:1100px;margin:0 auto;}
                  .item{display:flex;gap:12px;align-items:flex-start;padding:10px 12px;margin:8px 0;border-radius:10px;background:var(--card);border:1px solid #ddd}
                  .rank{min-width:48px;text-align:center;background:var(--chip);border-radius:8px;padding:6px 8px;color:var(--muted)}
                  .title{font-weight:600;margin:0 0 4px 0}
                  .meta{font-size:12px;color:var(--muted)}
                  a{color:var(--accent);text-decoration:none}
                  a:hover{text-decoration:underline}
                  .kv{display:grid;grid-template-columns:120px 1fr;gap:4px 10px;font-size:12px;margin-top:6px;color:var(--muted)}
                  .kv div:nth-child(odd){opacity:.8}
                  .thumb{width:80px;height:80px;object-fit:cover;border-radius:6px;background:#eee}
                </style>
                </head>
                <body>
                """);

            var subtitle = string.IsNullOrWhiteSpace(subcategory)
                ? $"{category}"
                : $"{category} • {subcategory}";

            sb.Append($"""
                <header>
                  <h1>Feeds <span class="muted">({System.Net.WebUtility.HtmlEncode(subtitle)})</span></h1>
                </header>
                <a href="EVENT:action:value" 
                   style="display:inline-block;
                          padding:10px 20px;
                          background-color:#007bff;
                          color:#ffffff;
                          text-decoration:none;
                          font-family:sans-serif;
                          border-radius:6px;
                          font-weight:bold;">
                    Test Event Button
                </a>
    
                <div class="list">
                """);

            int n = 0;
            foreach (var f in feeds)
            {
                n++;
                var title = Safe(f.Title, "(untitled)");
                var author = Safe(f.Author, "");
                var url = Safe(f.Link, "#");
                var published = Safe(f.PublishedDate, "");
                sb.Append($"""
                    <div class="item" style="max-width: 480px !important;"
                         data-feedid="{Safe(f.FeedId)}"
                         data-category="{Safe(f.Category)}"
                         data-subcategory="{Safe(f.Subcategory)}"
                         data-catsub="{Safe(f.CatSub)}"
                         data-groupcategory="{Safe(f.GroupCategory)}"
                         data-moviecategory="{Safe(f.MovieCategory)}"
                         data-rank="{f.Rank}"
                         data-title="{Safe(f.Title)}"
                         data-author="{Safe(f.Author)}"
                         data-link="{Safe(f.Link)}"
                         data-linktype="{Safe(f.LinkType)}"
                         data-linkvalue="{Safe(f.LinkValue)}"
                         data-shortdescription="{Safe(f.ShortDescription)}"
                         data-description="{Safe(f.Description)}"
                         data-bodylinks="{Safe(f.BodyLinks)}"
                         data-image="{Safe(f.Image)}"
                         data-publisheddate="{Safe(f.PublishedDate)}"
                         data-duration="{Safe(f.Duration)}"
                         data-tags="{Safe(f.Tags)}">
          
                      <!-- Square image on the left -->
                      <div class="item-image">
                          <img class="thumb" src="{Safe(f.Image)}" alt="{title}"
                              style="width:100px;height:100px;object-fit:cover;border-radius:10px;">
                      </div>

                      <!-- Content on the right -->
                      <div class="item-content">
                        <div class="title mb-1">
                          <a href="{url}" target="_blank" rel="noopener">{title}</a>
                        </div>
                        <div class="meta" style="margin-top:6px;">{Safe(f.ShortDescription)}</div>

                        <!-- Every field returned below as key/value -->
                        <div class="kv">
                        </div>
                      </div>
                    </div>
                    """);
            }

            if (n == 0)
            {
                sb.Append("""
                  <div class="item">
                    <div>No feeds found for this selection.</div>
                  </div>
                """);
            }

            sb.Append("""
                </div>
                </body>
                </html>
                """);

            return sb.ToString();

            static string Safe(string? s, string fallback = "")
                => System.Net.WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(s) ? fallback : s);
        }


        // --------------------------------------------------------------------
        // Internals
        // --------------------------------------------------------------------

        private static SqliteConnection OpenConnection()
        {
            try
            {
                var con = new SqliteConnection($"Data Source={GetDbPath()};Cache=Shared");
                con.Open();
                return con;
            }
            catch (SqliteException ex) { ReportError("OpenConnection", ex); throw; }
            catch (Exception ex) { ReportError("OpenConnection", ex); throw; }
        }

        private static string GetDbPath()
        {
            try
            {
                // PathManager is a separate class; Tubes CALLS it to resolve a writable folder.
                var pm = new PathManager();
                var folder = pm.GetWritableFolder("Databases");
                return Path.Combine(folder, "rssfeeds.aidb");
            }
            catch (Exception ex) { ReportError("GetDbPath", ex); throw; }
        }

        private static long GetRowCount(SqliteConnection con)
        {
            try
            {
                using var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Feeds;";
                var o = cmd.ExecuteScalar();
                return (o is long L) ? L : Convert.ToInt64(o);
            }
            catch (SqliteException ex) { ReportError("GetRowCount", ex); throw; }
            catch (Exception ex) { ReportError("GetRowCount", ex); throw; }
        }

        private static void SeedSample(SqliteConnection con, int groups, int itemsPerGroup)
        {
            try
            {
                using var tx = con.BeginTransaction();
                using var cmd = con.CreateCommand();
                cmd.Transaction = tx;

                cmd.CommandText = @"
                    INSERT INTO Feeds
                    (FeedId, category, subcategory, catsub, groupcategory, moviecategory, rank, title, author, link, linkType, linkValue, shortDescription, description, bodyLinks, image, publishedDate, duration, tags)
                    VALUES
                    ($id, $category, $subcategory, $catsub, $groupcategory, $moviecategory, $rank, $title, $author, $link, $linkType, $linkValue, $shortDescription, $description, $bodyLinks, $image, $publishedDate, $duration, $tags);";

                var nowIso = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                // Seed across a couple categories/subcategories for initial UX
                string[] categories = { "YouTube", "Podcast" };
                string[] subcats = { "Science", "Engineering", "News" };

                int idCounter = 0;
                for (int g = 1; g <= groups; g++)
                {
                    var groupName = $"Group {g:00}";
                    for (int i = 1; i <= itemsPerGroup; i++)
                    {
                        idCounter++;
                        var cat = categories[(idCounter) % categories.Length];
                        var sub = subcats[(idCounter) % subcats.Length];

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("$id", Guid.NewGuid().ToString().ToUpperInvariant());
                        cmd.Parameters.AddWithValue("$category", cat);
                        cmd.Parameters.AddWithValue("$subcategory", sub);
                        cmd.Parameters.AddWithValue("$catsub", $"{cat}/{sub}");
                        cmd.Parameters.AddWithValue("$groupcategory", groupName);
                        cmd.Parameters.AddWithValue("$moviecategory", "General");
                        cmd.Parameters.AddWithValue("$rank", i);
                        cmd.Parameters.AddWithValue("$title", $"{cat} {sub} - Item {g:00}-{i:000}");
                        cmd.Parameters.AddWithValue("$author", $"Author {g:00}");
                        cmd.Parameters.AddWithValue("$link", $"https://example.com/{g}/{i}");
                        cmd.Parameters.AddWithValue("$linkType", "url");
                        cmd.Parameters.AddWithValue("$linkValue", "");
                        cmd.Parameters.AddWithValue("$shortDescription", $"Short description for {g:00}-{i:000}");
                        cmd.Parameters.AddWithValue("$description", $"Long description for item {g:00}-{i:000}");
                        cmd.Parameters.AddWithValue("$bodyLinks", "");
                        cmd.Parameters.AddWithValue("$image", "");
                        cmd.Parameters.AddWithValue("$publishedDate", nowIso);
                        cmd.Parameters.AddWithValue("$duration", "");
                        cmd.Parameters.AddWithValue("$tags", "seed,sample");

                        cmd.ExecuteNonQuery();
                    }
                }

                tx.Commit();
            }
            catch (SqliteException ex) { ReportError("SeedSample", ex); throw; }
            catch (Exception ex) { ReportError("SeedSample", ex); throw; }
        }

        /// <summary>
        /// Import CSV rows when Feeds is empty. Supports files with or without a header row.
        /// Expects columns in this order when no header: 
        /// FeedId, category, subcategory, catsub, groupcategory, moviecategory, rank, title, author, link, linkType, linkValue, shortDescription, description, bodyLinks, image, publishedDate, duration, tags
        /// Any missing fields default to NULL/empty; any extra fields are ignored.
        /// </summary>
        private static void ImportCsvIfEmpty(SqliteConnection con, string csvPath)
        {
            try
            {
                if (!File.Exists(csvPath)) return;

                using var tx = con.BeginTransaction();
                using var cmd = con.CreateCommand();
                cmd.Transaction = tx;

                cmd.CommandText = @"
                    INSERT INTO Feeds
                    (FeedId, category, subcategory, catsub, groupcategory, moviecategory, rank, title, author, link, linkType, linkValue, shortDescription, description, bodyLinks, image, publishedDate, duration, tags)
                    VALUES
                    ($FeedId, $category, $subcategory, $catsub, $groupcategory, $moviecategory, $rank, $title, $author, $link, $linkType, $linkValue, $shortDescription, $description, $bodyLinks, $image, $publishedDate, $duration, $tags);";

                using var sr = OpenCsvStream(csvPath);
                var records = EnumerateCsvRecords(sr).ToList();

                // Detect header row (first logical record)
                List<string>? first = records.Count > 0 ? records[0] : null;
                if (first == null) { tx.Commit(); return; }

                // Treat as header if the first row contains several known column names (tab- or comma-delimited)
                var required = new HashSet<string>(new[]
                {
                    "FeedId","category","subcategory","catsub","groupcategory","moviecategory","rank","title",
                    "author","link","linkType","linkValue","shortDescription","description","bodyLinks","image",
                    "publishedDate","duration","tags"
                }, StringComparer.OrdinalIgnoreCase);

                int headerHits = first.Count(h => required.Contains((h ?? "").Trim()));
                bool hasHeader = headerHits >= 5;

                IEnumerable<List<string>> toImport = hasHeader ? records.Skip(1) : records;

                // Build header index map if header present
                if (hasHeader) _headerIndexCache = BuildHeaderIndex(first);

                foreach (var cells in toImport)
                {
                    // Map fields by header (if present) or by fixed position (0-based)
                    string GetPos(int index) => (index >= 0 && index < cells.Count) ? cells[index] : "";

                    string feedId = TrimOrEmpty(SelectByNameOrPos("FeedId", 0));
                    if (string.IsNullOrWhiteSpace(feedId)) feedId = Guid.NewGuid().ToString().ToUpperInvariant();

                    string category = SelectByNameOrPos("category", 1);
                    string subcategory = SelectByNameOrPos("subcategory", 2, allowNull: true);
                    string catsub = SelectByNameOrPos("catsub", 3, allowNull: true);
                    string groupcategory = SelectByNameOrPos("groupcategory", 4, allowNull: true);
                    string moviecategory = SelectByNameOrPos("moviecategory", 5);
                    if (string.IsNullOrWhiteSpace(moviecategory)) moviecategory = "General";

                    int rank = ParseInt(SelectByNameOrPos("rank", 6));

                    string title = SelectByNameOrPos("title", 7, allowNull: true);
                    string author = SelectByNameOrPos("author", 8, allowNull: true);
                    string link = SelectByNameOrPos("link", 9, allowNull: true);
                    string linkType = SelectByNameOrPos("linkType", 10, allowNull: true);
                    string linkValue = SelectByNameOrPos("linkValue", 11, allowNull: true);
                    string shortDescription = SelectByNameOrPos("shortDescription", 12, allowNull: true);
                    string description = SelectByNameOrPos("description", 13, allowNull: true);
                    string bodyLinks = SelectByNameOrPos("bodyLinks", 14, allowNull: true);
                    string image = SelectByNameOrPos("image", 15, allowNull: true);
                    string publishedDate = SelectByNameOrPos("publishedDate", 16, allowNull: true);
                    string duration = SelectByNameOrPos("duration", 17, allowNull: true);
                    string tags = SelectByNameOrPos("tags", 18, allowNull: true);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("$FeedId", feedId);
                    cmd.Parameters.AddWithValue("$category", category ?? "");
                    cmd.Parameters.AddWithValue("$subcategory", (object?)NullIfEmpty(subcategory) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$catsub", (object?)NullIfEmpty(catsub) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$groupcategory", (object?)NullIfEmpty(groupcategory) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$moviecategory", moviecategory ?? "General");
                    cmd.Parameters.AddWithValue("$rank", rank);
                    cmd.Parameters.AddWithValue("$title", (object?)NullIfEmpty(title) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$author", (object?)NullIfEmpty(author) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$link", (object?)NullIfEmpty(link) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$linkType", (object?)NullIfEmpty(linkType) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$linkValue", (object?)NullIfEmpty(linkValue) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$shortDescription", (object?)NullIfEmpty(shortDescription) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$description", (object?)NullIfEmpty(description) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$bodyLinks", (object?)NullIfEmpty(bodyLinks) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$image", (object?)NullIfEmpty(image) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$publishedDate", (object?)NullIfEmpty(publishedDate) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$duration", (object?)NullIfEmpty(duration) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("$tags", (object?)NullIfEmpty(tags) ?? DBNull.Value);

                    cmd.ExecuteNonQuery();

                    // Local mapping helper
                    string SelectByNameOrPos(string name, int pos, bool allowNull = false)
                    {
                        if (hasHeader && _headerIndexCache != null && _headerIndexCache.TryGetValue(name, out var idx))
                        {
                            var val = (idx >= 0 && idx < cells.Count) ? cells[idx] : "";
                            return allowNull ? NullIfEmpty(val)! : (val ?? "");
                        }
                        return allowNull ? NullIfEmpty(GetPos(pos))! : GetPos(pos);
                    }
                }

                tx.Commit();
            }
            catch (SqliteException ex) { ReportError("ImportCsvIfEmpty", ex); throw; }
            catch (Exception ex) { ReportError("ImportCsvIfEmpty", ex); throw; }
        }

        private static Dictionary<string, int> BuildHeaderIndex(List<string> header)
        {
            try
            {
                var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < header.Count; i++)
                {
                    var h = (header[i] ?? "").Trim();
                    if (!string.IsNullOrEmpty(h) && !dict.ContainsKey(h))
                        dict[h] = i;
                }
                return dict;
            }
            catch (Exception ex) { ReportError("BuildHeaderIndex", ex); throw; }
        }

        private static string TrimOrEmpty(string s) => (s ?? "").Trim();

        private static int ParseInt(string s)
        {
            try
            {
                if (int.TryParse((s ?? "").Trim(), out var v)) return v;
                return 0;
            }
            catch (Exception ex) { ReportError("ParseInt", ex); throw; }
        }

        private static string? NullIfEmpty(string? s)
            => string.IsNullOrWhiteSpace(s) || string.Equals(s?.Trim(), "NULL", StringComparison.OrdinalIgnoreCase) ? null : s;

        /// <summary>
        /// Open a StreamReader for CSV with tolerant UTF-8 decoding (no code-page files created).
        /// </summary>
        private static StreamReader OpenCsvStream(string path)
        {
            try
            {
                // UTF-8 with BOM detection and no throw on invalid bytes (they'll be replaced)
                return new StreamReader(path, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false), detectEncodingFromByteOrderMarks: true);
            }
            catch (Exception ex) { ReportError("OpenCsvStream", ex); throw; }
        }

        /// <summary>
        /// Robust CSV parser that supports quoted fields, embedded quotes, and embedded newlines.
        /// Also tolerates stray (unescaped) quotes inside fields by treating them as literal text.
        /// Returns all records as a materialized list to avoid 'yield' inside try/catch (CS1626).
        /// </summary>
        private static IEnumerable<List<string>> EnumerateCsvRecords(TextReader reader)
        {
            var results = new List<List<string>>();
            try
            {
                // Read entire content so we can auto-detect delimiter (comma vs tab)
                var content = reader.ReadToEnd();
                if (string.IsNullOrEmpty(content))
                    return results;

                int firstLineEnd = content.IndexOfAny(new[] { '\r', '\n' });
                var headerSlice = firstLineEnd >= 0 ? content.AsSpan(0, firstLineEnd) : content.AsSpan();
                int commaCount = 0, tabCount = 0;
                foreach (char ch in headerSlice)
                {
                    if (ch == ',') commaCount++;
                    else if (ch == '\t') tabCount++;
                }
                char delim = tabCount > commaCount ? '\t' : ',';

                var row = new List<string>();
                var sb = new StringBuilder();
                bool inQuotes = false;

                for (int idx = 0; idx < content.Length; idx++)
                {
                    char c = content[idx];

                    if (inQuotes)
                    {
                        if (c == '\"')
                        {
                            char next = idx + 1 < content.Length ? content[idx + 1] : '\0';

                            if (next == '\"')
                            {
                                // Escaped quote ("")
                                sb.Append('\"');
                                idx++; // consume the second quote
                            }
                            else if (next == delim || next == '\r' || next == '\n' || next == '\0')
                            {
                                // Proper closing quote (followed by delimiter or EOL)
                                inQuotes = false;
                            }
                            else
                            {
                                // Stray quote inside a quoted field -> treat as literal
                                sb.Append('\"');
                                // remain inQuotes
                            }
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                    else
                    {
                        if (c == '\"')
                        {
                            inQuotes = true;
                        }
                        else if (c == delim)
                        {
                            row.Add(sb.ToString());
                            sb.Clear();
                        }
                        else if (c == '\r')
                        {
                            // normalize CRLF to line break
                            char next = idx + 1 < content.Length ? content[idx + 1] : '\0';
                            if (next == '\n') idx++;
                            row.Add(sb.ToString());
                            sb.Clear();
                            results.Add(row);
                            row = new List<string>();
                        }
                        else if (c == '\n')
                        {
                            row.Add(sb.ToString());
                            sb.Clear();
                            results.Add(row);
                            row = new List<string>();
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                }

                // Flush last field/row
                row.Add(sb.ToString());
                // If the last row has any non-empty field, keep it
                bool any = false;
                for (int i = 0; i < row.Count; i++)
                {
                    if (!string.IsNullOrEmpty(row[i])) { any = true; break; }
                }
                if (any) results.Add(row);

                return results;
            }
            catch (Exception ex) { ReportError("EnumerateCsvRecords", ex); throw; }
        }

        public static void UpdateFeeds(ZFeedItem f, System.Windows.Forms.DataGridView dgv, Label labelx)
        {
            try
            {
                //EnsureDatabase();

                using var con = OpenConnection();
                using var cmd = con.CreateCommand();

                cmd.CommandText = @"
            INSERT INTO Feeds
            (FeedId, category, subcategory, catsub, groupcategory, moviecategory, rank, title, author, link, linkType, linkValue, shortDescription, description, bodyLinks, image, publishedDate, duration, tags)
            VALUES
            ($FeedId, $category, $subcategory, $catsub, $groupcategory, $moviecategory, $rank, $title, $author, $link, $linkType, $linkValue, $shortDescription, $description, $bodyLinks, $image, $publishedDate, $duration, $tags);";

                // Required / defaults
                var feedId = string.IsNullOrWhiteSpace(f.FeedId)
                    ? Guid.NewGuid().ToString().ToUpperInvariant()
                    : f.FeedId!.Trim();
                var category = f.Category ?? string.Empty;
                var movieCategory = string.IsNullOrWhiteSpace(f.MovieCategory) ? "General" : f.MovieCategory!;
                var rank = f.Rank;

                cmd.Parameters.AddWithValue("$FeedId", feedId);
                cmd.Parameters.AddWithValue("$category", category);
                cmd.Parameters.AddWithValue("$subcategory", (object?)NullIfEmpty(f.SubCategory) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$catsub", (object?)NullIfEmpty(f.CatSub) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$groupcategory", (object?)NullIfEmpty(f.GroupCategory) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$moviecategory", movieCategory);
                cmd.Parameters.AddWithValue("$rank", rank);
                cmd.Parameters.AddWithValue("$title", (object?)NullIfEmpty(f.Title) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$author", (object?)NullIfEmpty(f.Author) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$link", (object?)NullIfEmpty(f.Link) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$linkType", (object?)NullIfEmpty(f.LinkType) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$linkValue", (object?)NullIfEmpty(f.LinkValue) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$shortDescription", (object?)NullIfEmpty(f.ShortDescription) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$description", (object?)NullIfEmpty(f.Description) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$bodyLinks", (object?)NullIfEmpty(f.BodyLinks) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$image", (object?)NullIfEmpty(f.Image) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$publishedDate", (object?)NullIfEmpty(f.PublishedDate) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$duration", (object?)NullIfEmpty(f.Duration) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$tags", (object?)NullIfEmpty(f.Tags) ?? DBNull.Value);

                cmd.ExecuteNonQuery();
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex) { ReportError("UpdateFeeds/Insert", ex); throw; }
            catch (Exception ex) { ReportError("UpdateFeeds/Insert", ex); throw; }
        }

        /// <summary>
        /// Concatenate the first record with the rest without using 'yield' inside try/catch (fixes CS1626).
        /// </summary>
        private static IEnumerable<List<string>> Concat(List<string> first, IEnumerable<List<string>> rest)
        {
            try
            {
                var list = new List<List<string>> { first };
                foreach (var r in rest) list.Add(r);
                return list;
            }
            catch (Exception ex) { ReportError("Concat", ex); throw; }
        }

        private static void ReportError(string context, Exception ex)
        {
            try
            {
                // Do not create any files or folders. Only write to standard error.
                Console.Error.WriteLine($"[Tubes:{context}] {ex.GetType().Name}: {ex.Message}");
            }
            catch { /* Never throw from error reporter */ }
        }
    }

    // DTOs / Models -----------------------------------------------------------

    public sealed record FeedItem(
        string FeedId,
        string? Category,
        string? Subcategory,
        string? CatSub,
        string? GroupCategory,
        string? MovieCategory,
        int Rank,
        string? Title,
        string? Author,
        string? Link,
        string? LinkType,
        string? LinkValue,
        string? ShortDescription,
        string? Description,
        string? BodyLinks,
        string? Image,
        string? PublishedDate,
        string? Duration,
        string? Tags
    );

    public sealed record FeedUpsert(
        string FeedId,
        string Category,
        string MovieCategory,
        int Rank,
        string? Title = null,
        string? Author = null,
        string? Link = null,
        string? LinkType = null,
        string? LinkValue = null,
        string? ShortDescription = null,
        string? Description = null,
        string? BodyLinks = null,
        string? Image = null,
        string? PublishedDate = null,
        string? Duration = null,
        string? Tags = null,
        string? Subcategory = null,
        string? CatSub = null,
        string? GroupCategory = null
    );
}





//sb.Append($"""
//            <div class="item"
//                 data-feedid="{Safe(f.FeedId)}"
//                 data-category="{Safe(f.Category)}"
//                 data-subcategory="{Safe(f.Subcategory)}"
//                 data-catsub="{Safe(f.CatSub)}"
//                 data-groupcategory="{Safe(f.GroupCategory)}"
//                 data-moviecategory="{Safe(f.MovieCategory)}"
//                 data-rank="{f.Rank}"
//                 data-title="{Safe(f.Title)}"
//                 data-author="{Safe(f.Author)}"
//                 data-link="{Safe(f.Link)}"
//                 data-linktype="{Safe(f.LinkType)}"
//                 data-linkvalue="{Safe(f.LinkValue)}"
//                 data-shortdescription="{Safe(f.ShortDescription)}"
//                 data-description="{Safe(f.Description)}"
//                 data-bodylinks="{Safe(f.BodyLinks)}"
//                 data-image="{Safe(f.Image)}"
//                 data-publisheddate="{Safe(f.PublishedDateIso)}"
//                 data-duration="{Safe(f.Duration)}"
//                 data-tags="{Safe(f.Tags)}">

//              <div class="rank">{f.Rank}</div>

//              <!-- Square image on the left -->
//              <div class="item-image">
//                <img class="thumb" src="{Safe(f.Image)}" alt="{title}">
//              </div>

//              <!-- Content on the right -->
//              <div class="item-content">
//                <div class="title mb-1">
//                  <a href="{url}" target="_blank" rel="noopener">{title}</a>
//                </div>
//                <div class="meta">
//                  {author}{(string.IsNullOrWhiteSpace(author) ? "" : " • ")}{published}
//                </div>
//                <div class="meta" style="margin-top:6px;">{Safe(f.ShortDescription)}</div>

//                <!-- Every field returned below as key/value -->
//                <div class="kv">
//                  <div>FeedId</div><div>{Safe(f.FeedId)}</div>
//                  <div>category</div><div>{Safe(f.Category)}</div>
//                  <div>subcategory</div><div>{Safe(f.Subcategory)}</div>
//                  <div>catsub</div><div>{Safe(f.CatSub)}</div>
//                  <div>groupcategory</div><div>{Safe(f.GroupCategory)}</div>
//                  <div>moviecategory</div><div>{Safe(f.MovieCategory)}</div>
//                  <div>rank</div><div>{f.Rank}</div>
//                  <div>title</div><div>{Safe(f.Title)}</div>
//                  <div>author</div><div>{Safe(f.Author)}</div>
//                  <div>link</div><div><a href="{url}" target="_blank" rel="noopener">{url}</a></div>
//                  <div>linkType</div><div>{Safe(f.LinkType)}</div>
//                  <div>linkValue</div><div>{Safe(f.LinkValue)}</div>
//                  <div>shortDescription</div><div>{Safe(f.ShortDescription)}</div>
//                  <div>description</div><div>{Safe(f.Description)}</div>
//                  <div>bodyLinks</div><div>{Safe(f.BodyLinks)}</div>
//                  <div>image</div><div>{Safe(f.Image)}</div>
//                  <div>publishedDate</div><div>{Safe(f.PublishedDateIso)}</div>
//                  <div>duration</div><div>{Safe(f.Duration)}</div>
//                  <div>tags</div><div>{Safe(f.Tags)}</div>
//                </div>
//              </div>
//            </div>
//            """);


/// <summary>
/// Generates a simple, clean HTML page listing the feeds.
/// </summary>
//public static string BuildFeedsHtml(string category, string? subcategory, IEnumerable<FeedItem> feeds)
//{
//    var sb = new StringBuilder();
//    sb.Append("""
//    <!doctype html>
//    <html lang="en">
//    <head>
//    <meta charset="utf-8">
//    <title>Feeds</title>
//    <meta name="viewport" content="width=device-width, initial-scale=1">
//    <style>
//      :root{
//        --bg:#0f1115; --fg:#e6e6e6; --muted:#a8b0c0; --accent:#4da3ff; --card:#171a21; --chip:#232838;
//      }
//      html,body{margin:0;padding:0;background:var(--bg);color:var(--fg);font:14px/1.5 -apple-system, Segoe UI, Roboto, Helvetica, Arial, sans-serif;}
//      header{position:sticky;top:0;background:linear-gradient(180deg,var(--bg),rgba(15,17,21,.9));backdrop-filter:saturate(140%) blur(6px);padding:12px 16px;border-bottom:1px solid #1f2330;z-index:2}
//      h1{font-size:16px;margin:0}
//      .muted{color:var(--muted)}
//      .list{padding:10px 8px 32px;max-width:1100px;margin:0 auto;}
//      .item{display:flex;gap:12px;align-items:flex-start;padding:10px 12px;margin:8px 0;border-radius:10px;background:var(--card);border:1px solid #1e2230}
//      .rank{min-width:48px;text-align:center;background:var(--chip);border-radius:8px;padding:6px 8px;color:var(--muted)}
//      .title{font-weight:600;margin:0 0 4px 0}
//      .meta{font-size:12px;color:var(--muted)}
//      a{color:var(--accent);text-decoration:none}
//      a:hover{text-decoration:underline}
//      .kv{display:grid;grid-template-columns:120px 1fr;gap:4px 10px;font-size:12px;margin-top:6px;color:var(--muted)}
//      .kv div:nth-child(odd){opacity:.8}
//      .thumb{width:80px;height:80px;object-fit:cover;border-radius:6px;background:#0b0d12}
//    </style>
//    </head>
//    <body>
//    """);

//    var subtitle = string.IsNullOrWhiteSpace(subcategory)
//        ? $"{category}"
//        : $"{category} • {subcategory}";

//    sb.Append($"""
//    <header>
//      <h1>Feeds <span class="muted">({System.Net.WebUtility.HtmlEncode(subtitle)})</span></h1>
//    </header>
//    <a href="EVENT:action:value" 
//       style="display:inline-block;
//              padding:10px 20px;
//              background-color:#007bff;
//              color:#ffffff;
//              text-decoration:none;
//              font-family:sans-serif;
//              border-radius:6px;
//              font-weight:bold;">
//        Test Event Button
//    </a>

