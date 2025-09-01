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
            labelEx1 = new CustomControls.LabelEx();
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
            label20 = new Label();
            textBox12 = new TextBox();
            label21 = new Label();
            textBox13 = new TextBox();
            label19 = new Label();
            textBox11 = new TextBox();
            label18 = new Label();
            textBox10 = new TextBox();
            label17 = new Label();
            textBox9 = new TextBox();
            label16 = new Label();
            textBox8 = new TextBox();
            label15 = new Label();
            textBox7 = new TextBox();
            textBox6 = new TextBox();
            button5 = new Button();
            textBox5 = new TextBox();
            button4 = new Button();
            comboBox4 = new ComboBox();
            comboBox3 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            ddSCategories = new ComboBox();
            label10 = new Label();
            textBox4 = new TextBox();
            label9 = new Label();
            textBox3 = new TextBox();
            label8 = new Label();
            textBox2 = new TextBox();
            label7 = new Label();
            textBox1 = new TextBox();
            label6 = new Label();
            s_category = new TextBox();
            gradientPanel2 = new CustomControls.GradientPanel();
            comboBox8 = new ComboBox();
            label13 = new Label();
            label14 = new Label();
            label12 = new Label();
            label11 = new Label();
            comboBox5 = new ComboBox();
            comboBox6 = new ComboBox();
            comboBox7 = new ComboBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
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
            gradientPanel2.SuspendLayout();
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
            splitContainer1.Size = new Size(1153, 900);
            splitContainer1.SplitterDistance = 422;
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
            dgvVideos.Size = new Size(1112, 245);
            dgvVideos.TabIndex = 2;
            // 
            // gradientPanel1
            // 
            gradientPanel1.BackColor = Color.IndianRed;
            gradientPanel1.BackgroundImage = (Image)resources.GetObject("gradientPanel1.BackgroundImage");
            gradientPanel1.BackgroundImageLayout = ImageLayout.Stretch;
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
            gradientPanel1.Controls.Add(labelEx1);
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
            gradientPanel1.Size = new Size(1153, 147);
            gradientPanel1.TabIndex = 1;
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
            btnRefreshGroups.FlatStyle = FlatStyle.Popup;
            btnRefreshGroups.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnRefreshGroups.ForeColor = Color.White;
            btnRefreshGroups.Location = new Point(978, 95);
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
            btnCreateJsonFile.FlatStyle = FlatStyle.Popup;
            btnCreateJsonFile.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCreateJsonFile.ForeColor = Color.White;
            btnCreateJsonFile.Location = new Point(748, 89);
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
            btnGetDescription.FlatStyle = FlatStyle.Popup;
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
            btnUnCheckAll.FlatStyle = FlatStyle.Popup;
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
            btnCheckAll.FlatStyle = FlatStyle.Popup;
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
            labelEx2.Location = new Point(415, 3);
            labelEx2.Name = "labelEx2";
            labelEx2.ShadowColor = Color.Black;
            labelEx2.Size = new Size(270, 26);
            labelEx2.TabIndex = 18;
            labelEx2.Text = "Search for Videos";
            labelEx2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelEx1
            // 
            labelEx1.BackColor = Color.Transparent;
            labelEx1.Font = new Font("Comic Sans MS", 10F, FontStyle.Bold);
            labelEx1.ForeColor = Color.White;
            labelEx1.Location = new Point(22, 95);
            labelEx1.Name = "labelEx1";
            labelEx1.ShadowColor = Color.Black;
            labelEx1.Size = new Size(154, 34);
            labelEx1.TabIndex = 17;
            labelEx1.Text = "labelEx1";
            labelEx1.TextAlign = ContentAlignment.MiddleCenter;
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
            btnLoad.Location = new Point(1076, 12);
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
            cbSearchTypes.DropDownWidth = 160;
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
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(dgvFeeds);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(label20);
            splitContainer2.Panel2.Controls.Add(textBox12);
            splitContainer2.Panel2.Controls.Add(label21);
            splitContainer2.Panel2.Controls.Add(textBox13);
            splitContainer2.Panel2.Controls.Add(label19);
            splitContainer2.Panel2.Controls.Add(textBox11);
            splitContainer2.Panel2.Controls.Add(label18);
            splitContainer2.Panel2.Controls.Add(textBox10);
            splitContainer2.Panel2.Controls.Add(label17);
            splitContainer2.Panel2.Controls.Add(textBox9);
            splitContainer2.Panel2.Controls.Add(label16);
            splitContainer2.Panel2.Controls.Add(textBox8);
            splitContainer2.Panel2.Controls.Add(label15);
            splitContainer2.Panel2.Controls.Add(textBox7);
            splitContainer2.Panel2.Controls.Add(textBox6);
            splitContainer2.Panel2.Controls.Add(button5);
            splitContainer2.Panel2.Controls.Add(textBox5);
            splitContainer2.Panel2.Controls.Add(button4);
            splitContainer2.Panel2.Controls.Add(comboBox4);
            splitContainer2.Panel2.Controls.Add(comboBox3);
            splitContainer2.Panel2.Controls.Add(comboBox2);
            splitContainer2.Panel2.Controls.Add(comboBox1);
            splitContainer2.Panel2.Controls.Add(ddSCategories);
            splitContainer2.Panel2.Controls.Add(label10);
            splitContainer2.Panel2.Controls.Add(textBox4);
            splitContainer2.Panel2.Controls.Add(label9);
            splitContainer2.Panel2.Controls.Add(textBox3);
            splitContainer2.Panel2.Controls.Add(label8);
            splitContainer2.Panel2.Controls.Add(textBox2);
            splitContainer2.Panel2.Controls.Add(label7);
            splitContainer2.Panel2.Controls.Add(textBox1);
            splitContainer2.Panel2.Controls.Add(label6);
            splitContainer2.Panel2.Controls.Add(s_category);
            splitContainer2.Panel2.Controls.Add(gradientPanel2);
            splitContainer2.Size = new Size(1153, 458);
            splitContainer2.SplitterDistance = 312;
            splitContainer2.TabIndex = 0;
            // 
            // dgvFeeds
            // 
            dgvFeeds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFeeds.Dock = DockStyle.Fill;
            dgvFeeds.Location = new Point(0, 0);
            dgvFeeds.Name = "dgvFeeds";
            dgvFeeds.RowHeadersWidth = 51;
            dgvFeeds.Size = new Size(312, 458);
            dgvFeeds.TabIndex = 3;
            // 
            // label20
            // 
            label20.Location = new Point(402, 334);
            label20.Name = "label20";
            label20.Size = new Size(235, 25);
            label20.TabIndex = 66;
            label20.Text = "Description";
            label20.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox12
            // 
            textBox12.Location = new Point(402, 362);
            textBox12.Multiline = true;
            textBox12.Name = "textBox12";
            textBox12.Size = new Size(363, 91);
            textBox12.TabIndex = 65;
            // 
            // label21
            // 
            label21.Location = new Point(402, 233);
            label21.Name = "label21";
            label21.Size = new Size(235, 25);
            label21.TabIndex = 64;
            label21.Text = "Short Description";
            label21.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox13
            // 
            textBox13.Location = new Point(402, 261);
            textBox13.Multiline = true;
            textBox13.Name = "textBox13";
            textBox13.Size = new Size(363, 64);
            textBox13.TabIndex = 63;
            // 
            // label19
            // 
            label19.Location = new Point(402, 168);
            label19.Name = "label19";
            label19.Size = new Size(120, 25);
            label19.TabIndex = 62;
            label19.Text = "Video Title";
            label19.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox11
            // 
            textBox11.Location = new Point(402, 196);
            textBox11.Name = "textBox11";
            textBox11.Size = new Size(363, 27);
            textBox11.TabIndex = 61;
            // 
            // label18
            // 
            label18.Location = new Point(397, 130);
            label18.Name = "label18";
            label18.Size = new Size(63, 25);
            label18.TabIndex = 60;
            label18.Text = "Feed ID";
            label18.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox10
            // 
            textBox10.Location = new Point(470, 130);
            textBox10.Name = "textBox10";
            textBox10.Size = new Size(295, 27);
            textBox10.TabIndex = 59;
            // 
            // label17
            // 
            label17.Location = new Point(6, 428);
            label17.Name = "label17";
            label17.Size = new Size(120, 25);
            label17.TabIndex = 58;
            label17.Text = "Rank/Views";
            label17.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox9
            // 
            textBox9.Location = new Point(131, 428);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(235, 27);
            textBox9.TabIndex = 57;
            // 
            // label16
            // 
            label16.Location = new Point(6, 395);
            label16.Name = "label16";
            label16.Size = new Size(120, 25);
            label16.TabIndex = 56;
            label16.Text = "Author";
            label16.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(131, 395);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(235, 27);
            textBox8.TabIndex = 55;
            // 
            // label15
            // 
            label15.Location = new Point(6, 362);
            label15.Name = "label15";
            label15.Size = new Size(120, 25);
            label15.TabIndex = 54;
            label15.Text = "Duration";
            label15.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(131, 362);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(235, 27);
            textBox7.TabIndex = 53;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(131, 329);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(235, 27);
            textBox6.TabIndex = 52;
            // 
            // button5
            // 
            button5.BackgroundImage = (Image)resources.GetObject("button5.BackgroundImage");
            button5.BackgroundImageLayout = ImageLayout.Stretch;
            button5.FlatStyle = FlatStyle.Popup;
            button5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button5.ForeColor = Color.White;
            button5.Location = new Point(18, 330);
            button5.Name = "button5";
            button5.Size = new Size(107, 28);
            button5.TabIndex = 51;
            button5.Text = "Link";
            button5.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(132, 296);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(235, 27);
            textBox5.TabIndex = 50;
            // 
            // button4
            // 
            button4.BackgroundImage = (Image)resources.GetObject("button4.BackgroundImage");
            button4.BackgroundImageLayout = ImageLayout.Stretch;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button4.ForeColor = Color.White;
            button4.Location = new Point(19, 297);
            button4.Name = "button4";
            button4.Size = new Size(107, 28);
            button4.TabIndex = 49;
            button4.Text = "LinkValue";
            button4.UseVisualStyleBackColor = true;
            // 
            // comboBox4
            // 
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownWidth = 180;
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            comboBox4.Location = new Point(347, 261);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(20, 28);
            comboBox4.TabIndex = 48;
            // 
            // comboBox3
            // 
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownWidth = 180;
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            comboBox3.Location = new Point(347, 228);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(20, 28);
            comboBox3.TabIndex = 47;
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownWidth = 180;
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            comboBox2.Location = new Point(347, 194);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(20, 28);
            comboBox2.TabIndex = 46;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DropDownWidth = 180;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            comboBox1.Location = new Point(347, 161);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(20, 28);
            comboBox1.TabIndex = 45;
            // 
            // ddSCategories
            // 
            ddSCategories.DropDownStyle = ComboBoxStyle.DropDownList;
            ddSCategories.DropDownWidth = 180;
            ddSCategories.FormattingEnabled = true;
            ddSCategories.Items.AddRange(new object[] { "Search General", "Search ChannelItems", "Search Playlist" });
            ddSCategories.Location = new Point(347, 129);
            ddSCategories.Name = "ddSCategories";
            ddSCategories.Size = new Size(20, 28);
            ddSCategories.TabIndex = 44;
            // 
            // label10
            // 
            label10.Location = new Point(6, 261);
            label10.Name = "label10";
            label10.Size = new Size(120, 25);
            label10.TabIndex = 43;
            label10.Text = "LinkType";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(131, 261);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(210, 27);
            textBox4.TabIndex = 42;
            // 
            // label9
            // 
            label9.Location = new Point(6, 228);
            label9.Name = "label9";
            label9.Size = new Size(120, 25);
            label9.TabIndex = 41;
            label9.Text = "Movie Category";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(131, 228);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(210, 27);
            textBox3.TabIndex = 40;
            // 
            // label8
            // 
            label8.Location = new Point(6, 195);
            label8.Name = "label8";
            label8.Size = new Size(120, 25);
            label8.TabIndex = 39;
            label8.Text = "Group Category";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(131, 195);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(210, 27);
            textBox2.TabIndex = 38;
            // 
            // label7
            // 
            label7.Location = new Point(6, 162);
            label7.Name = "label7";
            label7.Size = new Size(120, 25);
            label7.TabIndex = 37;
            label7.Text = "Sub Category";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(131, 162);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(210, 27);
            textBox1.TabIndex = 36;
            // 
            // label6
            // 
            label6.Location = new Point(6, 129);
            label6.Name = "label6";
            label6.Size = new Size(120, 25);
            label6.TabIndex = 35;
            label6.Text = "Category";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // s_category
            // 
            s_category.Location = new Point(131, 129);
            s_category.Name = "s_category";
            s_category.Size = new Size(210, 27);
            s_category.TabIndex = 34;
            // 
            // gradientPanel2
            // 
            gradientPanel2.BackgroundImage = (Image)resources.GetObject("gradientPanel2.BackgroundImage");
            gradientPanel2.BackgroundImageLayout = ImageLayout.Stretch;
            gradientPanel2.Controls.Add(comboBox8);
            gradientPanel2.Controls.Add(label13);
            gradientPanel2.Controls.Add(label14);
            gradientPanel2.Controls.Add(label12);
            gradientPanel2.Controls.Add(label11);
            gradientPanel2.Controls.Add(comboBox5);
            gradientPanel2.Controls.Add(comboBox6);
            gradientPanel2.Controls.Add(comboBox7);
            gradientPanel2.Controls.Add(button1);
            gradientPanel2.Controls.Add(button2);
            gradientPanel2.Controls.Add(button3);
            gradientPanel2.Dock = DockStyle.Top;
            gradientPanel2.ForeColor = Color.White;
            gradientPanel2.GradientBottom = Color.Transparent;
            gradientPanel2.GradientTop = Color.Transparent;
            gradientPanel2.Location = new Point(0, 0);
            gradientPanel2.Name = "gradientPanel2";
            gradientPanel2.Size = new Size(837, 122);
            gradientPanel2.TabIndex = 0;
            // 
            // comboBox8
            // 
            comboBox8.FormattingEnabled = true;
            comboBox8.Location = new Point(359, 46);
            comboBox8.Name = "comboBox8";
            comboBox8.Size = new Size(145, 28);
            comboBox8.TabIndex = 37;
            // 
            // label13
            // 
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label13.ForeColor = Color.White;
            label13.Location = new Point(281, 46);
            label13.Name = "label13";
            label13.Size = new Size(73, 20);
            label13.TabIndex = 36;
            label13.Text = "Movie";
            label13.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            label14.BackColor = Color.Transparent;
            label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label14.ForeColor = Color.White;
            label14.Location = new Point(288, 13);
            label14.Name = "label14";
            label14.Size = new Size(67, 20);
            label14.TabIndex = 35;
            label14.Text = "Group";
            label14.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label12.ForeColor = Color.White;
            label12.Location = new Point(6, 44);
            label12.Name = "label12";
            label12.Size = new Size(91, 20);
            label12.TabIndex = 34;
            label12.Text = "Sub";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label11.ForeColor = Color.White;
            label11.Location = new Point(13, 11);
            label11.Name = "label11";
            label11.Size = new Size(91, 20);
            label11.TabIndex = 33;
            label11.Text = "Category";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboBox5
            // 
            comboBox5.FormattingEnabled = true;
            comboBox5.Location = new Point(357, 11);
            comboBox5.Name = "comboBox5";
            comboBox5.Size = new Size(145, 28);
            comboBox5.TabIndex = 32;
            // 
            // comboBox6
            // 
            comboBox6.FormattingEnabled = true;
            comboBox6.Location = new Point(107, 42);
            comboBox6.Name = "comboBox6";
            comboBox6.Size = new Size(172, 28);
            comboBox6.TabIndex = 31;
            // 
            // comboBox7
            // 
            comboBox7.FormattingEnabled = true;
            comboBox7.Location = new Point(107, 8);
            comboBox7.Name = "comboBox7";
            comboBox7.Size = new Size(172, 28);
            comboBox7.TabIndex = 30;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(203, 79);
            button1.Name = "button1";
            button1.Size = new Size(74, 35);
            button1.TabIndex = 29;
            button1.Text = "Next";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button2.ForeColor = Color.White;
            button2.Location = new Point(107, 78);
            button2.Name = "button2";
            button2.Size = new Size(83, 35);
            button2.TabIndex = 28;
            button2.Text = "Prev";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button3.ForeColor = Color.White;
            button3.Location = new Point(19, 79);
            button3.Name = "button3";
            button3.Size = new Size(77, 35);
            button3.TabIndex = 27;
            button3.Text = "Feeds";
            button3.UseVisualStyleBackColor = true;
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
            Size = new Size(1153, 900);
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
            gradientPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ImageList imgListBlack;
        private CustomControls.GradientPanel gradientPanel1;
        private Label label5;
        private Button btnSearch;
        private CustomControls.LabelEx labelEx2;
        private CustomControls.LabelEx labelEx1;
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
        private Button button1;
        private Button button2;
        private Button button3;
        private TextBox s_category;
        private Label label6;
        private Label label7;
        private TextBox textBox1;
        private Label label8;
        private TextBox textBox2;
        private Label label9;
        private TextBox textBox3;
        private ComboBox ddSCategories;
        private Label label10;
        private TextBox textBox4;
        private ComboBox comboBox4;
        private ComboBox comboBox3;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private ComboBox comboBox6;
        private ComboBox comboBox7;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private ComboBox comboBox8;
        private ComboBox comboBox5;
        private Button button4;
        private Label label17;
        private TextBox textBox9;
        private Label label16;
        private TextBox textBox8;
        private Label label15;
        private TextBox textBox7;
        private TextBox textBox6;
        private Button button5;
        private TextBox textBox5;
        private Label label20;
        private TextBox textBox12;
        private Label label21;
        private TextBox textBox13;
        private Label label19;
        private TextBox textBox11;
        private Label label18;
        private TextBox textBox10;
    }
}
