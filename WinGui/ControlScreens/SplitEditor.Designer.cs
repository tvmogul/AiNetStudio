namespace AiNetStudio.WinGui.ControlScreens
{
    partial class SplitEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplitEditor));
            splitContainer1 = new SplitContainer();
            dgvVideos = new DataGridView();
            gradientPanel1 = new CustomControls.GradientPanel();
            lblCount = new Label();
            txtURLRumble = new TextBox();
            btnGetRumble = new Button();
            btnRefreshGroups = new Button();
            btnCreateJsonFile = new Button();
            btnAddChecked = new Button();
            ddGroupCategories = new ComboBox();
            btnGetDescription = new Button();
            btnUnCheckAll = new Button();
            btnCheckAll = new Button();
            label5 = new Label();
            btnSearch = new Button();
            labelEx2 = new CustomControls.LabelEx();
            labelx = new CustomControls.LabelEx();
            lblSub = new Label();
            ddSubCategories = new ComboBox();
            lblCat = new Label();
            label4 = new Label();
            ddCategories = new ComboBox();
            ddMinimumLength = new ComboBox();
            btnLoad = new CustomControls.RoundButton();
            label3 = new Label();
            label2 = new Label();
            upDown = new NumericUpDown();
            txtSearch = new TextBox();
            label1 = new Label();
            cbSearchTypes = new ComboBox();
            splitContainer2 = new SplitContainer();
            dgvFeeds = new DataGridView();
            label26 = new Label();
            pbImage = new PictureBox();
            s_image = new TextBox();
            btnGetCoverImage = new Button();
            label25 = new Label();
            label24 = new Label();
            ddSImages = new ComboBox();
            ddSFolders = new ComboBox();
            s_bodylinks = new TextBox();
            label23 = new Label();
            label22 = new Label();
            s_description = new TextBox();
            s_title = new TextBox();
            s_FeedId = new TextBox();
            label20 = new Label();
            label21 = new Label();
            s_shortdescription = new TextBox();
            label19 = new Label();
            label18 = new Label();
            label17 = new Label();
            s_rank = new TextBox();
            label16 = new Label();
            s_author = new TextBox();
            label15 = new Label();
            s_duration = new TextBox();
            s_link = new TextBox();
            btnLink2 = new Button();
            s_linkvalue = new TextBox();
            btnLinkValue2 = new Button();
            ddSLinkTypes = new ComboBox();
            ddSMovieCategories = new ComboBox();
            ddSGroupCategories = new ComboBox();
            ddSSubCategories = new ComboBox();
            ddSCategories = new ComboBox();
            label10 = new Label();
            s_linktype = new TextBox();
            label9 = new Label();
            s_moviecategory = new TextBox();
            label8 = new Label();
            s_groupcategory = new TextBox();
            label7 = new Label();
            s_subcategory = new TextBox();
            label6 = new Label();
            s_category = new TextBox();
            gradientPanel2 = new CustomControls.GradientPanel();
            txtSearch2 = new TextBox();
            ddSearch2 = new ComboBox();
            btnSearchVideoTitles2 = new Button();
            btnClean2 = new Button();
            btnJSON2 = new Button();
            btnGetDescription2 = new Button();
            btnDelete2 = new Button();
            btnNew2 = new Button();
            btnSave2 = new Button();
            cbGroup2 = new CheckBox();
            upDownRecords2 = new NumericUpDown();
            lblCounts2 = new Label();
            ddMCategories2 = new ComboBox();
            label13 = new Label();
            label14 = new Label();
            label12 = new Label();
            label11 = new Label();
            ddGroupCategories2 = new ComboBox();
            ddSubCategories2 = new ComboBox();
            ddCategories2 = new ComboBox();
            btnNext2 = new Button();
            btnPrev2 = new Button();
            btnFeeds2 = new Button();
            imgListBlack = new ImageList(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVideos).BeginInit();
            gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)upDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFeeds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbImage).BeginInit();
            gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)upDownRecords2).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dgvVideos);
            splitContainer1.Panel1.Controls.Add(gradientPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1215, 900);
            splitContainer1.SplitterDistance = 306;
            splitContainer1.SplitterWidth = 20;
            splitContainer1.TabIndex = 0;
            // 
            // dgvVideos
            // 
            dgvVideos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvVideos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVideos.Location = new Point(22, 157);
            dgvVideos.Name = "dgvVideos";
            dgvVideos.RowHeadersWidth = 51;
            dgvVideos.Size = new Size(1174, 129);
            dgvVideos.TabIndex = 2;
            // 
            // gradientPanel1
            // 
            gradientPanel1.BackColor = Color.IndianRed;
            gradientPanel1.BackgroundImage = (Image)resources.GetObject("gradientPanel1.BackgroundImage");
            gradientPanel1.BackgroundImageLayout = ImageLayout.Stretch;
            gradientPanel1.Controls.Add(lblCount);
            gradientPanel1.Controls.Add(txtURLRumble);
            gradientPanel1.Controls.Add(btnGetRumble);
            gradientPanel1.Controls.Add(btnRefreshGroups);
            gradientPanel1.Controls.Add(btnCreateJsonFile);
            gradientPanel1.Controls.Add(btnAddChecked);
            gradientPanel1.Controls.Add(ddGroupCategories);
            gradientPanel1.Controls.Add(btnGetDescription);
            gradientPanel1.Controls.Add(btnUnCheckAll);
            gradientPanel1.Controls.Add(btnCheckAll);
            gradientPanel1.Controls.Add(label5);
            gradientPanel1.Controls.Add(btnSearch);
            gradientPanel1.Controls.Add(labelEx2);
            gradientPanel1.Controls.Add(labelx);
            gradientPanel1.Controls.Add(lblSub);
            gradientPanel1.Controls.Add(ddSubCategories);
            gradientPanel1.Controls.Add(lblCat);
            gradientPanel1.Controls.Add(label4);
            gradientPanel1.Controls.Add(ddCategories);
            gradientPanel1.Controls.Add(ddMinimumLength);
            gradientPanel1.Controls.Add(btnLoad);
            gradientPanel1.Controls.Add(label3);
            gradientPanel1.Controls.Add(label2);
            gradientPanel1.Controls.Add(upDown);
            gradientPanel1.Controls.Add(txtSearch);
            gradientPanel1.Controls.Add(label1);
            gradientPanel1.Controls.Add(cbSearchTypes);
            gradientPanel1.Dock = DockStyle.Top;
            gradientPanel1.GradientBottom = Color.Transparent;
            gradientPanel1.GradientTop = Color.Transparent;
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(1215, 147);
            gradientPanel1.TabIndex = 1;
            // 
            // lblCount
            // 
            lblCount.BackColor = Color.Transparent;
            lblCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCount.ForeColor = Color.White;
            lblCount.Location = new Point(22, 103);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(155, 23);
            lblCount.TabIndex = 33;
            lblCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtURLRumble
            // 
            txtURLRumble.Location = new Point(448, 96);
            txtURLRumble.Name = "txtURLRumble";
            txtURLRumble.Size = new Size(158, 27);
            txtURLRumble.TabIndex = 32;
            // 
            // btnGetRumble
            // 
            btnGetRumble.BackgroundImage = (Image)resources.GetObject("btnGetRumble.BackgroundImage");
            btnGetRumble.BackgroundImageLayout = ImageLayout.Stretch;
            btnGetRumble.FlatStyle = FlatStyle.Popup;
            btnGetRumble.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnGetRumble.ForeColor = Color.White;
            btnGetRumble.Location = new Point(343, 92);
            btnGetRumble.Name = "btnGetRumble";
            btnGetRumble.Size = new Size(96, 40);
            btnGetRumble.TabIndex = 31;
            btnGetRumble.Text = "Rumble";
            btnGetRumble.UseVisualStyleBackColor = true;
            // 
            // btnRefreshGroups
            // 
            btnRefreshGroups.BackgroundImage = (Image)resources.GetObject("btnRefreshGroups.BackgroundImage");
            btnRefreshGroups.BackgroundImageLayout = ImageLayout.Stretch;
            btnRefreshGroups.Cursor = Cursors.Hand;
            btnRefreshGroups.FlatStyle = FlatStyle.Popup;
            btnRefreshGroups.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnRefreshGroups.ForeColor = Color.White;
            btnRefreshGroups.Location = new Point(936, 91);
            btnRefreshGroups.Name = "btnRefreshGroups";
            btnRefreshGroups.Size = new Size(101, 40);
            btnRefreshGroups.TabIndex = 30;
            btnRefreshGroups.Text = "Refresh";
            btnRefreshGroups.UseVisualStyleBackColor = true;
            // 
            // btnCreateJsonFile
            // 
            btnCreateJsonFile.BackgroundImage = (Image)resources.GetObject("btnCreateJsonFile.BackgroundImage");
            btnCreateJsonFile.BackgroundImageLayout = ImageLayout.Stretch;
            btnCreateJsonFile.Cursor = Cursors.Hand;
            btnCreateJsonFile.FlatStyle = FlatStyle.Popup;
            btnCreateJsonFile.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCreateJsonFile.ForeColor = Color.White;
            btnCreateJsonFile.Location = new Point(748, 91);
            btnCreateJsonFile.Name = "btnCreateJsonFile";
            btnCreateJsonFile.Size = new Size(72, 40);
            btnCreateJsonFile.TabIndex = 29;
            btnCreateJsonFile.Text = "JSON";
            btnCreateJsonFile.UseVisualStyleBackColor = true;
            // 
            // btnAddChecked
            // 
            btnAddChecked.BackgroundImage = (Image)resources.GetObject("btnAddChecked.BackgroundImage");
            btnAddChecked.BackgroundImageLayout = ImageLayout.Stretch;
            btnAddChecked.Cursor = Cursors.Hand;
            btnAddChecked.FlatStyle = FlatStyle.Popup;
            btnAddChecked.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnAddChecked.ForeColor = Color.White;
            btnAddChecked.Location = new Point(647, 92);
            btnAddChecked.Name = "btnAddChecked";
            btnAddChecked.Size = new Size(85, 40);
            btnAddChecked.TabIndex = 28;
            btnAddChecked.Text = "Save";
            btnAddChecked.UseVisualStyleBackColor = true;
            // 
            // ddGroupCategories
            // 
            ddGroupCategories.FormattingEnabled = true;
            ddGroupCategories.Location = new Point(933, 56);
            ddGroupCategories.Name = "ddGroupCategories";
            ddGroupCategories.Size = new Size(145, 28);
            ddGroupCategories.TabIndex = 27;
            // 
            // btnGetDescription
            // 
            btnGetDescription.BackColor = Color.Transparent;
            btnGetDescription.BackgroundImage = (Image)resources.GetObject("btnGetDescription.BackgroundImage");
            btnGetDescription.BackgroundImageLayout = ImageLayout.Stretch;
            btnGetDescription.Cursor = Cursors.Hand;
            btnGetDescription.FlatAppearance.BorderSize = 0;
            btnGetDescription.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnGetDescription.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnGetDescription.FlatStyle = FlatStyle.Flat;
            btnGetDescription.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnGetDescription.ForeColor = Color.White;
            btnGetDescription.Location = new Point(286, 92);
            btnGetDescription.Name = "btnGetDescription";
            btnGetDescription.Size = new Size(46, 42);
            btnGetDescription.TabIndex = 26;
            btnGetDescription.UseVisualStyleBackColor = false;
            // 
            // btnUnCheckAll
            // 
            btnUnCheckAll.BackColor = Color.Transparent;
            btnUnCheckAll.BackgroundImage = (Image)resources.GetObject("btnUnCheckAll.BackgroundImage");
            btnUnCheckAll.BackgroundImageLayout = ImageLayout.Stretch;
            btnUnCheckAll.Cursor = Cursors.Hand;
            btnUnCheckAll.FlatAppearance.BorderSize = 0;
            btnUnCheckAll.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnUnCheckAll.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnUnCheckAll.FlatStyle = FlatStyle.Flat;
            btnUnCheckAll.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnUnCheckAll.ForeColor = Color.White;
            btnUnCheckAll.Location = new Point(237, 92);
            btnUnCheckAll.Name = "btnUnCheckAll";
            btnUnCheckAll.Size = new Size(39, 40);
            btnUnCheckAll.TabIndex = 25;
            btnUnCheckAll.UseVisualStyleBackColor = false;
            // 
            // btnCheckAll
            // 
            btnCheckAll.BackColor = Color.Transparent;
            btnCheckAll.BackgroundImage = (Image)resources.GetObject("btnCheckAll.BackgroundImage");
            btnCheckAll.BackgroundImageLayout = ImageLayout.Stretch;
            btnCheckAll.Cursor = Cursors.Hand;
            btnCheckAll.FlatAppearance.BorderSize = 0;
            btnCheckAll.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCheckAll.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnCheckAll.FlatStyle = FlatStyle.Flat;
            btnCheckAll.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCheckAll.ForeColor = Color.White;
            btnCheckAll.Location = new Point(187, 92);
            btnCheckAll.Name = "btnCheckAll";
            btnCheckAll.Size = new Size(41, 40);
            btnCheckAll.TabIndex = 24;
            btnCheckAll.TextImageRelation = TextImageRelation.ImageAboveText;
            btnCheckAll.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.Location = new Point(938, 33);
            label5.Name = "label5";
            label5.Size = new Size(133, 20);
            label5.TabIndex = 23;
            label5.Text = "Group";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            btnSearch.BackgroundImage = (Image)resources.GetObject("btnSearch.BackgroundImage");
            btnSearch.BackgroundImageLayout = ImageLayout.Stretch;
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatStyle = FlatStyle.Popup;
            btnSearch.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(521, 50);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(85, 40);
            btnSearch.TabIndex = 21;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // labelEx2
            // 
            labelEx2.BackColor = Color.Transparent;
            labelEx2.Font = new Font("Arial", 14F, FontStyle.Bold);
            labelEx2.ForeColor = Color.White;
            labelEx2.Location = new Point(452, 3);
            labelEx2.Name = "labelEx2";
            labelEx2.ShadowColor = Color.Black;
            labelEx2.Size = new Size(270, 26);
            labelEx2.TabIndex = 18;
            labelEx2.Text = "Search for Videos";
            labelEx2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelx
            // 
            labelx.BackColor = Color.Transparent;
            labelx.Font = new Font("Comic Sans MS", 10F, FontStyle.Bold);
            labelx.ForeColor = Color.White;
            labelx.Location = new Point(826, 96);
            labelx.Name = "labelx";
            labelx.ShadowColor = Color.Black;
            labelx.Size = new Size(101, 27);
            labelx.TabIndex = 17;
            labelx.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSub
            // 
            lblSub.BackColor = Color.Transparent;
            lblSub.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSub.ForeColor = Color.White;
            lblSub.Location = new Point(795, 33);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(133, 20);
            lblSub.TabIndex = 3;
            lblSub.Text = "Sub Category";
            // 
            // ddSubCategories
            // 
            ddSubCategories.DropDownHeight = 140;
            ddSubCategories.FormattingEnabled = true;
            ddSubCategories.IntegralHeight = false;
            ddSubCategories.Location = new Point(791, 57);
            ddSubCategories.Name = "ddSubCategories";
            ddSubCategories.Size = new Size(136, 28);
            ddSubCategories.TabIndex = 4;
            // 
            // lblCat
            // 
            lblCat.BackColor = Color.Transparent;
            lblCat.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCat.ForeColor = Color.White;
            lblCat.Location = new Point(652, 31);
            lblCat.Name = "lblCat";
            lblCat.Size = new Size(126, 20);
            lblCat.TabIndex = 5;
            lblCat.Text = "Category";
            lblCat.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(451, 31);
            label4.Name = "label4";
            label4.Size = new Size(66, 23);
            label4.TabIndex = 13;
            label4.Text = "Pages";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ddCategories
            // 
            ddCategories.DropDownHeight = 140;
            ddCategories.FormattingEnabled = true;
            ddCategories.IntegralHeight = false;
            ddCategories.Location = new Point(647, 56);
            ddCategories.Name = "ddCategories";
            ddCategories.Size = new Size(136, 28);
            ddCategories.TabIndex = 2;
            // 
            // ddMinimumLength
            // 
            ddMinimumLength.DropDownStyle = ComboBoxStyle.DropDownList;
            ddMinimumLength.DropDownWidth = 160;
            ddMinimumLength.FormattingEnabled = true;
            ddMinimumLength.Items.AddRange(new object[] { "0", "26", "30", "58", "60", "90" });
            ddMinimumLength.Location = new Point(343, 57);
            ddMinimumLength.Name = "ddMinimumLength";
            ddMinimumLength.Size = new Size(103, 28);
            ddMinimumLength.TabIndex = 12;
            // 
            // btnLoad
            // 
            btnLoad.BackgroundImage = (Image)resources.GetObject("btnLoad.BackgroundImage");
            btnLoad.BackgroundImageLayout = ImageLayout.Stretch;
            btnLoad.FlatStyle = FlatStyle.Popup;
            btnLoad.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLoad.ForeColor = Color.White;
            btnLoad.Location = new Point(1091, 50);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(74, 41);
            btnLoad.TabIndex = 1;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Visible = false;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(344, 31);
            label3.Name = "label3";
            label3.Size = new Size(104, 23);
            label3.TabIndex = 11;
            label3.Text = "Min Length";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(187, 31);
            label2.Name = "label2";
            label2.Size = new Size(142, 23);
            label2.TabIndex = 10;
            label2.Text = "Enter Search";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // upDown
            // 
            upDown.Location = new Point(454, 57);
            upDown.Name = "upDown";
            upDown.Size = new Size(58, 27);
            upDown.TabIndex = 9;
            upDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(186, 59);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(148, 27);
            txtSearch.TabIndex = 8;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(26, 31);
            label1.Name = "label1";
            label1.Size = new Size(155, 23);
            label1.TabIndex = 7;
            label1.Text = "Tpe of Search";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbSearchTypes
            // 
            cbSearchTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSearchTypes.DropDownWidth = 200;
            cbSearchTypes.FormattingEnabled = true;
            cbSearchTypes.Items.AddRange(new object[] { "YouTube: Search General", "YouTube: Search Channel", "YouTube: Search Playlist", "Rumble: Search" });
            cbSearchTypes.Location = new Point(22, 59);
            cbSearchTypes.Name = "cbSearchTypes";
            cbSearchTypes.Size = new Size(158, 28);
            cbSearchTypes.TabIndex = 6;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.FixedPanel = FixedPanel.Panel1;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(dgvFeeds);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(label26);
            splitContainer2.Panel2.Controls.Add(pbImage);
            splitContainer2.Panel2.Controls.Add(s_image);
            splitContainer2.Panel2.Controls.Add(btnGetCoverImage);
            splitContainer2.Panel2.Controls.Add(label25);
            splitContainer2.Panel2.Controls.Add(label24);
            splitContainer2.Panel2.Controls.Add(ddSImages);
            splitContainer2.Panel2.Controls.Add(ddSFolders);
            splitContainer2.Panel2.Controls.Add(s_bodylinks);
            splitContainer2.Panel2.Controls.Add(label23);
            splitContainer2.Panel2.Controls.Add(label22);
            splitContainer2.Panel2.Controls.Add(s_description);
            splitContainer2.Panel2.Controls.Add(s_title);
            splitContainer2.Panel2.Controls.Add(s_FeedId);
            splitContainer2.Panel2.Controls.Add(label20);
            splitContainer2.Panel2.Controls.Add(label21);
            splitContainer2.Panel2.Controls.Add(s_shortdescription);
            splitContainer2.Panel2.Controls.Add(label19);
            splitContainer2.Panel2.Controls.Add(label18);
            splitContainer2.Panel2.Controls.Add(label17);
            splitContainer2.Panel2.Controls.Add(s_rank);
            splitContainer2.Panel2.Controls.Add(label16);
            splitContainer2.Panel2.Controls.Add(s_author);
            splitContainer2.Panel2.Controls.Add(label15);
            splitContainer2.Panel2.Controls.Add(s_duration);
            splitContainer2.Panel2.Controls.Add(s_link);
            splitContainer2.Panel2.Controls.Add(btnLink2);
            splitContainer2.Panel2.Controls.Add(s_linkvalue);
            splitContainer2.Panel2.Controls.Add(btnLinkValue2);
            splitContainer2.Panel2.Controls.Add(ddSLinkTypes);
            splitContainer2.Panel2.Controls.Add(ddSMovieCategories);
            splitContainer2.Panel2.Controls.Add(ddSGroupCategories);
            splitContainer2.Panel2.Controls.Add(ddSSubCategories);
            splitContainer2.Panel2.Controls.Add(ddSCategories);
            splitContainer2.Panel2.Controls.Add(label10);
            splitContainer2.Panel2.Controls.Add(s_linktype);
            splitContainer2.Panel2.Controls.Add(label9);
            splitContainer2.Panel2.Controls.Add(s_moviecategory);
            splitContainer2.Panel2.Controls.Add(label8);
            splitContainer2.Panel2.Controls.Add(s_groupcategory);
            splitContainer2.Panel2.Controls.Add(label7);
            splitContainer2.Panel2.Controls.Add(s_subcategory);
            splitContainer2.Panel2.Controls.Add(label6);
            splitContainer2.Panel2.Controls.Add(s_category);
            splitContainer2.Panel2.Controls.Add(gradientPanel2);
            splitContainer2.Size = new Size(1215, 574);
            splitContainer2.SplitterDistance = 320;
            splitContainer2.TabIndex = 0;
            // 
            // dgvFeeds
            // 
            dgvFeeds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFeeds.Dock = DockStyle.Fill;
            dgvFeeds.Location = new Point(0, 0);
            dgvFeeds.Name = "dgvFeeds";
            dgvFeeds.RowHeadersWidth = 51;
            dgvFeeds.Size = new Size(320, 574);
            dgvFeeds.TabIndex = 3;
            // 
            // label26
            // 
            label26.BackColor = Color.Transparent;
            label26.Location = new Point(485, 472);
            label26.Name = "label26";
            label26.Size = new Size(99, 25);
            label26.TabIndex = 78;
            label26.Text = "image path";
            label26.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pbImage
            // 
            pbImage.BackColor = Color.Transparent;
            pbImage.BackgroundImageLayout = ImageLayout.Stretch;
            pbImage.Location = new Point(714, 403);
            pbImage.Name = "pbImage";
            pbImage.Size = new Size(125, 122);
            pbImage.TabIndex = 77;
            pbImage.TabStop = false;
            // 
            // s_image
            // 
            s_image.Location = new Point(485, 499);
            s_image.Name = "s_image";
            s_image.Size = new Size(207, 27);
            s_image.TabIndex = 76;
            // 
            // btnGetCoverImage
            // 
            btnGetCoverImage.BackgroundImage = (Image)resources.GetObject("btnGetCoverImage.BackgroundImage");
            btnGetCoverImage.BackgroundImageLayout = ImageLayout.Stretch;
            btnGetCoverImage.Cursor = Cursors.Hand;
            btnGetCoverImage.FlatStyle = FlatStyle.Popup;
            btnGetCoverImage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGetCoverImage.ForeColor = Color.White;
            btnGetCoverImage.Location = new Point(590, 469);
            btnGetCoverImage.Name = "btnGetCoverImage";
            btnGetCoverImage.Size = new Size(100, 28);
            btnGetCoverImage.TabIndex = 75;
            btnGetCoverImage.Text = "Get Cover";
            btnGetCoverImage.UseVisualStyleBackColor = true;
            // 
            // label25
            // 
            label25.BackColor = Color.Transparent;
            label25.Location = new Point(409, 436);
            label25.Name = "label25";
            label25.Size = new Size(70, 25);
            label25.TabIndex = 73;
            label25.Text = "Images";
            label25.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            label24.BackColor = Color.Transparent;
            label24.Location = new Point(414, 402);
            label24.Name = "label24";
            label24.Size = new Size(65, 25);
            label24.TabIndex = 72;
            label24.Text = "folders";
            label24.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddSImages
            // 
            ddSImages.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSImages.FormattingEnabled = true;
            ddSImages.Location = new Point(485, 435);
            ddSImages.Name = "ddSImages";
            ddSImages.Size = new Size(207, 28);
            ddSImages.TabIndex = 71;
            // 
            // ddSFolders
            // 
            ddSFolders.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSFolders.FormattingEnabled = true;
            ddSFolders.Location = new Point(485, 401);
            ddSFolders.Name = "ddSFolders";
            ddSFolders.Size = new Size(207, 28);
            ddSFolders.TabIndex = 70;
            // 
            // s_bodylinks
            // 
            s_bodylinks.Location = new Point(485, 331);
            s_bodylinks.Multiline = true;
            s_bodylinks.Name = "s_bodylinks";
            s_bodylinks.Size = new Size(352, 61);
            s_bodylinks.TabIndex = 69;
            // 
            // label23
            // 
            label23.BackColor = Color.Transparent;
            label23.Location = new Point(485, 262);
            label23.Name = "label23";
            label23.Size = new Size(339, 66);
            label23.TabIndex = 68;
            label23.Text = "study | web|url, \r\nvideo | embed_youtube |\r\nhttp://www.youtube.com/embed/UbNi1lQBU0I\r\n";
            label23.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            label22.BackColor = Color.Transparent;
            label22.Location = new Point(390, 331);
            label22.Name = "label22";
            label22.Size = new Size(89, 25);
            label22.TabIndex = 67;
            label22.Text = "body links";
            label22.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_description
            // 
            s_description.Location = new Point(485, 191);
            s_description.Multiline = true;
            s_description.Name = "s_description";
            s_description.Size = new Size(354, 61);
            s_description.TabIndex = 65;
            // 
            // s_title
            // 
            s_title.Location = new Point(132, 166);
            s_title.Name = "s_title";
            s_title.Size = new Size(234, 27);
            s_title.TabIndex = 61;
            // 
            // s_FeedId
            // 
            s_FeedId.Font = new Font("Segoe UI", 8F);
            s_FeedId.Location = new Point(132, 133);
            s_FeedId.Name = "s_FeedId";
            s_FeedId.Size = new Size(235, 25);
            s_FeedId.TabIndex = 59;
            // 
            // label20
            // 
            label20.BackColor = Color.Transparent;
            label20.Location = new Point(400, 194);
            label20.Name = "label20";
            label20.Size = new Size(79, 25);
            label20.TabIndex = 66;
            label20.Text = "Description";
            label20.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            label21.BackColor = Color.Transparent;
            label21.Location = new Point(385, 135);
            label21.Name = "label21";
            label21.Size = new Size(94, 44);
            label21.TabIndex = 64;
            label21.Text = "Short\r\nDescription";
            label21.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_shortdescription
            // 
            s_shortdescription.Location = new Point(485, 133);
            s_shortdescription.Multiline = true;
            s_shortdescription.Name = "s_shortdescription";
            s_shortdescription.Size = new Size(354, 50);
            s_shortdescription.TabIndex = 63;
            // 
            // label19
            // 
            label19.BackColor = Color.Transparent;
            label19.ImageAlign = ContentAlignment.MiddleRight;
            label19.Location = new Point(33, 165);
            label19.Name = "label19";
            label19.Size = new Size(91, 25);
            label19.TabIndex = 62;
            label19.Text = "Video Title";
            label19.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            label18.BackColor = Color.Transparent;
            label18.ImageAlign = ContentAlignment.MiddleRight;
            label18.Location = new Point(30, 133);
            label18.Name = "label18";
            label18.Size = new Size(96, 25);
            label18.TabIndex = 60;
            label18.Text = "Feed ID";
            label18.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            label17.Location = new Point(6, 501);
            label17.Name = "label17";
            label17.Size = new Size(120, 25);
            label17.TabIndex = 58;
            label17.Text = "Rank/Views";
            label17.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_rank
            // 
            s_rank.Location = new Point(131, 501);
            s_rank.Name = "s_rank";
            s_rank.Size = new Size(235, 27);
            s_rank.TabIndex = 57;
            // 
            // label16
            // 
            label16.Location = new Point(6, 468);
            label16.Name = "label16";
            label16.Size = new Size(120, 25);
            label16.TabIndex = 56;
            label16.Text = "Author";
            label16.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_author
            // 
            s_author.Location = new Point(131, 468);
            s_author.Name = "s_author";
            s_author.Size = new Size(235, 27);
            s_author.TabIndex = 55;
            // 
            // label15
            // 
            label15.Location = new Point(6, 435);
            label15.Name = "label15";
            label15.Size = new Size(120, 25);
            label15.TabIndex = 54;
            label15.Text = "Duration";
            label15.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_duration
            // 
            s_duration.Location = new Point(131, 435);
            s_duration.Name = "s_duration";
            s_duration.Size = new Size(235, 27);
            s_duration.TabIndex = 53;
            // 
            // s_link
            // 
            s_link.Location = new Point(131, 402);
            s_link.Name = "s_link";
            s_link.Size = new Size(235, 27);
            s_link.TabIndex = 52;
            // 
            // btnLink2
            // 
            btnLink2.BackgroundImage = (Image)resources.GetObject("btnLink2.BackgroundImage");
            btnLink2.BackgroundImageLayout = ImageLayout.Stretch;
            btnLink2.Cursor = Cursors.Hand;
            btnLink2.FlatStyle = FlatStyle.Popup;
            btnLink2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLink2.ForeColor = Color.White;
            btnLink2.Location = new Point(18, 403);
            btnLink2.Name = "btnLink2";
            btnLink2.Size = new Size(107, 28);
            btnLink2.TabIndex = 51;
            btnLink2.Text = "Link";
            btnLink2.UseVisualStyleBackColor = true;
            // 
            // s_linkvalue
            // 
            s_linkvalue.Location = new Point(132, 369);
            s_linkvalue.Name = "s_linkvalue";
            s_linkvalue.Size = new Size(235, 27);
            s_linkvalue.TabIndex = 50;
            // 
            // btnLinkValue2
            // 
            btnLinkValue2.BackgroundImage = (Image)resources.GetObject("btnLinkValue2.BackgroundImage");
            btnLinkValue2.BackgroundImageLayout = ImageLayout.Stretch;
            btnLinkValue2.Cursor = Cursors.Hand;
            btnLinkValue2.FlatStyle = FlatStyle.Popup;
            btnLinkValue2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLinkValue2.ForeColor = Color.White;
            btnLinkValue2.Location = new Point(19, 370);
            btnLinkValue2.Name = "btnLinkValue2";
            btnLinkValue2.Size = new Size(107, 28);
            btnLinkValue2.TabIndex = 49;
            btnLinkValue2.Text = "LinkValue";
            btnLinkValue2.UseVisualStyleBackColor = true;
            // 
            // ddSLinkTypes
            // 
            ddSLinkTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSLinkTypes.DropDownWidth = 180;
            ddSLinkTypes.FormattingEnabled = true;
            ddSLinkTypes.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            ddSLinkTypes.Location = new Point(347, 334);
            ddSLinkTypes.Name = "ddSLinkTypes";
            ddSLinkTypes.Size = new Size(20, 28);
            ddSLinkTypes.TabIndex = 48;
            // 
            // ddSMovieCategories
            // 
            ddSMovieCategories.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSMovieCategories.DropDownWidth = 180;
            ddSMovieCategories.FormattingEnabled = true;
            ddSMovieCategories.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            ddSMovieCategories.Location = new Point(347, 301);
            ddSMovieCategories.Name = "ddSMovieCategories";
            ddSMovieCategories.Size = new Size(20, 28);
            ddSMovieCategories.TabIndex = 47;
            // 
            // ddSGroupCategories
            // 
            ddSGroupCategories.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSGroupCategories.DropDownWidth = 180;
            ddSGroupCategories.FormattingEnabled = true;
            ddSGroupCategories.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            ddSGroupCategories.Location = new Point(347, 267);
            ddSGroupCategories.Name = "ddSGroupCategories";
            ddSGroupCategories.Size = new Size(20, 28);
            ddSGroupCategories.TabIndex = 46;
            // 
            // ddSSubCategories
            // 
            ddSSubCategories.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSSubCategories.DropDownWidth = 180;
            ddSSubCategories.FormattingEnabled = true;
            ddSSubCategories.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            ddSSubCategories.Location = new Point(347, 234);
            ddSSubCategories.Name = "ddSSubCategories";
            ddSSubCategories.Size = new Size(20, 28);
            ddSSubCategories.TabIndex = 45;
            // 
            // ddSCategories
            // 
            ddSCategories.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSCategories.DropDownWidth = 180;
            ddSCategories.FormattingEnabled = true;
            ddSCategories.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            ddSCategories.Location = new Point(347, 202);
            ddSCategories.Name = "ddSCategories";
            ddSCategories.Size = new Size(20, 28);
            ddSCategories.TabIndex = 44;
            // 
            // label10
            // 
            label10.Location = new Point(6, 334);
            label10.Name = "label10";
            label10.Size = new Size(120, 25);
            label10.TabIndex = 43;
            label10.Text = "LinkType";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_linktype
            // 
            s_linktype.Location = new Point(131, 334);
            s_linktype.Name = "s_linktype";
            s_linktype.Size = new Size(210, 27);
            s_linktype.TabIndex = 42;
            // 
            // label9
            // 
            label9.Location = new Point(6, 301);
            label9.Name = "label9";
            label9.Size = new Size(120, 25);
            label9.TabIndex = 41;
            label9.Text = "Movie Category";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_moviecategory
            // 
            s_moviecategory.Location = new Point(131, 301);
            s_moviecategory.Name = "s_moviecategory";
            s_moviecategory.Size = new Size(210, 27);
            s_moviecategory.TabIndex = 40;
            // 
            // label8
            // 
            label8.Location = new Point(6, 268);
            label8.Name = "label8";
            label8.Size = new Size(120, 25);
            label8.TabIndex = 39;
            label8.Text = "Group Category";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_groupcategory
            // 
            s_groupcategory.Location = new Point(131, 268);
            s_groupcategory.Name = "s_groupcategory";
            s_groupcategory.Size = new Size(210, 27);
            s_groupcategory.TabIndex = 38;
            // 
            // label7
            // 
            label7.Location = new Point(6, 235);
            label7.Name = "label7";
            label7.Size = new Size(120, 25);
            label7.TabIndex = 37;
            label7.Text = "Sub Category";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_subcategory
            // 
            s_subcategory.Location = new Point(131, 235);
            s_subcategory.Name = "s_subcategory";
            s_subcategory.Size = new Size(210, 27);
            s_subcategory.TabIndex = 36;
            // 
            // label6
            // 
            label6.Location = new Point(6, 202);
            label6.Name = "label6";
            label6.Size = new Size(120, 25);
            label6.TabIndex = 35;
            label6.Text = "Category";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_category
            // 
            s_category.Location = new Point(131, 202);
            s_category.Name = "s_category";
            s_category.Size = new Size(210, 27);
            s_category.TabIndex = 34;
            // 
            // gradientPanel2
            // 
            gradientPanel2.BackgroundImage = (Image)resources.GetObject("gradientPanel2.BackgroundImage");
            gradientPanel2.BackgroundImageLayout = ImageLayout.Stretch;
            gradientPanel2.Controls.Add(txtSearch2);
            gradientPanel2.Controls.Add(ddSearch2);
            gradientPanel2.Controls.Add(btnSearchVideoTitles2);
            gradientPanel2.Controls.Add(btnClean2);
            gradientPanel2.Controls.Add(btnJSON2);
            gradientPanel2.Controls.Add(btnGetDescription2);
            gradientPanel2.Controls.Add(btnDelete2);
            gradientPanel2.Controls.Add(btnNew2);
            gradientPanel2.Controls.Add(btnSave2);
            gradientPanel2.Controls.Add(cbGroup2);
            gradientPanel2.Controls.Add(upDownRecords2);
            gradientPanel2.Controls.Add(lblCounts2);
            gradientPanel2.Controls.Add(ddMCategories2);
            gradientPanel2.Controls.Add(label13);
            gradientPanel2.Controls.Add(label14);
            gradientPanel2.Controls.Add(label12);
            gradientPanel2.Controls.Add(label11);
            gradientPanel2.Controls.Add(ddGroupCategories2);
            gradientPanel2.Controls.Add(ddSubCategories2);
            gradientPanel2.Controls.Add(ddCategories2);
            gradientPanel2.Controls.Add(btnNext2);
            gradientPanel2.Controls.Add(btnPrev2);
            gradientPanel2.Controls.Add(btnFeeds2);
            gradientPanel2.Dock = DockStyle.Top;
            gradientPanel2.ForeColor = Color.White;
            gradientPanel2.GradientBottom = Color.Transparent;
            gradientPanel2.GradientTop = Color.Transparent;
            gradientPanel2.Location = new Point(0, 0);
            gradientPanel2.Name = "gradientPanel2";
            gradientPanel2.Size = new Size(891, 122);
            gradientPanel2.TabIndex = 0;
            // 
            // txtSearch2
            // 
            txtSearch2.Location = new Point(668, 82);
            txtSearch2.Name = "txtSearch2";
            txtSearch2.Size = new Size(171, 27);
            txtSearch2.TabIndex = 60;
            // 
            // ddSearch2
            // 
            ddSearch2.FormattingEnabled = true;
            ddSearch2.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            ddSearch2.Location = new Point(559, 82);
            ddSearch2.Name = "ddSearch2";
            ddSearch2.Size = new Size(105, 28);
            ddSearch2.TabIndex = 49;
            // 
            // btnSearchVideoTitles2
            // 
            btnSearchVideoTitles2.BackgroundImage = (Image)resources.GetObject("btnSearchVideoTitles2.BackgroundImage");
            btnSearchVideoTitles2.BackgroundImageLayout = ImageLayout.Stretch;
            btnSearchVideoTitles2.Cursor = Cursors.Hand;
            btnSearchVideoTitles2.FlatStyle = FlatStyle.Popup;
            btnSearchVideoTitles2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSearchVideoTitles2.ForeColor = Color.White;
            btnSearchVideoTitles2.Location = new Point(479, 79);
            btnSearchVideoTitles2.Name = "btnSearchVideoTitles2";
            btnSearchVideoTitles2.Size = new Size(72, 34);
            btnSearchVideoTitles2.TabIndex = 48;
            btnSearchVideoTitles2.Text = "Search";
            btnSearchVideoTitles2.UseVisualStyleBackColor = true;
            // 
            // btnClean2
            // 
            btnClean2.BackgroundImage = (Image)resources.GetObject("btnClean2.BackgroundImage");
            btnClean2.BackgroundImageLayout = ImageLayout.Stretch;
            btnClean2.Cursor = Cursors.Hand;
            btnClean2.FlatStyle = FlatStyle.Popup;
            btnClean2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClean2.ForeColor = Color.White;
            btnClean2.Location = new Point(739, 44);
            btnClean2.Name = "btnClean2";
            btnClean2.Size = new Size(102, 34);
            btnClean2.TabIndex = 47;
            btnClean2.Text = "Clean";
            btnClean2.UseVisualStyleBackColor = true;
            // 
            // btnJSON2
            // 
            btnJSON2.BackgroundImage = (Image)resources.GetObject("btnJSON2.BackgroundImage");
            btnJSON2.BackgroundImageLayout = ImageLayout.Stretch;
            btnJSON2.Cursor = Cursors.Hand;
            btnJSON2.FlatStyle = FlatStyle.Popup;
            btnJSON2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnJSON2.ForeColor = Color.White;
            btnJSON2.Location = new Point(645, 43);
            btnJSON2.Name = "btnJSON2";
            btnJSON2.Size = new Size(90, 34);
            btnJSON2.TabIndex = 46;
            btnJSON2.Text = "JSON";
            btnJSON2.UseVisualStyleBackColor = true;
            // 
            // btnGetDescription2
            // 
            btnGetDescription2.BackgroundImage = (Image)resources.GetObject("btnGetDescription2.BackgroundImage");
            btnGetDescription2.BackgroundImageLayout = ImageLayout.Stretch;
            btnGetDescription2.Cursor = Cursors.Hand;
            btnGetDescription2.FlatStyle = FlatStyle.Popup;
            btnGetDescription2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGetDescription2.ForeColor = Color.White;
            btnGetDescription2.Location = new Point(558, 44);
            btnGetDescription2.Name = "btnGetDescription2";
            btnGetDescription2.Size = new Size(84, 34);
            btnGetDescription2.TabIndex = 45;
            btnGetDescription2.Text = "Get Info";
            btnGetDescription2.UseVisualStyleBackColor = true;
            // 
            // btnDelete2
            // 
            btnDelete2.BackgroundImage = (Image)resources.GetObject("btnDelete2.BackgroundImage");
            btnDelete2.BackgroundImageLayout = ImageLayout.Stretch;
            btnDelete2.Cursor = Cursors.Hand;
            btnDelete2.FlatStyle = FlatStyle.Popup;
            btnDelete2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete2.ForeColor = Color.White;
            btnDelete2.Location = new Point(739, 7);
            btnDelete2.Name = "btnDelete2";
            btnDelete2.Size = new Size(102, 34);
            btnDelete2.TabIndex = 44;
            btnDelete2.Text = "Delete";
            btnDelete2.UseVisualStyleBackColor = true;
            // 
            // btnNew2
            // 
            btnNew2.BackgroundImage = (Image)resources.GetObject("btnNew2.BackgroundImage");
            btnNew2.BackgroundImageLayout = ImageLayout.Stretch;
            btnNew2.Cursor = Cursors.Hand;
            btnNew2.FlatStyle = FlatStyle.Popup;
            btnNew2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNew2.ForeColor = Color.White;
            btnNew2.Location = new Point(645, 6);
            btnNew2.Name = "btnNew2";
            btnNew2.Size = new Size(90, 34);
            btnNew2.TabIndex = 43;
            btnNew2.Text = "New";
            btnNew2.UseVisualStyleBackColor = true;
            // 
            // btnSave2
            // 
            btnSave2.BackgroundImage = (Image)resources.GetObject("btnSave2.BackgroundImage");
            btnSave2.BackgroundImageLayout = ImageLayout.Stretch;
            btnSave2.Cursor = Cursors.Hand;
            btnSave2.FlatStyle = FlatStyle.Popup;
            btnSave2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave2.ForeColor = Color.White;
            btnSave2.Location = new Point(558, 7);
            btnSave2.Name = "btnSave2";
            btnSave2.Size = new Size(84, 34);
            btnSave2.TabIndex = 42;
            btnSave2.Text = "Save";
            btnSave2.UseVisualStyleBackColor = true;
            // 
            // cbGroup2
            // 
            cbGroup2.AutoSize = true;
            cbGroup2.BackColor = Color.Transparent;
            cbGroup2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            cbGroup2.Location = new Point(477, 13);
            cbGroup2.Name = "cbGroup2";
            cbGroup2.Size = new Size(82, 27);
            cbGroup2.TabIndex = 41;
            cbGroup2.Text = "Group";
            cbGroup2.UseVisualStyleBackColor = false;
            // 
            // upDownRecords2
            // 
            upDownRecords2.Location = new Point(477, 47);
            upDownRecords2.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            upDownRecords2.Name = "upDownRecords2";
            upDownRecords2.Size = new Size(72, 27);
            upDownRecords2.TabIndex = 40;
            upDownRecords2.TextAlign = HorizontalAlignment.Right;
            upDownRecords2.Value = new decimal(new int[] { 300, 0, 0, 0 });
            // 
            // lblCounts2
            // 
            lblCounts2.BackColor = Color.Transparent;
            lblCounts2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCounts2.ForeColor = Color.White;
            lblCounts2.Location = new Point(260, 86);
            lblCounts2.Name = "lblCounts2";
            lblCounts2.Size = new Size(192, 20);
            lblCounts2.TabIndex = 38;
            lblCounts2.Text = "Total: 0";
            lblCounts2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ddMCategories2
            // 
            ddMCategories2.FormattingEnabled = true;
            ddMCategories2.Location = new Point(318, 46);
            ddMCategories2.Name = "ddMCategories2";
            ddMCategories2.Size = new Size(132, 28);
            ddMCategories2.TabIndex = 37;
            // 
            // label13
            // 
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label13.ForeColor = Color.White;
            label13.Location = new Point(253, 46);
            label13.Name = "label13";
            label13.Size = new Size(60, 20);
            label13.TabIndex = 36;
            label13.Text = "Movie";
            label13.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            label14.BackColor = Color.Transparent;
            label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label14.ForeColor = Color.White;
            label14.Location = new Point(254, 13);
            label14.Name = "label14";
            label14.Size = new Size(60, 20);
            label14.TabIndex = 35;
            label14.Text = "Group";
            label14.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label12.ForeColor = Color.White;
            label12.Location = new Point(9, 44);
            label12.Name = "label12";
            label12.Size = new Size(85, 20);
            label12.TabIndex = 34;
            label12.Text = "Sub";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label11.ForeColor = Color.White;
            label11.Location = new Point(10, 11);
            label11.Name = "label11";
            label11.Size = new Size(85, 20);
            label11.TabIndex = 33;
            label11.Text = "Category";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddGroupCategories2
            // 
            ddGroupCategories2.FormattingEnabled = true;
            ddGroupCategories2.Location = new Point(316, 11);
            ddGroupCategories2.Name = "ddGroupCategories2";
            ddGroupCategories2.Size = new Size(132, 28);
            ddGroupCategories2.TabIndex = 32;
            // 
            // ddSubCategories2
            // 
            ddSubCategories2.FormattingEnabled = true;
            ddSubCategories2.Location = new Point(95, 42);
            ddSubCategories2.Name = "ddSubCategories2";
            ddSubCategories2.Size = new Size(145, 28);
            ddSubCategories2.TabIndex = 31;
            // 
            // ddCategories2
            // 
            ddCategories2.FormattingEnabled = true;
            ddCategories2.Location = new Point(95, 8);
            ddCategories2.Name = "ddCategories2";
            ddCategories2.Size = new Size(145, 28);
            ddCategories2.TabIndex = 30;
            // 
            // btnNext2
            // 
            btnNext2.BackgroundImage = (Image)resources.GetObject("btnNext2.BackgroundImage");
            btnNext2.BackgroundImageLayout = ImageLayout.Stretch;
            btnNext2.Cursor = Cursors.Hand;
            btnNext2.FlatStyle = FlatStyle.Popup;
            btnNext2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNext2.ForeColor = Color.White;
            btnNext2.Location = new Point(178, 79);
            btnNext2.Name = "btnNext2";
            btnNext2.Size = new Size(61, 30);
            btnNext2.TabIndex = 29;
            btnNext2.Text = "Next";
            btnNext2.UseVisualStyleBackColor = true;
            // 
            // btnPrev2
            // 
            btnPrev2.BackgroundImage = (Image)resources.GetObject("btnPrev2.BackgroundImage");
            btnPrev2.BackgroundImageLayout = ImageLayout.Stretch;
            btnPrev2.Cursor = Cursors.Hand;
            btnPrev2.FlatStyle = FlatStyle.Popup;
            btnPrev2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPrev2.ForeColor = Color.White;
            btnPrev2.Location = new Point(95, 78);
            btnPrev2.Name = "btnPrev2";
            btnPrev2.Size = new Size(70, 30);
            btnPrev2.TabIndex = 28;
            btnPrev2.Text = "Prev";
            btnPrev2.UseVisualStyleBackColor = true;
            // 
            // btnFeeds2
            // 
            btnFeeds2.BackgroundImage = (Image)resources.GetObject("btnFeeds2.BackgroundImage");
            btnFeeds2.BackgroundImageLayout = ImageLayout.Stretch;
            btnFeeds2.Cursor = Cursors.Hand;
            btnFeeds2.FlatStyle = FlatStyle.Popup;
            btnFeeds2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFeeds2.ForeColor = Color.White;
            btnFeeds2.Location = new Point(19, 79);
            btnFeeds2.Name = "btnFeeds2";
            btnFeeds2.Size = new Size(64, 30);
            btnFeeds2.TabIndex = 27;
            btnFeeds2.Text = "Feeds";
            btnFeeds2.UseVisualStyleBackColor = true;
            // 
            // imgListBlack
            // 
            imgListBlack.ColorDepth = ColorDepth.Depth32Bit;
            imgListBlack.ImageStream = (ImageListStreamer)resources.GetObject("imgListBlack.ImageStream");
            imgListBlack.TransparentColor = Color.Transparent;
            imgListBlack.Images.SetKeyName(0, "add");
            imgListBlack.Images.SetKeyName(1, "back");
            imgListBlack.Images.SetKeyName(2, "close");
            imgListBlack.Images.SetKeyName(3, "forward");
            imgListBlack.Images.SetKeyName(4, "reload");
            imgListBlack.Images.SetKeyName(5, "minus");
            imgListBlack.Images.SetKeyName(6, "home");
            imgListBlack.Images.SetKeyName(7, "down");
            imgListBlack.Images.SetKeyName(8, "up");
            imgListBlack.Images.SetKeyName(9, "go");
            imgListBlack.Images.SetKeyName(10, "ai");
            imgListBlack.Images.SetKeyName(11, "tv");
            imgListBlack.Images.SetKeyName(12, "uncheck");
            imgListBlack.Images.SetKeyName(13, "check");
            // 
            // SplitEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "SplitEditor";
            Size = new Size(1215, 900);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVideos).EndInit();
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)upDown).EndInit();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvFeeds).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbImage).EndInit();
            gradientPanel2.ResumeLayout(false);
            gradientPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)upDownRecords2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ImageList imgListBlack;
        private CustomControls.GradientPanel gradientPanel1;
        private Label label5;
        private Button btnSearch;
        private CustomControls.LabelEx labelEx2;
        private CustomControls.LabelEx labelx;
        private Label lblSub;
        private ComboBox ddSubCategories;
        private Label lblCat;
        private Label label4;
        private ComboBox ddCategories;
        private ComboBox ddMinimumLength;
        private CustomControls.RoundButton btnLoad;
        private Label label3;
        private Label label2;
        private NumericUpDown upDown;
        private TextBox txtSearch;
        private Label label1;
        private ComboBox cbSearchTypes;
        private DataGridView dgvVideos;
        private SplitContainer splitContainer2;
        private DataGridView dgvFeeds;
        private CustomControls.GradientPanel gradientPanel2;
        private Button btnGetDescription;
        private Button btnUnCheckAll;
        private Button btnCheckAll;
        private ComboBox ddGroupCategories;
        private Button btnAddChecked;
        private Button btnCreateJsonFile;
        private Button btnRefreshGroups;
        private Button btnGetRumble;
        private TextBox txtURLRumble;
        private Button btnNext2;
        private Button btnPrev2;
        private Button btnFeeds2;
        private TextBox s_category;
        private Label label6;
        private Label label7;
        private TextBox s_subcategory;
        private Label label8;
        private TextBox s_groupcategory;
        private Label label9;
        private TextBox s_moviecategory;
        private ComboBox ddSCategories;
        private Label label10;
        private TextBox s_linktype;
        private ComboBox ddSLinkTypes;
        private ComboBox ddSMovieCategories;
        private ComboBox ddSGroupCategories;
        private ComboBox ddSSubCategories;
        private ComboBox ddSubCategories2;
        private ComboBox ddCategories2;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private ComboBox ddMCategories2;
        private ComboBox ddGroupCategories2;
        private Button btnLinkValue2;
        private Label label17;
        private TextBox s_rank;
        private Label label16;
        private TextBox s_author;
        private Label label15;
        private TextBox s_duration;
        private TextBox s_link;
        private Button btnLink2;
        private TextBox s_linkvalue;
        private Label label20;
        private TextBox s_description;
        private Label label21;
        private TextBox s_shortdescription;
        private Label label19;
        private TextBox s_title;
        private Label label18;
        private TextBox s_FeedId;
        private Label lblCount;
        private Label lblCounts2;
        private CheckBox cbGroup2;
        private NumericUpDown upDownRecords2;
        private Button btnDelete2;
        private Button btnNew2;
        private Button btnSave2;
        private Button btnClean2;
        private Button btnJSON2;
        private Button btnGetDescription2;
        private TextBox txtSearch2;
        private ComboBox ddSearch2;
        private Button btnSearchVideoTitles2;
        private Label label23;
        private Label label22;
        private TextBox s_bodylinks;
        private Label label25;
        private Label label24;
        private ComboBox ddSImages;
        private ComboBox ddSFolders;
        private TextBox s_image;
        private Button btnGetCoverImage;
        private PictureBox pbImage;
        private Label label26;
    }
}
