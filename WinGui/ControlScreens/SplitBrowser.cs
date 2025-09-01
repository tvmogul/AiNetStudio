using AiNetStudio.DataAccess;
using AiNetStudio.WinGui.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AiNetStudio.WinGui.ControlScreens
{
    public partial class SplitBrowser : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WinGUIMain? MainFormReference { get; set; }

        MultiTabBrowser mtb = new MultiTabBrowser();

        public SplitBrowser() : this(null!) { }

        public SplitBrowser(WinGUIMain mainForm)
        {
            InitializeComponent();

            //SplitContainer.Panel2
            mtb.Dock = DockStyle.Fill;
            this.splitContainer1.Panel2.Controls.Add(mtb);

            // hook mtb toggle to control splitter position
            HookMtbToggle();

            // subscribe to mtb ToggleStateChanged to control splitter position
            mtb.ToggleStateChanged += (s, tooltip) =>
            {
                try
                {
                    if (tooltip == "▲")
                    {
                        // restore and move to middle
                        this.splitContainer1.Panel1Collapsed = false;
                        int mid = Math.Max(0, this.splitContainer1.Height / 2);
                        this.splitContainer1.SplitterDistance = mid;
                    }
                    else if (tooltip == "▼")
                    {
                        // collapse Panel1 completely to remove any top gap
                        this.splitContainer1.Panel1Collapsed = true;
                    }
                }
                catch
                {
                }
            };

            MainFormReference = mainForm;

            // Ensure the embedded MultiTabBrowser can call back into the main form for event: URLs
            mtb.MainFormReference = mainForm;

            this.Load += SplitBrowser_Load;
            //cbCategory.SelectedIndexChanged += CbCategory_SelectedIndexChanged;
            //btnLoad.Click += BtnLoad_Click;

        }

        private void SplitBrowser_Load(object? sender, EventArgs e)
        {
            // Ensure DB and load categories
            //Tubes.EnsureDatabase(seedIfEmpty: true);
            //var cats = Tubes.GetCategories();
            //cbCategory.Items.Clear();
            //foreach (var c in cats) cbCategory.Items.Add(c);
            //if (cbCategory.Items.Count > 0) cbCategory.SelectedIndex = 0;

            // Initialize WebView2
            //await EnsureWebReady();
            //_web.CoreWebView2.NavigateToString("<html><body style='font-family:sans-serif;color:#666;padding:16px'>Select a category and click <b>Load</b>.</body></html>");
        }

        //private void CbCategory_SelectedIndexChanged(object? sender, EventArgs e)
        //{
        //    var cat = cbCategory.SelectedItem?.ToString() ?? "";
        //    var subs = Tubes.GetSubcategories(cat);

        //    cbSubCategory.Items.Clear();
        //    cbSubCategory.Items.Add(""); // allow 'no subcategory' option
        //    foreach (var s in subs) cbSubCategory.Items.Add(s);
        //    cbSubCategory.SelectedIndex = 0;
        //}

        //private void BtnLoad_Click(object? sender, EventArgs e)
        //{
        //    var category = cbCategory.SelectedItem?.ToString() ?? "";
        //    var subcategory = cbSubCategory.SelectedItem?.ToString();
        //    if (subcategory != null && subcategory.Trim().Length == 0) subcategory = null;

        //    var feeds = Tubes.GetFeedsByCategory(category, subcategory, take: 1000, skip: 0);
        //    var html = Tubes.BuildFeedsHtml(category, subcategory, feeds);

        //    // Write HTML to a local file and navigate MultiTabBrowser to it (no async/await)
        //    try
        //    {
        //        var pm = new PathManager();
        //        var folder = pm.GetWritableFolder("RSSFeeds");
        //        var htmlPath = Path.Combine(folder, "feeds_list.html");
        //        File.WriteAllText(htmlPath, html, Encoding.UTF8);

        //        // Navigate synchronously by blocking on the Task (still no async/await keywords used)
        //        var fileUrl = new Uri(htmlPath).AbsoluteUri;
        //        var navTask = mtb.NavigateAsync(fileUrl);
        //        navTask.GetAwaiter().GetResult();
        //    }
        //    catch
        //    {
        //        // swallow navigation/file errors to avoid crashing UI
        //    }
        //}

        private void HookMtbToggle()
        {
            // Try to locate a ToolStripItem named "_btnToggle" inside mtb and hook its Click
            ToolStripButton? toggleBtn = FindToggleButton(mtb, "_btnToggle");
            if (toggleBtn != null)
            {
                // Run after mtb's own click handler so we react to the updated ToolTipText
                toggleBtn.Click += (s, e) =>
                {
                    if (this.IsHandleCreated)
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            try
                            {
                                if (toggleBtn.ToolTipText == "▲")
                                {
                                    // restore and move to middle
                                    this.splitContainer1.Panel1Collapsed = false;
                                    int mid = Math.Max(0, this.splitContainer1.Height / 2);
                                    // SplitterDistance is measured from the top to the splitter
                                    this.splitContainer1.SplitterDistance = mid;
                                }
                                else if (toggleBtn.ToolTipText == "▼")
                                {
                                    // collapse Panel1 completely to remove any top gap
                                    this.splitContainer1.Panel1Collapsed = true;
                                }
                            }
                            catch
                            {
                                // ignore any layout exceptions
                            }
                        }));
                    }
                };
            }
        }

        private ToolStripButton? FindToggleButton(Control root, string name)
        {
            // Search all ToolStrips within the control tree for a ToolStripButton with the given name
            foreach (Control child in root.Controls)
            {
                if (child is ToolStrip ts)
                {
                    foreach (ToolStripItem item in ts.Items)
                    {
                        if (item is ToolStripButton btn && string.Equals(btn.Name, name, StringComparison.Ordinal))
                            return btn;
                    }
                }

                var found = FindToggleButton(child, name);
                if (found != null) return found;
            }
            return null;
        }

    }
}