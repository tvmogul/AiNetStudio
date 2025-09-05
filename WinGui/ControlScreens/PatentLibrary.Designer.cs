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
            btnSearch = new Button();
            dgvPDF = new DataGridView();
            label9 = new Label();
            textBox3 = new TextBox();
            label6 = new Label();
            label7 = new Label();
            comboBox3 = new ComboBox();
            comboBox4 = new ComboBox();
            label8 = new Label();
            textBox2 = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            label2 = new Label();
            textBox1 = new TextBox();
            label1 = new Label();
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
            splitContainer1.Panel1.Controls.Add(btnSearch);
            splitContainer1.Panel1.Controls.Add(dgvPDF);
            splitContainer1.Panel1.Controls.Add(label9);
            splitContainer1.Panel1.Controls.Add(textBox3);
            splitContainer1.Panel1.Controls.Add(label6);
            splitContainer1.Panel1.Controls.Add(label7);
            splitContainer1.Panel1.Controls.Add(comboBox3);
            splitContainer1.Panel1.Controls.Add(comboBox4);
            splitContainer1.Panel1.Controls.Add(label8);
            splitContainer1.Panel1.Controls.Add(textBox2);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(comboBox2);
            splitContainer1.Panel1.Controls.Add(comboBox1);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(textBox1);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Size = new Size(1033, 647);
            splitContainer1.SplitterDistance = 344;
            splitContainer1.SplitterWidth = 24;
            splitContainer1.TabIndex = 0;
            // 
            // btnSearch
            // 
            btnSearch.BackgroundImage = (Image)resources.GetObject("btnSearch.BackgroundImage");
            btnSearch.BackgroundImageLayout = ImageLayout.Stretch;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(216, 135);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(106, 35);
            btnSearch.TabIndex = 32;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // dgvPDF
            // 
            dgvPDF.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPDF.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPDF.Location = new Point(0, 209);
            dgvPDF.Name = "dgvPDF";
            dgvPDF.RowHeadersWidth = 51;
            dgvPDF.Size = new Size(341, 206);
            dgvPDF.TabIndex = 17;
            // 
            // label9
            // 
            label9.FlatStyle = FlatStyle.Flat;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label9.Location = new Point(7, 523);
            label9.Name = "label9";
            label9.Size = new Size(103, 25);
            label9.TabIndex = 16;
            label9.Text = "Description";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(116, 522);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(206, 34);
            textBox3.TabIndex = 15;
            // 
            // label6
            // 
            label6.FlatStyle = FlatStyle.Flat;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(3, 489);
            label6.Name = "label6";
            label6.Size = new Size(107, 25);
            label6.TabIndex = 14;
            label6.Text = "SubCategory";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.FlatStyle = FlatStyle.Flat;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(3, 454);
            label7.Name = "label7";
            label7.Size = new Size(103, 25);
            label7.TabIndex = 13;
            label7.Text = "Category";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(116, 488);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(206, 28);
            comboBox3.TabIndex = 12;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(116, 454);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(206, 28);
            comboBox4.TabIndex = 11;
            // 
            // label8
            // 
            label8.FlatStyle = FlatStyle.Flat;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(3, 420);
            label8.Name = "label8";
            label8.Size = new Size(103, 25);
            label8.TabIndex = 10;
            label8.Text = "Title";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(116, 421);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(206, 27);
            textBox2.TabIndex = 9;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label5.BackColor = Color.FromArgb(101, 101, 102);
            label5.FlatStyle = FlatStyle.Flat;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.Location = new Point(0, 177);
            label5.Name = "label5";
            label5.Size = new Size(344, 29);
            label5.TabIndex = 8;
            label5.Text = "DOUBLE CLICK TO LOAD PDF";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.FlatStyle = FlatStyle.Flat;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(3, 100);
            label4.Name = "label4";
            label4.Size = new Size(107, 25);
            label4.TabIndex = 7;
            label4.Text = "SubCategory";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.FlatStyle = FlatStyle.Flat;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(3, 67);
            label3.Name = "label3";
            label3.Size = new Size(103, 25);
            label3.TabIndex = 6;
            label3.Text = "Category";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(116, 99);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(206, 28);
            comboBox2.TabIndex = 5;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(116, 67);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(206, 28);
            comboBox1.TabIndex = 4;
            // 
            // label2
            // 
            label2.FlatStyle = FlatStyle.Flat;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(3, 35);
            label2.Name = "label2";
            label2.Size = new Size(103, 25);
            label2.TabIndex = 3;
            label2.Text = "Keywords";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(116, 36);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(206, 27);
            textBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.BackColor = Color.FromArgb(101, 101, 102);
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(344, 29);
            label1.TabIndex = 1;
            label1.Text = "SEARCH PATENTS, ARTICLES";
            label1.TextAlign = ContentAlignment.MiddleCenter;
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
        private TextBox textBox1;
        private Label label4;
        private Label label3;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private Label label9;
        private TextBox textBox3;
        private Label label6;
        private Label label7;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private Label label8;
        private TextBox textBox2;
        private Label label5;
        private DataGridView dgvPDF;
        private Button btnSearch;
    }
}
