//
////////////////////////////////////////////////////////////////
//
/**************************************************************
**        __                                          __
**     __/_/__________________________________________\_\__
**  __|_                                                  |__
** (___O)     Ouslan, Inc.                              (O___)
**(_____O)	  ainetstudio.com              			   (O_____)
**(_____O)	  Author: Bill SerGio, Infomercial King™   (O_____)
** (__O)                                                (O__)
**    |___________________________________________________|
**
****************************************************************/
/*
 * (C) Copyright 2024-2025 Ouslan,Inc, All Rights Reserved Worldwide.
 * software-rus.com   
 * tvmogul1@yahoo.com  
 *
 */

using AiNetStudio.DataAccess;
using AiNetStudio.WinGui.Dialogs;
using AiNetStudio.WinGui.Forms;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AiNetStudio
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            // ---- Serilog bootstrap ----
            var exeDir = Path.GetDirectoryName(Application.ExecutablePath) ?? AppContext.BaseDirectory;
            var logDir = Path.Combine(exeDir, "AAALogs");
            Directory.CreateDirectory(logDir);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.Debug() // VS Output window
                .WriteTo.File(
                    Path.Combine(logDir, "app-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    shared: true)
                .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception");

            Application.ThreadException += (s, e) =>
                Log.Error(e.Exception, "WinForms thread exception");

            // Single-instance mutex to prevent multiple app instances
            bool createdNew = false;
            using var _singleInstanceMutex = new Mutex(initiallyOwned: true, name: @"Global\AiNetStudio_SingleInstance", out createdNew);
            if (!createdNew)
            {
                Log.Information("Another instance is already running. Exiting.");
                Log.CloseAndFlush();
                return;
            }

            try
            {
                Log.Information("AiNetStudio starting…");

                // ---- Copy required files if they don't already exist ----
                // First-run seeding: only perform copy if the app data root does NOT exist yet
                var localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var appRoot = Path.Combine(localApp, "AiNetStudio");
                var isFirstRun = !Directory.Exists(appRoot);

                if (isFirstRun)
                {
                    Directory.CreateDirectory(appRoot); // ensure root exists for writable folders

                    try
                    {
                        var pm = new PathManager();
                        var dbPath = Path.Combine(pm.GetWritableFolder("Databases"), "rssfeeds.aidb");
                        var dir = Path.GetDirectoryName(dbPath)!;
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        // UPDATED: Resolve Libs from multiple possible locations (bin output, project root, parent folders)
                        var srcLibs = ResolveLibsFolder(exeDir);
                        if (srcLibs == null)
                        {
                            var msgNoLibs = $"Unable to locate 'Libs' folder.\nSearched around:\n{exeDir}\n\nEnsure a 'Libs' folder exists with required files (techarchive.aidb, rssfeeds.aidb).";
                            Log.Error(msgNoLibs);
                            ErrorDialog.Show("Startup Copy Error", msgNoLibs);
                            return;
                        }
                        Log.Information("Using Libs source at: {SrcLibs}", srcLibs);

                        var dstLibs = pm.GetWritableFolder("Libs");

                        var dstDatabases = pm.GetWritableFolder("Databases");

                        if (!Directory.Exists(dstLibs)) Directory.CreateDirectory(dstLibs);
                        if (!Directory.Exists(dstDatabases)) Directory.CreateDirectory(dstDatabases);

                        var srcExport = Path.Combine(srcLibs, "feeds_export.sql");
                        var dstExport = Path.Combine(dstLibs, "feeds_export.sql");
                        if (File.Exists(srcExport) && !File.Exists(dstExport)) File.Copy(srcExport, dstExport, overwrite: false);

                        var srcSubset = Path.Combine(srcLibs, "feeds_subset.csv");
                        var dstSubset = Path.Combine(dstLibs, "feeds_subset.csv");
                        if (File.Exists(srcSubset) && !File.Exists(dstSubset)) File.Copy(srcSubset, dstSubset, overwrite: false);

                        var srcAidb = Path.Combine(srcLibs, "rssfeeds.aidb");
                        var dstAidb = Path.Combine(dstDatabases, "rssfeeds.aidb");
                        if (File.Exists(srcAidb) && !File.Exists(dstAidb)) File.Copy(srcAidb, dstAidb, overwrite: false);

                        var srcAidb2 = Path.Combine(srcLibs, "techarchive.aidb");
                        var dstAidb2 = Path.Combine(dstDatabases, "techarchive.aidb");
                        if (File.Exists(srcAidb2) && !File.Exists(dstAidb2)) File.Copy(srcAidb2, dstAidb2, overwrite: false);

                        // ////////////////////////////////////////////////////////////
                        var srcPdfs = Path.Combine(srcLibs, "Library");
                        var dstPdfs = pm.GetWritableFolder("Library");

                        Directory.CreateDirectory(dstPdfs);
                        // Copy any missing PDFs (idempotent)
                        if (Directory.Exists(srcPdfs))
                        {
                            foreach (var srcFile in Directory.EnumerateFiles(srcPdfs, "*.pdf", SearchOption.TopDirectoryOnly))
                            {
                                var fileName = Path.GetFileName(srcFile);
                                var dstFile = Path.Combine(dstPdfs, fileName);
                                if (!File.Exists(dstFile))
                                {
                                    File.Copy(srcFile, dstFile, overwrite: false);
                                }
                            }
                        }
                        else
                        {
                            Log.Error("Source PDF library folder not found at: {SrcPdfs}", srcPdfs);
                        }
                        ////////////////////////////////////////////////////////////

                        // Final verification: ensure both DBs exist after copy attempt
                        var missing = new StringBuilder();
                        if (!File.Exists(Path.Combine(dstDatabases, "rssfeeds.aidb"))) missing.AppendLine("rssfeeds.aidb");
                        if (!File.Exists(Path.Combine(dstDatabases, "techarchive.aidb"))) missing.AppendLine("techarchive.aidb");

                        if (missing.Length > 0)
                        {
                            var msg = $"Database file(s) missing after copy attempt in:\n{dstDatabases}\n\nMissing:\n{missing}\n" +
                                      $"Source Libs: {srcLibs}";
                            Log.Error(msg);
                            ErrorDialog.Show("Startup Copy Error", msg);
                            return;
                        }

                        Log.Information("First-run seeding complete.");
                    }
                    catch (Exception copyEx)
                    {
                        Log.Error(copyEx, "Error copying seed files.");
                        ErrorDialog.Show("Startup Copy Error", copyEx.ToString());
                        return;
                    }
                }
                else
                {
                    Log.Information("App data folder already exists; skipping first-run seeding.");
                }
                // ---------------------------------------------------------

                // COPY-ONLY MODE: PDFs are handled above; importer disabled.
                // (Removed asynchronous importer block intentionally.)

                ApplicationConfiguration.Initialize();
                //Application.Run(new WinGUIMain());
                using (SkinDlg splash = new SkinDlg())
                {
                    if (splash.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new WinGUIMain());
                    }
                    else
                    {
                        //return;
                        Application.Run(new WinGUIMain());
                    }
                }

                Log.Information("AiNetStudio exited normally.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal app crash");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Locate the Libs folder by checking:
        /// 1) PathManager installation folder
        /// 2) exeDir\Libs
        /// 3) Walking up parent directories for a 'Libs' that contains techarchive.aidb
        /// Returns null if not found.
        /// </summary>
        private static string? ResolveLibsFolder(string exeDir)
        {
            try
            {
                // 1) PathManager-provided installation folder
                try
                {
                    var pm = new PathManager();
                    var pmLibs = pm.GetInstallationFolder("Libs");
                    if (Directory.Exists(pmLibs) &&
                        File.Exists(Path.Combine(pmLibs, "techarchive.aidb")))
                        return pmLibs;
                }
                catch { /* ignore if PathManager not ready */ }

                // 2) exeDir\Libs
                var directLibs = Path.Combine(exeDir, "Libs");
                if (Directory.Exists(directLibs) &&
                    File.Exists(Path.Combine(directLibs, "techarchive.aidb")))
                    return directLibs;

                // 3) Walk up parent directories to find a Libs containing the DB
                var cursor = new DirectoryInfo(exeDir);
                int hops = 0;
                while (cursor != null && hops < 6) // up to 6 levels up should cover common dev layouts
                {
                    var probe = Path.Combine(cursor.FullName, "Libs");
                    if (Directory.Exists(probe) &&
                        File.Exists(Path.Combine(probe, "techarchive.aidb")))
                        return probe;

                    cursor = cursor.Parent;
                    hops++;
                }
            }
            catch
            {
                // swallow and let caller handle null
            }
            return null;
        }
    }
}









//using AiNetStudio.DataAccess;
//using AiNetStudio.WinGui.Dialogs;
//using AiNetStudio.WinGui.Forms;
//using Serilog;
//using Serilog.Events;
//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Text;
//using System.Threading;
//using System.Windows.Forms;

//namespace AiNetStudio
//{
//    internal static class Program
//    {
//        /// <summary>
//        ///  The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            // To customize application configuration such as set high DPI settings or default font,
//            // see https://aka.ms/applicationconfiguration.

//            // ---- Serilog bootstrap ----
//            var exeDir = Path.GetDirectoryName(Application.ExecutablePath) ?? AppContext.BaseDirectory;
//            var logDir = Path.Combine(exeDir, "AAALogs");
//            Directory.CreateDirectory(logDir);

//            Log.Logger = new LoggerConfiguration()
//                .MinimumLevel.Debug()
//                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//                .Enrich.WithProcessId()
//                .Enrich.WithThreadId()
//                .Enrich.FromLogContext()
//                .WriteTo.Debug() // VS Output window
//                .WriteTo.File(
//                    Path.Combine(logDir, "app-.log"),
//                    rollingInterval: RollingInterval.Day,
//                    retainedFileCountLimit: 7,
//                    restrictedToMinimumLevel: LogEventLevel.Information,
//                    shared: true)
//                .CreateLogger();

//            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
//                Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception");

//            Application.ThreadException += (s, e) =>
//                Log.Error(e.Exception, "WinForms thread exception");

//            // Single-instance mutex to prevent multiple app instances
//            bool createdNew = false;
//            using var _singleInstanceMutex = new Mutex(initiallyOwned: true, name: @"Global\AiNetStudio_SingleInstance", out createdNew);
//            if (!createdNew)
//            {
//                Log.Information("Another instance is already running. Exiting.");
//                Log.CloseAndFlush();
//                return;
//            }

//            try
//            {
//                Log.Information("AiNetStudio starting…");

//                // ---- Copy required files if they don't already exist ----
//                try
//                {
//                    var pm = new PathManager();
//                    var dbPath = Path.Combine(pm.GetWritableFolder("Databases"), "rssfeeds.aidb");
//                    var dir = Path.GetDirectoryName(dbPath)!;
//                    if (!Directory.Exists(dir))
//                        Directory.CreateDirectory(dir);

//                    var srcLibs = Path.Combine(exeDir, "Libs");
//                    var dstLibs = pm.GetWritableFolder("Libs");

//                    var dstDatabases = pm.GetWritableFolder("Databases");

//                    if (!Directory.Exists(dstLibs)) Directory.CreateDirectory(dstLibs);
//                    if (!Directory.Exists(dstDatabases)) Directory.CreateDirectory(dstDatabases);

//                    var srcExport = Path.Combine(srcLibs, "feeds_export.sql");
//                    var dstExport = Path.Combine(dstLibs, "feeds_export.sql");
//                    if (File.Exists(srcExport) && !File.Exists(dstExport)) File.Copy(srcExport, dstExport, overwrite: false);

//                    var srcSubset = Path.Combine(srcLibs, "feeds_subset.csv");
//                    var dstSubset = Path.Combine(dstLibs, "feeds_subset.csv");
//                    if (File.Exists(srcSubset) && !File.Exists(dstSubset)) File.Copy(srcSubset, dstSubset, overwrite: false);

//                    var srcAidb = Path.Combine(srcLibs, "rssfeeds.aidb");
//                    var dstAidb = Path.Combine(dstDatabases, "rssfeeds.aidb");
//                    if (File.Exists(srcAidb) && !File.Exists(dstAidb)) File.Copy(srcAidb, dstAidb, overwrite: false);

//                    var srcAidb2 = Path.Combine(srcLibs, "techarchive.aidb");
//                    var dstAidb2 = Path.Combine(dstDatabases, "techarchive.aidb");
//                    if (File.Exists(srcAidb2) && !File.Exists(dstAidb2)) File.Copy(srcAidb2, dstAidb2, overwrite: false);

//                    // ////////////////////////////////////////////////////////////
//                    var srcPdfs = Path.Combine(srcLibs, "Library");
//                    var dstPdfs = pm.GetWritableFolder("Library");

//                    Directory.CreateDirectory(dstPdfs);
//                    // Copy any missing PDFs (idempotent)
//                    foreach (var srcFile in Directory.EnumerateFiles(srcPdfs, "*.pdf", SearchOption.TopDirectoryOnly))
//                    {
//                        var fileName = Path.GetFileName(srcFile);
//                        var dstFile = Path.Combine(dstPdfs, fileName);
//                        if (!File.Exists(dstFile))
//                        {
//                            File.Copy(srcFile, dstFile, overwrite: false);
//                        }
//                    }
//                    ////////////////////////////////////////////////////////////

//                    //var q = pm.GetWritableFolder("Databases");
//                    //var dbPDFs = Path.Combine(q, "techarchive.aidb");
//                    //DbInitializer.EnsurePatentsSchema(dbPDFs);
//                }
//                catch { /* ignore copy errors */ }
//                // ---------------------------------------------------------

//                // Run import on startup (wrap in Task.
//                // Run so we can call async code)
//                Task.Run(async () =>
//                {
//                    var pm2 = new PathManager();
//                    var dbDir = pm2.GetWritableFolder("Databases");
//                    Directory.CreateDirectory(dbDir);
//                    var dbPDFs = Path.Combine(dbDir, "techarchive.aidb");

//                    string sourceFolder = Path.Combine(pm2.GetInstallationFolder("Libs"), "Library");
//                    string appDataRoot = pm2.GetWritableFolder("");

//                    MessageBox.Show($"Import from:\n{sourceFolder}\n\nDB:\n{dbPDFs}\n\nLib root:\n{appDataRoot}", "Importer Paths");

//                    try
//                    {
//                        int imported = await PdfBulkImporter.ImportFolderAsync(
//                            sourceFolder,
//                            appDataRoot,
//                            dbPDFs,
//                            new Progress<(int done, int total, string file)>(p =>
//                            {
//                                Debug.WriteLine($"[{p.done}/{p.total}] {p.file}");
//                            })
//                        );

//                        MessageBox.Show($"Imported {imported} PDFs.", "Importer");
//                    }
//                    catch (Exception ex)
//                    {
//                        var msg = $"IMPORT FAILED\n\nMessage:\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}";
//                        ErrorDialog.Show("Importer Error", msg);
//                    }
//                }).Wait();

//                ApplicationConfiguration.Initialize();
//                //Application.Run(new WinGUIMain());
//                using (SkinDlg splash = new SkinDlg())
//                {
//                    if (splash.ShowDialog() == DialogResult.OK)
//                    {
//                        Application.Run(new WinGUIMain());
//                    }
//                    else
//                    {
//                        //return;
//                        Application.Run(new WinGUIMain());
//                    }
//                }

//                Log.Information("AiNetStudio exited normally.");
//            }
//            catch (Exception ex)
//            {
//                Log.Fatal(ex, "Fatal app crash");
//                throw;
//            }
//            finally
//            {
//                Log.CloseAndFlush();
//            }
//        }
//    }
//}




//public static class ErrorDialog
//{
//    public static void Show(string title, string message)
//    {
//        Form f = new Form
//        {
//            Text = title,
//            Width = 700,
//            Height = 500,
//            StartPosition = FormStartPosition.CenterScreen
//        };

//        var tb = new TextBox
//        {
//            Multiline = true,
//            ReadOnly = true,
//            ScrollBars = ScrollBars.Both,
//            Dock = DockStyle.Fill,
//            Font = new Font("Consolas", 10),
//            Text = message,
//            WordWrap = false
//        };

//        f.Controls.Add(tb);

//        var ok = new Button
//        {
//            Text = "OK",
//            Dock = DockStyle.Bottom,
//            DialogResult = DialogResult.OK
//        };
//        f.Controls.Add(ok);

//        f.AcceptButton = ok;
//        f.ShowDialog();
//    }
//}
