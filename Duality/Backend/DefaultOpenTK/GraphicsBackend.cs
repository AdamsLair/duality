using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend.DefaultOpenTK
{
	public class GraphicsBackend : IGraphicsBackend, IVertexUploader
	{
		private	IDrawDevice		currentDevice	= null;
		private RenderStats		renderStats		= null;
		private	uint			primaryVBO		= 0;

		void IDualityBackend.Init() { }
		void IDualityBackend.Shutdown()
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.primaryVBO != 0)
			{
				DualityApp.GuardSingleThreadState();
				GL.DeleteBuffers(1, ref this.primaryVBO);
				this.primaryVBO = 0;
			}
		}

		void IGraphicsBackend.BeginRendering(IDrawDevice device, RenderOptions options, RenderStats stats)
		{
			this.currentDevice = device;
			this.renderStats = stats;

			// Prepare the target surface for rendering
			NativeRenderTarget.Bind(options.Target as NativeRenderTarget);

			if (this.primaryVBO == 0) GL.GenBuffers(1, out this.primaryVBO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.primaryVBO);

			// Setup viewport
			Rect viewportRect = options.Viewport;
			GL.Viewport((int)viewportRect.X, (int)viewportRect.Y, (int)viewportRect.W, (int)viewportRect.H);
			GL.Scissor((int)viewportRect.X, (int)viewportRect.Y, (int)viewportRect.W, (int)viewportRect.H);

			// Clear buffers
			ClearBufferMask glClearMask = 0;
			ColorRgba clearColor = options.ClearColor;
			if ((options.ClearFlags & ClearFlag.Color) != ClearFlag.None) glClearMask |= ClearBufferMask.ColorBufferBit;
			if ((options.ClearFlags & ClearFlag.Depth) != ClearFlag.None) glClearMask |= ClearBufferMask.DepthBufferBit;
			GL.ClearColor(clearColor.R / 255.0f, clearColor.G / 255.0f, clearColor.B / 255.0f, clearColor.A / 255.0f);
			GL.ClearDepth((double)options.ClearDepth); // The "float version" is from OpenGL 4.1..
			GL.Clear(glClearMask);

			// Configure Rendering params
			if (options.RenderMode == RenderMatrix.OrthoScreen)
			{
				GL.Enable(EnableCap.ScissorTest);
				GL.Enable(EnableCap.DepthTest);
				GL.DepthFunc(DepthFunction.Always);
			}
			else
			{
				GL.Enable(EnableCap.ScissorTest);
				GL.Enable(EnableCap.DepthTest);
				GL.DepthFunc(DepthFunction.Lequal);
			}
			
			OpenTK.Matrix4 openTkModelView;
			Matrix4 modelView = options.ModelViewMatrix;
			GetOpenTKMatrix(ref modelView, out openTkModelView);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref openTkModelView);
			
			OpenTK.Matrix4 openTkProjection;
			Matrix4 projection = options.ProjectionMatrix;
			GetOpenTKMatrix(ref projection, out openTkProjection);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref openTkProjection);

			if (NativeRenderTarget.BoundRT != null)
			{
				if (options.RenderMode == RenderMatrix.OrthoScreen) GL.Translate(0.0f, NativeRenderTarget.BoundRT.Height * 0.5f, 0.0f);
				GL.Scale(1.0f, -1.0f, 1.0f);
				if (options.RenderMode == RenderMatrix.OrthoScreen) GL.Translate(0.0f, -NativeRenderTarget.BoundRT.Height * 0.5f, 0.0f);
			}
		}
		void IGraphicsBackend.Render(IReadOnlyList<IDrawBatch> batches)
		{
			List<IDrawBatch> batchesSharingVBO = new List<IDrawBatch>();
			IDrawBatch lastBatchRendered = null;
			IDrawBatch lastBatch = null;
			int drawCalls = 0;

			for (int i = 0; i < batches.Count; i++)
			{
				IDrawBatch currentBatch = batches[i];
				IDrawBatch nextBatch = (i < batches.Count - 1) ? batches[i + 1] : null;

				if (lastBatch == null || lastBatch.SameVertexType(currentBatch))
				{
					batchesSharingVBO.Add(currentBatch);
				}

				if (batchesSharingVBO.Count > 0 && (nextBatch == null || !currentBatch.SameVertexType(nextBatch)))
				{
					int vertexOffset = 0;
					batchesSharingVBO[0].UploadVertices(this, batchesSharingVBO);
					drawCalls++;

					foreach (IDrawBatch batch in batchesSharingVBO)
					{
						drawCalls++;

						this.PrepareRenderBatch(batch);
						this.RenderBatch(batch, vertexOffset, lastBatchRendered);
						this.FinishRenderBatch(batch);

						vertexOffset += batch.VertexCount;
						lastBatchRendered = batch;
					}

					batchesSharingVBO.Clear();
					lastBatch = null;
				}
				else
					lastBatch = currentBatch;
			}

			if (lastBatchRendered != null)
			{
				this.FinishMaterial(lastBatchRendered.Material);
			}

			if (this.renderStats != null)
			{
				this.renderStats.DrawCalls += drawCalls;
			}
		}
		void IGraphicsBackend.EndRendering()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			this.currentDevice = null;
		}
		void IVertexUploader.UploadBatchVertices<T>(VertexDeclaration declaration, T[] vertices, int vertexCount)
		{
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(declaration.Size * vertexCount), IntPtr.Zero, BufferUsageHint.StreamDraw);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(declaration.Size * vertexCount), vertices, BufferUsageHint.StreamDraw);
		}
		
		INativeTexture IGraphicsBackend.CreateTexture()
		{
			return new NativeTexture();
		}
		INativeRenderTarget IGraphicsBackend.CreateRenderTarget()
		{
			return new NativeRenderTarget();
		}
		INativeShaderPart IGraphicsBackend.CreateShaderPart()
		{
			return new NativeShaderPart();
		}
		INativeShaderProgram IGraphicsBackend.CreateShaderProgram()
		{
			return new NativeShaderProgram();
		}

		private void PrepareRenderBatch(IDrawBatch renderBatch)
		{
			DrawTechnique technique = renderBatch.Material.Technique.Res ?? DrawTechnique.Solid.Res;
			ShaderProgram program = technique.Shader.Res;

			VertexDeclaration vertexDeclaration = renderBatch.VertexDeclaration;

			VertexElement[] elements = vertexDeclaration.Elements;
			for (int elementIndex = 0; elementIndex < elements.Length; elementIndex++)
			{
				switch (elements[elementIndex].Role)
				{
					case VertexElementRole.Position:
					{
						GL.EnableClientState(ArrayCap.VertexArray);
						GL.VertexPointer(
							elements[elementIndex].Count, 
							VertexPointerType.Float, 
							vertexDeclaration.Size, 
							elements[elementIndex].Offset);
						break;
					}
					case VertexElementRole.TexCoord:
					{
						GL.EnableClientState(ArrayCap.TextureCoordArray);
						GL.TexCoordPointer(
							elements[elementIndex].Count, 
							TexCoordPointerType.Float, 
							vertexDeclaration.Size, 
							elements[elementIndex].Offset);
						break;
					}
					case VertexElementRole.Color:
					{
						ColorPointerType attribType;
						switch (elements[elementIndex].Type)
						{
							default:
							case VertexElementType.Float: attribType = ColorPointerType.Float; break;
							case VertexElementType.Byte: attribType = ColorPointerType.UnsignedByte; break;
						}

						GL.EnableClientState(ArrayCap.ColorArray);
						GL.ColorPointer(
							elements[elementIndex].Count, 
							attribType, 
							vertexDeclaration.Size, 
							elements[elementIndex].Offset);
						break;
					}
					default:
					{
						ShaderFieldInfo[] varInfo = program != null ? program.Fields : null;
						if (varInfo != null)
						{
							int selectedVar = -1;
							for (int varIndex = 0; varIndex < varInfo.Length; varIndex++)
							{
								if (varInfo[varIndex].Handle == -1) continue;
								if (!ShaderVarMatches(
									ref varInfo[varIndex],
									elements[elementIndex].Type, 
									elements[elementIndex].Count))
									continue;
								
								selectedVar = varIndex;
								break;
							}
							if (selectedVar == -1) break;

							VertexAttribPointerType attribType;
							switch (elements[elementIndex].Type)
							{
								default:
								case VertexElementType.Float: attribType = VertexAttribPointerType.Float; break;
								case VertexElementType.Byte: attribType = VertexAttribPointerType.UnsignedByte; break;
							}

							GL.EnableVertexAttribArray(varInfo[selectedVar].Handle);
							GL.VertexAttribPointer(
								varInfo[selectedVar].Handle, 
								elements[elementIndex].Count, 
								attribType, 
								false, 
								vertexDeclaration.Size, 
								elements[elementIndex].Offset);
						}
						break;
					}
				}
			}
		}
		private void RenderBatch(IDrawBatch renderBatch, int vertexOffset, IDrawBatch lastBatchRendered)
		{
			if (lastBatchRendered == null || lastBatchRendered.Material != renderBatch.Material)
				this.SetupMaterial(renderBatch.Material, lastBatchRendered == null ? null : lastBatchRendered.Material);

			GL.DrawArrays(GetOpenTKVertexMode(renderBatch.VertexMode), vertexOffset, renderBatch.VertexCount);

			lastBatchRendered = renderBatch;
		}
		private void FinishRenderBatch(IDrawBatch renderBatch)
		{
			DrawTechnique technique = renderBatch.Material.Technique.Res ?? DrawTechnique.Solid.Res;
			ShaderProgram program = technique.Shader.Res;

			VertexDeclaration vertexDeclaration = renderBatch.VertexDeclaration;

			VertexElement[] elements = vertexDeclaration.Elements;
			for (int elementIndex = 0; elementIndex < elements.Length; elementIndex++)
			{
				switch (elements[elementIndex].Role)
				{
					case VertexElementRole.Position:
					{
						GL.DisableClientState(ArrayCap.VertexArray);
						break;
					}
					case VertexElementRole.TexCoord:
					{
						GL.DisableClientState(ArrayCap.TextureCoordArray);
						break;
					}
					case VertexElementRole.Color:
					{
						GL.DisableClientState(ArrayCap.ColorArray);
						break;
					}
					default:
					{
						ShaderFieldInfo[] varInfo = program != null ? program.Fields : null;
						if (varInfo != null)
						{
							int selectedVar = -1;
							for (int varIndex = 0; varIndex < varInfo.Length; varIndex++)
							{
								if (varInfo[varIndex].Handle == -1) continue;
								if (!ShaderVarMatches(
									ref varInfo[varIndex],
									elements[elementIndex].Type, 
									elements[elementIndex].Count))
									continue;
								
								selectedVar = varIndex;
								break;
							}
							if (selectedVar == -1) break;

							GL.DisableVertexAttribArray(varInfo[selectedVar].Handle);
						}
						break;
					}
				}
			}
		}

		private void SetupMaterial(BatchInfo material, BatchInfo lastMaterial)
		{
			if (material == lastMaterial) return;
			DrawTechnique tech = material.Technique.Res ?? DrawTechnique.Solid.Res;
			DrawTechnique lastTech = lastMaterial != null ? lastMaterial.Technique.Res : null;
			
			// Prepare Rendering
			if (tech.NeedsPreparation)
			{
				material = new BatchInfo(material);
				tech.PrepareRendering(this.currentDevice, material);
			}
			
			// Setup BlendType
			if (lastTech == null || tech.Blending != lastTech.Blending)
				this.SetupBlendType(tech.Blending, this.currentDevice.DepthWrite);

			// Bind Shader
			ContentRef<ShaderProgram> selShader = tech.Shader;
			if (lastTech == null || selShader.Res != lastTech.Shader.Res)
				NativeShaderProgram.Bind(selShader);

			// Setup shader data
			if (selShader.IsAvailable)
			{
				ShaderFieldInfo[] varInfo = selShader.Res.Fields;

				// Setup sampler bindings automatically
				int curSamplerIndex = 0;
				if (material.Textures != null)
				{
					for (int i = 0; i < varInfo.Length; i++)
					{
						if (varInfo[i].Handle == -1) continue;
						if (varInfo[i].Type != ShaderFieldType.Sampler2D) continue;

						// Bind Texture
						ContentRef<Texture> texRef = material.GetTexture(varInfo[i].Name);
						NativeTexture.Bind(texRef, curSamplerIndex);
						GL.Uniform1(varInfo[i].Handle, curSamplerIndex);

						curSamplerIndex++;
					}
				}
				NativeTexture.ResetBinding(curSamplerIndex);

				// Transfer uniform data from material to actual shader
				if (material.Uniforms != null)
				{
					for (int i = 0; i < varInfo.Length; i++)
					{
						if (varInfo[i].Handle == -1) continue;
						float[] data = material.GetUniform(varInfo[i].Name);
						if (data == null) continue;
						NativeShaderProgram.SetUniform(ref varInfo[i], data);
					}
				}
			}
			// Setup fixed function data
			else
			{
				// Fixed function texture binding
				if (material.Textures != null)
				{
					int samplerIndex = 0;
					foreach (var pair in material.Textures)
					{
						NativeTexture.Bind(pair.Value, samplerIndex);
						samplerIndex++;
					}
					NativeTexture.ResetBinding(samplerIndex);
				}
				else
					NativeTexture.ResetBinding();
			}
		}
		private void SetupBlendType(BlendMode mode, bool depthWrite = true)
		{
			switch (mode)
			{
				default:
				case BlendMode.Reset:
				case BlendMode.Solid:
					GL.DepthMask(depthWrite);
					GL.Disable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					break;
				case BlendMode.Mask:
					GL.DepthMask(depthWrite);
					GL.Disable(EnableCap.Blend);

					bool useAlphaToCoverage = false;
					if (NativeRenderTarget.BoundRT != null)
						useAlphaToCoverage = NativeRenderTarget.BoundRT.Samples > 0;
					else
						useAlphaToCoverage = DualityApp.TargetMode.Samples > 0; 

					if (useAlphaToCoverage)
					{
						GL.Disable(EnableCap.AlphaTest);
						GL.Enable(EnableCap.SampleAlphaToCoverage);
					}
					else
					{
						GL.Enable(EnableCap.AlphaTest);
						GL.AlphaFunc(AlphaFunction.Gequal, 0.5f);
					}
					break;
				case BlendMode.Alpha:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
					break;
				case BlendMode.AlphaPre:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
					break;
				case BlendMode.Add:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One, BlendingFactorSrc.One, BlendingFactorDest.One);
					break;
				case BlendMode.Light:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.DstColor, BlendingFactorDest.One, BlendingFactorSrc.Zero, BlendingFactorDest.One);
					break;
				case BlendMode.Multiply:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.Zero);
					break;
				case BlendMode.Invert:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.AlphaTest);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFunc(BlendingFactorSrc.OneMinusDstColor, BlendingFactorDest.OneMinusSrcColor);
					break;
			}
		}
		private void FinishMaterial(BatchInfo material)
		{
			DrawTechnique tech = material.Technique.Res;
			this.SetupBlendType(BlendMode.Reset);
			NativeShaderProgram.Bind(null as NativeShaderProgram);
			NativeTexture.ResetBinding();
		}
		
		private static PrimitiveType GetOpenTKVertexMode(VertexMode mode)
		{
			switch (mode)
			{
				default:
				case VertexMode.Points:			return PrimitiveType.Points;
				case VertexMode.Lines:			return PrimitiveType.Lines;
				case VertexMode.LineStrip:		return PrimitiveType.LineStrip;
				case VertexMode.LineLoop:		return PrimitiveType.LineLoop;
				case VertexMode.TriangleStrip:	return PrimitiveType.TriangleStrip;
				case VertexMode.TriangleFan:	return PrimitiveType.TriangleFan;
				case VertexMode.Quads:			return PrimitiveType.Quads;
			}
		}
		private static void GetOpenTKMatrix(ref Matrix4 source, out OpenTK.Matrix4 target)
		{
			target = new OpenTK.Matrix4(
				source.M11, source.M12, source.M13, source.M14,
				source.M21, source.M22, source.M23, source.M24,
				source.M31, source.M32, source.M33, source.M34,
				source.M41, source.M42, source.M43, source.M44);
		}
		private static bool ShaderVarMatches(ref ShaderFieldInfo varInfo, VertexElementType type, int count)
		{
			if (varInfo.Scope != ShaderFieldScope.Attribute) return false;

			Type elementPrimitive = varInfo.Type.GetElementPrimitive();
			Type requiredPrimitive = null;
			switch (type)
			{
				case VertexElementType.Byte:
					requiredPrimitive = typeof(byte);
					break;
				case VertexElementType.Float:
					requiredPrimitive = typeof(float);
					break;
			}
			if (elementPrimitive != requiredPrimitive)
				return false;

			int elementCount = varInfo.Type.GetElementCount();
			if (count != elementCount * varInfo.ArrayLength)
				return false;

			return true;
		}
	}
}
