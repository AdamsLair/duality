using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
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
		private static readonly Regex RegexMetadataDirective = new Regex(@"^\s*#pragma\s+duality\s+(.+)", RegexOptions.Compiled);
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

			// Note: The CompileShader call below might crash on Intel HD graphics cards
			// due to an Intel driver bug where an unknown pragma directive leads to an
			// access violation. No workaround for now, but keep in mind should this be
			// an issue at some point.
			// More info: https://software.intel.com/en-us/forums/graphics-driver-bug-reporting/topic/623485

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

			this.fields = this.ParseFields(sourceCode);
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

		private ShaderFieldInfo[] ParseFields(string sourceCode)
		{
			// Remove comments from source code before extracting variables
			string sourceWithoutComments = this.GetSourceWithoutComments(sourceCode);

			// Scan remaining code chunk for variable declarations
			List<ShaderFieldInfo> varInfoList = new List<ShaderFieldInfo>();
			List<string> fieldMetadata = new List<string>();
			using (StringReader reader = new StringReader(sourceWithoutComments))
			{
				while (true)
				{
					string line = reader.ReadLine();
					if (line == null) break;

					line = line.Trim();
					if (string.IsNullOrEmpty(line)) continue;

					string metadataDirective = this.ParseMetadataDirective(line);
					if (metadataDirective != null)
					{
						fieldMetadata.Add(metadataDirective);
						continue;
					}

					ShaderFieldInfo field = this.ParseFieldDeclaration(line, fieldMetadata);
					if (field != null)
					{
						varInfoList.Add(field);
						fieldMetadata.Clear();
						continue;
					}

					fieldMetadata.Clear();
				}
			}

			return varInfoList.ToArray();
		}
		private string ParseMetadataDirective(string line)
		{
			Match match = RegexMetadataDirective.Match(line);
			if (match == null || match.Length == 0)
				return null;
			else
				return match.Groups[1].Value;
		}
		private ShaderFieldInfo ParseFieldDeclaration(string line, IReadOnlyList<string> fieldMetadata)
		{
			line = line.TrimEnd(';');

			ShaderFieldScope scope;
			if (RegexUniformLine.IsMatch(line))
				scope = ShaderFieldScope.Uniform;
			else if (RegexAttributeLine.IsMatch(line))
				scope = ShaderFieldScope.Attribute;
			else
				return null;

			string[] lineToken = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string typeToken = lineToken[1];
			string nameToken = lineToken[2];
			ShaderFieldType varType = ShaderFieldType.Unknown;
			switch (typeToken.ToUpper())
			{
				case "FLOAT": varType = ShaderFieldType.Float; break;
				case "VEC2": varType = ShaderFieldType.Vec2; break;
				case "VEC3": varType = ShaderFieldType.Vec3; break;
				case "VEC4": varType = ShaderFieldType.Vec4; break;
				case "MAT2": varType = ShaderFieldType.Mat2; break;
				case "MAT3": varType = ShaderFieldType.Mat3; break;
				case "MAT4": varType = ShaderFieldType.Mat4; break;
				case "INT": varType = ShaderFieldType.Int; break;
				case "BOOL": varType = ShaderFieldType.Bool; break;
				case "SAMPLER2D": varType = ShaderFieldType.Sampler2D; break;
			}

			int arrayLength = 1;
			int arrayStart = nameToken.IndexOf('[');
			int arrayEnd = nameToken.IndexOf(']');
			if (arrayStart != -1 && arrayEnd != -1)
			{
				string arrayLengthToken = nameToken.Substring(arrayStart + 1, arrayEnd - arrayStart - 1).Trim();
				arrayLength = int.Parse(arrayLengthToken);
			}

			string name =
				(arrayStart == -1) ?
				nameToken :
				nameToken.Substring(0, arrayStart);

			// Parse field metadata for known properties
			string description = null;
			string editorTypeTag = null;
			float minValue = float.MinValue;
			float maxValue = float.MaxValue;
			const string unableToParseError = "Unable to parse shader field metadata property '{0}'. Ignoring value '{1}'";
			foreach (string metadata in fieldMetadata)
			{
				int propertyEnd = metadata.IndexOf(' ');
				if (propertyEnd == -1) continue;
				if (propertyEnd == metadata.Length - 1) continue;

				string property = metadata.Substring(0, propertyEnd);
				string value = metadata.Substring(propertyEnd + 1, metadata.Length - propertyEnd - 1);
				if (property == "description")
				{
					int descStart = value.IndexOf('"');
					int descEnd = value.LastIndexOf('"');
					if (descStart == -1 || descEnd == -1)
					{
						Logs.Core.WriteWarning(unableToParseError, property, value);
						continue;
					}
					description = value.Substring(descStart + 1, descEnd - descStart - 1);
				}
				else if (property == "editorType")
				{
					editorTypeTag = value.Trim();
				}
				else if (property == "minValue")
				{
					float parsedValue;
					if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedValue))
					{
						Logs.Core.WriteWarning(unableToParseError, property, value);
						continue;
					}
					minValue = parsedValue;
				}
				else if (property == "maxValue")
				{
					float parsedValue;
					if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedValue))
					{
						Logs.Core.WriteWarning(unableToParseError, property, value);
						continue;
					}
					maxValue = parsedValue;
				}
				else
				{
					Logs.Core.WriteWarning(
						"Unknown shader field metadata property '{0}'. Ignoring value '{1}'", 
						property, 
						value);
				}
			}

			return new ShaderFieldInfo(
				name, 
				varType, 
				scope,
				arrayLength,
				editorTypeTag,
				description,
				minValue,
				maxValue);
		}
		private string GetSourceWithoutComments(string sourceCode)
		{
			const string blockComments = @"/\*(.*?)\*/";
			const string lineComments = @"//(.*?)\r?\n";
			const string strings = @"""((\\[^\n]|[^""\n])*)""";
			const string verbatimStrings = @"@(""[^""]*"")+";
			return Regex.Replace(sourceCode,
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
