using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BitArray = System.Collections.BitArray;

using Duality;
using Duality.Input;

namespace Duality.Editor
{
	public static class ExtMethodsKeys
	{
		private	static Dictionary<int,int>	mapToDuality;
		static ExtMethodsKeys()
		{
			mapToDuality = new Dictionary<int,int>();
			Dictionary<Keys,string> keysNames = new Dictionary<Keys,string>();
			foreach (Keys k in Enum.GetValues(typeof(Keys)).Cast<Keys>())
			{
				keysNames[k] = k.ToString();
			}

			// Adjust some key names to OpenTK names
			keysNames[Keys.None] = Key.Unknown.ToString();
			keysNames[Keys.Return] = Key.Enter.ToString();
			keysNames[Keys.ShiftKey] = Key.ShiftLeft.ToString();
			keysNames[Keys.LShiftKey] = Key.ShiftLeft.ToString();
			keysNames[Keys.RShiftKey] = Key.ShiftRight.ToString();
			keysNames[Keys.ControlKey] = Key.ControlLeft.ToString();
			keysNames[Keys.LControlKey] = Key.ControlLeft.ToString();
			keysNames[Keys.RControlKey] = Key.ControlRight.ToString();
			keysNames[Keys.NumPad0] = Key.Keypad0.ToString();
			keysNames[Keys.NumPad1] = Key.Keypad1.ToString();
			keysNames[Keys.NumPad2] = Key.Keypad2.ToString();
			keysNames[Keys.NumPad3] = Key.Keypad3.ToString();
			keysNames[Keys.NumPad4] = Key.Keypad4.ToString();
			keysNames[Keys.NumPad5] = Key.Keypad5.ToString();
			keysNames[Keys.NumPad6] = Key.Keypad6.ToString();
			keysNames[Keys.NumPad7] = Key.Keypad7.ToString();
			keysNames[Keys.NumPad8] = Key.Keypad8.ToString();
			keysNames[Keys.NumPad9] = Key.Keypad9.ToString();
			keysNames[Keys.D0] = Key.Number0.ToString();
			keysNames[Keys.D1] = Key.Number1.ToString();
			keysNames[Keys.D2] = Key.Number2.ToString();
			keysNames[Keys.D3] = Key.Number3.ToString();
			keysNames[Keys.D4] = Key.Number4.ToString();
			keysNames[Keys.D5] = Key.Number5.ToString();
			keysNames[Keys.D6] = Key.Number6.ToString();
			keysNames[Keys.D7] = Key.Number7.ToString();
			keysNames[Keys.D8] = Key.Number8.ToString();
			keysNames[Keys.D9] = Key.Number9.ToString();
			keysNames[Keys.Multiply] = Key.KeypadMultiply.ToString();
			keysNames[Keys.Add] = Key.KeypadAdd.ToString();
			keysNames[Keys.Subtract] = Key.KeypadSubtract.ToString();
			keysNames[Keys.Divide] = Key.KeypadDivide.ToString();
			keysNames[Keys.Decimal] = Key.KeypadDecimal.ToString();
			keysNames[Keys.Oemcomma] = Key.Comma.ToString();
			keysNames[Keys.OemPeriod] = Key.Period.ToString();
			keysNames[Keys.OemMinus] = Key.Slash.ToString();
			keysNames[Keys.OemQuestion] = Key.BackSlash.ToString();
			keysNames[Keys.Oemplus] = Key.BracketRight.ToString();
			keysNames[Keys.OemCloseBrackets] = Key.Plus.ToString();
			keysNames[Keys.OemOpenBrackets] = Key.Minus.ToString();
			keysNames[Keys.OemSemicolon] = Key.BracketLeft.ToString();
			keysNames[Keys.Oemtilde] = Key.Semicolon.ToString();
			keysNames[Keys.OemQuotes] = Key.Quote.ToString();
			keysNames[Keys.OemBackslash] = Key.NonUSBackSlash.ToString();
			keysNames[Keys.OemPipe] = Key.Tilde.ToString();
			keysNames[Keys.PageDown] = Key.PageDown.ToString();
			keysNames[Keys.Back] = Key.BackSpace.ToString();

			// Generate mapping
			foreach (var pair in keysNames)
			{
				Key keyVal;
				if (Enum.TryParse<Key>(pair.Value, out keyVal)) mapToDuality[(int)pair.Key] = (int)keyVal;
			}
		}

		public static Key ToDualitySingle(this Keys buttons)
		{
			int k;
			if (mapToDuality.TryGetValue((int)(buttons & ~Keys.Modifiers), out k))
				return (Key)k;
			else
				return Key.Unknown;
		}
		public static BitArray ToDuality(this Keys buttons)
		{
			BitArray result = new BitArray((int)Key.Last + 1, false);
			int k;
			if (mapToDuality.TryGetValue((int)(buttons & ~Keys.Modifiers), out k))
				result[k] = true;
			return result;
		}
	}
}
