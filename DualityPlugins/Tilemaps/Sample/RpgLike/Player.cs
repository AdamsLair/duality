using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Input;
using Duality.Components;
using Duality.Editor;
using Duality.Plugins.Tilemaps;
using Duality.Plugins.Tilemaps.Properties;
using Duality.Plugins.Tilemaps.Sample.RpgLike.Properties;

namespace Duality.Plugins.Tilemaps.Sample.RpgLike
{
	/// <summary>
	/// Represents a player and applies user input to the controlled <see cref="CharacterController"/>.
	/// </summary>
	[EditorHintCategory(SampleResNames.CategoryRpgLike)]
	public class Player : Component, ICmpUpdatable
	{
		private CharacterController character;

		public CharacterController Character
		{
			get { return this.character; }
			set { this.character = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (this.character == null) return;

            Vector2 movement = Vector2.Zero;
            if (DualityApp.Keyboard[Key.A])
                movement -= Vector2.UnitX;
            if (DualityApp.Keyboard[Key.D])
                movement += Vector2.UnitX;
            if (DualityApp.Keyboard[Key.W])
                movement -= Vector2.UnitY;
            if (DualityApp.Keyboard[Key.S])
                movement += Vector2.UnitY;

			if (movement.Length > 1.0f)
				movement = movement.Normalized;

			this.character.TargetMovement = movement;
		}
	}
}
