using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

using Duality.Drawing;
using Duality.Serialization;

namespace Duality.Resources
{
	/// <summary>
	/// Represents a block of bitmap <see cref="Font"/> data.
	/// </summary>
	public class FontData : ISerializeExplicit
	{
		/// <summary>
		/// Represents an unknown kind of <see cref="Font"/> serialization.
		/// </summary>
		private const int Serialize_Version_Unknown = 0;
		/// <summary>
		/// Represents the first custom <see cref="Font"/> serialization format that
		/// compresses repetitive data.
		/// </summary>
		private const int Serialize_Version_Compression = 1;


		private FontGlyphData[]   glyphs       = null;
		private FontKerningPair[] kerningPairs = null;
		private Rect[]            atlas        = null;
		private PixelData         bitmap       = null;
		private FontMetrics       metrics      = null;


		/// <summary>
		/// [GET] Data about the glyphs that are supported by this bitmap <see cref="Font"/>, including the metrics 
		/// that are required to write a text using them.
		/// </summary>
		public FontGlyphData[] Glyphs
		{
			get { return this.glyphs; }
		}
		/// <summary>
		/// [GET] An optional array of kerning pairs that represent deviations from the default spacing for certain
		/// pairs of glyphs when occurring next to each other.
		/// </summary>
		public FontKerningPair[] KerningPairs
		{
			get { return this.kerningPairs; }
		}
		/// <summary>
		/// [GET] A block of pixel data that contains the visual representation for all supported glyphs.
		/// </summary>
		public PixelData Bitmap
		{
			get { return this.bitmap; }
		}
		/// <summary>
		/// [GET] An atlas that allows to address each glyph's visual representation inside the <see cref="Bitmap"/> of this font.
		/// </summary>
		public Rect[] Atlas
		{
			get { return this.atlas; }
		}
		/// <summary>
		/// [GET] Provides access to various metrics that are inherent to the represented font, such as size, height and other
		/// typographic values.
		/// </summary>
		public FontMetrics Metrics
		{
			get { return this.metrics; }
		}


		public FontData(PixelData bitmap, Rect[] atlas, FontGlyphData[] glyphs, FontMetrics metrics, FontKerningPair[] kerningPairs)
		{
			this.bitmap = bitmap;
			this.atlas = atlas;
			this.glyphs = glyphs;
			this.metrics = metrics;
			this.kerningPairs = kerningPairs;
		}

		void ISerializeExplicit.WriteData(IDataWriter writer)
		{
			writer.WriteValue("version", Serialize_Version_Compression);

			// Write default-serialized data
			writer.WriteValue("bitmap" , this.bitmap );
			writer.WriteValue("metrics", this.metrics);

			// Write compressed data
			writer.WriteValue("atlasData", this.CompressData(this.atlas, (value, binary) => 
			{
				binary.Write((int)value.Length);
				for (int i = 0; i < this.atlas.Length; i++)
				{
					binary.Write((float)value[i].X);
					binary.Write((float)value[i].Y);
					binary.Write((float)value[i].W);
					binary.Write((float)value[i].H);
				}
			}));
			writer.WriteValue("glyphData", this.CompressData(this.glyphs, (value, binary) => 
			{
				binary.Write((int)value.Length);
				for (int i = 0; i < this.glyphs.Length; i++)
				{
					binary.Write((char)value[i].Glyph);
					binary.Write((float)value[i].Size.X);
					binary.Write((float)value[i].Size.Y);
					binary.Write((float)value[i].Offset.X);
					binary.Write((float)value[i].Offset.Y);
					binary.Write((float)value[i].Advance);
				}
			}));
			writer.WriteValue("kerningData", this.CompressData(this.kerningPairs, (value, binary) => 
			{
				binary.Write((int)value.Length);
				for (int i = 0; i < this.kerningPairs.Length; i++)
				{
					binary.Write((char)value[i].FirstChar);
					binary.Write((char)value[i].SecondChar);
					binary.Write((float)value[i].AdvanceOffset);
				}
			}));
		}
		void ISerializeExplicit.ReadData(IDataReader reader)
		{
			int version;
			try { reader.ReadValue("version", out version); }
			catch (Exception) { version = Serialize_Version_Unknown; }
			
			// Read default-serialized data
			reader.ReadValue("bitmap" , out this.bitmap );
			reader.ReadValue("metrics", out this.metrics);
			
			// Read compressed data
			this.DecompressData(reader.ReadValue<byte[]>("atlasData"), out this.atlas, binary =>
			{
				Rect[] value = new Rect[binary.ReadInt32()];
				for (int i = 0; i < value.Length; i++)
				{
					value[i].X = binary.ReadSingle();
					value[i].Y = binary.ReadSingle();
					value[i].W = binary.ReadSingle();
					value[i].H = binary.ReadSingle();
				}
				return value;
			});
			this.DecompressData(reader.ReadValue<byte[]>("glyphData"), out this.glyphs, binary =>
			{
				FontGlyphData[] value = new FontGlyphData[binary.ReadInt32()];
				for (int i = 0; i < value.Length; i++)
				{
					value[i].Glyph = binary.ReadChar();
					value[i].Size.X = binary.ReadSingle();
					value[i].Size.Y = binary.ReadSingle();
					value[i].Offset.X = binary.ReadSingle();
					value[i].Offset.Y = binary.ReadSingle();
					value[i].Advance = binary.ReadSingle();
				}
				return value;
			});
			this.DecompressData(reader.ReadValue<byte[]>("kerningData"), out this.kerningPairs, binary =>
			{
				FontKerningPair[] value = new FontKerningPair[binary.ReadInt32()];
				for (int i = 0; i < value.Length; i++)
				{
					value[i].FirstChar = binary.ReadChar();
					value[i].SecondChar = binary.ReadChar();
					value[i].AdvanceOffset = binary.ReadSingle();
				}
				return value;
			});
		}
		
		private byte[] CompressData<T>(T value, Action<T,BinaryWriter> writeFunc) where T : class
		{
			if (value == null)
				return null;

			using (MemoryStream stream = new MemoryStream())
			{
				using (DeflateStream compressedStream = new DeflateStream(stream, CompressionLevel.Optimal, true))
				using (BinaryWriter compressedWriter = new BinaryWriter(compressedStream, Encoding.UTF8, true))
				{
					writeFunc(value, compressedWriter);
				}
				byte[] data = stream.ToArray();
				return data;
			}
		}
		private void DecompressData<T>(byte[] data, out T value, Func<BinaryReader,T> readFunc) where T : class
		{
			if (data == null)
			{
				value = null;
				return;
			}

			using (MemoryStream stream = new MemoryStream(data))
			using (DeflateStream decompressedStream = new DeflateStream(stream, CompressionMode.Decompress))
			using (BinaryReader decompressedReader = new BinaryReader(decompressedStream, Encoding.UTF8))
			{
				value = readFunc(decompressedReader);
			}
		}
	}
}
