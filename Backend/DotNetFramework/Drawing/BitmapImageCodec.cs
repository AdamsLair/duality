using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Duality;
using Duality.Drawing;

namespace Duality.Backend.DotNetFramework
{
	public class BitmapImageCodec : IImageCodec
	{
		public string CodecId
		{
			get { return "System.Drawing.Bitmap Image Codec"; }
		}
		public int Priority
		{
			get { return 0; }
		}

		public bool CanReadFormat(string formatId)
		{
			return this.CanReadWriteFormat(formatId);
		}
		public PixelData Read(Stream stream)
		{
			ColorRgba[] rawColorData;
			int width;
			int height;
			PixelData pixelData = new PixelData();

			using (Bitmap bitmap = Bitmap.FromStream(stream) as Bitmap)
			{
				// Retrieve data
				BitmapData bitmapData = bitmap.LockBits(
					new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					ImageLockMode.ReadOnly,
					PixelFormat.Format32bppArgb);
			
				int pixelCount = bitmapData.Width * bitmapData.Height;
				int[] argbValues = new int[pixelCount];
				Marshal.Copy(bitmapData.Scan0, argbValues, 0, pixelCount);
				bitmap.UnlockBits(bitmapData);
				
				width = bitmapData.Width;
				height = bitmapData.Height;
				rawColorData = new ColorRgba[width * height];
				Parallel.ForEach(Partitioner.Create(0, rawColorData.Length), range =>
				{
					for (int i = range.Item1; i < range.Item2; i++)
					{
						rawColorData[i].A = (byte)((argbValues[i] & 0xFF000000) >> 24);
						rawColorData[i].R = (byte)((argbValues[i] & 0x00FF0000) >> 16);
						rawColorData[i].G = (byte)((argbValues[i] & 0x0000FF00) >> 8);
						rawColorData[i].B = (byte)((argbValues[i] & 0x000000FF) >> 0);
					}
				});
			}

			pixelData.SetData(rawColorData, width, height);
			pixelData.ColorTransparentPixels();

			return pixelData;
		}

		public bool CanWriteFormat(string formatId)
		{
			return this.CanReadWriteFormat(formatId);
		}
		public void Write(Stream stream, PixelData pixelData, string formatId)
		{
			Bitmap bitmap = null;
			try
			{
				if (pixelData.Width == 0 || pixelData.Height == 0)
				{
					bitmap = new Bitmap(1, 1);
				}
				else
				{
					ColorRgba[] rawColorData = pixelData.Data;
					int[] argbValues = new int[rawColorData.Length];
					unchecked
					{
						for (int i = 0; i < rawColorData.Length; i++)
							argbValues[i] = rawColorData[i].ToIntArgb();
					}

					bitmap = new Bitmap(pixelData.Width, pixelData.Height);
					BitmapData data = bitmap.LockBits(
						new Rectangle(0, 0, bitmap.Width, bitmap.Height),
						ImageLockMode.WriteOnly,
						PixelFormat.Format32bppArgb);
			
					int pixels = data.Width * data.Height;
					Marshal.Copy(argbValues, 0, data.Scan0, pixels);

					bitmap.UnlockBits(data);
				}

				ImageFormat bitmapFormat;
				switch (formatId)
				{
					case ImageCodec.FormatBmp:
						bitmapFormat = ImageFormat.Bmp;
						break;
					case ImageCodec.FormatJpeg:
						bitmapFormat = ImageFormat.Jpeg;
						break;
					case ImageCodec.FormatTiff:
						bitmapFormat = ImageFormat.Tiff;
						break;
					default:
					case ImageCodec.FormatPng:
						bitmapFormat = ImageFormat.Png;
						break;
				}
				bitmap.Save(stream, bitmapFormat);
			}
			finally
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
					bitmap = null;
				}
			}
		}

		private bool CanReadWriteFormat(string formatId)
		{
			switch (formatId)
			{
				case ImageCodec.FormatBmp:
				case ImageCodec.FormatJpeg:
				case ImageCodec.FormatTiff:
				case ImageCodec.FormatPng:
					return true;
				default:
					return false;
			}
		}
	}
}
