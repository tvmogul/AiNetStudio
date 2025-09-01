using CustomControls;

namespace AiNetStudio.WinGui.ControlScreens
{
    partial class DeviceHubControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceHubControl));
            btnStartLogging = new RoundButton();
            btnStopLogging = new RoundButton();
            btnScale = new RoundButton();
            btnTH = new RoundButton();
            btnTach = new RoundButton();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            panelTiles = new TableLayoutPanel();
            tileScale = new Panel();
            lblScaleValue = new Label();
            lblScaleTitle = new Label();
            tileTH = new Panel();
            lblTHValue = new Label();
            lblTHTitle = new Label();
            tileTach = new Panel();
            lblTachValue = new Label();
            lblTachTitle = new Label();
            gridReadings = new DataGridView();
            TimestampUtc = new DataGridViewTextBoxColumn();
            DeviceId = new DataGridViewTextBoxColumn();
            Metric = new DataGridViewTextBoxColumn();
            Value = new DataGridViewTextBoxColumn();
            Unit = new DataGridViewTextBoxColumn();
            ExtraJson = new DataGridViewTextBoxColumn();
            panelTop = new GradientPanel();
            label1 = new Label();
            chkSaveReadings = new CheckBox();
            rbActive = new RadioButton();
            rbFaux = new RadioButton();
            splitContainer1 = new SplitContainer();
            label2 = new Label();
            panelTiles.SuspendLayout();
            tileScale.SuspendLayout();
            tileTH.SuspendLayout();
            tileTach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridReadings).BeginInit();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStartLogging
            // 
            btnStartLogging.BackgroundImage = (Image)resources.GetObject("btnStartLogging.BackgroundImage");
            btnStartLogging.BackgroundImageLayout = ImageLayout.Stretch;
            btnStartLogging.FlatStyle = FlatStyle.Flat;
            btnStartLogging.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnStartLogging.Location = new Point(18, 35);
            btnStartLogging.Name = "btnStartLogging";
            btnStartLogging.Size = new Size(100, 80);
            btnStartLogging.TabIndex = 0;
            btnStartLogging.Text = "Start\r\nLogging";
            btnStartLogging.UseVisualStyleBackColor = true;
            // 
            // btnStopLogging
            // 
            btnStopLogging.BackgroundImage = (Image)resources.GetObject("btnStopLogging.BackgroundImage");
            btnStopLogging.BackgroundImageLayout = ImageLayout.Stretch;
            btnStopLogging.Enabled = false;
            btnStopLogging.FlatStyle = FlatStyle.Popup;
            btnStopLogging.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnStopLogging.Location = new Point(118, 35);
            btnStopLogging.Name = "btnStopLogging";
            btnStopLogging.Size = new Size(100, 80);
            btnStopLogging.TabIndex = 1;
            btnStopLogging.Text = "Stop\r\nLogging";
            btnStopLogging.UseVisualStyleBackColor = true;
            // 
            // btnScale
            // 
            btnScale.BackgroundImage = (Image)resources.GetObject("btnScale.BackgroundImage");
            btnScale.BackgroundImageLayout = ImageLayout.Stretch;
            btnScale.FlatStyle = FlatStyle.Popup;
            btnScale.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnScale.Location = new Point(217, 35);
            btnScale.Name = "btnScale";
            btnScale.Size = new Size(100, 80);
            btnScale.TabIndex = 2;
            btnScale.Tag = "scale";
            btnScale.Text = "Start\r\nScale";
            btnScale.UseVisualStyleBackColor = true;
            // 
            // btnTH
            // 
            btnTH.BackgroundImage = (Image)resources.GetObject("btnTH.BackgroundImage");
            btnTH.BackgroundImageLayout = ImageLayout.Stretch;
            btnTH.FlatStyle = FlatStyle.Popup;
            btnTH.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnTH.Location = new Point(320, 35);
            btnTH.Name = "btnTH";
            btnTH.Size = new Size(126, 80);
            btnTH.TabIndex = 3;
            btnTH.Tag = "th";
            btnTH.Text = "Start Temp\r\n/Humidity";
            btnTH.UseVisualStyleBackColor = true;
            // 
            // btnTach
            // 
            btnTach.BackgroundImage = (Image)resources.GetObject("btnTach.BackgroundImage");
            btnTach.BackgroundImageLayout = ImageLayout.Stretch;
            btnTach.FlatStyle = FlatStyle.Popup;
            btnTach.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnTach.Location = new Point(452, 35);
            btnTach.Name = "btnTach";
            btnTach.Size = new Size(100, 80);
            btnTach.TabIndex = 4;
            btnTach.Tag = "tach";
            btnTach.Text = "Start\r\nTach";
            btnTach.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.BackColor = Color.Transparent;
            radioButton1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            radioButton1.Location = new Point(721, 7);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(73, 29);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "faux";
            radioButton1.UseVisualStyleBackColor = false;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.BackColor = Color.Transparent;
            radioButton2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            radioButton2.Location = new Point(721, 34);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(84, 29);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.Text = "active";
            radioButton2.UseVisualStyleBackColor = false;
            // 
            // panelTiles
            // 
            panelTiles.ColumnCount = 3;
            panelTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            panelTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            panelTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            panelTiles.Controls.Add(tileScale, 0, 0);
            panelTiles.Controls.Add(tileTH, 1, 0);
            panelTiles.Controls.Add(tileTach, 2, 0);
            panelTiles.Dock = DockStyle.Top;
            panelTiles.Location = new Point(0, 0);
            panelTiles.Name = "panelTiles";
            panelTiles.RowCount = 1;
            panelTiles.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelTiles.Size = new Size(1045, 76);
            panelTiles.TabIndex = 1;
            // 
            // tileScale
            // 
            tileScale.Controls.Add(lblScaleValue);
            tileScale.Controls.Add(lblScaleTitle);
            tileScale.Dock = DockStyle.Fill;
            tileScale.Location = new Point(3, 3);
            tileScale.Name = "tileScale";
            tileScale.Size = new Size(338, 70);
            tileScale.TabIndex = 0;
            // 
            // lblScaleValue
            // 
            lblScaleValue.Dock = DockStyle.Fill;
            lblScaleValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblScaleValue.Location = new Point(0, 18);
            lblScaleValue.Name = "lblScaleValue";
            lblScaleValue.Size = new Size(338, 52);
            lblScaleValue.TabIndex = 1;
            lblScaleValue.Text = "--";
            // 
            // lblScaleTitle
            // 
            lblScaleTitle.Dock = DockStyle.Top;
            lblScaleTitle.Location = new Point(0, 0);
            lblScaleTitle.Name = "lblScaleTitle";
            lblScaleTitle.Size = new Size(338, 18);
            lblScaleTitle.TabIndex = 0;
            lblScaleTitle.Text = "Scale (lb)";
            // 
            // tileTH
            // 
            tileTH.Controls.Add(lblTHValue);
            tileTH.Controls.Add(lblTHTitle);
            tileTH.Dock = DockStyle.Fill;
            tileTH.Location = new Point(347, 3);
            tileTH.Name = "tileTH";
            tileTH.Size = new Size(349, 70);
            tileTH.TabIndex = 1;
            // 
            // lblTHValue
            // 
            lblTHValue.Dock = DockStyle.Fill;
            lblTHValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTHValue.Location = new Point(0, 18);
            lblTHValue.Name = "lblTHValue";
            lblTHValue.Size = new Size(349, 52);
            lblTHValue.TabIndex = 3;
            lblTHValue.Text = "--";
            // 
            // lblTHTitle
            // 
            lblTHTitle.Dock = DockStyle.Top;
            lblTHTitle.Location = new Point(0, 0);
            lblTHTitle.Name = "lblTHTitle";
            lblTHTitle.Size = new Size(349, 18);
            lblTHTitle.TabIndex = 2;
            lblTHTitle.Text = "Temp/Humidity";
            // 
            // tileTach
            // 
            tileTach.Controls.Add(lblTachValue);
            tileTach.Controls.Add(lblTachTitle);
            tileTach.Dock = DockStyle.Fill;
            tileTach.Location = new Point(702, 3);
            tileTach.Name = "tileTach";
            tileTach.Size = new Size(340, 70);
            tileTach.TabIndex = 2;
            // 
            // lblTachValue
            // 
            lblTachValue.Dock = DockStyle.Fill;
            lblTachValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTachValue.Location = new Point(0, 18);
            lblTachValue.Name = "lblTachValue";
            lblTachValue.Size = new Size(340, 52);
            lblTachValue.TabIndex = 3;
            lblTachValue.Text = "--";
            // 
            // lblTachTitle
            // 
            lblTachTitle.Dock = DockStyle.Top;
            lblTachTitle.Location = new Point(0, 0);
            lblTachTitle.Name = "lblTachTitle";
            lblTachTitle.Size = new Size(340, 18);
            lblTachTitle.TabIndex = 2;
            lblTachTitle.Text = "RPM";
            // 
            // gridReadings
            // 
            gridReadings.AllowUserToAddRows = false;
            gridReadings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridReadings.Columns.AddRange(new DataGridViewColumn[] { TimestampUtc, DeviceId, Metric, Value, Unit, ExtraJson });
            gridReadings.Location = new Point(18, 94);
            gridReadings.Name = "gridReadings";
            gridReadings.ReadOnly = true;
            gridReadings.RowHeadersWidth = 51;
            gridReadings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridReadings.Size = new Size(987, 321);
            gridReadings.TabIndex = 2;
            // 
            // TimestampUtc
            // 
            TimestampUtc.HeaderText = "Time (UTC)";
            TimestampUtc.MinimumWidth = 6;
            TimestampUtc.Name = "TimestampUtc";
            TimestampUtc.ReadOnly = true;
            TimestampUtc.Width = 125;
            // 
            // DeviceId
            // 
            DeviceId.HeaderText = "Device";
            DeviceId.MinimumWidth = 6;
            DeviceId.Name = "DeviceId";
            DeviceId.ReadOnly = true;
            DeviceId.Width = 125;
            // 
            // Metric
            // 
            Metric.HeaderText = "Metric";
            Metric.MinimumWidth = 6;
            Metric.Name = "Metric";
            Metric.ReadOnly = true;
            Metric.Width = 125;
            // 
            // Value
            // 
            Value.HeaderText = "Value";
            Value.MinimumWidth = 6;
            Value.Name = "Value";
            Value.ReadOnly = true;
            Value.Width = 125;
            // 
            // Unit
            // 
            Unit.HeaderText = "Unit";
            Unit.MinimumWidth = 6;
            Unit.Name = "Unit";
            Unit.ReadOnly = true;
            Unit.Width = 125;
            // 
            // ExtraJson
            // 
            ExtraJson.HeaderText = "Extra";
            ExtraJson.MinimumWidth = 6;
            ExtraJson.Name = "ExtraJson";
            ExtraJson.ReadOnly = true;
            ExtraJson.Width = 125;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(label2);
            panelTop.Controls.Add(label1);
            panelTop.Controls.Add(chkSaveReadings);
            panelTop.Controls.Add(rbActive);
            panelTop.Controls.Add(rbFaux);
            panelTop.Controls.Add(btnStartLogging);
            panelTop.Controls.Add(btnTach);
            panelTop.Controls.Add(btnStopLogging);
            panelTop.Controls.Add(btnTH);
            panelTop.Controls.Add(btnScale);
            panelTop.Dock = DockStyle.Fill;
            panelTop.GradientBottom = Color.Gainsboro;
            panelTop.GradientTop = Color.WhiteSmoke;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1045, 133);
            panelTop.TabIndex = 5;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label1.Location = new Point(439, -2);
            label1.Name = "label1";
            label1.Size = new Size(169, 33);
            label1.TabIndex = 8;
            label1.Text = "Device Hub";
            // 
            // chkSaveReadings
            // 
            chkSaveReadings.AutoSize = true;
            chkSaveReadings.BackColor = Color.Transparent;
            chkSaveReadings.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            chkSaveReadings.Location = new Point(807, 0);
            chkSaveReadings.Name = "chkSaveReadings";
            chkSaveReadings.Size = new Size(129, 32);
            chkSaveReadings.TabIndex = 7;
            chkSaveReadings.Text = "Save Data";
            chkSaveReadings.UseVisualStyleBackColor = false;
            // 
            // rbActive
            // 
            rbActive.AutoSize = true;
            rbActive.BackColor = Color.Transparent;
            rbActive.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            rbActive.ForeColor = Color.Black;
            rbActive.Location = new Point(709, -1);
            rbActive.Name = "rbActive";
            rbActive.Size = new Size(93, 32);
            rbActive.TabIndex = 6;
            rbActive.Text = "Active";
            rbActive.UseVisualStyleBackColor = false;
            // 
            // rbFaux
            // 
            rbFaux.AutoSize = true;
            rbFaux.BackColor = Color.Transparent;
            rbFaux.Checked = true;
            rbFaux.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            rbFaux.ForeColor = Color.Black;
            rbFaux.Location = new Point(627, -1);
            rbFaux.Name = "rbFaux";
            rbFaux.Size = new Size(76, 32);
            rbFaux.TabIndex = 5;
            rbFaux.TabStop = true;
            rbFaux.Text = "Faux";
            rbFaux.UseVisualStyleBackColor = false;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(8, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panelTop);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panelTiles);
            splitContainer1.Panel2.Controls.Add(gridReadings);
            splitContainer1.Size = new Size(1045, 555);
            splitContainer1.SplitterDistance = 133;
            splitContainer1.TabIndex = 6;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label2.Location = new Point(577, 41);
            label2.Name = "label2";
            label2.Size = new Size(406, 74);
            label2.TabIndex = 9;
            label2.Text = "UNDER CONSTRUCTION\r\nCOMING SOON";
            // 
            // DeviceHubControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "DeviceHubControl";
            Padding = new Padding(8, 0, 8, 0);
            Size = new Size(1061, 555);
            Tag = "";
            panelTiles.ResumeLayout(false);
            tileScale.ResumeLayout(false);
            tileTH.ResumeLayout(false);
            tileTach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridReadings).EndInit();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private RoundButton btnStartLogging;
        private RoundButton btnStopLogging;
        private RoundButton btnScale;
        private RoundButton btnTH;
        private RoundButton btnTach;
        private TableLayoutPanel panelTiles;
        private Panel tileScale;
        private Panel tileTH;
        private Panel tileTach;
        private Label lblScaleValue;
        private Label lblScaleTitle;
        private Label lblTHValue;
        private Label lblTHTitle;
        private Label lblTachValue;
        private Label lblTachTitle;
        private DataGridView gridReadings;
        private DataGridViewTextBoxColumn TimestampUtc;
        private DataGridViewTextBoxColumn DeviceId;
        private DataGridViewTextBoxColumn Metric;
        private DataGridViewTextBoxColumn Value;
        private DataGridViewTextBoxColumn Unit;
        private DataGridViewTextBoxColumn ExtraJson;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private CustomControls.GradientPanel gradientPanel1;
        private CustomControls.RoundButton roundButton5;
        private CustomControls.RoundButton roundButton4;
        private CustomControls.RoundButton roundButton3;
        private CustomControls.RoundButton roundButton2;
        private CustomControls.GradientPanel panelTop;
        private CheckBox chkSaveReadings;
        private RadioButton rbActive;
        private RadioButton rbFaux;
        private Label label1;
        private SplitContainer splitContainer1;
        private Label label2;
    }
}
