using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;
using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Plugins.Tilemaps;

namespace Duality.Plugins.Tilemaps.Sample
{
	[EditorHintCategory("Test")]
	public class TestComponent : Component, ICmpInitializable
	{
		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				// Test Code
				PixelData sourceRaw;
				using (Stream stream = DualityApp.SystemBackend.FileSystem.OpenFile(@"TestTiles.png", FileAccessMode.Read))
				{
					IImageCodec codec = ImageCodec.GetRead(ImageCodec.FormatPng);
					sourceRaw = codec.Read(stream);
				}
				Pixmap sourceData = new Pixmap(sourceRaw);

				Tileset tileset = new Tileset();
				tileset.VisualInputLayers.Add(new TilesetVisualInput
				{
					SourceData = sourceData,
					SourceTileSpacing = 1,
					SourceTileSize = new Point2(32, 32)
				});
				tileset.Compile();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) { }
	}
}
