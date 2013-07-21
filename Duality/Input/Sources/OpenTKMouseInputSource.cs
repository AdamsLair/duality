using System;
using OpenTK.Input;

namespace Duality
{
	public class OpenTKMouseInputSource : IMouseInputSource
	{
		public delegate void CursorPosSetter(int v);

		private	MouseDevice	device;
		private CursorPosSetter cursorPosSetterX;
		private CursorPosSetter cursorPosSetterY;
		private bool cursorInView;
		
		public int X
		{
			get { return this.device.X; }
			set { if (this.cursorPosSetterX != null) this.cursorPosSetterX(value); }
		}
		public int Y
		{
			get { return this.device.Y; }
			set { if (this.cursorPosSetterY != null) this.cursorPosSetterY(value); }
		}
		public float Wheel
		{
			get { return this.device.WheelPrecise; }
		}
		public bool CursorInView
		{
			get { return this.cursorInView; }
		}
		public bool this[MouseButton key]
		{
			get { return this.device[key]; }
		}

		public OpenTKMouseInputSource(MouseDevice device, CursorPosSetter cursorPosSetterX, CursorPosSetter cursorPosSetterY)
		{
			this.device = device;
			this.cursorPosSetterX = cursorPosSetterX;
			this.cursorPosSetterY = cursorPosSetterY;

			this.device.Enter += this.device_Enter;
			this.device.Leave += this.device_Leave;
		}

		private void device_Enter(object sender, EventArgs e)
		{
			this.cursorInView = true;
		}
		private void device_Leave(object sender, EventArgs e)
		{
			this.cursorInView = false;
		}
	}
}
