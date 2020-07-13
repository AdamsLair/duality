namespace Duality.Editor.Plugins.Tilemaps
{
	public class TilemapsSettings
	{
		public TilemapToolSourcePaletteSettings TilemapToolSourcePaletteSettings { get; set; } = new TilemapToolSourcePaletteSettings();
		public TilesetEditorSettings TilesetEditorSettings { get; set; } = new TilesetEditorSettings();
	}

	public class TilemapToolSourcePaletteSettings
	{
		private bool darkBackground;
		public bool DarkBackground
		{
			get { return this.darkBackground; }
			set { this.darkBackground = value; }
		}

		private TilesetView.TileIndexDrawMode displayTileIndices;
		public TilesetView.TileIndexDrawMode DisplayTileIndices
		{
			get { return this.displayTileIndices; }
			set { this.displayTileIndices = value; }
		}
	}

	public class TilesetEditorSettings
	{
		private bool darkBackground;
		public bool DarkBackground
		{
			get { return this.darkBackground; }
			set { this.darkBackground = value; }
		}

		private TilesetView.TileIndexDrawMode displayTileIndices;
		public TilesetView.TileIndexDrawMode DisplayTileIndices
		{
			get { return this.displayTileIndices; }
			set { this.displayTileIndices = value; }
		}
	}
}
