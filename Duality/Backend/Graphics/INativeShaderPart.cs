using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Resources;
using Duality.Drawing;

namespace Duality.Backend
{
	public interface INativeShaderPart : IDisposable
	{
		/// <summary>
		/// Loads the specified source code and prepares the shader part for being used.
		/// </summary>
		/// <param name="sourceCode"></param>
		/// <param name="type"></param>
		void LoadSource(string sourceCode, ShaderType type);

		/// <summary>
		/// Retrieves reflection data on the shaders fields.
		/// </summary>
		/// <returns></returns>
		ShaderVarInfo[] GetFields();
	}
}
