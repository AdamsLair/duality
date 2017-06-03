using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Plugins.Tilemaps.Properties
{
	/// <summary>
	/// This static class contains constant string representations of certain resource names.
	/// </summary>
	public static class TilemapsResNames
	{
		private const string ManifestBaseName    = "Duality.Plugins.Tilemaps.EmbeddedResources.";

		public const string CategoryTilemaps     = "Tilemaps";

		public const string ImageTilemap         = ManifestBaseName + "ImageTilemap.png";
		public const string ImageTilemapRenderer = ManifestBaseName + "ImageTilemapRenderer.png";
		public const string ImageTilemapCollider = ManifestBaseName + "ImageTilemapCollider.png";
		public const string ImageTileset         = ManifestBaseName + "ImageTileset.png";
		public const string ImageActorRenderer   = ManifestBaseName + "ImageActorRenderer.png";
	}
}
