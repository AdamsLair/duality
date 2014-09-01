using System;
using System.Drawing;

namespace Duality.Serialization.Surrogates
{
	/// <summary>
	/// De/Serializes a <see cref="System.Drawing.Bitmap"/>.
	/// </summary>
	public class BitmapSurrogate : SerializeSurrogate<Bitmap>
	{
		public override void WriteConstructorData(IDataWriter writer)
		{
			writer.WriteValue("width", this.RealObject.Width);
			writer.WriteValue("height", this.RealObject.Height);
		}
		public override void WriteData(IDataWriter writer)
		{
			int[] data = this.RealObject.GetPixelDataIntArgb();

			writer.WriteValue("data", data);
		}

		public override object ConstructObject(IDataReader reader, Type objType)
		{
			int width = reader.ReadValue<int>("width");
			int height = reader.ReadValue<int>("height");

			return new Bitmap(width, height);
		}
		public override void ReadData(IDataReader reader)
		{
			int[] data = reader.ReadValue<int[]>("data");

			this.RealObject.SetPixelDataIntArgb(data);
		}
	}
}
