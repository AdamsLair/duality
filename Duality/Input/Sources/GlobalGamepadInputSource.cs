using System;
using OpenTK.Input;

namespace Duality
{
	public class GlobalGamepadInputSource : IGamepadInputSource
	{
		private	int	deviceIndex;
		private	GamePadState state;
		private	GamePadCapabilities caps;
		
		public string Description
		{
			get { return string.Format("Gamepad {0}", this.deviceIndex); }
		}
		public bool IsAvailable
		{
			get { return this.caps.IsConnected; }
		}
		public GamePadType GamepadType
		{
			get { return this.caps.GamePadType; }
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
					case GamepadAxis.LeftThumbstickY:	return MathF.Clamp(-this.state.ThumbSticks.Left.Y, -1.0f, 1.0f);

					case GamepadAxis.RightTrigger:		return MathF.Clamp(this.state.Triggers.Right, 0.0f, 1.0f);
					case GamepadAxis.RightThumbstickX:	return MathF.Clamp(this.state.ThumbSticks.Right.X, -1.0f, 1.0f);
					case GamepadAxis.RightThumbstickY:	return MathF.Clamp(-this.state.ThumbSticks.Right.Y, -1.0f, 1.0f);

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
			this.caps = GamePad.GetCapabilities(this.deviceIndex);
			this.state = GamePad.GetState(this.deviceIndex);
		}
		public void SetVibration(float left, float right)
		{
			GamePad.SetVibration(this.deviceIndex, left, right);
		}
	}
}
