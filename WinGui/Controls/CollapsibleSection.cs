using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace AiNetStudio.WinGui.Controls
{
    public class CollapsibleSection : UserControl
    {
        private readonly Panel _header;
        private readonly Label _title;
        private readonly Button _toggle;
        private readonly Panel _contentHost;

        private int _expandedHeight = 180;
        private const int HeaderHeight = 36;

        public CollapsibleSection()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            BackColor = Color.White;

            _header = new Panel
            {
                Dock = DockStyle.Top,
                Height = HeaderHeight,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(10, 8, 10, 8)
            };

            _title = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(SystemFonts.MenuFont, FontStyle.Bold),
                Text = "Section"
            };

            _toggle = new Button
            {
                Dock = DockStyle.Right,
                Width = 28,
                FlatStyle = FlatStyle.Flat,
                Text = "▾",
                TabStop = false
            };
            _toggle.FlatAppearance.BorderSize = 0;
            _toggle.Click += (_, __) => IsExpanded = !IsExpanded;

            _header.Controls.Add(_title);
            _header.Controls.Add(_toggle);

            _contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Controls.Add(_contentHost);
            Controls.Add(_header);

            Height = HeaderHeight + 1; // collapsed by default
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Title
        {
            get => _title.Text;
            set => _title.Text = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control.ControlCollection Content => _contentHost.Controls;

        private bool _isExpanded;
        [Browsable(true)]
        [DefaultValue(false)]
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                ApplyExpandedState();
                ExpandedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [DefaultValue(180)]
        public int ExpandedHeight
        {
            get => _expandedHeight;
            set
            {
                _expandedHeight = Math.Max(HeaderHeight + 1, value);
                if (IsExpanded) Height = _expandedHeight;
            }
        }

        public event EventHandler? ExpandedChanged;

        private void ApplyExpandedState()
        {
            _toggle.Text = _isExpanded ? "▴" : "▾";
            Height = _isExpanded ? _expandedHeight : HeaderHeight + 1;
        }
    }
}
