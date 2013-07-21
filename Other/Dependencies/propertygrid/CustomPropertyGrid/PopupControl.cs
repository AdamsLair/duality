using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

// This file is more or less taken from CodeProject. See:
// http://www.codeproject.com/Articles/25471/Customizable-ComboBox-Drop-Down
// (c) Lea Hayes

namespace AdamsLair.PropertyGrid
{
    public enum PopupResizeMode
    {
        None = 0,

        // Individual styles.
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8,

        // Combined styles.
        All = (Top | Left | Bottom | Right),
        TopLeft = (Top | Left),
        TopRight = (Top | Right),
        BottomLeft = (Bottom | Left),
        BottomRight = (Bottom | Right),
    }

    public enum GripAlignMode
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    public sealed class GripRenderer
    {
        private static Bitmap m_sGripBitmap;
        private static Bitmap GripBitmap
        {
            get { return m_sGripBitmap; }
        }

        private GripRenderer()
        {
        }

        public static void RefreshSystemColors(Graphics g, Size size)
        {
            InitializeGripBitmap(g, size, true);
        }
        public static void Render(Graphics g, Point location, Size size, GripAlignMode mode)
        {
            InitializeGripBitmap(g, size, false);

            // Calculate display size and position of grip.
            switch (mode)
            {
                case GripAlignMode.TopLeft:
                    size.Height = -size.Height;
                    size.Width = -size.Width;
                    break;

                case GripAlignMode.TopRight:
                    size.Height = -size.Height;
                    break;

                case GripAlignMode.BottomLeft:
                    size.Width = -size.Height;
                    break;
            }

            // Reverse size grip for left-aligned.
            if (size.Width < 0)
                location.X -= size.Width;
            if (size.Height < 0)
                location.Y -= size.Height;

            g.DrawImage(GripBitmap, location.X, location.Y, size.Width, size.Height);
        }
        public static void Render(Graphics g, Point location, GripAlignMode mode)
        {
            Render(g, location, new Size(16, 16), mode);
        }

        private static void InitializeGripBitmap(Graphics g, Size size, bool forceRefresh)
        {
            if (m_sGripBitmap == null || forceRefresh || size != m_sGripBitmap.Size)
            {
                // Draw size grip into a bitmap image.
                m_sGripBitmap = new Bitmap(size.Width, size.Height, g);
                using (Graphics gripG = Graphics.FromImage(m_sGripBitmap))
                    ControlPaint.DrawSizeGrip(gripG, SystemColors.ButtonFace, 0, 0, size.Width, size.Height);
            }
        }
    }

    public class PopupDropDown : ToolStripDropDown
    {
        private PopupResizeMode m_resizeMode = PopupResizeMode.None;
        private Rectangle m_gripBounds = Rectangle.Empty;

        private bool m_lockedHostedControlSize = false;
        private bool m_lockedThisSize = false;
        private bool m_refreshSize = false;


        /// <summary>
        /// Type of resize mode, grips are automatically drawn at bottom-left and bottom-right corners.
        /// </summary>
        public PopupResizeMode ResizeMode
        {
            get { return m_resizeMode; }
            set
            {
                if (value != m_resizeMode)
                {
                    m_resizeMode = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// Bounds of active grip box position.
        /// </summary>
        protected Rectangle GripBounds
        {
            get { return this.m_gripBounds; }
            set { this.m_gripBounds = value; }
        }
        /// <summary>
        /// Indicates when a grip box is shown.
        /// </summary>
        protected bool IsGripShown
        {
            get
            {
                return (ResizeMode == PopupResizeMode.TopLeft || ResizeMode == PopupResizeMode.TopRight ||
                        ResizeMode == PopupResizeMode.BottomLeft || ResizeMode == PopupResizeMode.BottomRight);
            }
        }


        public PopupDropDown(bool autoSize)
        {
            AutoSize = autoSize;
            Padding = Margin = Padding.Empty;
        }

        protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
                hostedControl.SizeChanged -= hostedControl_SizeChanged;
            base.OnClosing(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            GripBounds = Rectangle.Empty;

            if (CompareResizeMode(PopupResizeMode.BottomLeft))
            {
                // Draw grip area at bottom-left of popup.
                e.Graphics.FillRectangle(SystemBrushes.ButtonFace, 1, Height - 16, Width, 14);
                GripBounds = new Rectangle(1, Height - 16, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.BottomLeft);
            }
            else if (CompareResizeMode(PopupResizeMode.BottomRight))
            {
                // Draw grip area at bottom-right of popup.
                e.Graphics.FillRectangle(SystemBrushes.ButtonFace, 1, Height - 16, Width, 14);
                GripBounds = new Rectangle(Width - 17, Height - 16, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.BottomRight);
            }
            else if (CompareResizeMode(PopupResizeMode.TopLeft))
            {
                // Draw grip area at top-left of popup.
                e.Graphics.FillRectangle(SystemBrushes.ButtonFace, 1, 1, Width - 2, 14);
                GripBounds = new Rectangle(1, 0, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.TopLeft);
            }
            else if (CompareResizeMode(PopupResizeMode.TopRight))
            {
                // Draw grip area at top-right of popup.
                e.Graphics.FillRectangle(SystemBrushes.ButtonFace, 1, 1, Width - 2, 14);
                GripBounds = new Rectangle(Width - 17, 0, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.TopRight);
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // When drop-down window is being resized by the user (i.e. not locked),
            // update size of hosted control.
            if (!m_lockedThisSize)
                RecalculateHostedControlLayout();
        }
        protected void hostedControl_SizeChanged(object sender, EventArgs e)
        {
            // Only update size of this container when it is not locked.
            if (!m_lockedHostedControlSize)
                ResizeFromContent(-1);
        }
        
        public new void Show(Control parent, int x, int y)
        {
            Show(parent, x, y, -1, -1);
        }
        public void Show(Control parent, int x, int y, int width, int height)
        {
            // If no hosted control is associated, this procedure is pointless!
            Control hostedControl = GetHostedControl();
            if (hostedControl == null)
                return;

            // Initially hosted control should be displayed within a drop down of 1x1, however
            // its size should exceed the dimensions of the drop-down.
            {
                m_lockedHostedControlSize = true;
                m_lockedThisSize = true;

                // Display actual popup and occupy just 1x1 pixel to avoid automatic reposition.
                Size = new Size(1, 1);
                base.Show(parent, x, y);

                m_lockedHostedControlSize = false;
                m_lockedThisSize = false;
            }

            // Resize drop-down to fit its contents.
            ResizeFromContent(width);

            // If client area was enlarged using the minimum width paramater, then the hosted
            // control must also be enlarged.
            if (m_refreshSize)
                RecalculateHostedControlLayout();

            // If popup is overlapping the initial position then move above!
            if (y > Top && y <= Bottom)
            {
                Top = y - Height - (height != -1 ? height : 0);

                PopupResizeMode previous = ResizeMode;
                if (ResizeMode == PopupResizeMode.BottomLeft)
                    ResizeMode = PopupResizeMode.TopLeft;
                else if (ResizeMode == PopupResizeMode.BottomRight)
                    ResizeMode = PopupResizeMode.TopRight;

                if (ResizeMode != previous)
                    RecalculateHostedControlLayout();
            }

            // Assign event handler to control.
            hostedControl.SizeChanged += hostedControl_SizeChanged;
        }

        protected void ResizeFromContent(int width)
        {
            if (m_lockedThisSize)
                return;

            // Prevent resizing hosted control to 1x1 pixel!
            m_lockedHostedControlSize = true;

            // Resize from content again because certain information was not available before.
            Rectangle bounds = Bounds;
            bounds.Size = SizeFromContent(width);

            if (!CompareResizeMode(PopupResizeMode.None))
            {
                if (width > 0 && bounds.Width > width)
                    if (!CompareResizeMode(PopupResizeMode.Right))
                        bounds.X -= bounds.Width - width;
            }

            Bounds = bounds;

            m_lockedHostedControlSize = false;
        }
        protected void RecalculateHostedControlLayout()
        {
            if (m_lockedHostedControlSize)
                return;

            m_lockedThisSize = true;
            
            // Update size of hosted control.
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                // Fetch control bounds and adjust as necessary.
                Rectangle bounds = hostedControl.Bounds;
                if (CompareResizeMode(PopupResizeMode.TopLeft) || CompareResizeMode(PopupResizeMode.TopRight))
                    bounds.Location = new Point(0, 16);
                else
                    bounds.Location = new Point(0, 0);

                bounds.Width = ClientRectangle.Width;
                bounds.Height = ClientRectangle.Height;
                if (IsGripShown)
                    bounds.Height -= 16;

                if (bounds.Size != hostedControl.Size)
                    hostedControl.Size = bounds.Size;
                if (bounds.Location != hostedControl.Location)
                    hostedControl.Location = bounds.Location;
            }
            
            m_lockedThisSize = false;
        }

        public Control GetHostedControl()
        {
            if (Items.Count > 0)
            {
                ToolStripControlHost host = Items[0] as ToolStripControlHost;
                if (host != null)
                    return host.Control;
            }
            return null;
        }
        public bool CompareResizeMode(PopupResizeMode resizeMode)
        {
            return (ResizeMode & resizeMode) == resizeMode;
        }
        protected Size SizeFromContent(int width)
        {
            Size contentSize = Size.Empty;

            m_refreshSize = false;

            // Fetch hosted control.
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                if (CompareResizeMode(PopupResizeMode.TopLeft) || CompareResizeMode(PopupResizeMode.TopRight))
                    hostedControl.Location = new Point(0, 16);
                else
                    hostedControl.Location = new Point(0, 0);
                contentSize = SizeFromClientSize(hostedControl.Size);

                // Use minimum width (if specified).
                if (width > 0 && contentSize.Width < width)
                {
                    contentSize.Width = width;
                    m_refreshSize = true;
                }
            }

            // If a grip box is shown then add it into the drop down height.
            if (IsGripShown)
                contentSize.Height += 16;

            return contentSize;
        }

        #region Win32 stuff

        protected const int WM_GETMINMAXINFO = 0x0024;
        protected const int WM_NCHITTEST = 0x0084;

        protected const int HTTRANSPARENT = -1;
        protected const int HTLEFT = 10;
        protected const int HTRIGHT = 11;
        protected const int HTTOP = 12;
        protected const int HTTOPLEFT = 13;
        protected const int HTTOPRIGHT = 14;
        protected const int HTBOTTOM = 15;
        protected const int HTBOTTOMLEFT = 16;
        protected const int HTBOTTOMRIGHT = 17;

        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }

        protected static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }
        protected static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }
        protected static int LOWORD(int n)
        {
            return n & 0xffff;
        }
        protected static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        #endregion

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (!ProcessGrip(ref m, false))
                base.WndProc(ref m);
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ProcessGrip(ref Message m)
        {
            return ProcessGrip(ref m, true);
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool ProcessGrip(ref Message m, bool contentControl)
        {
            if (ResizeMode != PopupResizeMode.None)
            {
                switch (m.Msg)
                {
                    case WM_NCHITTEST:
                        return OnNcHitTest(ref m, contentControl);

                    case WM_GETMINMAXINFO:
                        return OnGetMinMaxInfo(ref m);
                }
            }
            return false;
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                MINMAXINFO minmax = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));

                // Maximum size.
                if (hostedControl.MaximumSize.Width != 0)
                    minmax.maxTrackSize.Width = hostedControl.MaximumSize.Width;
                if (hostedControl.MaximumSize.Height != 0)
                    minmax.maxTrackSize.Height = hostedControl.MaximumSize.Height;

                // Minimum size.
                minmax.minTrackSize = new Size(32, 32);
                if (hostedControl.MinimumSize.Width > minmax.minTrackSize.Width)
                    minmax.minTrackSize.Width = hostedControl.MinimumSize.Width;
                if (hostedControl.MinimumSize.Height > minmax.minTrackSize.Height)
                    minmax.minTrackSize.Height = hostedControl.MinimumSize.Height;

                Marshal.StructureToPtr(minmax, m.LParam, false);
            }
            return true;
        }
        private bool OnNcHitTest(ref Message m, bool contentControl)
        {
            Point location = PointToClient(new Point(LOWORD(m.LParam), HIWORD(m.LParam)));
            IntPtr transparent = new IntPtr(HTTRANSPARENT);

            // Check for simple gripper dragging.
            if (GripBounds.Contains(location))
            {
                if (CompareResizeMode(PopupResizeMode.BottomLeft))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOMLEFT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.BottomRight))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOMRIGHT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.TopLeft))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOPLEFT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.TopRight))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOPRIGHT;
                    return true;
                }
            }
            else   // Check for edge based dragging.
            {
                Rectangle rectClient = ClientRectangle;
                if (location.X > rectClient.Right - 3 && location.X <= rectClient.Right && CompareResizeMode(PopupResizeMode.Right))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTRIGHT;
                    return true;
                }
                else if (location.Y > rectClient.Bottom - 3 && location.Y <= rectClient.Bottom && CompareResizeMode(PopupResizeMode.Bottom))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOM;
                    return true;
                }
                else if (location.X > -1 && location.X < 3 && CompareResizeMode(PopupResizeMode.Left))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTLEFT;
                    return true;
                }
                else if (location.Y > -1 && location.Y < 3 && CompareResizeMode(PopupResizeMode.Top))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOP;
                    return true;
                }
            }
            return false;
        }
    }

    public interface IPopupControlHost
    {
		bool IsDropDownOpened { get; }

        void ShowDropDown();
        void HideDropDown();
    }

    public class PopupControl
    {
        private ToolStripControlHost m_host;
        private PopupDropDown m_dropDown;
        private Padding m_padding = Padding.Empty;
        private Padding m_margin = new Padding(1);
        private bool m_autoReset = false;
		
        public event ToolStripDropDownClosingEventHandler Closing
        {
            add
            {
                m_dropDown.Closing += value;
            }
            remove
            {
                m_dropDown.Closing -= value;
            }
        }
        public event ToolStripDropDownClosedEventHandler Closed
        {
            add
            {
                m_dropDown.Closed += value;
            }
            remove
            {
                m_dropDown.Closed -= value;
            }
        }

        public bool Visible
        {
            get { return (this.m_dropDown != null && this.m_dropDown.Visible) ? true : false; }
        }
        public Control Control
        {
            get { return (this.m_host != null) ? this.m_host.Control : null; }
        }
        public Padding Padding
        {
            get { return this.m_padding; }
            set { this.m_padding = value; }
        }
        public Padding Margin
        {
            get { return this.m_margin; }
            set { this.m_margin = value; }
        }
        public bool AutoResetWhenClosed
        {
            get { return this.m_autoReset; }
            set { this.m_autoReset = value; }
        }
        /// <summary>
        /// Gets or sets the popup control host, this is used to hide/show popup.
        /// </summary>
        public IPopupControlHost PopupControlHost { get; set; }


        public PopupControl()
        {
            InitializeDropDown();
        }

        private void m_dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (AutoResetWhenClosed)
                DisposeHost();
            
            // Hide drop down within popup control.
            if (PopupControlHost != null)
                PopupControlHost.HideDropDown();
        }

        public void Show(Control parent, Control control, int x, int y)
        {
            Show(parent, control, x, y, PopupResizeMode.None);
        }
        public void Show(Control parent, Control control, int x, int y, PopupResizeMode resizeMode)
        {
            Show(parent, control, x, y, -1, -1, resizeMode);
        }
        public void Show(Control parent, Control control, int x, int y, int width, int height, PopupResizeMode resizeMode)
        {
            Size controlSize = control.Size;

            InitializeHost(control);

            m_dropDown.ResizeMode = resizeMode;
            m_dropDown.Show(parent, x, y, width, height);

            control.Focus();
        }
        public void Hide()
        {
            if (m_dropDown != null && m_dropDown.Visible)
            {
                m_dropDown.Hide();
                DisposeHost();
            }
        }
        public void Reset()
        {
            DisposeHost();
        }

        protected void DisposeHost()
        {
            if (m_host != null)
            {
                // Make sure host is removed from drop down.
                if (m_dropDown != null)
                    m_dropDown.Items.Clear();

                // Dispose of host.
                m_host = null;
            }

            PopupControlHost = null;
        }
        protected void InitializeHost(Control control)
        {
            InitializeDropDown();

            // If control is not yet being hosted then initialize host.
            if (control != Control)
                DisposeHost();

            // Create a new host?
            if (m_host == null)
            {
                m_host = new ToolStripControlHost(control);
                m_host.AutoSize = false;
                m_host.Padding = Padding;
                m_host.Margin = Margin;
            }
            
            // Add control to drop-down.
            m_dropDown.Items.Clear();
            m_dropDown.Padding = m_dropDown.Margin = Padding.Empty;
            m_dropDown.Items.Add(m_host);
        }
        protected void InitializeDropDown()
        {
            // Does a drop down exist?
            if (m_dropDown == null)
            {
                m_dropDown = new PopupDropDown(false);
                m_dropDown.Closed += new ToolStripDropDownClosedEventHandler(m_dropDown_Closed);
            }
        }
    }
}
