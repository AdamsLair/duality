using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Input;

using Key = OpenTK.Input.Key;

namespace DualStickSpaceShooter
{
	public class InputMapping
	{
		private	InputMethod	method				= InputMethod.Unknown;
		private	int			creationFrame		= Time.FrameCount;
		private	Vector2		controlMovement		= Vector2.Zero;
		private	float		controlLookSpeed	= 0.0f;
		private	float		controlLookAngle	= 0.0f;
		private	bool		controlFireWeapon	= false;
		private	bool		controlQuit			= false;
		private	bool		controlStart		= false;

		public InputMethod Method
		{
			get { return this.method; }
			set { this.method = value; }
		}
		public Vector2 ControlMovement
		{
			get { return this.controlMovement; }
		}
		public float ControlLookSpeed
		{
			get { return this.controlLookSpeed; }
		}
		public float ControlLookAngle
		{
			get { return this.controlLookAngle; }
		}
		public bool ControlFireWeapon
		{
			get { return this.controlFireWeapon; }
		}
		public bool ControlQuit
		{
			get { return this.controlQuit; }
		}
		public bool ControlStart
		{
			get { return this.controlStart; }
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
			Vector3 objPos = (referenceObj != null) ? referenceObj.Pos : Vector3.Zero;
			Vector2 objPosOnScreen = (mainCamera != null) ? mainCamera.GetScreenCoord(objPos).Xy : Vector2.Zero;
			this.controlLookAngle = (mouse.Pos - objPosOnScreen).Angle;
			this.controlLookSpeed = MathF.Clamp((mouse.Pos - objPosOnScreen).Length / 100.0f, 0.0f, 1.0f);

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
			this.controlQuit = keyboard.KeyHit(Key.Escape);
			this.controlStart = keyboard.KeyHit(Key.Enter);
		}
		private void UpdateFromGamepad(Transform referenceObj, GamepadInput gamepad)
		{
			float referenceAngle = (referenceObj != null) ? referenceObj.Angle : 0.0f;

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
				this.controlLookSpeed = (gamepad.RightThumbstick.Length - 0.5f) / 0.5f;
			}
			else if (gamepad.LeftThumbstick.Length > 0.25f)
			{
				this.controlLookAngle = gamepad.LeftThumbstick.Angle;
				this.controlLookSpeed = (gamepad.LeftThumbstick.Length - 0.25f) / 0.75f;
			}

			bool targetAimed = MathF.CircularDist(referenceAngle, this.controlLookAngle) < MathF.RadAngle1 * 10;
			this.controlFireWeapon = 
				(targetAimed && gamepad.RightThumbstick.Length > 0.9f) ||
				gamepad[GamepadAxis.RightTrigger] > 0.5f ||
				gamepad[GamepadButton.RightShoulder] ||
				gamepad[GamepadButton.A];
			this.controlQuit = gamepad.ButtonHit(GamepadButton.Back);
			this.controlStart = gamepad.ButtonHit(GamepadButton.Start);
		}
	}
}
