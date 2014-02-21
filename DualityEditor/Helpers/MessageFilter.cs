using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Duality.Editor
{
	enum WindowsMessages
	{
		WM_LBUTTONDOWN	= 0x0201,
		WM_LBUTTONUP	= 0x0202,
		WM_MOUSEMOVE	= 0x0200,
		WM_MOUSEWHEEL	= 0x020A,
		WM_RBUTTONDOWN	= 0x0204,
		WM_RBUTTONUP	= 0x0205,
		WM_MOUSELEAVE	= 0x02A3,

		WM_KEYDOWN		= 0x0100
	}

	public class InputEventMessageFilter : IMessageFilter
	{
		public event EventHandler MouseMove;
		public event EventHandler MouseLeave;
		public event EventHandler MouseUp;
		public event EventHandler MouseWheel;
		public event EventHandler<KeyEventArgs> KeyDown;

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == (int)WindowsMessages.WM_MOUSEMOVE)
			{
				if (this.MouseMove != null) this.MouseMove(this, EventArgs.Empty);
			}
			else if (m.Msg == (int)WindowsMessages.WM_MOUSELEAVE)
			{
				if (this.MouseLeave != null) this.MouseLeave(this, EventArgs.Empty);
			}
			else if (m.Msg == (int)WindowsMessages.WM_LBUTTONUP)
			{
				if (this.MouseUp != null) this.MouseUp(this, EventArgs.Empty);
			}
			else if (m.Msg == (int)WindowsMessages.WM_RBUTTONUP)
			{
				if (this.MouseUp != null) this.MouseUp(this, EventArgs.Empty);
			}
			else if (m.Msg == (int)WindowsMessages.WM_MOUSEWHEEL)
			{
				if (this.MouseWheel != null) this.MouseWheel(this, EventArgs.Empty);
			}
			else if (m.Msg == (int)WindowsMessages.WM_KEYDOWN)
			{
				if (this.KeyDown != null)
				{
					KeyEventArgs args = new KeyEventArgs((Keys)m.WParam.ToInt32());
					this.KeyDown(this, args);
					return args.Handled;
				}
			}
			return false;
		}
	}
	public class InputEventMessageRedirector : IMessageFilter
	{
		public delegate bool MessageFilter(MessageType type, EventArgs args);
		public enum MessageType
		{
			MouseWheel = WindowsMessages.WM_MOUSEWHEEL,
			KeyDown = WindowsMessages.WM_KEYDOWN
		}

		private	MessageFilter		filter		= null;
		private	Control				redirectTo	= null;
		private	List<MessageType>	redirectMsg = new List<MessageType>();

		public InputEventMessageRedirector(Control target, MessageFilter filter, params MessageType[] types)
		{
			this.redirectTo = target;
			this.filter = filter;
			this.redirectMsg = types.ToList();
		}

		public bool PreFilterMessage(ref Message m)
		{
			MessageType type = (MessageType)m.Msg;
			if (this.redirectMsg.Contains(type))
			{
				EventArgs args = EventArgs.Empty;

				if (m.Msg == (int)WindowsMessages.WM_KEYDOWN)
				{
					args = new KeyEventArgs((Keys)m.WParam.ToInt32());
				}

				if (this.filter == null || this.filter(type, args))
				{
					if (this.redirectTo != null)
						SendMessage(this.redirectTo.Handle, m.Msg, m.WParam, m.LParam);
					return true;
				}
			}

			return false;
		}

		[DllImport("user32.dll", SetLastError = false)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
	}
}
