using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Properties;
using Duality.Editor;
using Duality.Cloning;
using Duality.IO;

namespace Duality.Resources
{
	/// <summary>
	/// Materials are standardized <see cref="BatchInfo">BatchInfos</see>, stored as a Resource. 
	/// Just like BatchInfo objects, they describe how an object, represented by a set of vertices, 
	/// looks like. Using Materials is generally more performant than using BatchInfos but not always
	/// reasonable, for example when there is a single, unique GameObject with a special appearance:
	/// This is a typical <see cref="BatchInfo"/> case.
	/// </summary>
	/// <seealso cref="BatchInfo"/>
	[ExplicitResourceReference(typeof(Texture))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageMaterial)]
	public class Material : Resource
	{
		/// <summary>
		/// A solid, white Material.
		/// </summary>
		public static ContentRef<Material> SolidWhite			{ get; private set; }
		/// <summary>
		/// A solid, black Material.
		/// </summary>
		public static ContentRef<Material> SolidBlack			{ get; private set; }
		/// <summary>
		/// A Material that inverts its background.
		/// </summary>
		public static ContentRef<Material> InvertWhite			{ get; private set; }
		/// <summary>
		/// A Material showing the Duality icon.
		/// </summary>
		public static ContentRef<Material> DualityIcon			{ get; private set; }
		/// <summary>
		/// A Material showing the Duality icon, but without the text on it.
		/// </summary>
		public static ContentRef<Material> DualityIconB			{ get; private set; }
		/// <summary>
		/// A Material showing the Duality logo.
		/// </summary>
		public static ContentRef<Material> DualityLogoBig		{ get; private set; }
		/// <summary>
		/// A Material showing the Duality logo.
		/// </summary>
		public static ContentRef<Material> DualityLogoMedium	{ get; private set; }
		/// <summary>
		/// A Material showing the Duality logo.
		/// </summary>
		public static ContentRef<Material> DualityLogoSmall		{ get; private set; }
		/// <summary>
		/// A Material showing a black and white checkerboard.
		/// </summary>
		public static ContentRef<Material> Checkerboard			{ get; private set; }

		internal static void InitDefaultContent()
		{
			DefaultContent.InitType<Material>(new Dictionary<string,Material>
			{
				{ "SolidWhite", new Material(DrawTechnique.Solid, ColorRgba.White) },
				{ "SolidBlack", new Material(DrawTechnique.Solid, ColorRgba.Black) },
				{ "InvertWhite", new Material(DrawTechnique.Invert, ColorRgba.White) },
				{ "DualityIcon", new Material(DrawTechnique.Mask, Texture.DualityIcon) },
				{ "DualityIconB", new Material(DrawTechnique.Mask, Texture.DualityIconB) },
				{ "DualityLogoBig", new Material(DrawTechnique.Alpha, Texture.DualityLogoBig) },
				{ "DualityLogoMedium", new Material(DrawTechnique.Alpha, Texture.DualityLogoMedium) },
				{ "DualityLogoSmall", new Material(DrawTechnique.Alpha, Texture.DualityLogoSmall) },
				{ "Checkerboard", new Material(DrawTechnique.Solid, Texture.Checkerboard) },
			});
		}


		private BatchInfo info = new BatchInfo();
		
		/// <summary>
		/// [GET] Returns the Materials internal <see cref="BatchInfo"/> instance, which can be used for rendering.
		/// </summary>
		public BatchInfo Info
		{
			get { return this.info; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.DrawTechnique"/> that is used.
		/// </summary>
		public ContentRef<DrawTechnique> Technique
		{
			get { return this.info.Technique; }
			set { this.info.Technique = value; }
		}
		/// <summary>
		/// [GET / SET] The main color of the material. This property is a shortcut for
		/// a regular shader parameter as accessible via <see cref="GetValue"/>.
		/// </summary>
		public ColorRgba MainColor
		{
			get { return this.info.MainColor; }
			set { this.info.MainColor = value; }
		}
		/// <summary>
		/// [GET / SET] The main texture of the material. This property is a shortcut for
		/// a regular shader parameter as accessible via <see cref="GetTexture"/>.
		/// </summary>
		public ContentRef<Texture> MainTexture
		{
			get { return this.info.MainTexture; }
			set { this.info.MainTexture = value; }
		}

		/// <summary>
		/// Creates a new Material
		/// </summary>
		public Material()
		{
			this.info = new BatchInfo();
		}
		/// <summary>
		/// Creates a new Material based on the specified <see cref="DrawTechnique"/>.
		/// </summary>
		/// <param name="technique">The <see cref="Duality.Resources.DrawTechnique"/> to use.</param>
		public Material(ContentRef<DrawTechnique> technique)
		{
			this.info = new BatchInfo(technique);
		}
		/// <summary>
		/// Creates a new color-only Material.
		/// </summary>
		/// <param name="technique">The <see cref="Duality.Resources.DrawTechnique"/> to use.</param>
		/// <param name="mainColor">The <see cref="MainColor"/> to use.</param>
		public Material(ContentRef<DrawTechnique> technique, ColorRgba mainColor)
		{
			this.info = new BatchInfo(technique, mainColor);
		}
		/// <summary>
		/// Creates a new single-texture Material.
		/// </summary>
		/// <param name="technique">The <see cref="Duality.Resources.DrawTechnique"/> to use.</param>
		/// <param name="mainTex">The main <see cref="Duality.Resources.Texture"/> to use.</param>
		public Material(ContentRef<DrawTechnique> technique, ContentRef<Texture> mainTex)
		{
			this.info = new BatchInfo(technique, mainTex);
		}
		/// <summary>
		/// Creates a new tinted, single-texture Material.
		/// </summary>
		/// <param name="technique">The <see cref="Duality.Resources.DrawTechnique"/> to use.</param>
		/// <param name="mainColor">The <see cref="MainColor"/> to use.</param>
		/// <param name="mainTex">The main <see cref="Duality.Resources.Texture"/> to use.</param>
		public Material(ContentRef<DrawTechnique> technique, ColorRgba mainColor, ContentRef<Texture> mainTex)
		{
			this.info = new BatchInfo(technique, mainColor, mainTex);
		}
		/// <summary>
		/// Creates a new Material based on the specified BatchInfo
		/// </summary>
		/// <param name="info"></param>
		public Material(BatchInfo info)
		{
			this.info = new BatchInfo(info);
		}

		/// <summary>
		/// Assigns an array of values to the specified variable. All values are copied and converted into
		/// a shared internal format.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <seealso cref="BatchInfo.SetArray"/>
		public void SetArray<T>(string name, T[] value) where T : struct
		{
			this.info.SetArray<T>(name, value);
		}
		/// <summary>
		/// Assigns a blittable value to the specified variable. All values are copied and converted into
		/// a shared internal format.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <seealso cref="BatchInfo.SetValue"/>
		public void SetValue<T>(string name, T value) where T : struct
		{
			this.info.SetValue<T>(name, value);
		}
		/// <summary>
		/// Assigns a texture to the specified variable.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <seealso cref="BatchInfo.SetTexture"/>
		public void SetTexture(string name, ContentRef<Texture> value)
		{
			this.info.SetTexture(name, value);
		}
		
		/// <summary>
		/// Retrieves a copy of the values that are assigned the specified variable. If the internally 
		/// stored type does not match the specified type, it will be converted before returning.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <seealso cref="BatchInfo.GetArray"/>
		public T[] GetArray<T>(string name) where T : struct
		{
			return this.info.GetArray<T>(name);
		}
		/// <summary>
		/// Retrieves a blittable value from the specified variable. All values are copied and converted into
		/// a shared internal format.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <seealso cref="BatchInfo.GetValue"/>
		public T GetValue<T>(string name) where T : struct
		{
			return this.info.GetValue<T>(name);
		}
		/// <summary>
		/// Retrieves a texture from the specified variable.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <seealso cref="BatchInfo.GetTexture"/>
		public ContentRef<Texture> GetTexture(string name)
		{
			return this.info.GetTexture(name);
		}

		protected override void OnLoaded()
		{
			// Fallback to default in case the internal data failed to load
			if (this.info == null)
				this.info = new BatchInfo();

			base.OnLoaded();
		}
	}
}
