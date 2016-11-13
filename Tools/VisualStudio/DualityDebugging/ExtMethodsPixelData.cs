using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Duality.Drawing;

namespace Duality.VisualStudio
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
	}
}
