using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

using Duality.Drawing;
using Duality.Editor;
using Duality.Cloning;
using Duality.Backend;


namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL Shader in an abstract form.
	/// </summary>
	[ExplicitResourceReference()]
	public abstract class AbstractShader : Resource
	{
		private	string source = null;
		[DontSerialize] private	INativeShaderPart	native		= null;
		[DontSerialize] private	bool				compiled	= false;
		[DontSerialize] private	ShaderFieldInfo[]	fields		= null;

		/// <summary>
		/// [GET] The shaders native backend. Don't use this unless you know exactly what you're doing.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public INativeShaderPart Native
		{
			get { return this.native; }
		}
		/// <summary>
		/// The type of OpenGL shader that is represented.
		/// </summary>
		protected abstract ShaderType Type { get; }
		/// <summary>
		/// [GET] Whether this shader has been compiled yet or not.
		/// </summary>
		public bool Compiled
		{
			get { return this.compiled; }
		}
		/// <summary>
		/// [GET] Information about the <see cref="ShaderFieldInfo">variables</see> declared in the shader.
		/// </summary>
		public ShaderFieldInfo[] Fields
		{
			get { return this.fields; }
		}
		/// <summary>
		/// [GET] The shaders source code.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public string Source
		{
			get { return this.source; }
			set
			{
				this.compiled = false;
				this.sourcePath = null;
				this.source = value;
			}
		}


		protected AbstractShader() {}
		protected AbstractShader(string sourceCode)
		{
			this.Source = sourceCode;
		}


		/// <summary>
		/// Loads new shader source code from the specified <see cref="System.IO.Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="System.IO.Stream"/> to read the source code from.</param>
		public void LoadSource(Stream stream)
		{
			StreamReader reader = new StreamReader(stream);

			this.compiled = false;
			this.sourcePath = null;
			this.source = reader.ReadToEnd();
		}
		/// <summary>
		/// Loads new shader source code from the specified file.
		/// </summary>
		/// <param name="filePath">The path of the file to read the source code from.</param>
		public void LoadSource(string filePath = null)
		{
			if (filePath == null) filePath = this.sourcePath;

			this.compiled = false;
			this.sourcePath = filePath;
			this.source = "";
			if (!File.Exists(this.sourcePath)) return;

			this.source = File.ReadAllText(this.sourcePath);
		}
		/// <summary>
		/// Saves the current shader source code to the specified file.
		/// </summary>
		/// <param name="filePath">The path of the file to write the source code to.</param>
		public void SaveSource(string filePath = null)
		{
			if (filePath == null) filePath = this.sourcePath;

			// We're saving this data for the first time
			if (!this.IsDefaultContent && this.sourcePath == null) this.sourcePath = filePath;

			if (this.source != null)
				File.WriteAllText(filePath, this.source);
			else
				File.WriteAllText(filePath, "");
		}

		/// <summary>
		/// Compiles the shader. This is done automatically when loading the shader
		/// or attaching it to a <see cref="Duality.Resources.ShaderProgram"/>.
		/// </summary>
		public void Compile()
		{
			DualityApp.GuardSingleThreadState();

			if (this.compiled) return;
			if (String.IsNullOrEmpty(this.source)) return;

			if (this.native == null) this.native = DualityApp.GraphicsBackend.CreateShaderPart();
			try
			{
				this.native.LoadSource(this.source, this.Type);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error loading Shader {0}:{2}{1}", this.FullName, Log.Exception(e), Environment.NewLine);
			}

			this.compiled = true;
			this.fields = this.native.GetFields();
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
			AbstractShader targetShader = target as AbstractShader;
			if (this.compiled) targetShader.Compile();
		}
	}
}
