using System;
using System.Linq;

using Duality.EditorHints;

using OpenTK.Graphics.OpenGL;

namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL ShaderProgram which consists of a Vertex- and a FragmentShader
	/// </summary>
	/// <seealso cref="Duality.Resources.AbstractShader"/>
	/// <seealso cref="Duality.Resources.VertexShader"/>
	/// <seealso cref="Duality.Resources.FragmentShader"/>
	[Serializable]
	[ExplicitResourceReference(typeof(AbstractShader))]
	public class ShaderProgram : Resource
	{
		/// <summary>
		/// A ShaderProgram resources file extension.
		/// </summary>
		public new const string FileExt = ".ShaderProgram" + Resource.FileExt;

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
		/// The SmoothAnim ShaderProgram, using a <see cref="Duality.Resources.VertexShader.SmoothAnim"/> VertexShader and
		/// a <see cref="Duality.Resources.FragmentShader.SmoothAnim"/> FragmentShader. Some <see cref="Duality.Components.Renderer">Renderers</see>
		/// might react automatically to <see cref="Duality.Resources.Material">Materials</see> using this ShaderProgram and provide a suitable
		/// vertex format.
		/// </summary>
		public static ContentRef<ShaderProgram> SmoothAnim	{ get; private set; }
		/// <summary>
		/// The SharpMask ShaderProgram, using a <see cref="Duality.Resources.VertexShader.Minimal"/> VertexShader and
		/// a <see cref="Duality.Resources.FragmentShader.SharpAlpha"/> FragmentShader.
		/// </summary>
		public static ContentRef<ShaderProgram> SharpAlpha	{ get; private set; }

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath		= ContentProvider.VirtualContentPath + "ShaderProgram:";
			const string ContentPath_Minimal	= VirtualContentPath + "Minimal";
			const string ContentPath_Picking	= VirtualContentPath + "Picking";
			const string ContentPath_SmoothAnim	= VirtualContentPath + "SmoothAnim";
			const string ContentPath_SharpMask	= VirtualContentPath + "SharpAlpha";

			ContentProvider.RegisterContent(ContentPath_Minimal, new ShaderProgram(VertexShader.Minimal, FragmentShader.Minimal));
			ContentProvider.RegisterContent(ContentPath_Picking, new ShaderProgram(VertexShader.Minimal, FragmentShader.Picking));
			ContentProvider.RegisterContent(ContentPath_SmoothAnim, new ShaderProgram(VertexShader.SmoothAnim, FragmentShader.SmoothAnim));
			ContentProvider.RegisterContent(ContentPath_SharpMask, new ShaderProgram(VertexShader.Minimal, FragmentShader.SharpAlpha));

			Minimal		= ContentProvider.RequestContent<ShaderProgram>(ContentPath_Minimal);
			Picking		= ContentProvider.RequestContent<ShaderProgram>(ContentPath_Picking);
			SmoothAnim	= ContentProvider.RequestContent<ShaderProgram>(ContentPath_SmoothAnim);
			SharpAlpha	= ContentProvider.RequestContent<ShaderProgram>(ContentPath_SharpMask);
		}
		
		/// <summary>
		/// Refers to a null reference ShaderProgram.
		/// </summary>
		/// <seealso cref="ContentRef{T}.Null"/>
		public static readonly ContentRef<ShaderProgram> None	= ContentRef<ShaderProgram>.Null;
		private	static	ShaderProgram	curBound	= null;
		/// <summary>
		/// [GET] Returns the currently bound ShaderProgram.
		/// </summary>
		public static ContentRef<ShaderProgram> BoundProgram
		{
			get { return new ContentRef<ShaderProgram>(curBound); }
		}

		/// <summary>
		/// Binds a ShaderProgram in order to use it.
		/// </summary>
		/// <param name="prog">The ShaderProgram to be bound.</param>
		public static void Bind(ContentRef<ShaderProgram> prog)
		{
			ShaderProgram progRes = prog.IsExplicitNull ? null : prog.Res;
			if (curBound == progRes) return;

			if (progRes == null)
			{
				GL.UseProgram(0);
				curBound = null;
			}
			else
			{
				if (!progRes.compiled) progRes.Compile();

				if (progRes.glProgramId == 0)	throw new ArgumentException("Specified shader program has no valid OpenGL program Id! Maybe it hasn't been loaded / initialized properly?", "prog");
				if (progRes.Disposed)			throw new ArgumentException("Specified shader program has already been deleted!", "prog");
					
				GL.UseProgram(progRes.glProgramId);
				curBound = progRes;
			}
		}


		private	ContentRef<VertexShader>	vert	= VertexShader.Minimal;
		private	ContentRef<FragmentShader>	frag	= FragmentShader.Minimal;
		[NonSerialized] private	int				glProgramId	= 0;
		[NonSerialized] private bool			compiled	= false;
		[NonSerialized] private	ShaderVarInfo[]	varInfo		= null;

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
		public ShaderVarInfo[] VarInfo
		{
			get { return this.varInfo; }
		}
		/// <summary>
		/// [GET] Returns the number of vertex attributes that have been declared.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int AttribCount
		{
			get { return this.varInfo != null ? this.varInfo.Count(v => v.scope == ShaderVarScope.Attribute) : 0; }
		}
		/// <summary>
		/// [GET] Returns the number of uniform variables that have been declared.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int UniformCount
		{
			get { return this.varInfo != null ? this.varInfo.Count(v => v.scope == ShaderVarScope.Uniform) : 0; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="VertexShader"/> that is used by this ShaderProgram.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public ContentRef<VertexShader> Vertex
		{
			get { return this.vert; }
			set { this.AttachShaders(value, this.frag); this.Compile(); }
		}
		/// <summary>
		/// [GET / SET] The <see cref="FragmentShader"/> that is used by this ShaderProgram.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public ContentRef<FragmentShader> Fragment
		{
			get { return this.frag; }
			set { this.AttachShaders(this.vert, value); this.Compile(); }
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
			this.AttachShaders(v, f);
			this.Compile();
		}

		/// <summary>
		/// Re-Attaches the currently used <see cref="VertexShader">Vertex-</see> and <see cref="FragmentShader"/>.
		/// </summary>
		public void AttachShaders()
		{
			this.AttachShaders(this.vert, this.frag);
		}
		/// <summary>
		/// Attaches the specified <see cref="VertexShader">Vertex-</see> and <see cref="FragmentShader"/> to this ShaderProgram.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="f"></param>
		public void AttachShaders(ContentRef<VertexShader> v, ContentRef<FragmentShader> f)
		{
			DualityApp.GuardSingleThreadState();

			if (this.glProgramId == 0)	this.glProgramId = GL.CreateProgram();
			else						this.DetachShaders();

			this.compiled = false;
			this.vert = v;
			this.frag = f;

			// Assure both shaders are compiled
			if (this.vert.IsAvailable) this.vert.Res.Compile();
			if (this.frag.IsAvailable) this.frag.Res.Compile();
			
			// Attach both shaders
			if (this.vert.IsAvailable) GL.AttachShader(this.glProgramId, this.vert.Res.OglShaderId);
			if (this.frag.IsAvailable) GL.AttachShader(this.glProgramId, this.frag.Res.OglShaderId);
		}
		/// <summary>
		/// Detaches <see cref="VertexShader">Vertex-</see> and <see cref="FragmentShader"/> from the ShaderProgram.
		/// </summary>
		public void DetachShaders()
		{
			if (this.glProgramId == 0) return;
			this.compiled = false;

			// Determine currently attached shaders
			int[] attachedShaders = new int[10];
			int attachCount = 0;
			unsafe
			{
				GL.GetAttachedShaders(this.glProgramId, attachedShaders.Length, &attachCount, attachedShaders);
			}

			// Detach all attached shaders
			for (int i = 0; i < attachCount; i++)
			{
				GL.DetachShader(this.glProgramId, attachedShaders[i]);
			}
		}

		/// <summary>
		/// Compiles the ShaderProgram. This is done automatically when loading the ShaderProgram
		/// or when binding it.
		/// </summary>
		public void Compile()
		{
			DualityApp.GuardSingleThreadState();

			if (this.glProgramId == 0) return;
			if (this.compiled) return;

			GL.LinkProgram(this.glProgramId);

			int result;
			GL.GetProgram(this.glProgramId, ProgramParameter.LinkStatus, out result);
			if (result == 0)
			{
				string infoLog = GL.GetProgramInfoLog(this.glProgramId);
				Log.Core.WriteError("Error linking shader program. InfoLog:{1}{0}", infoLog, Environment.NewLine);
				return;
			}
			this.compiled = true;

			// Collect variable infos from sub programs
			ShaderVarInfo[] fragVarArray = this.frag.IsAvailable ? this.frag.Res.VarInfo : null;
			ShaderVarInfo[] vertVarArray = this.vert.IsAvailable ? this.vert.Res.VarInfo : null;
			if (fragVarArray != null && vertVarArray != null)
				this.varInfo = vertVarArray.Union(fragVarArray).ToArray();
			else if (vertVarArray != null)
				this.varInfo = vertVarArray.ToArray();
			else
				this.varInfo = fragVarArray.ToArray();
			// Determine actual variable locations
			for (int i = 0; i < this.varInfo.Length; i++)
			{
				if (this.varInfo[i].scope == ShaderVarScope.Uniform)
					this.varInfo[i].glVarLoc = GL.GetUniformLocation(this.glProgramId, this.varInfo[i].name);
				else
					this.varInfo[i].glVarLoc = GL.GetAttribLocation(this.glProgramId, this.varInfo[i].name);
			}
		}

		protected override void OnLoaded()
		{
			this.AttachShaders();
			this.Compile();
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.glProgramId != 0)
			{
				this.DetachShaders();
				GL.DeleteProgram(this.glProgramId);
				this.glProgramId = 0;
			}
		}

		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			ShaderProgram c = r as ShaderProgram;
			c.AttachShaders(this.vert, this.frag);
			if (this.compiled) c.Compile();
		}
	}
}
