using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

using Duality.Serialization;

namespace Duality.Drawing
{
	/// <summary>
	/// Represents a block of pixel data.
	/// </summary>
	public class PixelData : ISerializeExplicit
	{
		/// <summary>
		/// Represents an unknown <see cref="PixelData"/> version.
		/// </summary>
		private const int Serialize_Version_Unknown		= 0;
		/// <summary>
		/// Represents the PNG-compressed <see cref="PixelData"/> version.
		/// </summary>
		private const int Serialize_Version_LayerPng	= 3;
		/// <summary>
		/// Represents the first v2.x <see cref="PixelData"/> version that requires an explicitly stated format id for image codec support.
		/// </summary>
		private const int Serialize_Version_FormatId	= 4;


		private	int	width;
		private	int height;
		private	ColorRgba[]	data;


		/// <summary>
		/// [GET] The layers width in pixels
		/// </summary>
		public int Width
		{
			get { return this.width; }
		}
		/// <summary>
		/// [GET] The layers height in pixels
		/// </summary>
		public int Height
		{
			get { return this.height; }
		}
		/// <summary>
		/// [GET] The layers pixel data
		/// </summary>
		public ColorRgba[] Data
		{
			get { return this.data; }
		}
		/// <summary>
		/// [GET / SET] A single pixels color.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public ColorRgba this[int x, int y]
		{
			get
			{
				int n = x + y * this.width;
				return this.data[n];
			}
			set
			{
				int n = x + y * this.width;
				this.data[n] = value;
			}
		}

			
		public PixelData() : this(0, 0, ColorRgba.TransparentBlack) {}
		public PixelData(int width, int height) : this(width, height, ColorRgba.TransparentBlack) {}
		public PixelData(int width, int height, ColorRgba backColor)
		{
			if (width < 0) throw new ArgumentException("Width may not be negative.", "width");
			if (height < 0) throw new ArgumentException("Height may not be negative.", "height");

			this.width = width;
			this.height = height;
			this.data = new ColorRgba[width * height];

			for (int i = 0; i < this.data.Length; i++)
				this.data[i] = backColor;
		}
		/// <summary>
		/// Creates a new <see cref="PixelData"/> object using the specified dimensions and data array.
		/// The specified data block will be used directly without copying it first.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="data"></param>
		public PixelData(int width, int height, ColorRgba[] data)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (width < 0) throw new ArgumentException("Width may not be negative.", "width");
			if (height < 0) throw new ArgumentException("Height may not be negative.", "height");

			this.SetData(data, width, height);
		}

		/// <summary>
		/// Clones the pixel data layer and returns the new instance.
		/// </summary>
		/// <returns></returns>
		public PixelData Clone()
		{
			return new PixelData(
				this.width, 
				this.height, 
				this.data.Clone() as ColorRgba[]);
		}
		
		/// <summary>
		/// Replaces the <see cref="PixelData"/>s content with the specified color data.
		/// Ownership of the data block will be assumed - it won't be copied before using it.
		/// </summary>
		/// <param name="pixelData"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void SetData(ColorRgba[] pixelData, int width = -1, int height = -1)
		{
			if (width < 0) width = this.width;
			if (height < 0) height = this.height;
			if (pixelData.Length != width * height) throw new ArgumentException("Data length doesn't match width * height", "pixelData");

			this.width = width;
			this.height = height;
			this.data = pixelData;
		}

		/// <summary>
		/// Rescales the Layer, stretching it to the specified size.
		/// </summary>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="filter">The filtering method to use when rescaling</param>
		public void Rescale(int w, int h, ImageScaleFilter filter = ImageScaleFilter.Linear)
		{
			ColorRgba[] result = this.InternalRescale(w, h, filter);
			if (result == null) return;

			this.data = result;
			this.width = w;
			this.height = h;

			return;
		}
		/// <summary>
		/// Resizes the Layers boundaries.
		/// </summary>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="origin"></param>
		public void Resize(int w, int h, Alignment origin = Alignment.TopLeft)
		{
			int x = 0;
			int y = 0;

			if (origin == Alignment.Right || 
				origin == Alignment.TopRight || 
				origin == Alignment.BottomRight)
				x = w - this.width;
			else if (
				origin == Alignment.Center || 
				origin == Alignment.Top || 
				origin == Alignment.Bottom)
				x = (w - this.width) / 2;

			if (origin == Alignment.Bottom || 
				origin == Alignment.BottomLeft || 
				origin == Alignment.BottomRight)
				y = h - this.height;
			else if (
				origin == Alignment.Center || 
				origin == Alignment.Left || 
				origin == Alignment.Right)
				y = (h - this.height) / 2;

			this.SubImage(-x, -y, w, h);
		}
		/// <summary>
		/// Extracts a rectangular region of this Layer. If the extracted region is bigger than the original Layer,
		/// all new space is filled with a background color.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public void SubImage(int x, int y, int w, int h)
		{
			this.SubImage(x, y, w, h, ColorRgba.TransparentBlack);
		}
		/// <summary>
		/// Extracts a rectangular region of this Layer. If the extracted region is bigger than the original Layer,
		/// all new space is filled with a background color.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="backColor"></param>
		public void SubImage(int x, int y, int w, int h, ColorRgba backColor)
		{
			PixelData tempLayer = new PixelData(w, h, backColor);
			this.DrawOnto(tempLayer, BlendMode.Solid, -x, -y);
			this.width = tempLayer.width;
			this.height = tempLayer.height;
			this.data = tempLayer.data;
		}
		/// <summary>
		/// Crops the Layer, removing transparent / empty border areas.
		/// </summary>
		/// <param name="cropX">Whether the Layer should be cropped in X-direction</param>
		/// <param name="cropY">Whether the Layer should be cropped in Y-direction</param>
		public void Crop(bool cropX = true, bool cropY = true)
		{
			if (!cropX && !cropY) return;
			Point2 topLeft;
			Point2 size;
			this.GetOpaqueBoundaries(out topLeft, out size);
			this.SubImage(
				cropX ? topLeft.X : 0, 
				cropY ? topLeft.Y : 0, 
				cropX ? size.X : this.width, 
				cropY ? size.Y : this.height);
		}

		/// <summary>
		/// Measures the bounding rectangle of the Layers opaque pixels.
		/// </summary>
		/// <param name="topLeft"></param>
		/// <param name="size"></param>
		public void GetOpaqueBoundaries(out Point2 topLeft, out Point2 size)
		{
			topLeft = new Point2(this.width, this.height);
			size = new Point2(0, 0);

			for (int i = 0; i < this.data.Length; i++)
			{
				if (this.data[i].A == 0) continue;
				int cX = i % this.width;
				int cY = i / this.width;
				topLeft.X = Math.Min(topLeft.X, cX);
				topLeft.Y = Math.Min(topLeft.Y, cY);
				size.X = Math.Max(size.X, cX);
				size.Y = Math.Max(size.Y, cY);
			}
			size.X = 1 + Math.Max(0, size.X - topLeft.X);
			size.Y = 1 + Math.Max(0, size.Y - topLeft.Y);
		}
		/// <summary>
		/// Determines the average color of a Layer.
		/// </summary>
		/// <param name="weightTransparent">If true, the alpha value weights a pixels color value. </param>
		/// <returns></returns>
		public ColorRgba GetAverageColor(bool weightTransparent = true)
		{
			float[] sum = new float[4];
			int count = 0;

			if (weightTransparent)
			{
				for (int i = 0; i < this.data.Length; i++)
				{
					sum[0] += this.data[i].R * ((float)this.data[i].A / 255.0f);
					sum[1] += this.data[i].G * ((float)this.data[i].A / 255.0f);
					sum[2] += this.data[i].B * ((float)this.data[i].A / 255.0f);
					sum[3] += (float)this.data[i].A / 255.0f;
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
				for (int i = 0; i < this.data.Length; i++)
				{
					sum[0] += this.data[i].R;
					sum[1] += this.data[i].G;
					sum[2] += this.data[i].B;
					sum[3] += this.data[i].A;
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
		/// Sets the color of all transparent pixels based on the non-transparent color values next to them.
		/// This does not affect any alpha values but prepares the Layer for correct filtering once uploaded
		/// to <see cref="Duality.Resources.Texture"/>.
		/// </summary>
		public void ColorTransparentPixels()
		{
			ColorRgba[] dataCopy = new ColorRgba[this.data.Length];
			Array.Copy(this.data, dataCopy, this.data.Length);

			Parallel.ForEach(Partitioner.Create(0, this.data.Length), range =>
			{
				Point2	pos		= new Point2();
				int[]	nPos	= new int[8];
				bool[]	nOk		= new bool[8];
				int[]	mixClr	= new int[4];

				for (int i = range.Item1; i < range.Item2; i++)
				{
					if (dataCopy[i].A != 0) continue;

					pos.Y	= i / this.width;
					pos.X	= i - (pos.Y * this.width);

					mixClr[0] = 0;
					mixClr[1] = 0;
					mixClr[2] = 0;
					mixClr[3] = 0;

					nPos[0] = i - this.width;
					nPos[1] = i + this.width;
					nPos[2] = i - 1;
					nPos[3] = i + 1;
					nPos[4] = i - this.width - 1;
					nPos[5] = i + this.width - 1;
					nPos[6] = i - this.width + 1;
					nPos[7] = i + this.width + 1;

					nOk[0]	= pos.Y > 0;
					nOk[1]	= pos.Y < this.height - 1;
					nOk[2]	= pos.X > 0;
					nOk[3]	= pos.X < this.width - 1;
					nOk[4]	= nOk[2] && nOk[0];
					nOk[5]	= nOk[2] && nOk[1];
					nOk[6]	= nOk[3] && nOk[0];
					nOk[7]	= nOk[3] && nOk[1];

					int nMult = 2;
					for (int j = 0; j < 8; j++)
					{
						if (!nOk[j]) continue;
						if (dataCopy[nPos[j]].A == 0) continue;

						mixClr[0] += dataCopy[nPos[j]].R * nMult;
						mixClr[1] += dataCopy[nPos[j]].G * nMult;
						mixClr[2] += dataCopy[nPos[j]].B * nMult;
						mixClr[3] += nMult;

						if (j > 3) nMult = 1;
					}

					if (mixClr[3] > 0)
					{
						this.data[i].R = (byte)Math.Round((float)mixClr[0] / (float)mixClr[3]);
						this.data[i].G = (byte)Math.Round((float)mixClr[1] / (float)mixClr[3]);
						this.data[i].B = (byte)Math.Round((float)mixClr[2] / (float)mixClr[3]);
					}
				}
			});
		}
		/// <summary>
		/// Sets the color of all transparent pixels to the specified color.
		/// </summary>
		/// <param name="transparentColor"></param>
		public void ColorTransparentPixels(ColorRgba transparentColor)
		{
			for (int i = 0; i < this.data.Length; i++)
			{
				if (this.data[i].A != 0) continue;
				this.data[i] = transparentColor;
			}
		}
			
		/// <summary>
		/// Rescales the Layer, stretching it to the specified size.
		/// </summary>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="filter">The filtering method to use when rescaling</param>
		public PixelData CloneRescale(int w, int h, ImageScaleFilter filter = ImageScaleFilter.Linear)
		{
			ColorRgba[] result = this.InternalRescale(w, h, filter);
			if (result == null) return this.Clone();

			return new PixelData(w, h, result);
		}
		/// <summary>
		/// Resizes the Layers boundaries.
		/// </summary>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="origin"></param>
		public PixelData CloneResize(int w, int h, Alignment origin = Alignment.TopLeft)
		{
			int x = 0;
			int y = 0;

			if (origin == Alignment.Right || 
				origin == Alignment.TopRight || 
				origin == Alignment.BottomRight)
				x = w - this.width;
			else if (
				origin == Alignment.Center || 
				origin == Alignment.Top || 
				origin == Alignment.Bottom)
				x = (w - this.width) / 2;

			if (origin == Alignment.Bottom || 
				origin == Alignment.BottomLeft || 
				origin == Alignment.BottomRight)
				y = h - this.height;
			else if (
				origin == Alignment.Center || 
				origin == Alignment.Left || 
				origin == Alignment.Right)
				y = (h - this.height) / 2;

			return this.CloneSubImage(-x, -y, w, h);
		}
		/// <summary>
		/// Extracts a rectangular region of this Layer. If the extracted region is bigger than the original Layer,
		/// all new space is filled with a background color.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public PixelData CloneSubImage(int x, int y, int w, int h)
		{
			return this.CloneSubImage(x, y, w, h, ColorRgba.TransparentBlack);
		}
		/// <summary>
		/// Extracts a rectangular region of this Layer. If the extracted region is bigger than the original Layer,
		/// all new space is filled with a background color.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="backColor"></param>
		public PixelData CloneSubImage(int x, int y, int w, int h, ColorRgba backColor)
		{
			PixelData tempLayer = new PixelData(w, h, backColor);
			this.DrawOnto(tempLayer, BlendMode.Solid, -x, -y);
			return tempLayer;
		}
		/// <summary>
		/// Crops the Layer, removing transparent / empty border areas.
		/// </summary>
		/// <param name="cropX">Whether the Layer should be cropped in X-direction</param>
		/// <param name="cropY">Whether the Layer should be cropped in Y-direction</param>
		public PixelData CloneCrop(bool cropX = true, bool cropY = true)
		{
			if (!cropX && !cropY) return this.Clone();
			Point2 topLeft;
			Point2 size;
			this.GetOpaqueBoundaries(out topLeft, out size);
			return this.CloneSubImage(
				cropX ? topLeft.X : 0, 
				cropY ? topLeft.Y : 0, 
				cropX ? size.X : this.width, 
				cropY ? size.Y : this.height);
		}
			
		/// <summary>
		/// Performs a drawing operation from this Layer to a target layer.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="blend"></param>
		/// <param name="destX"></param>
		/// <param name="destY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="srcX"></param>
		/// <param name="srcY"></param>
		public void DrawOnto(PixelData target, BlendMode blend, int destX, int destY, int width = -1, int height = -1, int srcX = 0, int srcY = 0)
		{
			if (width == -1) width = this.width;
			if (height == -1) height = this.height;

			int beginX = MathF.Max(0, -destX, -srcX);
			int beginY = MathF.Max(0, -destY, -srcY);
			int endX = MathF.Min(width, this.width, target.width - destX, this.width - srcX);
			int endY = MathF.Min(height, this.height, target.height - destY, this.height - srcY);
			if (endX - beginX < 1) return;
			if (endY - beginY < 1) return;

			Parallel.ForEach(Partitioner.Create(beginX, endX), range =>
			{
				for (int i = range.Item1; i < range.Item2; i++)
				{
					for (int j = beginY; j < endY; j++)
					{
						int sourceN = srcX + i + this.width * (srcY + j);
						int targetN = destX + i + target.width * (destY + j);

						if (blend == BlendMode.Solid)
						{
							target.data[targetN] = this.data[sourceN];
						}
						else if (blend == BlendMode.Mask)
						{
							if (this.data[sourceN].A >= 0) target.data[targetN] = this.data[sourceN];
						}
						else if (blend == BlendMode.Add)
						{
							ColorRgba targetColor	= target.data[targetN];
							float alphaTemp = (float)this.data[sourceN].A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.R + this.data[sourceN].R * alphaTemp)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.G + this.data[sourceN].G * alphaTemp)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.B + this.data[sourceN].B * alphaTemp)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)targetColor.A + (int)this.data[sourceN].A));
						}
						else if (blend == BlendMode.Alpha)
						{
							ColorRgba targetColor	= target.data[targetN];
							float alphaTemp = (float)this.data[sourceN].A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.R * (1.0f - alphaTemp) + this.data[sourceN].R * alphaTemp)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.G * (1.0f - alphaTemp) + this.data[sourceN].G * alphaTemp)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.B * (1.0f - alphaTemp) + this.data[sourceN].B * alphaTemp)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.A * (1.0f - alphaTemp) + this.data[sourceN].A)));
						}
						else if (blend == BlendMode.AlphaPre)
						{
							ColorRgba targetColor	= target.data[targetN];
							float alphaTemp = (float)this.data[sourceN].A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.R * (1.0f - alphaTemp) + this.data[sourceN].R)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.G * (1.0f - alphaTemp) + this.data[sourceN].G)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.B * (1.0f - alphaTemp) + this.data[sourceN].B)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(targetColor.A * (1.0f - alphaTemp) + this.data[sourceN].A)));
						}
						else if (blend == BlendMode.Multiply)
						{
							ColorRgba targetColor	= target.data[targetN];
							float clrTempR = (float)targetColor.R / 255.0f;
							float clrTempG = (float)targetColor.G / 255.0f;
							float clrTempB = (float)targetColor.B / 255.0f;
							float clrTempA = (float)targetColor.A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].R * clrTempR)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].G * clrTempG)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].B * clrTempB)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)targetColor.A + (int)this.data[sourceN].A));
						}
						else if (blend == BlendMode.Light)
						{
							ColorRgba targetColor	= target.data[targetN];
							float clrTempR = (float)targetColor.R / 255.0f;
							float clrTempG = (float)targetColor.G / 255.0f;
							float clrTempB = (float)targetColor.B / 255.0f;
							float clrTempA = (float)targetColor.A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].R * clrTempR + targetColor.R)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].G * clrTempG + targetColor.G)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].B * clrTempB + targetColor.B)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)targetColor.A + (int)this.data[sourceN].A));
						}
						else if (blend == BlendMode.Invert)
						{
							ColorRgba targetColor	= target.data[targetN];
							float clrTempR = (float)targetColor.R / 255.0f;
							float clrTempG = (float)targetColor.G / 255.0f;
							float clrTempB = (float)targetColor.B / 255.0f;
							float clrTempA = (float)targetColor.A / 255.0f;
							float clrTempR2 = (float)this.data[sourceN].R / 255.0f;
							float clrTempG2 = (float)this.data[sourceN].G / 255.0f;
							float clrTempB2 = (float)this.data[sourceN].B / 255.0f;
							float clrTempA2 = (float)this.data[sourceN].A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].R * (1.0f - clrTempR) + targetColor.R * (1.0f - clrTempR2))));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].G * (1.0f - clrTempG) + targetColor.G * (1.0f - clrTempG2))));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(this.data[sourceN].B * (1.0f - clrTempB) + targetColor.B * (1.0f - clrTempB2))));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)(targetColor.A + this.data[sourceN].A)));
						}
					}
				}
			});
		}
		/// <summary>
		/// Performs a drawing operation from this Layer to a target layer.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="blend"></param>
		/// <param name="destX"></param>
		/// <param name="destY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="srcX"></param>
		/// <param name="srcY"></param>
		/// <param name="colorTint"></param>
		public void DrawOnto(PixelData target, BlendMode blend, int destX, int destY, int width, int height, int srcX, int srcY, ColorRgba colorTint)
		{
			if (colorTint == ColorRgba.White)
			{
				this.DrawOnto(target, blend, destX, destY, width, height, srcX, srcY);
				return;
			}
			if (width == -1) width = this.width;
			if (height == -1) height = this.height;

			int beginX = MathF.Max(0, -destX, -srcX);
			int beginY = MathF.Max(0, -destY, -srcY);
			int endX = MathF.Min(width, this.width, target.width - destX, this.width - srcX);
			int endY = MathF.Min(height, this.height, target.height - destY, this.height - srcY);
			if (endX - beginX < 1) return;
			if (endY - beginY < 1) return;

			ColorRgba clrSource;
			ColorRgba clrTarget;
			Parallel.ForEach(Partitioner.Create(beginX, endX), range =>
			{
				for (int i = range.Item1; i < range.Item2; i++)
				{
					for (int j = beginY; j < endY; j++)
					{
						int sourceN = srcX + i + this.width * (srcY + j);
						int targetN = destX + i + target.width * (destY + j);

						clrSource = this.data[sourceN] * colorTint;

						if (blend == BlendMode.Solid)
						{
							target.data[targetN] = clrSource;
						}
						else if (blend == BlendMode.Mask)
						{
							if (clrSource.A >= 0) target.data[targetN] = this.data[sourceN];
						}
						else if (blend == BlendMode.Add)
						{
							clrTarget	= target.data[targetN];
							float alphaTemp = (float)clrSource.A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.R + clrSource.R * alphaTemp)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.G + clrSource.G * alphaTemp)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.B + clrSource.B * alphaTemp)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)clrTarget.A + (int)clrSource.A));
						}
						else if (blend == BlendMode.Alpha)
						{
							clrTarget	= target.data[targetN];
							float alphaTemp = (float)clrSource.A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.R * (1.0f - alphaTemp) + clrSource.R * alphaTemp)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.G * (1.0f - alphaTemp) + clrSource.G * alphaTemp)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.B * (1.0f - alphaTemp) + clrSource.B * alphaTemp)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.A * (1.0f - alphaTemp) + clrSource.A)));
						}
						else if (blend == BlendMode.AlphaPre)
						{
							clrTarget	= target.data[targetN];
							float alphaTemp = (float)clrSource.A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.R * (1.0f - alphaTemp) + clrSource.R)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.G * (1.0f - alphaTemp) + clrSource.G)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.B * (1.0f - alphaTemp) + clrSource.B)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrTarget.A * (1.0f - alphaTemp) + clrSource.A)));
						}
						else if (blend == BlendMode.Multiply)
						{
							clrTarget	= target.data[targetN];
							float clrTempR = (float)clrTarget.R / 255.0f;
							float clrTempG = (float)clrTarget.G / 255.0f;
							float clrTempB = (float)clrTarget.B / 255.0f;
							float clrTempA = (float)clrTarget.A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.R * clrTempR)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.G * clrTempG)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.B * clrTempB)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)clrTarget.A + (int)clrSource.A));
						}
						else if (blend == BlendMode.Light)
						{
							clrTarget	= target.data[targetN];
							float clrTempR = (float)clrTarget.R / 255.0f;
							float clrTempG = (float)clrTarget.G / 255.0f;
							float clrTempB = (float)clrTarget.B / 255.0f;
							float clrTempA = (float)clrTarget.A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.R * clrTempR + clrTarget.R)));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.G * clrTempG + clrTarget.G)));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.B * clrTempB + clrTarget.B)));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)clrTarget.A + (int)clrSource.A));
						}
						else if (blend == BlendMode.Invert)
						{
							clrTarget	= target.data[targetN];
							float clrTempR = (float)clrTarget.R / 255.0f;
							float clrTempG = (float)clrTarget.G / 255.0f;
							float clrTempB = (float)clrTarget.B / 255.0f;
							float clrTempA = (float)clrTarget.A / 255.0f;
							float clrTempR2 = (float)clrSource.R / 255.0f;
							float clrTempG2 = (float)clrSource.G / 255.0f;
							float clrTempB2 = (float)clrSource.B / 255.0f;
							float clrTempA2 = (float)clrSource.A / 255.0f;
							target.data[targetN].R = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.R * (1.0f - clrTempR) + clrTarget.R * (1.0f - clrTempR2))));
							target.data[targetN].G = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.G * (1.0f - clrTempG) + clrTarget.G * (1.0f - clrTempG2))));
							target.data[targetN].B = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(clrSource.B * (1.0f - clrTempB) + clrTarget.B * (1.0f - clrTempB2))));
							target.data[targetN].A = (byte)Math.Min(255, Math.Max(0, (int)(clrTarget.A + clrSource.A)));
						}
					}
				}
			});
		}

		private ColorRgba[] InternalRescale(int w, int h, ImageScaleFilter filter)
		{
			if (this.width == w && this.height == h) return null;
				
			ColorRgba[]	tempDestData = new ColorRgba[w * h];
			if (filter == ImageScaleFilter.Nearest)
			{
				// Don't use Parallel.For here, the overhead is too big and the compiler 
				// does a great job optimizing this piece of code without, so don't get in the way.
				for (int i = 0; i < tempDestData.Length; i++)
				{
					int y = i / w;
					int x = i - (y * w);

					int xTmp	= (x * this.width) / w;
					int yTmp	= (y * this.height) / h;
					int nTmp	= xTmp + (yTmp * this.width);
					tempDestData[i] = this.data[nTmp];
				}
			}
			else if (filter == ImageScaleFilter.Linear)
			{
				Parallel.ForEach(Partitioner.Create(0, tempDestData.Length), range =>
				{
					for (int i = range.Item1; i < range.Item2; i++)
					{
						int y = i / w;
						int x = i - (y * w);

						float	xRatio	= ((float)(x * this.width) / (float)w) + 0.5f;
						float	yRatio	= ((float)(y * this.height) / (float)h) + 0.5f;
						int		xTmp	= (int)xRatio;
						int		yTmp	= (int)yRatio;
						xRatio -= xTmp;
						yRatio -= yTmp;

						int		xTmp2	= xTmp + 1;
						int		yTmp2	= yTmp + 1;
						xTmp = xTmp < this.width ? xTmp : this.width - 1;
						yTmp = (yTmp < this.height ? yTmp : this.height - 1) * this.width;
						xTmp2 = xTmp2 < this.width ? xTmp2 : this.width - 1;
						yTmp2 = (yTmp2 < this.height ? yTmp2 : this.height - 1) * this.width;

						int		nTmp0	= xTmp + yTmp;
						int		nTmp1	= xTmp2 + yTmp;
						int		nTmp2	= xTmp + yTmp2;
						int		nTmp3	= xTmp2 + yTmp2;

						tempDestData[i].R = 
							(byte)
							(
								((float)this.data[nTmp0].R * (1.0f - xRatio) * (1.0f - yRatio)) +
								((float)this.data[nTmp1].R * xRatio * (1.0f - yRatio)) + 
								((float)this.data[nTmp2].R * yRatio * (1.0f - xRatio)) +
								((float)this.data[nTmp3].R * xRatio * yRatio)
							);
						tempDestData[i].G = 
							(byte)
							(
								((float)this.data[nTmp0].G * (1.0f - xRatio) * (1.0f - yRatio)) +
								((float)this.data[nTmp1].G * xRatio * (1.0f - yRatio)) + 
								((float)this.data[nTmp2].G * yRatio * (1.0f - xRatio)) +
								((float)this.data[nTmp3].G * xRatio * yRatio)
							);
						tempDestData[i].B = 
							(byte)
							(
								((float)this.data[nTmp0].B * (1.0f - xRatio) * (1.0f - yRatio)) +
								((float)this.data[nTmp1].B * xRatio * (1.0f - yRatio)) + 
								((float)this.data[nTmp2].B * yRatio * (1.0f - xRatio)) +
								((float)this.data[nTmp3].B * xRatio * yRatio)
							);
						tempDestData[i].A = 
							(byte)
							(
								((float)this.data[nTmp0].A * (1.0f - xRatio) * (1.0f - yRatio)) +
								((float)this.data[nTmp1].A * xRatio * (1.0f - yRatio)) + 
								((float)this.data[nTmp2].A * yRatio * (1.0f - xRatio)) +
								((float)this.data[nTmp3].A * xRatio * yRatio)
							);
					}
				});
			}
				
			return tempDestData;
		}

		void ISerializeExplicit.WriteData(IDataWriter writer)
		{
			string formatId = ImageCodec.FormatPng;

			writer.WriteValue("version", Serialize_Version_FormatId);
			writer.WriteValue("formatId", formatId);
			
			IImageCodec codec = ImageCodec.GetWrite(formatId);
			if (codec == null)
			{
				throw new NotSupportedException(string.Format(
					"Unable to retrieve image codec for format '{0}'. Can't save image data.",
					formatId));
			}

			using (MemoryStream str = new MemoryStream(1024 * 64))
			{
				codec.Write(str, this, formatId);
				writer.WriteValue("pixelData", str.ToArray());
			}
		}
		void ISerializeExplicit.ReadData(IDataReader reader)
		{
			int version;
			try { reader.ReadValue("version", out version); }
			catch (Exception) { version = Serialize_Version_Unknown; }

			string formatId;
			if (version == Serialize_Version_FormatId)
			{
				reader.ReadValue("formatId", out formatId);
			}
			else if (version == Serialize_Version_LayerPng)
			{
				formatId = ImageCodec.FormatPng;
			}
			else
			{
				throw new NotSupportedException(string.Format(
					"Unknown PixelData serialization version '{0}'. Can't load image data.",
					version));
			}
			
			IImageCodec codec = ImageCodec.GetRead(formatId);
			if (codec == null)
			{
				throw new NotSupportedException(string.Format(
					"Unable to retrieve image codec for format '{0}'. Can't load image data.",
					formatId));
			}

			byte[] dataBlock;
			reader.ReadValue("pixelData", out dataBlock);
			using (MemoryStream stream = new MemoryStream(dataBlock))
			{
				PixelData pixelData = codec.Read(stream);
				this.data = pixelData.data;
				this.width = pixelData.width;
				this.height = pixelData.height;
				pixelData = null;
			}
		}
	}

}
