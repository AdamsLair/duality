using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Duality.Drawing;
using Duality.Resources;

using OpenTK.Graphics.OpenGL;
using ShaderType = Duality.Resources.ShaderType;
using GLShaderType = OpenTK.Graphics.OpenGL.ShaderType;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeShaderPart : INativeShaderPart
	{
		private int handle;
		private ShaderFieldInfo[] fields;

		public int Handle
		{
			get { return this.handle; }
		}
		public ShaderFieldInfo[] Fields
		{
			get { return this.fields; }
		}

		void INativeShaderPart.LoadSource(string sourceCode, ShaderType type)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			if (this.handle == 0) this.handle = GL.CreateShader(GetOpenTKShaderType(type));
			GL.ShaderSource(this.handle, sourceCode);
			GL.CompileShader(this.handle);

			int result;
			GL.GetShader(this.handle, ShaderParameter.CompileStatus, out result);
			if (result == 0)
			{
				string infoLog = GL.GetShaderInfoLog(this.handle);
				throw new BackendException(string.Format("{0} Compiler error:{2}{1}", type, infoLog, Environment.NewLine));
			}

			// Remove comments from source code before extracting variables
			string sourceWithoutComments;
			{
				const string blockComments = @"/\*(.*?)\*/";
				const string lineComments = @"//(.*?)\r?\n";
				const string strings = @"""((\\[^\n]|[^""\n])*)""";
				const string verbatimStrings = @"@(""[^""]*"")+";
				sourceWithoutComments = Regex.Replace(sourceCode,
					blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
					match =>
					{
						if (match.Value.StartsWith("/*") || match.Value.StartsWith("//"))
							return match.Value.StartsWith("//") ? Environment.NewLine : "";
						else
							return match.Value;
					},
					RegexOptions.Singleline);
			}

			// Scan remaining code chunk for variable declarations
			List<ShaderFieldInfo> varInfoList = new List<ShaderFieldInfo>();
			string[] lines = sourceWithoutComments.Split(new[] {';','\n'}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string t in lines)
			{
				string curLine = t.TrimStart();

				ShaderFieldScope scope;
				int arrayLength;

				if (curLine.StartsWith("uniform"))
					scope = ShaderFieldScope.Uniform;
				else if (curLine.StartsWith("attribute"))
					scope = ShaderFieldScope.Attribute;
				else continue;

				string[] curLineSplit = curLine.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				ShaderFieldType varType = ShaderFieldType.Unknown;
				switch (curLineSplit[1].ToUpper())
				{
					case "FLOAT":     varType = ShaderFieldType.Float; break;
					case "VEC2":      varType = ShaderFieldType.Vec2; break;
					case "VEC3":      varType = ShaderFieldType.Vec3; break;
					case "VEC4":      varType = ShaderFieldType.Vec4; break;
					case "MAT2":      varType = ShaderFieldType.Mat2; break;
					case "MAT3":      varType = ShaderFieldType.Mat3; break;
					case "MAT4":      varType = ShaderFieldType.Mat4; break;
					case "INT":       varType = ShaderFieldType.Int; break;
					case "BOOL":      varType = ShaderFieldType.Bool; break;
					case "SAMPLER2D": varType = ShaderFieldType.Sampler2D; break;
				}

				curLineSplit = curLineSplit[2].Split(new char[] {'[', ']'}, StringSplitOptions.RemoveEmptyEntries);
				arrayLength = (curLineSplit.Length > 1) ? int.Parse(curLineSplit[1]) : 1;

				varInfoList.Add(new ShaderFieldInfo(curLineSplit[0], varType, scope, arrayLength));
			}

			this.fields = varInfoList.ToArray();
		}
		void IDisposable.Dispose()
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.handle != 0)
			{
				DefaultOpenTKBackendPlugin.GuardSingleThreadState();
				GL.DeleteShader(this.handle);
				this.handle = 0;
			}
		}

		private static GLShaderType GetOpenTKShaderType(ShaderType type)
		{
			switch (type)
			{
				case ShaderType.Vertex:   return GLShaderType.VertexShader;
				case ShaderType.Fragment: return GLShaderType.FragmentShader;
			}
			return GLShaderType.VertexShader;
		}
	}
}
