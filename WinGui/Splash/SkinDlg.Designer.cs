using CustomControls;

namespace AiNetStudio.WinGui.Dialogs
{
    partial class SkinDlg
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
            picGif = new PictureBox();
            exLogo = new LabelEx();
            ex1 = new LabelEx();
            pnlLogin = new Panel();
            btnClose = new Button();
            txtPassword = new TextBox();
            btnLogin = new Button();
            txtUsername = new TextBox();
            lblUsername = new Label();
            lblPassword = new Label();
            exLogin2 = new LabelEx();
            exLogin1 = new LabelEx();
            ((System.ComponentModel.ISupportInitialize)picGif).BeginInit();
            pnlLogin.SuspendLayout();
            SuspendLayout();
            // 
            // picGif
            // 
            picGif.BackColor = Color.Transparent;
            picGif.BackgroundImageLayout = ImageLayout.Stretch;
            picGif.Dock = DockStyle.Fill;
            picGif.Location = new Point(0, 0);
            picGif.Name = "picGif";
            picGif.Size = new Size(800, 450);
            picGif.TabIndex = 0;
            picGif.TabStop = false;
            // 
            // exLogo
            // 
            exLogo.BackColor = Color.Transparent;
            exLogo.Font = new Font("Adobe Gothic Std B", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 128);
            exLogo.ForeColor = Color.White;
            exLogo.Location = new Point(93, 409);
            exLogo.Name = "exLogo";
            exLogo.ShadowColor = Color.Black;
            exLogo.ShadowStyle = LabelEx.ShadowDrawingType.DrawShadow;
            exLogo.Size = new Size(203, 59);
            exLogo.TabIndex = 12;
            exLogo.Text = "AiNetStudio®";
            exLogo.TextAlign = ContentAlignment.BottomCenter;
            // 
            // ex1
            // 
            ex1.BackColor = Color.Transparent;
            ex1.Font = new Font("Adobe Gothic Std B", 18F, FontStyle.Bold, GraphicsUnit.Point, 128);
            ex1.ForeColor = Color.White;
            ex1.Location = new Point(41, 209);
            ex1.Name = "ex1";
            ex1.ShadowColor = Color.Black;
            ex1.ShadowDepth = 36;
            ex1.ShowTextShadow = true;
            ex1.Size = new Size(725, 83);
            ex1.TabIndex = 13;
            ex1.Text = "ex1";
            ex1.TextAlign = ContentAlignment.BottomLeft;
            // 
            // pnlLogin
            // 
            pnlLogin.BackColor = Color.Transparent;
            pnlLogin.BackgroundImageLayout = ImageLayout.Stretch;
            pnlLogin.Controls.Add(btnClose);
            pnlLogin.Controls.Add(txtPassword);
            pnlLogin.Controls.Add(btnLogin);
            pnlLogin.Controls.Add(txtUsername);
            pnlLogin.Controls.Add(lblUsername);
            pnlLogin.Controls.Add(lblPassword);
            pnlLogin.Location = new Point(417, 0);
            pnlLogin.Name = "pnlLogin";
            pnlLogin.Size = new Size(383, 155);
            pnlLogin.TabIndex = 14;
            pnlLogin.Visible = false;
            // 
            // btnClose
            // 
            btnClose.DialogResult = DialogResult.OK;
            btnClose.FlatAppearance.MouseDownBackColor = SystemColors.ControlDarkDark;
            btnClose.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(255, 109);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 29);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = SystemColors.Menu;
            txtPassword.Font = new Font("Segoe UI", 10.2F);
            txtPassword.Location = new Point(134, 66);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(215, 30);
            txtPassword.TabIndex = 5;
            // 
            // btnLogin
            // 
            btnLogin.FlatAppearance.MouseDownBackColor = SystemColors.ControlDarkDark;
            btnLogin.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(134, 109);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(94, 29);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            // 
            // txtUsername
            // 
            txtUsername.BackColor = SystemColors.Menu;
            txtUsername.Font = new Font("Segoe UI", 10.2F);
            txtUsername.Location = new Point(134, 28);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(215, 30);
            txtUsername.TabIndex = 4;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.BackColor = Color.Transparent;
            lblUsername.Font = new Font("Segoe UI", 12F);
            lblUsername.ForeColor = Color.White;
            lblUsername.Location = new Point(31, 31);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(99, 28);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.BackColor = Color.Transparent;
            lblPassword.Font = new Font("Segoe UI", 12F);
            lblPassword.ForeColor = Color.White;
            lblPassword.Location = new Point(36, 69);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(93, 28);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password";
            // 
            // exLogin2
            // 
            exLogin2.BackColor = Color.Transparent;
            exLogin2.Font = new Font("Arial Narrow", 10.8F, FontStyle.Bold);
            exLogin2.ForeColor = Color.White;
            exLogin2.Location = new Point(405, 355);
            exLogin2.Name = "exLogin2";
            exLogin2.ShadowColor = Color.Black;
            exLogin2.Size = new Size(365, 56);
            exLogin2.TabIndex = 7;
            exLogin2.Text = "exLogin2";
            exLogin2.TextAlign = ContentAlignment.TopCenter;
            // 
            // exLogin1
            // 
            exLogin1.BackColor = Color.Transparent;
            exLogin1.Font = new Font("Arial Narrow", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exLogin1.ForeColor = Color.White;
            exLogin1.Location = new Point(381, 178);
            exLogin1.Name = "exLogin1";
            exLogin1.ShadowColor = Color.Black;
            exLogin1.Size = new Size(389, 99);
            exLogin1.TabIndex = 6;
            exLogin1.Text = "exLogin1";
            exLogin1.TextAlign = ContentAlignment.TopCenter;
            // 
            // SkinDlg
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(800, 450);
            Controls.Add(exLogin2);
            Controls.Add(pnlLogin);
            Controls.Add(exLogin1);
            Controls.Add(ex1);
            Controls.Add(exLogo);
            Controls.Add(picGif);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "SkinDlg";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginDlg";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)picGif).EndInit();
            pnlLogin.ResumeLayout(false);
            pnlLogin.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picGif;
        private Label label2;
        private Label label1;
        private LabelEx exLogo;
        private LabelEx ex1;
        private Panel pnlLogin;
        private Button btnClose;
        private TextBox txtPassword;
        private Button btnLogin;
        private TextBox txtUsername;
        private Label lblUsername;
        private Label lblPassword;
        private LabelEx exLogin1;
        private LabelEx exLogin2;
    }
}