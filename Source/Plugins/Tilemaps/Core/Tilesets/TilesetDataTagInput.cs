namespace Duality.Plugins.Tilemaps
{
	public class TilesetDataTagInput
	{
		public string Key { get; set; }

		private RawList<DataTagTileItem> tileData = new RawList<DataTagTileItem>();
		public RawList<DataTagTileItem> TileData { get { return this.tileData; } }
	}

	public class DataTagTileItem
	{
		public object Value { get; set; }
	}
}
