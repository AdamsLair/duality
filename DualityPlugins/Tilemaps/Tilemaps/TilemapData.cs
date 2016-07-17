using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;

using Duality.Serialization;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Represents a block of <see cref="Tilemap"/> data.
	/// </summary>
	public class TilemapData : ISerializeExplicit
	{
		/// <summary>
		/// Represents an unknown <see cref="TilemapData"/> version.
		/// </summary>
		private const int Serialize_Version_Unknown     = 0;
		/// <summary>
		/// Represents the zip-compressed byte array <see cref="TilemapData"/> version.
		/// </summary>
		private const int Serialize_Version_ZippedBytes = 1;
		/// <summary>
		/// Represents the first <see cref="TilemapData"/> version that includes depth offsets.
		/// </summary>
		private const int Serialize_Version_DepthOffset = 2;


		private Grid<Tile> tiles = new Grid<Tile>();


		/// <summary>
		/// [GET] The tile data that is stored within this instance.
		/// </summary>
		public Grid<Tile> Tiles
		{
			get { return this.tiles; }
		}
		/// <summary>
		/// [GET] The number of tiles on each axis.
		/// </summary>
		public Point2 Size
		{
			get { return new Point2(this.tiles.Width, this.tiles.Height); }
		}


		void ISerializeExplicit.WriteData(IDataWriter writer)
		{
			// Write all Tile data to a compressed memory stream
			byte[] compressedData;
			using (MemoryStream compressedStream = new MemoryStream(1024 * 16))
			{
				using (GZipStream stream = new GZipStream(compressedStream, CompressionLevel.Optimal))
				using (BinaryWriter streamWriter = new BinaryWriter(stream))
				{
					streamWriter.Write((int)this.tiles.Width);
					streamWriter.Write((int)this.tiles.Height);

					Tile[] rawData = this.tiles.RawData;
					for (int i = 0; i < this.tiles.Capacity; i++)
					{
						streamWriter.Write((int)rawData[i].Index);
						streamWriter.Write((short)rawData[i].DepthOffset);
					}
				}
				compressedData = compressedStream.ToArray();
			}

			// Transfer the compressed data to the serializer to handle it
			writer.WriteValue("version", Serialize_Version_DepthOffset);
			writer.WriteValue("data", compressedData);
		}
		void ISerializeExplicit.ReadData(IDataReader reader)
		{
			// Determine which Tile data version we're dealing with
			int version;
			try { reader.ReadValue("version", out version); }
			catch (Exception) { version = Serialize_Version_Unknown; }
			
			// Read the compressed data and unpack it
			if (version >= Serialize_Version_ZippedBytes && 
				version <= Serialize_Version_DepthOffset)
			{
				byte[] compressedData;
				reader.ReadValue("data", out compressedData);

				using (MemoryStream compressedStream = new MemoryStream(compressedData))
				using (GZipStream stream = new GZipStream(compressedStream, CompressionMode.Decompress))
				using (BinaryReader streamReader = new BinaryReader(stream))
				{
					int width = streamReader.ReadInt32();
					int height = streamReader.ReadInt32();

					Tile[] rawData = new Tile[width * height];
					switch (version)
					{
						case Serialize_Version_ZippedBytes: this.ReadBinVersionFirst(streamReader, rawData); break;
						case Serialize_Version_DepthOffset: this.ReadBinVersionDepth(streamReader, rawData); break;
					}

					this.tiles = new Grid<Tile>(width, height, rawData);
				}
			}
			else
			{
				throw new NotSupportedException(string.Format(
					"Unknown TilemapData serialization version '{0}'. Can't load tilemap data.",
					version));
			}
		}

		private void ReadBinVersionFirst(BinaryReader reader, Tile[] target)
		{
			for (int i = 0; i < target.Length; i++)
			{
				target[i].Index = reader.ReadInt32();
			}
		}
		private void ReadBinVersionDepth(BinaryReader reader, Tile[] target)
		{
			for (int i = 0; i < target.Length; i++)
			{
				target[i].Index = reader.ReadInt32();
				target[i].DepthOffset = reader.ReadInt16();
			}
		}
	}
}
