// DeviceHubControl.Logic.cs
using AiNetStudio.WinGui.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace AiNetStudio.WinGui.ControlScreens
{
    public partial class DeviceHubControl : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WinGUIMain? MainFormReference { get; set; }

        // --- Mode handling ---
        private enum AppMode { Faux, Active }
        private AppMode _mode = AppMode.Faux; // default

        // ---- logging plumbing ----
        private bool _logging;
        private readonly ConcurrentQueue<SensorReading> _logQ = new();
        private CancellationTokenSource? _logCts;
        private Task? _loggerTask;
        private readonly string _dbPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AiNetStudio", "sensors.db");

        // ===== Live data binding for the grid =====
        private readonly BindingList<SensorReading> _live = new();

        // ===== Simple flags and timers per device =====
        private bool _scaleRunning, _thRunning, _tachRunning;
        // use fully qualified type name
        private readonly System.Windows.Forms.Timer _scaleTimer = new System.Windows.Forms.Timer { Interval = 200 }; // ~5 Hz
        private readonly System.Windows.Forms.Timer _thTimer = new System.Windows.Forms.Timer { Interval = 500 }; // ~2 Hz
        private readonly System.Windows.Forms.Timer _tachTimer = new System.Windows.Forms.Timer { Interval = 200 }; // ~5 Hz

        private readonly Random _rnd = new();

        public DeviceHubControl(WinGUIMain mainForm)
        {
            MainFormReference = mainForm;

            InitializeComponent();

            // Bind grid to our binding list
            gridReadings.AutoGenerateColumns = false;
            gridReadings.DataSource = _live;

            // Ensure DataPropertyName matches our model
            TimestampUtc.DataPropertyName = nameof(SensorReading.TimestampUtc);
            DeviceId.DataPropertyName = nameof(SensorReading.DeviceId);
            Metric.DataPropertyName = nameof(SensorReading.Metric);
            Value.DataPropertyName = nameof(SensorReading.Value);
            Unit.DataPropertyName = nameof(SensorReading.Unit);
            ExtraJson.DataPropertyName = nameof(SensorReading.ExtraJson);

            // Button events
            btnScale.Click += (_, __) => ToggleScale();
            btnTH.Click += (_, __) => ToggleTH();
            btnTach.Click += (_, __) => ToggleTach();

            // (We’ll wire Start/Stop Logging later when we add SQLite)
            btnStartLogging.Click += (_, __) => MessageBox.Show("Logging not implemented yet. We’ll add SQLite next.");
            btnStopLogging.Click += (_, __) => MessageBox.Show("Logging not implemented yet. We’ll add SQLite next.");
            //btnStartLogging.Click += async (_, __) => await StartLoggingAsync();
            //btnStopLogging.Click += (_, __) => StopLogging();

            // Timers → generate fake readings and update UI/grid
            _scaleTimer.Tick += (_, __) =>
            {
                // ~5 lb with tiny jitter
                double val = 5.000 + (_rnd.NextDouble() - 0.5) * 0.020;
                lblScaleValue.Text = $"{val:F3} lb";
                AddReading(new SensorReading(DateTime.UtcNow, "scale-01", "weight-lb", val, "lb", ""));
            };

            _thTimer.Tick += (_, __) =>
            {
                double t = 23.5 + (_rnd.NextDouble() - 0.5) * 0.4;
                double h = 45.0 + (_rnd.NextDouble() - 0.5) * 2.0;
                lblTHValue.Text = $"{t:F2} °C, {h:F1} %";
                AddReading(new SensorReading(DateTime.UtcNow, "th-01", "temp-C", t, "°C", ""));
                AddReading(new SensorReading(DateTime.UtcNow, "th-01", "humidity", h, "%", ""));
            };

            _tachTimer.Tick += (_, __) =>
            {
                double rpm = 1800 + (_rnd.NextDouble() - 0.5) * 30; // ±15 rpm
                lblTachValue.Text = $"{rpm:F0} rpm";
                AddReading(new SensorReading(DateTime.UtcNow, "tach-01", "rpm", rpm, "rpm", ""));
            };
        }

        // Insert newest at top, trim to 500 rows to keep UI snappy
        private void AddReading(SensorReading r)
        {
            _live.Insert(0, r);
            if (_live.Count > 500) _live.RemoveAt(_live.Count - 1);
        }

        private void ToggleScale()
        {
            if (_scaleRunning)
            {
                _scaleTimer.Stop();
                _scaleRunning = false;
                btnScale.Text = "Start Scale";
                lblScaleValue.Text = "--";
            }
            else
            {
                _scaleTimer.Start();
                _scaleRunning = true;
                btnScale.Text = "Stop Scale";
            }
        }

        private void ToggleTH()
        {
            if (_thRunning)
            {
                _thTimer.Stop();
                _thRunning = false;
                btnTH.Text = "Start Temp/Humidity";
                lblTHValue.Text = "--";
            }
            else
            {
                _thTimer.Start();
                _thRunning = true;
                btnTH.Text = "Stop Temp/Humidity";
            }
        }

        private void ToggleTach()
        {
            if (_tachRunning)
            {
                _tachTimer.Stop();
                _tachRunning = false;
                btnTach.Text = "Start Tach";
                lblTachValue.Text = "--";
            }
            else
            {
                _tachTimer.Start();
                _tachRunning = true;
                btnTach.Text = "Stop Tach";
            }
        }
    }

    // ===== Minimal data model to match your grid columns =====
    public class SensorReading
    {
        public DateTime TimestampUtc { get; set; }
        public string DeviceId { get; set; }
        public string Metric { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public string ExtraJson { get; set; }

        public SensorReading(DateTime ts, string deviceId, string metric, double value, string unit, string extra)
        {
            TimestampUtc = ts;
            DeviceId = deviceId;
            Metric = metric;
            Value = value;
            Unit = unit;
            ExtraJson = extra;
        }


        // /////////////////////////////////////////////////////////
        // /////////////////////////////////////////////////////////
        // /////////////////////////////////////////////////////////

        //private async Task StartLoggingAsync()
        //{
        //    //if (_logging) return;
        //    //await EnsureDatabaseAsync();

        //    //_logCts = new CancellationTokenSource();
        //    //_loggerTask = Task.Run(() => LoggerLoopAsync(_logCts.Token));
        //    //_logging = true;

        //    //btnStartLogging.Enabled = false;
        //    //btnStopLogging.Enabled = true;
        //}

        //private void StopLogging()
        //{
        //    //if (!_logging) return;
        //    //_logging = false;

        //    //try { _logCts?.Cancel(); } catch { /* ignore */ }
        //    //try { _loggerTask?.Wait(2000); } catch { /* ignore */ }

        //    //btnStartLogging.Enabled = true;
        //    //btnStopLogging.Enabled = false;
        //}

        //private async Task EnsureDatabaseAsync()
        //{
        //    var dir = System.IO.Path.GetDirectoryName(_dbPath)!;
        //    if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);

        //    await using var conn = new SqliteConnection($"Data Source={_dbPath}");
        //    await conn.OpenAsync();
        //    await using var cmd = conn.CreateCommand();
        //    cmd.CommandText = @"
        //        CREATE TABLE IF NOT EXISTS SensorReadings(
        //            Id INTEGER PRIMARY KEY AUTOINCREMENT,
        //            TimestampUtc TEXT NOT NULL,
        //            DeviceId     TEXT NOT NULL,
        //            Metric       TEXT NOT NULL,
        //            Value        REAL NOT NULL,
        //            Unit         TEXT NULL,
        //            ExtraJson    TEXT NULL
        //        );
        //        CREATE INDEX IF NOT EXISTS IX_Sensor_Time   ON SensorReadings(TimestampUtc);
        //        CREATE INDEX IF NOT EXISTS IX_Sensor_Device ON SensorReadings(DeviceId);";
        //    await cmd.ExecuteNonQueryAsync();
        //}

        //private async Task LoggerLoopAsync(CancellationToken ct)
        //{
        //    await using var conn = new SqliteConnection($"Data Source={_dbPath}");
        //    await conn.OpenAsync(ct);

        //    // Prepare insert
        //    await using var cmd = conn.CreateCommand();
        //    cmd.CommandText = @"INSERT INTO SensorReadings
        //    (TimestampUtc, DeviceId, Metric, Value, Unit, ExtraJson)
        //    VALUES ($t,$d,$m,$v,$u,$x)";
        //        var pT = cmd.CreateParameter(); pT.ParameterName = "$t"; cmd.Parameters.Add(pT);
        //        var pD = cmd.CreateParameter(); pD.ParameterName = "$d"; cmd.Parameters.Add(pD);
        //        var pM = cmd.CreateParameter(); pM.ParameterName = "$m"; cmd.Parameters.Add(pM);
        //        var pV = cmd.CreateParameter(); pV.ParameterName = "$v"; cmd.Parameters.Add(pV);
        //        var pU = cmd.CreateParameter(); pU.ParameterName = "$u"; cmd.Parameters.Add(pU);
        //        var pX = cmd.CreateParameter(); pX.ParameterName = "$x"; cmd.Parameters.Add(pX);

        //    // Simple loop: batch small groups to reduce disk I/O
        //    var buffer = new System.Collections.Generic.List<SensorReading>(64);

        //    while (!ct.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            // drain queue (up to 64 per batch)
        //            buffer.Clear();
        //            while (buffer.Count < 64 && _logQ.TryDequeue(out var r))
        //                buffer.Add(r);

        //            if (buffer.Count == 0)
        //            {
        //                await Task.Delay(100, ct);
        //                continue;
        //            }

        //            // write batch
        //            await using var tx = await conn.BeginTransactionAsync(ct);
        //            cmd.Transaction = tx as SqliteTransaction;

        //            foreach (var r in buffer)
        //            {
        //                pT.Value = r.TimestampUtc.ToString("o"); // ISO 8601
        //                pD.Value = r.DeviceId;
        //                pM.Value = r.Metric;
        //                pV.Value = r.Value;
        //                pU.Value = r.Unit ?? "";
        //                pX.Value = r.ExtraJson ?? "";
        //                await cmd.ExecuteNonQueryAsync(ct);
        //            }

        //            await tx.CommitAsync(ct);
        //        }
        //        catch (OperationCanceledException)
        //        {
        //            break;
        //        }
        //        catch
        //        {
        //            // swallow/log if you like
        //            await Task.Delay(200, ct);
        //        }
        //    }

        //    // Final drain on cancel
        //    while (_logQ.TryDequeue(out var r))
        //    {
        //        try
        //        {
        //            pT.Value = r.TimestampUtc.ToString("o");
        //            pD.Value = r.DeviceId;
        //            pM.Value = r.Metric;
        //            pV.Value = r.Value;
        //            pU.Value = r.Unit ?? "";
        //            pX.Value = r.ExtraJson ?? "";
        //            await cmd.ExecuteNonQueryAsync(ct);
        //        }
        //        catch { break; }
        //    }
        //}





    }
}
