// WinGui/Controls/ChromeToolStripComboBox.cs
// .NET 9 WinForms - ToolStripComboBox-derived control with Chrome-like rounded blue focus ring.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AiNetStudio.WinGui.Controls
{
    public class ChromeToolStripComboBox : ToolStripComboBox, IDisposable
    {
        private BorderPainter? _painter;

        //private static readonly Color DefaultFocusBlue = Color.FromArgb(94, 158, 214); //blue

        //darker safety-vest orange (high-visibility, construction-vest style) is usually around
        //private static readonly Color DefaultFocusBlue = Color.FromArgb(255, 102, 0);

        //fluorescent neon-like orange (pushing toward #FF4500) for even stronger emphasis in UI
        private static readonly Color DefaultFocusBlue = Color.FromArgb(255, 69, 0);

        private static readonly Color DefaultBorderGray = Color.FromArgb(200, 200, 200);
        private static readonly Color DefaultHoverGray = Color.FromArgb(160, 160, 160);

        public ChromeToolStripComboBox()
        {
            AutoSize = false;
            Size = new Size(300, 32);
        }

        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);
            if (c is ComboBox combo)
            {
                combo.HandleCreated += Combo_HandleCreated;
                combo.HandleDestroyed += Combo_HandleDestroyed;
                combo.GotFocus += Combo_GotFocus;
                combo.LostFocus += Combo_LostFocus;
                combo.MouseEnter += Combo_MouseEnter;
                combo.MouseLeave += Combo_MouseLeave;
                combo.Resize += Combo_Resize;

                if (combo.IsHandleCreated)
                {
                    EnsurePainterAttached(combo);
                }
            }
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            if (c is ComboBox combo)
            {
                combo.HandleCreated -= Combo_HandleCreated;
                combo.HandleDestroyed -= Combo_HandleDestroyed;
                combo.GotFocus -= Combo_GotFocus;
                combo.LostFocus -= Combo_LostFocus;
                combo.MouseEnter -= Combo_MouseEnter;
                combo.MouseLeave -= Combo_MouseLeave;
                combo.Resize -= Combo_Resize;
            }
            base.OnUnsubscribeControlEvents(c);
        }

        private void Combo_HandleCreated(object? sender, EventArgs e)
        {
            if (sender is ComboBox combo)
            {
                EnsurePainterAttached(combo);
            }
        }

        private void Combo_HandleDestroyed(object? sender, EventArgs e)
        {
            _painter?.Detach();
        }

        private void Combo_GotFocus(object? sender, EventArgs e)
        {
            if (_painter != null) { _painter.Focused = true; _painter.Invalidate(); }
        }

        private void Combo_LostFocus(object? sender, EventArgs e)
        {
            if (_painter != null) { _painter.Focused = false; _painter.Invalidate(); }
        }

        private void Combo_MouseEnter(object? sender, EventArgs e)
        {
            if (_painter != null) { _painter.Hover = true; _painter.Invalidate(); }
        }

        private void Combo_MouseLeave(object? sender, EventArgs e)
        {
            if (_painter != null) { _painter.Hover = false; _painter.Invalidate(); }
        }

        private void Combo_Resize(object? sender, EventArgs e)
        {
            _painter?.UpdateRegion();
            _painter?.ApplyTextMargins();
            _painter?.Invalidate();
        }

        private void EnsurePainterAttached(ComboBox combo)
        {
            if (_painter == null)
            {
                _painter = new BorderPainter(combo, this);
            }
            else
            {
                _painter.Attach(combo);
            }

            // keep the combo flat so the border we draw is visible/clean
            combo.FlatStyle = FlatStyle.Flat;

            // propagate current settings into painter on attach
            _painter.CornerRadius = CornerRadius;
            _painter.FocusBorderColor = FocusBorderColor;
            _painter.BorderColor = BorderColor;
            _painter.HoverBorderColor = HoverBorderColor;
            _painter.FocusBorderThickness = FocusBorderThickness;
            _painter.BorderThickness = BorderThickness;
            _painter.EnableFocusGlow = EnableFocusGlow;
            _painter.GlowRadius = GlowRadius;
            _painter.GlowMaxAlpha = GlowMaxAlpha;
            _painter.GlowSpeed = GlowSpeed;
            _painter.TextLeftPadding = TextLeftPadding;
            _painter.ApplyTextMargins();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboBox ComboBox => base.ComboBox;

        // ---- Style properties (designer-friendly with DefaultValue + ShouldSerialize/Reset) ----

        [Browsable(true)]
        [DefaultValue(10)]
        public int CornerRadius
        {
            get => _painter?.CornerRadius ?? 10;
            set { if (_painter != null) { _painter.CornerRadius = value; _painter.UpdateRegion(); _painter.Invalidate(); } }
        }
        public bool ShouldSerializeCornerRadius() => CornerRadius != 10;
        public void ResetCornerRadius() { CornerRadius = 10; }

        [Browsable(true)]
        public Color FocusBorderColor
        {
            get => _painter?.FocusBorderColor ?? DefaultFocusBlue;
            set { if (_painter != null) { _painter.FocusBorderColor = value; _painter.Invalidate(); } }
        }
        public bool ShouldSerializeFocusBorderColor() => FocusBorderColor != DefaultFocusBlue;
        public void ResetFocusBorderColor() { FocusBorderColor = DefaultFocusBlue; }

        [Browsable(true)]
        public Color BorderColor
        {
            get => _painter?.BorderColor ?? DefaultBorderGray;
            set { if (_painter != null) { _painter.BorderColor = value; _painter.Invalidate(); } }
        }
        public bool ShouldSerializeBorderColor() => BorderColor != DefaultBorderGray;
        public void ResetBorderColor() { BorderColor = DefaultBorderGray; }

        [Browsable(true)]
        public Color HoverBorderColor
        {
            get => _painter?.HoverBorderColor ?? DefaultHoverGray;
            set { if (_painter != null) { _painter.HoverBorderColor = value; _painter.Invalidate(); } }
        }
        public bool ShouldSerializeHoverBorderColor() => HoverBorderColor != DefaultHoverGray;
        public void ResetHoverBorderColor() { HoverBorderColor = DefaultHoverGray; }

        [Browsable(true)]
        [DefaultValue(2)]
        public int FocusBorderThickness
        {
            get => _painter?.FocusBorderThickness ?? 2;
            set { if (_painter != null) { _painter.FocusBorderThickness = value; _painter.Invalidate(); } }
        }
        public bool ShouldSerializeFocusBorderThickness() => FocusBorderThickness != 2;
        public void ResetFocusBorderThickness() { FocusBorderThickness = 2; }

        [Browsable(true)]
        [DefaultValue(1)]
        public int BorderThickness
        {
            get => _painter?.BorderThickness ?? 1;
            set { if (_painter != null) { _painter.BorderThickness = value; _painter.Invalidate(); } }
        }
        public bool ShouldSerializeBorderThickness() => BorderThickness != 1;
        public void ResetBorderThickness() { BorderThickness = 1; }

        [Browsable(true)]
        [DefaultValue(true)]
        public bool EnableFocusGlow
        {
            get => _painter?.EnableFocusGlow ?? true;
            set { if (_painter != null) { _painter.EnableFocusGlow = value; _painter.Invalidate(); } }
        }
        public bool ShouldSerializeEnableFocusGlow() => EnableFocusGlow != true;
        public void ResetEnableFocusGlow() { EnableFocusGlow = true; }

        [Browsable(true)]
        [DefaultValue(6)]
        public int GlowRadius
        {
            get => _painter?.GlowRadius ?? 6;
            set { if (_painter != null) { _painter.GlowRadius = Math.Max(0, value); _painter.Invalidate(); } }
        }
        public bool ShouldSerializeGlowRadius() => GlowRadius != 6;
        public void ResetGlowRadius() { GlowRadius = 6; }

        [Browsable(true)]
        [DefaultValue(64)]
        public int GlowMaxAlpha
        {
            get => _painter?.GlowMaxAlpha ?? 64;
            set { if (_painter != null) { _painter.GlowMaxAlpha = Math.Max(0, Math.Min(255, value)); _painter.Invalidate(); } }
        }
        public bool ShouldSerializeGlowMaxAlpha() => GlowMaxAlpha != 64;
        public void ResetGlowMaxAlpha() { GlowMaxAlpha = 64; }

        [Browsable(true)]
        [DefaultValue(0.12f)]
        public float GlowSpeed
        {
            get => _painter?.GlowSpeed ?? 0.12f;
            set { if (_painter != null) { _painter.GlowSpeed = Math.Max(0.01f, Math.Min(1.0f, value)); _painter.Invalidate(); } }
        }
        public bool ShouldSerializeGlowSpeed() => Math.Abs(GlowSpeed - 0.12f) > float.Epsilon;
        public void ResetGlowSpeed() { GlowSpeed = 0.12f; }

        [Browsable(true)]
        [DefaultValue(8)]
        public int TextLeftPadding
        {
            get => _painter?.TextLeftPadding ?? 8;
            set { if (_painter != null) { _painter.TextLeftPadding = Math.Max(0, value); _painter.ApplyTextMargins(); _painter.Invalidate(); } }
        }
        public bool ShouldSerializeTextLeftPadding() => TextLeftPadding != 8;
        public void ResetTextLeftPadding() { TextLeftPadding = 8; }

        public void SelectAllText() => ComboBox?.SelectAll();

        public void Dispose()
        {
            _painter?.Dispose();
            _painter = null;
        }

        private sealed class BorderPainter : NativeWindow, IDisposable
        {
            private ComboBox? _combo;
            private readonly ChromeToolStripComboBox _owner;

            public bool Focused { get; set; }
            public bool Hover { get; set; }

            public int CornerRadius { get; set; } = 10;
            public int FocusBorderThickness { get; set; } = 2;
            public int BorderThickness { get; set; } = 1;
            public Color FocusBorderColor { get; set; } = DefaultFocusBlue;
            public Color BorderColor { get; set; } = DefaultBorderGray;
            public Color HoverBorderColor { get; set; } = DefaultHoverGray;

            // NEW: Dropdown button background and triangle (arrow) colors.
            //      Keeps the edit area untouched (white); only colors the right-side button area.
            public Color DropButtonBackColor { get; set; } = Color.FromArgb(255, 140, 0); // orange background
            public Color ArrowColor { get; set; } = Color.White; // triangle color (white or set to Color.Black)

            // Glow animation settings
            public bool EnableFocusGlow { get; set; } = true;
            public int GlowRadius { get; set; } = 6;          // how many outer rings
            public int GlowMaxAlpha { get; set; } = 64;       // peak alpha for outermost ring
            public float GlowSpeed { get; set; } = 0.12f;     // radians per tick (0.01..1.0)

            // Text left padding in the inner EDIT control
            public int TextLeftPadding { get; set; } = 8;

            private readonly System.Windows.Forms.Timer _animTimer;
            private float _glowPhase; // animated phase [0..2π]

            public BorderPainter(ComboBox combo, ChromeToolStripComboBox owner)
            {
                _owner = owner;
                _animTimer = new System.Windows.Forms.Timer { Interval = 33 }; // ~30 FPS
                _animTimer.Tick += (s, e) =>
                {
                    _glowPhase += GlowSpeed;
                    if (_glowPhase > MathF.PI * 2f) _glowPhase -= MathF.PI * 2f;
                    Invalidate();
                };

                Attach(combo);
            }

            public void StartGlow()
            {
                // Animation removed
            }

            public void StopGlow()
            {
                // Animation removed
            }

            public void Attach(ComboBox combo)
            {
                _combo = combo;
                if (combo.IsHandleCreated)
                {
                    AssignHandle(combo.Handle);
                    UpdateRegion();
                    ApplyTextMargins();
                }
            }

            public void Detach()
            {
                if (Handle != IntPtr.Zero)
                {
                    ReleaseHandle();
                }
                _combo = null;
            }

            public void UpdateRegion()
            {
                if (_combo == null || !_combo.IsHandleCreated) return;
                try
                {
                    IntPtr rgn = CreateRoundRectRgn(0, 0, _combo.Width + 1, _combo.Height + 1, CornerRadius * 2, CornerRadius * 2);
                    SetWindowRgn(_combo.Handle, rgn, true);
                    // ownership of rgn transfers to window; no DeleteObject here
                }
                catch { /* ignore */ }
            }

            public void ApplyTextMargins()
            {
                if (_combo == null || !_combo.IsHandleCreated) return;
                try
                {
                    // Only applies to DropDown and Simple; not DropDownList (no edit control).
                    if (_combo.DropDownStyle == ComboBoxStyle.DropDown || _combo.DropDownStyle == ComboBoxStyle.Simple)
                    {
                        var info = new COMBOBOXINFO();
                        info.cbSize = (uint)Marshal.SizeOf<COMBOBOXINFO>();
                        if (GetComboBoxInfo(_combo.Handle, ref info))
                        {
                            IntPtr editHwnd = info.hwndItem; // edit control
                            if (editHwnd != IntPtr.Zero)
                            {
                                int left = TextLeftPadding;
                                int right = 0;
                                // wParam: EC_LEFTMARGIN | EC_RIGHTMARGIN, lParam: LoWord=left, HiWord=right
                                int wParam = EC_LEFTMARGIN | EC_RIGHTMARGIN;
                                int lParam = (right << 16) | (left & 0xFFFF);
                                SendMessage(editHwnd, EM_SETMARGINS, new IntPtr(wParam), new IntPtr(lParam));
                            }
                        }
                    }
                }
                catch { /* ignore */ }
            }

            public void Invalidate()
            {
                if (_combo == null || !_combo.IsHandleCreated) return;
                _combo.Invalidate();
            }

            protected override void WndProc(ref Message m)
            {
                const int WM_PAINT = 0x000F;
                const int WM_NCPAINT = 0x0085;
                const int WM_PRINTCLIENT = 0x0318;
                const int WM_SIZE = 0x0005;
                const int WM_NCDESTROY = 0x0082;

                switch (m.Msg)
                {
                    case WM_SIZE:
                        base.WndProc(ref m);
                        UpdateRegion();
                        ApplyTextMargins();
                        DrawBorder();
                        DrawDropButton(); // color the right-side button orange and draw a white/black triangle
                        return;

                    case WM_PAINT:
                    case WM_NCPAINT:
                    case WM_PRINTCLIENT:
                        base.WndProc(ref m);
                        DrawBorder();
                        DrawDropButton(); // ensure our button overlay is drawn after default painting
                        return;

                    case WM_NCDESTROY:
                        base.WndProc(ref m);
                        Detach();
                        return;

                    default:
                        base.WndProc(ref m);
                        return;
                }
            }

            private void DrawBorder()
            {
                if (_combo == null || !_combo.IsHandleCreated) return;
                try
                {
                    using var g = Graphics.FromHwnd(_combo.Handle);
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    var rect = new Rectangle(0, 0, _combo.Width - 1, _combo.Height - 1);
                    using var path = GetRoundRect(rect, CornerRadius);

                    var color = Focused ? FocusBorderColor : (Hover ? HoverBorderColor : BorderColor);
                    var thickness = Focused ? FocusBorderThickness : BorderThickness;

                    using (var pen = new Pen(color, thickness))
                        g.DrawPath(pen, path);
                }
                catch { /* ignore paint exceptions */ }
            }

            private void DrawDropButton()
            {
                if (_combo == null || !_combo.IsHandleCreated) return;
                try
                {
                    var info = new COMBOBOXINFO();
                    info.cbSize = (uint)Marshal.SizeOf<COMBOBOXINFO>();
                    if (!GetComboBoxInfo(_combo.Handle, ref info)) return;

                    // Button rect (system-defined area on the right where the dropdown arrow lives)
                    Rectangle btnRect = new Rectangle(
                        info.rcButton.Left,
                        info.rcButton.Top,
                        info.rcButton.Right - info.rcButton.Left,
                        info.rcButton.Bottom - info.rcButton.Top
                    );

                    using var g = Graphics.FromHwnd(_combo.Handle);

                    // Fill the button area with ORANGE (do NOT touch the edit portion – remains white)
                    using (var back = new SolidBrush(DropButtonBackColor))
                        g.FillRectangle(back, btnRect);

                    // Draw the triangle (white by default; set ArrowColor to Color.Black if desired)
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    int margin = Math.Max(2, btnRect.Height / 4);
                    int centerX = btnRect.Left + btnRect.Width / 2;
                    int topY = btnRect.Top + margin;
                    int bottomY = btnRect.Bottom - margin;

                    Point[] tri =
                    {
                        new Point(centerX - (btnRect.Height/5), topY + (btnRect.Height/4)), // left
                        new Point(centerX + (btnRect.Height/5), topY + (btnRect.Height/4)), // right
                        new Point(centerX, bottomY - (btnRect.Height/6))                    // bottom tip
                    };

                    using (var brush = new SolidBrush(ArrowColor))
                        g.FillPolygon(brush, tri);
                }
                catch { /* ignore */ }
            }

            private static GraphicsPath GetRoundRect(Rectangle bounds, int radius)
            {
                int d = Math.Max(0, radius) * 2;
                var path = new GraphicsPath();
                if (d <= 0) { path.AddRectangle(bounds); return path; }

                path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
                path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
                path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
                path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }

            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

            [DllImport("user32.dll")]
            private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

            [DllImport("user32.dll", SetLastError = true)]
            private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref COMBOBOXINFO info);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            private const int EM_SETMARGINS = 0x00D3;
            private const int EC_LEFTMARGIN = 0x0001;
            private const int EC_RIGHTMARGIN = 0x0002;

            [StructLayout(LayoutKind.Sequential)]
            private struct COMBOBOXINFO
            {
                public uint cbSize;
                public RECT rcItem;
                public RECT rcButton;
                public int stateButton;
                public IntPtr hwndCombo;
                public IntPtr hwndItem;   // Edit control
                public IntPtr hwndList;   // Listbox
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            public void Dispose()
            {
                Detach();
                _animTimer.Dispose();
            }
        }
    }
}
