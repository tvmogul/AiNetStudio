// File: AccordionControls.cs
// WinForms .NET 9 — Outlook-style Accordion with robust design-time support.
// - Sections appear immediately in designer (adds 2 defaults on drop)
// - You can drop controls onto each section's ContentPanel at design time
// - Proper serialization of child controls via DesignerSerializationVisibility.Content
// - IToolboxUser routes toolbox drops to ContentPanel
// - Designer verbs: Add Section / Add 2 Demo Sections

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design; // IToolboxUser
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AiNetStudio.WinGui.Controls
{
    [Designer(typeof(AccordionPanelDesigner))]
    [DesignTimeVisible(true)]
    [ToolboxItem(true)]
    public partial class AccordionPanel : UserControl
    {
        private readonly AccordionSectionCollection _sections;

        public AccordionPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw, true);
            AutoScroll = true;
            BackColor = Color.White;
            Padding = new Padding(0);
            Size = new Size(300, 300);

            _sections = new AccordionSectionCollection(this);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Behavior")]
        [Editor(typeof(AccordionSectionsEditor), typeof(UITypeEditor))]
        public AccordionSectionCollection Sections => _sections;

        [Category("Appearance")]
        [DefaultValue(6)]
        public int SectionSpacing
        {
            get => _sectionSpacing;
            set { _sectionSpacing = Math.Max(0, value); PerformLayout(); Invalidate(); }
        }
        private int _sectionSpacing = 6;

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is AccordionSection sec && !_sections.Contains(sec))
            {
                _sections.InternalAddFromControls(sec);
                HookSection(sec);

                // Ensure visibility immediately (esp. in designer) before Layout runs.
                if (DesignModeActive)
                {
                    int y = 0;
                    foreach (Control c in Controls)
                        if (c is AccordionSection s && !ReferenceEquals(s, sec))
                            y = Math.Max(y, s.Bottom + SectionSpacing);

                    int initialHeight = sec.HeaderHeight + Math.Max(20, sec.DesignTimeContentHeightWhenEmpty);
                    sec.Bounds = new Rectangle(0, y, Math.Max(100, ClientSize.Width), initialHeight);
                }

                PerformLayout();
                Invalidate();
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (e.Control is AccordionSection sec && _sections.Contains(sec))
            {
                UnhookSection(sec);
                _sections.InternalRemoveFromControls(sec);
                PerformLayout();
                Invalidate();
            }
        }

        private void HookSection(AccordionSection sec)
        {
            sec.ExpandCollapseRequested -= Section_ExpandCollapseRequested;
            sec.ExpandCollapseRequested += Section_ExpandCollapseRequested;
            sec.HeaderHeightChanged -= Section_HeaderHeightChanged;
            sec.HeaderHeightChanged += Section_HeaderHeightChanged;
            sec.Margin = new Padding(0);
            sec.Dock = DockStyle.None; // Manual stacking
            sec.IsExpanded = true;
        }

        private void UnhookSection(AccordionSection sec)
        {
            sec.ExpandCollapseRequested -= Section_ExpandCollapseRequested;
            sec.HeaderHeightChanged -= Section_HeaderHeightChanged;
        }

        private void Section_HeaderHeightChanged(object? sender, EventArgs e)
        {
            PerformLayout();
            Invalidate();
        }

        private void Section_ExpandCollapseRequested(object? sender, EventArgs e)
        {
            // Enable this for "single open at a time":
            // foreach (var s in _sections) if (!ReferenceEquals(s, sender)) s.IsExpanded = false;
            PerformLayout();
            Invalidate();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            int y = AutoScrollPosition.Y;
            int w = ClientSize.Width;
            if (VerticalScroll.Visible) w -= SystemInformation.VerticalScrollBarWidth;

            foreach (var sec in _sections.ToArray())
            {
                if (sec.IsDisposed) continue;

                int contentHeight = sec.IsExpanded ? sec.GetCurrentContentHeightForLayout() : 0;
                int h = sec.HeaderHeight + contentHeight;

                if (DesignModeActive && h < sec.HeaderHeight + sec.DesignTimeContentHeightWhenEmpty)
                    h = sec.HeaderHeight + sec.DesignTimeContentHeightWhenEmpty;

                sec.Bounds = new Rectangle(0, y, Math.Max(0, w), h);
                y += h + SectionSpacing;
            }

            AutoScrollMinSize = new Size(0, Math.Max(0, y - AutoScrollPosition.Y));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            PerformLayout();
        }

        internal static bool DesignModeActiveFor(Control c)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            return c.Site?.DesignMode ?? false;
        }
        private bool DesignModeActive => DesignModeActiveFor(this);
    }

    [Designer(typeof(AccordionSectionDesigner))]
    [DesignTimeVisible(true)]
    [ToolboxItem(true)]
    public partial class AccordionSection : UserControl
    {
        private readonly Panel _header;
        private readonly Label _titleLabel;
        private readonly Button _toggleButton;
        private readonly Panel _content;

        public event EventHandler? ExpandCollapseRequested;
        public event EventHandler? HeaderHeightChanged;

        public AccordionSection()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw, true);

            BackColor = Color.White;
            Margin = new Padding(0);
            Padding = new Padding(0);
            Size = new Size(250, 120);

            _header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.FromArgb(245, 245, 245),
                Padding = new Padding(8, 6, 8, 6)
            };

            _toggleButton = new Button
            {
                Text = "▸",
                AutoSize = false,
                Width = 24,
                Height = 24,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Left,
                Margin = new Padding(0),
                TabStop = false
            };
            _toggleButton.FlatAppearance.BorderSize = 0;
            _toggleButton.Click += (_, __) => Toggle();

            _titleLabel = new Label
            {
                Text = "Section",
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold)
            };
            _titleLabel.Click += (_, __) => Toggle();

            _header.Controls.Add(_titleLabel);
            _header.Controls.Add(_toggleButton);

            _content = new Panel
            {
                Name = "ContentPanel",               // shows in Document Outline
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0),
                Padding = new Padding(6)
            };

            Controls.Add(_content);
            Controls.Add(_header);

            UpdateToggleGlyph();
        }

        [Category("Appearance")]
        public override string Text
        {
            get => _titleLabel.Text;
            set { _titleLabel.Text = value; Invalidate(); }
        }

        [Category("Behavior")]
        [DefaultValue(true)]
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                UpdateToggleGlyph();
                PerformLayout();
                Invalidate();
                ExpandCollapseRequested?.Invoke(this, EventArgs.Empty);
            }
        }
        private bool _isExpanded = true;

        [Category("Layout")]
        [DefaultValue(32)]
        public int HeaderHeight
        {
            get => _header.Height;
            set
            {
                int v = Math.Max(24, value);
                if (_header.Height == v) return;
                _header.Height = v;
                PerformLayout();
                Invalidate();
                HeaderHeightChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // Expose ContentPanel as a designable/content-serializable surface.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        [Category("Design")]
        public Panel ContentPanel => _content;

        // Keeps empty sections visibly tall in the designer.
        [Category("Design")]
        [DefaultValue(80)]
        public int DesignTimeContentHeightWhenEmpty
        {
            get => _designTimeHeight;
            set { _designTimeHeight = Math.Max(20, value); Invalidate(); }
        }
        private int _designTimeHeight = 80;

        internal int GetCurrentContentHeightForLayout()
        {
            if (!IsExpanded) return 0;

            int h = _content.Height;
            if (h <= 4) h = _content.PreferredSize.Height;
            if (h <= 4 && AccordionPanel.DesignModeActiveFor(this))
                h = Math.Max(h, DesignTimeContentHeightWhenEmpty);

            return Math.Max(h, 1);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            if (!IsExpanded)
            {
                if (_content.Dock != DockStyle.None)
                {
                    _content.Dock = DockStyle.None;
                    _content.Visible = false;
                }
                _content.Bounds = new Rectangle(0, _header.Bottom, Width, 0);
            }
            else
            {
                _content.Visible = true;
                _content.Dock = DockStyle.Fill;
            }
        }

        private void Toggle() => IsExpanded = !IsExpanded;

        private void UpdateToggleGlyph()
        {
            _toggleButton.Text = IsExpanded ? "▾" : "▸";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            using var pen = new Pen(Color.Gainsboro);
            g.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));

            using var pen2 = new Pen(Color.Silver);
            g.DrawLine(pen2, 0, _header.Bottom, Width, _header.Bottom);
        }
    }

    // ===========================
    // DESIGNERS
    // ===========================

    // AccordionPanel designer: adds default sections, verbs, and enables ContentPanel design surfaces.
    public class AccordionPanelDesigner : ParentControlDesigner
    {
        private DesignerVerbCollection? _verbs;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (Control is AccordionPanel panel)
            {
                foreach (Control c in panel.Controls)
                    if (c is AccordionSection sec)
                        EnableContentDesign(sec);
            }
        }

        // Create 2 sections automatically when the control is first dropped on a form.
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            if (Control is not AccordionPanel panel) return;

            var host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            using DesignerTransaction? tx = host?.CreateTransaction("Initialize Accordion with sections");

            // Ensure the panel has a reasonable size on drop
            if (panel.Size.Width < 240 || panel.Size.Height < 120)
                panel.Size = new Size(Math.Max(240, panel.Size.Width), Math.Max(160, panel.Size.Height));

            AddSectionDesignTime(panel, "Section 1");
            AddSectionDesignTime(panel, "Section 2");

            RaiseChanged(panel, nameof(panel.Controls));
            tx?.Commit();
        }

        public override bool CanParent(Control control) => control is AccordionSection;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                _verbs ??= new DesignerVerbCollection
                {
                    new DesignerVerb("Add Section", OnAddSection),
                    new DesignerVerb("Add 2 Demo Sections", OnAddTwo)
                };
                return _verbs;
            }
        }

        private void OnAddSection(object? sender, EventArgs e)
        {
            if (Control is AccordionPanel panel)
            {
                var host = GetService(typeof(IDesignerHost)) as IDesignerHost;
                using DesignerTransaction? tx = host?.CreateTransaction("Add Section");
                var sec = AddSectionDesignTime(panel, $"Section {panel.Controls.OfType<AccordionSection>().Count() + 1}");

                var sel = GetService(typeof(ISelectionService)) as ISelectionService;
                sel?.SetSelectedComponents(new[] { sec }, SelectionTypes.Primary);

                RaiseChanged(panel, nameof(panel.Controls));
                tx?.Commit();
            }
        }

        private void OnAddTwo(object? sender, EventArgs e)
        {
            if (Control is AccordionPanel panel)
            {
                var host = GetService(typeof(IDesignerHost)) as IDesignerHost;
                using DesignerTransaction? tx = host?.CreateTransaction("Add 2 Sections");
                AddSectionDesignTime(panel, $"Section {panel.Controls.OfType<AccordionSection>().Count() + 1}");
                AddSectionDesignTime(panel, $"Section {panel.Controls.OfType<AccordionSection>().Count() + 1}");
                RaiseChanged(panel, nameof(panel.Controls));
                tx?.Commit();
            }
        }

        private AccordionSection AddSectionDesignTime(AccordionPanel panel, string title)
        {
            var host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            var sec = (host?.CreateComponent(typeof(AccordionSection)) as AccordionSection) ?? new AccordionSection();
            sec.Text = title;
            sec.IsExpanded = true;

            // Place visibly now (before layout) so it shows in designer immediately.
            int y = 0;
            foreach (Control c in panel.Controls)
                if (c is AccordionSection s)
                    y = Math.Max(y, s.Bottom + panel.SectionSpacing);

            int initialHeight = sec.HeaderHeight + Math.Max(20, sec.DesignTimeContentHeightWhenEmpty);
            sec.Bounds = new Rectangle(0, y, Math.Max(160, panel.ClientSize.Width), initialHeight);

            // Add through Sections to keep list and Controls in sync
            panel.Sections.Add(sec);

            EnableContentDesign(sec);
            panel.PerformLayout();
            panel.Invalidate();
            return sec;
        }

        internal void EnableContentDesign(AccordionSection section)
        {
            // Expose inner ContentPanel as a design surface so you can drop controls there
            var host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (host is null) return;
            string baseName = section.Site?.Name ?? section.Name ?? "AccordionSection";
            string uniqueChildName = baseName + "_ContentPanel";
            EnableDesignMode(section.ContentPanel, uniqueChildName);
        }

        private void RaiseChanged(object component, string propName)
        {
            var cs = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            var pd = TypeDescriptor.GetProperties(component)[propName];
            cs?.OnComponentChanging(component, pd);
            cs?.OnComponentChanged(component, pd, null, null);
        }
    }

    // AccordionSection designer: routes toolbox drops into ContentPanel and enables its design surface.
    public class AccordionSectionDesigner : ParentControlDesigner, IToolboxUser
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (component is AccordionSection section)
            {
                string baseName = section.Site?.Name ?? section.Name ?? "AccordionSection";
                EnableDesignMode(section.ContentPanel, baseName + "_ContentPanel");
            }
        }

        // Prevent dropping arbitrary controls directly onto the section root (header area),
        // but allow via toolbox routing into ContentPanel.
        public override bool CanParent(Control control) => false; // Root is not a parent surface; ContentPanel is.

        bool IToolboxUser.GetToolSupported(ToolboxItem tool) => true; // allow all; we'll route to ContentPanel

        void IToolboxUser.ToolPicked(ToolboxItem tool)
        {
            if (Component is not AccordionSection section) return;

            var host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (host == null) return;

            using DesignerTransaction? tx = host.CreateTransaction("Add control to AccordionSection.ContentPanel");
            try
            {
                var comps = tool.CreateComponents(host);
                foreach (var comp in comps)
                {
                    if (comp is Control c)
                    {
                        section.ContentPanel.Controls.Add(c);
                        c.Location = new Point(8, 8);

                        var cs = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                        var pd = TypeDescriptor.GetProperties(section)["ContentPanel"];
                        cs?.OnComponentChanging(section, pd);
                        cs?.OnComponentChanged(section, pd, null, null);
                    }
                }
            }
            finally
            {
                tx?.Commit();
            }
        }
    }

    // ===========================================
    // Strongly-typed collection wired to owner; keeps designer in sync
    // ===========================================
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class AccordionSectionCollection : Collection<AccordionSection>
    {
        private readonly AccordionPanel _owner;
        internal AccordionPanel Owner => _owner;

        public AccordionSectionCollection(AccordionPanel owner) => _owner = owner;

        protected override void InsertItem(int index, AccordionSection item)
        {
            if (item is null) return;

            base.InsertItem(index, item);

            if (!_owner.Controls.Contains(item))
                _owner.Controls.Add(item);

            _owner.Controls.SetChildIndex(item, index);

            if (_owner is not null)
            {
                item.IsExpanded = true;
                int minH = item.HeaderHeight + Math.Max(20, item.DesignTimeContentHeightWhenEmpty);
                if (item.Height < minH) item.Height = minH;

                _owner.PerformLayout();
                _owner.Invalidate();
            }
        }

        protected override void SetItem(int index, AccordionSection item)
        {
            var old = this[index];
            if (old != null && _owner.Controls.Contains(old))
                _owner.Controls.Remove(old);

            base.SetItem(index, item);

            if (!_owner.Controls.Contains(item))
                _owner.Controls.Add(item);

            _owner.Controls.SetChildIndex(item, index);
            _owner.PerformLayout();
            _owner.Invalidate();
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);

            if (item != null && _owner.Controls.Contains(item))
            {
                _owner.Controls.Remove(item);
                item.Dispose();
            }

            _owner.PerformLayout();
            _owner.Invalidate();
        }

        protected override void ClearItems()
        {
            foreach (var s in this.ToArray())
            {
                if (_owner.Controls.Contains(s))
                {
                    _owner.Controls.Remove(s);
                    s.Dispose();
                }
            }

            base.ClearItems();
            _owner.PerformLayout();
            _owner.Invalidate();
        }

        // Keep collection in sync when someone adds/removes directly to Controls.
        internal void InternalAddFromControls(AccordionSection item)
        {
            if (!this.Contains(item))
                base.InsertItem(Count, item);
        }

        internal void InternalRemoveFromControls(AccordionSection item)
        {
            int idx = this.IndexOf(item);
            if (idx >= 0)
                base.RemoveItem(idx);
        }
    }

    // ===========================================
    // Custom UITypeEditor for Sections — avoids CollectionEditor NRE by managing components via DesignerHost
    // ===========================================
    internal sealed class AccordionSectionsEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context?.Instance is not AccordionPanel panel || provider == null)
                return value;

            var host = (IDesignerHost?)provider.GetService(typeof(IDesignerHost));
            var changeSvc = (IComponentChangeService?)provider.GetService(typeof(IComponentChangeService));
            var edSvc = (IWindowsFormsEditorService?)provider.GetService(typeof(IWindowsFormsEditorService));

            if (host == null || edSvc == null)
                return value;

            using var tx = host.CreateTransaction("Edit Accordion Sections");
            using var dlg = new SectionsEditorForm(panel, host, changeSvc);
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                changeSvc?.OnComponentChanged(panel, TypeDescriptor.GetProperties(panel)["Sections"], null, null);
                tx.Commit();
            }
            else
            {
                tx.Cancel();
            }

            return panel.Sections;
        }

        // Minimal collection editor dialog specialized for AccordionSection
        private sealed class SectionsEditorForm : Form
        {
            private readonly AccordionPanel _panel;
            private readonly IDesignerHost _host;
            private readonly IComponentChangeService? _changeSvc;

            private readonly ListBox _list = new() { Dock = DockStyle.Fill };
            private readonly Button _add = new() { Text = "Add", Width = 80, DialogResult = DialogResult.None };
            private readonly Button _remove = new() { Text = "Remove", Width = 80, DialogResult = DialogResult.None };
            private readonly Button _up = new() { Text = "Up", Width = 80, DialogResult = DialogResult.None };
            private readonly Button _down = new() { Text = "Down", Width = 80, DialogResult = DialogResult.None };
            private readonly Button _ok = new() { Text = "OK", Width = 90, DialogResult = DialogResult.OK };
            private readonly Button _cancel = new() { Text = "Cancel", Width = 90, DialogResult = DialogResult.Cancel };

            public SectionsEditorForm(AccordionPanel panel, IDesignerHost host, IComponentChangeService? changeSvc)
            {
                _panel = panel;
                _host = host;
                _changeSvc = changeSvc;

                Text = "AccordionSection Collection Editor";
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
                StartPosition = FormStartPosition.CenterParent;
                MinimizeBox = false;
                MaximizeBox = false;
                ShowInTaskbar = false;
                ClientSize = new Size(520, 360);

                var right = new FlowLayoutPanel
                {
                    Dock = DockStyle.Right,
                    FlowDirection = FlowDirection.TopDown,
                    Width = 100,
                    Padding = new Padding(6)
                };
                right.Controls.AddRange(new Control[] { _add, _remove, _up, _down, new Label { Height = 12 }, _ok, _cancel });

                Controls.Add(_list);
                Controls.Add(right);

                _list.SelectionMode = SelectionMode.One;
                _list.DisplayMember = "Text";
                RefreshList();

                _add.Click += (_, __) => DoAdd();
                _remove.Click += (_, __) => DoRemove();
                _up.Click += (_, __) => DoMove(-1);
                _down.Click += (_, __) => DoMove(+1);
            }

            private void RefreshList()
            {
                _list.Items.Clear();
                foreach (var s in _panel.Sections)
                    _list.Items.Add(s);
            }

            private void DoAdd()
            {
                var comp = _host.CreateComponent(typeof(AccordionSection)) as AccordionSection;
                if (comp == null) return;

                comp.Text = $"Section {_panel.Sections.Count + 1}";
                comp.IsExpanded = true;
                _panel.Sections.Add(comp);
                EnableContentDesignFor(comp);

                _changeSvc?.OnComponentChanging(_panel, TypeDescriptor.GetProperties(_panel)["Sections"]);
                _changeSvc?.OnComponentChanged(_panel, TypeDescriptor.GetProperties(_panel)["Sections"], null, null);

                RefreshList();
                _list.SelectedItem = comp;
            }

            private void DoRemove()
            {
                if (_list.SelectedItem is not AccordionSection sec) return;

                _panel.Sections.Remove(sec);
                _host.DestroyComponent(sec);

                _changeSvc?.OnComponentChanging(_panel, TypeDescriptor.GetProperties(_panel)["Sections"]);
                _changeSvc?.OnComponentChanged(_panel, TypeDescriptor.GetProperties(_panel)["Sections"], null, null);

                RefreshList();
            }

            private void DoMove(int delta)
            {
                if (_list.SelectedItem is not AccordionSection sec) return;
                var index = _panel.Sections.IndexOf(sec);
                var newIndex = Math.Max(0, Math.Min(_panel.Sections.Count - 1, index + delta));
                if (newIndex == index) return;

                _panel.Sections.RemoveAt(index);
                _panel.Sections.Insert(newIndex, sec);

                _changeSvc?.OnComponentChanging(_panel, TypeDescriptor.GetProperties(_panel)["Sections"]);
                _changeSvc?.OnComponentChanged(_panel, TypeDescriptor.GetProperties(_panel)["Sections"], null, null);

                RefreshList();
                _list.SelectedItem = sec;
            }

            private void EnableContentDesignFor(AccordionSection section)
            {
                // Mirror what the designer does so newly added sections are editable immediately
                if (_host.GetDesigner(_panel) is AccordionPanelDesigner designer)
                {
                    designer.EnableContentDesign(section);
                }
            }
        }
    }
}
