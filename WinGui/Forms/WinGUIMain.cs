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

//using AiNetStudio.WinGui.Dialogs;
using AiNetStudio.Helpers;
using AiNetStudio.WinGui.Controls;
using AiNetStudio.WinGui.ControlScreens;
using CustomControls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
//using SkinDialogs;
//using AIDemos.Models;
//using BlackBox;
//using AiNetStudio.DataAccess;
//using AiNetStudio.WinGui.ControlScreens;
//using AiNetStudio.Helpers;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Drawing.ThemedColors;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace AiNetStudio.WinGui.Forms
{
    public partial class WinGUIMain : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WinGUIMain? WinGUIMainRef { get; internal set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAuthorized
        {
            get { return m_bAuthorized; }
            set { m_bAuthorized = value; }
        }

        // Subscribe to events
        public event Action<string>? DisableTabsEvent;
        public event Action<string>? EnableTabsEvent;

        public bool m_bAuthorized = false;
        public string m_sOptions = string.Empty;

        private List<string> disabledTabs = new(); // no need for nullable string here

        private TabControlEx tabControlEx1;
        TabPage tp1 = new TabPage();
        TabPage tp2 = new TabPage();
        TabPage tp3 = new TabPage();
        TabPage tp4 = new TabPage();
        TabPage tp5 = new TabPage();
        TabPage tp6 = new TabPage();
        TabPage tp7 = new TabPage();
        TabPage tp8 = new TabPage();
        TabPage tp9 = new TabPage();
        TabPage tp10 = new TabPage();

        //private readonly FdicApiService? _fdicApiService;
        //public List<BankInfo?> PreloadedBanks = new(); // Store the bank list in memory

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsBankDataLoaded { get; private set; } = false; // Track loading status

        public WinGUIMain()
        {
            DoubleBuffered = true;
            base.DoubleBuffered = true;

            // AllPaintingInWmPaint - instead of OnPaintBackground getting called from the WM_ERASEBACKGROUND message,
            // it will be called from WM_PAINT. This way you dont have to change your existing painting.
            this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, false);
            this.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();

            tabControlEx1 = new TabControlEx();

            tabControlEx1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            // Set properties for tabControlEx1
            tabControlEx1.Location = new Point(0, 55);
            tabControlEx1.Size = new System.Drawing.Size(400, 300);
            tabControlEx1.Dock = DockStyle.Fill;
            tabControlEx1.TabIndex = 0;
            tabControlEx1.SelectedIndex = 0;

            tabControlEx1.DisplayStyle = TabStyle.Chrome;

            tabControlEx1.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            tabControlEx1.DisplayStyleProvider.FocusTrack = false;
            tabControlEx1.DisplayStyleProvider.HotTrack = true;
            tabControlEx1.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            tabControlEx1.DisplayStyleProvider.Opacity = 1F;
            tabControlEx1.DisplayStyleProvider.Overlap = 7;
            tabControlEx1.DisplayStyleProvider.Padding = new System.Drawing.Point(14, 1);
            tabControlEx1.DisplayStyleProvider.ShowTabCloser = false;
            tabControlEx1.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            tabControlEx1.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            tabControlEx1.HotTrack = true;
            //tabControlEx1.ImageList = this.imageList1;
            tabControlEx1.Location = new System.Drawing.Point(12, 44);
            tabControlEx1.Name = "tabControlEx1";
            tabControlEx1.SelectedIndex = 0;
            tabControlEx1.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            // Add tabControlEx1 to the form's controls
            this.Controls.Add(tabControlEx1);

            //this.AutoScaleMode = AutoScaleMode.Dpi;

            InitializeComponent();

            SQLitePCL.Batteries.Init();

            //string serial = HDDSerial.GetSerial("NP1732");
            //IsAuthorized = LicenseManager.ReadLicenseFile(serial, out string custEmail, out string authCode, out string options);
            //if (IsAuthorized)
            //{
            //    MessageBox.Show("EUREKA!");
            //}
            //GlobalData.IsAuthorized = IsAuthorized;

            this.FormClosing += WinGUIMain_FormClosing;

            //CompanyManager companyMgr = new CompanyManager();
            //companyMgr.CreateAndInsertCompany();
            //var importer = new NaicsImporter();
            //importer.ImportNaicsCodes();

            SetTabStyle();

            this.Load += WinGUIMain_Load!;

            tabControlEx1.BackColor = Color.White;
            this.BackColor = Color.White;

            SetHandCursor();

            this.Width = (int)(1200);
            this.Height = (int)(760);

            LoadTabs();

            //DatabasePathManager dbManager = new DatabasePathManager();
            //string dbFolder = dbManager.GetDatabaseFolder();
            //MessageBox.Show($"Database Folder: {dbFolder}");

            //DatabasePathManager dbManager = new DatabasePathManager();
            //string dbPath = dbManager.GetDatabasePath("MyDatabase");
            //MessageBox.Show($"Database Path: {dbPath}");

            tabControlEx1.SelectedIndexChanged += tabControlEx1_SelectedIndexChanged;

            //SelectCompany();

            #region ========== BEGIN SHOW HAND CURSOR OVER TABS ================

            //GetTabRect(int) gives bounding rectangle of each tab header; you
            //just hit-test mouse location against those rects and set the cursor
            //accordingly. This is intended API for tab header geometry and hit
            //testing (see GetTabRect and tab control hit testing in the docs).

            tabControlEx1.MouseMove += (s, e) =>
            {
                for (int i = 0; i < tabControlEx1.TabPages.Count; i++)
                {
                    var headerRect = tabControlEx1.GetTabRect(i); // header area for tab i
                    if (headerRect.Contains(e.Location))
                    {
                        if (tabControlEx1.Cursor != Cursors.Hand)
                            tabControlEx1.Cursor = Cursors.Hand;
                        return; // we're over a tab header; done
                    }
                }
                // not over any header
                if (tabControlEx1.Cursor != Cursors.Default)
                    tabControlEx1.Cursor = Cursors.Default;
            };

            tabControlEx1.MouseLeave += (s, e) =>
            {
                tabControlEx1.Cursor = Cursors.Default;
            };

            #endregion ======= END SHOW HAND CURSOR OVER TABS ==================

        }

        public void SelectTab(int iTab)
        {
            tabControlEx1.SelectedIndex = iTab;
        }

        //public void SelectCompany()
        //{
        //    using (SelectDlg dlg = new SelectDlg())
        //    {
        //        dlg.selectedDatabase = GlobalData.SelectedDatabase;

        //        var result = dlg.ShowDialog();
        //        GlobalData.SelectedDatabase = dlg.selectedDatabase;

        //        if (dlg.OpenNewCompanyDlg)
        //        {
        //            try
        //            {
        //                using (CompanyDlg companyDlg = new CompanyDlg(this))
        //                {
        //                    companyDlg.ShowDialog();
        //                    //GlobalData.SelectedDatabase = companyDlg.selectedDatabase;
        //                    //lblDBName.Text = GlobalData.SelectedDatabase;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Failed to launch CompanyDlg: " + ex.Message);
        //            }

        //        }
        //    }
        //}

        public void HandleEventFromBrowser(string payload /* format: "action:value" */)
        {
            var action = payload;
            var value = string.Empty;
            int i = payload.IndexOf(':');
            if (i >= 0)
            {
                action = payload.Substring(0, i);
                value = payload.Substring(i + 1);
            }

            // Show a message box with action and value
            MessageBox.Show(
                $"Action: {action}\nValue: {value}",
                "Browser Event",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            System.Diagnostics.Debug.WriteLine($"BrowserEvent => action={action}, value={value}");
        }


        private void WinGUIMain_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!IsAuthorized)
            {
                return;
            }

            DialogResult result = MessageBox.Show("Do you want to back up your data before exiting?", "Backup Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            //if (result == DialogResult.Yes)
            //{
            //    try
            //    {
            //        PathManager path = new PathManager();
            //        string companyFolder = path.GetAnyFolder("Company");

            //        string tempBackupFolder = Path.Combine(Path.GetTempPath(), "AINetBackupTemp");
            //        if (Directory.Exists(tempBackupFolder))
            //            Directory.Delete(tempBackupFolder, true);
            //        Directory.CreateDirectory(tempBackupFolder);

            //        // 🧠 Copy all files to temp folder
            //        foreach (var file in Directory.GetFiles(companyFolder))
            //        {
            //            var fileName = Path.GetFileName(file);
            //            var dest = Path.Combine(tempBackupFolder, fileName);
            //            File.Copy(file, dest, true); // 🔥 Force overwrite even if in use
            //        }

            //        string backupFileName = $"CompanyBackup_{DateTime.Now:MM_dd_yyyy}.zip";

            //        using SaveFileDialog saveDialog = new SaveFileDialog();
            //        saveDialog.Title = "Save Backup As";
            //        saveDialog.Filter = "ZIP files (*.zip)|*.zip";
            //        saveDialog.FileName = backupFileName;

            //        if (saveDialog.ShowDialog() == DialogResult.OK)
            //        {
            //            string destinationZip = saveDialog.FileName;

            //            if (File.Exists(destinationZip))
            //                File.Delete(destinationZip);

            //            ZipFile.CreateFromDirectory(tempBackupFolder, destinationZip, CompressionLevel.Optimal, includeBaseDirectory: true);

            //            MessageBox.Show("✅ Backup created successfully.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }

            //        // 🧼 Cleanup
            //        Directory.Delete(tempBackupFolder, true);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"❌ Backup failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        private void tabControlEx1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Fire a generic "tab activated" hook if the tab's main control implements ITabAware
            //var host = tabControlEx1.SelectedTab?.Controls.Count > 0 ? tabControlEx1.SelectedTab.Controls[0] : null;
            //if (host is ITabAware aware)
            //{
            //    aware.OnTabActivated();
            //}

            //WS
            TabPage selectedTab = tabControlEx1.SelectedTab!;

            tp1.Text = "Welcome";
            tp2.Text = "Browser";
            tp3.Text = "Device Hub";
            tp4.Text = "Video Editor";
            //tp5.Text = "Transactions";
            //tp6.Text = "Reports";
            //tp7.Text = "Categories";
            //tp8.Text = "Export";
            //tp9.Text = "Settings";

            // Example: handle tab switch
            if (selectedTab == tp1) // "Welcome"
            {
                if (tp1.Controls[0] is WelcomeControl welcomeCtrl)
                {
                    //welcomeCtrl.UpdateCompany();
                }
            }
            else if (selectedTab == tp2)  // "Browser"
            {
                if (tp2.Controls[0] is MultiTabBrowser browserCtrl)
                {
                    browserCtrl.OnTabActivated();
                }
            }
            else if (selectedTab == tp3)  // "DeviceHub"
            {
                // Refresh or update controls for company tab
                if (tp3.Controls[0] is DeviceHubControl bankCtrl)
                {
                   // bankCtrl.UpdateCompany();
                }
            }
            else if (selectedTab == tp4)  // "SplitEditor"
            {
                if (tp4.Controls[0] is SplitEditor importCtrl)
                {
                    //importCtrl.UpdateCompany();
                }
            }
            //else if (selectedTab == tp5) // "Transactions"
            //{
            //    if (tp5.Controls[0] is TransactionControl transCtrl)
            //    {
            //        transCtrl.UpdateCompany();
            //        transCtrl.UpdateCompanyComboBox();

            //        transCtrl.LoadUserCompanyCategories();

            //        transCtrl.LoadTransactionsIntoGrid();

            //        //GlobalData.SelectedDatabase = "demo_company.aidb";
            //        //if (statusStrip1.Text.Length > 0)
            //        //    GlobalData.SelectedDatabase = statusStrip1.Text;
            //    }
            //}
            //else if (selectedTab == tp6) // "Reports"
            //{
            //    if (tp6.Controls[0] is ReportControl reportCtrl)
            //    {
            //        reportCtrl.UpdateCompany();
            //        //reportCtrl.UpdateExportedReports();
            //        reportCtrl.UpdateCompanyComboBox();
            //        //checkedListBoxAccounts
            //    }
            //}
            //else if (selectedTab == tp7) // "Categories"
            //{
            //    if (tp7.Controls[0] is CategoryControl categoryCtrl)
            //    {
            //        //categoryCtrl.UpdateCompany();
            //        categoryCtrl.UpdateCompanyComboBox();
            //    }
            //}
            //else if (selectedTab == tp8) // "Export"
            //{
            //    if (tp8.Controls[0] is ExportControl exportCtrl)
            //    {
            //        exportCtrl.UpdateCompany();
            //    }
            //}
            //else if (selectedTab == tp9) // "Settings"
            //{
            //    if (tp9.Controls[0] is SettingsControl settingsCtrl)
            //    {
            //        //settingsCtrl.UpdateExportedReports();
            //        //settingsCtrl.UpdateCompanyComboBox();
            //    }
            //}
        }

        #region ========= BEGIN btnAuthorize =================

        //public void Authorize()
        //{
        //    //tsbAuthorize_Click(null, EventArgs.Empty);
        //    //BlackBox.SoftwareLock SLGen = new BlackBox.SoftwareLock();
        //    //SLGen.AppID = "NP1732";
        //    //m_bAuthorized = SLGen.IsAuthorized;

        //    using (AuthorizeDlg authorizeDlg = new AuthorizeDlg())
        //    {
        //        authorizeDlg.ShowDialog();
        //        Application.Restart();
        //        Environment.Exit(0);

        //        //authorizeDlg.AppID = "NP1732";

        //        //if (authorizeDlg.ShowDialog() == DialogResult.OK)
        //        //{
        //        //    m_bAuthorized = authorizeDlg.IsAuthorized;
        //        //    IsAuthorized = m_bAuthorized;
        //        //    GlobalData.IsAuthorized = m_bAuthorized;
        //        //}
        //        //if (tabControlEx1.SelectedTab?.Controls[0] is CompanyControl myUserControl)
        //        //{
        //        //    myUserControl.SetAuthoriseLabel(m_bAuthorized);
        //        //}
        //    }
        //}

        private void tsbAuthorize_Click(object? sender, EventArgs e)
        {
            //Authorize();
        }

        #endregion ========= END btnAuthorize =================

        //public void Authorize()
        //{
        //    ToolStripButton btn = toolStrip1.Items
        //        .OfType<ToolStripButton>()
        //        .FirstOrDefault(b => b.Text == "License")!;

        //    btn?.PerformClick(); // Perform click if the button exists

        //}

        private void TabControlEx1_DrawItem1(object? sender, DrawItemEventArgs e)
        {
            //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 45)), e.Bounds);
            //Rectangle paddedBounds = e.Bounds;
            //paddedBounds.Inflate(-2, -2);
            //e.Graphics.DrawString(tabControlEx1.TabPages[e.Index].Text, this.Font, SystemBrushes.HighlightText, paddedBounds);
            //Graphics g = e.Graphics;
            //Pen p = new Pen(Color.FromArgb(45, 45, 45), 10);
            //g.DrawRectangle(p, tp1.Bounds);
        }

        private void SetHandCursor()
        {
            toolStrip1.MouseEnter += (s, ev) => toolStrip1.Cursor = Cursors.Hand;
            toolStrip1.MouseLeave += (s, ev) => toolStrip1.Cursor = Cursors.Default;
        }

        private void LoadTabs()
        {
            //pnlAccounts
            //pnlBanks
            //pnlAccounts
            //pnlImport
            //pnlReport
            //pnlSettings
            //pnlTransaction

            tp1.Text = "Welcome";
            tabControlEx1.TabPages.Add(tp1);
            WelcomeControl welcomeControl = new WelcomeControl(this);
            welcomeControl.MainFormReference = this;
            welcomeControl.Dock = DockStyle.Fill;
            tp1.Controls.Add(welcomeControl);

            tp2.Text = "Browser";
            tabControlEx1.TabPages.Add(tp2);
            MultiTabBrowser browserControl = new MultiTabBrowser(this);
            browserControl.MainFormReference = this;
            browserControl.Dock = DockStyle.Fill;
            tp2.Controls.Add(browserControl);

            tp3.Text = "Device Hub";
            tabControlEx1.TabPages.Add(tp3);
            DeviceHubControl deviceHybControl = new DeviceHubControl(this);
            deviceHybControl.MainFormReference = this;
            deviceHybControl.Dock = DockStyle.Fill;
            tp3.Controls.Add(deviceHybControl);

            tp4.Text = "Video Editor";
            tabControlEx1.TabPages.Add(tp4);
            SplitEditor splitEditor = new SplitEditor(this);
            splitEditor.MainFormReference = this;
            splitEditor.Dock = DockStyle.Fill;
            tp4.Controls.Add(splitEditor);

            //tp5.Text = "Transactions";
            //tabControlEx1.TabPages.Add(tp5);
            //TransactionControl transControl = new TransactionControl(this);
            //transControl.MainFormReference = this;
            //transControl.Dock = DockStyle.Fill;
            //tp5.Controls.Add(transControl);

            //tp6.Text = "Reports";
            //tabControlEx1.TabPages.Add(tp6);
            //ReportControl reportsControl = new ReportControl(this);
            //reportsControl.MainFormReference = this;
            //reportsControl.Dock = DockStyle.Fill;
            //tp6.Controls.Add(reportsControl);

            //tp7.Text = "Categories";
            //tabControlEx1.TabPages.Add(tp7);
            //CategoryControl categoryControl = new CategoryControl(this);
            //categoryControl.MainFormReference = this;
            //categoryControl.Dock = DockStyle.Fill;
            //tp7.Controls.Add(categoryControl);

            //tp8.Text = "Export";
            //tabControlEx1.TabPages.Add(tp8);
            //ExportControl exportControl = new ExportControl(this);
            //exportControl.MainFormReference = this;
            //exportControl.Dock = DockStyle.Fill;
            //tp8.Controls.Add(exportControl);

            //tp9.Text = "Settings";
            //tabControlEx1.TabPages.Add(tp9);
            ////SettingsControl settingsControl = new SettingsControl(this);
            //settingsControl.MainFormReference = this;
            //settingsControl.Dock = DockStyle.Fill;
            //tp9.Controls.Add(settingsControl);

            tabControlEx1.Selecting += tabControlEx1_Selecting!;

            statusStripCopyright.Text = $"Copyright 2024 - {DateTime.Now.Year} Ouslan, Inc., All Rights Reserved Worldwide.";
            statusStripVersion.Text = "Version: " + VersionHelper.GetAssemblyVersion();
            //Version: VersionHelper.GetInformationalVersion() </ span >

            toolStrip1.AutoSize = false;
            toolStrip1.Height = 40; //pixels

            AddDefaultButtons();
            AddScienceSitesMenu();
            AddAISitesMenu();
            //AddMerchantAccountsMenu();
            //AddYouTubeFinanceMenu();
            AddTubesMenu();
            AddYouTubeSerGioMenu();

            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item is ToolStripButton button)
                {
                    button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
            }

            //toolStrip1.Renderer = new CustomToolStripRenderer();
            SetAllToolStripRenderMode(this);
            toolStrip1.BackColor = Color.White;

            //tsbAbout.Click += tsbAbout_Click;
            //tsbManual.Click += tsbManual_Click;
            //tsbPassGuard.Click += tsbPassGuard_Click;

        }



        private void SetAllToolStripRenderMode(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is MenuStrip menuStrip)
                {
                    menuStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                }
                else if (ctrl is ToolStrip toolStrip)
                {
                    toolStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                }

                // Recursively check for nested controls (e.g., inside panels, tab pages, etc.)
                if (ctrl.HasChildren)
                {
                    SetAllToolStripRenderMode(ctrl);
                }
            }
        }


        //private void tsbAbout_Click(object? sender, EventArgs e)
        //{
        //    using (AboutDialog aboutDialog = new AboutDialog())
        //    {
        //        aboutDialog.ShowDialog();
        //    }
        //}

        //private void tsbManual_Click(object? sender, EventArgs e)
        //{
        //    OpenUrlInChromeOrDefault("https://ainetprofit.com/Manual");
        //}

        //private void tsbPassGuard_Click(object? sender, EventArgs e)
        //{
        //    DatabaseManager pathManager = new DatabaseManager();
        //    string? path = pathManager.GetProgramFiles32();
        //    string? passGuard = Path.Combine(path, "PassGuard/PassGuard.exe");

        //    if (File.Exists(passGuard))
        //    {
        //        try
        //        {
        //            Process.Start(new ProcessStartInfo
        //            {
        //                FileName = passGuard,
        //                UseShellExecute = true
        //            });
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Failed to launch PassGuard.exe:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("PassGuard.exe not found in the specified folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        private void TabControlEx1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Get the tab page and its text
            TabPage tabPage = tabControlEx1.TabPages[3];
            string tabText = tabPage.Text;

            // Set the default drawing
            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), e.Bounds);

            // Set the text color to Red
            using (Brush brush = new SolidBrush(Color.Red))
            {
                // Draw the text in the specified color and font
                e.Graphics.DrawString(tabText, e.Font!, brush, e.Bounds.Left + 3, e.Bounds.Top + 3);
            }
        }

        public void UpdateStatusStrip(string database)
        {
            // Use BeginInvoke to ensure it's updated on the UI thread
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(() =>
                {
                    //statusStripDatabase.Text = database;
                    //statusStripCopyright.Text = $"Copyright 1991 - {DateTime.Now.Year} Ouslan, Inc., All Rights Reserved Worldwide.";
                    //statusStripVersion.Text = VersionHelper.GetFormattedAppTitle();
                });
            }
        }

        private void tabControlEx1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage != null && disabledTabs.Contains(e.TabPage.Text))
            {
                e.Cancel = true;
            }
        }

        private void WinGUIMain_Load(object sender, EventArgs e)
        {

            //var listCompany = new List<CompanyDatabase>();
            //CompanyManager cm = new CompanyManager();
            //listCompany = cm.GetCompanies();

            //string s = "";

            //var _theme = AppSettings.Get("Theme");
            //if (_theme == "Dark")
            //{
            //    ApplyDarkTheme(this);
            //} else
            //{
            //    ApplyLightTheme(this);
            //}

            //var qqq = new TransactionManager();
            //qqq.CreateTransactionsTable();

            //var importer = new NaicsImporter();
            //importer.ImportNaicsCodes();

            //CategoryMasterTableManager caty = new CategoryMasterTableManager();
            //caty.LoadDefaultCategories();
            //caty.CopyAllCategoriesToUserCategories();

            //AccountManager acctManager = new AccountManager();
            //acctManager.CreateAccountsTable();

            //Thread thread = new Thread(() =>
            //{
            //    try
            //    {
            //        var _service = new BankImportService();
            //        _service.ImportCsvToSqlite();
            //    }
            //    catch (Exception ex)
            //    {
            //        if (this.IsHandleCreated && !this.IsDisposed)
            //        {
            //            this.Invoke(() =>
            //            {
            //                MessageBox.Show(this, $"Error loading banks: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            });
            //        }
            //    }
            //});
            //thread.IsBackground = true;
            //thread.Start();
        }


        #region ============= BEGIN MENU ITEMS ===============================


        private void ToolStripItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            MessageBox.Show($"You clicked {clickedItem.Text}");
        }

        private void TabStyleItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem? clickedItem = sender as ToolStripMenuItem;

            switch (clickedItem!.Text!.ToString())
            {
                case "Chrome":
                    tabControlEx1.DisplayStyle = TabStyle.Chrome;
                    break;
                case "VisualStudio":
                    tabControlEx1.DisplayStyle = TabStyle.VisualStudio;
                    break;
                case "Rounded":
                    tabControlEx1.DisplayStyle = TabStyle.Rounded;
                    break;
                case "Angled":
                    tabControlEx1.DisplayStyle = TabStyle.Angled;
                    break;
                case "Rectangular":
                    tabControlEx1.DisplayStyle = TabStyle.Rectangular;
                    break;
                case "Default":
                    tabControlEx1.DisplayStyle = TabStyle.Default;
                    break;
                case "VS2010":
                    tabControlEx1.DisplayStyle = TabStyle.VS2010;
                    break;
                case "VS2012":
                    tabControlEx1.DisplayStyle = TabStyle.VS2012;
                    break;
                case "IE8":
                    tabControlEx1.DisplayStyle = TabStyle.IE8;
                    break;
            }

            //AppSettings.Set("TabStyle", clickedItem.Text); // Save user preference
            tabControlEx1.Update(); // Redraw
        }

        public void SetTabStyle()
        {
            //string? tabStyle = AppSettings.Get("TabStyle");

            //switch (tabStyle)
            //{
            //    case "Chrome":
            //        tabControlEx1.DisplayStyle = TabStyle.Chrome;
            //        break;
            //    case "VisualStudio":
            //        tabControlEx1.DisplayStyle = TabStyle.VisualStudio;
            //        break;
            //    case "Rounded":
            //        tabControlEx1.DisplayStyle = TabStyle.Rounded;
            //        break;
            //    case "Angled":
            //        tabControlEx1.DisplayStyle = TabStyle.Angled;
            //        break;
            //    case "Rectangular":
            //        tabControlEx1.DisplayStyle = TabStyle.Rectangular;
            //        break;
            //    case "Default":
            //        tabControlEx1.DisplayStyle = TabStyle.Default;
            //        break;
            //    case "VS2010":
            //        tabControlEx1.DisplayStyle = TabStyle.VS2010;
            //        break;
            //    case "VS2012":
            //        tabControlEx1.Font = new Font("Arial", 18, FontStyle.Bold);
            //        tabControlEx1.DisplayStyle = TabStyle.VS2012;
            //        break;
            //    case "IE8":
            //        tabControlEx1.DisplayStyle = TabStyle.IE8;
            //        break;
            //}
            tabControlEx1.DisplayStyle = TabStyle.Rounded;
            tabControlEx1.Update(); // Redraw
        }

        #endregion =============== END MENU ITEMS ============================

        #region ================== BEGIN EXTRA MENUS ==========================

        private void AddDefaultButtons()
        {
            #region ========= BEGIN btnExit =================

            ToolStripButton btnExit = new ToolStripButton
            {
                Text = "Exit",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            btnExit.Click += (sender, e) =>
            {
                Application.Exit();
            };

            toolStrip1.Items.Add(btnExit);

            #endregion ========= END btnExit =================

            #region ========= BEGIN btnCompany =================

            ToolStripButton btnCompany = new ToolStripButton
            {
                Text = "Company",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            //btnCompany.Click += (sender, e) =>
            //{
            //    SelectCompany();
            //};

            //toolStrip1.Items.Add(btnCompany);

            #endregion ========= END btnExit =================

            #region ========= BEGIN btnAbout =================

            ToolStripButton btnAbout = new ToolStripButton
            {
                Text = "About",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            btnAbout.Click += (sender, e) =>
            {
                //using (AboutDialog aboutDialog = new AboutDialog())
                //{
                //    aboutDialog.ShowDialog();
                //}
                OpenUrlInChromeOrDefault("https://ainetstudio.com/");

                // Set form title
                //this.Text = VersionHelper.GetFormattedAppTitle();

                // Show About dialog
                //MessageDlg dlgCreateRgn = new MessageDlg();
                //dlgCreateRgn.SetMessage(
                //    $"AiNetStudio Version:\n{VersionHelper.GetInformationalVersion()}",
                //    "Version",
                //    "Click below to purchase",
                //    "https://ainetprofit.com/");
                //dlgCreateRgn.Show();
            };
            toolStrip1.Items.Add(btnAbout);

            #endregion ========= END btnAbout =================

            #region ========= BEGIN btnAuthorize =================
            ToolStripButton btnAuthorize = new ToolStripButton
            {
                Text = "License",
                AutoSize = true,
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            btnAuthorize.Click += (sender, e) =>
            {
                //singleton oSingleton = singleton.GetCurrentSingleton;
                //SoftwareLock SLGen = new SoftwareLock();
                //SLGen.AppID = "NP1732";
                //oSingleton.IsAuthorized = SLGen.IsAuthorized;
                //BlackBox.LoginForm loginForm = new BlackBox.LoginForm();
                //loginForm.AppID = "NP1732";
                //loginForm.ShowDialog();
                //Application.Restart();
                //Environment.Exit(0);

                //if (this.FindForm() is WinGUIMain mainForm)
                //{
                //    mainForm.Authorize();
                //}
            };
            toolStrip1.Items.Add(btnAuthorize);
            #endregion ========= END btnAuthorize =================

            #region ========= BEGIN ddTabStyles =================
            //ToolStripDropDownButton ddTabStyles = new ToolStripDropDownButton
            //{
            //    Text = "Tab Style",
            //    AutoSize = true,
            //    DisplayStyle = ToolStripItemDisplayStyle.Text,
            //    Font = new Font("Segoe UI", 10, FontStyle.Regular)
            //};

            //string[] tabStyles = new[] { "Chrome", "VisualStudio", "Angled", "Default", "VS2010", "IE8" };

            //foreach (string style in tabStyles)
            //{
            //    var item = new ToolStripMenuItem(style);
            //    item.Click += (s, e) =>
            //    {
            //        // Suspend layout to prevent flicker/distortion
            //        this.SuspendLayout();
            //        tabControlEx1.SuspendLayout();

            //        // Apply tab style
            //        switch (style)
            //        {
            //            case "Chrome":
            //                tabControlEx1.DisplayStyle = TabStyle.Chrome;
            //                break;
            //            case "VisualStudio":
            //                tabControlEx1.DisplayStyle = TabStyle.VisualStudio;
            //                break;
            //            case "Angled":
            //                tabControlEx1.DisplayStyle = TabStyle.Angled;
            //                break;
            //            case "Default":
            //                tabControlEx1.DisplayStyle = TabStyle.Default;
            //                break;
            //            case "VS2010":
            //                tabControlEx1.DisplayStyle = TabStyle.VS2010;
            //                break;
            //            case "IE8":
            //                tabControlEx1.DisplayStyle = TabStyle.IE8;
            //                break;
            //        }

            //        // Force repaint
            //        tabControlEx1.ResumeLayout(true);
            //        tabControlEx1.Refresh();
            //        tabControlEx1.Invalidate();

            //        this.ResumeLayout(true);
            //        this.PerformLayout();
            //    };
            //    ddTabStyles.DropDownItems.Add(item);
            //}

            //toolStrip1.Items.Add(ddTabStyles);
            #endregion ========= END ddTabStyles =================

            #region ========= BEGIN btnPassGuard =================

            //tsbAbout.Text = "About" + Environment.NewLine + "Us!";
            //tsbPassGuard.Text = "Cloaker®";
            //ToolStripButton btnPassGuard = new ToolStripButton
            //{
            //    Text = "Cloaker®",
            //    ForeColor = Color.Red,
            //    Font = new Font("Segoe UI", 10, FontStyle.Bold),
            //    AutoSize = true,
            //    DisplayStyle = ToolStripItemDisplayStyle.Text
            //};

            //btnPassGuard.Click += (sender, e) =>
            //{
            //    //CloakerForm cloakerForm = new CloakerForm();
            //    //cloakerForm.ShowDialog();

            //    //DatabaseManager pathManager = new DatabaseManager();
            //    //string? path = pathManager.GetProgramFiles32();
            //    //string? passGuard = Path.Combine(path, "PassGuard/PassGuard.exe");
            //    //if (File.Exists(passGuard))
            //    //{
            //    //    try
            //    //    {
            //    //        Process.Start(new ProcessStartInfo
            //    //        {
            //    //            FileName = passGuard,
            //    //            UseShellExecute = true
            //    //        });
            //    //    }
            //    //    catch (Exception ex)
            //    //    {
            //    //        OpenUrlInChromeOrDefault("https://software-rus.com/PassGuard");
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    OpenUrlInChromeOrDefault("https://software-rus.com/PassGuard");
            //    //}
            //};
            //toolStrip1.Items.Add(btnPassGuard);

            #endregion ========= END btnPassGuard =================

            #region ========= BEGIN btnManual =================

            ToolStripButton btnManual = new ToolStripButton
            {
                Text = "Manual",
                AutoSize = true,
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            btnManual.Click += (sender, e) =>
            {
                string url = "https://ainetstudio.com/Manual";
                OpenUrlInChromeOrDefault(url);
            };
            toolStrip1.Items.Add(btnManual);
            #endregion ========= END btnManual =================

            #region ========= BEGIN btnPurchase =================
            ToolStripButton btnPurchase = new ToolStripButton
            {
                Text = "Purchase",
                AutoSize = true,
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            btnPurchase.Click += (sender, e) =>
            {
                // Switch to the "Videos" tab
                //tabControlEx1.SelectedTab = tp3;
                // Get the YouTube UserControl instance
                //BrowserControl web = tp3.Controls.OfType<BrowserControl>().FirstOrDefault()!;
                string url = "https://ainetprofit.com/Checkout";
                OpenUrlInChromeOrDefault(url);
            };
            //toolStrip1.Items.Add(btnPurchase);
            #endregion ========= END btnPurchase =================

        }

        private void AddScienceSitesMenu()
        {
            // Create the main dropdown menu item
            ToolStripMenuItem domainAuctionsMenu = new ToolStripMenuItem("Science Sites");
            domainAuctionsMenu.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Define the menu items and their URLs
            var menuItems = new (string Name, string Url)[]
            {
                ("AlienScientist", "https://www.youtube.com/AlienScientist"),
                ("APEC", "https://www.altpropulsion.com/"),
                ("TheoriesofEverything", "https://www.youtube.com/TheoriesofEverything")
            };

            // Add each submenu item with click event
            foreach (var (name, url) in menuItems)
            {
                ToolStripMenuItem subMenuItem = new ToolStripMenuItem(name);
                subMenuItem.Click += (sender, e) => OpenUrlInChromeOrDefault(url);
                domainAuctionsMenu.DropDownItems.Add(subMenuItem);
            }

            // Add the dropdown menu to menuStrip1
            toolStrip1.Items.Add(domainAuctionsMenu);

        }

        private void AddAISitesMenu()
        {
            // Create the main dropdown menu item
            ToolStripMenuItem aiSitesMenu = new ToolStripMenuItem("AI-Sites");
            aiSitesMenu.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Define the menu items and their URLs
            var menuItems = new (string Name, string Url)[]
            {
                ("FREE AI Accounting Software", "https://ainetprofit.com"),
                ("AI Anty Gravity Research", "https://aiantigravity.com"),
                ("Ai Net Studio", "https://ainetstudio.com"),
                ("Ai Health Buzz", "https://aihealthbuzz.com/"),
                ("Ollama: Run Large Language Models Locally", "https://ollama.com"),
                ("DeepSeek: Advanced AI Chat Platform", "https://deepseek.com"),
                ("ChatGPT by OpenAI", "https://chat.openai.com"),
                ("Google AI: Advancing AI for Everyone", "https://ai.google"),
                ("Alibaba Cloud AI Solutions", "https://www.alibabacloud.com/solutions/ai"),
                ("Anthropic: AI Safety and Research", "https://www.anthropic.com"),
                ("Perplexity AI: AI-Powered Search Engine", "https://www.perplexity.ai"),
                ("Hugging Face: Open AI Community", "https://huggingface.co"),
                ("DeepMind: Pioneering AI Research", "https://deepmind.com"),
                ("IBM Watson: AI for Business", "https://www.ibm.com/watson"),
                ("Microsoft AI Platform", "https://www.microsoft.com/ai"),
                ("NVIDIA AI: Accelerated Computing", "https://www.nvidia.com/en-us/artificial-intelligence"),
                ("OpenAI: Leading AI Research and Deployment", "https://openai.com"),
                ("Stability AI: Open-Source AI Models", "https://stability.ai"),
                ("Character.AI: Conversational AI Chatbots", "https://beta.character.ai"),
                ("DataRobot: Enterprise AI Platform", "https://www.datarobot.com"),
                ("Cohere: Natural Language Processing Solutions", "https://cohere.com"),
                ("Anthropic: AI Safety and Research", "https://www.anthropic.com"),
                ("You.com: AI-Powered Search Engine", "https://you.com"),
                ("INDIAai: National AI Portal of India", "https://indiaai.gov.in"),
                ("Reverse Image Search", "https://tineye.com/")
            };

            // Add each submenu item with click event
            foreach (var (name, url) in menuItems)
            {
                ToolStripMenuItem subMenuItem = new ToolStripMenuItem(name);
                subMenuItem.Click += (sender, e) => OpenUrlInChromeOrDefault(url);
                aiSitesMenu.DropDownItems.Add(subMenuItem);
            }

            // Add the dropdown menu to menuStrip1
            toolStrip1.Items.Add(aiSitesMenu);
        }

        private void AddMerchantAccountsMenu()
        {
            // Create the main dropdown menu item
            ToolStripMenuItem merchantAccountsMenu = new ToolStripMenuItem("Merchants");
            merchantAccountsMenu.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Define the menu items and their URLs
            var menuItems = new (string Name, string Url)[]
            {
                ("Swipesum: Payment Processing Consultants", "https://www.swipesum.com/"),
                ("Square: Merchant Services and Credit Card Processing", "https://squareup.com/us/en/payments/merchant-services"),
                ("North: Payment Processing and Merchant Services", "https://www.north.com/"),
                ("Authorize.net: Merchant Account Providers Directory", "https://www.authorize.net/sign-up/reseller-directory/merchant-account.html"),
                ("Stripe: Payment Processing Platform", "https://stripe.com/"),
                ("Worldpay: Global Payment Processing", "https://www.worldpay.com/"),
                ("TSYS: Payment Solutions", "https://www.tsys.com/"),
                ("Global Payments: Payment Technology Services", "https://www.globalpayments.com/"),
                ("Payway: Merchant Accounts vs. Payment Service Providers", "https://www.payway.com/resources/merchant-accounts-vs-payment-service-providers-which-should-you-choose"),
                ("Stax Payments: Understanding Merchant Services Providers", "https://staxpayments.com/blog/merchant-services-provider/"),
                ("Payline Data: Flexible Payment Processing Solutions", "https://www.paylinedata.com/"),
                ("Helcim: Transparent Credit Card Processing", "https://www.helcim.com/"),
                ("Dharma Merchant Services: Ethical Payment Processing", "https://www.dharmamerchantservices.com/"),
                ("First Data: Global Payment Solutions", "https://www.firstdata.com/"),
                ("Cayan: Payment Technology Solutions", "https://cayan.com/"),
                ("Fattmerchant: Subscription-Based Merchant Services", "https://fattmerchant.com/"),
                ("National Processing: Affordable Payment Processing", "https://nationalprocessing.com/"),
                ("PaymentCloud: High-Risk Merchant Services", "https://paymentcloudinc.com/"),
                ("QuickBooks Payments: Integrated Payment Processing", "https://quickbooks.intuit.com/payments/"),
                ("PayPal: Online Payment Solutions", "https://www.paypal.com/")
            };

            // Add each submenu item with click event
            foreach (var (name, url) in menuItems)
            {
                ToolStripMenuItem subMenuItem = new ToolStripMenuItem(name);
                subMenuItem.Click += (sender, e) => OpenUrlInChromeOrDefault(url);
                merchantAccountsMenu.DropDownItems.Add(subMenuItem);
            }

            // Add the dropdown menu to menuStrip1
            toolStrip1.Items.Add(merchantAccountsMenu);
        }

        private void AddYouTubeFinanceMenu()
        {
            // Create the main dropdown menu item
            ToolStripMenuItem youtubeFinanceMenu = new ToolStripMenuItem("Finance");
            youtubeFinanceMenu.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Define the menu items and their URLs
            var menuItems = new (string Name, string Url)[]
            {
                ("Export Licenses for Software", "https://www.torrestradelaw.com/posts/Persistent-Errors-in-the-Export-Classification-of-Software-Products/352"),
                ("How To Buy Tax Lien Certificates In Michigan", "https://www.youtube.com/watch?v=TfaGbpLmC4M"),
                ("Tax Sales In Michigan - The Ultimate Beginner's Guide", "https://www.youtube.com/watch?v=GXLdnx_Wv2U"),
                ("How To Buy Tax Liens In Michigan? - CountyOffice.org", "https://www.youtube.com/watch?v=Dj3y0ImLIlg"),
                ("Michigan Property Tax Sales | Unlocking Real Estate Gems", "https://www.youtube.com/watch?v=cswLGVe2-dM"),
                ("Your Guide to Michigan Tax Deed Sales", "https://www.youtube.com/watch?v=JlWfLHnXziw"),
                ("Michigan Tax Deed Sales", "https://www.youtube.com/watch?v=1npUnbKEgnE"),
                ("Why Should You Invest In Michigan Tax Deed Auctions This Year?", "https://www.youtube.com/watch?v=dE_hfb7W7-U"),
                ("The Best Brokerages of 2024 (+ How to Choose the Right One)", "https://www.youtube.com/watch?v=KXlKme-6p6M"),
                ("What Stock Broker Should You Use in 2025? (Top 3 Recommendations)", "https://www.youtube.com/watch?v=59c4tjwxWDw"),
                ("Top Brokerages for 2024", "https://www.youtube.com/watch?v=-NwPSi7uHkc"),
                ("Best Online Stock Brokerage Tier List (The BEST Stock Brokers in 2023)", "https://www.youtube.com/watch?v=orP3mItV-tE"),
                ("The Best Stock Brokerages for Dividend Investing - My Top Picks!", "https://www.youtube.com/watch?v=ueidlL4kwWQ"),
                ("StockBrokers.com - YouTube Channel", "https://www.youtube.com/c/Stockbrokers"),
                ("How Do Stock Brokerages Make Money?", "https://www.youtube.com/watch?v=HlKXEEqpe78"),
                ("What Does A Stock Broker Do? A Beginner's Overview", "https://www.youtube.com/watch?v=I4qx7dVnePs"),
                ("Tax Sales In Michigan - The Ultimate Beginner's Guide", "https://www.youtube.com/watch?v=GXLdnx_Wv2U")
            };

            // Add each submenu item with click event
            foreach (var (name, url) in menuItems)
            {
                ToolStripMenuItem subMenuItem = new ToolStripMenuItem(name);
                subMenuItem.Click += (sender, e) => OpenUrlInChromeOrDefault(url);
                youtubeFinanceMenu.DropDownItems.Add(subMenuItem);
            }

            // Add the dropdown menu to toolStrip1
            toolStrip1.Items.Add(youtubeFinanceMenu);
        }

        private void AddYouTubeSerGioMenu()
        {
            // Create the main dropdown menu item
            ToolStripMenuItem youtubeFinanceMenu = new ToolStripMenuItem("Press");
            youtubeFinanceMenu.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            //©®™

            var menuItems = new (string Name, string Url)[]
            {
                ("People Magazine", "https://ainetprofit.com/Article"),
                ("Medical Breakthroughs", "https://mdhealthbuzz.com/"),
                ("MaxLifespan®", "https://maxlifespan.com/"),
                ("People Are Talking with Bill SerGio, The Infomercial King™", "https://www.youtube.com/watch?v=1Tv6XjeeVqM"),
                ("DomainGenie®", "https://domain-genie.com/"),
                ("Best FREE Software", "https://software-rus.com/"),
                ("GeminiGroup TV", "https://geminigrouptv.com/"),
                ("AI Accounting with AiNetStudio®", "https://ainetprofit.com/"),
                ("Clive James with Bill SerGio, The Infomercial King™", "https://www.youtube.com/watch?v=92eR46ZbMAg"),
                ("Bill SerGio on Kelly & Company", "https://www.youtube.com/watch?v=QrfOoDxoZDM"),
                ("Entropia Universe pays YOU REAL CASH for shooting monsters!", "https://www.youtube.com/watch?v=BUUYVTkHzU0&t=362s"),
                ("Bill SerGio, The Infomercial King™", "https://www.youtube.com/watch?v=5CYS0Ti9YC0&t=156s"),
                ("Malibu Diet®", "https://www.youtube.com/watch?v=j2vfP7U-LBI")
            };

            // Add each submenu item with click event
            foreach (var (name, url) in menuItems)
            {
                ToolStripMenuItem subMenuItem = new ToolStripMenuItem(name);
                subMenuItem.Click += (sender, e) => OpenUrlInChromeOrDefault(url);
                youtubeFinanceMenu.DropDownItems.Add(subMenuItem);
            }

            // Add the dropdown menu to toolStrip1
            toolStrip1.Items.Add(youtubeFinanceMenu);
        }

        private void AddTubesMenu()
        {
            // Create the main dropdown menu item
            ToolStripMenuItem tubesMenu = new ToolStripMenuItem("Tubes");
            tubesMenu.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Define the menu items and their URLs
            var menuItems = new (string Name, string Url)[]
            {
                ("YouTube", "https://www.youtube.com/watch?v=5CYS0Ti9YC0"),
                ("YoKu", "https://www.youku.com"),
                ("Vimeo", "https://www.vimeo.com"),
                ("Dailymotion", "https://www.dailymotion.com"),
                ("Twitch", "https://www.twitch.tv"),
                ("Metacafe", "https://www.metacafe.com"),
                ("Veoh", "https://www.veoh.com"),
                ("BitChute", "https://www.bitchute.com"),
                ("RuTube", "https://www.ruclip.com"),
                ("Brighteon", "https://www.brighteon.com"),
                ("TED", "https://www.ted.com"),
                ("Flickr (Video Section)", "https://www.flickr.com"),
                ("VideoMotion", "https://www.videomotion.com"),
                ("Viddler", "https://www.viddler.com"),
                ("DTube", "https://www.dtube.video"),
                ("Facebook Watch", "https://www.facebook.com/watch"),
                ("Instagram Reels", "https://www.instagram.com/reels"),
                ("TikTok", "https://www.tiktok.com"),
                ("Snapchat (Spotlight)", "https://www.snapchat.com"),
                ("Pinterest (Video Pins)", "https://www.pinterest.com"),
                ("LinkedIn Video", "https://www.linkedin.com/video"),
                ("Twitter Video", "https://www.twitter.com")
            };

            // Add each submenu item with click event
            foreach (var (name, url) in menuItems)
            {
                ToolStripMenuItem subMenuItem = new ToolStripMenuItem(name);
                subMenuItem.Click += (sender, e) => OpenUrlInChromeOrDefault(url);
                tubesMenu.DropDownItems.Add(subMenuItem);
            }

            // Add the dropdown menu to toolStrip1
            toolStrip1.Items.Add(tubesMenu);
        }

        #endregion ============== END EXTRA MENUS ==========================

        #region ============= BEGIN UTLITIES ===============================

        public void ApplyLightTheme(Control control)
        {
            foreach (Control child in control.Controls)
            {
                ApplyLightTheme(child);
            }

            //if (control is CustomControls.GradientPanel gp)
            //{
            //    gp.GradientTop = Color.LimeGreen;
            //    gp.GradientBottom = Color.SeaGreen;

            //    if (gp.Name == "pnlThemes")
            //    {
            //        gp.BackColor = Color.Transparent;
            //        gp.GradientTop = Color.Transparent;
            //        gp.GradientBottom = Color.Transparent;
            //    }

            //    foreach (Control inner in gp.Controls)
            //    {
            //        if (inner is Label lbl)
            //        {
            //            lbl.BackColor = Color.Transparent;
            //        }
            //    }
            //}
        }

        public void ApplyDarkTheme(Control control)
        {
            control.BackColor = Color.FromArgb(69, 69, 60);
            control.ForeColor = Color.White;

            foreach (Control child in control.Controls)
            {
                ApplyDarkTheme(child);
            }

            if (control is DataGridView dgv)
            {
                dgv.EnableHeadersVisualStyles = false;
                dgv.BackgroundColor = Color.FromArgb(69, 69, 69);
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(69, 69, 69);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.DefaultCellStyle.BackColor = Color.FromArgb(69, 69, 69);
                dgv.DefaultCellStyle.ForeColor = Color.White;
                dgv.DefaultCellStyle.SelectionBackColor = Color.DimGray;
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            }
            //else if (control is CustomControls.GradientPanel gp)
            //{
            //    gp.GradientTop = Color.LightGray;
            //    gp.GradientBottom = Color.Gray;

            //    if (gp.Name == "pnlThemes")
            //    {
            //        gp.BackColor = Color.Transparent;
            //        gp.GradientTop = Color.Transparent;
            //        gp.GradientBottom = Color.Transparent;
            //    }

            //    foreach (Control inner in gp.Controls)
            //    {
            //        if (inner is Label lbl)
            //        {
            //            lbl.BackColor = Color.Transparent;
            //        }

            //        if (inner is RoundButton btn)
            //        {
            //            btn.BackColor = Color.Black;
            //            btn.ForeColor = Color.White;

            //            btn.MouseEnter += (s, e) =>
            //            {
            //                btn.ForeColor = Color.Black;
            //            };

            //            btn.MouseLeave += (s, e) =>
            //            {
            //                btn.ForeColor = Color.White;
            //            };

            //            btn.MouseDown += (s, e) =>
            //            {
            //                btn.ForeColor = Color.Black;
            //            };

            //            btn.MouseUp += (s, e) =>
            //            {
            //                btn.ForeColor = Color.White;
            //            };

            //            btn.Click += (s, e) =>
            //            {
            //                btn.ForeColor = Color.Black;
            //            };
            //        }
            //    }


            //}


        }


        //private string ConvertMarkdownToHtml(string markdown)
        //{
        //    var pipeline = new MarkdownPipelineBuilder().Build();
        //    return Markdown.ToHtml(markdown, pipeline);
        //}

        private string SaveHtmlToTempFile(string htmlContent)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "tempMarkdownPage.html");
            File.WriteAllText(tempFilePath, htmlContent);
            return tempFilePath;
        }

        public static void OpenUrlInChromeOrDefault(string url)
        {
            string chromePath = GetChromePath();

            try
            {
                if (!string.IsNullOrEmpty(chromePath))
                {
                    // Open the URL in Chrome
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = chromePath,
                        Arguments = url,
                        UseShellExecute = true
                    });
                }
                else
                {
                    // Open the URL in the default browser
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening URL: {ex.Message}");
            }
        }

        private static string GetChromePath()
        {
            string[] registryKeys =
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe",
                @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"
            };

            foreach (string key in registryKeys)
            {
                using (RegistryKey? regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(key))
                {
                    if (regKey != null)
                    {
                        object? path = regKey.GetValue(null);
                        if (path != null)
                        {
                            return path.ToString()!;
                        }
                    }
                }
            }

            return string.Empty;
        }

        //public void DisableTabs(string nodisable)
        //{
        //    disabledTabs.Clear(); // Clear any previous settings
        //    foreach (TabPage tp in tabControlEx1.TabPages)
        //    {
        //        if (!tp.Text.Equals(nodisable, StringComparison.OrdinalIgnoreCase))
        //        {
        //            disabledTabs.Add(tp.Text); // Track disabled tab
        //            tp.Enabled = false;
        //        }
        //        else
        //        {
        //            tp.Enabled = true;
        //        }
        //    }
        //}

        //public void EnableTabs(string noenable)
        //{
        //    disabledTabs.Clear(); // Clear any previous settings
        //    foreach (TabPage tp in tabControlEx1.TabPages)
        //    {
        //        if (tp.Text.Equals(noenable, StringComparison.OrdinalIgnoreCase))
        //        {
        //            disabledTabs.Add(tp.Text); // Track disabled tab
        //            tp.Enabled = false;
        //        }
        //        else
        //        {
        //            tp.Enabled = true;
        //        }
        //    }
        //}


        #endregion ============= END UTLITIES ===============================



    } //public partial class WinGUIMain : Form

} //namespace AiNetStudio.WinGui.Forms





