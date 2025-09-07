using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AiNetStudio.WinGui.Controls
{
    [DesignerCategory("Code")]
    [DesignTimeVisible(true)]
    [ToolboxItem(true)]
    public class SlidingPanel : Panel
    {
        public SlidingPanel()
        {
            AutoScroll = true;
            BackColor = Color.White;
        }

        public Panel AddSection(string title, Control? content = null, bool expanded = false, int expandedHeight = 200)
        {
            var sec = new Panel
            {
                Dock = DockStyle.Top,
                // Title = title,
                // IsExpanded = expanded,
                // ExpandedHeight = expandedHeight,
                Margin = new Padding(0, 0, 0, 8),
                Height = expanded ? expandedHeight : 30
            };
            if (content != null)
            {
                content.Dock = DockStyle.Fill;
                // sec.Content.Add(content);
                sec.Controls.Add(content);
            }
            // sec.ExpandedChanged += (_, __) => Relayout();
            Controls.Add(sec);
            Controls.SetChildIndex(sec, 0); // newest on top
            Relayout();
            return sec;
        }

        public void Relayout()
        {
            SuspendLayout();
            int y = Padding.Top;
            foreach (var sec in Controls.OfType<Panel>().Reverse()) // maintain visual order (top first)
            {
                sec.Top = y;
                sec.Left = Padding.Left;
                sec.Width = ClientSize.Width - Padding.Horizontal - (VScroll ? SystemInformation.VerticalScrollBarWidth : 0);
                y += sec.Height + 6;
            }
            ResumeLayout();
            Invalidate();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Relayout();
        }
    }
}
