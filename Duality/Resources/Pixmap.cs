using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

using Duality.ColorFormat;
using Duality.EditorHints;
using Duality.Serialization;

using OpenTK;

namespace Duality.Resources
{
	/// <summary>
	/// A Pixmap stores pixel data in system memory. 
	/// </summary>
	/// <seealso cref="Duality.Resources.Texture"/>
	[Serializable]
	[ExplicitResourceReference()]
	public class Pixmap : Resource
	{
		/// <summary>
		/// A Pixmap resources file extension.
		/// </summary>
		public new const string FileExt = ".Pixmap" + Resource.FileExt;

		/// <summary>
		/// Represents an unknown Pixmap version.
		/// </summary>
		protected const int ResFormat_Version_Unknown	= 0;
		/// <summary>
		/// Represents the old, uncompressed Pixmap version using a <see cref="System.Drawing.Bitmap"/>.
		/// </summary>
		protected const int ResFormat_Version_Bitmap	= 1;
		/// <summary>
		/// Represents the PNG-compressed Pixmap version.
		/// </summary>
		protected const int ResFormat_Version_Png		= 2;
		/// <summary>
		/// Represents the PNG-compressed layered Pixmap version.
		/// </summary>
		protected const int ResFormat_Version_LayerPng	= 3;
		
		/// <summary>
		/// [GET] A Pixmap showing the Duality icon.
		/// </summary>
		public static ContentRef<Pixmap> DualityIcon		{ get; private set; }
		/// <summary>
		/// [GET] A Pixmap showing the Duality icon without the text on it.
		/// </summary>
		public static ContentRef<Pixmap> DualityIconB		{ get; private set; }
		/// <summary>
		/// A Pixmap showing the Duality logo.
		/// </summary>
		public static ContentRef<Pixmap> DualityLogoBig		{ get; private set; }
		/// <summary>
		/// A Pixmap showing the Duality logo.
		/// </summary>
		public static ContentRef<Pixmap> DualityLogoMedium	{ get; private set; }
		/// <summary>
		/// A Pixmap showing the Duality logo.
		/// </summary>
		public static ContentRef<Pixmap> DualityLogoSmall	{ get; private set; }
		/// <summary>
		/// [GET] A plain white 1x1 Pixmap. Can be used as a dummy.
		/// </summary>
		public static ContentRef<Pixmap> White				{ get; private set; }
		/// <summary>
		/// [GET] A 256x256 black and white checkerboard image.
		/// </summary>
		public static ContentRef<Pixmap> Checkerboard		{ get; private set; }

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath				= ContentProvider.VirtualContentPath + "Pixmap:";
			const string ContentPath_DualityIcon		= VirtualContentPath + "DualityIcon";
			const string ContentPath_DualityIconB		= VirtualContentPath + "DualityIconB";
			const string ContentPath_DualityLogoBig		= VirtualContentPath + "DualityLogoBig";
			const string ContentPath_DualityLogoMedium	= VirtualContentPath + "DualityLogoMedium";
			const string ContentPath_DualityLogoSmall	= VirtualContentPath + "DualityLogoSmall";
			const string ContentPath_White				= VirtualContentPath + "White";
			const string ContentPath_Checkerboard		= VirtualContentPath + "Checkerboard";

			ContentProvider.RegisterContent(ContentPath_DualityIcon,		new Pixmap(DefaultRes.DualityIcon));
			ContentProvider.RegisterContent(ContentPath_DualityIconB,		new Pixmap(DefaultRes.DualityIconB));
			ContentProvider.RegisterContent(ContentPath_DualityLogoBig,		new Pixmap(DefaultRes.DualityLogoBig));
			ContentProvider.RegisterContent(ContentPath_DualityLogoMedium,	new Pixmap(DefaultRes.DualityLogoMedium));
			ContentProvider.RegisterContent(ContentPath_DualityLogoSmall,	new Pixmap(DefaultRes.DualityLogoSmall));
			ContentProvider.RegisterContent(ContentPath_White,				new Pixmap(new Layer(1, 1, ColorRgba.White)));
			ContentProvider.RegisterContent(ContentPath_Checkerboard,		new Pixmap(DefaultRes.Checkerboard));

			DualityIcon			= ContentProvider.RequestContent<Pixmap>(ContentPath_DualityIcon);
			DualityIconB		= ContentProvider.RequestContent<Pixmap>(ContentPath_DualityIconB);
			DualityLogoBig		= ContentProvider.RequestContent<Pixmap>(ContentPath_DualityLogoBig);
			DualityLogoMedium	= ContentProvider.RequestContent<Pixmap>(ContentPath_DualityLogoMedium);
			DualityLogoSmall	= ContentProvider.RequestContent<Pixmap>(ContentPath_DualityLogoSmall);
			White				= ContentProvider.RequestContent<Pixmap>(ContentPath_White);
			Checkerboard		= ContentProvider.RequestContent<Pixmap>(ContentPath_Checkerboard);
		}

		
		/// <summary>
		/// Represents a filtering method.
		/// </summary>
		public enum FilterMethod
		{
			/// <summary>
			/// Nearest neighbor filterting. (No interpolation)
			/// </summary>
			Nearest,
			/// <summary>
			/// Linear interpolation.
			/// </summary>
			Linear
		}
		/// <summary>
		/// Represents a pixel data layer.
		/// </summary>
		public class Layer : Duality.Cloning.ICloneable, ISerializable
		{
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

			
			public Layer() : this(0, 0, ColorRgba.TransparentBlack) {}
			public Layer(int width, int height) : this(width, height, ColorRgba.TransparentBlack) {}
			public Layer(int width, int height, ColorRgba backColor)
			{
				if (width < 0) throw new ArgumentException("Width may not be negative.", "width");
				if (height < 0) throw new ArgumentException("Height may not be negative.", "height");

				this.width = width;
				this.height = height;
				this.data = new ColorRgba[width * height];

				for (int i = 0; i < this.data.Length; i++)
					this.data[i] = backColor;
			}
			public Layer(int width, int height, ColorRgba[] data)
			{
				if (data == null) throw new ArgumentNullException("data");
				if (width < 0) throw new ArgumentException("Width may not be negative.", "width");
				if (height < 0) throw new ArgumentException("Height may not be negative.", "height");

				this.SetPixelDataRgba(data, width, height);
			}
			public Layer(Bitmap image)
			{
				if (image == null) throw new ArgumentNullException("image");
				this.FromBitmap(image);
			}
			public Layer(string imagePath)
			{
				if (string.IsNullOrEmpty(imagePath)) throw new ArgumentNullException("imagePath");
				
				byte[] buffer = File.ReadAllBytes(imagePath);
				Bitmap bm = new Bitmap(new MemoryStream(buffer));
				this.FromBitmap(bm);
			}
			public Layer(Layer baseLayer)
			{
				if (baseLayer == null) throw new ArgumentNullException("baseLayer");
				baseLayer.CopyTo(this);
			}

			/// <summary>
			/// Clones the pixel data layer and returns the new instance.
			/// </summary>
			/// <returns></returns>
			public Layer Clone()
			{
				return Duality.Cloning.CloneProvider.DeepClone(this);
			}
			/// <summary>
			/// Copies all data contained in this pixel data layer to a target layer.
			/// </summary>
			/// <param name="target"></param>
			public void CopyTo(Layer target)
			{
				if (target == null) throw new ArgumentNullException("target");
				Duality.Cloning.CloneProvider.DeepCopyTo(this, target);
			}

			/// <summary>
			/// Saves the pixel data contained in this layer to the specified file.
			/// </summary>
			/// <param name="imagePath"></param>
			public void SavePixelData(string imagePath)
			{
				this.ToBitmap().Save(imagePath);
			}
			/// <summary>
			/// Loads the pixel data in this layer from the specified file.
			/// </summary>
			/// <param name="imagePath"></param>
			public void LoadPixelData(string imagePath)
			{
				this.FromBitmap(new Bitmap(imagePath));
			}
			/// <summary>
			/// Discards all pixel data in this Layer.
			/// </summary>
			public void ClearPixelData()
			{
				this.data = new ColorRgba[0];
				this.width = 0;
				this.height = 0;
			}
			
			/// <summary>
			/// Creates a <see cref="System.Drawing.Bitmap"/> out of this Layer.
			/// </summary>
			/// <returns></returns>
			public Bitmap ToBitmap()
			{
				if (this.width == 0 || this.height == 0)
					return new Bitmap(1, 1);

				int[] argbValues = this.GetPixelDataIntArgb();
				Bitmap bm = new Bitmap(this.width, this.height);
				BitmapData data = bm.LockBits(
					new Rectangle(0, 0, bm.Width, bm.Height),
					ImageLockMode.WriteOnly,
					PixelFormat.Format32bppArgb);
			
				int pixels = data.Width * data.Height;
				System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, data.Scan0, pixels);

				bm.UnlockBits(data);
				return bm;
			}
			/// <summary>
			/// Gets the Layers pixel data in the ColorRgba format. Note that this data is a clone and thus modifying it won't
			/// affect the Layer it has been retrieved from.
			/// </summary>
			/// <returns></returns>
			public ColorRgba[] GetPixelDataRgba()
			{
				return this.data.Clone() as ColorRgba[];
			}
			/// <summary>
			/// Gets the Layers pixel data in bytewise Rgba format. (Four elements per pixel)
			/// </summary>
			/// <returns></returns>
			public byte[] GetPixelDataByteRgba()
			{
				byte[] rgbaValues = new byte[this.data.Length * 4];
				for (int i = 0; i < this.data.Length; i++)
				{
					rgbaValues[i * 4 + 0] = this.data[i].R;
					rgbaValues[i * 4 + 1] = this.data[i].G;
					rgbaValues[i * 4 + 2] = this.data[i].B;
					rgbaValues[i * 4 + 3] = this.data[i].A;
				}
				return rgbaValues;
			}
			/// <summary>
			/// Gets the Layers pixel data in the integer Argb format. (One element per pixel)
			/// </summary>
			/// <returns></returns>
			public int[] GetPixelDataIntArgb()
			{
				int[] argbValues = new int[this.data.Length];
				unchecked
				{
					for (int i = 0; i < this.data.Length; i++)
						argbValues[i] = this.data[i].ToIntArgb();
				}
				return argbValues;
			}

			/// <summary>
			/// Sets this Layers pixel data to the one contained in the specified <see cref="System.Drawing.Bitmap"/>
			/// </summary>
			/// <param name="bm"></param>
			public void FromBitmap(Bitmap bm)
			{
				// Retrieve data
				BitmapData data = bm.LockBits(
					new Rectangle(0, 0, bm.Width, bm.Height),
					ImageLockMode.ReadOnly,
					PixelFormat.Format32bppArgb);
			
				int pixels = data.Width * data.Height;
				int[] argbValues = new int[pixels];
				System.Runtime.InteropServices.Marshal.Copy(data.Scan0, argbValues, 0, pixels);
				bm.UnlockBits(data);
				
				this.SetPixelDataArgb(argbValues, bm.Width, bm.Height);
				this.ColorTransparentPixels();
			}
			/// <summary>
			/// Sets the layers pixel data in the ColorRgba format. Note that the specified data will be copied and thus modifying it
			/// outside won't affect the Layer it has been inserted into.
			/// </summary>
			/// <param name="pixelData"></param>
			/// <param name="width"></param>
			/// <param name="height"></param>
			public void SetPixelDataRgba(ColorRgba[] pixelData, int width = -1, int height = -1)
			{
				if (width < 0) width = this.width;
				if (height < 0) height = this.height;
				if (pixelData.Length != width * height) throw new ArgumentException("Data length doesn't match width * height", "pixelData");

				this.width = width;
				this.height = height;
				this.data = pixelData.Clone() as ColorRgba[];
			}
			/// <summary>
			/// Sets the Layers pixel data in bytewise Rgba format. (Four elements per pixel)
			/// </summary>
			/// <param name="pixelData"></param>
			/// <param name="width"></param>
			/// <param name="height"></param>
			public void SetPixelDataRgba(byte[] pixelData, int width = -1, int height = -1)
			{
				if (width < 0) width = this.width;
				if (height < 0) height = this.height;
				if (pixelData.Length != 4 * width * height) throw new ArgumentException("Data length doesn't match 4 * width * height", "pixelData");

				this.width = width;
				this.height = height;
				if (this.data == null || this.data.Length != this.width * this.height)
					this.data = new ColorRgba[this.width * this.height];

				for (int i = 0; i < this.data.Length; i++)
				{
					this.data[i].R = pixelData[i * 4 + 0];
					this.data[i].G = pixelData[i * 4 + 1];
					this.data[i].B = pixelData[i * 4 + 2];
					this.data[i].A = pixelData[i * 4 + 3];
				}
			}
			/// <summary>
			/// Sets the Layers pixel data in the integer Argb format. (One element per pixel)
			/// </summary>
			/// <param name="pixelData"></param>
			/// <param name="width"></param>
			/// <param name="height"></param>
			public void SetPixelDataArgb(int[] pixelData, int width = -1, int height = -1)
			{
				if (width < 0) width = this.width;
				if (height < 0) height = this.height;
				if (pixelData.Length != width * height) throw new ArgumentException("Data length doesn't match width * height", "pixelData");

				this.width = width;
				this.height = height;
				if (this.data == null || this.data.Length != this.width * this.height) 
					this.data = new ColorRgba[this.width * this.height];

				for (int i = 0; i < this.data.Length; i++)
					this.data[i].SetIntArgb(pixelData[i]);
			}

			/// <summary>
			/// Rescales the Layer, stretching it to the specified size.
			/// </summary>
			/// <param name="w"></param>
			/// <param name="h"></param>
			/// <param name="filter">The filtering method to use when rescaling</param>
			public void Rescale(int w, int h, FilterMethod filter = FilterMethod.Linear)
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
				Layer tempLayer = new Layer(w, h, backColor);
				this.DrawOnto(tempLayer, BlendMode.Solid, -x, -y);
				tempLayer.CopyTo(this);
			}
			/// <summary>
			/// Crops the Layer, removing transparent / empty border areas.
			/// </summary>
			/// <param name="cropX">Whether the Layer should be cropped in X-direction</param>
			/// <param name="cropY">Whether the Layer should be cropped in Y-direction</param>
			public void Crop(bool cropX = true, bool cropY = true)
			{
				if (!cropX && !cropY) return;
				Rectangle bounds = this.OpaqueBounds();
				this.SubImage(cropX ? bounds.X : 0, cropY ? bounds.Y : 0, cropX ? bounds.Width : this.width, cropY ? bounds.Height : this.height);
			}

			/// <summary>
			/// Measures the bounding rectangle of the Layers opaque pixels.
			/// </summary>
			/// <returns></returns>
			public Rectangle OpaqueBounds()
			{
				Rectangle bounds = new Rectangle(this.width, this.height, 0, 0);
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].A == 0) continue;
					int x = i % this.width;
					int y = i / this.width;
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
				Point	pos		= new Point();
				int[]	nPos	= new int[8];
				bool[]	nOk		= new bool[8];
				int[]	nMult	= new int[] {2, 2, 2, 2, 1, 1, 1, 1};
				int[]	mixClr	= new int[4];

				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].A != 0) continue;

					pos.Y	= i / this.width;
					pos.X	= i - (pos.Y * this.width);

					mixClr[0] = 0;
					mixClr[1] = 0;
					mixClr[2] = 0;
					mixClr[3] = 0;

					nPos[0] = (pos.X + ((pos.Y - 1) * this.width));
					nPos[1] = (pos.X + ((pos.Y + 1) * this.width));
					nPos[2] = ((pos.X - 1) + (pos.Y * this.width));
					nPos[3] = ((pos.X + 1) + (pos.Y * this.width));
					nPos[4] = ((pos.X - 1) + ((pos.Y - 1) * this.width));
					nPos[5] = ((pos.X - 1) + ((pos.Y + 1) * this.width));
					nPos[6] = ((pos.X + 1) + ((pos.Y - 1) * this.width));
					nPos[7] = ((pos.X + 1) + ((pos.Y + 1) * this.width));

					nOk[0]	= pos.Y > 0;
					nOk[1]	= pos.Y < this.height - 1;
					nOk[2]	= pos.X > 0;
					nOk[3]	= pos.X < this.width - 1;
					nOk[4]	= pos.X > 0 && pos.Y > 0;
					nOk[5]	= pos.X > 0 && pos.Y < this.height - 1;
					nOk[6]	= pos.X < this.width - 1 && pos.Y > 0;
					nOk[7]	= pos.X < this.width - 1 && pos.Y < this.height - 1;

					for (int j = 0; j < nPos.Length; j++)
					{
						if (!nOk[j]) continue;
						if (this.data[nPos[j]].A == 0) continue;

						mixClr[0] += this.data[nPos[j]].R * nMult[j];
						mixClr[1] += this.data[nPos[j]].G * nMult[j];
						mixClr[2] += this.data[nPos[j]].B * nMult[j];
						mixClr[3] += nMult[j];
					}

					if (mixClr[3] > 0)
					{
						this.data[i].R = (byte)Math.Round((float)mixClr[0] / (float)mixClr[3]);
						this.data[i].G = (byte)Math.Round((float)mixClr[1] / (float)mixClr[3]);
						this.data[i].B = (byte)Math.Round((float)mixClr[2] / (float)mixClr[3]);
					}
				}
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
			public Layer CloneRescale(int w, int h, FilterMethod filter = FilterMethod.Linear)
			{
				ColorRgba[] result = this.InternalRescale(w, h, filter);
				if (result == null) return this.Clone();

				return new Layer(w, h, result);
			}
			/// <summary>
			/// Resizes the Layers boundaries.
			/// </summary>
			/// <param name="w"></param>
			/// <param name="h"></param>
			/// <param name="origin"></param>
			public Layer CloneResize(int w, int h, Alignment origin = Alignment.TopLeft)
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
			public Layer CloneSubImage(int x, int y, int w, int h)
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
			public Layer CloneSubImage(int x, int y, int w, int h, ColorRgba backColor)
			{
				Layer tempLayer = new Layer(w, h, backColor);
				this.DrawOnto(tempLayer, BlendMode.Solid, -x, -y);
				return tempLayer;
			}
			/// <summary>
			/// Crops the Layer, removing transparent / empty border areas.
			/// </summary>
			/// <param name="cropX">Whether the Layer should be cropped in X-direction</param>
			/// <param name="cropY">Whether the Layer should be cropped in Y-direction</param>
			public Layer CloneCrop(bool cropX = true, bool cropY = true)
			{
				if (!cropX && !cropY) return this.Clone();
				Rectangle bounds = this.OpaqueBounds();
				return this.CloneSubImage(cropX ? bounds.X : 0, cropY ? bounds.Y : 0, cropX ? bounds.Width : this.width, cropY ? bounds.Height : this.height);
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
			public void DrawOnto(Layer target, BlendMode blend, int destX, int destY, int width = -1, int height = -1, int srcX = 0, int srcY = 0)
			{
				if (width == -1) width = this.width;
				if (height == -1) height = this.height;

				int beginX = MathF.Max(0, -destX, -srcX);
				int beginY = MathF.Max(0, -destY, -srcY);
				int endX = MathF.Min(width, this.width, target.width - destX, this.width - srcX);
				int endY = MathF.Min(height, this.height, target.height - destY, this.height - srcY);
				if (endX - beginX < 1) return;
				if (endY - beginY < 1) return;

				System.Threading.Tasks.Parallel.For(beginX, endX, i =>
				//for (int i = beginX; i < endX; i++)
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
			public void DrawOnto(Layer target, BlendMode blend, int destX, int destY, int width, int height, int srcX, int srcY, ColorRgba colorTint)
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
				System.Threading.Tasks.Parallel.For(beginX, endX, i =>
				//for (int i = beginX; i < endX; i++)
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
				});
			}

			private ColorRgba[] InternalRescale(int w, int h, FilterMethod filter)
			{
				if (this.width == w && this.height == h) return null;
				
				ColorRgba[]	tempDestData	= new ColorRgba[w * h];
				if (filter == FilterMethod.Nearest)
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
				else if (filter == FilterMethod.Linear)
				{
					//for (int i = 0; i < tempDestData.Length; i++)
					System.Threading.Tasks.Parallel.For(0, tempDestData.Length, i =>
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
					});
				}
				
				return tempDestData;
			}

			void Cloning.ICloneable.CopyDataTo(object targetObj, Cloning.CloneProvider provider)
			{
				Layer targetLayer = targetObj as Layer;
				targetLayer.width = this.width;
				targetLayer.height = this.height;
				targetLayer.data = this.data == null ? null : this.data.Clone() as ColorRgba[];
			}
			void ISerializable.WriteData(IDataWriter writer)
			{
				writer.WriteValue("version", ResFormat_Version_LayerPng);

				using (MemoryStream str = new MemoryStream(1024 * 64))
				{
					this.ToBitmap().Save(str, System.Drawing.Imaging.ImageFormat.Png);
					writer.WriteValue("pixelData", str.ToArray());
				}
			}
			void ISerializable.ReadData(IDataReader reader)
			{
				int version;
				try { reader.ReadValue("version", out version); }
				catch (Exception) { version = ResFormat_Version_Unknown; }

				if (version == ResFormat_Version_LayerPng)
				{
					byte[] dataBlock;
					reader.ReadValue("pixelData", out dataBlock);
					Bitmap bm = dataBlock != null ? new Bitmap(new MemoryStream(dataBlock)) : null;
					if (bm != null) this.FromBitmap(bm);
				}
			}
		}


		private	List<Layer>	layers		= new List<Layer>();
		private	List<Rect>	atlas		= null;
		private	int			animCols	= 0;
		private	int			animRows	= 0;
		
		/// <summary>
		/// [GET / SET] The main pixel data <see cref="Duality.Resources.Pixmap.Layer"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Layer MainLayer
		{
			get { return this.layers.Count > 0 ? this.layers[0] : null; }
			set
			{
				if (value == null) value = new Layer();
				if (this.layers.Count > 0)
					this.layers[0] = value;
				else
					this.layers.Add(value);
			}
		}
		/// <summary>
		/// [GET / SET] A list of pixel data <see cref="Duality.Resources.Pixmap.Layer">Layers</see>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public List<Layer> PixelData
		{
			get { return this.layers; }
			set
			{
				if (value == null)
					this.layers.Clear();
				else
					this.layers = value;
			}
		}
		/// <summary>
		/// [GET] The Width of the actual pixel data.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int Width
		{
			get { return this.MainLayer != null ? this.MainLayer.Width : 0; }
		}
		/// <summary>
		/// [GET] The Height of the actual pixel data.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int Height
		{
			get { return this.MainLayer != null ? this.MainLayer.Height : 0; }
		}
		/// <summary>
		/// [GET / SET] The Pixmaps atlas array, distinguishing different areas in pixel coordinates
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public List<Rect> Atlas
		{
			get { return this.atlas; }
			set { this.atlas = value; }
		}					//	GS
		/// <summary>
		/// [GET / SET] Information about different animation frames contained in this Pixmap.
		/// Setting this will lead to an auto-generated atlas map according to the animation.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		[EditorHintRange(0, 1024)]
		public int AnimCols
		{
			get { return this.animCols; }
			set { this.GenerateAnimAtlas(value, value == 0 ? 0 : this.animRows); }
		}						//	GS
		/// <summary>
		/// [GET / SET] Information about different animation frames contained in this Pixmap.
		/// Setting this will lead to an auto-generated atlas map according to the animation.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		[EditorHintRange(0, 1024)]
		public int AnimRows
		{
			get { return this.animRows; }
			set { this.GenerateAnimAtlas(value == 0 ? 0 : this.animCols, value); }
		}						//	GS
		/// <summary>
		/// [GET] Total number of animation frames in this Pixmap
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int AnimFrames
		{
			get { return this.animRows * this.animCols; }
		}					//	G
 
		/// <summary>
		/// Creates a new, empty Pixmap.
		/// </summary>
		public Pixmap() : this(Checkerboard.Res.MainLayer.Clone()) {}
		/// <summary>
		/// Creates a new Pixmap from the specified <see cref="System.Drawing.Bitmap"/>.
		/// </summary>
		/// <param name="image">The <see cref="System.Drawing.Bitmap"/> that will be used by the Pixmap.</param>
		public Pixmap(Bitmap image)
		{
			this.MainLayer = new Layer(image);
		}
		/// <summary>
		/// Creates a new Pixmap from the specified <see cref="Duality.Resources.Pixmap.Layer"/>.
		/// </summary>
		/// <param name="image"></param>
		public Pixmap(Layer image)
		{
			this.MainLayer = image;
		}
		/// <summary>
		/// Creates a new Pixmap from the specified image file.
		/// </summary>
		/// <param name="imagePath">A path to the image file that will be used as pixel data source.</param>
		public Pixmap(string imagePath)
		{
			this.LoadPixelData(imagePath);
		}

		/// <summary>
		/// Saves the Pixmaps pixel data as image file. Its image format is determined by the file extension.
		/// </summary>
		/// <param name="imagePath">The path of the file to which the pixel data is written.</param>
		public void SavePixelData(string imagePath = null)
		{
			if (imagePath == null) imagePath = this.sourcePath;

			// We're saving this Pixmaps pixel data for the first time
			if (!this.path.Contains(':') && this.sourcePath == null) this.sourcePath = imagePath;

			if (this.MainLayer != null)
				this.MainLayer.SavePixelData(imagePath);
			else
				Checkerboard.Res.MainLayer.SavePixelData(imagePath);
		}
		/// <summary>
		/// Replaces the Pixmaps pixel data with a new dataset that has been retrieved from file.
		/// </summary>
		/// <param name="imagePath">The path of the file from which to retrieve the new pixel data.</param>
		public void LoadPixelData(string imagePath = null)
		{
			if (imagePath == null) imagePath = this.sourcePath;

			this.sourcePath = imagePath;

			if (String.IsNullOrEmpty(this.sourcePath) || !File.Exists(this.sourcePath))
				this.MainLayer = null;
			else
				this.MainLayer = new Layer(imagePath);
		}

		/// <summary>
		/// Generates a <see cref="Atlas">pixmap atlas</see> for sprite animations but leaves
		/// previously existing atlas entries as they are, if possible. An automatically generated
		/// pixmap atlas will always occupy the first indices, followed by custom atlas entries.
		/// </summary>
		/// <param name="cols">The number of columns in an animated sprite Pixmap</param>
		/// <param name="rows">The number of rows in an animated sprite Pixmap</param>
		public void GenerateAnimAtlas(int cols, int rows)
		{
			// Remove previously existing animation atlas data
			int frames = this.animCols * this.animRows;
			if (this.atlas != null) this.atlas.RemoveRange(0, Math.Min(frames, this.atlas.Count));

			// Set up animation frame data
			if (cols == 0 && rows == 0)
			{
				this.animCols = this.animRows = 0;
				if (this.atlas != null && this.atlas.Count == 0) this.atlas = null;
				return;
			}
			this.animCols = Math.Max(cols, 1);
			this.animRows = Math.Max(rows, 1);

			// Set up new atlas data
			frames = this.animCols * this.animRows;
			if (frames > 0)
			{
				if (this.atlas == null) this.atlas = new List<Rect>(frames);
				int i = 0;
				Vector2 frameSize = new Vector2((float)this.Width / this.animCols, (float)this.Height / this.animRows);
				for (int y = 0; y < this.animRows; y++)
				{
					for (int x = 0; x < this.animCols; x++)
					{
						this.atlas.Insert(i, new Rect(
							x * frameSize.X,
							y * frameSize.Y,
							frameSize.X,
							frameSize.Y));
						i++;
					}
				}
			}
			else if (this.atlas.Count == 0)
				this.atlas = null;
		}
		/// <summary>
		/// Does a safe (null-checked, clamped) pixmap <see cref="Atlas"/> lookup.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="region"></param>
		public void LookupAtlas(int index, out Rect region)
		{
			if (this.atlas == null)
			{
				region.X = region.Y = 0.0f;
				region.W = this.Width;
				region.H = this.Height;
			}
			else
			{
				region = this.atlas[MathF.Clamp(index, 0, this.atlas.Count - 1)];
			}
		}
		/// <summary>
		/// Does a safe (null-checked, clamped) pixmap <see cref="Atlas"/> lookup.
		/// </summary>
		/// <param name="index"></param>
		public Rect LookupAtlas(int index)
		{
			Rect result;
			this.LookupAtlas(index, out result);
			return result;
		}

		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			Pixmap c = r as Pixmap;
			c.layers = this.layers != null ? new List<Layer>(this.layers.Select(l => l.Clone())) : null;
			c.atlas = this.atlas == null ? null : new List<Rect>(this.atlas);
			c.animCols = this.animCols;
			c.animRows = this.animRows;
		}
	}
}
