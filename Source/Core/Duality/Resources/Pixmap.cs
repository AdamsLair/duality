using System;
using System.Linq;
using System.Collections.Generic;
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
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImagePixmap)]
	public class Pixmap : Resource
	{
		/// <summary>
		/// [GET] A Pixmap showing the Duality icon.
		/// </summary>
		public static ContentRef<Pixmap> DualityIcon       { get; private set; }
		/// <summary>
		/// [GET] A Pixmap showing the Duality icon without the text on it.
		/// </summary>
		public static ContentRef<Pixmap> DualityIconB      { get; private set; }
		/// <summary>
		/// A Pixmap showing the Duality logo.
		/// </summary>
		public static ContentRef<Pixmap> DualityLogoBig    { get; private set; }
		/// <summary>
		/// A Pixmap showing the Duality logo.
		/// </summary>
		public static ContentRef<Pixmap> DualityLogoMedium { get; private set; }
		/// <summary>
		/// A Pixmap showing the Duality logo.
		/// </summary>
		public static ContentRef<Pixmap> DualityLogoSmall  { get; private set; }
		/// <summary>
		/// [GET] A plain white 1x1 Pixmap. Can be used as a dummy.
		/// </summary>
		public static ContentRef<Pixmap> White             { get; private set; }
		/// <summary>
		/// [GET] A 256x256 black and white checkerboard image.
		/// </summary>
		public static ContentRef<Pixmap> Checkerboard      { get; private set; }

		internal static void InitDefaultContent()
		{
			IImageCodec codec = ImageCodec.GetRead(ImageCodec.FormatPng);
			if (codec == null)
			{
				Logs.Core.WriteError(
					"Unable to retrieve image codec for format '{0}'. Can't initialize default {1} Resources.",
					ImageCodec.FormatPng,
					typeof(Pixmap).Name);

				// Initialize default content with generic error instances, so
				// everything else can still work as expected. We logged the error,
				// and there's nothing anyone can do about this at runtime, so just 
				// fail gracefully without causing more trouble.
				DefaultContent.InitType<Pixmap>(name => new Pixmap(new PixelData(1, 1, new ColorRgba(255, 0, 255))));

				return;
			}
			DefaultContent.InitType<Pixmap>(".png", stream => new Pixmap(codec.Read(stream)));
		}

		
		private List<PixelData> layers = new List<PixelData>();
		private List<Rect>      atlas  = null;
		
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
		/// [GET] The image width of this <see cref="Pixmap"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int Width
		{
			get { return this.MainLayer != null ? this.MainLayer.Width : 0; }
		}
		/// <summary>
		/// [GET] The image height of this <see cref="Pixmap"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int Height
		{
			get { return this.MainLayer != null ? this.MainLayer.Height : 0; }
		}
		/// <summary>
		/// [GET] The image size of this <see cref="Pixmap"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Point2 Size
		{
			get { return this.MainLayer != null ? this.MainLayer.Size : Point2.Zero; }
		}
		/// <summary>
		/// [GET / SET] The Pixmaps atlas array, distinguishing different areas in pixel coordinates
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public List<Rect> Atlas
		{
			get { return this.atlas; }
			set { this.atlas = value; }
		}

 
		/// <summary>
		/// Creates a new, empty Pixmap.
		/// </summary>
		public Pixmap() : this(null) {}
		/// <summary>
		/// Creates a new Pixmap from the specified <see cref="Duality.Drawing.PixelData"/>.
		/// </summary>
		/// <param name="image"></param>
		public Pixmap(PixelData image)
		{
			this.MainLayer = image;
		}

		/// <summary>
		/// Does a safe (null-checked, clamped) pixmap <see cref="Atlas"/> lookup.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="region"></param>
		public void LookupAtlas(int index, out Rect region)
		{
			if (this.atlas == null)
				region = new Rect(this.Size);
			else
				region = this.atlas[MathF.Clamp(index, 0, this.atlas.Count - 1)];
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

		protected override void OnLoaded()
		{
			// Fallback to default in case the internal data failed to load
			if (this.layers == null)
				this.layers = new List<PixelData>();

			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);

			// Get rid of the big data blob, so the GC can collect it.
			this.layers.Clear();
		}
	}
}
