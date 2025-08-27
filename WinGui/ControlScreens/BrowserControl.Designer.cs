namespace AiNetStudio.WinGui.Controls
{
    partial class BrowserControl
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
            btnGo = new Button();
            pnlBrowser = new Panel();
            txtURL = new TextBox();
            toolStrip1 = new ToolStrip();
            SuspendLayout();
            // 
            // btnGo
            // 
            btnGo.Location = new Point(499, 43);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(94, 29);
            btnGo.TabIndex = 0;
            btnGo.Text = "Go";
            btnGo.UseVisualStyleBackColor = true;
            // 
            // pnlBrowser
            // 
            pnlBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlBrowser.Location = new Point(37, 88);
            pnlBrowser.Name = "pnlBrowser";
            pnlBrowser.Size = new Size(1174, 253);
            pnlBrowser.TabIndex = 1;
            // 
            // txtURL
            // 
            txtURL.Location = new Point(37, 43);
            txtURL.Name = "txtURL";
            txtURL.Size = new Size(456, 27);
            txtURL.TabIndex = 2;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1260, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // BrowserControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(toolStrip1);
            Controls.Add(txtURL);
            Controls.Add(pnlBrowser);
            Controls.Add(btnGo);
            DoubleBuffered = true;
            Name = "BrowserControl";
            Size = new Size(1260, 365);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtComapny;
        private Button btnGo;
        private Panel pnlBrowser;
        private TextBox txtURL;
        private ToolStrip toolStrip1;
        //private CustomControls.RoundButton btnCreateCompany;
        //private CustomControls.RoundButton btnAddAccounts;
        //private CustomControls.RoundButton btnMarketing;
    }
}
