using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Duality.Drawing;

namespace Duality.VisualStudio
{
	/// <summary>
	/// Provides extension methods for <see cref="System.Drawing.Bitmap">Bitmaps</see>.
	/// </summary>
	public static class ExtMethodsBitmap
	{
		/// <summary>
		/// Determines the average color of a Bitmap.
		/// </summary>
		/// <param name="bm"></param>
		/// <param name="weightTransparent">If true, the alpha value weights a pixels color value. </param>
		/// <returns></returns>
		public static ColorRgba GetAverageColor(this Bitmap bm, bool weightTransparent = true)
		{
			float[] sum = new float[4];
			int count = 0;
			ColorRgba[] pixelData = bm.GetPixelDataRgba();

			if (weightTransparent)
			{
				for (int i = 0; i < pixelData.Length; i++)
				{
					sum[0] += pixelData[i].R * ((float)pixelData[i].A / 255.0f);
					sum[1] += pixelData[i].G * ((float)pixelData[i].A / 255.0f);
					sum[2] += pixelData[i].B * ((float)pixelData[i].A / 255.0f);
					sum[3] += (float)pixelData[i].A / 255.0f;
					++count;
				}
				if (sum[3] <= 0.001f) return ColorRgba.TransparentBlack;

				return new ColorRgba(
					(byte)MathF.Clamp((int)(sum[0] / sum[3]), 0, 255),
					(byte)MathF.Clamp((int)(sum[1] / sum[3]), 0, 255),
					(byte)MathF.Clamp((int)(sum[2] / sum[3]), 0, 255),
					(byte)MathF.Clamp((int)(sum[3] / (float)count), 0, 255));
			}
			else
			{
				for (int i = 0; i < pixelData.Length; i++)
				{
					sum[0] += pixelData[i].R;
					sum[1] += pixelData[i].G;
					sum[2] += pixelData[i].B;
					sum[3] += pixelData[i].A;
					++count;
				}
				if (count == 0) return ColorRgba.TransparentBlack;

				return new ColorRgba(
					(byte)MathF.Clamp((int)(sum[0] / (float)count), 0, 255),
					(byte)MathF.Clamp((int)(sum[1] / (float)count), 0, 255),
					(byte)MathF.Clamp((int)(sum[2] / (float)count), 0, 255),
					(byte)MathF.Clamp((int)(sum[3] / (float)count), 0, 255));
			}
		}
		/// <summary>
		/// Extracts a Bitmaps pixel data.
		/// </summary>
		/// <param name="bm"></param>
		/// <returns></returns>
		public static ColorRgba[] GetPixelDataRgba(this Bitmap bm)
		{
			int[] argbValues = GetPixelDataIntArgb(bm);

			// Convert to ColorRGBA
			ColorRgba[] result = new ColorRgba[argbValues.Length];
			unchecked
			{
				for (int i = 0; i < argbValues.Length; i++)
					result[i].SetIntArgb(argbValues[i]);
			}
			return result;
		}
		/// <summary>
		/// Extracts a Bitmaps pixel data as (signed) IntArgb values.
		/// </summary>
		/// <param name="bm"></param>
		public static int[] GetPixelDataIntArgb(this Bitmap bm)
		{
			BitmapData data = bm.LockBits(
				new Rectangle(0, 0, bm.Width, bm.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);
			
			int pixels = data.Width * data.Height;
			int[] argbValues = new int[pixels];
			System.Runtime.InteropServices.Marshal.Copy(data.Scan0, argbValues, 0, pixels);
			bm.UnlockBits(data);

			return argbValues;
		}
	}
}
