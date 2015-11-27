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
			if (context == InitContext.AddToGameObject)
			{
				// Load test tileset pixel data
				PixelData sourceRaw;
				using (Stream stream = DualityApp.SystemBackend.FileSystem.OpenFile(@"TestTiles.png", FileAccessMode.Read))
				{
					IImageCodec codec = ImageCodec.GetRead(ImageCodec.FormatPng);
					sourceRaw = codec.Read(stream);
				}
				Pixmap sourceData = new Pixmap(sourceRaw);
				sourceData.Save("Data\\TestTilesColor" + Resource.GetFileExtByType<Pixmap>());

				// Create test tileset and compile it
				Tileset tileset = new Tileset();
				tileset.RenderConfig.Add(new TilesetRenderInput
				{
					Id = "mainTex",
					Name = "Main Texture",
					SourceData = sourceData,
					SourceTileSpacing = 1,
					SourceTileSize = new Point2(32, 32)
				});
				tileset.Compile();
				tileset.Save("Data\\TestTiles" + Resource.GetFileExtByType<Tileset>());

				// Create test tilemap object
				if (this.GameObj.ParentScene != null)
				{
					GameObject tilemapObj = new GameObject("Tilemap");
					Tilemap tilemap = tilemapObj.AddComponent<Tilemap>();
					tilemap.Tileset = tileset;
					{
						Grid<Tile> tileData = tilemap.BeginUpdateTiles();
						tileData.Resize(100, 100);
						for (int i = 0; i < tileData.Capacity; i++)
						{
							tileData.RawData[i].Index = i % 100;
						}
						tilemap.EndUpdateTiles();
					}
					this.GameObj.ParentScene.AddObject(tilemapObj);
				}
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) { }
	}
}
