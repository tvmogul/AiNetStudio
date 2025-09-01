using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    public class LabelEx : Control
    {
        // Property Backing Fields
        private Pen _OutLinePen = new Pen(Color.Black);
        private Pen _BorderPen = new Pen(Color.Black);
        private SolidBrush _CenterBrush = new SolidBrush(Color.White);
        private SolidBrush _BackgroundBrush = new SolidBrush(Color.Transparent);
        private BorderType _BorderStyle = BorderType.None;
        private Bitmap _Image = null!;
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleCenter;
        private ContentAlignment _TextAlign = ContentAlignment.MiddleCenter;
        private Bitmap _TextPatternImage = null!;
        private PatternLayout _TextPatternImageLayout = PatternLayout.Stretch;
        private SolidBrush _ShadowBrush = new SolidBrush(Color.FromArgb(128, Color.Black));
        private Pen _ShadowPen = new Pen(Color.FromArgb(128, Color.Black));
        private Color _ShadowColor = Color.Black;
        private bool _ShowTextShadow = false;
        private ShadowArea _ShadowPosition = ShadowArea.BottomRight;
        private int _ShadowDepth = 2;
        private int _ShadowTransparency = 128;
        private ShadowDrawingType _ShadowStyle = ShadowDrawingType.FillShadow;
        private int _ForeColorTransparency = 255;
        private int _OutlineThickness = 1;

        // Enum Definitions
        public enum BorderType : int
        {
            None = 0,
            Squared = 1,
            Rounded = 2
        }

        public enum PatternLayout : int
        {
            Normal = 0,
            Center = 1,
            Stretch = 2,
            Tile = 3
        }

        public enum ShadowArea : int
        {
            TopLeft = 0,
            TopRight = 1,
            BottomLeft = 2,
            BottomRight = 3
        }

        public enum ShadowDrawingType : int
        {
            DrawShadow = 0,
            FillShadow = 1
        }

        // Constructor
        public LabelEx()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Font = new Font("Comic Sans MS", 18, FontStyle.Bold);
            this.Size = new Size(130, 38);
            this.ForeColor = Color.White;
            this.BackColor = Color.Transparent;
        }

        // OnPaint Override - Fixes ® symbol resizing while keeping it in its original position
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.FillRectangle(_BackgroundBrush, new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height));
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (!string.IsNullOrEmpty(this.Text))
            {
                string mainText = this.Text.TrimEnd('®');
                bool hasRegisteredSymbol = this.Text.EndsWith("®");

                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    Rectangle textRect = new Rectangle(this.Padding.Left, this.Padding.Top,
                        (this.ClientSize.Width - 1) - (this.Padding.Left + this.Padding.Right),
                        (this.ClientSize.Height - 1) - (this.Padding.Top + this.Padding.Bottom));

                    // Measure the main text (excluding ®)
                    SizeF mainTextSize = graphics.MeasureString(mainText, this.Font);
                    PointF textStart = new PointF(textRect.Left + (textRect.Width - mainTextSize.Width) / 2, textRect.Top);

                    // Draw the main text first (without ® symbol)
                    graphics.DrawString(mainText, this.Font, _CenterBrush, textStart);

                    if (hasRegisteredSymbol)
                    {
                        using (Font smallFont = new Font(this.Font.FontFamily, this.Font.Size * 0.5f, this.Font.Style))
                        {
                            // Measure and correctly position the ® symbol
                            SizeF smallRSize = graphics.MeasureString("®", smallFont);
                            PointF registeredPosition = new PointF(
                                textStart.X + mainTextSize.Width - smallRSize.Width / 2,
                                textStart.Y + (mainTextSize.Height * 0.2f) // Align it slightly raised
                            );

                            // Draw the smaller ® symbol
                            graphics.DrawString("®", smallFont, _CenterBrush, registeredPosition);
                        }
                    }
                }
            }
        }

        // Fix for missing ApplyShadowTransform
        private void ApplyShadowTransform(Graphics graphics)
        {
            float offsetX = _ShadowDepth;
            float offsetY = _ShadowDepth;

            if (_ShadowPosition == ShadowArea.TopLeft) { offsetX = -_ShadowDepth; offsetY = -_ShadowDepth; }
            else if (_ShadowPosition == ShadowArea.TopRight) { offsetX = _ShadowDepth; offsetY = -_ShadowDepth; }
            else if (_ShadowPosition == ShadowArea.BottomLeft) { offsetX = -_ShadowDepth; offsetY = _ShadowDepth; }

            graphics.TranslateTransform(offsetX, offsetY);
        }

        // Fix for missing ResetShadowTransform
        private void ResetShadowTransform(Graphics graphics)
        {
            graphics.ResetTransform();
        }

        // Full Designer Serialization Fix for .NET 9
        [Category("Appearance"), Description("The style used to draw the shadow.")]
        [Browsable(true), DefaultValue(typeof(ShadowDrawingType), "FillShadow")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ShadowDrawingType ShadowStyle
        {
            get => _ShadowStyle;
            set
            {
                _ShadowStyle = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Determines whether a shadow is drawn behind the text.")]
        [Browsable(true), DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowTextShadow
        {
            get => _ShowTextShadow;
            set
            {
                _ShowTextShadow = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Depth of the text shadow.")]
        [Browsable(true), DefaultValue(2)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ShadowDepth
        {
            get => _ShadowDepth;
            set
            {
                _ShadowDepth = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Color of the text shadow.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ShadowColor
        {
            get => _ShadowColor;
            set
            {
                _ShadowColor = value;
                _ShadowBrush = new SolidBrush(Color.FromArgb(_ShadowTransparency, value));
                _ShadowPen = new Pen(Color.FromArgb(_ShadowTransparency, value));
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Text Alignment")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ContentAlignment TextAlign
        {
            get => _TextAlign;
            set
            {
                _TextAlign = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Transparency of the shadow color.")]
        [Browsable(true), DefaultValue(128)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ShadowTransparency
        {
            get => _ShadowTransparency;
            set
            {
                _ShadowTransparency = value;
                _ShadowBrush = new SolidBrush(Color.FromArgb(value, _ShadowColor));
                _ShadowPen = new Pen(Color.FromArgb(value, _ShadowColor));
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Position of the shadow relative to the text.")]
        [Browsable(true), DefaultValue(typeof(ShadowArea), "BottomRight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ShadowArea ShadowPosition
        {
            get => _ShadowPosition;
            set
            {
                _ShadowPosition = value;
                Invalidate();
            }
        }
    }
}
