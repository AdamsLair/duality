using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Duality.Editor
{
	/// <summary>
	/// Represents a system-wide color picking operation that allows you to click anywhere
	/// and pick the color of the pixel at that position. A single <see cref="GlobalColorPickOperation"/>
	/// object can be used for any number of picking operations.
	/// </summary>
    public class GlobalColorPickOperation
    {
		private IntPtr hookPtr     = IntPtr.Zero;
		private Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
		private bool   active      = false;
		private bool   canceled    = false;
		private Color  pickedColor = Color.Transparent;

		private Form   cursorForm      = null;
		private Panel  cursorFormPanel = null;
		private Timer  cursorFormTimer = null;

		private NativeMethods.LowLevelMouseProc mouseHook = null;
		private InteractionFilter interactionFilter = null;
		private Point pickedCursorPos = Point.Empty;
		private Point globalCursorPos = Point.Empty;

		public event EventHandler PickedColorChanged = null;
		public event EventHandler OperationEnded = null;


		/// <summary>
		/// [GET] Whether the picking operation is currently in progress.
		/// </summary>
		public bool InProgress
		{
			get { return this.active; }
		}
		/// <summary>
		/// [GET] Whether the operation was canceled.
		/// </summary>
		public bool IsCanceled
		{
			get { return this.canceled; }
		}
		/// <summary>
		/// [GET] The color that was picked at the cursor position.
		/// </summary>
		public Color PickedColor
		{
			get { return this.pickedColor; }
		}


		public GlobalColorPickOperation()
		{
			// This is needed in order to stop the Garbage Collector from removing the hook
			this.mouseHook = this.MouseHookCallback;
		}

		/// <summary>
		/// Starts a global picking operation. This will disable some mouse interaction.
		/// </summary>
		public void Start()
		{
			if (this.active) throw new InvalidOperationException("Can't start picking operation when one is already in progress.");
			this.active = true;
			this.canceled = false;
			this.InstallGlobalHook();
			this.DisplayPickingWindow();
		}
		/// <summary>
		/// Cancels the currently active picking operation.
		/// </summary>
		public void Cancel()
		{
			this.End(true);
		}
		private void End(bool isCanceled)
		{
			if (!this.active) throw new InvalidOperationException("Can't end picking operation when none is in progress.");
			this.DisposePickingWindow();
			this.ReleaseGlobalHook();
			this.active = false;
			this.canceled = isCanceled;
			this.OnOperationEnded();
		}

		private void DisplayPickingWindow()
		{
			this.cursorForm = new Form();
			this.cursorForm.Text = "Picking Color...";
			this.cursorForm.StartPosition = FormStartPosition.Manual;
			this.cursorForm.FormBorderStyle = FormBorderStyle.None;
			this.cursorForm.MinimizeBox = false;
			this.cursorForm.MaximizeBox = false;
			this.cursorForm.MinimumSize = new Size(1, 1);
			this.cursorForm.ShowIcon = false;
			this.cursorForm.ShowInTaskbar = false;
			this.cursorForm.Size = new Size(30, 30);
			this.cursorForm.TopMost = true;

			this.cursorFormPanel = new Panel();
			this.cursorFormPanel.BorderStyle = BorderStyle.FixedSingle;
			this.cursorFormPanel.Size = new Size(1, 1);
			this.cursorFormPanel.MinimumSize = Size.Empty;
			this.cursorFormPanel.Dock = DockStyle.Fill;
			this.cursorForm.Controls.Add(this.cursorFormPanel);

			this.cursorFormTimer = new Timer();
			this.cursorFormTimer.Interval = 16;
			this.cursorFormTimer.Tick += this.cursorFormTimer_Tick;

			this.cursorFormTimer.Start();
			this.cursorForm.Show();
		}
		private void DisposePickingWindow()
		{
			this.cursorFormTimer.Tick -= this.cursorFormTimer_Tick;
			this.cursorFormTimer.Dispose();
			this.cursorFormTimer = null;

			this.cursorForm.Dispose();
			this.cursorForm = null;
		}
		private void cursorFormTimer_Tick(object sender, EventArgs e)
		{
			// Pick color from global mouse coordinates
			if (this.pickedCursorPos != this.globalCursorPos)
			{
				Color color = this.GetColorAt(this.globalCursorPos.X, this.globalCursorPos.Y);
				if (this.pickedColor != color)
				{
					this.pickedCursorPos = this.globalCursorPos;
					this.pickedColor = color;
					this.OnPickedColorChanged();
				}
			}

			// Adjust the picking window color
			this.cursorFormPanel.BackColor = this.pickedColor;
		}

		private void InstallGlobalHook()
		{
			// We'll install a system global hook to detect mouse movement
			// and intercept clicks even outside Duality
			this.hookPtr = NativeMethods.SetWindowsMouseHookEx(this.mouseHook);

			// At the same time, we'll filter all interaction events within
			// Duality, so the UI doesn't highlight on hover, etc.
			this.interactionFilter = new InteractionFilter();
			Application.AddMessageFilter(this.interactionFilter);
		}
		private void ReleaseGlobalHook()
		{
			NativeMethods.UnhookWindowsHookEx(this.hookPtr);
			Application.RemoveMessageFilter(this.interactionFilter);
			this.interactionFilter = null;
		}
		private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			bool mouseDownUp = false;

			if (nCode >= 0)
			{
				NativeMethods.LowLevelHookStruct hookStruct = (NativeMethods.LowLevelHookStruct)
					Marshal.PtrToStructure(lParam, typeof(NativeMethods.LowLevelHookStruct));

				if ((NativeMethods.MouseMessages)wParam == NativeMethods.MouseMessages.WM_MOUSEMOVE)
				{
					this.globalCursorPos = new Point(hookStruct.pt.x, hookStruct.pt.y);

					// Adjust the picking window position
					Point targetPos = new Point(
						this.globalCursorPos.X + 10,
						this.globalCursorPos.Y + 10);
					this.cursorForm.DesktopLocation = targetPos;
				}
				
				if ((NativeMethods.MouseMessages)wParam == NativeMethods.MouseMessages.WM_LBUTTONDOWN ||
					(NativeMethods.MouseMessages)wParam == NativeMethods.MouseMessages.WM_RBUTTONDOWN)
				{
					mouseDownUp = true;
				}

				if ((NativeMethods.MouseMessages)wParam == NativeMethods.MouseMessages.WM_LBUTTONUP ||
					(NativeMethods.MouseMessages)wParam == NativeMethods.MouseMessages.WM_RBUTTONUP)
				{
					mouseDownUp = true;
					bool isCanceled = ((NativeMethods.MouseMessages)wParam == NativeMethods.MouseMessages.WM_RBUTTONUP);
					this.End(isCanceled);
				}
			}

			return mouseDownUp ? NativeMethods.SUPPRESS_OTHER_HOOKS : NativeMethods.CallNextHookEx(hookPtr, nCode, wParam, lParam);
		}

		private Color GetColorAt(int x, int y)
		{
			// See here: http://stackoverflow.com/questions/1483928/how-to-read-the-color-of-a-screen-pixel
			using (Graphics gdest = Graphics.FromImage(this.screenPixel))
			{
				using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
				{
					IntPtr hSrcDC = gsrc.GetHdc();
					IntPtr hDC = gdest.GetHdc();
					int retval = NativeMethods.BitBlt(hDC, 0, 0, 1, 1, hSrcDC, x, y, (int)CopyPixelOperation.SourceCopy);
					gdest.ReleaseHdc();
					gsrc.ReleaseHdc();
				}
			}
			return this.screenPixel.GetPixel(0, 0);
		}

		private void OnPickedColorChanged()
		{
			if (this.PickedColorChanged != null)
				this.PickedColorChanged(this, EventArgs.Empty);
		}
		private void OnOperationEnded()
		{
			if (this.OperationEnded != null)
				this.OperationEnded(this, EventArgs.Empty);
		}

		private class InteractionFilter : IMessageFilter
		{
			public bool PreFilterMessage(ref Message m)
			{
				if (m.Msg == (int)WindowsMessages.WM_MOUSEMOVE ||
					m.Msg == (int)WindowsMessages.WM_MOUSELEAVE ||
					m.Msg == (int)WindowsMessages.WM_MOUSEWHEEL)
				{
					return true;
				}
				return false;
			}
		}
	}
}
