using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

using Duality.Drawing;
using Duality.Editor;
using Duality.Serialization;
using Duality.Cloning;
using Duality.Properties;

namespace Duality.Resources
{
	/// <summary>
	/// A Pixmap stores pixel data in system memory. 
	/// </summary>
	/// <seealso cref="Duality.Resources.Texture"/>
	[ExplicitResourceReference()]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImagePixmap)]
	public class Pixmap : Resource
	{
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
			InitDefaultContentFromEmbedded<Pixmap>(".png", stream => new Pixmap(stream));
		}

		
		private	List<PixelData>	layers			= new List<PixelData>();
		private	List<Rect>		atlas			= null;
		private	int				animCols		= 0;
		private	int				animRows		= 0;
		private	int				animFrameBorder	= 0;
		
		/// <summary>
		/// [GET / SET] The main <see cref="Duality.Drawing.PixelData"/> layer of this <see cref="Pixmap"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public PixelData MainLayer
		{
			get { return this.layers.Count > 0 ? this.layers[0] : null; }
			set
			{
				if (value == null) value = new PixelData();
				if (this.layers.Count > 0)
					this.layers[0] = value;
				else
					this.layers.Add(value);
			}
		}
		/// <summary>
		/// [GET / SET] A list of <see cref="Duality.Drawing.PixelData"/> layers.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public List<PixelData> PixelData
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
		/// [GET / SET] Pixel size of the border around each individual animation frame.
		/// within the image.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		[EditorHintRange(0, 64)]
		public int AnimFrameBorder
		{
			get { return this.animFrameBorder; }
			set { this.GenerateAnimAtlas(this.animCols, this.animRows, value); }
		}						//	GS
		/// <summary>
		/// [GET / SET] Information about different animation frames contained in this Pixmap.
		/// Setting this will lead to an auto-generated atlas map according to the animation.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		[EditorHintRange(0, 1024)]
		public int AnimCols
		{
			get { return this.animCols; }
			set { this.GenerateAnimAtlas(value, value == 0 ? 0 : this.animRows, this.animFrameBorder); }
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
			set { this.GenerateAnimAtlas(value == 0 ? 0 : this.animCols, value, this.animFrameBorder); }
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
		/// Creates a new Pixmap from the specified <see cref="Duality.Drawing.PixelData"/>.
		/// </summary>
		/// <param name="image"></param>
		public Pixmap(PixelData image)
		{
			this.MainLayer = image;
		}
		/// <summary>
		/// Creates a new Pixmap from the specified <see cref="System.Drawing.Bitmap"/>.
		/// </summary>
		/// <param name="image">The <see cref="System.Drawing.Bitmap"/> that will be used by the Pixmap.</param>
		public Pixmap(Bitmap image)
		{
			this.MainLayer = new PixelData(image);
		}
		/// <summary>
		/// Creates a new Pixmap from the specified image stream.
		/// </summary>
		/// <param name="imagePath">A path to the image stream that will be used as pixel data source.</param>
		public Pixmap(Stream imageStream)
		{
			using (Bitmap image = new Bitmap(imageStream))
			{
				this.MainLayer = new PixelData(image);
			}
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
			if (!this.IsDefaultContent && this.sourcePath == null) this.sourcePath = imagePath;

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
				this.MainLayer = new PixelData(imagePath);
		}

		/// <summary>
		/// Generates a <see cref="Atlas">pixmap atlas</see> for sprite animations but leaves
		/// previously existing atlas entries as they are, if possible. An automatically generated
		/// pixmap atlas will always occupy the first indices, followed by custom atlas entries.
		/// </summary>
		/// <param name="cols">The number of columns in an animated sprite Pixmap</param>
		/// <param name="rows">The number of rows in an animated sprite Pixmap</param>
		public void GenerateAnimAtlas(int cols, int rows, int frameBorder)
		{
			// Remove previously existing animation atlas data
			int frames = this.animCols * this.animRows;
			if (this.atlas != null) this.atlas.RemoveRange(0, Math.Min(frames, this.atlas.Count));

			// Discard empty atlas
			if (cols == 0 && rows == 0)
			{
				this.animCols = 0;
				this.animRows = 0;
				this.animFrameBorder = frameBorder;
				if (this.atlas != null && this.atlas.Count == 0) this.atlas = null;
				return;
			}

			this.animCols = Math.Max(cols, 1);
			this.animRows = Math.Max(rows, 1);

			Vector2 frameSize = new Vector2((float)this.Width / this.animCols, (float)this.Height / this.animRows);

			this.animFrameBorder = MathF.Clamp(frameBorder, 0, (int)(MathF.Min(frameSize.X, frameSize.Y) * 0.5f));

			// Set up new atlas data
			frames = this.animCols * this.animRows;
			if (frames > 0)
			{
				if (this.atlas == null) this.atlas = new List<Rect>(frames);
				int i = 0;
				for (int y = 0; y < this.animRows; y++)
				{
					for (int x = 0; x < this.animCols; x++)
					{
						Rect frameRect = new Rect(
							x * frameSize.X + this.animFrameBorder,
							y * frameSize.Y + this.animFrameBorder,
							frameSize.X - this.animFrameBorder * 2,
							frameSize.Y - this.animFrameBorder * 2);
						this.atlas.Insert(i, frameRect);
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

		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);

			// Get rid of the big data blob, so the GC can collect it.
			this.layers.Clear();
		}
	}
}
