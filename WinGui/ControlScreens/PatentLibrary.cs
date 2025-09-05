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

using AiNetStudio.WinGui.Forms;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AiNetStudio.WinGui.ControlScreens
{
    public partial class PatentLibrary : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WinGUIMain? MainFormReference { get; set; }

        private System.Windows.Forms.Button btnToggle;
        private bool _toggleLeft = false;

        // Keep a small visible strip of Panel1/Panel2 
        // so the splitter (and button) are still visible.
        private const int SplitterReveal = 24;

        // Hard cap so toggle never exceeds splitter height requested (e.g., 20px)
        private const int MaxToggleWidth = 20;

        // ===== New: designer-tweakable properties =====
        private int _leftModeReveal = 0; // height of Panel1 when showing Panel2 (arrow down)
        private int _rightModeReveal = 0;    // height of Panel2 when showing Panel1 (arrow up)
        private int _toggleMaxWidth = 20; // cap for toggle height
        private int _toggleButtonWidth = 24;
        private bool _toggleOnSplitterDoubleClick = true;

        [Browsable(true), Category("PatentLibrary"), Description("Width (px) of Panel1 kept visible when showing Panel2 (back arrow). 0 hides Panel1 completely (splitter at left)."), DefaultValue(0)]
        public int LeftModeReveal
        {
            get => _leftModeReveal;
            set { 
                _leftModeReveal = Math.Max(0, value); 
                ApplyRevealForCurrentMode(); 
                RepositionButton(); 
            }
        }

        [Browsable(true), Category("PatentLibrary"), Description("Width (px) of Panel2 kept visible when showing Panel1 (up arrow). 0 hides Panel2 completely (splitter at right)."), DefaultValue(0)]
        public int TopModeReveal
        {
            get => _rightModeReveal;
            set { 
                _rightModeReveal = Math.Max(0, value); 
                ApplyRevealForCurrentMode(); 
                RepositionButton(); 
            }
        }

        [Browsable(true), Category("PatentLibrary"), Description("Maximum width (px) of the toggle button; also clamped by SplitterWidth."), DefaultValue(20)]
        public int ToggleMaxWidth
        {
            get => _toggleMaxWidth;
            set {
                _toggleMaxWidth = Math.Max(1, value); 
                SyncButtonSizeToSplitter(); 
                SetToggleIcon(_toggleLeft); 
                RepositionButton(); 
            }
        }

        [Browsable(true), Category("PatentLibrary"), Description("Width (px) of the toggle button."), DefaultValue(24)]
        public int ToggleButtonWidth
        {
            get => _toggleButtonWidth;
            set { 
                _toggleButtonWidth = Math.Max(8, value); 
                SyncButtonSizeToSplitter(); 
                SetToggleIcon(_toggleLeft); 
                RepositionButton(); 
            }
        }

        [Browsable(true), Category("PatentLibrary"), Description("If true, double-clicking the splitter toggles between top/bottom views."), DefaultValue(true)]
        public bool ToggleOnSplitterDoubleClick
        {
            get => _toggleOnSplitterDoubleClick;
            set => _toggleOnSplitterDoubleClick = value;
        }
        // ===== End designer-tweakable properties =====

        // Added: fields to avoid local shadowing and allow async init
        private readonly string _patentsFolder = string.Empty;
        private MultiTabBrowser _browser;

        public PatentLibrary() : this(null!) { }

        public PatentLibrary(WinGUIMain mainForm)
        {
            InitializeComponent();

            MainFormReference = mainForm;

            // Create and add MultiTabBrowser into splitContainer1.Panel2
            _browser = new MultiTabBrowser(MainFormReference);
            _browser.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(_browser);

            #region =================== BEGIN SPLITER ========================

            // allow full collapse both ways, but we will
            // prefer "reveal" instead of true collapse
            splitContainer1.Panel1MinSize = 0;
            splitContainer1.Panel2MinSize = 0;

            btnToggle = new System.Windows.Forms.Button
            {
                Text = "",
                Width = _toggleButtonWidth,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnToggle.FlatAppearance.BorderSize = 0;
            btnToggle.ImageAlign = ContentAlignment.MiddleCenter;
            SyncButtonSizeToSplitter(); // ensure the button is not taller than the splitter (<= 20px)
            SetToggleIcon(_toggleLeft); // start with "back"
            btnToggle.Click += btnToggle_Click;

            // IMPORTANT: You cannot add directly to splitContainer1.Controls (it's read-only).
            // Keep the button on this UserControl and position it over the splitter.
            Controls.Add(btnToggle);

            // reposition the button whenever the splitter moves or resizes
            splitContainer1.SplitterMoved += (s, e) => RepositionButton();
            splitContainer1.SizeChanged += (s, e) =>
            {
                // Maintain the reveal amount when the control resizes
                ApplyRevealForCurrentMode();
                SyncButtonSizeToSplitter();     // keep size consistent on resize
                SetToggleIcon(_toggleLeft);     // rescale icon to new size
                RepositionButton();
            };
            splitContainer1.LocationChanged += (s, e) => RepositionButton();
            this.SizeChanged += (s, e) =>
            {
                ApplyRevealForCurrentMode();
                SyncButtonSizeToSplitter();
                SetToggleIcon(_toggleLeft);
                RepositionButton();
            };

            // New: double-click on the splitter to toggle (optional via property)
            splitContainer1.MouseDoubleClick += (s, e) =>
            {
                if (!_toggleOnSplitterDoubleClick) return;
                if (splitContainer1.Orientation != Orientation.Horizontal) return;
                if (splitContainer1.SplitterRectangle.Contains(e.Location))
                {
                    btnToggle_Click(btnToggle, EventArgs.Empty);
                }
            };

            // Initialize with a visible splitter at the center
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = false;
            //splitContainer1.SplitterDistance = Math.Max(SplitterReveal, splitContainer1.Height / 2);
            splitContainer1.SplitterDistance = 320; // Panel1 starts at 300px wide

            RepositionButton();

            this.Load += patentLibrary_Load;

            #endregion ================ END SPLITTER =========================

            var pm = new PathManager();
            _patentsFolder = pm.GetWritableFolder("Library");
            Directory.CreateDirectory(_patentsFolder);

            // Added: move async work to Load event so constructor isn't async
            this.Load += async (_, __) =>
            {
                await InitPdfPreviewAsync(_browser, _patentsFolder, "0A0625E967774EB5885A86794D259FF3.pdf");
            };
        }

        private void patentLibrary_Load(object? sender, EventArgs e)
        {
            var pm = new PathManager();
            var q = pm.GetWritableFolder("Databases");
            var dbPath = Path.Combine(q, "techarchive.aidb");
            LoadPdfGrid(dbPath);
        }

        private void LoadPdfGrid(string dbPath)
        {
            try
            {
                using var con = new SqliteConnection($"Data Source={dbPath};Mode=ReadWriteCreate;Cache=Shared");
                con.Open();

                using var cmd = con.CreateCommand();
                // Include PdfPath so we can open the file on double-click; keep Guid as TEXT to avoid Image-column issues
                cmd.CommandText = @"SELECT lower(hex(PdfGuid)) AS PdfGuid, FileName, PdfPath, Title FROM Pdfs ORDER BY Title;";

                using var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);

                void Bind()
                {
                    dgvPDF.AutoGenerateColumns = true;
                    dgvPDF.ReadOnly = true;                         // ← read-only
                    dgvPDF.AllowUserToAddRows = false;
                    dgvPDF.AllowUserToDeleteRows = false;
                    dgvPDF.MultiSelect = false;
                    dgvPDF.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvPDF.RowHeadersVisible = false;

                    dgvPDF.DataSource = dt;

                    // Hide internal columns
                    if (dgvPDF.Columns["PdfGuid"] != null) dgvPDF.Columns["PdfGuid"].Visible = false;
                    if (dgvPDF.Columns["FileName"] != null) dgvPDF.Columns["FileName"].Visible = false;
                    if (dgvPDF.Columns["PdfPath"] != null) dgvPDF.Columns["PdfPath"].Visible = false;

                    if (dgvPDF.Columns["Title"] != null)
                    {
                        dgvPDF.Columns["Title"].HeaderText = "Title";
                        dgvPDF.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    // Wire double-click once
                    dgvPDF.CellDoubleClick -= dgvPDF_CellDoubleClick;
                    dgvPDF.CellDoubleClick += dgvPDF_CellDoubleClick;
                }

                if (dgvPDF.InvokeRequired) dgvPDF.Invoke((Action)Bind); else Bind();
            }
            catch (SqliteException ex)
            {
                ErrorDialog.Show("SQLite Exception", ex.ToString());
            }
            catch (Exception ex)
            {
                ErrorDialog.Show("General Exception", ex.ToString());
            }
        }


        private async void dgvPDF_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                var row = dgvPDF.Rows[e.RowIndex];
                if (row?.DataBoundItem is not DataRowView drv)
                {
                    ErrorDialog.Show("Open PDF Error", "Row is not bound to a DataRowView.");
                    return;
                }

                var table = drv.DataView?.Table;
                if (table == null || !table.Columns.Contains("PdfPath"))
                {
                    ErrorDialog.Show("Open PDF Error", "The data does not include a 'PdfPath' column.");
                    return;
                }

                var relPath = drv["PdfPath"]?.ToString();
                if (string.IsNullOrWhiteSpace(relPath))
                {
                    ErrorDialog.Show("Open PDF Error", "PdfPath is empty for the selected row.");
                    return;
                }

                // Resolve to full path
                var pm = new PathManager();
                var baseRoot = pm.GetWritableFolder("");
                var fullPath = Path.Combine(baseRoot, relPath.Replace('/', Path.DirectorySeparatorChar));

                if (!File.Exists(fullPath))
                {
                    ErrorDialog.Show("Missing File", $"File not found:\r\n{fullPath}");
                    return;
                }

                var fileUrl = new Uri(fullPath).AbsoluteUri;

                // Ensure we actually have a browser instance
                if (_browser == null)
                {
                    ErrorDialog.Show("Open PDF Error", "_browser is not initialized.");
                    return;
                }

                await _browser.CreateNewTab(fileUrl);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show("Open PDF Error", ex.ToString());
            }
        }



        #region ============== BEGIN SPLITTER =============================

        private void btnToggle_Click(object? sender, EventArgs e)
        {
            // Flip between panels:
            // If current icon is "forward" ( _toggleLeft == false ), clicking shows BOTTOM panel (Panel2) with a small reveal for Panel1.
            // If current icon is "back" ( _toggleLeft == true ), clicking shows TOP panel (Panel1) with a small reveal for Panel2.

            if (!_toggleLeft)
            {
                // Show bottom panel "full", keep a configurable band of
                // Panel1 (often 0) so the splitter is visible at top.
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;

                splitContainer1.SplitterDistance = ClampSplitterDistance(Math.Max(0, _leftModeReveal));
                _toggleLeft = true; // switch icon to "back"
            }
            else
            {
                // Show top panel "full", keep a configurable band of
                // Panel2 (often 0) so the splitter is visible at Left.
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;

                int distanceForTop = Math.Max(0, splitContainer1.Height - splitContainer1.SplitterWidth - _rightModeReveal);
                //splitContainer1.SplitterDistance = ClampSplitterDistance(distanceForTop);
                splitContainer1.SplitterDistance = 320;
                _toggleLeft = false; // switch icon to "forward"
            }

            SetToggleIcon(_toggleLeft);
            RepositionButton();
        }

        private void SetToggleIcon(bool down)
        {
            try
            {
                // Use the ImageList ("back" / "forward") and 
                // scale to the larger button via BackgroundImage
                var key = down ? "forward" : "back";
                var img = imgListBlack?.Images[key];
                if (img != null)
                {
                    // scale a tad smaller than button for padding
                    var targetSize = new Size(Math.Max(1, btnToggle.Width - 6), Math.Max(1, btnToggle.Height - 6));
                    btnToggle.BackgroundImage = new Bitmap(img, targetSize);
                    btnToggle.BackgroundImageLayout = ImageLayout.Zoom;
                    btnToggle.Image = null; // ensure BackgroundImage is what renders
                }
            }
            catch { /* ignore scaling/image errors */ }
        }

        private void RepositionButton()
        {
            if (splitContainer1.Orientation != Orientation.Vertical) return;

            // Always center on the splitter bar within this UserControl's coordinate space
            Rectangle splitRect = splitContainer1.SplitterRectangle;

            // X centered over the SplitContainer
            int x = splitContainer1.Left + splitRect.Left + (splitRect.Width - btnToggle.Width) / 2;

            // Y centered on the splitter line
            int y = splitContainer1.Top + splitRect.Top + (splitRect.Height - btnToggle.Height) / 2;

            btnToggle.Left = Math.Max(0, x);
            btnToggle.Top = Math.Max(0, y);

            btnToggle.BringToFront();
        }

        private void ApplyRevealForCurrentMode()
        {
            if (splitContainer1.Orientation != Orientation.Horizontal) return;

            // Re-apply the reveal rule when the control size changes so the splitter remains visible.
            if (_toggleLeft)
            {
                // Bottom mode: keep Panel1 at LeftModeReveal
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.SplitterDistance = ClampSplitterDistance(Math.Max(0, _leftModeReveal));
            }
            else
            {
                // Top mode: keep Panel2 at rightModeReveal
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;
                int distanceForRight = Math.Max(0, splitContainer1.Height - splitContainer1.SplitterWidth - _rightModeReveal);
                splitContainer1.SplitterDistance = ClampSplitterDistance(distanceForRight);
            }
        }

        private int ClampSplitterDistance(int desired)
        {
            // Ensure the splitter distance stays within valid bounds
            int min = splitContainer1.Panel1MinSize;
            int max = Math.Max(0, splitContainer1.Width - splitContainer1.SplitterWidth - splitContainer1.Panel2MinSize);
            if (desired < min) desired = min;
            if (desired > max) desired = max;
            return desired;
        }

        private void SyncButtonSizeToSplitter()
        {
            // Ensure the button height is never larger than
            // splitter width and never exceeds property cap
            int splitterH = Math.Max(1, splitContainer1.SplitterWidth);
            int targetH = Math.Min(splitterH - 2, _toggleMaxWidth); // leave 1px margins
            if (targetH < 12) targetH = Math.Min(12, splitterH);    // keep it reasonably clickable

            // Use designer-tweakable width
            int targetW = Math.Max(8, _toggleButtonWidth);

            btnToggle.Size = new Size(targetW, targetH);
        }

        #endregion ============== END SPLITTER =============================

        // Added: helper to perform the async WebView2 navigation safely
        private static async Task InitPdfPreviewAsync(MultiTabBrowser browser, string patentsFolder, string fileName)
        {
            try
            {
                string path = Path.Combine(patentsFolder, fileName);

                if (File.Exists(path))
                {
                    // Convert to file:// URI and navigate using MultiTabBrowser API
                    var fileUrl = new Uri(Path.GetFullPath(path)).AbsoluteUri;
                    await browser.CreateNewTab(fileUrl);
                }
                else
                {
                    MessageBox.Show("PDF not found at: " + path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing PDF preview: " + ex.Message);
            }
        }
    }
}

public static class ErrorDialog
{
    public static void Show(string title, string message)
    {
        Form f = new Form
        {
            Text = title,
            Width = 700,
            Height = 500,
            StartPosition = FormStartPosition.CenterScreen
        };

        var tb = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Both,
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 10),
            Text = message,
            WordWrap = false
        };

        f.Controls.Add(tb);

        var ok = new Button
        {
            Text = "OK",
            Dock = DockStyle.Bottom,
            DialogResult = DialogResult.OK
        };
        f.Controls.Add(ok);

        f.AcceptButton = ok;
        f.ShowDialog();
    }
}