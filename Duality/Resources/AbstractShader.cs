using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

using Duality.Drawing;
using Duality.Editor;
using Duality.Cloning;

using OpenTK.Graphics.OpenGL;
using ShaderType = Duality.Drawing.ShaderType;

namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL Shader in an abstract form.
	/// </summary>
	[ExplicitResourceReference()]
	public abstract class AbstractShader : Resource
	{
		private	string	source	= null;
		[DontSerialize] private	int				glShaderId	= 0;
		[DontSerialize] private	bool			compiled	= false;
		[DontSerialize] private	ShaderVarInfo[]	varInfo		= null;

		/// <summary>
		/// The type of OpenGL shader that is represented.
		/// </summary>
		protected abstract ShaderType Type { get; }
		/// <summary>
		/// [GET] The shaders OpenGL id.
		/// </summary>
		internal protected int OglShaderId
		{
			get { return this.glShaderId; }
		}
		/// <summary>
		/// [GET] Whether this shader has been compiled yet or not.
		/// </summary>
		public bool Compiled
		{
			get { return this.compiled; }
		}
		/// <summary>
		/// [GET] Information about the <see cref="ShaderVarInfo">variables</see> declared in the shader.
		/// </summary>
		public ShaderVarInfo[] VarInfo
		{
			get { return this.varInfo; }
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
			if (this.glShaderId == 0) this.glShaderId = GL.CreateShader(GetOpenTKShaderType(this.Type));
			GL.ShaderSource(this.glShaderId, this.source);
			GL.CompileShader(this.glShaderId);

			int result;
			GL.GetShader(this.glShaderId, ShaderParameter.CompileStatus, out result);
			if (result == 0)
			{
				string infoLog = GL.GetShaderInfoLog(this.glShaderId);
				Log.Core.WriteError("Error compiling {0}. InfoLog:\n{1}", this.Type, infoLog);
				return;
			}
			this.compiled = true;

			// Remove comments from source code before extracting variables
			string sourceWithoutComments;
			{
				const string blockComments = @"/\*(.*?)\*/";
				const string lineComments = @"//(.*?)\r?\n";
				const string strings = @"""((\\[^\n]|[^""\n])*)""";
				const string verbatimStrings = @"@(""[^""]*"")+";
				sourceWithoutComments = Regex.Replace(this.source,
					blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
					me => {
						if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
							return me.Value.StartsWith("//") ? Environment.NewLine : "";
						// Keep the literal strings
						return me.Value;
					},
					RegexOptions.Singleline);
			}

			// Scan remaining code chunk for variable declarations
			List<ShaderVarInfo> varInfoList = new List<ShaderVarInfo>();
			string[] lines = sourceWithoutComments.Split(new[] {';','\n'}, StringSplitOptions.RemoveEmptyEntries);
			ShaderVarInfo varInfo = new ShaderVarInfo();
			foreach (string t in lines)
			{
				string curLine = t.TrimStart();

				if (curLine.StartsWith("uniform"))
					varInfo.Scope = ShaderVarScope.Uniform;
				else if (curLine.StartsWith("attribute"))
					varInfo.Scope = ShaderVarScope.Attribute;
				else continue;

				string[] curLineSplit = curLine.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				switch (curLineSplit[1].ToUpper())
				{
					case "FLOAT":		varInfo.Type = ShaderVarType.Float; break;
					case "VEC2":		varInfo.Type = ShaderVarType.Vec2; break;
					case "VEC3":		varInfo.Type = ShaderVarType.Vec3; break;
					case "VEC4":		varInfo.Type = ShaderVarType.Vec4; break;
					case "MAT2":		varInfo.Type = ShaderVarType.Mat2; break;
					case "MAT3":		varInfo.Type = ShaderVarType.Mat3; break;
					case "MAT4":		varInfo.Type = ShaderVarType.Mat4; break;
					case "INT":			varInfo.Type = ShaderVarType.Int; break;
					case "SAMPLER2D":	varInfo.Type = ShaderVarType.Sampler2D; break;
				}

				curLineSplit = curLineSplit[2].Split(new char[] {'[', ']'}, StringSplitOptions.RemoveEmptyEntries);
				varInfo.Name = curLineSplit[0];
				varInfo.ArrayLength = (curLineSplit.Length > 1) ? int.Parse(curLineSplit[1]) : 1;
				varInfo.Handle = -1;

				varInfoList.Add(varInfo);
			}

			this.varInfo = varInfoList.ToArray();
		}

		protected override void OnLoaded()
		{
			this.Compile();
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.glShaderId != 0)
			{
				GL.DeleteShader(this.glShaderId);
				this.glShaderId = 0;
			}
		}
		protected override void OnCopyDataTo(object target, ICloneOperation operation)
		{
			base.OnCopyDataTo(target, operation);
			AbstractShader targetShader = target as AbstractShader;
			if (this.compiled) targetShader.Compile();
		}

		private static OpenTK.Graphics.OpenGL.ShaderType GetOpenTKShaderType(ShaderType type)
		{
			switch (type)
			{
				case ShaderType.Vertex:		return OpenTK.Graphics.OpenGL.ShaderType.VertexShader;
				case ShaderType.Fragment:	return OpenTK.Graphics.OpenGL.ShaderType.FragmentShader;
			}
			return OpenTK.Graphics.OpenGL.ShaderType.VertexShader;
		}
	}
}
