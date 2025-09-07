namespace AiNetStudio.WinGui.ControlScreens
{
    partial class MultiTabBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiTabBrowser));
            imgListBlack = new ImageList(components);
            imgListOrange = new ImageList(components);
            SuspendLayout();
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
            imgListBlack.Images.SetKeyName(12, "pa");
            // 
            // imgListOrange
            // 
            imgListOrange.ColorDepth = ColorDepth.Depth32Bit;
            imgListOrange.ImageStream = (ImageListStreamer)resources.GetObject("imgListOrange.ImageStream");
            imgListOrange.TransparentColor = Color.Transparent;
            imgListOrange.Images.SetKeyName(0, "ai");
            imgListOrange.Images.SetKeyName(1, "back");
            imgListOrange.Images.SetKeyName(2, "close");
            imgListOrange.Images.SetKeyName(3, "down");
            imgListOrange.Images.SetKeyName(4, "forward");
            imgListOrange.Images.SetKeyName(5, "go");
            imgListOrange.Images.SetKeyName(6, "reload");
            imgListOrange.Images.SetKeyName(7, "up");
            imgListOrange.Images.SetKeyName(8, "add");
            imgListOrange.Images.SetKeyName(9, "home");
            imgListOrange.Images.SetKeyName(10, "tv");
            imgListOrange.Images.SetKeyName(11, "pa");
            // 
            // MultiTabBrowser
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "MultiTabBrowser";
            Size = new Size(1004, 501);
            ResumeLayout(false);
        }

        #endregion

        private ImageList imgListBlack;
        private ImageList imgListOrange;
    }
}
