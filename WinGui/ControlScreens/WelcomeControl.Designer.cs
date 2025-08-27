namespace AiNetStudio.WinGui.Controls
{
    partial class WelcomeControl
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
            pnlBrowser = new Panel();
            SuspendLayout();
            // 
            // pnlBrowser
            // 
            pnlBrowser.Location = new Point(32, 39);
            pnlBrowser.Name = "pnlBrowser";
            pnlBrowser.Size = new Size(938, 328);
            pnlBrowser.TabIndex = 0;
            // 
            // WelcomeControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlBrowser);
            DoubleBuffered = true;
            Name = "WelcomeControl";
            Size = new Size(1260, 700);
            ResumeLayout(false);
        }

        #endregion
        private TextBox txtComapny;
        private Panel pnlBrowser;
        //private CustomControls.RoundButton btnCreateCompany;
        //private CustomControls.RoundButton btnAddAccounts;
        //private CustomControls.RoundButton btnMarketing;
    }
}
