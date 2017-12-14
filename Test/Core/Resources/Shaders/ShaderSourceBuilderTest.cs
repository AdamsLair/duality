using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Tests.Properties;

using NUnit.Framework;


namespace Duality.Tests.Resources
{
	[TestFixture]
	public class ShaderSourceBuilderTest
	{
		[Test] public void Basics()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("uniform vec4 mainUniformA;")
				.AppendLine("uniform float mainUniformB;")
				.AppendLine("uniform vec4 sharedUniformA;")
				.AppendLine()
				.AppendLine("attribute vec4 mainAttributeA;")
				.AppendLine("attribute vec4 sharedAttributeA;")
				.AppendLine("in float mainAttributeB;")
				.AppendLine()
				.AppendLine("varying vec4 sharedVaryingA;")
				.AppendLine("varying vec4 mainVaryingA;")
				.AppendLine("out float mainVaryingB;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = sharedFunc(gl_Vertex);")
				.AppendLine("}")
				.ToString();
			string sharedShaderA = new StringBuilder()
				.AppendLine("uniform vec4 sharedUniformA;")
				.AppendLine("attribute vec4 sharedAttributeA;")
				.AppendLine("varying vec4 sharedVaryingA;")
				.AppendLine()
				.AppendLine("vec4 sharedFuncA(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.ToString();
			string sharedShaderB = new StringBuilder()
				.AppendLine("uniform float sharedUniformB;")
				.AppendLine("in float sharedAttributeB;")
				.AppendLine("out float sharedVaryingB;")
				.AppendLine()
				.AppendLine("vec4 sharedFuncB(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.ToString();
			string expectedResultShader = new StringBuilder()
				.AppendLine("#line 10000")
				.AppendLine("uniform vec4 sharedUniformA;")
				.AppendLine("attribute vec4 sharedAttributeA;")
				.AppendLine("varying vec4 sharedVaryingA;")
				.AppendLine()
				.AppendLine("vec4 sharedFuncA(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.AppendLine()
				.AppendLine("#line 20000")
				.AppendLine("uniform float sharedUniformB;")
				.AppendLine("in float sharedAttributeB;")
				.AppendLine("out float sharedVaryingB;")
				.AppendLine()
				.AppendLine("vec4 sharedFuncB(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.AppendLine()
				.AppendLine("#line 1")
				.AppendLine("uniform vec4 mainUniformA;")
				.AppendLine("uniform float mainUniformB;")
				.AppendLine("// Shared: uniform vec4 sharedUniformA;")
				.AppendLine()
				.AppendLine("attribute vec4 mainAttributeA;")
				.AppendLine("// Shared: attribute vec4 sharedAttributeA;")
				.AppendLine("in float mainAttributeB;")
				.AppendLine()
				.AppendLine("// Shared: varying vec4 sharedVaryingA;")
				.AppendLine("varying vec4 mainVaryingA;")
				.AppendLine("out float mainVaryingB;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = sharedFunc(gl_Vertex);")
				.AppendLine("}")
				.ToString();

			builder.SetMainChunk(mainShader);
			builder.AddSharedChunk(sharedShaderA);
			builder.AddSharedChunk(sharedShaderB);

			string resultShader = builder.Build();
			Assert.AreEqual(expectedResultShader, resultShader);
		}
	}
}
