using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Renders s <see cref="Tilemap"/> that belongs to the same <see cref="GameObject"/>.
	/// </summary>
	[RequiredComponent(typeof(Tilemap))]
	[EditorHintCategory(TilemapsResNames.CategoryTilemaps)]
	[EditorHintImage(TilemapsResNames.ImageTilemapRenderer)]
	public class TilemapRenderer : Renderer
	{
		public override float BoundRadius
		{
			get { throw new NotImplementedException(); }
		}

		public override void Draw(Drawing.IDrawDevice device)
		{
			throw new NotImplementedException();
		}
	}
}
