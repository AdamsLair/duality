using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Properties;
using Duality.Cloning;
using Duality.Backend;

namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL ShaderProgram which consists of a Vertex- and a FragmentShader
	/// </summary>
	/// <seealso cref="Duality.Resources.AbstractShader"/>
	/// <seealso cref="Duality.Resources.VertexShader"/>
	/// <seealso cref="Duality.Resources.FragmentShader"/>
	[ExplicitResourceReference(typeof(AbstractShader))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageShaderProgram)]
	public class ShaderProgram : Resource
	{
		/// <summary>
		/// A minimal ShaderProgram, using a <see cref="Duality.Resources.VertexShader.Minimal"/> VertexShader and
		/// a <see cref="Duality.Resources.FragmentShader.Minimal"/> FragmentShader.
		/// </summary>
		public static ContentRef<ShaderProgram> Minimal		{ get; private set; }
		/// <summary>
		/// A ShaderProgram designed for picking operations. It uses a 
		/// <see cref="Duality.Resources.VertexShader.Minimal"/> VertexShader and a 
		/// <see cref="Duality.Resources.FragmentShader.Picking"/> FragmentShader.
		/// </summary>
		public static ContentRef<ShaderProgram> Picking		{ get; private set; }
		/// <summary>
		/// The SharpMask ShaderProgram, using a <see cref="Duality.Resources.VertexShader.Minimal"/> VertexShader and
		/// a <see cref="Duality.Resources.FragmentShader.SharpAlpha"/> FragmentShader.
		/// </summary>
		public static ContentRef<ShaderProgram> SharpAlpha	{ get; private set; }

		internal static void InitDefaultContent()
		{
			InitDefaultContent<ShaderProgram>(new Dictionary<string,ShaderProgram>
			{
				{ "Minimal", new ShaderProgram(VertexShader.Minimal, FragmentShader.Minimal) },
				{ "Picking", new ShaderProgram(VertexShader.Minimal, FragmentShader.Picking) },
				{ "SharpAlpha", new ShaderProgram(VertexShader.Minimal, FragmentShader.SharpAlpha) }
			});
		}


		private	ContentRef<VertexShader>	vert	= VertexShader.Minimal;
		private	ContentRef<FragmentShader>	frag	= FragmentShader.Minimal;
		[DontSerialize] private	INativeShaderProgram	native		= null;
		[DontSerialize] private bool					compiled	= false;
		[DontSerialize] private	ShaderFieldInfo[]		fields		= null;
		
		/// <summary>
		/// [GET] The shaders native backend. Don't use this unless you know exactly what you're doing.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public INativeShaderProgram Native
		{
			get
			{
				if (!this.compiled)
					this.Compile();
				return this.native;
			}
		}
		/// <summary>
		/// [GET] Returns whether this ShaderProgram has been compiled.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool Compiled
		{
			get { return this.compiled; }
		}
		/// <summary>
		/// [GET] Returns an array containing information about the variables that have been declared in shader source code.
		/// </summary>
		public ShaderFieldInfo[] Fields
		{
			get
			{
				if (!this.compiled)
					this.Compile();
				return this.fields;
			}
		}
		/// <summary>
		/// [GET / SET] The <see cref="VertexShader"/> that is used by this ShaderProgram.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public ContentRef<VertexShader> Vertex
		{
			get { return this.vert; }
			set
			{
				this.vert = value;
				this.compiled = false;
			}
		}
		/// <summary>
		/// [GET / SET] The <see cref="FragmentShader"/> that is used by this ShaderProgram.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public ContentRef<FragmentShader> Fragment
		{
			get { return this.frag; }
			set
			{
				this.frag = value;
				this.compiled = false;
			}
		}

		/// <summary>
		/// Creates a new, empty ShaderProgram.
		/// </summary>
		public ShaderProgram() : this(VertexShader.Minimal, FragmentShader.Minimal) {}
		/// <summary>
		/// Creates a new ShaderProgram based on a <see cref="VertexShader">Vertex-</see> and a <see cref="FragmentShader"/>.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="f"></param>
		public ShaderProgram(ContentRef<VertexShader> v, ContentRef<FragmentShader> f)
		{
			this.vert = v;
			this.frag = f;
		}

		/// <summary>
		/// Compiles the ShaderProgram. This is done automatically when loading the ShaderProgram
		/// or when binding it.
		/// </summary>
		public void Compile()
		{
			Logs.Core.Write("Compiling ShaderProgram '{0}'...", this.FullName);
			Logs.Core.PushIndent();

			if (this.native == null)
				this.native = DualityApp.GraphicsBackend.CreateShaderProgram();

			// Create a list of all shader parts that we'll be linking
			List<AbstractShader> parts = new List<AbstractShader>();
			parts.Add(this.vert.Res ?? VertexShader.Minimal.Res);
			parts.Add(this.frag.Res ?? FragmentShader.Minimal.Res);

			// Ensure all shader parts are compiled
			List<INativeShaderPart> nativeParts = new List<INativeShaderPart>();
			foreach (AbstractShader part in parts)
			{
				this.CompileIfRequired(part);
				nativeParts.Add(part.Native);
			}

			// Load the program with all shader parts attached
			try
			{
				this.native.LoadProgram(nativeParts);
				this.fields = this.native.GetFields();

				// Validate that we have at least one attribute in the shader. Warn otherwise.
				if (!this.fields.Any(f => f.Scope == ShaderFieldScope.Attribute))
					Logs.Core.WriteWarning("The shader doesn't seem to define any vertex attributes. Is this intended?");
			}
			catch (Exception e)
			{
				this.fields = new ShaderFieldInfo[0];
				Logs.Core.WriteError("Failed to compile ShaderProgram:{1}{0}", LogFormat.Exception(e), Environment.NewLine);
			}

			// Even if we failed, we tried to compile it. Don't do it again and again.
			this.compiled = true;
			Logs.Core.PopIndent();
		}

		private void CompileIfRequired(AbstractShader part)
		{
			if (part == null) return;  // Shader not available? No need to compile.
			if (part.Compiled) return; // Shader already compiled? No need to compile.
			part.Compile();
		}

		protected override void OnLoaded()
		{
			this.Compile();
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			if (this.native != null)
			{
				this.native.Dispose();
				this.native = null;
			}
		}
		protected override void OnCopyDataTo(object target, ICloneOperation operation)
		{
			base.OnCopyDataTo(target, operation);
			ShaderProgram targetShader = target as ShaderProgram;
			if (this.compiled) targetShader.Compile();
		}
	}
}
