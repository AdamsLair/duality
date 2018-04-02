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
		private int handle;
		private ShaderType type;

		public int Handle
		{
			get { return this.handle; }
		}
		public ShaderType Type
		{
			get { return this.type; }
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
