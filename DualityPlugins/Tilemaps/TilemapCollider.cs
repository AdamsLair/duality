using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Uses the information from the local <see cref="Tilemap"/> to configure the local <see cref="RigidBody"/> for 
	/// simulating physical interaction with the <see cref="Tilemap"/>.
	/// </summary>
	[RequiredComponent(typeof(Tilemap))]
	[RequiredComponent(typeof(RigidBody))]
	[EditorHintCategory(TilemapsResNames.CategoryTilemaps)]
	[EditorHintImage(TilemapsResNames.ImageTilemapCollider)]
	public class TilemapCollider : Component
	{
	}
}
