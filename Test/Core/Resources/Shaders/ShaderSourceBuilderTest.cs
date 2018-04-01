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
				.AppendLine("// uniform vec4 sharedUniformA;")
				.AppendLine()
				.AppendLine("attribute vec4 mainAttributeA;")
				.AppendLine("// attribute vec4 sharedAttributeA;")
				.AppendLine("in float mainAttributeB;")
				.AppendLine()
				.AppendLine("// varying vec4 sharedVaryingA;")
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
		[Test] public void CommentHandling()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("uniform vec4 sharedUniformA;")
				.AppendLine("uniform float sharedUniformB;")
				.AppendLine("uniform float sharedUniformC;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = sharedFunc(gl_Vertex);")
				.AppendLine("}")
				.ToString();
			string sharedShader = new StringBuilder()
				.AppendLine("uniform vec4 sharedUniformA;")
				.AppendLine("//uniform float sharedUniformB;")
				.AppendLine("/*")
				.AppendLine("uniform float sharedUniformC;")
				.AppendLine("*/")
				.AppendLine()
				.AppendLine("vec4 sharedFuncA(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.ToString();
			string expectedResultShader = new StringBuilder()
				.AppendLine("#line 10000")
				.AppendLine("uniform vec4 sharedUniformA;")
				.AppendLine("//uniform float sharedUniformB;")
				.AppendLine("/*")
				.AppendLine("uniform float sharedUniformC;")
				.AppendLine("*/")
				.AppendLine()
				.AppendLine("vec4 sharedFuncA(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.AppendLine()
				.AppendLine("#line 1")
				.AppendLine("// uniform vec4 sharedUniformA;")
				.AppendLine("uniform float sharedUniformB;")
				.AppendLine("uniform float sharedUniformC;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = sharedFunc(gl_Vertex);")
				.AppendLine("}")
				.ToString();

			builder.SetMainChunk(mainShader);
			builder.AddSharedChunk(sharedShader);

			string resultShader = builder.Build();
			Assert.AreEqual(expectedResultShader, resultShader);
		}
		[Test] public void VersionDirective()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("#version 150")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = sharedFunc(gl_Vertex);")
				.AppendLine("}")
				.ToString();
			string sharedShader = new StringBuilder()
				.AppendLine("#version 140")
				.AppendLine()
				.AppendLine("vec4 sharedFuncA(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.ToString();
			string expectedResultShader = new StringBuilder()
				.AppendLine("#version 150")
				.AppendLine("#line 10000")
				.AppendLine("// #version 140")
				.AppendLine()
				.AppendLine("vec4 sharedFuncA(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.AppendLine()
				.AppendLine("#line 1")
				.AppendLine("// #version 150")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = sharedFunc(gl_Vertex);")
				.AppendLine("}")
				.ToString();

			builder.SetMainChunk(mainShader);
			builder.AddSharedChunk(sharedShader);

			string resultShader = builder.Build();
			Assert.AreEqual(expectedResultShader, resultShader);
		}
		[Test] public void ConditionalSymbols()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = sharedFunc(gl_Vertex);")
				.AppendLine("}")
				.ToString();
			string sharedShader = new StringBuilder()
				.AppendLine("vec4 sharedFuncA(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.ToString();
			string expectedResultShader = new StringBuilder()
				.AppendLine("#line 20000")
				.AppendLine("#define CONDITION_B")
				.AppendLine("#define CONDITION_C")
				.AppendLine()
				.AppendLine("#line 10000")
				.AppendLine("vec4 sharedFuncA(vec4 pos)")
				.AppendLine("{")
				.AppendLine("  return pos + vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.AppendLine()
				.AppendLine("#line 1")
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = sharedFunc(gl_Vertex);")
				.AppendLine("}")
				.ToString();

			builder.SetConditional("CONDITION_A", true);
			builder.SetConditional("CONDITION_B", true);
			builder.SetConditional("CONDITION_C", true);
			builder.SetConditional("CONDITION_A", false);
			builder.SetMainChunk(mainShader);
			builder.AddSharedChunk(sharedShader);

			string resultShader = builder.Build();
			Assert.AreEqual(expectedResultShader, resultShader);
		}
		[Test] public void VariableMetadata()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("#pragma duality editorType ColorRgba")
				.AppendLine("#pragma duality description \"Test mainUniform Desc\"")
				.AppendLine("uniform vec4 mainUniform;")
				.AppendLine()
				.AppendLine("#pragma duality editorType Single")
				.AppendLine("#pragma duality description \"Test sharedUniform Desc\"")
				.AppendLine("// Some comment")
				.AppendLine("uniform float sharedUniform;")
				.AppendLine()
				.AppendLine("#pragma duality editorType ColorRgba")
				.AppendLine()
				.AppendLine("#pragma duality description \"Test mainAttribute Desc\"")
				.AppendLine()
				.AppendLine("in vec4 mainAttribute;")
				.AppendLine()
				.AppendLine("#pragma duality editorType Single")
				.AppendLine("#pragma duality description \"Test sharedAttribute Desc\"")
				.AppendLine()
				.AppendLine("in vec4 sharedAttribute;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = vec4(0.0, 0.0, 0.0, 1.0);")
				.AppendLine("}")
				.ToString();
			string sharedShader = new StringBuilder()
				.AppendLine("#pragma duality editorType Single")
				.AppendLine("#pragma duality description \"Test sharedUniform Desc\"")
				.AppendLine("uniform float sharedUniform;")
				.AppendLine()
				.AppendLine("#pragma duality editorType Single")
				.AppendLine("#pragma duality description \"Test sharedAttribute Desc\"")
				.AppendLine("in vec4 sharedAttribute;")
				.AppendLine()
				.AppendLine("vec4 sharedFuncA(vec4 pos) { return pos; }")
				.ToString();
			string expectedResultShader = new StringBuilder()
				.AppendLine("#line 10000")
				.AppendLine("#pragma duality editorType Single")
				.AppendLine("#pragma duality description \"Test sharedUniform Desc\"")
				.AppendLine("uniform float sharedUniform;")
				.AppendLine()
				.AppendLine("#pragma duality editorType Single")
				.AppendLine("#pragma duality description \"Test sharedAttribute Desc\"")
				.AppendLine("in vec4 sharedAttribute;")
				.AppendLine()
				.AppendLine("vec4 sharedFuncA(vec4 pos) { return pos; }")
				.AppendLine()
				.AppendLine("#line 1")
				.AppendLine("#pragma duality editorType ColorRgba")
				.AppendLine("#pragma duality description \"Test mainUniform Desc\"")
				.AppendLine("uniform vec4 mainUniform;")
				.AppendLine()
				.AppendLine("// #pragma duality editorType Single")
				.AppendLine("// #pragma duality description \"Test sharedUniform Desc\"")
				.AppendLine("// Some comment")
				.AppendLine("// uniform float sharedUniform;")
				.AppendLine()
				.AppendLine("#pragma duality editorType ColorRgba")
				.AppendLine()
				.AppendLine("#pragma duality description \"Test mainAttribute Desc\"")
				.AppendLine()
				.AppendLine("in vec4 mainAttribute;")
				.AppendLine()
				.AppendLine("// #pragma duality editorType Single")
				.AppendLine("// #pragma duality description \"Test sharedAttribute Desc\"")
				.AppendLine()
				.AppendLine("// in vec4 sharedAttribute;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = vec4(0.0, 0.0, 0.0, 1.0);")
				.AppendLine("}")
				.ToString();

			builder.SetMainChunk(mainShader);
			builder.AddSharedChunk(sharedShader);

			string resultShader = builder.Build();
			Assert.AreEqual(expectedResultShader, resultShader);
		}
		[Test] public void BasicFieldParsing()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("#pragma duality editorType ColorRgba")
				.AppendLine("#pragma duality description \"Test firstUniform Desc\"")
				.AppendLine("uniform vec4 firstUniform;")
				.AppendLine()
				.AppendLine("#pragma duality editorType Single")
				.AppendLine("#pragma duality description \"Test secondUniform Desc\"")
				.AppendLine("// Some comment")
				.AppendLine("uniform float secondUniform;")
				.AppendLine()
				.AppendLine("#pragma duality editorType ColorRgba")
				.AppendLine()
				.AppendLine("#pragma duality description \"Test firstAttribute Desc\"")
				.AppendLine()
				.AppendLine("in vec4 firstAttribute;")
				.AppendLine()
				.AppendLine("#pragma duality editorType Single")
				.AppendLine("#pragma duality description \"Test secondAttribute Desc\"")
				.AppendLine()
				.AppendLine("in vec4 secondAttribute;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = vec4(0.0, 0.0, 0.0, 1.0);")
				.AppendLine("}")
				.ToString();

			builder.SetMainChunk(mainShader);

			string resultShader = builder.Build();
			Assert.AreEqual(4, builder.Fields.Count);

			ShaderFieldInfo firstUniform = builder.Fields[0];
			ShaderFieldInfo secondUniform = builder.Fields[1];
			ShaderFieldInfo firstAttribute = builder.Fields[2];
			ShaderFieldInfo secondAttribute = builder.Fields[3];

			Assert.AreEqual("firstUniform", firstUniform.Name);
			Assert.AreEqual(ShaderFieldScope.Uniform, firstUniform.Scope);
			Assert.AreEqual(ShaderFieldType.Vec4, firstUniform.Type);
			Assert.AreEqual("ColorRgba", firstUniform.EditorTypeTag);
			Assert.AreEqual("Test firstUniform Desc", firstUniform.Description);
			Assert.AreEqual(1, firstUniform.ArrayLength);
			Assert.AreEqual(false, firstUniform.IsPrivate);

			Assert.AreEqual("secondUniform", secondUniform.Name);
			Assert.AreEqual(ShaderFieldScope.Uniform, secondUniform.Scope);
			Assert.AreEqual(ShaderFieldType.Float, secondUniform.Type);
			Assert.AreEqual("Single", secondUniform.EditorTypeTag);
			Assert.AreEqual("Test secondUniform Desc", secondUniform.Description);
			Assert.AreEqual(1, secondUniform.ArrayLength);
			Assert.AreEqual(false, secondUniform.IsPrivate);

			Assert.AreEqual("firstAttribute", firstAttribute.Name);
			Assert.AreEqual(ShaderFieldScope.Attribute, firstAttribute.Scope);
			Assert.AreEqual(ShaderFieldType.Vec4, firstAttribute.Type);
			Assert.AreEqual("ColorRgba", firstAttribute.EditorTypeTag);
			Assert.AreEqual("Test firstAttribute Desc", firstAttribute.Description);
			Assert.AreEqual(1, firstAttribute.ArrayLength);
			Assert.AreEqual(false, firstAttribute.IsPrivate);

			Assert.AreEqual("secondAttribute", secondAttribute.Name);
			Assert.AreEqual(ShaderFieldScope.Attribute, secondAttribute.Scope);
			Assert.AreEqual(ShaderFieldType.Vec4, secondAttribute.Type);
			Assert.AreEqual("Single", secondAttribute.EditorTypeTag);
			Assert.AreEqual("Test secondAttribute Desc", secondAttribute.Description);
			Assert.AreEqual(1, secondAttribute.ArrayLength);
			Assert.AreEqual(false, secondAttribute.IsPrivate);
		}
	}
}
