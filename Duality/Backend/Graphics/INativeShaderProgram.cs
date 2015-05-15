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
		/// <param name="vertex"></param>
		/// <param name="fragment"></param>
		void LoadProgram(INativeShaderPart vertex, INativeShaderPart fragment);

		/// <summary>
		/// Updates reflection data on the shaders fields to include a location handle
		/// that can be used to set variable values.
		/// </summary>
		/// <returns></returns>
		void GetFieldLocations(ShaderFieldInfo[] fields);
	}
}
