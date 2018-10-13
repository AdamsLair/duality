using System;
using System.Collections.Generic;

using Duality.Input;

using OpenTK.Input;

namespace Duality.Backend.DefaultOpenTK
{
	public class GlobalGamepadInputSource : IGamepadInputSource
	{
		private static List<GlobalGamepadInputSource> cachedDevices = new List<GlobalGamepadInputSource>();

		private Guid productId;
		private string productName;
		private int deviceIndex;
		private bool hasAxesOrButtons;
		private GamePadState state;
		private GamePadCapabilities caps;
		
		public string Id
		{
			get { return string.Format("Gamepad {0}", this.deviceIndex); }
		}
		public Guid ProductId
		{
			get { return this.productId; }
		}
		public string ProductName
		{
			get { return this.productName; }
		}
		public bool IsAvailable
		{
			get { return this.caps.IsConnected && (this.caps.IsMapped || this.hasAxesOrButtons); }
		}
		public bool this[GamepadButton button]
		{
			get 
			{
				switch (button)
				{
					case GamepadButton.A:				return this.state.Buttons.A == ButtonState.Pressed;
					case GamepadButton.B:				return this.state.Buttons.B == ButtonState.Pressed;
					case GamepadButton.X:				return this.state.Buttons.X == ButtonState.Pressed;
					case GamepadButton.Y:				return this.state.Buttons.Y == ButtonState.Pressed;

					case GamepadButton.DPadLeft:		return this.state.DPad.Left == ButtonState.Pressed;
					case GamepadButton.DPadRight:		return this.state.DPad.Right == ButtonState.Pressed;
					case GamepadButton.DPadUp:			return this.state.DPad.Up == ButtonState.Pressed;
					case GamepadButton.DPadDown:		return this.state.DPad.Down == ButtonState.Pressed;

					case GamepadButton.LeftShoulder:	return this.state.Buttons.LeftShoulder == ButtonState.Pressed;
					case GamepadButton.LeftStick:		return this.state.Buttons.LeftStick == ButtonState.Pressed;
					case GamepadButton.RightShoulder:	return this.state.Buttons.RightShoulder == ButtonState.Pressed;
					case GamepadButton.RightStick:		return this.state.Buttons.RightStick == ButtonState.Pressed;

					case GamepadButton.BigButton:		return this.state.Buttons.BigButton == ButtonState.Pressed;
					case GamepadButton.Back:			return this.state.Buttons.Back == ButtonState.Pressed;
					case GamepadButton.Start:			return this.state.Buttons.Start == ButtonState.Pressed;

					default: return false;
				}
			}
		}
		public float this[GamepadAxis axis]
		{
			get 
			{
				switch (axis)
				{
					case GamepadAxis.LeftTrigger:		return MathF.Clamp(this.state.Triggers.Left, 0.0f, 1.0f);
					case GamepadAxis.LeftThumbstickX:	return MathF.Clamp(this.state.ThumbSticks.Left.X, -1.0f, 1.0f);
					case GamepadAxis.LeftThumbstickY:	return MathF.Clamp(this.state.ThumbSticks.Left.Y, -1.0f, 1.0f);

					case GamepadAxis.RightTrigger:		return MathF.Clamp(this.state.Triggers.Right, 0.0f, 1.0f);
					case GamepadAxis.RightThumbstickX:	return MathF.Clamp(this.state.ThumbSticks.Right.X, -1.0f, 1.0f);
					case GamepadAxis.RightThumbstickY:	return MathF.Clamp(this.state.ThumbSticks.Right.Y, -1.0f, 1.0f);

					default: return 0.0f;
				}
			}
		}

		public GlobalGamepadInputSource(int deviceIndex)
		{
			this.deviceIndex = deviceIndex;
		}

		public void UpdateState()
		{
			// Retrieve the gamepads hardware GUID from the corresponding joystick device
			this.productId = Joystick.GetGuid(this.deviceIndex);

			this.productName = GamePad.GetName(this.deviceIndex);
			this.caps = GamePad.GetCapabilities(this.deviceIndex);
			this.state = GamePad.GetState(this.deviceIndex);

			// If it's not a well-known gamepad, check the corresponding joystick whether there are any axes or buttons
			if (!this.caps.IsMapped)
			{
				JoystickCapabilities joystickCaps = Joystick.GetCapabilities(this.deviceIndex);
				this.hasAxesOrButtons = joystickCaps.AxisCount > 0 || joystickCaps.ButtonCount > 0 || joystickCaps.HatCount > 0;
			}
		}
		public void SetVibration(float left, float right)
		{
			GamePad.SetVibration(this.deviceIndex, left, right);
		}
		
		public static void UpdateAvailableDecives(GamepadInputCollection inputManager)
		{
			const int MinDeviceCheckCount = 8;
			const int MaxDeviceCheckCount = 32;

			// Determine which devices are currently active already, so we can skip their indices
			List<int> skipIndices = null;
			foreach (GamepadInput input in inputManager)
			{
				GlobalGamepadInputSource existingDevice = input.Source as GlobalGamepadInputSource;
				if (existingDevice != null)
				{
					if (skipIndices == null) skipIndices = new List<int>();
					skipIndices.Add(existingDevice.deviceIndex);
				}
			}

			// Iterate over device indices and see what responds
			int deviceIndex = -1;
			while (deviceIndex < MaxDeviceCheckCount)
			{
				deviceIndex++;

				if (skipIndices != null && skipIndices.Contains(deviceIndex))
					continue;

				while (deviceIndex >= cachedDevices.Count)
				{
					cachedDevices.Add(new GlobalGamepadInputSource(cachedDevices.Count));
				}
				GlobalGamepadInputSource gamepad = cachedDevices[deviceIndex];
				gamepad.UpdateState();

				if (gamepad.IsAvailable)
				{
					inputManager.AddSource(gamepad);
					Logs.Core.Write(
						"Detected new Gamepad Input: \"{0}\" ({1} | {2}) at index {3}",
						gamepad.Id, 
						gamepad.ProductId, 
						gamepad.ProductName, 
						deviceIndex);
				}
				else if (deviceIndex >= MinDeviceCheckCount)
					break;
			}
		}
	}
}
