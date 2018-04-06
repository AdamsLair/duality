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
		[Test] public void Composition()
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
		[Test] public void FieldParsingBasics()
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
		[Test] public void FieldParsingTypes()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("uniform bool fieldBool;")
				.AppendLine("uniform int fieldInt;")
				.AppendLine("uniform float fieldFloat;")
				.AppendLine("uniform vec2 fieldVec2;")
				.AppendLine("uniform vec3 fieldVec3;")
				.AppendLine("uniform vec4 fieldVec4;")
				.AppendLine("uniform mat2 fieldMat2;")
				.AppendLine("uniform mat3 fieldMat3;")
				.AppendLine("uniform mat4 fieldMat4;")
				.AppendLine()
				.AppendLine("in bool attribBool;")
				.AppendLine("in int attribInt;")
				.AppendLine("in float attribFloat;")
				.AppendLine("in vec2 attribVec2;")
				.AppendLine("in vec3 attribVec3;")
				.AppendLine("in vec4 attribVec4;")
				.AppendLine("in mat2 attribMat2;")
				.AppendLine("in mat3 attribMat3;")
				.AppendLine("in mat4 attribMat4;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = vec4(0.0, 0.0, 0.0, 1.0);")
				.AppendLine("}")
				.ToString();

			builder.SetMainChunk(mainShader);

			string resultShader = builder.Build();

			int typeCount = 9;
			Assert.AreEqual(typeCount * 2, builder.Fields.Count);
			for (int i = 0; i < 2; i++)
			{
				Assert.AreEqual(ShaderFieldType.Bool, builder.Fields[i * typeCount + 0].Type);
				Assert.AreEqual(ShaderFieldType.Int, builder.Fields[i * typeCount + 1].Type);
				Assert.AreEqual(ShaderFieldType.Float, builder.Fields[i * typeCount + 2].Type);
				Assert.AreEqual(ShaderFieldType.Vec2, builder.Fields[i * typeCount + 3].Type);
				Assert.AreEqual(ShaderFieldType.Vec3, builder.Fields[i * typeCount + 4].Type);
				Assert.AreEqual(ShaderFieldType.Vec4, builder.Fields[i * typeCount + 5].Type);
				Assert.AreEqual(ShaderFieldType.Mat2, builder.Fields[i * typeCount + 6].Type);
				Assert.AreEqual(ShaderFieldType.Mat3, builder.Fields[i * typeCount + 7].Type);
				Assert.AreEqual(ShaderFieldType.Mat4, builder.Fields[i * typeCount + 8].Type);
			}
		}
		[Test] public void FieldParsingArrays()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("uniform int fieldInt[1];")
				.AppendLine("uniform float fieldFloat[2];")
				.AppendLine("uniform vec2 fieldVec2[3];")
				.AppendLine()
				.AppendLine("in int attribInt[1];")
				.AppendLine("in float attribFloat[2];")
				.AppendLine("in vec2 attribVec2[3];")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = vec4(0.0, 0.0, 0.0, 1.0);")
				.AppendLine("}")
				.ToString();

			builder.SetMainChunk(mainShader);

			string resultShader = builder.Build();

			int arrayCount = 3;
			Assert.AreEqual(arrayCount * 2, builder.Fields.Count);
			for (int i = 0; i < 2; i++)
			{
				Assert.AreEqual(1, builder.Fields[i * arrayCount + 0].ArrayLength);
				Assert.AreEqual(2, builder.Fields[i * arrayCount + 1].ArrayLength);
				Assert.AreEqual(3, builder.Fields[i * arrayCount + 2].ArrayLength);

				Assert.AreEqual(ShaderFieldType.Int, builder.Fields[i * arrayCount + 0].Type);
				Assert.AreEqual(ShaderFieldType.Float, builder.Fields[i * arrayCount + 1].Type);
				Assert.AreEqual(ShaderFieldType.Vec2, builder.Fields[i * arrayCount + 2].Type);
			}
		}
		[Test] public void FieldParsingComposition()
		{
			ShaderSourceBuilder builder = new ShaderSourceBuilder();
			string mainShader = new StringBuilder()
				.AppendLine("#pragma duality editorType ColorRgba")
				.AppendLine("uniform vec4 mainColor;")
				.AppendLine()
				.AppendLine("in vec3 vertexPos;")
				.AppendLine()
				.AppendLine("out vec4 programColor;")
				.AppendLine()
				.AppendLine("void main()")
				.AppendLine("{")
				.AppendLine("  gl_Position = vec4(0.0, 0.0, 0.0, 1.0);")
				.AppendLine("  programColor = vec4(1.0, 0.0, 0.0, 0.0);")
				.AppendLine("}")
				.ToString();
			string sharedShader = new StringBuilder()
				.AppendLine("// Some comment")
				.AppendLine("// Another comment")
				.AppendLine("# version 130")
				.AppendLine()
				.AppendLine("uniform mat4 _viewMatrix;")
				.AppendLine("uniform mat4 _projectionMatrix")
				.AppendLine("uniform mat4 _viewProjectionMatrix")
				.ToString();

			builder.SetMainChunk(mainShader);
			builder.AddSharedChunk(sharedShader);

			string resultShader = builder.Build();

			// Make sure all fields are there as expected
			Assert.AreEqual("_viewMatrix", builder.Fields[0].Name);
			Assert.AreEqual("_projectionMatrix", builder.Fields[1].Name);
			Assert.AreEqual("_viewProjectionMatrix", builder.Fields[2].Name);
			Assert.AreEqual("mainColor", builder.Fields[3].Name);
			Assert.AreEqual("vertexPos", builder.Fields[4].Name);
		}
	}
}
