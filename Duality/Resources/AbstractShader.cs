using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

using Duality.EditorHints;

using OpenTK.Graphics.OpenGL;

namespace Duality.Resources
{
	/// <summary>
	/// The type of a <see cref="AbstractShader">shader</see> variable.
	/// </summary>
	public enum ShaderVarType
	{
		/// <summary>
		/// Unknown type.
		/// </summary>
		Unknown = -1,

		/// <summary>
		/// A <see cref="System.Int32"/> variable.
		/// </summary>
		Int,
		/// <summary>
		/// A <see cref="System.Single"/> variable.
		/// </summary>
		Float,

		/// <summary>
		/// A two-dimensional vector with <see cref="System.Single"/> precision.
		/// </summary>
		Vec2,
		/// <summary>
		/// A three-dimensional vector with <see cref="System.Single"/> precision.
		/// </summary>
		Vec3,
		/// <summary>
		/// A four-dimensional vector with <see cref="System.Single"/> precision.
		/// </summary>
		Vec4,
		
		/// <summary>
		/// A 2x2 matrix with <see cref="System.Single"/> precision.
		/// </summary>
		Mat2,
		/// <summary>
		/// A 3x3 matrix with <see cref="System.Single"/> precision.
		/// </summary>
		Mat3,
		/// <summary>
		/// A 4x4 matrix with <see cref="System.Single"/> precision.
		/// </summary>
		Mat4,
		
		/// <summary>
		/// Represents a texture binding and provides lookups.
		/// </summary>
		Sampler2D
	}

	/// <summary>
	/// The scope of a <see cref="AbstractShader">shader</see> variable
	/// </summary>
	public enum ShaderVarScope
	{
		/// <summary>
		/// Unknown scope
		/// </summary>
		Unknown = -1,

		/// <summary>
		/// It is a uniform variable, i.e. constant during all rendering stages
		/// and set once per <see cref="Duality.Resources.BatchInfo">draw batch</see>.
		/// </summary>
		Uniform,
		/// <summary>
		/// It is a vertex attribute, i.e. defined for each vertex separately.
		/// </summary>
		Attribute
	}
	
	/// <summary>
	/// Provides information about a <see cref="AbstractShader">shader</see> variable.
	/// </summary>
	public struct ShaderVarInfo
	{
		/// <summary>
		/// The default variable name for a materials main texture.
		/// </summary>
		public const string VarName_MainTex = "mainTex";

		/// <summary>
		/// The <see cref="ShaderVarScope">scope</see> of the variable
		/// </summary>
		public	ShaderVarScope	scope;
		/// <summary>
		/// The <see cref="ShaderVarType">type</see> of the variable
		/// </summary>
		public	ShaderVarType	type;
		/// <summary>
		/// If the variable is an array, this is its length. Arrays
		/// are only supported for <see cref="ShaderVarType.Int"/> and
		/// <see cref="ShaderVarType.Float"/>.
		/// </summary>
		public	int				arraySize;
		/// <summary>
		/// The name of the variable, as declared in the shader.
		/// </summary>
		public	string			name;
		/// <summary>
		/// OpenGL handle of the variables memory location.
		/// </summary>
		public	int				glVarLoc;

		/// <summary>
		/// [GET] Returns whether the shader variable will be visible in the editor.
		/// </summary>
		public bool IsEditorVisible
		{
			get { return !string.IsNullOrEmpty(this.name) && this.name[0] != '_'; }
		}

		/// <summary>
		/// Assigns the specified data to the OpenGL uniform represented by this <see cref="ShaderVarInfo"/>.
		/// </summary>
		/// <param name="data">Incoming uniform data.</param>
		public void SetupUniform(float[] data)
		{
			if (this.scope != ShaderVarScope.Uniform) return;
			if (this.glVarLoc == -1) return;
			switch (this.type)
			{
				case ShaderVarType.Int:
					int[] arrI = new int[this.arraySize];
					for (int j = 0; j < arrI.Length; j++) arrI[j] = (int)data[j];
					GL.Uniform1(this.glVarLoc, arrI.Length, arrI);
					break;
				case ShaderVarType.Float:
					GL.Uniform1(this.glVarLoc, data.Length, data);
					break;
				case ShaderVarType.Vec2:
					GL.Uniform2(this.glVarLoc, data.Length / 2, data);
					break;
				case ShaderVarType.Vec3:
					GL.Uniform3(this.glVarLoc, data.Length / 3, data);
					break;
				case ShaderVarType.Vec4:
					GL.Uniform4(this.glVarLoc, data.Length / 4, data);
					break;
				case ShaderVarType.Mat2:
					GL.UniformMatrix2(this.glVarLoc, data.Length / 4, false, data);
					break;
				case ShaderVarType.Mat3:
					GL.UniformMatrix3(this.glVarLoc, data.Length / 9, false, data);
					break;
				case ShaderVarType.Mat4:
					GL.UniformMatrix4(this.glVarLoc, data.Length / 16, false, data);
					break;
			}
		}
		/// <summary>
		/// Initializes a uniform dataset based on the type of the represented variable.
		/// </summary>
		/// <returns>A new uniform dataset</returns>
		public float[] InitUniformData()
		{
			switch (this.type)
			{
				case ShaderVarType.Int:
					return new float[this.arraySize];
				case ShaderVarType.Float:
					return new float[this.arraySize];
				case ShaderVarType.Vec2:
					return new float[2 * this.arraySize];
				case ShaderVarType.Vec3:
					return new float[3 * this.arraySize];
				case ShaderVarType.Vec4:
					return new float[4 * this.arraySize];
				case ShaderVarType.Mat2:
					return new float[4 * this.arraySize];
				case ShaderVarType.Mat3:
					return new float[9 * this.arraySize];
				case ShaderVarType.Mat4:
					return new float[16 * this.arraySize];
			}
			return null;
		}

		public override string ToString()
		{
			return string.Format("{1} {0}{2}", 
				this.name, 
				this.type, 
				this.arraySize > 1 ? string.Format("[{0}]", this.arraySize) : "");
		}
	}

	/// <summary>
	/// Represents an OpenGL Shader in an abstract form.
	/// </summary>
	[Serializable]
	[ExplicitResourceReference()]
	public abstract class AbstractShader : Resource
	{
		private	string	source		= null;
		[NonSerialized] private	int				glShaderId	= 0;
		[NonSerialized] private	bool			compiled	= false;
		[NonSerialized] private	ShaderVarInfo[]	varInfo		= null;

		/// <summary>
		/// The type of OpenGL shader that is represented.
		/// </summary>
		protected abstract ShaderType OglShaderType { get; }
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
			if (this.glShaderId == 0) this.glShaderId = GL.CreateShader(this.OglShaderType);
			GL.ShaderSource(this.glShaderId, this.source);
			GL.CompileShader(this.glShaderId);

			int result;
			GL.GetShader(this.glShaderId, ShaderParameter.CompileStatus, out result);
			if (result == 0)
			{
				string infoLog = GL.GetShaderInfoLog(this.glShaderId);
				Log.Core.WriteError("Error compiling {0}. InfoLog:\n{1}", this.OglShaderType, infoLog);
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
					varInfo.scope = ShaderVarScope.Uniform;
				else if (curLine.StartsWith("attribute"))
					varInfo.scope = ShaderVarScope.Attribute;
				else continue;

				string[] curLineSplit = curLine.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				switch (curLineSplit[1].ToUpper())
				{
					case "FLOAT":		varInfo.type = ShaderVarType.Float; break;
					case "VEC2":		varInfo.type = ShaderVarType.Vec2; break;
					case "VEC3":		varInfo.type = ShaderVarType.Vec3; break;
					case "VEC4":		varInfo.type = ShaderVarType.Vec4; break;
					case "MAT2":		varInfo.type = ShaderVarType.Mat2; break;
					case "MAT3":		varInfo.type = ShaderVarType.Mat3; break;
					case "MAT4":		varInfo.type = ShaderVarType.Mat4; break;
					case "INT":			varInfo.type = ShaderVarType.Int; break;
					case "SAMPLER2D":	varInfo.type = ShaderVarType.Sampler2D; break;
				}

				curLineSplit = curLineSplit[2].Split(new char[] {'[', ']'}, StringSplitOptions.RemoveEmptyEntries);
				varInfo.name = curLineSplit[0];
				varInfo.arraySize = (curLineSplit.Length > 1) ? int.Parse(curLineSplit[1]) : 1;
				varInfo.glVarLoc = -1;

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

		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			AbstractShader c = r as AbstractShader;
			c.source		= this.source;
			c.sourcePath	= null;
			if (this.compiled) c.Compile();
		}
	}
}
