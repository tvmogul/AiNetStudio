using AiNetStudio.WinGui.Forms;
using System.Threading;
using System;
using System.IO;
using System.Windows.Forms;
using Serilog;
using Serilog.Events;

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
            var logDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "AiNetStudio", "AAALogs");
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

                ApplicationConfiguration.Initialize();
                Application.Run(new WinGUIMain());

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
