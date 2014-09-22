using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Input;

using Duality;
using Duality.Components;
using Duality.Resources;

namespace DualStickSpaceShooter
{
	public class InputMapping
	{
		private	InputMethod	method				= InputMethod.Unknown;
		private	int			creationFrame		= Time.FrameCount;
		private	Vector2		controlMovement		= Vector2.Zero;
		private	float		controlLookAngle	= 0.0f;
		private	bool		controlFireWeapon	= false;

		public InputMethod Method
		{
			get { return this.method; }
			set { this.method = value; }
		}
		public Vector2 ControlMovement
		{
			get { return this.controlMovement; }
		}
		public float ControlLookAngle
		{
			get { return this.controlLookAngle; }
		}
		public bool ControlFireWeapon
		{
			get { return this.controlFireWeapon; }
		}

		public void Update(Transform referenceObj)
		{
			if (this.method == InputMethod.Unknown)
			{
				if (Time.FrameCount - this.creationFrame < 5) return;

				InputMethod[] takenMethods = 
					Scene.Current.FindComponents<Player>()
					.Select(p => p.InputMethod)
					.Where(m => m != InputMethod.Unknown)
					.ToArray();
				InputMethod[] freeMethods = 
					Enum.GetValues(typeof(InputMethod))
					.Cast<InputMethod>()
					.Except(takenMethods)
					.ToArray();
				
				for (int i = 0; i < freeMethods.Length; i++)
				{
					if (this.Detect(freeMethods[i]))
					{
						this.method = freeMethods[i];
						break;
					}
				}
			}
			else
			{
				this.UpdateFrom(referenceObj, this.method);
			}
		}
		
		private bool Detect(InputMethod method)
		{
			switch (method)
			{
				case InputMethod.MouseAndKeyboard:	return this.DetectMouseAndKeyboard(DualityApp.Mouse, DualityApp.Keyboard);
				case InputMethod.FirstGamepad:		return this.DetectGamepad(DualityApp.Gamepads[0]);
				case InputMethod.SecondGamepad:		return this.DetectGamepad(DualityApp.Gamepads[1]);
				default:							return false;
			}
		}
		private bool DetectMouseAndKeyboard(MouseInput mouse, KeyboardInput keyboard)
		{
			return 
				keyboard[Key.W] || keyboard[Key.A] || keyboard[Key.S] ||keyboard[Key.D] ||
				mouse[MouseButton.Left] || 
				mouse.Vel.Length > 50.0f;
		}
		private bool DetectGamepad(GamepadInput gamepad)
		{
			return 
				gamepad.LeftThumbstick.Length > 0.5f || 
				gamepad.RightThumbstick.Length > 0.5f ||
				gamepad[GamepadAxis.RightTrigger] > 0.5f ||
				gamepad[GamepadButton.RightShoulder];
		}
		
		private void UpdateFrom(Transform referenceObj, InputMethod method)
		{
			switch (method)
			{
				case InputMethod.MouseAndKeyboard:	this.UpdateFromMouseAndKeyboard(referenceObj, DualityApp.Mouse, DualityApp.Keyboard); break;
				case InputMethod.FirstGamepad:		this.UpdateFromGamepad(referenceObj, DualityApp.Gamepads[0]); break;
				case InputMethod.SecondGamepad:		this.UpdateFromGamepad(referenceObj, DualityApp.Gamepads[1]); break;
			}
		}
		private void UpdateFromMouseAndKeyboard(Transform referenceObj, MouseInput mouse, KeyboardInput keyboard)
		{
			Camera mainCamera = Scene.Current.FindComponent<Camera>();
			Vector3 objPos = referenceObj.Pos;
			Vector2 objPosOnScreen = mainCamera.GetScreenCoord(objPos).Xy;
			this.controlLookAngle = (mouse.Pos - objPosOnScreen).Angle;

			this.controlMovement = Vector2.Zero;
			{
				if (keyboard[Key.W]) this.controlMovement += new Vector2(0.0f, -1.0f);
				if (keyboard[Key.A]) this.controlMovement += new Vector2(-1.0f, 0.0f);
				if (keyboard[Key.S]) this.controlMovement += new Vector2(0.0f, 1.0f);
				if (keyboard[Key.D]) this.controlMovement += new Vector2(1.0f, 0.0f);
			}
			if (this.controlMovement.Length > 1.0f)
				this.controlMovement.Normalize();

			this.controlFireWeapon = mouse[MouseButton.Left];
		}
		private void UpdateFromGamepad(Transform referenceObj, GamepadInput gamepad)
		{
			if (gamepad.LeftThumbstick.Length > 0.25f)
			{
				float mappedLength = (gamepad.LeftThumbstick.Length - 0.25f) / 0.75f;
				this.controlMovement = gamepad.LeftThumbstick * mappedLength / gamepad.LeftThumbstick.Length;
			}
			else
			{
				this.controlMovement = Vector2.Zero;
			}

			if (gamepad.RightThumbstick.Length > 0.5f)
			{
				this.controlLookAngle = gamepad.RightThumbstick.Angle;
			}

			bool targetAimed = MathF.CircularDist(referenceObj.Angle, this.controlLookAngle) < MathF.RadAngle1 * 10;
			this.controlFireWeapon = 
				(targetAimed && gamepad.RightThumbstick.Length > 0.9f) ||
				gamepad[GamepadAxis.RightTrigger] > 0.5f ||
				gamepad[GamepadButton.RightShoulder];
		}
	}
}
