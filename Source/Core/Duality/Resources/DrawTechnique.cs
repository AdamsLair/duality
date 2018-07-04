using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Backend;
using Duality.Drawing;
using Duality.Editor;
using Duality.Cloning;
using Duality.Properties;

namespace Duality.Resources
{
	/// <summary>
	/// DrawTechniques represent the method by which a set of colors, <see cref="Duality.Resources.Texture">Textures</see> and
	/// vertex data is applied to screen. 
	/// </summary>
	/// <seealso cref="Duality.Resources.Material"/>
	/// <seealso cref="Duality.Resources.FragmentShader"/>
	/// <seealso cref="Duality.Resources.VertexShader"/>
	/// <seealso cref="Duality.Drawing.BlendMode"/>
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageDrawTechnique)]
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

		internal static void InitDefaultContent()
		{
			DefaultContent.InitType<DrawTechnique>(new Dictionary<string,DrawTechnique>
			{
				{ "Solid", new DrawTechnique(BlendMode.Solid) },
				{ "Mask", new DrawTechnique(BlendMode.Mask) },
				{ "Add", new DrawTechnique(BlendMode.Add) },
				{ "Alpha", new DrawTechnique(BlendMode.Alpha) },
				{ "Multiply", new DrawTechnique(BlendMode.Multiply) },
				{ "Light", new DrawTechnique(BlendMode.Light) },
				{ "Invert", new DrawTechnique(BlendMode.Invert) },

				{ "Picking", new DrawTechnique(BlendMode.Mask, VertexShader.Minimal, FragmentShader.Picking) },
				{ "SharpAlpha", new DrawTechnique(BlendMode.Alpha, VertexShader.Minimal, FragmentShader.SharpAlpha) }
			});
		}
		

		private BlendMode                  blendType         = BlendMode.Solid;
		private ContentRef<VertexShader>   vertexShader      = VertexShader.Minimal;
		private ContentRef<FragmentShader> fragmentShader    = FragmentShader.Minimal;
		private Type                       prefType          = null;

		[DontSerialize] private VertexDeclaration         prefFormat        = null;
		[DontSerialize] private ShaderParameterCollection defaultParameters = null;
		[DontSerialize] private INativeShaderProgram      nativeShader      = null;
		[DontSerialize] private bool                      compiled          = false;
		[DontSerialize] private ShaderFieldInfo[]         shaderFields      = null;


		/// <summary>
		/// [GET] The shaders native backend. Don't use this unless you know exactly what you're doing.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public INativeShaderProgram NativeShader
		{
			get
			{
				if (!this.compiled)
					this.Compile();
				return this.nativeShader;
			}
		}
		/// <summary>
		/// [GET] Returns whether the internal shader program of this <see cref="DrawTechnique"/> has been compiled.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool Compiled
		{
			get { return this.compiled; }
		}
		/// <summary>
		/// [GET] Returns an array containing information about the variables that have been declared in shader source code.
		/// May trigger compiling the technique, if it wasn't compiled already.
		/// </summary>
		public IReadOnlyList<ShaderFieldInfo> DeclaredFields
		{
			get
			{
				if (!this.compiled)
					this.Compile();
				return this.shaderFields;
			}
		}
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
		/// [GET / SET] The <see cref="Resources.VertexShader"/> that is used for rendering.
		/// </summary>
		public ContentRef<VertexShader> Vertex
		{
			get { return this.vertexShader; }
			set
			{
				this.vertexShader = value;
				this.compiled = false;
			}
		}
		/// <summary>
		/// [GET / SET] The <see cref="Resources.FragmentShader"/> that is used for rendering.
		/// </summary>
		public ContentRef<FragmentShader> Fragment
		{
			get { return this.fragmentShader; }
			set
			{
				this.fragmentShader = value;
				this.compiled = false;
			}
		}
		/// <summary>
		/// [GET] The set of default parameters that acts as a fallback in cases
		/// where a parameter has not been set by a <see cref="Material"/> or <see cref="BatchInfo"/>.
		/// 
		/// The result of this property should be treated as read-only.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public ShaderParameterCollection DefaultParameters
		{
			get
			{
				if (this.defaultParameters == null)
				{
					// Setup default values on demand - for now, just a few hardcoded ones
					this.defaultParameters = new ShaderParameterCollection();
					this.defaultParameters.Set(BuiltinShaderFields.MainColor, Vector4.One);
					this.defaultParameters.Set(BuiltinShaderFields.MainTex, Texture.White);
				}
				return this.defaultParameters;
			}
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
		/// Creates a new DrawTechnique using the specified <see cref="BlendMode"/> and shaders.
		/// </summary>
		/// <param name="blendType"></param>
		/// <param name="shader"></param>
		/// <param name="formatPref"></param>
		public DrawTechnique(BlendMode blendType, ContentRef<VertexShader> vertexShader, ContentRef<FragmentShader> fragmentShader) 
		{
			this.blendType = blendType;
			this.vertexShader = vertexShader;
			this.fragmentShader = fragmentShader;
		}

		/// <summary>
		/// Compiles the internal shader program of this <see cref="DrawTechnique"/>. This is 
		/// done automatically on load and only needs to be invoked manually when the technique
		/// or one of its shader dependencies changed.
		/// </summary>
		public void Compile()
		{
			Logs.Core.Write("Compiling DrawTechnique '{0}'...", this.FullName);
			Logs.Core.PushIndent();

			if (this.nativeShader == null)
				this.nativeShader = DualityApp.GraphicsBackend.CreateShaderProgram();

			// Create a list of all shader parts that we'll be linking
			List<Shader> parts = new List<Shader>();
			parts.Add(this.vertexShader.Res ?? VertexShader.Minimal.Res);
			parts.Add(this.fragmentShader.Res ?? FragmentShader.Minimal.Res);

			// Ensure all shader parts are compiled
			List<INativeShaderPart> nativeParts = new List<INativeShaderPart>();
			foreach (Shader part in parts)
			{
				if (!part.Compiled) part.Compile();
				nativeParts.Add(part.Native);
			}

			// Gather shader field declarations from all shader parts
			Dictionary<string, ShaderFieldInfo> fieldMap = new Dictionary<string, ShaderFieldInfo>();
			foreach (Shader part in parts)
			{
				foreach (ShaderFieldInfo field in part.DeclaredFields)
				{
					fieldMap[field.Name] = field;
				}
			}

			// Load the program with all shader parts attached
			try
			{
				this.shaderFields = fieldMap.Values.ToArray();
				this.nativeShader.LoadProgram(nativeParts, this.shaderFields);

				// Validate that we have at least one attribute in the shader. Warn otherwise.
				if (!this.shaderFields.Any(f => f.Scope == ShaderFieldScope.Attribute))
					Logs.Core.WriteWarning("The shader doesn't seem to define any vertex attributes. Is this intended?");
			}
			catch (Exception e)
			{
				this.shaderFields = new ShaderFieldInfo[0];
				Logs.Core.WriteError("Failed to compile DrawTechnique:{1}{0}", LogFormat.Exception(e), Environment.NewLine);
			}

			// Even if we failed, we tried to compile it. Don't do it again and again.
			this.compiled = true;
			Logs.Core.PopIndent();
		}

		protected override void OnLoaded()
		{
			this.Compile();
			base.OnLoaded();
			this.prefFormat = VertexDeclaration.Get(this.prefType);
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			if (this.nativeShader != null)
			{
				this.nativeShader.Dispose();
				this.nativeShader = null;
			}
		}
		protected override void OnCopyDataTo(object target, ICloneOperation operation)
		{
			base.OnCopyDataTo(target, operation);
			DrawTechnique targetTechnique = target as DrawTechnique;
			if (this.compiled) targetTechnique.Compile();
		}
	}
}
