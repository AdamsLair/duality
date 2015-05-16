using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeShaderPart : INativeShaderPart
	{
		void INativeShaderPart.LoadSource(string sourceCode, Resources.ShaderType type) { }
		void IDisposable.Dispose() { }
	}
}
