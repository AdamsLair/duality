using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
		private static readonly Regex RegexUniformLine = new Regex(@"^(?:layout\s*\(.+\)|)\s*(uniform)\s.+", RegexOptions.Compiled);
		private static readonly Regex RegexAttributeLine = new Regex(@"^(?:layout\s*\(.+\)|)\s*(attribute|in)\s.+", RegexOptions.Compiled);


		private int handle;
		private ShaderType type;
		private ShaderFieldInfo[] fields;

		public int Handle
		{
			get { return this.handle; }
		}
		public ShaderType Type
		{
			get { return this.type; }
		}
		public ShaderFieldInfo[] Fields
		{
			get { return this.fields; }
		}

		void INativeShaderPart.LoadSource(string sourceCode, ShaderType type)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			this.type = type;
			if (this.handle == 0)
				this.handle = GL.CreateShader(GetOpenTKShaderType(type));
			GL.ShaderSource(this.handle, sourceCode);
			GL.CompileShader(this.handle);

			// Log all errors and warnings from the info log
			string infoLog = GL.GetShaderInfoLog(this.handle);
			if (!string.IsNullOrWhiteSpace(infoLog))
			{
				using (StringReader reader = new StringReader(infoLog))
				{
					while (true)
					{
						string line = reader.ReadLine();
						if (line == null) break;
						if (string.IsNullOrWhiteSpace(line)) continue;

						if (line.IndexOf("warning", StringComparison.InvariantCultureIgnoreCase) != -1)
							Logs.Core.WriteWarning("{0}", line);
						else if (line.IndexOf("error", StringComparison.InvariantCultureIgnoreCase) != -1)
							Logs.Core.WriteError("{0}", line);
						else
							Logs.Core.Write("{0}", line);
					}
				}
			}

			// If compilation failed, throw an exception
			int result;
			GL.GetShader(this.handle, ShaderParameter.CompileStatus, out result);
			if (result == 0)
			{
				throw new BackendException(string.Format("Failed to compile {0} shader:{2}{1}", type, infoLog, Environment.NewLine));
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
				if (RegexUniformLine.IsMatch(curLine))
					scope = ShaderFieldScope.Uniform;
				else if (RegexAttributeLine.IsMatch(curLine))
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

				int arrayLength;
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
