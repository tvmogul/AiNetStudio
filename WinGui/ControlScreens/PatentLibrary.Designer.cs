namespace AiNetStudio.WinGui.ControlScreens
{
    partial class PatentLibrary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatentLibrary));
            splitContainer1 = new SplitContainer();
            label1 = new Label();
            btnSearch = new Button();
            btnSave = new Button();
            txtSearch = new TextBox();
            label2 = new Label();
            dgvPDF = new DataGridView();
            label3 = new Label();
            txtTitle = new TextBox();
            ddSSubCategory = new ComboBox();
            txtDescription = new TextBox();
            label4 = new Label();
            label7 = new Label();
            ddSCategory = new ComboBox();
            label9 = new Label();
            ddSubCategory = new ComboBox();
            label6 = new Label();
            label8 = new Label();
            ddCategory = new ComboBox();
            imgListBlack = new ImageList(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPDF).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(btnSearch);
            splitContainer1.Panel1.Controls.Add(btnSave);
            splitContainer1.Panel1.Controls.Add(txtSearch);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(dgvPDF);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(txtTitle);
            splitContainer1.Panel1.Controls.Add(ddSSubCategory);
            splitContainer1.Panel1.Controls.Add(txtDescription);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(label7);
            splitContainer1.Panel1.Controls.Add(ddSCategory);
            splitContainer1.Panel1.Controls.Add(label9);
            splitContainer1.Panel1.Controls.Add(ddSubCategory);
            splitContainer1.Panel1.Controls.Add(label6);
            splitContainer1.Panel1.Controls.Add(label8);
            splitContainer1.Panel1.Controls.Add(ddCategory);
            splitContainer1.Size = new Size(1033, 647);
            splitContainer1.SplitterDistance = 344;
            splitContainer1.SplitterWidth = 24;
            splitContainer1.TabIndex = 0;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(101, 101, 102);
            label1.Dock = DockStyle.Top;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(344, 29);
            label1.TabIndex = 1;
            label1.Text = "SEARCH PDFs, PATENTS, ARTICLES";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            btnSearch.BackgroundImage = (Image)resources.GetObject("btnSearch.BackgroundImage");
            btnSearch.BackgroundImageLayout = ImageLayout.Stretch;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(226, 74);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(79, 29);
            btnSearch.TabIndex = 32;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.BackgroundImage = (Image)resources.GetObject("btnSave.BackgroundImage");
            btnSave.BackgroundImageLayout = ImageLayout.Stretch;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(226, 552);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(79, 29);
            btnSave.TabIndex = 35;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(49, 74);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(170, 27);
            txtSearch.TabIndex = 2;
            // 
            // label2
            // 
            label2.FlatStyle = FlatStyle.Flat;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(10, 74);
            label2.Name = "label2";
            label2.Size = new Size(38, 25);
            label2.TabIndex = 3;
            label2.Text = "Key";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dgvPDF
            // 
            dgvPDF.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvPDF.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPDF.Location = new Point(0, 109);
            dgvPDF.Name = "dgvPDF";
            dgvPDF.RowHeadersWidth = 51;
            dgvPDF.Size = new Size(344, 237);
            dgvPDF.TabIndex = 17;
            // 
            // label3
            // 
            label3.FlatStyle = FlatStyle.Flat;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(10, 40);
            label3.Name = "label3";
            label3.Size = new Size(38, 25);
            label3.TabIndex = 6;
            label3.Text = "Cat";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtTitle
            // 
            txtTitle.Font = new Font("Segoe UI", 8F);
            txtTitle.Location = new Point(56, 361);
            txtTitle.Multiline = true;
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(272, 59);
            txtTitle.TabIndex = 9;
            // 
            // ddSSubCategory
            // 
            ddSSubCategory.FormattingEnabled = true;
            ddSSubCategory.Location = new Point(200, 39);
            ddSSubCategory.Name = "ddSSubCategory";
            ddSSubCategory.Size = new Size(103, 28);
            ddSSubCategory.TabIndex = 5;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(56, 497);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(272, 40);
            txtDescription.TabIndex = 15;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.FlatStyle = FlatStyle.Flat;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(160, 40);
            label4.Name = "label4";
            label4.Size = new Size(36, 25);
            label4.TabIndex = 7;
            label4.Text = "Sub";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.FlatStyle = FlatStyle.Flat;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(8, 429);
            label7.Name = "label7";
            label7.Size = new Size(41, 25);
            label7.TabIndex = 13;
            label7.Text = "Cat";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddSCategory
            // 
            ddSCategory.FormattingEnabled = true;
            ddSCategory.Location = new Point(51, 39);
            ddSCategory.Name = "ddSCategory";
            ddSCategory.Size = new Size(103, 28);
            ddSCategory.TabIndex = 4;
            // 
            // label9
            // 
            label9.FlatStyle = FlatStyle.Flat;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label9.Location = new Point(7, 506);
            label9.Name = "label9";
            label9.Size = new Size(44, 25);
            label9.TabIndex = 16;
            label9.Text = "Desc";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddSubCategory
            // 
            ddSubCategory.FormattingEnabled = true;
            ddSubCategory.Location = new Point(56, 463);
            ddSubCategory.Name = "ddSubCategory";
            ddSubCategory.Size = new Size(272, 28);
            ddSubCategory.TabIndex = 12;
            // 
            // label6
            // 
            label6.FlatStyle = FlatStyle.Flat;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(10, 464);
            label6.Name = "label6";
            label6.Size = new Size(41, 25);
            label6.TabIndex = 14;
            label6.Text = "Sub";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.FlatStyle = FlatStyle.Flat;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(10, 374);
            label8.Name = "label8";
            label8.Size = new Size(41, 25);
            label8.TabIndex = 10;
            label8.Text = "Title";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ddCategory
            // 
            ddCategory.DropDownWidth = 400;
            ddCategory.ForeColor = SystemColors.WindowFrame;
            ddCategory.FormattingEnabled = true;
            ddCategory.Location = new Point(56, 429);
            ddCategory.Name = "ddCategory";
            ddCategory.Size = new Size(272, 28);
            ddCategory.TabIndex = 11;
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
            // PatentLibrary
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "PatentLibrary";
            Size = new Size(1033, 647);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPDF).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ImageList imgListBlack;
        private Label label1;
        private Label label2;
        private TextBox txtSearch;
        private Label label4;
        private Label label3;
        private ComboBox ddSSubCategory;
        private ComboBox ddSCategory;
        private Label label9;
        private TextBox txtDescription;
        private Label label6;
        private Label label7;
        private ComboBox ddSubCategory;
        private ComboBox ddCategory;
        private Label label8;
        private TextBox txtTitle;
        private DataGridView dgvPDF;
        private Button btnSearch;
        private Button btnSave;
        private Controls.AccordionSection accordionSection1;
        private Controls.AccordionSection accordionSection2;
        private Controls.AccordionSection accordionSection3;
        private Controls.AccordionSection accordionSection4;
        private Controls.AccordionSection accordionSection5;
        private Controls.AccordionSection accordionSection6;
    }
}
