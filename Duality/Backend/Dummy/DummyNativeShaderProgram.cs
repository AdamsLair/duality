using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend.Dummy
{
	internal class DummyNativeShaderProgram : INativeShaderProgram
	{
		void INativeShaderProgram.LoadProgram(INativeShaderPart vertex, INativeShaderPart fragment) { }
		ShaderFieldInfo[] INativeShaderProgram.GetFields()
		{
			return new ShaderFieldInfo[0];
		}
		void IDisposable.Dispose() { }
	}
}
