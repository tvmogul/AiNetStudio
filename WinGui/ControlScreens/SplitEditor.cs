//using AiNetStudio.DataAccess;
//using AiNetStudio.Models;
//using AiNetStudio.WinGui.Forms;
//using CustomControls;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using YouTubeScrapper;
//using static NumSharp.np;

//namespace AiNetStudio.WinGui.ControlScreens
//{
//    public partial class SplitEditor : UserControl
//    {
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        public WinGUIMain? MainFormReference { get; set; }

//        private System.Windows.Forms.Button btnToggle;
//        private bool _toggleDown = false;

//        // Keep a small visible strip of Panel1/Panel2 
//        // so the splitter (and button) are still visible.
//        // Adjust this to taste.
//        private const int SplitterReveal = 24;

//        // Hard cap so toggle never exceeds splitter height requested (e.g., 20px)
//        private const int MaxToggleHeight = 20;

//        // ===== New: designer-tweakable properties =====
//        private int _bottomModeReveal = 0; // height of Panel1 when showing Panel2 (arrow down)
//        private int _topModeReveal = 0;    // height of Panel2 when showing Panel1 (arrow up)
//        private int _toggleMaxHeight = 20; // cap for toggle height
//        private int _toggleButtonWidth = 24;
//        private bool _toggleOnSplitterDoubleClick = true;

//        [Browsable(true), Category("SplitEditor"), Description("Height (px) of Panel1 kept visible when showing Panel2 (down arrow). 0 hides Panel1 completely (splitter at top)."), DefaultValue(0)]
//        public int BottomModeReveal
//        {
//            get => _bottomModeReveal;
//            set { _bottomModeReveal = Math.Max(0, value); ApplyRevealForCurrentMode(); RepositionButton(); }
//        }

//        [Browsable(true), Category("SplitEditor"), Description("Height (px) of Panel2 kept visible when showing Panel1 (up arrow). 0 hides Panel2 completely (splitter at bottom)."), DefaultValue(0)]
//        public int TopModeReveal
//        {
//            get => _topModeReveal;
//            set { _topModeReveal = Math.Max(0, value); ApplyRevealForCurrentMode(); RepositionButton(); }
//        }

//        [Browsable(true), Category("SplitEditor"), Description("Maximum height (px) of the toggle button; also clamped by SplitterWidth."), DefaultValue(20)]
//        public int ToggleMaxHeight
//        {
//            get => _toggleMaxHeight;
//            set { _toggleMaxHeight = Math.Max(1, value); SyncButtonSizeToSplitter(); SetToggleIcon(_toggleDown); RepositionButton(); }
//        }

//        [Browsable(true), Category("SplitEditor"), Description("Width (px) of the toggle button."), DefaultValue(24)]
//        public int ToggleButtonWidth
//        {
//            get => _toggleButtonWidth;
//            set { _toggleButtonWidth = Math.Max(8, value); SyncButtonSizeToSplitter(); SetToggleIcon(_toggleDown); RepositionButton(); }
//        }

//        [Browsable(true), Category("SplitEditor"), Description("If true, double-clicking the splitter toggles between top/bottom views."), DefaultValue(true)]
//        public bool ToggleOnSplitterDoubleClick
//        {
//            get => _toggleOnSplitterDoubleClick;
//            set => _toggleOnSplitterDoubleClick = value;
//        }
//        // ===== End designer-tweakable properties =====

//        public SplitEditor() : this(null!) { }

//        public SplitEditor(WinGUIMain mainForm)
//        {
//            InitializeComponent();

//            MainFormReference = mainForm;

//            // allow full collapse both ways, but we will
//            // prefer "reveal" instead of true collapse
//            splitContainer1.Panel1MinSize = 0;
//            splitContainer1.Panel2MinSize = 0;

//            btnToggle = new System.Windows.Forms.Button
//            {
//                Text = "",
//                Width = _toggleButtonWidth,
//                Height = 50,
//                FlatStyle = FlatStyle.Flat
//            };
//            btnToggle.FlatAppearance.BorderSize = 0;
//            btnToggle.ImageAlign = ContentAlignment.MiddleCenter;
//            SyncButtonSizeToSplitter();         // ensure the button is not taller than the splitter (<= 20px)
//            SetToggleIcon(_toggleDown); // start with "up"
//            btnToggle.Click += btnToggle_Click;

//            // IMPORTANT: You cannot add directly to splitContainer1.Controls (it's read-only).
//            // Keep the button on this UserControl and position it over the splitter.
//            Controls.Add(btnToggle);

//            // reposition the button whenever the splitter moves or resizes
//            splitContainer1.SplitterMoved += (s, e) => RepositionButton();
//            splitContainer1.SizeChanged += (s, e) =>
//            {
//                // Maintain the reveal amount when the control resizes
//                ApplyRevealForCurrentMode();
//                SyncButtonSizeToSplitter();     // keep size consistent on resize
//                SetToggleIcon(_toggleDown);     // rescale icon to new size
//                RepositionButton();
//            };
//            splitContainer1.LocationChanged += (s, e) => RepositionButton();
//            this.SizeChanged += (s, e) =>
//            {
//                ApplyRevealForCurrentMode();
//                SyncButtonSizeToSplitter();
//                SetToggleIcon(_toggleDown);
//                RepositionButton();
//            };

//            // New: double-click on the splitter to toggle (optional via property)
//            splitContainer1.MouseDoubleClick += (s, e) =>
//            {
//                if (!_toggleOnSplitterDoubleClick) return;
//                if (splitContainer1.Orientation != Orientation.Horizontal) return;
//                if (splitContainer1.SplitterRectangle.Contains(e.Location))
//                {
//                    btnToggle_Click(btnToggle, EventArgs.Empty);
//                }
//            };

//            // Initialize with a visible splitter at the middle
//            splitContainer1.Panel1Collapsed = false;
//            splitContainer1.Panel2Collapsed = false;
//            splitContainer1.SplitterDistance = Math.Max(SplitterReveal, splitContainer1.Height / 2);

//            RepositionButton();

//            this.Load += splitEditor_Load;
//        }

//        private void splitEditor_Load(object? sender, EventArgs e)
//        {
//            this.cbSearchTypes.SelectedIndexChanged += cbSearchTypes_SelectedIndexChanged;
//            this.btnSearch.Click += btnSearch_Click!;
//            this.ddCategories.SelectedIndexChanged += ddCategories_SelectedIndexChanged;

//            this.btnAddChecked.Click += btnAddChecked_Click;
//            this.btnCreateJsonFile.Click += btnCreateJsonFile_Click;

//            this.btnRefreshGroups.Click += btnRefreshGroups_Click;

//            this.dgvVideos.CellContentClick += dgvVideos_CellContentClick;
//            this.dgvVideos.CellFormatting += dgvVideos_CellFormatting;
//            this.dgvVideos.CellMouseEnter += dgvVideos_CellMouseEnter;
//            this.dgvVideos.CellMouseLeave += dgvVideos_CellMouseLeave;

//            this.btnCheckAll.Click += btnCheckAll_Click;
//            this.btnUnCheckAll.Click += btnUnCheckAll_Click;

//            this.btnGetRumble.Click += btnGetRumble_Click;

//            InitializeDataGridView();

//            AddColumns();

//            cbSearchTypes.SelectedIndex = 0;

//            UpdateCategories();
//            UpdateSubCategories();
//            UpdateGroupCategories();
//            //UpdateMovieCategories();

//            ddMinimumLength.SelectedIndex = 0;
//        }


//        #region ============== BEGIN SPLITTER =============================

//        private void btnToggle_Click(object? sender, EventArgs e)
//        {
//            // Flip between panels:
//            // If current icon is "up" ( _toggleDown == false ), clicking shows BOTTOM panel (Panel2) with a small reveal for Panel1.
//            // If current icon is "down" ( _toggleDown == true ), clicking shows TOP panel (Panel1) with a small reveal for Panel2.

//            if (!_toggleDown)
//            {
//                // Show bottom panel "full", keep a configurable band of Panel1 (often 0) so the splitter is visible at top.
//                splitContainer1.Panel1Collapsed = false;
//                splitContainer1.Panel2Collapsed = false;

//                splitContainer1.SplitterDistance = ClampSplitterDistance(Math.Max(0, _bottomModeReveal));
//                _toggleDown = true; // switch icon to "down"
//            }
//            else
//            {
//                // Show top panel "full", keep a configurable band of Panel2 (often 0) so the splitter is visible at bottom.
//                splitContainer1.Panel1Collapsed = false;
//                splitContainer1.Panel2Collapsed = false;

//                int distanceForTop = Math.Max(0, splitContainer1.Height - splitContainer1.SplitterWidth - _topModeReveal);
//                splitContainer1.SplitterDistance = ClampSplitterDistance(distanceForTop);
//                _toggleDown = false; // switch icon to "up"
//            }

//            SetToggleIcon(_toggleDown);
//            RepositionButton();
//        }

//        private void SetToggleIcon(bool down)
//        {
//            try
//            {
//                // Use the ImageList ("up" / "down") and scale to the larger button via BackgroundImage
//                var key = down ? "down" : "up";
//                var img = imgListBlack?.Images[key];
//                if (img != null)
//                {
//                    // scale a tad smaller than button for padding
//                    var targetSize = new Size(Math.Max(1, btnToggle.Width - 6), Math.Max(1, btnToggle.Height - 6));
//                    btnToggle.BackgroundImage = new Bitmap(img, targetSize);
//                    btnToggle.BackgroundImageLayout = ImageLayout.Zoom;
//                    btnToggle.Image = null; // ensure BackgroundImage is what renders
//                }
//            }
//            catch { /* ignore scaling/image errors */ }
//        }

//        private void RepositionButton()
//        {
//            if (splitContainer1.Orientation != Orientation.Horizontal) return;

//            // Always center on the splitter bar within this UserControl's coordinate space
//            Rectangle splitRect = splitContainer1.SplitterRectangle;

//            // X centered over the SplitContainer
//            int x = splitContainer1.Left + splitRect.Left + (splitRect.Width - btnToggle.Width) / 2;

//            // Y centered on the splitter line
//            int y = splitContainer1.Top + splitRect.Top + (splitRect.Height - btnToggle.Height) / 2;

//            btnToggle.Left = Math.Max(0, x);
//            btnToggle.Top = Math.Max(0, y);

//            btnToggle.BringToFront();
//        }

//        private void ApplyRevealForCurrentMode()
//        {
//            if (splitContainer1.Orientation != Orientation.Horizontal) return;

//            // Re-apply the reveal rule when the control size changes so the splitter remains visible.
//            if (_toggleDown)
//            {
//                // Bottom mode: keep Panel1 at BottomModeReveal
//                splitContainer1.Panel1Collapsed = false;
//                splitContainer1.Panel2Collapsed = false;
//                splitContainer1.SplitterDistance = ClampSplitterDistance(Math.Max(0, _bottomModeReveal));
//            }
//            else
//            {
//                // Top mode: keep Panel2 at TopModeReveal
//                splitContainer1.Panel1Collapsed = false;
//                splitContainer1.Panel2Collapsed = false;
//                int distanceForTop = Math.Max(0, splitContainer1.Height - splitContainer1.SplitterWidth - _topModeReveal);
//                splitContainer1.SplitterDistance = ClampSplitterDistance(distanceForTop);
//            }
//        }

//        private int ClampSplitterDistance(int desired)
//        {
//            // Ensure the splitter distance stays within valid bounds
//            int min = splitContainer1.Panel1MinSize;
//            int max = Math.Max(0, splitContainer1.Height - splitContainer1.SplitterWidth - splitContainer1.Panel2MinSize);
//            if (desired < min) desired = min;
//            if (desired > max) desired = max;
//            return desired;
//        }

//        private void SyncButtonSizeToSplitter()
//        {
//            // Ensure the button height is never larger than the splitter height and never exceeds the property cap
//            int splitterH = Math.Max(1, splitContainer1.SplitterWidth);
//            int targetH = Math.Min(splitterH - 2, _toggleMaxHeight); // leave 1px margins
//            if (targetH < 12) targetH = Math.Min(12, splitterH);    // keep it reasonably clickable

//            // Use designer-tweakable width
//            int targetW = Math.Max(8, _toggleButtonWidth);

//            btnToggle.Size = new Size(targetW, targetH);
//        }

//        #endregion ============== END SPLITTER =============================


//        #region ============== BEGIN TOP PANEL =============================

//        private void cbSearchTypes_SelectedIndexChanged(object? sender, EventArgs e)
//        {
//            string[] arrLabels = { "Enter General search Term", "Enter Channel ID", "Enter Playlist Name" };
//            string[] arrText = { "Fights in Sports", "UCU8Xw_KewvcC3LV3Qv--JZg", "Bushido" };
//        }

//        private void btnSearch_Click(object sender, EventArgs e)
//        {
//            List<RumbleVideoInfo> lstRumble = new List<RumbleVideoInfo>();
//            string _search = txtSearch.Text;
//            int _max = Convert.ToInt32(upDown.Value);

//            if (_search.Length < 1)
//            {
//                MessageBox.Show("Can't Be Empty!");
//                return;
//            }
//            if (_max < 1)
//            {
//                MessageBox.Show("Can't Be Zero!");
//                return;
//            }

//            labelEx1.Text = "Total: 0";
//            dgvVideos.Rows.Clear();

//            string s = cbSearchTypes.SelectedItem!.ToString()!;

//            string _duration = ddMinimumLength.SelectedItem!.ToString()!;

//            if (s == "YouTube: Search General")
//            {
//                Search(_search, _max, dgvVideos, labelEx1, _duration);
//            }
//            else if (s == "YouTube: Search Channel")
//            {
//                //Search_ChannelItems(_search, _max, dgvVideos, labelEx1, broadcast);
//            }
//            else if (s == "YouTube: Search Playlist")
//            {
//                //Search_Playlist(_search, _max, dgvVideos, labelEx1, broadcast);
//            }
//            else if (s == "YouTube: Download Video")
//            {
//                //DownloadYouTubeByVideoID(_search);
//            }
//            else if (s == "Rumble: Search")
//            {
//                //await Task.Run(() =>
//                //{
//                //    _ = SearchForRumbleVideosAsync(_search, _max, dgvVideos, labelEx1, broadcast, _duration);
//                //});
//            }
//        }

//        private void ddCategories_SelectedIndexChanged(object? sender, EventArgs e)
//        {
//            ////if (ddCategories.Text.Trim() == "All Movies" || ddCategories.Text.Trim() == "movies")
//            ////{
//            ////    lblMovieCategory.Visible = true;
//            ////    ddMovieCategories.Visible = true;
//            ////}
//            ////else
//            ////{
//            ////    lblMovieCategory.Visible = false;
//            ////    ddMovieCategories.Visible = false;
//            ////}
//            //txtCategory.Text = string.Empty;

//            UpdateSubCategories();

//        }

//        private void UpdateCategories()
//        {
//            Tubes.EnsureDatabase(seedIfEmpty: true);
//            var cats = Tubes.GetCategories();
//            ddCategories.Items.Clear();
//            foreach (var c in cats) ddCategories.Items.Add(c);
//            if (ddCategories.Items.Count > 0) ddCategories.SelectedIndex = 0;
//        }

//        private void UpdateSubCategories()
//        {
//            var cat = ddCategories.SelectedItem?.ToString() ?? "";
//            var subs = Tubes.GetSubcategories(cat);

//            ddSubCategories!.Items.Clear();
//            ddSubCategories.Items.Add(""); // allow 'no subcategory' option
//            foreach (var s in subs) ddSubCategories.Items.Add(s);
//            ddSubCategories.SelectedIndex = 0;
//        }

//        private void UpdateGroupCategories()
//        {
//            List<FeedGroupCategories> result = new List<FeedGroupCategories>();

//            var cat = ddCategories.SelectedItem?.ToString() ?? "";
//            var subs = Tubes.GetSubcategories(cat);

//            var qqq = Tubes.GetGroupCategories(cat, subs);

//            ddGroupCategories.Items.Clear();
//            //ddGroupCategories.Items.Add(""); // allow 'no subcategory' option
//            foreach (var s in subs) ddGroupCategories.Items.Add(s);
//            ddSubCategories.SelectedIndex = 0;
//        }

//        private void btnAddChecked_Click(object? sender, EventArgs e)
//        {
//            int _checked = 0;

//            // Ensure any in-progress edits are committed so checkbox values are current
//            dgvVideos.EndEdit();

//            foreach (DataGridViewRow row in dgvVideos.Rows)
//            {
//                if (row.IsNewRow) continue;
//                bool isChecked = (row.Cells["X"].Value as bool?) == true; // use named checkbox column
//                if (isChecked)
//                {
//                    _checked++;
//                }
//            }

//            if (_checked < 1)
//            {
//                MessageBox.Show("You MUST Check Rows You Want to Save!");
//                return;
//            }

//            //lblStatus.Text = "Processing: 0 / " + _checked + " Checked Videos";

//            // Assuming 'yourComboBox' is your SerGioComboBox instance
//            string typedValue = ddCategories.Text;
//            MessageBox.Show(typedValue);

//            string _category = ddCategories.Text.Trim();
//            string _subcategory = ddSubCategories.Text.Trim();
//            string _groupcategory = ddGroupCategories.Text.Trim();

//            if (_category.Length < 1)
//            {
//                MessageBox.Show("You MUST Select or Enter A Value for Category!");
//                return;
//            }

//            if (_subcategory.Length < 1)
//            {
//                MessageBox.Show("You MUST Select or Enter A Value for Sub Category!");
//                return;
//            }

//            if (_groupcategory.Length < 1)
//            {
//                MessageBox.Show("You MUST Select or Enter A Value for Group Category!");
//                return;
//            }

//            //string s = "Category: " + _category + "\nSubcategory: " + _subcategory + "\nGroup Category: " + _groupcategory;
//            //MessageBox.Show(s);

//            //string _moviecategory = ddMovieCategories.SelectedItem.ToString();
//            //if (_category == "All Movies")
//            //{

//            //}

//            //string _subcategory = txtSubCategory.Text.Trim();
//            ////if (_subcategory.Length < 1)
//            ////{
//            ////    _subcategory = ddCategories.SelectedItem.Col1.Trim();
//            ////}

//            //string _groupcategory = txtGroupCategory.Text.Trim();

//            //string str = "Do You Want to Save Checked Items with Category: " + _category;
//            //if (ddCategories.SelectedItem.Col1.Trim() == "movies")
//            //{
//            //    str = str + "\r\nAnd Movie Category: " + ddMovieCategories.SelectedItem.ToString();
//            //}

//            //DialogResult dialogResult = MessageBox.Show(str, "Question", MessageBoxButtons.YesNo);
//            //if (dialogResult == DialogResult.Yes)
//            //{
//            //}
//            //else if (dialogResult == DialogResult.No)
//            //{
//            //    return;
//            //}

//            List<ZFeedItem> list = new List<ZFeedItem>();

//            foreach (DataGridViewRow row in dgvVideos.Rows)
//            {
//                if (row.IsNewRow) continue;

//                bool isChecked = (row.Cells["X"].Value as bool?) == true; // use named checkbox column
//                if (isChecked)
//                {
//                    var f = new ZFeedItem();
//                    f.FeedId = string.Empty;    // row.Cells[2].Value.ToString();   //2 FeedId Guid id = Guid.NewGuid();
//                    f.Category = _category;
//                    f.SubCategory = _subcategory;
//                    f.CatSub = f.Category + "_" + f.SubCategory;
//                    f.GroupCategory = _groupcategory;
//                    f.Title = row.Cells["title"].Value?.ToString() ?? string.Empty;    //3 title
//                    f.Author = row.Cells["author"].Value?.ToString() ?? string.Empty;   //4 author

//                    var linkTypeVal = row.Cells["linkType"].Value?.ToString() ?? string.Empty;
//                    var linkValueVal = row.Cells["linkValue"].Value?.ToString() ?? string.Empty;

//                    // --- begin: robust linkType/linkValue normalization ---
//                    linkTypeVal = linkTypeVal.Trim();
//                    linkValueVal = linkValueVal.Trim();

//                    // If linkValue contains a full YouTube URL, extract the ID
//                    string ExtractYouTubeId(string url)
//                    {
//                        try
//                        {
//                            // common patterns: https://www.youtube.com/watch?v=ID, https://youtu.be/ID
//                            if (url.Contains("youtube.com", StringComparison.OrdinalIgnoreCase))
//                            {
//                                var uri = new Uri(url);
//                                var q = System.Web.HttpUtility.ParseQueryString(uri.Query);
//                                var v = q.Get("v");
//                                if (!string.IsNullOrWhiteSpace(v)) return v;
//                                // sometimes /embed/ID
//                                var seg = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
//                                int embedIdx = Array.FindIndex(seg, s => s.Equals("embed", StringComparison.OrdinalIgnoreCase));
//                                if (embedIdx >= 0 && embedIdx + 1 < seg.Length) return seg[embedIdx + 1];
//                            }
//                            if (url.Contains("youtu.be", StringComparison.OrdinalIgnoreCase))
//                            {
//                                var uri = new Uri(url);
//                                var seg = uri.AbsolutePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
//                                if (seg.Length > 0) return seg[0];
//                            }
//                        }
//                        catch { }
//                        return string.Empty;
//                    }

//                    if (linkValueVal.StartsWith("http", StringComparison.OrdinalIgnoreCase))
//                    {
//                        var maybeId = ExtractYouTubeId(linkValueVal);
//                        if (!string.IsNullOrWhiteSpace(maybeId))
//                            linkValueVal = maybeId;
//                    }

//                    // If linkType is empty but linkValue is present, assume youtube (matches your search loader)
//                    if (string.IsNullOrWhiteSpace(linkTypeVal) && !string.IsNullOrWhiteSpace(linkValueVal))
//                    {
//                        linkTypeVal = "youtube";
//                    }
//                    // --- end: robust linkType/linkValue normalization ---

//                    if (string.Equals(linkTypeVal, "rumble", StringComparison.OrdinalIgnoreCase))
//                    {
//                        f.LinkType = "rumble";
//                        f.LinkValue = linkValueVal;
//                        f.Link = "https://rumble.com/embed/" + linkValueVal;
//                    }
//                    else
//                    {
//                        f.LinkType = "youtube";
//                        f.LinkValue = linkValueVal;
//                        f.Link = "https://www.youtube.com/watch?v=" + linkValueVal;
//                    }

//                    f.ShortDescription = row.Cells["shortDescription"].Value?.ToString() ?? string.Empty; //5 shortDescription
//                    f.Description = row.Cells["description"].Value?.ToString() ?? string.Empty;  //6 descript6ion
//                    f.BodyLinks = string.Empty;

//                    // Handle image column (DataGridViewImageColumn). Prefer a URL stored in a companion place; otherwise construct if possible.
//                    var imgCell = row.Cells["image"];
//                    string? imageUrl = null;

//                    // 1) If you added a hidden text column to hold the URL, use it (no exception if it doesn't exist)
//                    if (dgvVideos.Columns.Contains("imageUrl"))
//                    {
//                        imageUrl = row.Cells["imageUrl"].Value?.ToString();
//                    }

//                    // 2) Try Tag on the image cell
//                    if (string.IsNullOrWhiteSpace(imageUrl))
//                        imageUrl = imgCell.Tag as string;

//                    // 3) Try ToolTipText on the image cell
//                    if (string.IsNullOrWhiteSpace(imageUrl))
//                        imageUrl = imgCell.ToolTipText;

//                    // 4) If the Image object itself has a Tag, use it
//                    if (string.IsNullOrWhiteSpace(imageUrl) && imgCell.Value is Image imgWithTag && imgWithTag?.Tag is string imgTag && !string.IsNullOrWhiteSpace(imgTag))
//                        imageUrl = imgTag;

//                    // 5) If the image cell actually holds a string (URL), use it
//                    if (string.IsNullOrWhiteSpace(imageUrl) && imgCell.Value is string sImg)
//                        imageUrl = sImg;

//                    // 6) Fallbacks based on linkType/linkValue (ensures FULL URL)
//                    if (string.IsNullOrWhiteSpace(imageUrl))
//                    {
//                        if (string.Equals(linkTypeVal, "youtube", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(linkValueVal))
//                        {
//                            imageUrl = "https://img.youtube.com/vi/" + linkValueVal + "/hqdefault.jpg";
//                        }
//                        else if (string.Equals(linkTypeVal, "rumble", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(linkValueVal))
//                        {
//                            // Generic rumble thumbnail fallback; replace if you persist a precise URL elsewhere
//                            imageUrl = "https://i1.rumble.com/" + linkValueVal + ".jpg";
//                        }
//                    }

//                    f.Image = imageUrl ?? string.Empty;

//                    //if ((f.image == null) || (f.image == ""))
//                    //{
//                    //    f.image = "https://img.youtube.com/vi/" + f.linkValue + "/hqdefault.jpg";
//                    //}

//                    f.Rank = Convert.ToInt32(row.Cells["rank"].Value ?? 0);
//                    f.PublishedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

//                    if (f.Category == "All Movies")
//                    {
//                        f.MovieCategory = f.SubCategory;
//                    }
//                    else
//                    {
//                        f.MovieCategory = string.Empty;
//                    }
//                    f.Duration = row.Cells["duration"].Value?.ToString() ?? string.Empty; //7 duration
//                    list.Add(f);
//                }

//            } // end foreach

//            var sqqq = "";

//            //UpdateServer(list, dgvVideos, labelEx1);
//        }




//        // "POCO" stands for plain old CLR object.A POCO is a.NET type that doesn't depend 
//        // on any framework-specific types, for example, through inheritance or attributes.
//        public void UpdateServer(List<ZFeedItem> videos, DataGridView dgv, LabelEx labelx)
//        {
//            foreach (ZFeedItem video in videos)
//            {
//                //PostData(video, dgv, labelx);
//                Tubes.UpdateFeeds(video, dgv, labelx);
//                Thread.Sleep(100);

//            }
//            //txtCategory.Text = string.Empty;
//            Thread.Sleep(1000);
//            UpdateCategories();
//            //UpdateMovieCategories();
//            MessageBox.Show("Completed!");
//        }


//        private void btnCreateJsonFile_Click(object? sender, EventArgs e)
//        {

//        }

//        private void btnRefreshGroups_Click(object? sender, EventArgs e)
//        {

//        }

//        private void dgvVideos_CellContentClick(object? sender, DataGridViewCellEventArgs e)
//        {

//        }

//        private void dgvVideos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
//        {

//        }

//        private void dgvVideos_CellMouseEnter(object? sender, DataGridViewCellEventArgs e)
//        {

//        }

//        private void dgvVideos_CellMouseLeave(object? sender, DataGridViewCellEventArgs e)
//        {

//        }
//        private void btnCheckAll_Click(object? sender, EventArgs e)
//        {
//            foreach (DataGridViewRow row in dgvVideos.Rows)
//            {
//                ((DataGridViewCheckBoxCell)row.Cells[0]).Value = true;
//            }
//        }

//        private void btnUnCheckAll_Click(object? sender, EventArgs e)
//        {
//            foreach (DataGridViewRow row in dgvVideos.Rows)
//            {
//                ((DataGridViewCheckBoxCell)row.Cells[0]).Value = false;
//            }
//        }
//        private void btnGetRumble_Click(object? sender, EventArgs e)
//        {

//        }

//        private void InitializeDataGridView()
//        {
//            //System.Windows.Forms.DataGridViewCellStyle styleHeader = new System.Windows.Forms.DataGridViewCellStyle();
//            //System.Windows.Forms.DataGridViewCellStyle styleRow = new System.Windows.Forms.DataGridViewCellStyle();
//            //System.Windows.Forms.DataGridViewCellStyle styleAlternating = new System.Windows.Forms.DataGridViewCellStyle();
//            //System.Windows.Forms.DataGridViewCellStyle styleSelected = new System.Windows.Forms.DataGridViewCellStyle();
//            //System.Drawing.Font DefaultStateFont = new System.Drawing.Font("Trebuchet MS", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            //styleRow.Font = DefaultStateFont;
//            //this.dgvVideos.RowsDefaultCellStyle = styleRow;

//            // Initialize basic DataGridView properties.
//            //dgvVideos.Dock = DockStyle.Fill;
//            //dgvVideos.BackgroundColor = Color.FromArgb(29, 29, 29);
//            //dgvVideos.BackgroundColor = Color.FromArgb(255, 255, 255);
//            //dgvVideos.BorderStyle = BorderStyle.None;
//            //dgvVideos.RowHeadersVisible = false;

//            //dgvVideos.AllowUserToAddRows = false;
//            //dgvVideos.AllowUserToDeleteRows = false;
//            //dgvVideos.AllowUserToOrderColumns = true;
//            //dgvVideos.ReadOnly = true;
//            //dgvVideos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            //dgvVideos.MultiSelect = false;
//            //dgvVideos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
//            //dgvVideos.AllowUserToResizeColumns = true;
//            ////dgvVideos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
//            //dgvVideos.AllowUserToResizeRows = false;
//            //dgvVideos.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

//            // Set the selection background color for all the cells.
//            //dgvVideos.DefaultCellStyle.SelectionBackColor = Color.DarkGray;
//            //dgvVideos.DefaultCellStyle.SelectionForeColor = Color.White;

//            // Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
//            // value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
//            dgvVideos.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty;

//            // Set the background color for all rows and for alternating rows. 
//            // The value for alternating rows overrides the value for all rows. 
//            //dgvVideos.RowsDefaultCellStyle.BackColor = Color.Black;
//            //dgvVideos.RowsDefaultCellStyle.BackColor = Color.White;
//            //dgvVideos.RowsDefaultCellStyle.ForeColor = Color.Black;
//            //dgvVideos.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // Color.FromArgb(30, 30, 30);
//            //dgvVideos.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

//            dgvVideos.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(29, 29, 29);

//            //splitContainer1.BackColor = Color.FromArgb(29, 29, 29);
//            //splitContainer1.Panel1.BackColor = Color.FromArgb(29, 29, 29);
//            //splitContainer1.Panel2.BackColor = Color.FromArgb(29, 29, 29);


//        }

//        public void AddColumns()
//        {
//            dgvVideos.DataSource = null;
//            dgvVideos.Rows.Clear();
//            dgvVideos.Columns.Clear();
//            //dgvVideos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

//            dgvVideos.ColumnCount = 8;
//            dgvVideos.RowTemplate.Height = 60;

//            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
//            checkColumn.Name = "X";
//            checkColumn.HeaderText = "X";
//            checkColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            checkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            checkColumn.Width = 30;
//            checkColumn.ReadOnly = false;
//            //if the datagridview is resized (on form resize) the checkbox won't take 
//            //up too much; value is relative to the other columns' fill values
//            checkColumn.FillWeight = 18;
//            //checkColumn.DefaultCellStyle.Font = new Font("Tahoma", 36);
//            // Ensure checkbox stores bools only (avoid nulls in normal use)
//            checkColumn.ValueType = typeof(bool);
//            checkColumn.ThreeState = false;
//            checkColumn.TrueValue = true;
//            checkColumn.FalseValue = false;
//            dgvVideos.Columns.Insert(0, checkColumn);

//            DataGridViewImageColumn dgvImageColumn = new DataGridViewImageColumn();
//            dgvImageColumn.Name = "image";
//            dgvImageColumn.Width = 100;
//            dgvImageColumn.HeaderText = "Image";
//            dgvImageColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvImageColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvImageColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
//            dgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
//            dgvVideos.Columns.Insert(1, dgvImageColumn);

//            dgvVideos.Columns[2].Name = "title";
//            dgvVideos.Columns[2].HeaderText = "Title";
//            dgvVideos.Columns[2].Width = 160;
//            //dgvVideos.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
//            dgvVideos.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvVideos.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

//            dgvVideos.Columns[3].Name = "author";
//            dgvVideos.Columns[3].HeaderText = "Author";
//            dgvVideos.Columns[3].Width = 120;
//            dgvVideos.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvVideos.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

//            dgvVideos.Columns[4].Name = "shortDescription";
//            dgvVideos.Columns[4].HeaderText = "Short Description";
//            dgvVideos.Columns[4].Width = 260;
//            dgvVideos.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvVideos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

//            dgvVideos.Columns[5].Name = "description";
//            dgvVideos.Columns[5].HeaderText = "Description";
//            dgvVideos.Columns[5].Width = 260;
//            dgvVideos.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvVideos.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

//            dgvVideos.Columns[6].Name = "duration";
//            dgvVideos.Columns[6].HeaderText = "Duration";
//            dgvVideos.Columns[6].Width = 50;
//            //dgvVideos.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
//            dgvVideos.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvVideos.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

//            dgvVideos.Columns[7].Name = "linkValue";
//            dgvVideos.Columns[7].HeaderText = "Link Value";
//            dgvVideos.Columns[7].Width = 150;
//            dgvVideos.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
//            dgvVideos.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvVideos.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

//            dgvVideos.Columns[8].Name = "rank";
//            dgvVideos.Columns[8].HeaderText = "Rank";
//            dgvVideos.Columns[8].Width = 80;
//            dgvVideos.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvVideos.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

//            dgvVideos.Columns[9].Name = "linkType";
//            dgvVideos.Columns[9].HeaderText = "Link Type";
//            dgvVideos.Columns[9].Width = 80;
//            dgvVideos.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
//            dgvVideos.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvVideos.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//        }

//        static async void Search(string search, int max, DataGridView dgv, LabelEx labelx, string dur)
//        {
//            //Log.setMode(false);

//            int idur = Convert.ToInt32(dur);

//            string querystring = search;
//            int querypages = max;

//            VideoSearch videos = new VideoSearch();
//            var items = await videos.GetVideos(querystring, querypages);
//            //var items = await videos.GetVideosPaged(querystring, querypages);

//            List<RSSFeed> list = new List<RSSFeed>();

//            foreach (var item in items)
//            {
//                RSSFeed f = new RSSFeed();
//                f.title = FilterString(item.getTitle());
//                f.author = item.getAuthor();

//                //////////////////////////////////////////////////////////
//                string _description = FilterString(item.getDescription());
//                if (_description.Length < 1)
//                {
//                    _description = item.getTitle();
//                }
//                f.description = _description;
//                f.shortDescription = _description;
//                //////////////////////////////////////////////////////////

//                // for movies!!!
//                f.duration = item.getDuration();

//                string x = item.getUrl();
//                char[] equal = new char[1] { '=' };
//                String[] strlist = x.Split(new char[] { '=' });

//                if (strlist[1].Length > 0)
//                {
//                    f.linkValue = strlist[1];
//                }
//                else
//                {
//                    f.linkValue = string.Empty;
//                }

//                f.image = item.getThumbnail();
//                string _views = item.getViewCount();
//                int parsedViews;
//                if (!string.IsNullOrEmpty(_views) && int.TryParse(_views.Replace(".", ""), out parsedViews))
//                {
//                    f.rank = parsedViews;
//                }
//                else
//                {
//                    f.rank = 0; // Default value if _views is null, empty, or not a valid int
//                }


//                Guid id = Guid.NewGuid();
//                f.FeedId = id.ToString();
//                f.category = "unknown";
//                //f.title = sdr.GetString("title");
//                //f.author = sdr.GetString("author");
//                f.link = item.getUrl();
//                f.linkType = "youtube";
//                //f.linkValue = sdr.GetString("linkValue");
//                f.bodyLinks = string.Empty;
//                //f.image = sdr.GetString("image");
//                f.warnings = string.Empty;
//                f.sideeffects = string.Empty;
//                f.dosage = string.Empty;
//                f.anticoagulant = false;
//                f.carcinogenic = false;
//                f.hypoglycemic = false;
//                f.liverdamage = false;
//                f.kidneydamage = false;
//                //f.rank = sdr.GetInt32("rank");
//                f.publishedDate = DateTime.UtcNow;
//                f.beginDate = DateTime.UtcNow;
//                f.endDate = DateTime.UtcNow;
//                f.city = string.Empty;
//                f.state = string.Empty;
//                f.postalCode = string.Empty;
//                f.country = string.Empty;
//                f.areaCode = string.Empty;
//                f.closed = false;
//                f.carousel = false;
//                f.carousel_caption = string.Empty;
//                f.showvideo = false;
//                f.moviecategory = "ZOther";

//                // for movies!!! idur
//                int mm = 0;
//                try
//                {
//                    mm = CalculateTimeInMinutes1(f.duration);
//                }
//                catch (Exception ex)
//                {
//                    mm = 0;
//                }
//                // if(hh >= idur)

//                if ((f.FeedId.Length > 0) && (mm >= idur))
//                {
//                    //string x = item.getUrl();
//                    if (!CheckedListBox(x, list))
//                    {
//                        list.Add(f);
//                    }
//                }

//            }
//            labelx.Text = "Total: " + list.Count;
//            //broadcast.SendToChannel("Channel1", string.Format("{0}", "updatedropdowns"));
//            //IXDBroadcast bc
//            LoadDataGrid(list, dgv);

//        }

//        private static bool CheckedListBox(string s, List<RSSFeed> z)
//        {
//            bool bFound = false;
//            foreach (var item in z)
//            {
//                if (s == item.link)
//                {
//                    bFound = true;
//                    break;
//                }
//            }
//            return bFound;
//        }

//        public static void LoadDataGrid(List<RSSFeed> list, DataGridView dg)
//        {
//            dg.Rows.Clear();
//            bool bb = false;

//            for (int i = 0; i < list.Count; i++)
//            {
//                var thumb = LoadImageFromUrl(list[i].image); // <-- convert URL -> Image

//                dg.Rows.Add(
//                    bb,                     // X (checkbox)
//                    thumb!,                  // image (Image, not string)
//                    list[i].title,
//                    list[i].author,
//                    list[i].shortDescription,
//                    list[i].description,
//                    list[i].duration,
//                    list[i].linkValue,
//                    list[i].rank,
//                    list[i].linkType
//                );

//                var lastRow = dg.Rows[dg.Rows.Count - 1];
//                // persist the FULL image URL so we can retrieve it later
//                lastRow.Cells[1].Tag = list[i].image;          // cell index 1 is your "image" column
//                lastRow.Cells[1].ToolTipText = list[i].image;  // redundant storage for robustness

//            }

//            // REMOVE the second foreach that was adding mismatched rows.
//        }



//        public static HashSet<char> _allowedChars = new HashSet<char>("0123456789numkMGHzVs%-.".ToArray());

//        public static string FilterString(string str)
//        {
//            //string g = "Best \"},{\"text\":\"sports fights\",\"bold\":true},{\"text\":\" Part 2: https://www.youtube.com/watch?v=OSkUtJIcdZg Part 3-Â ...";
//            //ã  ã  ã ä  ç Œã Œçµ ã  ã  ã  ã  ã  ã
//            //ãƒ ãƒ ã  ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ ãƒ

//            //ÉªÊÊ Fá  ÊÊá  á   Yá  á œ Dá  á  É      ƒ    Ž   Ž

//            string z1 = str.Replace("ã", "");
//            string z2 = z1.Replace("ç", "");
//            string z3 = z2.Replace("Œã", "");
//            string z4 = z3.Replace("Œçµ", "");
//            string z5 = z4.Replace("ãƒ", "");

//            string z6 = z5.Replace("É", "");
//            string z7 = z6.Replace("ª", "");
//            string z8 = z7.Replace("Ž", "");


//            string str1 = z8.Replace("\"},{\"text\":\"", "");
//            string str2 = str1.Replace("\",\"bold\":true},{\"text\":\"", "");
//            string str3 = str2.Replace("â", "");
//            string str4 = str3.Replace("ï", "");
//            string str5 = str4.Replace("ð", "");
//            string str6 = str5.Replace("Ÿ", "");
//            string str7 = str6.Replace("¤", "");
//            string str8 = str7.Replace("¬", "");
//            string str9 = str8.Replace("˜", "");
//            string str10 = str9.Replace("INTENSEðŸ˜", "");

//            Regex pattern = new Regex("[ðŸ¤¬˜]|[\n]{2}");
//            pattern.Replace(str10, "\n");

//            int index = str10.LastIndexOf("http");
//            if (index > 0)
//                str10 = str10.Substring(0, index);

//            index = str10.LastIndexOf("www");
//            if (index > 0)
//                str10 = str10.Substring(0, index);

//            string str11 = str10.Replace("http", "");
//            str11 = str11.Trim();

//            //StripUnicodeCharactersFromString()
//            string str12 = str11.Replace("Â", "");
//            str12 = str12.Trim();

//            //const string reduceMultiSpace = @"[ ]{2,}";
//            //string str13 = Regex.Replace(str12.Replace("\t", " "), reduceMultiSpace, " ");

//            //char tab = '\u0009';
//            //string str13 = str12.Replace(tab.ToString(), " ");

//            //str13 = Regex.Replace(str13, @"\s+", string.Empty, RegexOptions.Multiline);

//            // while (str12.Contains("  ")) str12 = str.Replace("  ", " ");
//            //str12 = str12.ReduceWhitespace();
//            //str13 = FilterWhiteSpaces(str13);

//            //str13 = str13.Replace(@"\r\n", " ");
//            //str13 = str13.Trim();

//            try
//            {
//                string z = Regex.Replace(str12, @"[^\w\.@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5));
//                return z.Trim();
//            }
//            // If we timeout when replacing invalid characters, we should return Empty.
//            catch (RegexMatchTimeoutException)
//            {
//                return String.Empty;
//            }

//        }

//        public static string FilterWhiteSpaces(string input)
//        {
//            if (input == null)
//                return string.Empty;

//            StringBuilder stringBuilder = new StringBuilder(input.Length);
//            for (int i = 0; i < input.Length; i++)
//            {
//                char c = input[i];
//                if (i == 0 || c != ' ' || (c == ' ' && input[i - 1] != ' '))
//                    stringBuilder.Append(c);
//            }
//            return stringBuilder.ToString();
//        }

//        public static String StripUnicodeCharactersFromString(string inputValue)
//        {
//            return Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(String.Empty), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(inputValue)));
//        }

//        private static int CalculateTimeInMinutes1(string input)
//        {
//            string[] parts = input.Split(':', ' ');
//            if (parts.Length < 3)
//                input = "00:" + input;

//            DateTime time = DateTime.Parse(input, new CultureInfo("en-US"));
//            return (int)(time - time.Date).TotalMinutes;
//        }

//        private static int CalculateTimeInMinutes2(string input)
//        {
//            if (string.IsNullOrWhiteSpace(input))
//                throw new ArgumentNullException(nameof(input));

//            string[] parts = input.Split(':', ' ');

//            if (parts.Length != 3)
//                throw new ArgumentOutOfRangeException(nameof(input));

//            if (parts[2].ToUpper() != "AM" && parts[2].ToUpper() != "PM")
//                throw new ArgumentOutOfRangeException(nameof(input), "Missing AM/PM qualifier");


//            if (!int.TryParse(parts[0], out int hours))
//            {
//                throw new ArgumentOutOfRangeException(nameof(input), "Invalid hours part.");
//            }

//            if (hours < 0 || hours > 12)
//                throw new ArgumentOutOfRangeException(nameof(input), "Hours must be between 00 and 12 (inclusive)");

//            // TODO: The same checks for minutes.
//            int.TryParse(parts[1], out int minutes);

//            int toPM = parts[2].ToUpper() == "PM" ? 12 : 0;
//            int hoursInminutes = (toPM + (hours % 60)) * 60;
//            int totalMinutes = hoursInminutes + minutes;
//            return totalMinutes;
//        }

//        private static string GetLable(string text)
//        {
//            if (string.IsNullOrEmpty(text))
//                return "00:00:00";
//            var builder = new StringBuilder();
//            var arr = text.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
//            if (arr.Length == 2)
//                builder.Append("00:");
//            var i = 0;
//            foreach (var t in arr)
//            {
//                builder.Append(t.Length == 1 ? "0" + t : t);
//                if (arr.Length - 1 != i++)
//                    builder.Append(":");
//            }
//            return builder.ToString();
//        }

//        private static Image? LoadImageFromUrl(string? url)
//        {
//            if (string.IsNullOrWhiteSpace(url)) return null;
//            try
//            {
//                using (var wc = new System.Net.WebClient())
//                {
//                    var bytes = wc.DownloadData(url);
//                    using (var ms = new System.IO.MemoryStream(bytes))
//                        return Image.FromStream(ms);
//                }
//            }
//            catch
//            {
//                return null; // or return a placeholder image if you have one
//            }
//        }

//        #endregion ================ END TOP PANEL =======================================




//        #region ============== BEGIN BOTTOM PANEL ===========================


//        #endregion =========== END BOTTOM PANEL =============================




//    } // public partial class SplitEditor : UserControl

//} // namespace AiNetStudio.WinGui.ControlScreens




using AiNetStudio.DataAccess;
using AiNetStudio.Models;
using AiNetStudio.WinGui.Forms;
using CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YouTubeScrapper;
using static NumSharp.np;

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

        public SplitEditor() : this(null!) { }

        public SplitEditor(WinGUIMain mainForm)
        {
            InitializeComponent();

            MainFormReference = mainForm;

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
        }

        private void splitEditor_Load(object? sender, EventArgs e)
        {
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

            labelEx1.Text = "Total: 0";
            dgvVideos.Rows.Clear();

            string s = cbSearchTypes.SelectedItem!.ToString()!;

            string _duration = ddMinimumLength.SelectedItem!.ToString()!;

            if (s == "YouTube: Search General")
            {
                Search(_search, _max, dgvVideos, labelEx1, _duration);
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

            List<ZFeedItem> list = new List<ZFeedItem>();

            foreach (DataGridViewRow row in dgvVideos.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = (row.Cells["X"].Value as bool?) == true; // use named checkbox column
                if (isChecked)
                {
                    var f = new ZFeedItem();
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

            UpdateServer(list, dgvVideos, labelEx1);
        }




        // "POCO" stands for plain old CLR object.A POCO is a.NET type that doesn't depend 
        // on any framework-specific types, for example, through inheritance or attributes.
        public void UpdateServer(List<ZFeedItem> videos, DataGridView dgv, LabelEx labelx)
        {
            foreach (ZFeedItem video in videos)
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


        private void btnCreateJsonFile_Click(object? sender, EventArgs e)
        {

        }

        private void btnRefreshGroups_Click(object? sender, EventArgs e)
        {

        }

        private void dgvVideos_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvVideos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void dgvVideos_CellMouseEnter(object? sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvVideos_CellMouseLeave(object? sender, DataGridViewCellEventArgs e)
        {

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

        }

        private void InitializeDataGridView()
        {
            //System.Windows.Forms.DataGridViewCellStyle styleHeader = new System.Windows.Forms.DataGridViewCellStyle();
            //System.Windows.Forms.DataGridViewCellStyle styleRow = new System.Windows.Forms.DataGridViewCellStyle();
            //System.Windows.Forms.DataGridViewCellStyle styleAlternating = new System.Windows.Forms.DataGridViewCellStyle();
            //System.Windows.Forms.DataGridViewCellStyle styleSelected = new System.Windows.Forms.DataGridViewCellStyle();
            //System.Drawing.Font DefaultStateFont = new System.Drawing.Font("Trebuchet MS", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //styleRow.Font = DefaultStateFont;
            //this.dgvVideos.RowsDefaultCellStyle = styleRow;

            // Initialize basic DataGridView properties.
            //dgvVideos.Dock = DockStyle.Fill;
            //dgvVideos.BackgroundColor = Color.FromArgb(29, 29, 29);
            //dgvVideos.BackgroundColor = Color.FromArgb(255, 255, 255);
            //dgvVideos.BorderStyle = BorderStyle.None;
            //dgvVideos.RowHeadersVisible = false;

            //dgvVideos.AllowUserToAddRows = false;
            //dgvVideos.AllowUserToDeleteRows = false;
            //dgvVideos.AllowUserToOrderColumns = true;
            //dgvVideos.ReadOnly = true;
            //dgvVideos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgvVideos.MultiSelect = false;
            //dgvVideos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dgvVideos.AllowUserToResizeColumns = true;
            ////dgvVideos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dgvVideos.AllowUserToResizeRows = false;
            //dgvVideos.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Set the selection background color for all the cells.
            //dgvVideos.DefaultCellStyle.SelectionBackColor = Color.DarkGray;
            //dgvVideos.DefaultCellStyle.SelectionForeColor = Color.White;

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

            dgvVideos.Columns[2].Name = "title";
            dgvVideos.Columns[2].HeaderText = "Title";
            dgvVideos.Columns[2].Width = 160;
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
            dgvVideos.Columns[4].Width = 260;
            dgvVideos.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVideos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            dgvVideos.Columns[5].Name = "description";
            dgvVideos.Columns[5].HeaderText = "Description";
            dgvVideos.Columns[5].Width = 260;
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

        static async void Search(string search, int max, DataGridView dgv, LabelEx labelx, string dur)
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


        #endregion =========== END BOTTOM PANEL =============================




    } // public partial class SplitEditor : UserControl

} // namespace AiNetStudio.WinGui.ControlScreens
