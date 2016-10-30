using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Duality.Editor
{
    public class GlobalColorPickOperation
    {
		private IntPtr hookPtr     = IntPtr.Zero;
		private Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
		private bool   active      = false;
		private bool   canceled    = false;
		private Color  pickedColor = Color.Transparent;

		private NativeMethods.LowLevelMouseProc mouseHook   = null;

		public event EventHandler PickedColorChanged = null;
		public event EventHandler OperationEnded = null;


		public bool InProgress
		{
			get { return this.active; }
		}
		public bool IsCanceled
		{
			get { return this.canceled; }
		}
		public Color PickedColor
		{
			get { return this.pickedColor; }
		}


		public GlobalColorPickOperation()
		{
			// This is needed in order to stop the Garbage Collector from removing the hook
			this.mouseHook = this.MouseHookCallback;
		}

		public void Start()
		{
			if (this.active) throw new InvalidOperationException("Can't start picking operation when one is already in progress.");
			this.active = true;
			this.canceled = false;
			this.InstallGlobalHook();
		}
		public void End()
		{
			this.End(true);
		}
		private void End(bool isCanceled)
		{
			if (!this.active) throw new InvalidOperationException("Can't end picking operation when none is in progress.");
			this.ReleaseGlobalHook();
			this.active = false;
			this.canceled = isCanceled;
			this.OnOperationEnded();
		}

		private void InstallGlobalHook()
		{
			this.hookPtr = NativeMethods.SetWindowsMouseHookEx(this.mouseHook);
		}
		private void ReleaseGlobalHook()
		{
			NativeMethods.UnhookWindowsHookEx(this.hookPtr);
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
					Color color = this.GetColorAt(hookStruct.pt.x, hookStruct.pt.y);
					if (this.pickedColor != color)
					{
						this.pickedColor = color;
						this.OnPickedColorChanged();
					}
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
	}
}
