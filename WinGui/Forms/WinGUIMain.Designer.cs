namespace AiNetStudio.WinGui.Forms
{
    partial class WinGUIMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinGUIMain));
            statusStrip1 = new StatusStrip();
            statusStripCopyright = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            statusStripDatabase = new ToolStripStatusLabel();
            statusStripVersion = new ToolStripStatusLabel();
            toolStrip1 = new ToolStrip();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusStripCopyright, toolStripStatusLabel1, statusStripDatabase, statusStripVersion });
            statusStrip1.Location = new Point(0, 624);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1242, 29);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusStripCopyright
            // 
            statusStripCopyright.DisplayStyle = ToolStripItemDisplayStyle.Text;
            statusStripCopyright.Font = new Font("Segoe UI", 9F);
            statusStripCopyright.Name = "statusStripCopyright";
            statusStripCopyright.Size = new Size(74, 23);
            statusStripCopyright.Text = "Copyright";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(30, 23);
            toolStripStatusLabel1.Text = "    ";
            // 
            // statusStripDatabase
            // 
            statusStripDatabase.Name = "statusStripDatabase";
            statusStripDatabase.Size = new Size(25, 23);
            statusStripDatabase.Text = "   ";
            // 
            // statusStripVersion
            // 
            statusStripVersion.DisplayStyle = ToolStripItemDisplayStyle.Text;
            statusStripVersion.Font = new Font("Segoe UI", 9F);
            statusStripVersion.ForeColor = Color.Black;
            statusStripVersion.ImageAlign = ContentAlignment.MiddleRight;
            statusStripVersion.Name = "statusStripVersion";
            statusStripVersion.Size = new Size(56, 23);
            statusStripVersion.Text = "version";
            statusStripVersion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // toolStrip1
            // 
            toolStrip1.BackgroundImageLayout = ImageLayout.Stretch;
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1242, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // WinGUIMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1242, 653);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1260, 700);
            Name = "WinGUIMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AiNetStudio®";
            Load += WinGUIMain_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusStripCopyright;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel statusStripDatabase;
        private ToolStrip toolStrip1;
        private ToolStripStatusLabel statusStripVersion;
    }
}