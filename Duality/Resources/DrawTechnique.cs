using System;
using System.Collections.Generic;
using System.Linq;

using Duality.VertexFormat;
using Duality.EditorHints;
using Duality.ColorFormat;

using OpenTK.Graphics.OpenGL;

namespace Duality.Resources
{
	/// <summary>
	/// DrawTechniques represent the method by which a set of colors, <see cref="Duality.Resources.Texture">Textures</see> and
	/// vertex data is applied to screen. 
	/// </summary>
	/// <seealso cref="Duality.Resources.Material"/>
	/// <seealso cref="Duality.Resources.ShaderProgram"/>
	/// <seealso cref="Duality.BlendMode"/>
	[Serializable]
	public class DrawTechnique : Resource
	{
		/// <summary>
		/// A DrawTechnique resources file extension.
		/// </summary>
		public new const string FileExt = ".DrawTechnique" + Resource.FileExt;
		
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
		/// Renders alpha-masked solid geometry and enforces sharp edges by using an adaptive antialiazing shader.
		/// This is the recommended DrawTechnique for rendering text or stencil sprites.
		/// </summary>
		public static ContentRef<DrawTechnique> SharpMask	{ get; private set; }
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

		/// <summary>
		/// [GET] Returns whether the <see cref="BlendMode.Mask">masked</see> DrawTechniques utilize OpenGL Alpha-to-Coverage to
		/// smooth alpha-edges in masked rendering.
		/// </summary>
		public static bool MaskUseAlphaToCoverage 
		{ 
			get 
			{
				if (RenderTarget.BoundRT.IsExplicitNull)
					return DualityApp.TargetMode.Samples > 0; 
				else
					return RenderTarget.BoundRT.Res.Samples > 0;
			}
		}

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath		= ContentProvider.VirtualContentPath + "DrawTechnique:";
			const string ContentDir_SmoothAnim	= VirtualContentPath + "SmoothAnim:";
		
			const string ContentPath_Solid		= VirtualContentPath + "Solid";
			const string ContentPath_Mask		= VirtualContentPath + "Mask";
			const string ContentPath_SharpMask	= VirtualContentPath + "SharpMask";
			const string ContentPath_Add		= VirtualContentPath + "Add";
			const string ContentPath_Alpha		= VirtualContentPath + "Alpha";
			const string ContentPath_Multiply	= VirtualContentPath + "Multiply";
			const string ContentPath_Light		= VirtualContentPath + "Light";
			const string ContentPath_Invert		= VirtualContentPath + "Invert";
			const string ContentPath_Picking	= VirtualContentPath + "Picking";
		
			const string ContentPath_SmoothAnim_Solid		= ContentDir_SmoothAnim + "Solid";
			const string ContentPath_SmoothAnim_Mask		= ContentDir_SmoothAnim + "Mask";
			const string ContentPath_SmoothAnim_Add			= ContentDir_SmoothAnim + "Add";
			const string ContentPath_SmoothAnim_Alpha		= ContentDir_SmoothAnim + "Alpha";
			const string ContentPath_SmoothAnim_Multiply	= ContentDir_SmoothAnim + "Multiply";
			const string ContentPath_SmoothAnim_Light		= ContentDir_SmoothAnim + "Light";
			const string ContentPath_SmoothAnim_Invert		= ContentDir_SmoothAnim + "Invert";

			ContentProvider.RegisterContent(ContentPath_Solid,		new DrawTechnique(BlendMode.Solid));
			ContentProvider.RegisterContent(ContentPath_Mask,		new DrawTechnique(BlendMode.Mask));
			ContentProvider.RegisterContent(ContentPath_Add,		new DrawTechnique(BlendMode.Add));
			ContentProvider.RegisterContent(ContentPath_Alpha,		new DrawTechnique(BlendMode.Alpha));
			ContentProvider.RegisterContent(ContentPath_Multiply,	new DrawTechnique(BlendMode.Multiply));
			ContentProvider.RegisterContent(ContentPath_Light,		new DrawTechnique(BlendMode.Light));
			ContentProvider.RegisterContent(ContentPath_Invert,		new DrawTechnique(BlendMode.Invert));

			ContentProvider.RegisterContent(ContentPath_Picking,	new DrawTechnique(BlendMode.Mask, ShaderProgram.Picking));
			ContentProvider.RegisterContent(ContentPath_SharpMask,	new DrawTechnique(BlendMode.Alpha, ShaderProgram.SharpMask));
			
			ContentProvider.RegisterContent(ContentPath_SmoothAnim_Solid,		new DrawTechnique(BlendMode.Solid,		ShaderProgram.SmoothAnim, VertexType_C1P3T4A1));
			ContentProvider.RegisterContent(ContentPath_SmoothAnim_Mask,		new DrawTechnique(BlendMode.Mask,		ShaderProgram.SmoothAnim, VertexType_C1P3T4A1));
			ContentProvider.RegisterContent(ContentPath_SmoothAnim_Add,			new DrawTechnique(BlendMode.Add,		ShaderProgram.SmoothAnim, VertexType_C1P3T4A1));
			ContentProvider.RegisterContent(ContentPath_SmoothAnim_Alpha,		new DrawTechnique(BlendMode.Alpha,		ShaderProgram.SmoothAnim, VertexType_C1P3T4A1));
			ContentProvider.RegisterContent(ContentPath_SmoothAnim_Multiply,	new DrawTechnique(BlendMode.Multiply,	ShaderProgram.SmoothAnim, VertexType_C1P3T4A1));
			ContentProvider.RegisterContent(ContentPath_SmoothAnim_Light,		new DrawTechnique(BlendMode.Light,		ShaderProgram.SmoothAnim, VertexType_C1P3T4A1));
			ContentProvider.RegisterContent(ContentPath_SmoothAnim_Invert,		new DrawTechnique(BlendMode.Invert,		ShaderProgram.SmoothAnim, VertexType_C1P3T4A1));

			Solid		= ContentProvider.RequestContent<DrawTechnique>(ContentPath_Solid);
			Mask		= ContentProvider.RequestContent<DrawTechnique>(ContentPath_Mask);
			Add			= ContentProvider.RequestContent<DrawTechnique>(ContentPath_Add);
			Alpha		= ContentProvider.RequestContent<DrawTechnique>(ContentPath_Alpha);
			Multiply	= ContentProvider.RequestContent<DrawTechnique>(ContentPath_Multiply);
			Light		= ContentProvider.RequestContent<DrawTechnique>(ContentPath_Light);
			Invert		= ContentProvider.RequestContent<DrawTechnique>(ContentPath_Invert);
			Picking		= ContentProvider.RequestContent<DrawTechnique>(ContentPath_Picking);
			SharpMask	= ContentProvider.RequestContent<DrawTechnique>(ContentPath_SharpMask);

			SmoothAnim_Solid	= ContentProvider.RequestContent<DrawTechnique>(ContentPath_SmoothAnim_Solid);
			SmoothAnim_Mask		= ContentProvider.RequestContent<DrawTechnique>(ContentPath_SmoothAnim_Mask);
			SmoothAnim_Add		= ContentProvider.RequestContent<DrawTechnique>(ContentPath_SmoothAnim_Add);
			SmoothAnim_Alpha	= ContentProvider.RequestContent<DrawTechnique>(ContentPath_SmoothAnim_Alpha);
			SmoothAnim_Multiply	= ContentProvider.RequestContent<DrawTechnique>(ContentPath_SmoothAnim_Multiply);
			SmoothAnim_Light	= ContentProvider.RequestContent<DrawTechnique>(ContentPath_SmoothAnim_Light);
			SmoothAnim_Invert	= ContentProvider.RequestContent<DrawTechnique>(ContentPath_SmoothAnim_Invert);
		}
		static DrawTechnique()
		{
			InitVertexTypeIndices();
		}
		
		/// <summary>
		/// The maximum number of vertex types.
		/// </summary>
		public	const	int		MaxVertexTypes		= 16;
		/// <summary>
		/// Unknown format.
		/// </summary>
		public	const	int		VertexType_Unknown	= -1;
		/// <summary>
		/// <see cref="Duality.VertexFormat.VertexC1P3"/> format.
		/// </summary>
		public	const	int		VertexType_C1P3		= 1;
		/// <summary>
		/// <see cref="Duality.VertexFormat.VertexC1P3T2"/> format.
		/// </summary>
		public	const	int		VertexType_C1P3T2	= 2;
		/// <summary>
		/// <see cref="Duality.VertexFormat.VertexC1P3T4A1"/> format.
		/// </summary>
		public	const	int		VertexType_C1P3T4A1	= 3;

		private static Dictionary<string,int>	vertexTypeIndexMap	= new Dictionary<string,int>();
		public static IEnumerable<KeyValuePair<string,int>> VertexTypeIndices
		{
			get { return vertexTypeIndexMap; }
		}

		private static void InitVertexTypeIndices()
		{
			vertexTypeIndexMap.Clear();
			vertexTypeIndexMap["Unknown"]					= VertexType_Unknown;
			vertexTypeIndexMap[typeof(VertexC1P3).Name]		= VertexType_C1P3;
			vertexTypeIndexMap[typeof(VertexC1P3T2).Name]	= VertexType_C1P3T2;
			vertexTypeIndexMap[typeof(VertexC1P3T4A1).Name] = VertexType_C1P3T4A1;
		}
		public static int RequestVertexTypeIndex(string name)
		{
			int index;
			if (vertexTypeIndexMap.TryGetValue(name, out index)) return index;
			for (int i = 0; i < MaxVertexTypes; i++)
			{
				if (!vertexTypeIndexMap.Values.Contains(i))
				{
					vertexTypeIndexMap[name] = i;
					return i;
				}
			}
			throw new InvalidOperationException("Maximum number of vertex formats reached");
		}
		public static void ReleaseVertexTypeIndex(string name)
		{
			if (name == "Unknown") return;
			vertexTypeIndexMap.Remove(name);
		}


		private	BlendMode					blendType	= BlendMode.Solid;
		private	ContentRef<ShaderProgram>	shader		= ContentRef<ShaderProgram>.Null;
		private	int							formatPref	= VertexType_Unknown;

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
		/// <see cref="VertexType_Unknown"/> is returned.
		/// </summary>
		public int PreferredVertexFormat
		{
			get { return this.formatPref; }
			set { this.formatPref = value; }
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
					this.blendType == BlendMode.Add ||
					this.blendType == BlendMode.Invert ||
					this.blendType == BlendMode.Multiply ||
					this.blendType == BlendMode.Light; 
			}
		}
		/// <summary>
		/// [GET] Returns whether this DrawTechnique requires <see cref="PreprocessBatch{T}">vertex preprocessing</see>.
		/// This is false for all standard DrawTechniques, but may return true when deriving custom DrawTechniques.
		/// </summary>
		public virtual bool NeedsPreprocess
		{
			get { return false; }
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
		public DrawTechnique(BlendMode blendType, ContentRef<ShaderProgram> shader, int formatPref = VertexType_Unknown) 
		{
			this.blendType = blendType;
			this.shader = shader;
			this.formatPref = formatPref;
		}
		
		/// <summary>
		/// Performs a preprocessing operation for incoming vertices. Does nothing by default but may be overloaded, if needed.
		/// </summary>
		/// <typeparam name="T">The incoming vertex type</typeparam>
		/// <param name="device"></param>
		/// <param name="material"><see cref="Duality.Resources.Material"/> information for the current batch.</param>
		/// <param name="vertexMode">The mode of incoming vertex data.</param>
		/// <param name="vertexBuffer">A buffer storing incoming vertex data.</param>
		/// <param name="vertexCount">The number of vertices to preprocess, beginning at the start of the specified buffer.</param>
		public virtual void PreprocessBatch<T>(IDrawDevice device, BatchInfo material, ref VertexMode vertexMode, ref T[] vertexBuffer, ref int vertexCount) {}
		/// <summary>
		/// Sets up the appropriate OpenGL rendering state for this DrawTechnique.
		/// </summary>
		/// <param name="lastTechnique">The last DrawTechnique that has been set up. This parameter is optional, but
		/// specifying it will increase performance by reducing redundant state changes.</param>
		/// <param name="textures">A set of <see cref="Duality.Resources.Texture">Textures</see> to use.</param>
		/// <param name="uniforms">A set of <see cref="Duality.Resources.ShaderVarInfo">uniform values</see> to apply.</param>
		public void SetupForRendering(IDrawDevice device, BatchInfo material, DrawTechnique lastTechnique)
		{
			// Prepare Rendering
			if (this.NeedsPreparation)
			{
				// Clone the material, if not done yet due to vertex preprocessing
				if (!this.NeedsPreprocess) material = new BatchInfo(material);
				this.PrepareRendering(device, material);
			}
			
			// Setup BlendType
			if (lastTechnique == null || this.blendType != lastTechnique.blendType)
				this.SetupBlendType(this.blendType, device.DepthWrite);

			// Bind Shader
			ContentRef<ShaderProgram> selShader = this.SelectShader();
			if (lastTechnique == null || selShader.Res != lastTechnique.shader.Res)
				ShaderProgram.Bind(selShader);

			// Setup shader data
			if (selShader.IsAvailable)
			{
				ShaderVarInfo[] varInfo = selShader.Res.VarInfo;

				// Setup sampler bindings automatically
				int curSamplerIndex = 0;
				if (material.Textures != null)
				{
					for (int i = 0; i < varInfo.Length; i++)
					{
						if (varInfo[i].glVarLoc == -1) continue;
						if (varInfo[i].type != ShaderVarType.Sampler2D) continue;
						ContentRef<Texture> tex = material.GetTexture(varInfo[i].name);
						Texture.Bind(tex, curSamplerIndex);
						GL.Uniform1(varInfo[i].glVarLoc, curSamplerIndex);
						curSamplerIndex++;
					}
				}
				Texture.ResetBinding(curSamplerIndex);

				// Transfer uniform data from material to actual shader
				if (material.Uniforms != null)
				{
					for (int i = 0; i < varInfo.Length; i++)
					{
						if (varInfo[i].glVarLoc == -1) continue;
						float[] data = material.GetUniform(varInfo[i].name);
						if (data == null) continue;
						varInfo[i].SetupUniform(data);
					}
				}
			}
			// Setup fixed function data
			else
			{
				// Fixed function texture binding
				if (material.Textures != null)
				{
					int samplerIndex = 0;
					foreach (var pair in material.Textures)
					{
						Texture.Bind(pair.Value, samplerIndex);
						samplerIndex++;
					}
					Texture.ResetBinding(samplerIndex);
				}
				else
					Texture.ResetBinding();
			}
		}
		/// <summary>
		/// Resets the OpenGL rendering state after finishing DrawTechnique-Setups. Only call this when there are no more
		/// DrawTechniques to follow directly.
		/// </summary>
		public void FinishRendering()
		{
			this.SetupBlendType(BlendMode.Reset);
			ShaderProgram.Bind(ContentRef<ShaderProgram>.Null);
			Texture.ResetBinding();
		}

		/// <summary>
		/// Sets up OpenGL rendering state according to a certain <see cref="BlendMode"/>.
		/// </summary>
		/// <param name="mode">The BlendMode to set up.</param>
		/// <param name="depthWrite">Whether or not to allow writing depth values.</param>
		protected void SetupBlendType(BlendMode mode, bool depthWrite = true)
		{
			switch (mode)
			{
				default:
				case BlendMode.Reset:
				case BlendMode.Solid:
					GL.DepthMask(depthWrite);
					GL.Disable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					break;
				case BlendMode.Mask:
					GL.DepthMask(depthWrite);
					GL.Disable(EnableCap.Blend);
					if (MaskUseAlphaToCoverage)
					{
						GL.Disable(EnableCap.AlphaTest);
						GL.Enable(EnableCap.SampleAlphaToCoverage);
					}
					else
					{
						GL.Enable(EnableCap.AlphaTest);
						GL.AlphaFunc(AlphaFunction.Gequal, 0.5f);
					}
					break;
				case BlendMode.Alpha:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
					break;
				case BlendMode.Add:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One, BlendingFactorSrc.One, BlendingFactorDest.One);
					break;
				case BlendMode.Light:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.DstColor, BlendingFactorDest.One, BlendingFactorSrc.Zero, BlendingFactorDest.One);
					break;
				case BlendMode.Multiply:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.Zero);
					break;
				case BlendMode.Invert:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFunc(BlendingFactorSrc.OneMinusDstColor, BlendingFactorDest.OneMinusSrcColor);
					break;
			}
		}
		/// <summary>
		/// Dynamically selects the <see cref="Duality.Resources.ShaderProgram"/> to use. Just returns <see cref="Shader"/> by default.
		/// </summary>
		/// <returns>The selected <see cref="Duality.Resources.ShaderProgram"/>.</returns>
		protected virtual ContentRef<ShaderProgram> SelectShader()
		{
			return this.shader;
		}
		/// <summary>
		/// Prepares rendering using this DrawTechnique.
		/// </summary>
		/// <param name="device"></param>
		/// <param name="material"></param>
		protected virtual void PrepareRendering(IDrawDevice device, BatchInfo material) {}

		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			DrawTechnique c = r as DrawTechnique;
			c.blendType = this.blendType;
		}
	}
}
