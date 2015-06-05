using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Editor;
using Duality.Properties;

namespace Duality.Resources
{
	/// <summary>
	/// DrawTechniques represent the method by which a set of colors, <see cref="Duality.Resources.Texture">Textures</see> and
	/// vertex data is applied to screen. 
	/// </summary>
	/// <seealso cref="Duality.Resources.Material"/>
	/// <seealso cref="Duality.Resources.ShaderProgram"/>
	/// <seealso cref="Duality.Drawing.BlendMode"/>
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageDrawTechnique)]
	public class DrawTechnique : Resource
	{
		/// <summary>
		/// Renders solid geometry without utilizing the alpha channel. This is the fastest default DrawTechnique.
		/// </summary>
		public static ContentRef<DrawTechnique> Solid		{ get; private set; }
		/// <summary>
		/// Renders alpha-masked solid geometry. This is the recommended DrawTechnique for regular sprite rendering.
		/// If multisampling is available, it is utilized to smooth masked edges.
		/// </summary>
		public static ContentRef<DrawTechnique> Mask		{ get; private set; }
		/// <summary>
		/// Renders geometry using the alpha channel, but enforces sharp edges by using an adaptive antialiazing shader.
		/// This is the recommended DrawTechnique for rendering text or stencil sprites.
		/// </summary>
		public static ContentRef<DrawTechnique> SharpAlpha	{ get; private set; }
		/// <summary>
		/// Renders additive geometry. Ideal for glow effects.
		/// </summary>
		public static ContentRef<DrawTechnique> Add			{ get; private set; }
		/// <summary>
		/// Renders geometry and using the alpha channel. However, for stencil-sharp alpha edges, <see cref="Mask"/> might
		/// be sufficient and is a lot faster. Consider using it.
		/// </summary>
		public static ContentRef<DrawTechnique> Alpha		{ get; private set; }
		/// <summary>
		/// Renders geometry multiplying the existing background with incoming color values. Can be used for shadowing effects.
		/// </summary>
		public static ContentRef<DrawTechnique> Multiply	{ get; private set; }
		/// <summary>
		/// Renders geometry adding incoming color values weighted based on the existing background. Can be used for lighting effects.
		/// </summary>
		public static ContentRef<DrawTechnique> Light		{ get; private set; }
		/// <summary>
		/// Renders geometry inverting the background color.
		/// </summary>
		public static ContentRef<DrawTechnique> Invert		{ get; private set; }
		/// <summary>
		/// Renders geometry for a picking operation. This isn't used for regular rendering.
		/// </summary>
		public static ContentRef<DrawTechnique> Picking		{ get; private set; }
		
		/// <summary>
		/// Renders SmoothAnim solid geometry without utilizing the alpha channel. This is the fastest default DrawTechnique.
		/// </summary>
		public static ContentRef<DrawTechnique> SmoothAnim_Solid	{ get; private set; }
		/// <summary>
		/// Renders SmoothAnim alpha-masked solid geometry. This is the recommended DrawTechnique for regular sprite rendering.
		/// If multisampling is available, it is utilized to smooth masked edges.
		/// </summary>
		public static ContentRef<DrawTechnique> SmoothAnim_Mask		{ get; private set; }
		/// <summary>
		/// Renders SmoothAnim additive geometry. Ideal for glow effects.
		/// </summary>
		public static ContentRef<DrawTechnique> SmoothAnim_Add		{ get; private set; }
		/// <summary>
		/// Renders SmoothAnim geometry and using the alpha channel. However, for stencil-sharp alpha edges, <see cref="Mask"/> might
		/// be sufficient and is a lot faster. Consider using it.
		/// </summary>
		public static ContentRef<DrawTechnique> SmoothAnim_Alpha	{ get; private set; }
		/// <summary>
		/// Renders SmoothAnim geometry multiplying the existing background with incoming color values. Can be used for shadowing effects.
		/// </summary>
		public static ContentRef<DrawTechnique> SmoothAnim_Multiply	{ get; private set; }
		/// <summary>
		/// Renders SmoothAnim geometry adding incoming color values weighted based on the existing background. Can be used for lighting effects.
		/// </summary>
		public static ContentRef<DrawTechnique> SmoothAnim_Light	{ get; private set; }
		/// <summary>
		/// Renders SmoothAnim geometry inverting the background color.
		/// </summary>
		public static ContentRef<DrawTechnique> SmoothAnim_Invert	{ get; private set; }

		internal static void InitDefaultContent()
		{
			InitDefaultContentFromDictionary<DrawTechnique>(new Dictionary<string,DrawTechnique>
			{
				{ "Solid", new DrawTechnique(BlendMode.Solid) },
				{ "Mask", new DrawTechnique(BlendMode.Mask) },
				{ "Add", new DrawTechnique(BlendMode.Add) },
				{ "Alpha", new DrawTechnique(BlendMode.Alpha) },
				{ "Multiply", new DrawTechnique(BlendMode.Multiply) },
				{ "Light", new DrawTechnique(BlendMode.Light) },
				{ "Invert", new DrawTechnique(BlendMode.Invert) },

				{ "Picking", new DrawTechnique(BlendMode.Mask, ShaderProgram.Picking) },
				{ "SharpAlpha", new DrawTechnique(BlendMode.Alpha, ShaderProgram.SharpAlpha) },
				
				{ "SmoothAnim_Solid", new DrawTechnique(BlendMode.Solid, ShaderProgram.SmoothAnim, VertexC1P3T4A1.Declaration) },
				{ "SmoothAnim_Mask", new DrawTechnique(BlendMode.Mask, ShaderProgram.SmoothAnim, VertexC1P3T4A1.Declaration) },
				{ "SmoothAnim_Add", new DrawTechnique(BlendMode.Add, ShaderProgram.SmoothAnim, VertexC1P3T4A1.Declaration) },
				{ "SmoothAnim_Alpha", new DrawTechnique(BlendMode.Alpha, ShaderProgram.SmoothAnim, VertexC1P3T4A1.Declaration) },
				{ "SmoothAnim_Multiply", new DrawTechnique(BlendMode.Multiply, ShaderProgram.SmoothAnim, VertexC1P3T4A1.Declaration) },
				{ "SmoothAnim_Light", new DrawTechnique(BlendMode.Light, ShaderProgram.SmoothAnim, VertexC1P3T4A1.Declaration) },
				{ "SmoothAnim_Invert", new DrawTechnique(BlendMode.Invert, ShaderProgram.SmoothAnim, VertexC1P3T4A1.Declaration) },
			});
		}
		

		private	BlendMode					blendType	= BlendMode.Solid;
		private	ContentRef<ShaderProgram>	shader		= null;
		private	Type						prefType	= null;
		[DontSerialize]
		private	VertexDeclaration		prefFormat	= null;

		/// <summary>
		/// [GET / SET] Specifies how incoming color values interact with the existing background color.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public BlendMode Blending
		{
			get { return this.blendType; }
			set { this.blendType = value; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.ShaderProgram"/> that is used for rendering.
		/// </summary>
		public ContentRef<ShaderProgram> Shader
		{
			get { return this.shader; }
			set { this.shader = value; }
		}
		/// <summary>
		/// [GET / SET] The vertex format that is preferred by this DrawTechnique. If there is no specific preference,
		/// null is returned.
		/// </summary>
		public VertexDeclaration PreferredVertexFormat
		{
			get { return this.prefFormat; }
			set
			{
				this.prefFormat = value;
				this.prefType = value != null ? value.DataType : null;
			}
		}
		/// <summary>
		/// [GET] Returns whether this DrawTechnique requires z sorting. It is derived from its <see cref="Blending"/>.
		/// </summary>
		public bool NeedsZSort
		{
			get 
			{ 
				return 
					this.blendType == BlendMode.Alpha ||
					this.blendType == BlendMode.AlphaPre ||
					this.blendType == BlendMode.Add ||
					this.blendType == BlendMode.Invert ||
					this.blendType == BlendMode.Multiply ||
					this.blendType == BlendMode.Light; 
			}
		}
		/// <summary>
		/// [GET] Returns whether this DrawTechnique requires any <see cref="PrepareRendering">rendering preparation</see>.
		/// This is false for all standard DrawTechniques, but may return true when deriving custom DrawTechniques.
		/// </summary>
		public virtual bool NeedsPreparation
		{
			get { return false; }
		}

		/// <summary>
		/// Creates a new, default DrawTechnique
		/// </summary>
		public DrawTechnique() {}
		/// <summary>
		/// Creates a new DrawTechnique that uses the specified <see cref="BlendMode"/>.
		/// </summary>
		/// <param name="blendType"></param>
		public DrawTechnique(BlendMode blendType) 
		{
			this.blendType = blendType;
		}
		/// <summary>
		/// Creates a new DrawTechnique using the specified <see cref="BlendMode"/> and <see cref="Duality.Resources.ShaderProgram"/>.
		/// </summary>
		/// <param name="blendType"></param>
		/// <param name="shader"></param>
		/// <param name="formatPref"></param>
		public DrawTechnique(BlendMode blendType, ContentRef<ShaderProgram> shader, VertexDeclaration formatPref = null) 
		{
			this.blendType = blendType;
			this.shader = shader;
			this.prefFormat = formatPref;
			this.prefType = formatPref != null ? formatPref.DataType : null;
		}

		/// <summary>
		/// Prepares rendering using this DrawTechnique.
		/// </summary>
		/// <param name="device"></param>
		/// <param name="material"></param>
		public virtual void PrepareRendering(IDrawDevice device, BatchInfo material) {}

		protected override void OnLoaded()
		{
			base.OnLoaded();
			this.prefFormat = VertexDeclaration.Get(this.prefType);
		}
	}
}
