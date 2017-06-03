﻿using System;
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

		float ICmpRenderer.BoundRadius
		{
			get { return float.MaxValue; }
		}

		bool ICmpRenderer.IsVisible(IDrawDevice device)
		{
			return 
				(device.VisibilityMask & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None &&
				(device.VisibilityMask & VisibilityFlag.AllGroups) != VisibilityFlag.None;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Canvas canvas = new Canvas(device);
			
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
				canvas.State.ColorTint = new ColorRgba(128, 192, 255, 128);
				canvas.FillThickLine(
					DualityApp.Mouse.X - DualityApp.Mouse.XSpeed, 
					DualityApp.Mouse.Y - DualityApp.Mouse.YSpeed, 
					DualityApp.Mouse.X, 
					DualityApp.Mouse.Y, 
					2);
				// Draw the mouse cursor at its local window coordiates
				canvas.State.ColorTint = ColorRgba.White;
				canvas.FillCircle(
					DualityApp.Mouse.X, 
					DualityApp.Mouse.Y, 
					2);
			}
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
				string.Format("Description: /cFF8800FF{0}/cFFFFFFFF/n", input.Description) +
				string.Format("IsAvailable: /cFF8800FF{0}/cFFFFFFFF/n", input.IsAvailable) +
				string.Format("X:     /c44AAFFFF{0,4}/cFFFFFFFF | XSpeed:     /c44AAFFFF{1,4}/cFFFFFFFF/n", input.X, input.XSpeed) +
				string.Format("Y:     /c44AAFFFF{0,4}/cFFFFFFFF | YSpeed:     /c44AAFFFF{1,4}/cFFFFFFFF/n", input.Y, input.YSpeed) +
				string.Format("Wheel: /c44AAFFFF{0,4}/cFFFFFFFF | WheelSpeed: /c44AAFFFF{1,4}/cFFFFFFFF/n", input.WheelPrecise, input.WheelSpeedPrecise) +
				string.Format("Buttons: /c44AAFFFF{0}/cFFFFFFFF/n", activeButtons);
		}
		private void WriteInputStats(ref FormattedText target, KeyboardInput input)
		{
			// Initialize the formatted text block we'll write to
			this.PrepareFormattedText(ref target);

			// Accumulated typed text
			if (input.CharInput.Length > 0)
			{
				typedText += input.CharInput;
				if (typedText.Length > 10)
					typedText = typedText.Substring(typedText.Length - 10, 10);
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
				string.Format("Description: /cFF8800FF{0}/cFFFFFFFF/n", input.Description) +
				string.Format("IsAvailable: /cFF8800FF{0}/cFFFFFFFF/n", input.IsAvailable) +
				string.Format("Text: /c44AAFFFF{0}/cFFFFFFFF/n", typedText) +
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
			foreach (JoystickButton button in Enum.GetValues(typeof(JoystickButton)))
			{
				if (input.ButtonPressed(button))
				{
					if (activeButtons.Length != 0)
						activeButtons += ", ";
					activeButtons += button.ToString();
				}
			}

			// Determine all joystick axis values
			string axisValues = "";
			foreach (JoystickAxis axis in Enum.GetValues(typeof(JoystickAxis)))
			{
				if (input.AxisValue(axis) == 0.0f && (int)axis >= input.AxisCount) 
					break;

				if (axisValues.Length != 0)
					axisValues += ", ";
				axisValues += string.Format("{0:F}", input.AxisValue(axis));
			}

			// Determine all joystick hat values
			string hatValues = "";
			foreach (JoystickHat hat in Enum.GetValues(typeof(JoystickHat)))
			{
				if (input.HatPosition(hat) == JoystickHatPosition.Centered && (int)hat >= input.HatCount) 
					break;

				if (hatValues.Length != 0)
					hatValues += ", ";
				hatValues += string.Format("({0})", input.HatPosition(hat));
			}

			return 
				string.Format("Description: /cFF8800FF{0}/cFFFFFFFF/n", input.Description) +
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
				string.Format("Description: /cFF8800FF{0}/cFFFFFFFF/n", input.Description) +
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
