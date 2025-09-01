/* feeds_export.sql
   Writes a real CSV file to disk using SQLCMD mode (:OUT).
   RUN THIS WITH SQLCMD (recommended) OR SSMS with "SQLCMD Mode" ON.

   Filters ONLY these categories:
     - Aliens UFOs
     - Artificial Intelligence
     - Drones
     - Antigravity
     - All Watch
     - All Movies
*/

/* ======= CONFIG: change output path if needed ======= */
:setvar OutputPath "C:\Temp\feeds_subset.csv"

/* ======= START WRITING TO FILE ======= */
:OUT $(OutputPath)

SET NOCOUNT ON;

/* CSV header: 19 columns, exact order expected by your SQLite table */
PRINT '"FeedId","category","subcategory","catsub","groupcategory","moviecategory","rank","title","author","link","linkType","linkValue","shortDescription","description","bodyLinks","image","publishedDate","duration","tags"';

/* Data rows, safely quoted and sanitized for CSV */
WITH FeedsFiltered AS (
    SELECT *
    FROM dbo.Feeds WITH (NOLOCK)
    WHERE category IN (
        N'Aliens UFOs',
        N'Artificial Intelligence',
        N'Drones',
        N'Antigravity',
        N'All Watch',
        N'All Movies'
    )
)
SELECT
  '"' + REPLACE(CONVERT(varchar(36), FeedId), '"', '""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(category,''),         CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(subcategory,''),      CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(catsub,''),           CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(groupcategory,''),    CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(moviecategory,''),    CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(ISNULL(CONVERT(varchar(20), [rank]), ''), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(title,''),            CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(author,''),           CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL([link],''),           CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(linkType,''),         CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(linkValue,''),        CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(shortDescription,''), CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL([description],''),    CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(bodyLinks,''),        CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL([image],''),          CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(ISNULL(CONVERT(varchar(23), publishedDate, 121), ''), '"','""') + '",' +  -- yyyy-mm-dd hh:mi:ss.mmm
  '"' + REPLACE(REPLACE(REPLACE(ISNULL([duration],''),       CHAR(13),' '), CHAR(10),' '), '"','""') + '",' +
  '"' + REPLACE(REPLACE(REPLACE(ISNULL(tags,''),             CHAR(13),' '), CHAR(10),' '), '"','""') + '"'
FROM FeedsFiltered
ORDER BY FeedId;

/* NOTE: DO NOT switch :OUT back to stdout here — leaving it out keeps the output in the CSV file. */
