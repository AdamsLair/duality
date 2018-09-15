using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Input;
using Duality.Components;

namespace CameraController
{
	[RequiredComponent(typeof(Transform))]
	public class PlayerCharacter : Component, ICmpUpdatable
	{
		private float speed;

		/// <summary>
		/// [GET / SET] The maximum movement speed of the player character.
		/// </summary>
		public float Speed
		{
			get { return this.speed; }
			set { this.speed = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			Transform transform = this.GameObj.Transform;
			Vector2 movement = Vector2.Zero;

			// Horizontal keyboard movement
			if (DualityApp.Keyboard[Key.Left])
				movement += new Vector2(-1.0f, 0.0f);
			else if (DualityApp.Keyboard[Key.Right])
				movement += new Vector2(1.0f, 0.0f);

			// Vertical keyboard movement
			if (DualityApp.Keyboard[Key.Up])
				movement += new Vector2(0.0f, -1.0f);
			else if (DualityApp.Keyboard[Key.Down])
				movement += new Vector2(0.0f, 1.0f);

			// Is there a Gamepad we can use?
			GamepadInput gamepad = DualityApp.Gamepads.FirstOrDefault(g => g.IsAvailable);
			if (gamepad != null)
			{
				// If the left thumbstick does something significant, use it as movement input.
				if (gamepad.LeftThumbstick.Length > 0.2f)
				{
					movement += gamepad.LeftThumbstick;
				}
			}

			// Make sure we don't move faster than intended when pressing all the keys at once.
			movement /= MathF.Max(movement.Length, 1.0f);

			// Apply movement
			transform.MoveBy(movement * this.speed * Time.TimeMult);
		}
	}
}
