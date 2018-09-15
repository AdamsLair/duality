using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeShaderProgram : INativeShaderProgram
	{
		void INativeShaderProgram.LoadProgram(IEnumerable<INativeShaderPart> shaderParts, IEnumerable<ShaderFieldInfo> shaderFields) { }
		void IDisposable.Dispose() { }
	}
}
