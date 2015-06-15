using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Duality.Drawing;

namespace Duality.Tests
{
	public static class ExtMethodsPixelData
	{
		public static Bitmap ToBitmap(this PixelData pixelData)
		{
			if (pixelData.Width == 0 || pixelData.Height == 0)
				return new Bitmap(1, 1);

			int[] argbValues = pixelData.GetPixelDataIntArgb();
			Bitmap bm = new Bitmap(pixelData.Width, pixelData.Height);
			BitmapData data = bm.LockBits(
				new Rectangle(0, 0, bm.Width, bm.Height),
				ImageLockMode.WriteOnly,
				PixelFormat.Format32bppArgb);
			
			int pixels = data.Width * data.Height;
			Marshal.Copy(argbValues, 0, data.Scan0, pixels);

			bm.UnlockBits(data);
			return bm;
		}
		public static void FromBitmap(this PixelData pixelData, Bitmap bm)
		{
			// Retrieve data
			BitmapData data = bm.LockBits(
				new Rectangle(0, 0, bm.Width, bm.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);
			
			int pixels = data.Width * data.Height;
			int[] argbValues = new int[pixels];
			Marshal.Copy(data.Scan0, argbValues, 0, pixels);
			bm.UnlockBits(data);
				
			pixelData.SetPixelDataArgb(argbValues, bm.Width, bm.Height);
			pixelData.ColorTransparentPixels();
		}
		public static int[] GetPixelDataIntArgb(this PixelData pixelData)
		{
			ColorRgba[] rawData = pixelData.Data;
			int[] argbValues = new int[rawData.Length];
			unchecked
			{
				for (int i = 0; i < rawData.Length; i++)
					argbValues[i] = rawData[i].ToIntArgb();
			}
			return argbValues;
		}
		public static void SetPixelDataArgb(this PixelData pixelData, int[] data, int width = -1, int height = -1)
		{
			if (width < 0) width = pixelData.Width;
			if (height < 0) height = pixelData.Height;
			if (data.Length != width * height) throw new ArgumentException("Data length doesn't match width * height", "pixelData");

			ColorRgba[] tempData = new ColorRgba[width * height];
			Parallel.ForEach(Partitioner.Create(0, tempData.Length), range =>
			{
				for (int i = range.Item1; i < range.Item2; i++)
				{
					tempData[i].A = (byte)((data[i] & 0xFF000000) >> 24);
					tempData[i].R = (byte)((data[i] & 0x00FF0000) >> 16);
					tempData[i].G = (byte)((data[i] & 0x0000FF00) >> 8);
					tempData[i].B = (byte)((data[i] & 0x000000FF) >> 0);
				}
			});

			pixelData.SetData(tempData, width, height);
		}
	}
}
