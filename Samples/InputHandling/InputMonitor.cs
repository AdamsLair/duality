using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Input;

namespace InputHandling
{
	public class InputMonitor : Component, ICmpRenderer
	{
		[DontSerialize] private string typedText = "";
		[DontSerialize] private FormattedText mouseStatsText = null;
		[DontSerialize] private FormattedText keyboardStatsText = null;
		[DontSerialize] private FormattedText joystickStatsText = null;
		[DontSerialize] private FormattedText gamepadStatsText = null;

		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Canvas canvas = new Canvas();
			canvas.Begin(device);
			
			// Update input stats texts for drawing
			this.WriteInputStats(ref this.mouseStatsText, DualityApp.Mouse);
			this.WriteInputStats(ref this.keyboardStatsText, DualityApp.Keyboard);
			this.WriteInputStats(ref this.joystickStatsText, DualityApp.Joysticks);
			this.WriteInputStats(ref this.gamepadStatsText, DualityApp.Gamepads);

			// Draw input stats texts
			{
				int y = 10;

				canvas.DrawText(this.mouseStatsText, 10, y, 0, null, Alignment.TopLeft, true);
				y += 20 + (int)this.mouseStatsText.TextMetrics.Size.Y;

				canvas.DrawText(this.keyboardStatsText, 10, y, 0, null, Alignment.TopLeft, true);
				y += 20 + (int)this.keyboardStatsText.TextMetrics.Size.Y;

				canvas.DrawText(this.joystickStatsText, 10, y, 0, null, Alignment.TopLeft, true);
				y += 20 + (int)this.joystickStatsText.TextMetrics.Size.Y;

				canvas.DrawText(this.gamepadStatsText, 10, y, 0, null, Alignment.TopLeft, true);
				y += 20 + (int)this.gamepadStatsText.TextMetrics.Size.Y;
			}

			// Draw the mouse cursor's movement trail
			if (DualityApp.Mouse.IsAvailable)
			{
				canvas.State.ColorTint = new ColorRgba(255, 255, 255, 128);
				canvas.FillThickLine(
					DualityApp.Mouse.Pos.X - DualityApp.Mouse.Vel.X, 
					DualityApp.Mouse.Pos.Y - DualityApp.Mouse.Vel.Y, 
					DualityApp.Mouse.Pos.X, 
					DualityApp.Mouse.Pos.Y, 
					2);
				// Draw the mouse cursor at its local window coordiates
				canvas.State.ColorTint = ColorRgba.White;
				canvas.FillCircle(
					DualityApp.Mouse.Pos.X, 
					DualityApp.Mouse.Pos.Y, 
					2);
			}

			canvas.End();
		}

		private void WriteInputStats(ref FormattedText target, MouseInput input)
		{
			// Initialize the formatted text block we'll write to
			this.PrepareFormattedText(ref target);

			// Determine all pressed mouse buttons
			string activeButtons = "";
			foreach (MouseButton button in Enum.GetValues(typeof(MouseButton)))
			{
				if (input.ButtonPressed(button))
				{
					if (activeButtons.Length != 0)
						activeButtons += ", ";
					activeButtons += button.ToString();
				}
			}

			// Compose the formatted text to display
			target.SourceText = 
				"/f[1]Mouse Stats/f[0]/n/n" +
				string.Format("Id: /cFF8800FF{0}/cFFFFFFFF/n", input.Id) +
				string.Format("ProductId: /cFF8800FF{0}/cFFFFFFFF/n", input.ProductId) +
				string.Format("ProductName: /cFF8800FF{0}/cFFFFFFFF/n", input.ProductName) +
				string.Format("IsAvailable: /cFF8800FF{0}/cFFFFFFFF/n", input.IsAvailable) +
				string.Format("X:     /c44AAFFFF{0,8:F}/cFFFFFFFF | XSpeed:     /c44AAFFFF{1,8:F}/cFFFFFFFF/n", input.Pos.X, input.Vel.X) +
				string.Format("Y:     /c44AAFFFF{0,8:F}/cFFFFFFFF | YSpeed:     /c44AAFFFF{1,8:F}/cFFFFFFFF/n", input.Pos.Y, input.Vel.Y) +
				string.Format("Wheel: /c44AAFFFF{0,8:F}/cFFFFFFFF | WheelSpeed: /c44AAFFFF{1,8:F}/cFFFFFFFF/n", input.Wheel, input.WheelSpeed) +
				string.Format("Buttons: /c44AAFFFF{0}/cFFFFFFFF/n", activeButtons);
		}
		private void WriteInputStats(ref FormattedText target, KeyboardInput input)
		{
			// Initialize the formatted text block we'll write to
			this.PrepareFormattedText(ref target);

			// Accumulated typed text
			if (input.CharInput.Length > 0)
			{
				this.typedText += input.CharInput;
				if (this.typedText.Length > 10)
					this.typedText = this.typedText.Substring(this.typedText.Length - 10, 10);
			}

			// Determine all pressed mouse buttons
			string activeKeys = "";
			foreach (Key key in Enum.GetValues(typeof(Key)))
			{
				if (input.KeyPressed(key))
				{
					if (activeKeys.Length != 0)
						activeKeys += ", ";
					activeKeys += key.ToString();
				}
			}

			// Compose the formatted text to display
			target.SourceText = 
				"/f[1]Keyboard Stats/f[0]/n/n" +
				string.Format("Id: /cFF8800FF{0}/cFFFFFFFF/n", input.Id) +
				string.Format("ProductId: /cFF8800FF{0}/cFFFFFFFF/n", input.ProductId) +
				string.Format("ProductName: /cFF8800FF{0}/cFFFFFFFF/n", input.ProductName) +
				string.Format("IsAvailable: /cFF8800FF{0}/cFFFFFFFF/n", input.IsAvailable) +
				string.Format("Text: /c44AAFFFF{0}/cFFFFFFFF/n", this.typedText) +
				string.Format("Keys: /c44AAFFFF{0}/cFFFFFFFF/n", activeKeys);
		}
		private void WriteInputStats(ref FormattedText target, JoystickInputCollection inputCollection)
		{
			// Initialize the formatted text block we'll write to
			this.PrepareFormattedText(ref target);

			// Compose the formatted text to display
			string allText = "/f[1]Joystick Stats/f[0]";
			foreach (JoystickInput input in inputCollection)
			{
				string inputText = this.GetInputStatsText(input);
				if (allText.Length != 0)
					allText += "/n/n";
				allText += inputText;
			}

			target.SourceText = allText;
		}
		private string GetInputStatsText(JoystickInput input)
		{
			// Determine all pressed joystick buttons
			string activeButtons = "";
			for (int i = 0; i < input.ButtonCount; i++)
			{
				if (input.ButtonPressed(i))
				{
					if (activeButtons.Length != 0)
						activeButtons += ", ";
					activeButtons += string.Format("Button #{0}", i).ToString();
				}
			}

			// Determine all joystick axis values
			string axisValues = "";
			for (int i = 0; i < input.AxisCount; i++)
			{
				if (input.AxisValue(i) == 0.0f) 
					break;

				if (axisValues.Length != 0)
					axisValues += ", ";
				axisValues += string.Format("{0:F}", input.AxisValue(i));
			}

			// Determine all joystick hat values
			string hatValues = "";
			for (int i = 0; i < input.HatCount; i++)
			{
				if (input.HatPosition(i) == JoystickHatPosition.Centered) 
					break;

				if (hatValues.Length != 0)
					hatValues += ", ";
				hatValues += string.Format("({0})", input.HatPosition(i));
			}

			return 
				string.Format("Id: /cFF8800FF{0}/cFFFFFFFF/n", input.Id) +
				string.Format("ProductId: /cFF8800FF{0}/cFFFFFFFF/n", input.ProductId) +
				string.Format("ProductName: /cFF8800FF{0}/cFFFFFFFF/n", input.ProductName) +
				string.Format("IsAvailable: /cFF8800FF{0}/cFFFFFFFF/n", input.IsAvailable) +
				string.Format("ButtonCount: /cFF8800FF{0,2}/cFFFFFFFF | AxisCount: /cFF8800FF{1,2}/cFFFFFFFF | HatCount: /cFF8800FF{2,2}/cFFFFFFFF/n", input.ButtonCount, input.AxisCount, input.HatCount) +
				string.Format("Buttons: /c44AAFFFF{0}/cFFFFFFFF/n", activeButtons) +
				string.Format("Axes:    /c44AAFFFF{0}/cFFFFFFFF/n", axisValues) +
				string.Format("Hats:    /c44AAFFFF{0}/cFFFFFFFF", hatValues);
		}
		private void WriteInputStats(ref FormattedText target, GamepadInputCollection inputCollection)
		{
			// Initialize the formatted text block we'll write to
			this.PrepareFormattedText(ref target);

			// Compose the formatted text to display
			string allText = "/f[1]Gamepad Stats/f[0]";
			foreach (GamepadInput input in inputCollection)
			{
				string inputText = this.GetInputStatsText(input);
				if (allText.Length != 0)
					allText += "/n/n";
				allText += inputText;
			}

			target.SourceText = allText;
		}
		private string GetInputStatsText(GamepadInput input)
		{
			// Determine all pressed gamepad buttons
			string activeButtons = "";
			foreach (GamepadButton button in Enum.GetValues(typeof(GamepadButton)))
			{
				if (input.ButtonPressed(button))
				{
					if (activeButtons.Length != 0)
						activeButtons += ", ";
					activeButtons += button.ToString();
				}
			}

			return
				string.Format("Id: /cFF8800FF{0}/cFFFFFFFF/n", input.Id) +
				string.Format("ProductId: /cFF8800FF{0}/cFFFFFFFF/n", input.ProductId) +
				string.Format("ProductName: /cFF8800FF{0}/cFFFFFFFF/n", input.ProductName) +
				string.Format("IsAvailable: /cFF8800FF{0}/cFFFFFFFF/n", input.IsAvailable) +
				string.Format("Buttons:          /c44AAFFFF{0}/cFFFFFFFF/n", activeButtons) +
				string.Format("Left  Trigger:    /c44AAFFFF{0}/cFFFFFFFF/n", input.LeftTrigger) +
				string.Format("Left  Thumbstick: /c44AAFFFF{0}/cFFFFFFFF/n", input.LeftThumbstick) +
				string.Format("Right Trigger:    /c44AAFFFF{0}/cFFFFFFFF/n", input.RightTrigger) +
				string.Format("Right Thumbstick: /c44AAFFFF{0}/cFFFFFFFF/n", input.RightThumbstick) +
				string.Format("Directional Pad:  /c44AAFFFF{0}/cFFFFFFFF", input.DPad);
		}

		private void PrepareFormattedText(ref FormattedText target)
		{
			if (target != null) return;

			target = new FormattedText();
			target.Fonts = new[]
			{
				Font.GenericMonospace8,
				Font.GenericMonospace10
			};
		}
	}
}
