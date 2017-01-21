using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class GraphicsBackend : IGraphicsBackend, IVertexUploader
	{
		private static readonly Version MinOpenGLVersion = new Version(2, 1);

		private static GraphicsBackend activeInstance = null;
		public static GraphicsBackend ActiveInstance
		{
			get { return activeInstance; }
		}

		private IDrawDevice           currentDevice           = null;
		private RenderStats           renderStats             = null;
		private HashSet<GraphicsMode> availGraphicsModes      = null;
		private GraphicsMode          defaultGraphicsMode     = null;
		private uint                  primaryVBO              = 0;
		private NativeWindow          activeWindow            = null;
		private Point2                externalBackbufferSize  = Point2.Zero;
		private bool                  useAlphaToCoverageBlend = false;
		private bool                  msaaIsDriverDisabled    = false;
		private bool                  contextCapsRetrieved    = false;

		private List<IDrawBatch>      renderBatchesSharingVBO = new List<IDrawBatch>();
		

		public GraphicsMode DefaultGraphicsMode
		{
			get { return this.defaultGraphicsMode; }
		}
		public IEnumerable<GraphicsMode> AvailableGraphicsModes
		{
			get { return this.availGraphicsModes; }
		}
		public IEnumerable<ScreenResolution> AvailableScreenResolutions
		{
			get
			{ 
				return DisplayDevice.Default.AvailableResolutions
					.Select(resolution => new ScreenResolution(resolution.Width, resolution.Height, resolution.RefreshRate))
					.Distinct();
			}
		}
		public NativeWindow ActiveWindow
		{
			get { return this.activeWindow; }
		}
		public Point2 ExternalBackbufferSize
		{
			get { return this.externalBackbufferSize; }
			set { this.externalBackbufferSize = value; }
		}

		string IDualityBackend.Id
		{
			get { return "DefaultOpenTKGraphicsBackend"; }
		}
		string IDualityBackend.Name
		{
			get { return "OpenGL 2.1 (OpenTK)"; }
		}
		int IDualityBackend.Priority
		{
			get { return 0; }
		}
		
		bool IDualityBackend.CheckAvailable()
		{
			// Since this is the default backend, it will always try to work.
			return true;
		}
		void IDualityBackend.Init()
		{
			// Initialize OpenTK, if not done yet
			DefaultOpenTKBackendPlugin.InitOpenTK();
			
			// Log information about the available display devices
			GraphicsBackend.LogDisplayDevices();

			// Determine available and default graphics modes
			this.QueryGraphicsModes();
			activeInstance = this;
		}
		void IDualityBackend.Shutdown()
		{
			if (activeInstance == this)
				activeInstance = null;

			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.primaryVBO != 0)
			{
				DefaultOpenTKBackendPlugin.GuardSingleThreadState();
				GL.DeleteBuffers(1, ref this.primaryVBO);
				this.primaryVBO = 0;
			}

			// Since the window outlives the graphics backend in the usual launcher setup, 
			// we'll need to unhook early, so Duality can complete its cleanup before the window does.
			if (this.activeWindow != null)
			{
				this.activeWindow.UnhookFromDuality();
				this.activeWindow = null;
			}
		}

		void IGraphicsBackend.BeginRendering(IDrawDevice device, RenderOptions options, RenderStats stats)
		{
			DebugCheckOpenGLErrors();
			this.CheckContextCaps();

			this.currentDevice = device;
			this.renderStats = stats;

			// Prepare the target surface for rendering
			NativeRenderTarget.Bind(options.Target as NativeRenderTarget);

			// Determine whether masked blending should use alpha-to-coverage mode
			if (this.msaaIsDriverDisabled)
				this.useAlphaToCoverageBlend = false;
			else if (NativeRenderTarget.BoundRT != null)
				this.useAlphaToCoverageBlend = NativeRenderTarget.BoundRT.Samples > 0;
			else if (this.activeWindow != null)
				this.useAlphaToCoverageBlend = this.activeWindow.IsMultisampled; 
			else
				this.useAlphaToCoverageBlend = this.defaultGraphicsMode.Samples > 0; 

			if (this.primaryVBO == 0) GL.GenBuffers(1, out this.primaryVBO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.primaryVBO);

			// Determine the available size on the active rendering surface
			Point2 availableSize;
			if (NativeRenderTarget.BoundRT != null)
				availableSize = new Point2(NativeRenderTarget.BoundRT.Width, NativeRenderTarget.BoundRT.Height);
			else if (this.activeWindow != null)
				availableSize = new Point2(this.activeWindow.Width, this.activeWindow.Height);
			else
				availableSize = this.externalBackbufferSize;

			// Translate viewport coordinates to OpenGL screen coordinates (borrom-left, rising), unless rendering
			// to a texture, which is laid out Duality-like (top-left, descending)
			Rect openGLViewport = options.Viewport;
			if (NativeRenderTarget.BoundRT == null)
			{
				openGLViewport.Y = (availableSize.Y - openGLViewport.H) - openGLViewport.Y;
			}

			// Setup viewport and scissor rects
			GL.Viewport((int)openGLViewport.X, (int)openGLViewport.Y, (int)MathF.Ceiling(openGLViewport.W), (int)MathF.Ceiling(openGLViewport.H));
			GL.Scissor((int)openGLViewport.X, (int)openGLViewport.Y, (int)MathF.Ceiling(openGLViewport.W), (int)MathF.Ceiling(openGLViewport.H));

			// Clear buffers
			ClearBufferMask glClearMask = 0;
			ColorRgba clearColor = options.ClearColor;
			if ((options.ClearFlags & ClearFlag.Color) != ClearFlag.None) glClearMask |= ClearBufferMask.ColorBufferBit;
			if ((options.ClearFlags & ClearFlag.Depth) != ClearFlag.None) glClearMask |= ClearBufferMask.DepthBufferBit;
			GL.ClearColor(clearColor.R / 255.0f, clearColor.G / 255.0f, clearColor.B / 255.0f, clearColor.A / 255.0f);
			GL.ClearDepth((double)options.ClearDepth); // The "float version" is from OpenGL 4.1..
			GL.Clear(glClearMask);

			// Configure Rendering params
			if (options.RenderMode == RenderMatrix.ScreenSpace)
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
				GL.Scale(1.0f, -1.0f, 1.0f);
				if (options.RenderMode == RenderMatrix.ScreenSpace)
					GL.Translate(0.0f, -options.Viewport.H, 0.0f);
			}
		}
		void IGraphicsBackend.Render(IReadOnlyList<IDrawBatch> batches)
		{
			IDrawBatch lastBatchRendered = null;
			IDrawBatch lastBatch = null;
			int drawCalls = 0;

			this.renderBatchesSharingVBO.Clear();
			for (int i = 0; i < batches.Count; i++)
			{
				IDrawBatch currentBatch = batches[i];
				IDrawBatch nextBatch = (i < batches.Count - 1) ? batches[i + 1] : null;

				if (lastBatch == null || lastBatch.SameVertexType(currentBatch))
				{
					this.renderBatchesSharingVBO.Add(currentBatch);
				}

				if (this.renderBatchesSharingVBO.Count > 0 && (nextBatch == null || !currentBatch.SameVertexType(nextBatch)))
				{
					int vertexOffset = 0;
					this.renderBatchesSharingVBO[0].UploadVertices(this, this.renderBatchesSharingVBO);
					drawCalls++;

					foreach (IDrawBatch batch in this.renderBatchesSharingVBO)
					{
						drawCalls++;

						this.PrepareRenderBatch(batch);
						this.RenderBatch(batch, vertexOffset, lastBatchRendered);
						this.FinishRenderBatch(batch);

						vertexOffset += batch.VertexCount;
						lastBatchRendered = batch;
					}

					this.renderBatchesSharingVBO.Clear();
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

			DebugCheckOpenGLErrors();
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
		INativeWindow IGraphicsBackend.CreateWindow(WindowOptions options)
		{
			// Only one game window allowed at a time
			if (this.activeWindow != null)
			{
				(this.activeWindow as INativeWindow).Dispose();
				this.activeWindow = null;
			}

			// Create a window and keep track of it
			this.activeWindow = new NativeWindow(defaultGraphicsMode, options);
			return this.activeWindow;
		}

		void IGraphicsBackend.GetOutputPixelData<T>(T[] buffer, ColorDataLayout dataLayout, ColorDataElementType dataElementType, int x, int y, int width, int height)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			NativeRenderTarget lastRt = NativeRenderTarget.BoundRT;
			NativeRenderTarget.Bind(null);
			{
				GL.ReadPixels(x, y, width, height, dataLayout.ToOpenTK(), dataElementType.ToOpenTK(), buffer);

				// The image will be upside-down because of OpenGL's coordinate system. Flip it.
				int structSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
				T[] switchLine = new T[width * 4 / structSize];
				for (int flipY = 0; flipY < height / 2; flipY++)
				{
					int lineIndex = flipY * width * 4 / structSize;
					int lineIndex2 = (height - 1 - flipY) * width * 4 / structSize;

					// Copy the current line to the switch buffer
					for (int lineX = 0; lineX < width; lineX++)
					{
						switchLine[lineX] = buffer[lineIndex + lineX];
					}

					// Copy the opposite line to the current line
					for (int lineX = 0; lineX < width; lineX++)
					{
						buffer[lineIndex + lineX] = buffer[lineIndex2 + lineX];
					}

					// Copy the switch buffer to the opposite line
					for (int lineX = 0; lineX < width; lineX++)
					{
						buffer[lineIndex2 + lineX] = switchLine[lineX];
					}
				}
			}
			NativeRenderTarget.Bind(lastRt);
		}

		private void QueryGraphicsModes()
		{
			int[] aaLevels = new int[] { 0, 2, 4, 6, 8, 16 };
			this.availGraphicsModes = new HashSet<GraphicsMode>(new GraphicsModeComparer());
			foreach (int samplecount in aaLevels)
			{
				GraphicsMode mode = new GraphicsMode(32, 24, 0, samplecount, new OpenTK.Graphics.ColorFormat(0), 2, false);
				if (!this.availGraphicsModes.Contains(mode)) this.availGraphicsModes.Add(mode);
			}
			int highestAALevel = MathF.RoundToInt(MathF.Log(MathF.Max(this.availGraphicsModes.Max(m => m.Samples), 1.0f), 2.0f));
			int targetAALevel = highestAALevel;
			if (DualityApp.AppData.MultisampleBackBuffer)
			{
				switch (DualityApp.UserData.AntialiasingQuality)
				{
					case AAQuality.High:	targetAALevel = highestAALevel;		break;
					case AAQuality.Medium:	targetAALevel = highestAALevel / 2; break;
					case AAQuality.Low:		targetAALevel = highestAALevel / 4; break;
					case AAQuality.Off:		targetAALevel = 0;					break;
				}
			}
			else
			{
				targetAALevel = 0;
			}
			int targetSampleCount = MathF.RoundToInt(MathF.Pow(2.0f, targetAALevel));
			this.defaultGraphicsMode = this.availGraphicsModes.LastOrDefault(m => m.Samples <= targetSampleCount) ?? this.availGraphicsModes.Last();
		}
		private void CheckContextCaps()
		{
			if (this.contextCapsRetrieved) return;
			this.contextCapsRetrieved = true;

			Logs.Core.Write("Determining OpenGL context capabilities...");
			Logs.Core.PushIndent();

			// Make sure we're not on a render target, which may override
			// some settings that we'd like to get from the main contexts
			// backbuffer.
			NativeRenderTarget oldTarget = NativeRenderTarget.BoundRT;
			NativeRenderTarget.Bind(null);

			int targetSamples = this.defaultGraphicsMode.Samples;
			int actualSamples;

			// Retrieve how many MSAA samples are actually available, despite what 
			// was offered and requested vis graphics mode.
			CheckOpenGLErrors(true);
			actualSamples = GL.GetInteger(GetPName.Samples);
			if (CheckOpenGLErrors()) actualSamples = targetSamples;

			// If the sample count differs, mention it in the logs. If it is
			// actually zero, assume MSAA is driver-disabled.
			if (targetSamples != actualSamples)
			{
				Logs.Core.Write("Requested {0} MSAA samples, but got {1} samples instead.", targetSamples, actualSamples);
				if (actualSamples == 0)
				{
					this.msaaIsDriverDisabled = true;
					Logs.Core.Write("Assuming MSAA is unavailable. Duality will not use Alpha-to-Coverage masking techniques.");
				}
			}

			NativeRenderTarget.Bind(oldTarget);

			Logs.Core.PopIndent();
		}

		private void PrepareRenderBatch(IDrawBatch renderBatch)
		{
			DrawTechnique technique = renderBatch.Material.Technique.Res ?? DrawTechnique.Solid.Res;
			NativeShaderProgram program = (technique.Shader.Res != null ? technique.Shader.Res.Native : null) as NativeShaderProgram;

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
						if (program != null)
						{
							ShaderFieldInfo[] varInfo = program.Fields;
							int[] locations = program.FieldLocations;

							int selectedVar = -1;
							for (int varIndex = 0; varIndex < varInfo.Length; varIndex++)
							{
								if (locations[varIndex] == -1) continue;
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

							GL.EnableVertexAttribArray(locations[selectedVar]);
							GL.VertexAttribPointer(
								locations[selectedVar], 
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
			NativeShaderProgram program = (technique.Shader.Res != null ? technique.Shader.Res.Native : null) as NativeShaderProgram;

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
						if (program != null)
						{
							ShaderFieldInfo[] varInfo = program.Fields;
							int[] locations = program.FieldLocations;

							int selectedVar = -1;
							for (int varIndex = 0; varIndex < varInfo.Length; varIndex++)
							{
								if (locations[varIndex] == -1) continue;
								if (!ShaderVarMatches(
									ref varInfo[varIndex],
									elements[elementIndex].Type, 
									elements[elementIndex].Count))
									continue;
								
								selectedVar = varIndex;
								break;
							}
							if (selectedVar == -1) break;

							GL.DisableVertexAttribArray(locations[selectedVar]);
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
			NativeShaderProgram shader = (tech.Shader.Res != null ? tech.Shader.Res.Native : null) as NativeShaderProgram;
			NativeShaderProgram.Bind(shader);

			// Setup shader data
			if (shader != null)
			{
				ShaderFieldInfo[] varInfo = shader.Fields;
				int[] locations = shader.FieldLocations;
				int[] builtinIndices = shader.BuiltinVariableIndex;

				// Setup sampler bindings automatically
				int curSamplerIndex = 0;
				if (material.Textures != null)
				{
					for (int i = 0; i < varInfo.Length; i++)
					{
						if (locations[i] == -1) continue;
						if (varInfo[i].Type != ShaderFieldType.Sampler2D) continue;

						// Bind Texture
						ContentRef<Texture> texRef = material.GetTexture(varInfo[i].Name);
						NativeTexture.Bind(texRef, curSamplerIndex);
						GL.Uniform1(locations[i], curSamplerIndex);

						curSamplerIndex++;
					}
				}
				NativeTexture.ResetBinding(curSamplerIndex);

				// Transfer uniform data from material to actual shader
				if (material.Uniforms != null)
				{
					for (int i = 0; i < varInfo.Length; i++)
					{
						if (locations[i] == -1) continue;
						float[] data = material.GetUniform(varInfo[i].Name);
						if (data == null) continue;

						NativeShaderProgram.SetUniform(ref varInfo[i], locations[i], data);
					}
				}

				// Specify builtin shader variables, if requested
				float[] fieldValue = null;
				for (int i = 0; i < builtinIndices.Length; i++)
				{
					if (BuiltinShaderFields.TryGetValue(this.currentDevice, builtinIndices[i], ref fieldValue))
						NativeShaderProgram.SetUniform(ref varInfo[i], locations[i], fieldValue);
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
					if (this.useAlphaToCoverageBlend)
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
				case VertexMode.Triangles:		return PrimitiveType.Triangles;
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

		public static void LogDisplayDevices()
		{
			Logs.Core.Write("Available display devices:");
			Logs.Core.PushIndent();
			foreach (DisplayIndex index in new[] { DisplayIndex.First, DisplayIndex.Second, DisplayIndex.Third, DisplayIndex.Fourth, DisplayIndex.Sixth } )
			{
				DisplayDevice display = DisplayDevice.GetDisplay(index);
				if (display == null) continue;

				Logs.Core.Write(
					"{0,-6}: {1,4}x{2,4} at {3,3} Hz, {4,2} bpp, pos [{5,4},{6,4}]{7}",
					index,
					display.Width,
					display.Height,
					display.RefreshRate,
					display.BitsPerPixel,
					display.Bounds.X,
					display.Bounds.Y,
					display.IsPrimary ? " (Primary)" : "");
			}
			Logs.Core.PopIndent();
		}
		public static void LogOpenGLSpecs()
		{
			// Accessing OpenGL functionality requires context. Don't get confused by AccessViolationExceptions, fail better instead.
			GraphicsContext.Assert();

			string versionString = null;
			try
			{
				CheckOpenGLErrors();
				versionString = GL.GetString(StringName.Version);
				Logs.Core.Write(
					"OpenGL Version: {0}" + Environment.NewLine +
					"Vendor: {1}" + Environment.NewLine +
					"Renderer: {2}" + Environment.NewLine +
					"Shader Version: {3}",
					versionString,
					GL.GetString(StringName.Vendor),
					GL.GetString(StringName.Renderer),
					GL.GetString(StringName.ShadingLanguageVersion));
				CheckOpenGLErrors();
			}
			catch (Exception e)
			{
				Logs.Core.WriteWarning("Can't determine OpenGL specs, because an error occurred: {0}", LogFormat.Exception(e));
			}

			// Parse the OpenGL version string in order to determine if it's sufficient
			if (versionString != null)
			{
				string[] token = versionString.Split(' ');
				for (int i = 0; i < token.Length; i++)
				{
					Version version;
					if (Version.TryParse(token[i], out version))
					{
						if (version.Major < MinOpenGLVersion.Major || (version.Major == MinOpenGLVersion.Major && version.Minor < MinOpenGLVersion.Minor))
						{
							Logs.Core.WriteWarning(
								"The detected OpenGL version {0} appears to be lower than the required minimum. Version {1} or higher is required to run Duality applications.",
								version,
								MinOpenGLVersion);
						}
						break;
					}
				}
			}
		}
		/// <summary>
		/// Checks for errors that might have occurred during video processing. You should avoid calling this method due to performance reasons.
		/// Only use it on suspect.
		/// </summary>
		/// <param name="silent">If true, errors aren't logged.</param>
		/// <returns>True, if an error occurred, false if not.</returns>
		public static bool CheckOpenGLErrors(bool silent = false, [CallerMemberName] string callerInfoMember = null, [CallerFilePath] string callerInfoFile = null, [CallerLineNumber] int callerInfoLine = -1)
		{
			// Accessing OpenGL functionality requires context. Don't get confused by AccessViolationExceptions, fail better instead.
			GraphicsContext.Assert();

			ErrorCode error;
			bool found = false;
			while ((error = GL.GetError()) != ErrorCode.NoError)
			{
				if (!silent)
				{
					Logs.Core.WriteError(
						"Internal OpenGL error, code {0} at {1} in {2}, line {3}.", 
						error,
						callerInfoMember,
						callerInfoFile,
						callerInfoLine);
				}
				found = true;
			}
			if (found && !silent && System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
			return found;
		}
		/// <summary>
		/// Checks for OpenGL errors using <see cref="CheckOpenGLErrors"/> when both compiled in debug mode and a with an attached debugger.
		/// </summary>
		/// <returns></returns>
		[System.Diagnostics.Conditional("DEBUG")]
		public static void DebugCheckOpenGLErrors([CallerMemberName] string callerInfoMember = null, [CallerFilePath] string callerInfoFile = null, [CallerLineNumber] int callerInfoLine = -1)
		{
			if (!System.Diagnostics.Debugger.IsAttached) return;
			CheckOpenGLErrors(false, callerInfoMember, callerInfoFile, callerInfoLine);
		}
	}
}
