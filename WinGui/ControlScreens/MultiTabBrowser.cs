// WinGui/ControlScreens/MultiTabBrowser.cs
using AiNetStudio.DataAccess;
using AiNetStudio.WinGui.Controls;
using AiNetStudio.WinGui.Forms;
using CustomControls;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AiNetStudio.WinGui.ControlScreens
{
    public partial class MultiTabBrowser : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WinGUIMain? MainFormReference { get; set; }

        // 👇 ADD THIS LINE AT THE TOP OF YOUR CLASS
        public event EventHandler<string>? ToggleStateChanged;

        private readonly ToolStrip _toolbar;
        private readonly ToolStripButton _btnBack;
        private readonly ToolStripButton _btnForward;
        private readonly ToolStripButton _btnRefresh;
        private readonly ToolStripButton _btnHome;
        private readonly ToolStripSeparator _sep1;
        private readonly ChromeToolStripComboBox _addressBox;
        private readonly ToolStripButton _btnGo;
        //private readonly ToolStripButton _btnAI;
        private readonly ToolStripDropDownButton _btnAI;
        private readonly ToolStripButton _btnLoad;
        private readonly ChromeToolStripComboBox _category;
        private readonly ChromeToolStripComboBox _subCategory;

        private readonly ToolStripSeparator _sep2;
        private readonly ToolStripButton _btnNewTab;
        private readonly ToolStripButton _btnCloseTab;
        private readonly ToolStripButton _btnToggle;

        private readonly ToolStripButton _btnRight;

        private readonly TabControlEx _tabs;

        private readonly Dictionary<TabPage, WebView2> _webByTab = new();

        private static bool IsInDesignMode()
        {
            try
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
                var p = Process.GetCurrentProcess();
                return p != null && string.Equals(p.ProcessName, "devenv", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public MultiTabBrowser() : this(null!) { }

        public MultiTabBrowser(WinGUIMain mainForm)
        {
            MainFormReference = mainForm;

            InitializeComponent(); // <- required for designer

            //← → ⟳ 🏠 AI🔍 ＋ ✕ ▲▼
            // Top toolbar

            // NEW: Initialize a safe, empty ImageList (no external resources needed)
            // Ensure ImageList is not null even if Designer resources were removed
            if (imgListBlack == null)
            {
                imgListBlack = new ImageList
                {
                    ColorDepth = ColorDepth.Depth32Bit,
                    ImageSize = new Size(24, 24)
                };
            }
            //if (imgListOrange == null)
            //{
            //    imgListOrange = new ImageList
            //    {
            //        ColorDepth = ColorDepth.Depth32Bit,
            //        ImageSize = new Size(24, 24)
            //    };
            //}

            _toolbar = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                ImageScalingSize = new Size(24, 24),
                Stretch = true
            };

            _toolbar.ImageList = imgListBlack;

            _toolbar.Font = new Font("Segoe UI Emoji", 16, FontStyle.Regular);

            //_btnBack = new ToolStripButton("←") { ToolTipText = "Back (Alt+Left)" };
            _btnBack = new ToolStripButton
            {
                ToolTipText = "Back (Alt+Left)",
                //Image = Properties.Resources.tbx_back,
                ImageKey = "back",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(2, 0, 2, 0)
            };

            //_btnForward = new ToolStripButton("→") { ToolTipText = "Forward (Alt+Right)" };
            _btnForward = new ToolStripButton
            {
                ToolTipText = "Forward (Alt+Right)",
                //Image = Properties.Resources.tbx_forward,
                ImageKey = "forward",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(2, 0, 2, 0)
            };

            //_btnRefresh = new ToolStripButton("⟳") { ToolTipText = "Reload (F5)" };
            _btnRefresh = new ToolStripButton
            {
                ToolTipText = "Reload (F5)",
                //Image = Properties.Resources.tbx_reload,
                ImageKey = "reload",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(2, 0, 2, 0)
            };

            //_btnHome = new ToolStripButton("🏠") { ToolTipText = "Home" };
            _btnHome = new ToolStripButton
            {
                ToolTipText = "Home",
                //Image = Properties.Resources.tbx_home,
                ImageKey = "home",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(2, 0, 2, 0)
            };

            _sep1 = new ToolStripSeparator();

            // Replaced address text box with a styled combo box (dropdown) for URL entry/history
            _addressBox = new ChromeToolStripComboBox
            {
                AutoSize = false,
                Width = 240,
                ToolTipText = "Enter or choose URL",
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(2, 0, 2, 0)
            };

            // Default URLs
            _addressBox.Items.AddRange(new object[]
            {
                "https://ainetstudio.com",
                "https://ainetprofit.com",
                "https://aiantigravity.com",
                "https://geminigrouptv.com",
                "https://station-break.com",
                "https://swipeclouds.com",
                "https://www.youtube.com/AlienScientist",
                "https://www.altpropulsion.com/",
                "https://www.youtube.com/TheoriesofEverything"
            });

            _btnGo = new ToolStripButton
            {
                ToolTipText = "Navigate (Enter)",
                //Image = Properties.Resources.tbx_go,
                ImageKey = "go",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(2, 0, 2, 0)
            };

            _btnAI = new ToolStripDropDownButton
            {
                ToolTipText = "AI Search",
                //Image = Properties.Resources.tbx_ai,
                ImageKey = "ai",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(2, 0, 2, 0)
            };
            //_btnAI.DropDown.Font = new Font("Segoe UI", 9f);
            //_btnAI.DropDownItems.Add("Google AI", null, (s, e) => OpenUrlInChromeOrDefault("https://www.google.com/search?q=&sourceid=chrome&ie=UTF-8&udm=50&aep=48&cud=0&qsubts=1756076983174"));
            //_btnAI.DropDownItems.Add("OpenAI", null, (s, e) => OpenUrlInChromeOrDefault("https://openai.com"));
            //_btnAI.DropDownItems.Add("Anthropic", null, (s, e) => OpenUrlInChromeOrDefault("https://www.anthropic.com"));
            //_btnAI.DropDownItems.Add("Hugging Face", null, (s, e) => OpenUrlInChromeOrDefault("https://huggingface.co"));
            //_btnAI.DropDownItems.Add("Stability AI", null, (s, e) => OpenUrlInChromeOrDefault("https://stability.ai"));
            //_btnAI.DropDownItems.Add("DeepMind", null, (s, e) => OpenUrlInChromeOrDefault("https://deepmind.com"));

            // define the list of name/url pairs
            (string Name, string Url)[] aiSites = new[]
            {
                ("Google AI", "https://www.google.com/search?q=&sourceid=chrome&ie=UTF-8&udm=50&aep=48&cud=0&qsubts=1756076983174"),
                ("AiNetProfit", "https://ainetprofit.com"),
                ("AlienScientist", "https://www.youtube.com/AlienScientist"),
                ("altpropulsion", "https://www.altpropulsion.com/"),
                ("PlasmaChannel", "https://www.youtube.com/PlasmaChannel"),
                ("TheoriesofEverything", "https://www.youtube.com/TheoriesofEverything")
            };

            //https://www.youtube.com/PlasmaChannel
            //("Old Man Builds", "https://www.youtube.com/channel/UCEF-4uYXyU1eG_W2RB0b7vQ"),

            // loop to generate menu items
            foreach (var site in aiSites)
            {
                _btnAI.DropDownItems.Add(site.Name, null, async (s, e) =>
                {
                    _addressBox.Text = site.Url;
                    await NavigateAsync(_addressBox.Text);
                });
            }

            _btnAI.DropDown.Font = new Font("Segoe UI", 9f);

            _sep2 = new ToolStripSeparator();

            //_btnNewTab = new ToolStripButton("＋") { ToolTipText = "New Tab (Ctrl+T)" };
            _btnNewTab = new ToolStripButton
            {
                ToolTipText = "New Tab (Ctrl+T)",
                //Image = Properties.Resources.tbx_newtab,
                ImageKey = "add",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(2, 0, 2, 0)
            };

            _btnCloseTab = new ToolStripButton
            {
                ToolTipText = "Close Tab (Ctrl+W)",
                //Image = Properties.Resources.tbx_removetab,
                ImageKey = "close",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(2, 0, 2, 0)
            };


            //private readonly ToolStripButton _btnLoad;
            //_btnLoad = new ToolStripButton
            //{
            //    ToolTipText = "Load",
            //    //Image = Properties.Resources.tbx_removetab,
            //    ImageKey = "tv",
            //    DisplayStyle = ToolStripItemDisplayStyle.Image,
            //    Margin = new Padding(2, 0, 2, 0)
            //};

            //private readonly ChromeToolStripComboBox _category;
            _category = new ChromeToolStripComboBox
            {
                AutoSize = false,
                Width = 140,
                ToolTipText = "Category",
                DropDownStyle = ComboBoxStyle.DropDownList,  // <-- makes it read-only
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(2, 0, 2, 0)
            };

            // Prevent text highlighting with mouse
            _category.ComboBox.MouseDown += (s, e) =>
            {
                ((ComboBox)s!).SelectionLength = 0;
            };

            Tubes.EnsureDatabase(seedIfEmpty: true);
            var cats = Tubes.GetCategories();
            _category.Items.Clear();
            foreach (var c in cats) _category.Items.Add(c);
            if (_category.Items.Count > 0) _category.SelectedIndex = 0;

            //private readonly ChromeToolStripComboBox _subCategory;
            _subCategory = new ChromeToolStripComboBox
            {
                AutoSize = false,
                Width = 140,
                ToolTipText = "SubCategory",
                DropDownStyle = ComboBoxStyle.DropDownList,  // <-- makes it read-only
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(2, 0, 2, 0)
            };

            // Prevent text highlighting with mouse
            _subCategory.ComboBox.MouseDown += (s, e) =>
            {
                ((ComboBox)s!).SelectionLength = 0;
            };

            // Prevent selecting text with keyboard
            _subCategory.ComboBox.KeyDown += (s, e) =>
            {
                e.SuppressKeyPress = true;
            };
            _subCategory.SelectedIndexChanged += async (_, __) =>
            {
                var category = _category.SelectedItem?.ToString() ?? "";
                var subcategory = _subCategory.SelectedItem?.ToString();
                if (subcategory != null && subcategory.Trim().Length == 0) subcategory = null;

                var feeds = Tubes.GetFeedsByCategory(category, subcategory, take: 1000, skip: 0);
                var html = Tubes.BuildFeedsHtml(category, subcategory, feeds);

                // Write HTML to a local file and navigate MultiTabBrowser to it (no async/await)
                try
                {
                    var pm = new PathManager();
                    var folder = pm.GetWritableFolder("RSSFeeds");
                    var htmlPath = Path.Combine(folder, "feeds_list.html");
                    File.WriteAllText(htmlPath, html, Encoding.UTF8);

                    // Navigate synchronously by blocking on the Task (still no async/await keywords used)
                    var fileUrl = new Uri(htmlPath).AbsoluteUri;
                    //var navTask = mtb.NavigateAsync(fileUrl);
                    await NavigateAsync(fileUrl);
                    //navTask.GetAwaiter().GetResult();
                }
                catch
                {
                    // swallow navigation/file errors to avoid crashing UI
                }
            };

            // Prevent selecting text with keyboard
            _category.ComboBox.KeyDown += (s, e) =>
            {
                e.SuppressKeyPress = true;
                //if (e.KeyCode == Keys.Enter)
                //{
                //    e.Handled = true;
                //    e.SuppressKeyPress = true;
                //    await NavigateAsync(_addressBox.Text);
                //}
            };
            _category.SelectedIndexChanged += async (_, __) =>
            {
                var cat = _category.SelectedItem?.ToString() ?? "";
                var subs = Tubes.GetSubcategories(cat);

                _subCategory!.Items.Clear();
                _subCategory.Items.Add(""); // allow 'no subcategory' option
                foreach (var s in subs) _subCategory.Items.Add(s);
                _subCategory.SelectedIndex = 0;
            };









            _btnToggle = new ToolStripButton
            {
                ToolTipText = "▲",
                //Image = Properties.Resources.tbx_up,
                ImageKey = "up",
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };

            // ///////////////////////////////////////////////////////

            //_btnRight = new ToolStripButton
            //{
            //    ToolTipText = "Settings",
            //    Image = Properties.Resources.tbx_youtube,
            //    DisplayStyle = ToolStripItemDisplayStyle.Image,
            //    Alignment = ToolStripItemAlignment.Right
            //};

            var _btnColor = new ToolStripDropDownButton("Color")
            {
                Alignment = ToolStripItemAlignment.Right,
                AutoSize = false,
                Width = 60,  // make it tighter
                TextImageRelation = TextImageRelation.TextBeforeImage,
                Padding = new Padding(2),
                Font = new Font("Segoe UI", 8f, FontStyle.Regular)
            };

            // Add menu items
            _btnColor.DropDownItems.Add("Black", null, (s, e) =>
            {
                // switch to black images
                _toolbar.ImageList = imgListBlack;
            });

            _btnColor.DropDownItems.Add("Orange", null, (s, e) =>
            {
                // switch to orange images
                _toolbar.ImageList = imgListOrange;
            });

            // Add the button to your ToolStrip
            _toolbar.Items.Add(_btnColor);

            // //////////////////////////////////////////////////////


            //_toolbar.Font = new Font("Segoe UI Emoji", 24, FontStyle.Regular);

            _toolbar.Items.AddRange(new ToolStripItem[] {
                _btnBack,
                _btnForward,
                _btnRefresh,
                _btnHome,
                _sep1,
                _addressBox,
                _btnGo,
                _btnAI,
                _sep2,
                _btnNewTab,
                _btnCloseTab,
                _category,
                _subCategory,
                _btnToggle });

            //_toolbar.Items.Add(_btnRight);

            // Tab control
            _tabs = new TabControlEx
            {
                Dock = DockStyle.Fill
            };

            _tabs.DisplayStyle = TabStyle.Chrome;

            _tabs.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            _tabs.DisplayStyleProvider.FocusTrack = false;
            _tabs.DisplayStyleProvider.HotTrack = true;
            _tabs.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            _tabs.DisplayStyleProvider.Opacity = 1F;
            _tabs.DisplayStyleProvider.Overlap = 7;
            _tabs.DisplayStyleProvider.Padding = new System.Drawing.Point(14, 1);
            _tabs.DisplayStyleProvider.ShowTabCloser = false;
            _tabs.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            _tabs.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            _tabs.HotTrack = true;
            //tabControlEx1.ImageList = this.imageList1;
            _tabs.Location = new System.Drawing.Point(12, 44);
            _tabs.Name = "tabControlEx1";
            _tabs.SelectedIndex = 0;
            _tabs.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            SuspendLayout();
            Controls.Add(_tabs);
            Controls.Add(_toolbar);
            _toolbar.Dock = DockStyle.Top;
            ResumeLayout();

            // Wire toolbar
            _btnBack.Click += (_, __) => SelectedWebView()?.CoreWebView2?.GoBack();
            _btnForward.Click += (_, __) => SelectedWebView()?.CoreWebView2?.GoForward();
            _btnRefresh.Click += (_, __) => SelectedWebView()?.CoreWebView2?.Reload();
            _btnHome.Click += async (_, __) => await NavigateAsync(HomeUrl);
            _btnGo.Click += async (_, __) => await NavigateAsync(_addressBox.Text);


            _btnToggle.Click += (s, e) =>
            {
                if (_btnToggle.ToolTipText == "▲")
                {
                    _btnToggle.ToolTipText = "▼";
                    //_btnToggle.Image = Properties.Resources.tbx_down;
                    _btnToggle.ImageKey = "down";
                }
                else
                {
                    _btnToggle.ToolTipText = "▲";
                    //_btnToggle.Image = Properties.Resources.tbx_up;
                    _btnToggle.ImageKey = "up";
                }
                // Raise event after updating tooltip
                ToggleStateChanged?.Invoke(this, _btnToggle.ToolTipText);
            };

            _addressBox.KeyDown += async (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    await NavigateAsync(_addressBox.Text);
                }
            };
            _addressBox.SelectedIndexChanged += async (_, __) =>
            {
                // Navigate when a preset/history item is chosen
                var selected = _addressBox.SelectedItem?.ToString();
                if (!string.IsNullOrWhiteSpace(selected))
                {
                    await NavigateAsync(selected);
                }
            };

            _btnNewTab.Click += (_, __) => _ = CreateNewTab();
            _btnCloseTab.Click += (_, __) => CloseCurrentTab();

            // Wire tabs
            _tabs.SelectedIndexChanged += (_, __) => SyncUiToSelectedTab();
            _tabs.DrawItem += Tabs_DrawItem;
            _tabs.MouseDown += Tabs_MouseDown;

            this.KeyDown += MultiTabBrowser_KeyDown;
            this.TabStop = true;

            // Start with one tab
            if (!IsInDesignMode())
            {
                _ = CreateNewTab(HomeUrl);
            }
            else
            {
                _tabs.TabPages.Add(new TabPage("Design Preview"));
            }
        }

        public void OnTabActivated()
        {
            Tubes.EnsureDatabase(seedIfEmpty: true);
            var cats = Tubes.GetCategories();
            _category.Items.Clear();
            foreach (var c in cats) _category.Items.Add(c);
            //if (_category.Items.Count > 0) _category.SelectedIndex = 0;
        }

        // ADD: handles event:action:value URLs
        private void HandleEventUrl(string url)
        {
            // url format: event:action:value
            // strip "event:"
            var payload = url.Length >= 6 ? url.Substring(6) : string.Empty;

            // split once on ':'
            string action, value;
            int idx = payload.IndexOf(':');
            if (idx >= 0)
            {
                action = payload.Substring(0, idx);
                value = payload.Substring(idx + 1);
            }
            else
            {
                action = payload;
                value = string.Empty;
            }

            // Call into your app (pass "action:value" as requested)
            try { MainFormReference?.HandleEventFromBrowser($"{action}:{value}"); } catch { }
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string HomeUrl { get; set; } = "https://ainetstudio.com";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double DefaultZoomFactor { get; set; } = .80; // 1.00 = 100%

        public Task CreateNewTab(string? url = null, bool switchTo = true) => CreateTabInternalAsync(url, switchTo);

        public void CloseCurrentTab()
        {
            if (_tabs.TabPages.Count == 0) return;

            var page = _tabs.SelectedTab!;
            if (_webByTab.TryGetValue(page, out var web))
            {
                try
                {
                    web.CoreWebView2?.Stop();
                    web.Dispose();
                }
                catch { }
                _webByTab.Remove(page);
            }

            _tabs.TabPages.Remove(page);

            if (_tabs.TabPages.Count == 0)
            {
                _ = CreateNewTab(HomeUrl);
            }
        }

        public async Task NavigateAsync(string? rawUrl)
        {
            if (string.IsNullOrWhiteSpace(rawUrl)) return;

            var url = rawUrl.Trim();

            // ADD: catch event:action:value typed into the address box
            if (url.StartsWith("event:", StringComparison.OrdinalIgnoreCase))
            {
                HandleEventUrl(url);
                return;
            }

            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
            {
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    // leave as-is
                }
                else if (File.Exists(url))
                {
                    url = new Uri(Path.GetFullPath(url)).AbsoluteUri;
                }
                else
                {
                    url = "https://" + url;
                }
            }

            var web = SelectedWebView();
            if (web?.CoreWebView2 != null)
            {
                web.CoreWebView2.Navigate(url);
            }
            else if (web != null)
            {
                await web.EnsureCoreWebView2Async();
                web.CoreWebView2.Navigate(url);
            }

            // Keep a simple history in the dropdown (dedup, cap at 50)
            AddToUrlHistory(url);
        }

        private async Task CreateTabInternalAsync(string? url, bool switchTo)
        {
            var page = new TabPage("New Tab");
            var web = new WebView2 { Dock = DockStyle.Fill };

            page.Controls.Add(web);
            _webByTab[page] = web;
            _tabs.TabPages.Add(page);
            if (switchTo) _tabs.SelectedTab = page;

            await web.EnsureCoreWebView2Async();

            // Set initial zoom for the new tab
            web.ZoomFactor = DefaultZoomFactor;

            web.CoreWebView2.NewWindowRequested += (s, e) =>
            {
                var targetUrl = e.Uri ?? string.Empty;

                // Handle target="_blank" for event: URLs
                if (targetUrl.StartsWith("event:", StringComparison.OrdinalIgnoreCase))
                {
                    e.Handled = true;
                    HandleEventUrl(targetUrl);
                    return;
                }

                e.Handled = true;
                _ = CreateNewTab(targetUrl);
            };

            // EQUIBALENT FOR OnBeforeNavigate!!!
            web.CoreWebView2.NavigationStarting += (s, e) =>
            {
                var uri = e.Uri ?? string.Empty;

                // ADD: catch event:action:value clicked from page content
                if (uri.StartsWith("event:", StringComparison.OrdinalIgnoreCase))
                {
                    e.Cancel = true;
                    HandleEventUrl(uri);
                    return;
                }

                _addressBox.Text = uri;

                if (!string.IsNullOrEmpty(uri) && uri.Contains("zebra_", StringComparison.OrdinalIgnoreCase))
                {
                    e.Cancel = true;
                    MessageBox.Show("Blocked URL: " + uri, "Navigation Blocked",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };


            web.CoreWebView2.SourceChanged += (s, e) =>
            {
                try { _addressBox.Text = web.Source?.ToString() ?? string.Empty; } catch { }
            };

            web.CoreWebView2.DocumentTitleChanged += (s, e) =>
            {
                try
                {
                    var title = web.CoreWebView2.DocumentTitle;
                    page.Text = string.IsNullOrWhiteSpace(title) ? "New Tab" : TrimToWidth(title, _tabs, page);
                    _tabs.Invalidate();
                }
                catch { page.Text = "New Tab"; }
            };

            web.NavigationCompleted += (_, __) => SyncBackForwardUi();

            if (!string.IsNullOrWhiteSpace(url))
            {
                await NavigateAsync(url);
            }
            else
            {
                web.CoreWebView2.Navigate("about:blank");
            }

        }

        private WebView2? SelectedWebView()
        {
            var page = _tabs.SelectedTab;
            if (page != null && _webByTab.TryGetValue(page, out var web)) return web;
            return null;
        }

        private void SyncUiToSelectedTab()
        {
            var web = SelectedWebView();
            _addressBox.Text = web?.Source?.ToString() ?? string.Empty;
            SyncBackForwardUi();
        }

        private void SyncBackForwardUi()
        {
            if (IsHandleCreated && InvokeRequired)
            {
                BeginInvoke((Action)SyncBackForwardUi);
                return;
            }

            var w = SelectedWebView();
            var c = w?.CoreWebView2;
            _btnBack.Enabled = c?.CanGoBack ?? false;
            _btnForward.Enabled = c?.CanGoForward ?? false;
        }

        private void MultiTabBrowser_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.T)
            {
                e.SuppressKeyPress = true;
                _ = CreateNewTab(HomeUrl);
            }
            else if (e.Control && e.KeyCode == Keys.W)
            {
                e.SuppressKeyPress = true;
                CloseCurrentTab();
            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                e.SuppressKeyPress = true;
                _addressBox.Focus();
                _addressBox.ComboBox.SelectAll();
            }
            else if (e.KeyCode == Keys.F5)
            {
                e.SuppressKeyPress = true;
                SelectedWebView()?.CoreWebView2?.Reload();
            }
            else if (e.Alt && e.KeyCode == Keys.Left)
            {
                e.SuppressKeyPress = true;
                SelectedWebView()?.CoreWebView2?.GoBack();
            }
            else if (e.Alt && e.KeyCode == Keys.Right)
            {
                e.SuppressKeyPress = true;
                SelectedWebView()?.CoreWebView2?.GoForward();
            }
            else if (e.Control && (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add))
            {
                e.SuppressKeyPress = true;
                ZoomIn();
            }
            else if (e.Control && (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract))
            {
                e.SuppressKeyPress = true;
                ZoomOut();
            }
            else if (e.Control && (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0))
            {
                e.SuppressKeyPress = true;
                ResetZoom();
            }
        }

        private void Tabs_DrawItem(object? sender, DrawItemEventArgs e)
        {
            var page = _tabs.TabPages[e.Index];
            var rect = e.Bounds;
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            using var back = new SolidBrush(isSelected ? Color.FromArgb(245, 245, 245) : Color.FromArgb(225, 225, 225));
            using var fore = new SolidBrush(Color.Black);
            using var border = new Pen(Color.FromArgb(200, 200, 200));

            e.Graphics.FillRectangle(back, rect);
            e.Graphics.DrawRectangle(border, rect);

            var textRect = new Rectangle(rect.X + 8, rect.Y + 6, rect.Width - 28, rect.Height - 12);
            var txt = page.Text;
            TextRenderer.DrawText(e.Graphics, txt, Font, textRect, fore.Color, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            var closeRect = GetCloseGlyphRect(rect);
            ControlPaint.DrawCaptionButton(e.Graphics, closeRect, CaptionButton.Close, ButtonState.Flat);
        }

        private void Tabs_MouseDown(object? sender, MouseEventArgs e)
        {
            for (int i = 0; i < _tabs.TabCount; i++)
            {
                var rect = _tabs.GetTabRect(i);
                var closeRect = GetCloseGlyphRect(rect);
                if (closeRect.Contains(e.Location))
                {
                    _tabs.SelectedIndex = i;
                    CloseCurrentTab();
                    return;
                }
            }
        }

        private static Rectangle GetCloseGlyphRect(Rectangle tabRect)
        {
            var size = 16;
            var padding = 6;
            return new Rectangle(tabRect.Right - size - padding, tabRect.Y + (tabRect.Height - size) / 2, size, size);
        }

        private static string TrimToWidth(string text, Control measureOn, TabPage page)
        {
            using var g = measureOn.CreateGraphics();
            var width = g.MeasureString(text, measureOn.Font).Width;
            var max = page.Bounds.Width - 32;
            if (width <= max || max <= 0) return text;
            var t = text;
            while (t.Length > 4 && width > max)
            {
                t = t[..^1];
                width = g.MeasureString(t + "…", measureOn.Font).Width;
            }
            return t + "…";
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;

            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Bottom - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void AddToUrlHistory(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            // Deduplicate (case-insensitive)
            for (int i = 0; i < _addressBox.Items.Count; i++)
            {
                if (string.Equals(_addressBox.Items[i]?.ToString(), url, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }
            _addressBox.Items.Insert(0, url);
            // Cap simple history size
            const int max = 50;
            while (_addressBox.Items.Count > max) _addressBox.Items.RemoveAt(_addressBox.Items.Count - 1);
        }

        private void SetZoom(double factor)
        {
            var w = SelectedWebView();
            if (w == null) return;
            w.ZoomFactor = Math.Max(0.25, Math.Min(3.0, factor));
        }

        private void ResetZoom() => SetZoom(DefaultZoomFactor);
        private void ZoomIn() => SetZoom((SelectedWebView()?.ZoomFactor ?? DefaultZoomFactor) + 0.10);
        private void ZoomOut() => SetZoom((SelectedWebView()?.ZoomFactor ?? DefaultZoomFactor) - 0.10);
    }

}


