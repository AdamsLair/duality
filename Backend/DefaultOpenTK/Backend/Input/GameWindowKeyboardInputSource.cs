using System;
using Duality.Input;
using System.Text;

using OpenTK;

namespace Duality.Backend.DefaultOpenTK
{
	public class GameWindowKeyboardInputSource : IKeyboardInputSource
	{
		private	GameWindow		window;
		private bool			hasFocus;
		private	int				repeatCounter;
		private	string			charInput;
		private	StringBuilder	charInputBuffer = new StringBuilder();
		
		public string Description
		{
			get { return "Keyboard"; }
		}
		public bool IsAvailable
		{
			get { return this.window != null && this.window.Keyboard != null && this.hasFocus; }
		}
		public bool KeyRepeat
		{
			get { return this.window.Keyboard.KeyRepeat; }
			set { this.window.Keyboard.KeyRepeat = value; }
		}
		public int KeyRepeatCounter
		{
			get { return this.repeatCounter; }
		}
		public string CharInput
		{
			get { return this.charInput ?? string.Empty; }
		}
		public bool this[Key key]
		{
			get { return this.window.Keyboard[GetOpenTKKey(key)]; }
		}

		public GameWindowKeyboardInputSource(GameWindow window)
		{
			this.window = window;
			this.window.Keyboard.GotFocus += this.device_GotFocus;
			this.window.Keyboard.LostFocus += this.device_LostFocus;
			this.window.Keyboard.KeyDown += this.device_KeyDown;
			this.window.KeyPress += this.window_KeyPress;
		}

		private void device_LostFocus(object sender, EventArgs e)
		{
			this.hasFocus = false;
		}
		private void device_GotFocus(object sender, EventArgs e)
		{
			this.hasFocus = true;
		}
		private void device_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			this.repeatCounter++;
		}
		private void window_KeyPress(object sender, OpenTK.KeyPressEventArgs e)
		{
			this.charInputBuffer.Append(e.KeyChar);
		}

		public void UpdateState()
		{
			this.charInput = this.charInputBuffer.ToString();
			this.charInputBuffer.Clear();
		}

		private static OpenTK.Input.Key GetOpenTKKey(Key key)
		{
			switch (key)
			{
				case Key.Unknown:		return OpenTK.Input.Key.Unknown;

				case Key.ShiftLeft:		return OpenTK.Input.Key.ShiftLeft;
				case Key.ShiftRight:	return OpenTK.Input.Key.ShiftRight;
				case Key.ControlLeft:	return OpenTK.Input.Key.ControlLeft;
				case Key.ControlRight:	return OpenTK.Input.Key.ControlRight;
				case Key.AltLeft:		return OpenTK.Input.Key.AltLeft;
				case Key.AltRight:		return OpenTK.Input.Key.AltRight;
				case Key.WinLeft:		return OpenTK.Input.Key.WinLeft;
				case Key.WinRight:		return OpenTK.Input.Key.WinRight;
				case Key.Menu:			return OpenTK.Input.Key.Menu;

				case Key.F1:			return OpenTK.Input.Key.F1;
				case Key.F2:			return OpenTK.Input.Key.F2;
				case Key.F3:			return OpenTK.Input.Key.F3;
				case Key.F4:			return OpenTK.Input.Key.F4;
				case Key.F5:			return OpenTK.Input.Key.F5;
				case Key.F6:			return OpenTK.Input.Key.F6;
				case Key.F7:			return OpenTK.Input.Key.F7;
				case Key.F8:			return OpenTK.Input.Key.F8;
				case Key.F9:			return OpenTK.Input.Key.F9;
				case Key.F10:			return OpenTK.Input.Key.F10;
				case Key.F11:			return OpenTK.Input.Key.F11;
				case Key.F12:			return OpenTK.Input.Key.F12;
				case Key.F13:			return OpenTK.Input.Key.F13;
				case Key.F14:			return OpenTK.Input.Key.F14;
				case Key.F15:			return OpenTK.Input.Key.F15;
				case Key.F16:			return OpenTK.Input.Key.F16;
				case Key.F17:			return OpenTK.Input.Key.F17;
				case Key.F18:			return OpenTK.Input.Key.F18;
				case Key.F19:			return OpenTK.Input.Key.F19;
				case Key.F20:			return OpenTK.Input.Key.F20;
				case Key.F21:			return OpenTK.Input.Key.F21;
				case Key.F22:			return OpenTK.Input.Key.F22;
				case Key.F23:			return OpenTK.Input.Key.F23;
				case Key.F24:			return OpenTK.Input.Key.F24;
				case Key.F25:			return OpenTK.Input.Key.F25;
				case Key.F26:			return OpenTK.Input.Key.F26;
				case Key.F27:			return OpenTK.Input.Key.F27;
				case Key.F28:			return OpenTK.Input.Key.F28;
				case Key.F29:			return OpenTK.Input.Key.F29;
				case Key.F30:			return OpenTK.Input.Key.F30;
				case Key.F31:			return OpenTK.Input.Key.F31;
				case Key.F32:			return OpenTK.Input.Key.F32;
				case Key.F33:			return OpenTK.Input.Key.F33;
				case Key.F34:			return OpenTK.Input.Key.F34;
				case Key.F35:			return OpenTK.Input.Key.F35;

				case Key.Up:			return OpenTK.Input.Key.Up;
				case Key.Down:			return OpenTK.Input.Key.Down;
				case Key.Left:			return OpenTK.Input.Key.Left;
				case Key.Right:			return OpenTK.Input.Key.Right;

				case Key.Enter:			return OpenTK.Input.Key.Enter;
				case Key.Escape:		return OpenTK.Input.Key.Escape;
				case Key.Space:			return OpenTK.Input.Key.Space;
				case Key.Tab:			return OpenTK.Input.Key.Tab;
				case Key.BackSpace:		return OpenTK.Input.Key.BackSpace;
				case Key.Insert:		return OpenTK.Input.Key.Insert;
				case Key.Delete:		return OpenTK.Input.Key.Delete;
				case Key.PageUp:		return OpenTK.Input.Key.PageUp;
				case Key.PageDown:		return OpenTK.Input.Key.PageDown;
				case Key.Home:			return OpenTK.Input.Key.Home;
				case Key.End:			return OpenTK.Input.Key.End;
				case Key.CapsLock:		return OpenTK.Input.Key.CapsLock;
				case Key.ScrollLock:	return OpenTK.Input.Key.ScrollLock;
				case Key.PrintScreen:	return OpenTK.Input.Key.PrintScreen;
				case Key.Pause:			return OpenTK.Input.Key.Pause;
				case Key.NumLock:		return OpenTK.Input.Key.NumLock;
				case Key.Clear:			return OpenTK.Input.Key.Clear;
				case Key.Sleep:			return OpenTK.Input.Key.Sleep;

				case Key.Keypad1:		return OpenTK.Input.Key.Keypad1;
				case Key.Keypad2:		return OpenTK.Input.Key.Keypad2;
				case Key.Keypad3:		return OpenTK.Input.Key.Keypad3;
				case Key.Keypad4:		return OpenTK.Input.Key.Keypad4;
				case Key.Keypad5:		return OpenTK.Input.Key.Keypad5;
				case Key.Keypad6:		return OpenTK.Input.Key.Keypad6;
				case Key.Keypad7:		return OpenTK.Input.Key.Keypad7;
				case Key.Keypad8:		return OpenTK.Input.Key.Keypad8;
				case Key.Keypad9:		return OpenTK.Input.Key.Keypad9;
				case Key.KeypadDivide:	return OpenTK.Input.Key.KeypadDivide;
				case Key.KeypadMultiply:return OpenTK.Input.Key.KeypadMultiply;
				case Key.KeypadSubtract:return OpenTK.Input.Key.KeypadSubtract;
				case Key.KeypadAdd:		return OpenTK.Input.Key.KeypadAdd;
				case Key.KeypadDecimal:	return OpenTK.Input.Key.KeypadDecimal;
				case Key.KeypadEnter:	return OpenTK.Input.Key.KeypadEnter;

				case Key.A:				return OpenTK.Input.Key.A;
				case Key.B:				return OpenTK.Input.Key.B;
				case Key.C:				return OpenTK.Input.Key.C;
				case Key.D:				return OpenTK.Input.Key.D;
				case Key.E:				return OpenTK.Input.Key.E;
				case Key.F:				return OpenTK.Input.Key.F;
				case Key.G:				return OpenTK.Input.Key.G;
				case Key.H:				return OpenTK.Input.Key.H;
				case Key.I:				return OpenTK.Input.Key.I;
				case Key.J:				return OpenTK.Input.Key.J;
				case Key.K:				return OpenTK.Input.Key.K;
				case Key.L:				return OpenTK.Input.Key.L;
				case Key.M:				return OpenTK.Input.Key.M;
				case Key.N:				return OpenTK.Input.Key.N;
				case Key.O:				return OpenTK.Input.Key.O;
				case Key.P:				return OpenTK.Input.Key.P;
				case Key.Q:				return OpenTK.Input.Key.Q;
				case Key.R:				return OpenTK.Input.Key.R;
				case Key.S:				return OpenTK.Input.Key.S;
				case Key.T:				return OpenTK.Input.Key.T;
				case Key.U:				return OpenTK.Input.Key.U;
				case Key.V:				return OpenTK.Input.Key.V;
				case Key.W:				return OpenTK.Input.Key.W;
				case Key.X:				return OpenTK.Input.Key.X;
				case Key.Y:				return OpenTK.Input.Key.Y;
				case Key.Z:				return OpenTK.Input.Key.Z;
    
				case Key.Number1:		return OpenTK.Input.Key.Number1;
				case Key.Number2:		return OpenTK.Input.Key.Number2;
				case Key.Number3:		return OpenTK.Input.Key.Number3;
				case Key.Number4:		return OpenTK.Input.Key.Number4;
				case Key.Number5:		return OpenTK.Input.Key.Number5;
				case Key.Number6:		return OpenTK.Input.Key.Number6;
				case Key.Number7:		return OpenTK.Input.Key.Number7;
				case Key.Number8:		return OpenTK.Input.Key.Number8;
				case Key.Number9:		return OpenTK.Input.Key.Number9;

				case Key.Tilde:			return OpenTK.Input.Key.Tilde;
				case Key.Minus:			return OpenTK.Input.Key.Minus;
				case Key.Plus:			return OpenTK.Input.Key.Plus;
				case Key.BracketLeft:	return OpenTK.Input.Key.BracketLeft;
				case Key.BracketRight:	return OpenTK.Input.Key.BracketRight;
				case Key.Semicolon:		return OpenTK.Input.Key.Semicolon;
				case Key.Quote:			return OpenTK.Input.Key.Quote;
				case Key.Comma:			return OpenTK.Input.Key.Comma;
				case Key.Period:		return OpenTK.Input.Key.Period;
				case Key.Slash:			return OpenTK.Input.Key.Slash;
				case Key.BackSlash:		return OpenTK.Input.Key.BackSlash;
				case Key.NonUSBackSlash:return OpenTK.Input.Key.NonUSBackSlash;

				case Key.Last:			return OpenTK.Input.Key.LastKey;
			}

			return OpenTK.Input.Key.Unknown;
		}
	}
}
