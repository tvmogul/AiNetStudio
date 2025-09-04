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

using AiNetStudio.WinGui.Dialogs;
using AiNetStudio.WinGui.Forms;
using Serilog;
using Serilog.Events;
using System;
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
                try
                {
                    var pm = new PathManager();
                    var dbPath = Path.Combine(pm.GetWritableFolder("Databases"), "rssfeeds.aidb");
                    var dir = Path.GetDirectoryName(dbPath)!;
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var srcLibs = Path.Combine(exeDir, "Libs");
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
                }
                catch { /* ignore copy errors */ }
                // ---------------------------------------------------------

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
    }
}
