using AiNetStudio.DataAccess;
using AiNetStudio.Models;
using AiNetStudio.WinGui.Controls;
using AiNetStudio.WinGui.Forms;
using CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YouTubeScrapper;
using static NumSharp.np;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static TorchSharp.torch.distributions.constraints;

namespace AiNetStudio.WinGui.ControlScreens
{
    public partial class SplitEditor : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WinGUIMain? MainFormReference { get; set; }

        private System.Windows.Forms.Button btnToggle;
        private bool _toggleDown = false;

        // Keep a small visible strip of Panel1/Panel2 
        // so the splitter (and button) are still visible.
        // Adjust this to taste.
        private const int SplitterReveal = 24;

        // Hard cap so toggle never exceeds splitter height requested (e.g., 20px)
        private const int MaxToggleHeight = 20;

        // ===== New: designer-tweakable properties =====
        private int _bottomModeReveal = 0; // height of Panel1 when showing Panel2 (arrow down)
        private int _topModeReveal = 0;    // height of Panel2 when showing Panel1 (arrow up)
        private int _toggleMaxHeight = 20; // cap for toggle height
        private int _toggleButtonWidth = 24;
        private bool _toggleOnSplitterDoubleClick = true;

        [Browsable(true), Category("SplitEditor"), Description("Height (px) of Panel1 kept visible when showing Panel2 (down arrow). 0 hides Panel1 completely (splitter at top)."), DefaultValue(0)]
        public int BottomModeReveal
        {
            get => _bottomModeReveal;
            set { _bottomModeReveal = Math.Max(0, value); ApplyRevealForCurrentMode(); RepositionButton(); }
        }

        [Browsable(true), Category("SplitEditor"), Description("Height (px) of Panel2 kept visible when showing Panel1 (up arrow). 0 hides Panel2 completely (splitter at bottom)."), DefaultValue(0)]
        public int TopModeReveal
        {
            get => _topModeReveal;
            set { _topModeReveal = Math.Max(0, value); ApplyRevealForCurrentMode(); RepositionButton(); }
        }

        [Browsable(true), Category("SplitEditor"), Description("Maximum height (px) of the toggle button; also clamped by SplitterWidth."), DefaultValue(20)]
        public int ToggleMaxHeight
        {
            get => _toggleMaxHeight;
            set { _toggleMaxHeight = Math.Max(1, value); SyncButtonSizeToSplitter(); SetToggleIcon(_toggleDown); RepositionButton(); }
        }

        [Browsable(true), Category("SplitEditor"), Description("Width (px) of the toggle button."), DefaultValue(24)]
        public int ToggleButtonWidth
        {
            get => _toggleButtonWidth;
            set { _toggleButtonWidth = Math.Max(8, value); SyncButtonSizeToSplitter(); SetToggleIcon(_toggleDown); RepositionButton(); }
        }

        [Browsable(true), Category("SplitEditor"), Description("If true, double-clicking the splitter toggles between top/bottom views."), DefaultValue(true)]
        public bool ToggleOnSplitterDoubleClick
        {
            get => _toggleOnSplitterDoubleClick;
            set => _toggleOnSplitterDoubleClick = value;
        }
        // ===== End designer-tweakable properties =====


        #region =================== BEGIN PANEL #2 ========================

        //private VideoDataDialog videoDataDialog;

        //private ProgressDialog progressDialog;
        //private int progressCount = 0;

        //private MainViewModel _viewModel;

        //private IXDListener listener;
        //private IXDBroadcast broadcast;

        //private static readonly string connRSSFeeds = ConfigurationManager.ConnectionStrings["RSSFeeds"].ConnectionString;
        private int _start = 0;
        private int _max = 3000;
        public List<RSSFeed> lstDisplay = new List<RSSFeed>();

        const int SPI_SETNONCLIENTMETRICS = 0x002A;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDCHANGE = 0x02;

        [DllImport("user32.dll")]
        public static extern int SystemParametersInfo(int uAction, int uParam, ref int lpvParam, int fuWinIni);

        #endregion ================ END PANEL #2 ==========================


        public SplitEditor() : this(null!) { }

        public SplitEditor(WinGUIMain mainForm)
        {
            InitializeComponent();

            MainFormReference = mainForm;

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
                FlatStyle = FlatStyle.Flat
            };
            btnToggle.FlatAppearance.BorderSize = 0;
            btnToggle.ImageAlign = ContentAlignment.MiddleCenter;
            SyncButtonSizeToSplitter();         // ensure the button is not taller than the splitter (<= 20px)
            SetToggleIcon(_toggleDown); // start with "up"
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
                SetToggleIcon(_toggleDown);     // rescale icon to new size
                RepositionButton();
            };
            splitContainer1.LocationChanged += (s, e) => RepositionButton();
            this.SizeChanged += (s, e) =>
            {
                ApplyRevealForCurrentMode();
                SyncButtonSizeToSplitter();
                SetToggleIcon(_toggleDown);
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

            // Initialize with a visible splitter at the middle
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = Math.Max(SplitterReveal, splitContainer1.Height / 2);

            RepositionButton();

            this.Load += splitEditor_Load;

            #endregion ================ END SPLITTER =========================

        }

        private void splitEditor_Load(object? sender, EventArgs e)
        {
            #region ================ BEGIN PANEL @1 =====================================

            this.cbSearchTypes.SelectedIndexChanged += cbSearchTypes_SelectedIndexChanged;
            this.btnSearch.Click += btnSearch_Click!;
            this.ddCategories.SelectedIndexChanged += ddCategories_SelectedIndexChanged;

            this.btnAddChecked.Click += btnAddChecked_Click;
            this.btnCreateJsonFile.Click += btnCreateJsonFile_Click;

            this.btnRefreshGroups.Click += btnRefreshGroups_Click;

            this.dgvVideos.CellContentClick += dgvVideos_CellContentClick;
            this.dgvVideos.CellFormatting += dgvVideos_CellFormatting;
            this.dgvVideos.CellMouseEnter += dgvVideos_CellMouseEnter;
            this.dgvVideos.CellMouseLeave += dgvVideos_CellMouseLeave;

            this.btnCheckAll.Click += btnCheckAll_Click;
            this.btnUnCheckAll.Click += btnUnCheckAll_Click;

            this.btnGetRumble.Click += btnGetRumble_Click;

            InitializeDataGridView();

            AddColumns();

            cbSearchTypes.SelectedIndex = 0;

            UpdateCategories();
            UpdateSubCategories();
            UpdateGroupCategories();
            //UpdateMovieCategories();

            ddMinimumLength.SelectedIndex = 0;

            txtSearch.KeyDown += txtSearch_KeyDown!;

            #endregion ============= BEGIN PANEL @1 =====================================


            #region ================ BEGIN PANEL @2 =====================================

            //InitializeMode(XDTransportMode.WindowsMessaging);
            //broadcast.SendToChannel("Status", string.Format("Handle: {0} connected!", this.Handle));

            this.btnPrev2.Click += btnPrev2_Click;
            this.btnNext2.Click += btnNext2_Click;
            this.ddCategories2.SelectedIndexChanged += ddCategories2_SelectedIndexChanged;
            this.ddSubCategories2.SelectedIndexChanged += ddSubCategories2_SelectedIndexChanged;
            this.ddMCategories2.SelectedIndexChanged += ddMCategories2_SelectedIndexChanged;


            this.ddSCategories.SelectedIndexChanged += ddSCategories_SelectedIndexChanged;
            this.ddSSubCategories.SelectedIndexChanged += ddSSubCategories_SelectedIndexChanged;
            this.ddSGroupCategories.SelectedIndexChanged += ddSGroupCategories_SelectedIndexChanged;
            this.ddSMovieCategories.SelectedIndexChanged += ddSMovieCategories_SelectedIndexChanged;
            this.ddSLinkTypes.SelectedIndexChanged += ddSLinkTypes_SelectedIndexChanged;


            //this.btnFeeds2.Click += btnFeeds2_Click;
            //this.btnSearchVideoTitles2.Click += new System.EventHandler(this.btnSearchVideoTitles_Click);
            ////this.btnJSON.Click += new System.EventHandler(this.btnJSON_Click);
            ////this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            ////this.btnLinkValue.Click += new System.EventHandler(this.btnLinkValue_Click);
            //this.btnGetDescription.Click += new System.EventHandler(this.btnGetDescription_Click);


            //this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            //this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //this.ddSFolders.SelectedIndexChanged += new System.EventHandler(this.ddSFolders_SelectedIndexChanged);
            //this.ddSImages.SelectedIndexChanged += new System.EventHandler(this.ddSImages_SelectedIndexChanged);

            ddSearch2.Items.Clear();
            ddSearch2.Items.Add("title");
            ddSearch2.Items.Add("author");
            ddSearch2.Items.Add("shortDescription");
            ddSearch2.Items.Add("description");
            ddSearch2.Items.Add("FeedId");
            ddSearch2.SelectedIndex = 0;

            ddMCategories2.Items.Clear();
            ddMCategories2.Items.Add("Action");
            ddMCategories2.Items.Add("Animation");
            ddMCategories2.Items.Add("Classics");
            ddMCategories2.Items.Add("Comedy");
            ddMCategories2.Items.Add("Cult Classics");
            ddMCategories2.Items.Add("Drama");
            ddMCategories2.Items.Add("Documentary");
            ddMCategories2.Items.Add("Horror");
            ddMCategories2.Items.Add("International");
            ddMCategories2.Items.Add("Family");
            ddMCategories2.Items.Add("LGBTQ+");
            ddMCategories2.Items.Add("Mystery");
            ddMCategories2.Items.Add("Musicals");
            ddMCategories2.Items.Add("Romance");
            ddMCategories2.Items.Add("Sci-Fi");
            ddMCategories2.Items.Add("Special Interest");
            ddMCategories2.Items.Add("Sports");
            ddMCategories2.Items.Add("Suspense");
            ddMCategories2.Items.Add("Thrillers");
            ddMCategories2.Items.Add("Time Travel");
            ddMCategories2.Items.Add("Western");
            ddMCategories2.SelectedIndex = 0;

            //AddColumns();

            //By default, text in a DataGridViewTextBoxCell does not wrap. 
            //This can be controlled via the WrapMode property on the cell style (e.g. DataGridView.DefaultCellStyle.WrapMode). 
            //Set the WrapMode property of a DataGridViewCellStyle to one of the DataGridViewTriState enumeration values. 
            //The following code example uses the DataGridView.DefaultCellStyle property to set the wrap mode for the entire control.
            //this.dgvFeeds.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //InitializeDataGridView();

            //this.dgvFeeds.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvFeeds_DataError);

            ///////////////////////////// GET LINK TYPES ////////////////////
            //ddSLinkTypes.Items.Clear();
            //string cat = "category";
            //List<string> lstLinkTypes = DataHelper.GetLinkTypes(cat);
            //foreach (string item in lstLinkTypes)
            //{
            //    if (item != null && item.Trim().Length > 0)
            //    {
            //        ddSLinkTypes.Items.Add(item.Trim());
            //    }
            //}
            ////////////////////////////////////////////////////////////////

            //updateCategories();
            ////updateSubCategories();
            ////updateGroupCategories();

            this.dgvFeeds.AllowUserToAddRows = false;  // Disable adding the new row
            this.dgvFeeds.CellFormatting += dgvFeeds_CellFormatting;
            //this.dgvFeeds.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFeeds_CellClick);
            //this.dgvFeeds.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvFeeds_CellPainting);

            //this.dgvFeeds.Columns[0].DefaultCellStyle.Padding = new Padding(4, 0, 0, 0);




            //dd_anticoagulant.SelectedIndex = 0;
            //dd_carcinogen.SelectedIndex = 0;
            //dd_hypoglycemic.SelectedIndex = 0;
            //dd_kidneydamage.SelectedIndex = 0;
            //dd_liverdamage.SelectedIndex = 0;

            //dd_neuropathy.SelectedIndex = 0;
            //dd_nootropic.SelectedIndex = 0;

            //loadImages();

            #endregion ============= BEGIN PANEL @2 =====================================
        }





        #region ============== BEGIN SPLITTER =============================

        private void btnToggle_Click(object? sender, EventArgs e)
        {
            // Flip between panels:
            // If current icon is "up" ( _toggleDown == false ), clicking shows BOTTOM panel (Panel2) with a small reveal for Panel1.
            // If current icon is "down" ( _toggleDown == true ), clicking shows TOP panel (Panel1) with a small reveal for Panel2.

            if (!_toggleDown)
            {
                // Show bottom panel "full", keep a configurable band of Panel1 (often 0) so the splitter is visible at top.
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;

                splitContainer1.SplitterDistance = ClampSplitterDistance(Math.Max(0, _bottomModeReveal));
                _toggleDown = true; // switch icon to "down"
            }
            else
            {
                // Show top panel "full", keep a configurable band of Panel2 (often 0) so the splitter is visible at bottom.
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;

                int distanceForTop = Math.Max(0, splitContainer1.Height - splitContainer1.SplitterWidth - _topModeReveal);
                splitContainer1.SplitterDistance = ClampSplitterDistance(distanceForTop);
                _toggleDown = false; // switch icon to "up"
            }

            SetToggleIcon(_toggleDown);
            RepositionButton();
        }

        private void SetToggleIcon(bool down)
        {
            try
            {
                // Use the ImageList ("up" / "down") and scale to the larger button via BackgroundImage
                var key = down ? "down" : "up";
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
            if (splitContainer1.Orientation != Orientation.Horizontal) return;

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
            if (_toggleDown)
            {
                // Bottom mode: keep Panel1 at BottomModeReveal
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.SplitterDistance = ClampSplitterDistance(Math.Max(0, _bottomModeReveal));
            }
            else
            {
                // Top mode: keep Panel2 at TopModeReveal
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;
                int distanceForTop = Math.Max(0, splitContainer1.Height - splitContainer1.SplitterWidth - _topModeReveal);
                splitContainer1.SplitterDistance = ClampSplitterDistance(distanceForTop);
            }
        }

        private int ClampSplitterDistance(int desired)
        {
            // Ensure the splitter distance stays within valid bounds
            int min = splitContainer1.Panel1MinSize;
            int max = Math.Max(0, splitContainer1.Height - splitContainer1.SplitterWidth - splitContainer1.Panel2MinSize);
            if (desired < min) desired = min;
            if (desired > max) desired = max;
            return desired;
        }

        private void SyncButtonSizeToSplitter()
        {
            // Ensure the button height is never larger than the splitter height and never exceeds the property cap
            int splitterH = Math.Max(1, splitContainer1.SplitterWidth);
            int targetH = Math.Min(splitterH - 2, _toggleMaxHeight); // leave 1px margins
            if (targetH < 12) targetH = Math.Min(12, splitterH);    // keep it reasonably clickable

            // Use designer-tweakable width
            int targetW = Math.Max(8, _toggleButtonWidth);

            btnToggle.Size = new Size(targetW, targetH);
        }

        #endregion ============== END SPLITTER =============================


        #region ============== BEGIN TOP PANEL =============================

        private void cbSearchTypes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string[] arrLabels = { "Enter General search Term", "Enter Channel ID", "Enter Playlist Name" };
            string[] arrText = { "Fights in Sports", "UCU8Xw_KewvcC3LV3Qv--JZg", "Bushido" };
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<RumbleVideoInfo> lstRumble = new List<RumbleVideoInfo>();
            string _search = txtSearch.Text;
            int _max = Convert.ToInt32(upDown.Value);

            if (_search.Length < 1)
            {
                MessageBox.Show("Can't Be Empty!");
                return;
            }
            if (_max < 1)
            {
                MessageBox.Show("Can't Be Zero!");
                return;
            }

            lblCount.Text = "Total: 0";
            dgvVideos.Rows.Clear();

            string s = cbSearchTypes.SelectedItem!.ToString()!;

            string _duration = ddMinimumLength.SelectedItem!.ToString()!;

            if (s == "YouTube: Search General")
            {
                Search(_search, _max, dgvVideos, lblCount, _duration);
            }
            else if (s == "YouTube: Search Channel")
            {
                //Search_ChannelItems(_search, _max, dgvVideos, labelEx1, broadcast);
            }
            else if (s == "YouTube: Search Playlist")
            {
                //Search_Playlist(_search, _max, dgvVideos, labelEx1, broadcast);
            }
            else if (s == "YouTube: Download Video")
            {
                //DownloadYouTubeByVideoID(_search);
            }
            else if (s == "Rumble: Search")
            {
                //await Task.Run(() =>
                //{
                //    _ = SearchForRumbleVideosAsync(_search, _max, dgvVideos, labelEx1, broadcast, _duration);
                //});
            }
        }

        private void ddCategories_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ////if (ddCategories.Text.Trim() == "All Movies" || ddCategories.Text.Trim() == "movies")
            ////{
            ////    lblMovieCategory.Visible = true;
            ////    ddMovieCategories.Visible = true;
            ////}
            ////else
            ////{
            ////    lblMovieCategory.Visible = false;
            ////    ddMovieCategories.Visible = false;
            ////}
            //txtCategory.Text = string.Empty;

            UpdateSubCategories();

        }

        private void UpdateCategories()
        {
            Tubes.EnsureDatabase(seedIfEmpty: true);
            var cats = Tubes.GetCategories();
            ddCategories.Items.Clear();
            foreach (var c in cats) ddCategories.Items.Add(c);
            if (ddCategories.Items.Count > 0) ddCategories.SelectedIndex = 0;
        }

        private void UpdateSubCategories()
        {
            var cat = ddCategories.SelectedItem?.ToString() ?? "";
            var subs = Tubes.GetSubcategories(cat);

            ddSubCategories!.Items.Clear();
            ddSubCategories.Items.Add(""); // allow 'no subcategory' option
            foreach (var s in subs) ddSubCategories.Items.Add(s);
            ddSubCategories.SelectedIndex = 0;
        }

        private void UpdateGroupCategories()
        {
            List<FeedGroupCategories> result = new List<FeedGroupCategories>();

            var cat = ddCategories.SelectedItem?.ToString() ?? "";
            var subs = Tubes.GetSubcategories(cat);

            var qqq = Tubes.GetGroupCategories(cat, subs);

            ddGroupCategories.Items.Clear();
            //ddGroupCategories.Items.Add(""); // allow 'no subcategory' option
            foreach (var s in subs) ddGroupCategories.Items.Add(s);
            ddSubCategories.SelectedIndex = 0;
        }

        private void btnAddChecked_Click(object? sender, EventArgs e)
        {
            int _checked = 0;

            // Ensure any in-progress edits are committed so checkbox values are current
            dgvVideos.EndEdit();

            foreach (DataGridViewRow row in dgvVideos.Rows)
            {
                if (row.IsNewRow) continue;
                bool isChecked = (row.Cells["X"].Value as bool?) == true; // use named checkbox column
                if (isChecked)
                {
                    _checked++;
                }
            }

            if (_checked < 1)
            {
                MessageBox.Show("You MUST Check Rows You Want to Save!");
                return;
            }

            //lblStatus.Text = "Processing: 0 / " + _checked + " Checked Videos";

            // Assuming 'yourComboBox' is your SerGioComboBox instance
            string typedValue = ddCategories.Text;
            MessageBox.Show(typedValue);

            string _category = ddCategories.Text.Trim();
            string _subcategory = ddSubCategories.Text.Trim();
            string _groupcategory = ddGroupCategories.Text.Trim();

            if (_category.Length < 1)
            {
                MessageBox.Show("You MUST Select or Enter A Value for Category!");
                return;
            }

            if (_subcategory.Length < 1)
            {
                MessageBox.Show("You MUST Select or Enter A Value for Sub Category!");
                return;
            }

            if (_groupcategory.Length < 1)
            {
                MessageBox.Show("You MUST Select or Enter A Value for Group Category!");
                return;
            }

            //string s = "Category: " + _category + "\nSubcategory: " + _subcategory + "\nGroup Category: " + _groupcategory;
            //MessageBox.Show(s);

            //string _moviecategory = ddMovieCategories.SelectedItem.ToString();
            //if (_category == "All Movies")
            //{

            //}

            //string _subcategory = txtSubCategory.Text.Trim();
            ////if (_subcategory.Length < 1)
            ////{
            ////    _subcategory = ddCategories.SelectedItem.Col1.Trim();
            ////}

            //string _groupcategory = txtGroupCategory.Text.Trim();

            //string str = "Do You Want to Save Checked Items with Category: " + _category;
            //if (ddCategories.SelectedItem.Col1.Trim() == "movies")
            //{
            //    str = str + "\r\nAnd Movie Category: " + ddMovieCategories.SelectedItem.ToString();
            //}

            //DialogResult dialogResult = MessageBox.Show(str, "Question", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //}
            //else if (dialogResult == DialogResult.No)
            //{
            //    return;
            //}

            List<FeedItem> list = new List<FeedItem>();

            foreach (DataGridViewRow row in dgvVideos.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = (row.Cells["X"].Value as bool?) == true; // use named checkbox column
                if (isChecked)
                {
                    var f = new FeedItem();
                    f.FeedId = string.Empty;    // row.Cells[2].Value.ToString();   //2 FeedId Guid id = Guid.NewGuid();
                    f.Category = _category;
                    f.SubCategory = _subcategory;
                    f.CatSub = f.Category + "_" + f.SubCategory;
                    f.GroupCategory = _groupcategory;
                    f.Title = row.Cells["title"].Value?.ToString() ?? string.Empty;    //3 title
                    f.Author = row.Cells["author"].Value?.ToString() ?? string.Empty;   //4 author

                    var linkTypeVal = row.Cells["linkType"].Value?.ToString() ?? string.Empty;
                    var linkValueVal = row.Cells["linkValue"].Value?.ToString() ?? string.Empty;

                    // --- begin: robust linkType/linkValue normalization ---
                    linkTypeVal = linkTypeVal.Trim();
                    linkValueVal = linkValueVal.Trim();

                    // If linkValue contains a full YouTube URL, extract the ID
                    string ExtractYouTubeId(string url)
                    {
                        try
                        {
                            // common patterns: https://www.youtube.com/watch?v=ID, https://youtu.be/ID
                            if (url.Contains("youtube.com", StringComparison.OrdinalIgnoreCase))
                            {
                                var uri = new Uri(url);
                                var q = System.Web.HttpUtility.ParseQueryString(uri.Query);
                                var v = q.Get("v");
                                if (!string.IsNullOrWhiteSpace(v)) return v;
                                // sometimes /embed/ID
                                var seg = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                                int embedIdx = Array.FindIndex(seg, s => s.Equals("embed", StringComparison.OrdinalIgnoreCase));
                                if (embedIdx >= 0 && embedIdx + 1 < seg.Length) return seg[embedIdx + 1];
                            }
                            if (url.Contains("youtu.be", StringComparison.OrdinalIgnoreCase))
                            {
                                var uri = new Uri(url);
                                var seg = uri.AbsolutePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
                                if (seg.Length > 0) return seg[0];
                            }
                        }
                        catch { }
                        return string.Empty;
                    }

                    if (linkValueVal.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        var maybeId = ExtractYouTubeId(linkValueVal);
                        if (!string.IsNullOrWhiteSpace(maybeId))
                            linkValueVal = maybeId;
                    }

                    // If linkType is empty but linkValue is present, assume youtube (matches your search loader)
                    if (string.IsNullOrWhiteSpace(linkTypeVal) && !string.IsNullOrWhiteSpace(linkValueVal))
                    {
                        linkTypeVal = "youtube";
                    }
                    // --- end: robust linkType/linkValue normalization ---

                    if (string.Equals(linkTypeVal, "rumble", StringComparison.OrdinalIgnoreCase))
                    {
                        f.LinkType = "rumble";
                        f.LinkValue = linkValueVal;
                        f.Link = "https://rumble.com/embed/" + linkValueVal;
                    }
                    else
                    {
                        f.LinkType = "youtube";
                        f.LinkValue = linkValueVal;
                        f.Link = "https://www.youtube.com/watch?v=" + linkValueVal;
                    }

                    f.ShortDescription = row.Cells["shortDescription"].Value?.ToString() ?? string.Empty; //5 shortDescription
                    f.Description = row.Cells["description"].Value?.ToString() ?? string.Empty;  //6 descript6ion
                    f.BodyLinks = string.Empty;

                    // Handle image column (DataGridViewImageColumn). Prefer a URL stored in a companion place; otherwise construct if possible.
                    var imgCell = row.Cells["image"];
                    string? imageUrl = null;

                    // 1) If you added a hidden text column to hold the URL, use it (no exception if it doesn't exist)
                    if (dgvVideos.Columns.Contains("imageUrl"))
                    {
                        imageUrl = row.Cells["imageUrl"].Value?.ToString();
                    }

                    // 2) Try Tag on the image cell
                    if (string.IsNullOrWhiteSpace(imageUrl))
                        imageUrl = imgCell.Tag as string;

                    // 3) Try ToolTipText on the image cell
                    if (string.IsNullOrWhiteSpace(imageUrl))
                        imageUrl = imgCell.ToolTipText;

                    // 4) If the Image object itself has a Tag, use it
                    if (string.IsNullOrWhiteSpace(imageUrl) && imgCell.Value is Image imgWithTag && imgWithTag?.Tag is string imgTag && !string.IsNullOrWhiteSpace(imgTag))
                        imageUrl = imgTag;

                    // 5) Explicitly IGNORE any non-URL string values like "Image" or type names
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        var trimmed = imageUrl.Trim();
                        if (trimmed.Equals("Image", StringComparison.OrdinalIgnoreCase) ||
                            trimmed.StartsWith("System.Drawing", StringComparison.OrdinalIgnoreCase))
                        {
                            imageUrl = null;
                        }
                    }

                    // 6) Fallbacks based on linkType/linkValue (ensures FULL URL)
                    if (string.IsNullOrWhiteSpace(imageUrl))
                    {
                        if (string.Equals(linkTypeVal, "youtube", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(linkValueVal))
                        {
                            imageUrl = "https://img.youtube.com/vi/" + linkValueVal + "/hqdefault.jpg";
                        }
                        else if (string.Equals(linkTypeVal, "rumble", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(linkValueVal))
                        {
                            // Generic rumble thumbnail fallback; replace if you persist a precise URL elsewhere
                            imageUrl = "https://i1.rumble.com/" + linkValueVal + ".jpg";
                        }
                    }

                    f.Image = imageUrl ?? string.Empty;

                    //if ((f.image == null) || (f.image == ""))
                    //{
                    //    f.image = "https://img.youtube.com/vi/" + f.linkValue + "/hqdefault.jpg";
                    //}

                    f.Rank = Convert.ToInt32(row.Cells["rank"].Value ?? 0);
                    f.PublishedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

                    if (f.Category == "All Movies")
                    {
                        f.MovieCategory = f.SubCategory;
                    }
                    else
                    {
                        f.MovieCategory = string.Empty;
                    }
                    f.Duration = row.Cells["duration"].Value?.ToString() ?? string.Empty; //7 duration
                    list.Add(f);
                }

            } // end foreach

            var sqqq = "";

            UpdateServer(list, dgvVideos, lblCount);
        }




        // "POCO" stands for plain old CLR object.A POCO is a.NET type that doesn't depend 
        // on any framework-specific types, for example, through inheritance or attributes.
        public void UpdateServer(List<FeedItem> videos, DataGridView dgv, Label labelx)
        {
            foreach (FeedItem video in videos)
            {
                //PostData(video, dgv, labelx);
                Tubes.UpdateFeeds(video, dgv, labelx);
                Thread.Sleep(100);

            }
            //txtCategory.Text = string.Empty;
            Thread.Sleep(1000);
            UpdateCategories();
            //UpdateMovieCategories();
            MessageBox.Show("Completed!");
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the ding sound on pressing Enter
                e.SuppressKeyPress = true;

                // TODO: Place the code you want to execute on Enter key press here
                //MessageBox.Show("Enter key pressed!");

                btnSearch_Click(btnSearch, EventArgs.Empty);

            }
        }


        private void btnCreateJsonFile_Click(object? sender, EventArgs e)
        {

        }

        private void btnRefreshGroups_Click(object? sender, EventArgs e)
        {

        }

        private void dgvVideos_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            //if (dgvVideos.Columns[e.ColumnIndex].Name == "image")
            //{
            //    var columnValue = dgvVideos.Rows[e.RowIndex].Cells[9].Value;
            //    string url = string.Empty;

            //    if (columnValue != null && columnValue.ToString() == "rumble")
            //    {
            //        url = "https://rumble.com/embed/" + (string)dgvVideos.Rows[e.RowIndex].Cells["linkValue"].Value!;
            //    }
            //    else
            //    {
            //        url = "https://www.youtube.com/watch?v=" + (string)dgvVideos.Rows[e.RowIndex].Cells["linkValue"].Value!;
            //    }
            //    WinGUIMain.OpenUrlInChromeOrDefault(url);
            //}
        }

        private void dgvVideos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            // assume your DataGridViewImageColumn name is Column1,
            // you can change this as your column name
            //if (dgvVideos.Columns[e.ColumnIndex].Name == "image")
            //{
            //    Bitmap bmp = (Bitmap)GetImageFromUrl(e.Value.ToString());
            //    if (bmp != null)
            //    {
            //        e.Value = bmp;
            //    }
            //    else
            //    {
            //        e.Value = null;
            //    }
            //}

            //if (dgvVideos.Columns[e.ColumnIndex].Name == "X")
            //{
            //    dgvVideos.Columns[e.ColumnIndex].DefaultCellStyle.Font = new Font("Tahoma", 36);
            //    //CheckBoxElement el = checkCell.Children[0] as RadCheckBoxEditorElement;
            //    //el.Checkmark.CheckElement.UseFixedCheckSize = false;
            //    //el.Checkmark.CheckElement.MinSize = new Size(32, 32);
            //}
        }

        public static System.Drawing.Image GetImageFromUrl(string url)
        {
            Uri z;
            bool v = Uri.TryCreate(url, UriKind.Absolute, out z);
            if (!v)
            {
                return null!;
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(z);
            // if you have proxy server, you may need to set proxy details like below 
            //httpWebRequest.Proxy = new WebProxy("proxyserver",port){ 
            //  Credentials = new NetworkCredential(){ UserName ="uname", Password = "pw"}};
            using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (Stream stream = httpWebReponse.GetResponseStream())
                {
                    return Image.FromStream(stream);
                }
            }
        }

        private void btnCheckAll_Click(object? sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvVideos.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).Value = true;
            }
        }

        private void btnUnCheckAll_Click(object? sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvVideos.Rows)
            {
                ((DataGridViewCheckBoxCell)row.Cells[0]).Value = false;
            }
        }

        private void btnGetRumble_Click(object? sender, EventArgs e)
        {
            //string videoUrl = string.Empty;
            //string linkValue = string.Empty;

            //List<RumbleVideoInfo> lstRumble = SearchForRumbleVideos("cars");

            //string z2 = "";

            //if (dgvVideos.SelectedRows.Count > 0)
            //{
            //    //RSSFeed f = new RSSFeed();
            //    //f.FeedId = string.Empty;    // row.Cells[2].Value.ToString();   //2 FeedId Guid id = Guid.NewGuid();
            //    //f.title = row.Cells[2].Value.ToString();    //3 title
            //    //f.author = row.Cells[3].Value.ToString();   //4 author
            //    //f.link = "https://www.youtube.com/watch?v=" + row.Cells[7].Value.ToString();
            //    //f.linkType = "youtube";
            //    //f.linkValue = row.Cells[7].Value.ToString();    //8 linkValue
            //    //f.shortDescription = row.Cells[4].Value.ToString(); //5 shortDescription
            //    //f.description = row.Cells[5].Value.ToString();  //6 descript6ion

            //    DataGridViewRow selectedRow = dgvVideos.SelectedRows[0];
            //    videoUrl = string.Empty;
            //    linkValue = selectedRow.Cells[7].Value.ToString();
            //    if (linkValue.Length > 0)
            //    {
            //        var url = txtURLRumble.Text.Trim(); 
            //        try
            //        {
            //            using (var httpClient = new HttpClient())
            //            {
            //                var html = httpClient.GetStringAsync(url).GetAwaiter().GetResult();
            //                var regex = new Regex("\"video\":\"(.*?)\"");
            //                var match = regex.Match(html);

            //                if (match.Success)
            //                {
            //                    Console.WriteLine($"[0] => \"{match.Groups[0].Value}\"");
            //                    Console.WriteLine($"[1] => {match.Groups[1].Value}");
            //                    linkValue = match.Groups[1].Value;
            //                    //selectedRow.Cells[7].Value = linkValue;
            //                    //selectedRow.Cells[8].Value = "https://rumble.com/embed/" + linkValue;
            //                    MessageBox.Show(linkValue);
            //                }
            //                else
            //                {
            //                    Console.WriteLine("No matches found.");
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine($"An error occurred: {ex.Message}");
            //        }
            //    }
            //}
        }

        private void InitializeDataGridView()
        {
            System.Windows.Forms.DataGridViewCellStyle styleHeader = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle styleRow = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle styleAlternating = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle styleSelected = new System.Windows.Forms.DataGridViewCellStyle();
            System.Drawing.Font DefaultStateFont = new System.Drawing.Font("Trebuchet MS", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            styleRow.Font = DefaultStateFont;
            this.dgvVideos.RowsDefaultCellStyle = styleRow;

            // Initialize basic DataGridView properties.
            dgvVideos.Dock = DockStyle.Fill;
            dgvVideos.BackgroundColor = Color.FromArgb(29, 29, 29);
            dgvVideos.BackgroundColor = Color.FromArgb(255, 255, 255);
            dgvVideos.BorderStyle = BorderStyle.None;
            dgvVideos.RowHeadersVisible = false;

            dgvVideos.AllowUserToAddRows = false;
            dgvVideos.AllowUserToDeleteRows = false;
            dgvVideos.AllowUserToOrderColumns = true;
            dgvVideos.ReadOnly = false;
            dgvVideos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVideos.MultiSelect = false;
            dgvVideos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvVideos.AllowUserToResizeColumns = true;
            //dgvVideos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvVideos.AllowUserToResizeRows = false;
            dgvVideos.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Set the selection background color for all the cells.
            dgvVideos.DefaultCellStyle.SelectionBackColor = Color.DarkGray;
            dgvVideos.DefaultCellStyle.SelectionForeColor = Color.White;

            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            dgvVideos.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;

            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            //dgvVideos.RowsDefaultCellStyle.BackColor = Color.Black;
            //dgvVideos.RowsDefaultCellStyle.BackColor = Color.White;
            //dgvVideos.RowsDefaultCellStyle.ForeColor = Color.Black;
            //dgvVideos.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // Color.FromArgb(30, 30, 30);
            //dgvVideos.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            dgvVideos.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(29, 29, 29);

            //splitContainer1.BackColor = Color.FromArgb(29, 29, 29);
            //splitContainer1.Panel1.BackColor = Color.FromArgb(29, 29, 29);
            //splitContainer1.Panel2.BackColor = Color.FromArgb(29, 29, 29);

            // --- Make the grid font one size smaller than default ---
            // Make the entire grid one size smaller
            dgvVideos.Font = new Font(dgvVideos.Font.FontFamily, dgvVideos.Font.Size - 1);

            // Ensure rows respect the new font
            dgvVideos.DefaultCellStyle.Font = dgvVideos.Font;
            dgvVideos.RowsDefaultCellStyle.Font = dgvVideos.Font;

            // (Optional) also apply to column headers if you want them smaller
            dgvVideos.ColumnHeadersDefaultCellStyle.Font = dgvVideos.Font;



            dgvVideos.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvVideos.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvVideos.IsCurrentCellDirty &&
                    dgvVideos.CurrentCell is DataGridViewCheckBoxCell)
                {
                    dgvVideos.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dgvVideos.EndEdit();
                }
            };



            // --- Wrap text in specific columns (ShortDescription, Description)
            // and resize rows ONCE --- Keeps image columns untouched so existing
            // image rendering isn’t altered.
            void __ApplyWrapAndResize()
            {
                string[] wrapCols = { "ShortDescription", "Description" };
                foreach (var name in wrapCols)
                {
                    if (dgvVideos.Columns.Contains(name))
                        dgvVideos.Columns[name]!.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }

                // Resize rows based on current content so wrapped text is visible.
                // This does not change image layout; it only adjusts row height once.
                dgvVideos.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
            }

            // Apply after data binding and whenever column widths change.
            dgvVideos.DataBindingComplete += (s, e) => __ApplyWrapAndResize();
            dgvVideos.ColumnWidthChanged += (s, e) => __ApplyWrapAndResize();

        }


        public void AddColumns()
        {
            dgvVideos.DataSource = null;
            dgvVideos.Rows.Clear();
            dgvVideos.Columns.Clear();
            //dgvVideos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgvVideos.ColumnCount = 8;
            dgvVideos.RowTemplate.Height = 60;

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "X";
            checkColumn.HeaderText = "X";
            checkColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            checkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            checkColumn.Width = 30;
            checkColumn.ReadOnly = false;
            //if the datagridview is resized (on form resize) the checkbox won't take 
            //up too much; value is relative to the other columns' fill values
            checkColumn.FillWeight = 18;
            //checkColumn.DefaultCellStyle.Font = new Font("Tahoma", 36);
            // Ensure checkbox stores bools only (avoid nulls in normal use)
            checkColumn.ValueType = typeof(bool);
            checkColumn.ThreeState = false;
            checkColumn.TrueValue = true;
            checkColumn.FalseValue = false;
            dgvVideos.Columns.Insert(0, checkColumn);

            DataGridViewImageColumn dgvImageColumn = new DataGridViewImageColumn();
            dgvImageColumn.Name = "image";
            dgvImageColumn.Width = 100;
            dgvImageColumn.HeaderText = "Image";
            dgvImageColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvImageColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvImageColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dgvVideos.Columns.Insert(1, dgvImageColumn);

            dgvVideos.CellClick += dgvVideos_CellClick;
            //dgvVideos.CellMouseEnter += dgvVideos_CellMouseEnter;
            //dgvVideos.CellMouseLeave += dgvVideos_CellMouseLeave;


            dgvVideos.Columns[2].Name = "title";
            dgvVideos.Columns[2].HeaderText = "Title";
            dgvVideos.Columns[2].Width = 150;
            //dgvVideos.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvVideos.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            dgvVideos.Columns[3].Name = "author";
            dgvVideos.Columns[3].HeaderText = "Author";
            dgvVideos.Columns[3].Width = 120;
            dgvVideos.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            dgvVideos.Columns[4].Name = "shortDescription";
            dgvVideos.Columns[4].HeaderText = "Short Description";
            dgvVideos.Columns[4].Width = 200;
            dgvVideos.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            dgvVideos.Columns[5].Name = "description";
            dgvVideos.Columns[5].HeaderText = "Description";
            dgvVideos.Columns[5].Width = 240;
            dgvVideos.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            dgvVideos.Columns[6].Name = "duration";
            dgvVideos.Columns[6].HeaderText = "Duration";
            dgvVideos.Columns[6].Width = 50;
            //dgvVideos.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvVideos.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvVideos.Columns[7].Name = "linkValue";
            dgvVideos.Columns[7].HeaderText = "Link Value";
            dgvVideos.Columns[7].Width = 150;
            dgvVideos.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvVideos.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvVideos.Columns[8].Name = "rank";
            dgvVideos.Columns[8].HeaderText = "Rank";
            dgvVideos.Columns[8].Width = 80;
            dgvVideos.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvVideos.Columns[9].Name = "linkType";
            dgvVideos.Columns[9].HeaderText = "Link Type";
            dgvVideos.Columns[9].Width = 80;
            dgvVideos.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvVideos.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Persist the FULL image URL in a hidden text column so it can always be retrieved later.
            var imageUrlCol = new DataGridViewTextBoxColumn
            {
                Name = "imageUrl",
                HeaderText = "Image Url",
                Visible = false
            };
            dgvVideos.Columns.Add(imageUrlCol);
        }

        // Handler for clicks on the image column
        private void dgvVideos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvVideos.Columns["image"]!.Index)
            {
                var linkValueObj = dgvVideos.Rows[e.RowIndex].Cells["linkValue"].Value;
                var linkTypeObj = dgvVideos.Rows[e.RowIndex].Cells["linkType"].Value;

                var linkValue = linkValueObj?.ToString() ?? string.Empty;
                var linkType = linkTypeObj?.ToString()?.ToLowerInvariant() ?? string.Empty;

                string url = string.Empty;

                if (linkType == "youtube" && !string.IsNullOrWhiteSpace(linkValue))
                {
                    url = "https://www.youtube.com/watch?v=" + linkValue;
                }
                // 🔑 Add other linkType handling here if needed
                // else if (linkType == "vimeo") { url = "https://vimeo.com/" + linkValue; }

                if (!string.IsNullOrWhiteSpace(url))
                {
                    WinGUIMain.OpenUrlInChromeOrDefault(url);
                }
                else
                {
                    MessageBox.Show("No URL available for this row.");
                }
            }
        }


        // Change cursor to hand when hovering over the image column
        private void dgvVideos_CellMouseEnter(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvVideos.Columns["image"]!.Index)
            {
                dgvVideos.Cursor = Cursors.Hand;
            }
        }

        // Restore cursor when leaving the image column
        private void dgvVideos_CellMouseLeave(object? sender, DataGridViewCellEventArgs e)
        {
            dgvVideos.Cursor = Cursors.Default;
        }

        static async void Search(string search, int max, DataGridView dgv, Label labelx, string dur)
        {
            //Log.setMode(false);

            int idur = Convert.ToInt32(dur);

            string querystring = search;
            int querypages = max;

            VideoSearch videos = new VideoSearch();
            var items = await videos.GetVideos(querystring, querypages);
            //var items = await videos.GetVideosPaged(querystring, querypages);

            List<RSSFeed> list = new List<RSSFeed>();

            foreach (var item in items)
            {
                RSSFeed f = new RSSFeed();
                f.title = FilterString(item.getTitle());
                f.author = item.getAuthor();

                //////////////////////////////////////////////////////////
                string _description = FilterString(item.getDescription());
                if (_description.Length < 1)
                {
                    _description = item.getTitle();
                }
                f.description = _description;
                f.shortDescription = _description;
                //////////////////////////////////////////////////////////

                // for movies!!!
                f.duration = item.getDuration();

                string x = item.getUrl();
                char[] equal = new char[1] { '=' };
                String[] strlist = x.Split(new char[] { '=' });

                if (strlist[1].Length > 0)
                {
                    f.linkValue = strlist[1];
                }
                else
                {
                    f.linkValue = string.Empty;
                }

                f.image = item.getThumbnail();
                string _views = item.getViewCount();
                int parsedViews;
                if (!string.IsNullOrEmpty(_views) && int.TryParse(_views.Replace(".", ""), out parsedViews))
                {
                    f.rank = parsedViews;
                }
                else
                {
                    f.rank = 0; // Default value if _views is null, empty, or not a valid int
                }


                Guid id = Guid.NewGuid();
                f.FeedId = id.ToString();
                f.category = "unknown";
                //f.title = sdr.GetString("title");
                //f.author = sdr.GetString("author");
                f.link = item.getUrl();
                f.linkType = "youtube";
                //f.linkValue = sdr.GetString("linkValue");
                f.bodyLinks = string.Empty;
                //f.image = sdr.GetString("image");
                f.warnings = string.Empty;
                f.sideeffects = string.Empty;
                f.dosage = string.Empty;
                f.anticoagulant = false;
                f.carcinogenic = false;
                f.hypoglycemic = false;
                f.liverdamage = false;
                f.kidneydamage = false;
                //f.rank = sdr.GetInt32("rank");
                f.publishedDate = DateTime.UtcNow;
                f.beginDate = DateTime.UtcNow;
                f.endDate = DateTime.UtcNow;
                f.city = string.Empty;
                f.state = string.Empty;
                f.postalCode = string.Empty;
                f.country = string.Empty;
                f.areaCode = string.Empty;
                f.closed = false;
                f.carousel = false;
                f.carousel_caption = string.Empty;
                f.showvideo = false;
                f.moviecategory = "ZOther";

                // for movies!!! idur
                int mm = 0;
                try
                {
                    mm = CalculateTimeInMinutes1(f.duration);
                }
                catch (Exception ex)
                {
                    mm = 0;
                }
                // if(hh >= idur)

                if ((f.FeedId.Length > 0) && (mm >= idur))
                {
                    //string x = item.getUrl();
                    if (!CheckedListBox(x, list))
                    {
                        list.Add(f);
                    }
                }

            }
            labelx.Text = "Total: " + list.Count;
            //broadcast.SendToChannel("Channel1", string.Format("{0}", "updatedropdowns"));
            //IXDBroadcast bc
            LoadDataGrid(list, dgv);

        }

        private static bool CheckedListBox(string s, List<RSSFeed> z)
        {
            bool bFound = false;
            foreach (var item in z)
            {
                if (s == item.link)
                {
                    bFound = true;
                    break;
                }
            }
            return bFound;
        }

        public static void LoadDataGrid(List<RSSFeed> list, DataGridView dg)
        {
            dg.Rows.Clear();
            bool bb = false;

            for (int i = 0; i < list.Count; i++)
            {
                var thumb = LoadImageFromUrl(list[i].image); // <-- convert URL -> Image

                dg.Rows.Add(
                    bb,                     // X (checkbox)
                    thumb!,                  // image (Image, not string)
                    list[i].title,
                    list[i].author,
                    list[i].shortDescription,
                    list[i].description,
                    list[i].duration,
                    list[i].linkValue,
                    list[i].rank,
                    list[i].linkType
                );

                var lastRow = dg.Rows[dg.Rows.Count - 1];
                // persist the FULL image URL so we can retrieve it later
                lastRow.Cells[1].Tag = list[i].image;          // cell index 1 is your "image" column
                lastRow.Cells[1].ToolTipText = list[i].image;  // redundant storage for robustness
                if (dg.Columns.Contains("imageUrl"))
                {
                    lastRow.Cells["imageUrl"].Value = list[i].image; // hidden text column for the URL
                }

            }

            //labelx.Text = "Total: " + list.Count;

            // REMOVE the second foreach that was adding mismatched rows.
        }



        public static HashSet<char> _allowedChars = new HashSet<char>("0123456789numkMGHzVs%-.".ToArray());

        public static string FilterString(string str)
        {
            //string g = "Best \"},{\"text\":\"sports fights\",\"bold\":true},{\"text\":\" Part 2: https://www.youtube.com/watch?v=OSkUtJIcdZg Part 3-Â ...";
            //ã  ã  ã ä  ç Œã Œçµ ã  ã  ã  ã  ã  ã
            //ãƒ ãƒ ã  ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ

            //ÉªÊÊ Fá  ÊÊá  á   Yá  á œ Dá  á  É      ƒ    Ž   Ž

            string z1 = str.Replace("ã", "");
            string z2 = z1.Replace("ç", "");
            string z3 = z2.Replace("Œã", "");
            string z4 = z3.Replace("Œçµ", "");
            string z5 = z4.Replace("ãƒ", "");

            string z6 = z5.Replace("É", "");
            string z7 = z6.Replace("ª", "");
            string z8 = z7.Replace("Ž", "");


            string str1 = z8.Replace("\"},{\"text\":\"", "");
            string str2 = str1.Replace("\",\"bold\":true},{\"text\":\"", "");
            string str3 = str2.Replace("â", "");
            string str4 = str3.Replace("ï", "");
            string str5 = str4.Replace("ð", "");
            string str6 = str5.Replace("Ÿ", "");
            string str7 = str6.Replace("¤", "");
            string str8 = str7.Replace("¬", "");
            string str9 = str8.Replace("˜", "");
            string str10 = str9.Replace("INTENSEðŸ˜", "");

            Regex pattern = new Regex("[ðŸ¤¬˜]|[\n]{2}");
            pattern.Replace(str10, "\n");

            int index = str10.LastIndexOf("http");
            if (index > 0)
                str10 = str10.Substring(0, index);

            index = str10.LastIndexOf("www");
            if (index > 0)
                str10 = str10.Substring(0, index);

            string str11 = str10.Replace("http", "");
            str11 = str11.Trim();

            //StripUnicodeCharactersFromString()
            string str12 = str11.Replace("Â", "");
            str12 = str12.Trim();

            //const string reduceMultiSpace = @"[ ]{2,}";
            //string str13 = Regex.Replace(str12.Replace("\t", " "), reduceMultiSpace, " ");

            //char tab = '\u0009';
            //string str13 = str12.Replace(tab.ToString(), " ");

            //str13 = Regex.Replace(str13, @"\s+", string.Empty, RegexOptions.Multiline);

            // while (str12.Contains("  ")) str12 = str.Replace("  ", " ");
            //str12 = str12.ReduceWhitespace();
            //str13 = FilterWhiteSpaces(str13);

            //str13 = str13.Replace(@"\r\n", " ");
            //str13 = str13.Trim();

            try
            {
                string z = Regex.Replace(str12, @"[^\w\.@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                return z.Trim();
            }
            // If we timeout when replacing invalid characters, we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }

        }

        public static string FilterWhiteSpaces(string input)
        {
            if (input == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (i == 0 || c != ' ' || (c == ' ' && input[i - 1] != ' '))
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }

        public static String StripUnicodeCharactersFromString(string inputValue)
        {
            return Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(String.Empty), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(inputValue)));
        }

        private static int CalculateTimeInMinutes1(string input)
        {
            string[] parts = input.Split(':', ' ');
            if (parts.Length < 3)
                input = "00:" + input;

            DateTime time = DateTime.Parse(input, new CultureInfo("en-US"));
            return (int)(time - time.Date).TotalMinutes;
        }

        private static int CalculateTimeInMinutes2(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input));

            string[] parts = input.Split(':', ' ');

            if (parts.Length != 3)
                throw new ArgumentOutOfRangeException(nameof(input));

            if (parts[2].ToUpper() != "AM" && parts[2].ToUpper() != "PM")
                throw new ArgumentOutOfRangeException(nameof(input), "Missing AM/PM qualifier");


            if (!int.TryParse(parts[0], out int hours))
            {
                throw new ArgumentOutOfRangeException(nameof(input), "Invalid hours part.");
            }

            if (hours < 0 || hours > 12)
                throw new ArgumentOutOfRangeException(nameof(input), "Hours must be between 00 and 12 (inclusive)");

            // TODO: The same checks for minutes.
            int.TryParse(parts[1], out int minutes);

            int toPM = parts[2].ToUpper() == "PM" ? 12 : 0;
            int hoursInminutes = (toPM + (hours % 60)) * 60;
            int totalMinutes = hoursInminutes + minutes;
            return totalMinutes;
        }

        private static string GetLable(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "00:00:00";
            var builder = new StringBuilder();
            var arr = text.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 2)
                builder.Append("00:");
            var i = 0;
            foreach (var t in arr)
            {
                builder.Append(t.Length == 1 ? "0" + t : t);
                if (arr.Length - 1 != i++)
                    builder.Append(":");
            }
            return builder.ToString();
        }

        private static Image? LoadImageFromUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    var bytes = wc.DownloadData(url);
                    using (var ms = new System.IO.MemoryStream(bytes))
                        return Image.FromStream(ms);
                }
            }
            catch
            {
                return null; // or return a placeholder image if you have one
            }
        }

        #endregion ================ END TOP PANEL =======================================




        #region ============== BEGIN BOTTOM PANEL ===========================

        private void btnPrev2_Click(object? sender, EventArgs e)
        {
            //_start = _start - _max - 1;
            if (_start < _max + 1)
            {
                _start = 0;
            }
            else
            {
                _start = _start - _max - 1;
            }
            lblCounts2.Text = "Page: " + _start.ToString() + " Count: " + dgvFeeds.Rows.Count.ToString();
            string _cat = ddCategories.Text.ToString();
            string _subcat = ddSubCategories.Text.ToString();

            //lstDisplay.Clear();
            lstDisplay = new List<RSSFeed>();
            //lstDisplay = DataHelper.GetFeeds(_cat, _subcat, _start, _max);
            //if (lstDisplay == null || lstDisplay.Count < 1)
            //{
            //    MessageBox.Show("No Results!?");
            //    return;
            //}
            //dgvFeeds.Rows.Clear();
            //LoadDataGrid(lstDisplay, dgvFeeds, broadcast);
        }

        private void btnNext2_Click(object? sender, EventArgs e)
        {
            _start = _start + _max + 1;
            lblCounts2.Text = "Page: " + _start.ToString() + " Count: " + dgvFeeds.Rows.Count.ToString();
            string _cat = ddCategories.Text.ToString();
            string _subcat = ddSubCategories.Text.ToString();

            //lstDisplay.Clear();
            lstDisplay = new List<RSSFeed>();

            //lstDisplay = DataHelper.GetFeeds(_cat, _subcat, _start, _max);
            //if (lstDisplay == null || lstDisplay.Count < 1)
            //{
            //    MessageBox.Show("No Results!?");
            //    return;
            //}
            //dgvFeeds.Rows.Clear();
            //LoadDataGrid(lstDisplay, dgvFeeds, broadcast);
        }

        private void ddCategories2_SelectedIndexChanged(object? sender, EventArgs e)
        {
            s_category.Text = ddSCategories.SelectedItem!.ToString()!.Trim();
        }

        private void ddSubCategories2_SelectedIndexChanged(object? sender, EventArgs e)
        {
            s_subcategory.Text = ddSubCategories.SelectedItem!.ToString()!.Trim();
        }

        private void ddMCategories2_SelectedIndexChanged(object? sender, EventArgs e)
        {
            s_moviecategory.Text = ddMCategories2.SelectedItem!.ToString()!.Trim();
        }


        private void ddSLinkTypes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            s_linktype.Text = ddSLinkTypes.SelectedItem!.ToString()!.Trim();
        }

        private void ddSMovieCategories_SelectedIndexChanged(object? sender, EventArgs e)
        {
            s_moviecategory.Text = ddSMovieCategories.SelectedItem!.ToString()!.Trim();
        }

        private void ddSGroupCategories_SelectedIndexChanged(object? sender, EventArgs e)
        {
            s_groupcategory.Text = ddSGroupCategories.SelectedItem!.ToString()!.Trim();
        }

        private void ddSSubCategories_SelectedIndexChanged(object? sender, EventArgs e)
        {
            s_subcategory.Text = ddSSubCategories.SelectedItem!.ToString()!.Trim();
        }

        private void ddSCategories_SelectedIndexChanged(object? sender, EventArgs e)
        {
            s_category.Text = ddSCategories.SelectedItem!.ToString()!.Trim();
        }

        private void BtnFeeds2_Click(object? sender, EventArgs e)
        {
            _start = 0;

            string _cat = string.Empty;
            string _subcat = string.Empty;
            string _groupcat = "none";

            if (ddCategories.Items.Count > 0 && ddCategories.SelectedItem != null)
            {
                _cat = ddCategories.SelectedItem.ToString()!;
            }
            if (ddSubCategories.Items.Count > 0 && ddSubCategories.SelectedItem != null)
            {
                _subcat = ddSubCategories.SelectedItem.ToString()!;
            }
            if (ddGroupCategories.Items.Count > 0 && ddGroupCategories.SelectedItem != null)
            {
                _groupcat = ddGroupCategories.SelectedItem.ToString()!;
            }

            //string _moviecat = string.Empty;
            //if (_cat == "movies" || _cat == "All Movies")
            //{
            //    if (ddMCategories.SelectedItem != null)
            //    {
            //        _moviecat = ddMCategories.SelectedItem.ToString();
            //    }
            //}


            //lstDisplay.Clear();

            //if (_moviecat.ToUpper() == "ALL")
            //{
            //    _moviecat = string.Empty;
            //}

            _max = Convert.ToInt32(upDownRecords2.Value);
            int skip = 0;

            List<FeedItem> lstDisplay = Tubes.GetFeedsByCategory(_cat, _subcat, 500, skip);


            if (cbGroup2.Checked)
            {
                var distinctGroupCategoryCount = lstDisplay != null ? lstDisplay.Select(feed => feed.GroupCategory).Distinct().Count() : 0;
                lblGroups2.Text = "Groups: " + distinctGroupCategoryCount.ToString();
                //lstDisplay = (List<RSSFeed>)FilterLstDisplay(_groupcat);
            }

            if (lstDisplay != null)
            {
                dgvFeeds.Rows.Clear();
                //LoadDataGrid(lstDisplay, dgvFeeds, broadcast);
            }
            else
            {
                MessageBox.Show("No Results!?");
                return;
            }
        }


        private void dgvFeeds_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        #endregion =========== END BOTTOM PANEL =============================




    } // public partial class SplitEditor : UserControl

} // namespace AiNetStudio.WinGui.ControlScreens
