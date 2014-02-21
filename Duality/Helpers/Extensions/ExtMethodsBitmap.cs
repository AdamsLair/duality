using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Provides extension methods for <see cref="System.Drawing.Bitmap">Bitmaps</see>.
	/// </summary>
	public static class ExtMethodsBitmap
	{
		/// <summary>
		/// Extracts a rectangular portion of the original image.
		/// </summary>
		/// <param name="bm">The original Bitmap.</param>
		/// <param name="x">The rectangular portion to extract.</param>
		/// <param name="y">The rectangular portion to extract.</param>
		/// <param name="w">The rectangular portion to extract.</param>
		/// <param name="h">The rectangular portion to extract.</param>
		/// <returns>A new Bitmap containing the selected area.</returns>
		public static Bitmap SubImage(this Bitmap bm, int x, int y, int w, int h)
		{
			if (w == 0 || h == 0) return null;
			Bitmap result = new Bitmap(w, h);
			using (Graphics g = Graphics.FromImage(result))
			{
				g.DrawImageUnscaledAndClipped(bm, new Rectangle(-x, -y, bm.Width, bm.Height));
			}
			return result;
		}
		/// <summary>
		/// Extracts a rectangular portion of the original image.
		/// </summary>
		/// <param name="bm">The original Bitmap.</param>
		/// <param name="rect">The rectangular portion to extract.</param>
		/// <returns>A new Bitmap containing the selected area.</returns>
		public static Bitmap SubImage(this Bitmap bm, Rect rect)
		{
			return SubImage(bm,
				MathF.RoundToInt(rect.X),
				MathF.RoundToInt(rect.Y),
				MathF.RoundToInt(rect.W),
				MathF.RoundToInt(rect.H));
		}
		/// <summary>
		/// Extracts a rectangular portion of the original image.
		/// </summary>
		/// <param name="bm">The original Bitmap.</param>
		/// <param name="rect">The rectangular portion to extract.</param>
		/// <returns>A new Bitmap containing the selected area.</returns>
		public static Bitmap SubImage(this Bitmap bm, Rectangle rect)
		{
			return SubImage(bm, rect.X, rect.Y, rect.Width, rect.Height);
		}
		/// <summary>
		/// Creates a resized version of a Bitmap. Gained space will be empty, lost space will crop the image.
		/// </summary>
		/// <param name="bm">The original Bitmap.</param>
		/// <param name="w">The desired width.</param>
		/// <param name="h">The desired height.</param>
		/// <param name="origin">The desired resize origin in the original image.</param>
		/// <returns>A new Bitmap that has the specified size.</returns>
		public static Bitmap Resize(this Bitmap bm, int w, int h, Alignment origin = Alignment.TopLeft)
		{
			int x = 0;
			int y = 0;

			if (origin == Alignment.Right || 
				origin == Alignment.TopRight || 
				origin == Alignment.BottomRight)
				x = w - bm.Width;
			else if (
				origin == Alignment.Center || 
				origin == Alignment.Top || 
				origin == Alignment.Bottom)
				x = (w - bm.Width) / 2;

			if (origin == Alignment.Bottom || 
				origin == Alignment.BottomLeft || 
				origin == Alignment.BottomRight)
				y = h - bm.Height;
			else if (
				origin == Alignment.Center || 
				origin == Alignment.Left || 
				origin == Alignment.Right)
				y = (h - bm.Height) / 2;

			return bm.SubImage(-x, -y, w, h);
		}
		/// <summary>
		/// Creates a rescaled version of a Bitmap.
		/// </summary>
		/// <param name="bm">The original Bitmap.</param>
		/// <param name="w">The desired width.</param>
		/// <param name="h">The desired height.</param>
		/// <param name="mode">Specified how to interpolate the original image in order to calculate the result image.</param>
		/// <returns>A new Bitmap that has been scaled to the specified size.</returns>
		public static Bitmap Rescale(this Bitmap bm, int w, int h, InterpolationMode mode = InterpolationMode.Bilinear)
		{
			Bitmap result = new Bitmap(w, h);
			using (Graphics g = Graphics.FromImage(result))
			{
				g.InterpolationMode = mode;

				ImageAttributes imageAttr = new ImageAttributes();
				imageAttr.SetWrapMode(WrapMode.TileFlipXY);
				g.DrawImage(bm, 
					new Rectangle(0, 0, w, h),
					0, 0, bm.Width, bm.Height,
					GraphicsUnit.Pixel,
					imageAttr);
			}
			return result;
		}
		/// <summary>
		/// Creates a cropped version of the specified Bitmap, removing transparent / empty border areas.
		/// </summary>
		/// <param name="bm">The original Bitmap.</param>
		/// <param name="cropX">Whether the image should be cropped in X-direction</param>
		/// <param name="cropY">Whether the image should be cropped in Y-direction</param>
		/// <returns>A cropped version of the original Bitmap.</returns>
		public static Bitmap Crop(this Bitmap bm, bool cropX = true, bool cropY = true)
		{
			if (!cropX && !cropY) return bm.Clone() as Bitmap;
			Rectangle bounds = bm.OpaqueBounds();
			return bm.SubImage(cropX ? bounds.X : 0, cropY ? bounds.Y : 0, cropX ? bounds.Width : bm.Width, cropY ? bounds.Height : bm.Height);
		}
		/// <summary>
		/// Measures the bounding rectangle of the opaque pixels in a Bitmap.
		/// </summary>
		/// <param name="bm"></param>
		/// <returns></returns>
		public static Rectangle OpaqueBounds(this Bitmap bm)
		{
			ColorRgba[] pixels = bm.GetPixelDataRgba();
			Rectangle bounds = new Rectangle(bm.Width, bm.Height, 0, 0);
			for (int i = 0; i < pixels.Length; i++)
			{
				if (pixels[i].A == 0) continue;
				int x = i % bm.Width;
				int y = i / bm.Width;
				bounds.X = Math.Min(bounds.X, x);
				bounds.Y = Math.Min(bounds.Y, y);
				bounds.Width = Math.Max(bounds.Width, x);
				bounds.Height = Math.Max(bounds.Height, y);
			}
			bounds.Width = 1 + Math.Max(0, bounds.Width - bounds.X);
			bounds.Height = 1 + Math.Max(0, bounds.Height - bounds.Y);

			return bounds;
		}
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

		/// <summary>
		/// Replaces a Bitmaps pixel data.
		/// </summary>
		/// <param name="bm"></param>
		/// <param name="pixelData"></param>
		public static void SetPixelDataRgba(this Bitmap bm, ColorRgba[] pixelData)
		{
			int[] argbValues = new int[pixelData.Length];
			unchecked
			{
				for (int i = 0; i < pixelData.Length; i++)
					argbValues[i] = pixelData[i].ToIntArgb();
			}
			SetPixelDataIntArgb(bm, argbValues);
		}
		/// <summary>
		/// Replaces a Bitmaps pixel data.
		/// </summary>
		/// <param name="bm"></param>
		/// <param name="pixelData"></param>
		public static void SetPixelDataRgba(this Bitmap bm, byte[] pixelData)
		{
			int pixels = (int)MathF.Ceiling(pixelData.Length / 4.0f);
			int[] argbValues = new int[pixels];
			unchecked
			{
				for (int i = 0; i < pixels; i++)
					argbValues[i] = 
						((int)pixelData[i * 4 + 3] << 24) |
						((int)pixelData[i * 4 + 0] << 16) |
						((int)pixelData[i * 4 + 1] << 8) |
						((int)pixelData[i * 4 + 2] << 0);
			}
			SetPixelDataIntArgb(bm, argbValues);
		}
		/// <summary>
		/// Replaces a Bitmaps pixel data.
		/// </summary>
		/// <param name="bm"></param>
		/// <param name="pixelData"></param>
		public static void SetPixelDataIntArgb(this Bitmap bm, int[] pixelData)
		{
			BitmapData data = bm.LockBits(
				new Rectangle(0, 0, bm.Width, bm.Height),
				ImageLockMode.WriteOnly,
				PixelFormat.Format32bppArgb);
			
			int pixels = data.Width * data.Height;
			System.Runtime.InteropServices.Marshal.Copy(pixelData, 0, data.Scan0, pixels);

			bm.UnlockBits(data);
		}
	}
}
