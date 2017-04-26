using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Input;

using DualityKey = Duality.Input.Key;
using VirtualKeyCode = System.Windows.Forms.Keys;

namespace Duality.Editor
{
	public static class ExtMethodsVirtualKeyCode
	{
		private	static Dictionary<VirtualKeyCode,DualityKey> virtualKeyToDualityKey;

		static ExtMethodsVirtualKeyCode()
		{
			virtualKeyToDualityKey = new Dictionary<VirtualKeyCode,DualityKey>();

			// Build a mapping from virtual keycodes to physical scancodes
			foreach (VirtualKeyCode virtualKey in Enum.GetValues(typeof(VirtualKeyCode)))
			{
				uint scancode = NativeMethods.MapVirtualKey((uint)virtualKey, NativeMethods.KeyMapType.MAPVK_VK_TO_VSC);
				if (scancode == 0) continue;

				DualityKey dualityKey = ParseScanCode(scancode);
				if (dualityKey == DualityKey.Unknown) continue;

				virtualKeyToDualityKey[virtualKey] = dualityKey;
			}

			// Override some specific keys where we really need the virtual key information
			virtualKeyToDualityKey[VirtualKeyCode.None] = DualityKey.Unknown;
			virtualKeyToDualityKey[VirtualKeyCode.Return] = DualityKey.Enter;
			virtualKeyToDualityKey[VirtualKeyCode.ShiftKey] = DualityKey.ShiftLeft;
			virtualKeyToDualityKey[VirtualKeyCode.LShiftKey] = DualityKey.ShiftLeft;
			virtualKeyToDualityKey[VirtualKeyCode.RShiftKey] = DualityKey.ShiftRight;
			virtualKeyToDualityKey[VirtualKeyCode.ControlKey] = DualityKey.ControlLeft;
			virtualKeyToDualityKey[VirtualKeyCode.LControlKey] = DualityKey.ControlLeft;
			virtualKeyToDualityKey[VirtualKeyCode.RControlKey] = DualityKey.ControlRight;
			virtualKeyToDualityKey[VirtualKeyCode.Menu] = DualityKey.AltLeft;
			virtualKeyToDualityKey[VirtualKeyCode.LMenu] = DualityKey.AltLeft;
			virtualKeyToDualityKey[VirtualKeyCode.RMenu] = DualityKey.AltRight;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad0] = DualityKey.Keypad0;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad1] = DualityKey.Keypad1;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad2] = DualityKey.Keypad2;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad3] = DualityKey.Keypad3;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad4] = DualityKey.Keypad4;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad5] = DualityKey.Keypad5;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad6] = DualityKey.Keypad6;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad7] = DualityKey.Keypad7;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad8] = DualityKey.Keypad8;
			virtualKeyToDualityKey[VirtualKeyCode.NumPad9] = DualityKey.Keypad9;
			virtualKeyToDualityKey[VirtualKeyCode.Multiply] = DualityKey.KeypadMultiply;
			virtualKeyToDualityKey[VirtualKeyCode.Add] = DualityKey.KeypadAdd;
			virtualKeyToDualityKey[VirtualKeyCode.Subtract] = DualityKey.KeypadSubtract;
			virtualKeyToDualityKey[VirtualKeyCode.Divide] = DualityKey.KeypadDivide;
			virtualKeyToDualityKey[VirtualKeyCode.Decimal] = DualityKey.KeypadDecimal;
			virtualKeyToDualityKey[VirtualKeyCode.PageDown] = DualityKey.PageDown;
			virtualKeyToDualityKey[VirtualKeyCode.Back] = DualityKey.BackSpace;

			//
			// Note that the above virtual key mapping is by definition incomplete:
			// Virtual keys do not distinguish between left and right modifiers.
			// Therefore it is not possible to map from a virtual key to the correct
			// scancode in all cases. The only way around this would be to parse
			// scancodes directly.
			//
			// The reason we're preferring scancodes over virtual keys is detailed here:
			// https://github.com/AdamsLair/duality/issues/289#issuecomment-184367749
			//
		}
		private static DualityKey ParseScanCode(uint scancode)
		{
			switch (scancode)
			{
				case 0:   return DualityKey.Unknown;
				case 1:   return DualityKey.Escape;
				case 2:   return DualityKey.Number1;
				case 3:   return DualityKey.Number2;
				case 4:   return DualityKey.Number3;
				case 5:   return DualityKey.Number4;
				case 6:   return DualityKey.Number5;
				case 7:   return DualityKey.Number6;
				case 8:   return DualityKey.Number7;
				case 9:   return DualityKey.Number8;
				case 10:  return DualityKey.Number9;
				case 11:  return DualityKey.Number0;
				case 12:  return DualityKey.Minus;
				case 13:  return DualityKey.Plus;
				case 14:  return DualityKey.BackSpace;
				case 15:  return DualityKey.Tab;
				case 16:  return DualityKey.Q;
				case 17:  return DualityKey.W;
				case 18:  return DualityKey.E;
				case 19:  return DualityKey.R;
				case 20:  return DualityKey.T;
				case 21:  return DualityKey.Y;
				case 22:  return DualityKey.U;
				case 23:  return DualityKey.I;
				case 24:  return DualityKey.O;
				case 25:  return DualityKey.P;
				case 26:  return DualityKey.BracketLeft;
				case 27:  return DualityKey.BracketRight;
				case 28:  return DualityKey.Enter;
				case 29:  return DualityKey.ControlLeft;
				case 30:  return DualityKey.A;
				case 31:  return DualityKey.S;
				case 32:  return DualityKey.D;
				case 33:  return DualityKey.F;
				case 34:  return DualityKey.G;
				case 35:  return DualityKey.H;
				case 36:  return DualityKey.J;
				case 37:  return DualityKey.K;
				case 38:  return DualityKey.L;
				case 39:  return DualityKey.Semicolon;
				case 40:  return DualityKey.Quote;
				case 41:  return DualityKey.Tilde;
				case 42:  return DualityKey.ShiftLeft;
				case 43:  return DualityKey.BackSlash;
				case 44:  return DualityKey.Z;
				case 45:  return DualityKey.X;
				case 46:  return DualityKey.C;
				case 47:  return DualityKey.V;
				case 48:  return DualityKey.B;
				case 49:  return DualityKey.N;
				case 50:  return DualityKey.M;
				case 51:  return DualityKey.Comma;
				case 52:  return DualityKey.Period;
				case 53:  return DualityKey.Slash;
				case 54:  return DualityKey.ShiftRight;
				case 55:  return DualityKey.PrintScreen;
				case 56:  return DualityKey.AltLeft;
				case 57:  return DualityKey.Space;
				case 58:  return DualityKey.CapsLock;
				case 59:  return DualityKey.F1;
				case 60:  return DualityKey.F2;
				case 61:  return DualityKey.F3;
				case 62:  return DualityKey.F4;
				case 63:  return DualityKey.F5;
				case 64:  return DualityKey.F6;
				case 65:  return DualityKey.F7;
				case 66:  return DualityKey.F8;
				case 67:  return DualityKey.F9;
				case 68:  return DualityKey.F10;
				case 69:  return DualityKey.NumLock;
				case 70:  return DualityKey.ScrollLock;
				case 71:  return DualityKey.Home;
				case 72:  return DualityKey.Up;
				case 73:  return DualityKey.PageUp;
				case 74:  return DualityKey.KeypadSubtract;
				case 75:  return DualityKey.Left;
				case 76:  return DualityKey.Keypad5;
				case 77:  return DualityKey.Right;
				case 78:  return DualityKey.KeypadAdd;
				case 79:  return DualityKey.End;
				case 80:  return DualityKey.Down;
				case 81:  return DualityKey.PageDown;
				case 82:  return DualityKey.Insert;
				case 83:  return DualityKey.Delete;
				case 84:  return DualityKey.Unknown;
				case 85:  return DualityKey.Unknown;
				case 86:  return DualityKey.NonUSBackSlash;
				case 87:  return DualityKey.F11;
				case 88:  return DualityKey.F12;
				case 89:  return DualityKey.Pause;
				case 90:  return DualityKey.Unknown;
				case 91:  return DualityKey.WinLeft;
				case 92:  return DualityKey.WinRight;
				case 93:  return DualityKey.Menu;
				case 94:  return DualityKey.Unknown;
				case 95:  return DualityKey.Unknown;
				case 96:  return DualityKey.Unknown;
				case 97:  return DualityKey.Unknown;
				case 98:  return DualityKey.Unknown;
				case 99:  return DualityKey.Unknown;
				case 100: return DualityKey.F13;
				case 101: return DualityKey.F14;
				case 102: return DualityKey.F15;
				case 103: return DualityKey.F16;
				case 104: return DualityKey.F17;
				case 105: return DualityKey.F18;
				case 106: return DualityKey.F19;
				default:  return DualityKey.Unknown;
			}
		}

		public static Key ToDualityKey(this VirtualKeyCode virtualKeyCode)
		{
			VirtualKeyCode virtualKeyWithoutModifiers = virtualKeyCode & ~VirtualKeyCode.Modifiers;
			DualityKey dualityKey;
			if (virtualKeyToDualityKey.TryGetValue(virtualKeyWithoutModifiers, out dualityKey))
				return dualityKey;
			else
				return DualityKey.Unknown;
		}
	}
}
