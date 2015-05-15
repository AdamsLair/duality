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

		void INativeShaderPart.LoadSource(string sourceCode, ShaderType type)
		{
			DualityApp.GuardSingleThreadState();

			if (this.handle == 0) this.handle = GL.CreateShader(GetOpenTKShaderType(type));
			GL.ShaderSource(this.handle, sourceCode);
			GL.CompileShader(this.handle);

			int result;
			GL.GetShader(this.handle, ShaderParameter.CompileStatus, out result);
			if (result == 0)
			{
				string infoLog = GL.GetShaderInfoLog(this.handle);
				Log.Core.WriteError("Error compiling {0} shader. InfoLog:\n{1}", type, infoLog);
				return;
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
			ShaderFieldInfo varInfo = new ShaderFieldInfo();
			foreach (string t in lines)
			{
				string curLine = t.TrimStart();

				if (curLine.StartsWith("uniform"))
					varInfo.Scope = ShaderFieldScope.Uniform;
				else if (curLine.StartsWith("attribute"))
					varInfo.Scope = ShaderFieldScope.Attribute;
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

			this.fields = varInfoList.ToArray();
		}
		ShaderFieldInfo[] INativeShaderPart.GetFields()
		{
			return this.fields.Clone() as ShaderFieldInfo[];
		}
		void IDisposable.Dispose()
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.handle != 0)
			{
				DualityApp.GuardSingleThreadState();
				GL.DeleteShader(this.handle);
				this.handle = 0;
			}
		}

		private static GLShaderType GetOpenTKShaderType(ShaderType type)
		{
			switch (type)
			{
				case ShaderType.Vertex:		return GLShaderType.VertexShader;
				case ShaderType.Fragment:	return GLShaderType.FragmentShader;
			}
			return GLShaderType.VertexShader;
		}
	}
}
