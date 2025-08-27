//
////////////////////////////////////////////////////////////////
//
/**************************************************************
**        __                                          __
**     __/_/__________________________________________\_\__
**  __|_                                                  |__
** (___O)     Ouslan, Inc.                              (O___)
**(_____O)	  www.software-rus.com         			   (O_____)
**(_____O)	  Author: Bill SerGio, Infomercial King™   (O_____)
** (__O)                                                (O__)
**    |___________________________________________________|
**
****************************************************************/
/*
 * (C) Copyright 1991-2025 Ouslan,Inc, All Rights Reserved Worldwide.
 * software-rus.com   
 * tvmogul1@yahoo.com  
 *
 */

//using System;
//using System.ComponentModel;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Windows.Forms;

//namespace CustomControls
//{
//    public class GradientPanel : Panel
//    {
//        private Color _gradientTop = Color.White;
//        private Color _gradientBottom = Color.Gray;

//        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
//        private static extern IntPtr CreateRoundRectRgn
//        (
//            int nLeftRect, // X-coordinate of upper-left corner or padding at start
//            int nTopRect,// Y-coordinate of upper-left corner or padding at the top of the textbox
//            int nRightRect, // X-coordinate of lower-right corner or Width of the object
//            int nBottomRect,// Y-coordinate of lower-right corner or Height of the object
//                            //RADIUS, how round do you want it to be?
//            int nheightRect, //height of ellipse 
//            int nweightRect //width of ellipse
//        );

//        protected override void OnResize(EventArgs e)
//        {
//            base.OnResize(e);
//            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 3, this.Width, this.Height, 20, 20));
//        }

//        [Browsable(true)]
//        [Category("Appearance")]
//        [Description("The top color of the gradient.")]
//        [DefaultValue(typeof(Color), "White")]
//        public Color GradientTop
//        {
//            get => _gradientTop;
//            set
//            {
//                _gradientTop = value;
//                Invalidate(); // Forces the panel to repaint when the color changes
//            }
//        }

//        [Browsable(true)]
//        [Category("Appearance")]
//        [Description("The bottom color of the gradient.")]
//        [DefaultValue(typeof(Color), "Gray")]
//        public Color GradientBottom
//        {
//            get => _gradientBottom;
//            set
//            {
//                _gradientBottom = value;
//                Invalidate(); // Forces the panel to repaint when the color changes
//            }
//        }

//        protected override void OnPaint(PaintEventArgs e)
//        {
//            using (LinearGradientBrush linear = new LinearGradientBrush(
//                this.ClientRectangle, this.GradientTop, this.GradientBottom, 90F)) // Changed from 80F to 90F for vertical gradient
//            {
//                e.Graphics.FillRectangle(linear, this.ClientRectangle);
//            }

//            base.OnPaint(e);
//        }
//    }
//}


using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    public class GradientPanel : Panel
    {
        private Color _gradientTop = Color.White;
        private Color _gradientBottom = Color.Gray;
        private int _cornerRadius = 15;

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // X-coordinate of upper-left corner or padding at start
            int nTopRect,// Y-coordinate of upper-left corner or padding at the top of the textbox
            int nRightRect, // X-coordinate of lower-right corner or Width of the object
            int nBottomRect,// Y-coordinate of lower-right corner or Height of the object
                            //RADIUS, how round do you want it to be?
            int nheightRect, //height of ellipse 
            int nweightRect //width of ellipse
        );

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 3, this.Width, this.Height, _cornerRadius, _cornerRadius));
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The top color of the gradient.")]
        [DefaultValue(typeof(Color), "White")]
        public Color GradientTop
        {
            get => _gradientTop;
            set
            {
                _gradientTop = value;
                Invalidate(); // Forces the panel to repaint when the color changes
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The bottom color of the gradient.")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color GradientBottom
        {
            get => _gradientBottom;
            set
            {
                _gradientBottom = value;
                Invalidate(); // Forces the panel to repaint when the color changes
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The corner radius of the panel.")]
        [DefaultValue(15)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                Invalidate(); // Redraw with new radius
                OnResize(EventArgs.Empty); // Reapply region
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (LinearGradientBrush linear = new LinearGradientBrush(
                this.ClientRectangle, this.GradientTop, this.GradientBottom, 90F)) // Changed from 80F to 90F for vertical gradient
            {
                e.Graphics.FillRectangle(linear, this.ClientRectangle);
            }

            base.OnPaint(e);
        }
    }
}
