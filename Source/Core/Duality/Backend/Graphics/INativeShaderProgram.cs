using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Resources;
using Duality.Drawing;

namespace Duality.Backend
{
	public interface INativeShaderProgram : IDisposable
	{
		/// <summary>
		/// Loads the specified shader parts and compiles them into a single shader program.
		/// </summary>
		/// <param name="shaderParts"></param>
		/// <param name="shaderFields"></param>
		void LoadProgram(IEnumerable<INativeShaderPart> shaderParts, IEnumerable<ShaderFieldInfo> shaderFields);
	}
}
