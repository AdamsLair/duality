using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend.DefaultOpenTK
{
	public class DefaultOpenTKBackend : IGraphicsBackend, IVertexUploader
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

			if (RenderTarget.BoundRT.IsAvailable)
			{
				if (options.RenderMode == RenderMatrix.OrthoScreen) GL.Translate(0.0f, RenderTarget.BoundRT.Res.Height * 0.5f, 0.0f);
				GL.Scale(1.0f, -1.0f, 1.0f);
				if (options.RenderMode == RenderMatrix.OrthoScreen) GL.Translate(0.0f, -RenderTarget.BoundRT.Res.Height * 0.5f, 0.0f);
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
				lastBatchRendered.Material.FinishRendering();
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
						ShaderVarInfo[] varInfo = program != null ? program.VarInfo : null;
						if (varInfo != null)
						{
							int selectedVar = -1;
							for (int varIndex = 0; varIndex < varInfo.Length; varIndex++)
							{
								if (varInfo[varIndex].glVarLoc == -1) continue;
								if (!varInfo[varIndex].MatchesVertexElement(
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

							GL.EnableVertexAttribArray(varInfo[selectedVar].glVarLoc);
							GL.VertexAttribPointer(
								varInfo[selectedVar].glVarLoc, 
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
				renderBatch.Material.SetupForRendering(this.currentDevice, lastBatchRendered == null ? null : lastBatchRendered.Material);

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
						ShaderVarInfo[] varInfo = program != null ? program.VarInfo : null;
						if (varInfo != null)
						{
							int selectedVar = -1;
							for (int varIndex = 0; varIndex < varInfo.Length; varIndex++)
							{
								if (varInfo[varIndex].glVarLoc == -1) continue;
								if (!varInfo[varIndex].MatchesVertexElement(
									elements[elementIndex].Type, 
									elements[elementIndex].Count))
									continue;
								
								selectedVar = varIndex;
								break;
							}
							if (selectedVar == -1) break;

							GL.DisableVertexAttribArray(varInfo[selectedVar].glVarLoc);
						}
						break;
					}
				}
			}
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
	}
}
