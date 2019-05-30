using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class GraphicsBackend : IGraphicsBackend
	{
		public static readonly Version MinOpenGLVersion = new Version(3, 0);

		private static GraphicsBackend activeInstance = null;
		public static GraphicsBackend ActiveInstance
		{
			get { return activeInstance; }
		}


		private OpenTKGraphicsCapabilities   capabilities            = new OpenTKGraphicsCapabilities();
		private IDrawDevice                  currentDevice           = null;
		private RenderOptions                renderOptions           = null;
		private RenderStats                  renderStats             = null;
		private HashSet<GraphicsMode>        availGraphicsModes      = null;
		private GraphicsMode                 defaultGraphicsMode     = null;
		private NativeGraphicsBuffer         sharedBatchIBO          = null;
		private RawList<ushort>              sharedBatchIndices      = new RawList<ushort>();
		private NativeWindow                 activeWindow            = null;
		private Point2                       externalBackbufferSize  = Point2.Zero;
		private bool                         useAlphaToCoverageBlend = false;
		private bool                         msaaIsDriverDisabled    = false;
		private bool                         contextCapsRetrieved    = false;
		private HashSet<NativeShaderProgram> activeShaders           = new HashSet<NativeShaderProgram>();
		private HashSet<string>              sharedShaderParameters  = new HashSet<string>();
		private int                          sharedSamplerBindings   = 0;
		private ShaderParameterCollection    internalShaderState     = new ShaderParameterCollection();
		

		public GraphicsBackendCapabilities Capabilities
		{
			get { return this.capabilities; }
		}
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
			DefaultOpenTKBackendPlugin.DetectInputDevices();

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

			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
			{
				DefaultOpenTKBackendPlugin.GuardSingleThreadState();
				if (this.sharedBatchIBO != null)
				{
					this.sharedBatchIBO.Dispose();
					this.sharedBatchIBO = null;
				}
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
			this.CheckRenderingCapabilities();

			this.currentDevice = device;
			this.renderOptions = options;
			this.renderStats = stats;

			// Prepare a shared index buffer object, in case we don't have one yet
			if (this.sharedBatchIBO == null)
				this.sharedBatchIBO = new NativeGraphicsBuffer(GraphicsBufferType.Index);

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
			GL.Enable(EnableCap.ScissorTest);
			GL.Enable(EnableCap.DepthTest);
			if (options.DepthTest)
				GL.DepthFunc(DepthFunction.Lequal);
			else
				GL.DepthFunc(DepthFunction.Always);

			// Prepare shared matrix stack for rendering
			Matrix4 viewMatrix = options.ViewMatrix;
			Matrix4 projectionMatrix = options.ProjectionMatrix;

			if (NativeRenderTarget.BoundRT != null)
			{
				Matrix4 flipOutput = Matrix4.CreateScale(1.0f, -1.0f, 1.0f);
				projectionMatrix = projectionMatrix * flipOutput;
			}

			this.renderOptions.ShaderParameters.Set(
				BuiltinShaderFields.ViewMatrix,
				viewMatrix);
			this.renderOptions.ShaderParameters.Set(
				BuiltinShaderFields.ProjectionMatrix,
				projectionMatrix);
			this.renderOptions.ShaderParameters.Set(
				BuiltinShaderFields.ViewProjectionMatrix,
				viewMatrix * projectionMatrix);
		}
		void IGraphicsBackend.Render(IReadOnlyList<DrawBatch> batches)
		{
			if (batches.Count == 0) return;

			this.RetrieveActiveShaders(batches);
			this.SetupSharedParameters(this.renderOptions.ShaderParameters);

			int drawCalls = 0;
			DrawBatch lastRendered = null;
			for (int i = 0; i < batches.Count; i++)
			{
				DrawBatch batch = batches[i];
				VertexDeclaration vertexType = batch.VertexBuffer.VertexType;

				// Bind the vertex buffer we'll use. Note that this needs to be done
				// before setting up any vertex format state.
				NativeGraphicsBuffer vertexBuffer = batch.VertexBuffer.NativeVertex as NativeGraphicsBuffer;
				NativeGraphicsBuffer.Bind(GraphicsBufferType.Vertex, vertexBuffer);

				bool first = (i == 0);
				bool sameMaterial = 
					lastRendered != null && 
					lastRendered.Material.Equals(batch.Material);

				// Setup vertex bindings. Note that the setup differs based on the 
				// materials shader, so material changes can be vertex binding changes.
				if (lastRendered != null)
				{
					this.FinishVertexFormat(lastRendered.Material, lastRendered.VertexBuffer.VertexType);
				}
				this.SetupVertexFormat(batch.Material, vertexType);

				// Setup material when changed.
				if (!sameMaterial)
				{
					this.SetupMaterial(
						batch.Material, 
						lastRendered != null ? lastRendered.Material : null);
				}

				// Draw the current batch
				this.DrawVertexBatch(
					batch.VertexBuffer, 
					batch.VertexRanges, 
					batch.VertexMode);

				drawCalls++;
				lastRendered = batch;
			}

			// Cleanup after rendering
			NativeGraphicsBuffer.Bind(GraphicsBufferType.Vertex, null);
			NativeGraphicsBuffer.Bind(GraphicsBufferType.Index, null);
			if (lastRendered != null)
			{
				this.FinishMaterial(lastRendered.Material);
				this.FinishVertexFormat(lastRendered.Material, lastRendered.VertexBuffer.VertexType);
			}

			if (this.renderStats != null)
			{
				this.renderStats.DrawCalls += drawCalls;
			}

			this.FinishSharedParameters();
		}
		void IGraphicsBackend.EndRendering()
		{
			this.currentDevice = null;
			this.renderOptions = null;
			this.renderStats = null;

			DebugCheckOpenGLErrors();
		}
		
		INativeGraphicsBuffer IGraphicsBackend.CreateBuffer(GraphicsBufferType type)
		{
			return new NativeGraphicsBuffer(type);
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
			this.activeWindow = new NativeWindow(this.defaultGraphicsMode, options);
			return this.activeWindow;
		}

		void IGraphicsBackend.GetOutputPixelData(IntPtr buffer, ColorDataLayout dataLayout, ColorDataElementType dataElementType, int x, int y, int width, int height)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			NativeRenderTarget lastRt = NativeRenderTarget.BoundRT;
			NativeRenderTarget.Bind(null);
			{
				// Use a temporary local buffer, since the image will be upside-down because
				// of OpenGL's coordinate system and we'll need to flip it before returning.
				byte[] byteData = new byte[width * height * 4];
				
				// Retrieve pixel data
				GL.ReadPixels(x, y, width, height, dataLayout.ToOpenTK(), dataElementType.ToOpenTK(), byteData);
				
				// Flip the retrieved image vertically
				int bytesPerLine = width * 4;
				byte[] switchLine = new byte[width * 4];
				for (int flipY = 0; flipY < height / 2; flipY++)
				{
					int lineIndex = flipY * width * 4;
					int lineIndex2 = (height - 1 - flipY) * width * 4;
					
					// Copy the current line to the switch buffer
					for (int lineX = 0; lineX < bytesPerLine; lineX++)
					{
						switchLine[lineX] = byteData[lineIndex + lineX];
					}

					// Copy the opposite line to the current line
					for (int lineX = 0; lineX < bytesPerLine; lineX++)
					{
						byteData[lineIndex + lineX] = byteData[lineIndex2 + lineX];
					}

					// Copy the switch buffer to the opposite line
					for (int lineX = 0; lineX < bytesPerLine; lineX++)
					{
						byteData[lineIndex2 + lineX] = switchLine[lineX];
					}
				}
				
				// Copy the flipped data to the output buffer
				Marshal.Copy(byteData, 0, buffer, width * height * 4);
			}
			NativeRenderTarget.Bind(lastRt);
		}

		public void QueryOpenGLCapabilities()
		{
			// Retrieve and log GL version as well as detected capabilities and limits
			this.capabilities.RetrieveFromAPI();
			this.capabilities.WriteToLog(Logs.Core);

			// Log a warning if the detected GL version is below our supported minspec
			Version glVersion = this.capabilities.GLVersion;
			if (glVersion < MinOpenGLVersion)
			{
				Logs.Core.WriteWarning(
					"The detected OpenGL version {0} appears to be lower than the required minimum. Version {1} or higher is required to run Duality applications.",
					glVersion,
					MinOpenGLVersion);
			}
		}
		private void QueryGraphicsModes()
		{
			// Gather available graphics modes
			Logs.Core.Write("Available graphics modes:");
			Logs.Core.PushIndent();
			int[] aaLevels = new int[] { 0, 2, 4, 6, 8, 16 };
			this.availGraphicsModes = new HashSet<GraphicsMode>(new GraphicsModeComparer());
			foreach (int samplecount in aaLevels)
			{
				GraphicsMode mode = new GraphicsMode(32, 24, 0, samplecount, new OpenTK.Graphics.ColorFormat(0), 2, false);
				if (!this.availGraphicsModes.Contains(mode))
				{
					this.availGraphicsModes.Add(mode);
					Logs.Core.Write("{0}", mode);
				}
			}
			Logs.Core.PopIndent();

			// Select the default graphics mode we'll prefer for game window and other rendering surfaces
			List<GraphicsMode> sortedModes = this.availGraphicsModes.ToList();
			sortedModes.StableSort((a, b) => a.Samples - b.Samples);
			int highestAALevel = MathF.RoundToInt(MathF.Log(MathF.Max(sortedModes.Max(m => m.Samples), 1.0f), 2.0f));
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
			this.defaultGraphicsMode = sortedModes.LastOrDefault(m => m.Samples <= targetSampleCount) ?? sortedModes.Last();

			Logs.Core.Write("Duality default graphics mode: {0}", this.defaultGraphicsMode);
			Logs.Core.Write("OpenTK default graphics mode: {0}", GraphicsMode.Default);
		}
		private void CheckRenderingCapabilities()
		{
			if (this.contextCapsRetrieved) return;
			this.contextCapsRetrieved = true;

			Logs.Core.Write("Determining OpenGL rendering capabilities...");
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

		/// <summary>
		/// Updates the internal list of active shaders based on the specified rendering batches.
		/// </summary>
		/// <param name="batches"></param>
		private void RetrieveActiveShaders(IReadOnlyList<DrawBatch> batches)
		{
			this.activeShaders.Clear();
			for (int i = 0; i < batches.Count; i++)
			{
				DrawBatch batch = batches[i];
				BatchInfo material = batch.Material;
				DrawTechnique tech = material.Technique.Res ?? DrawTechnique.Solid.Res;
				this.activeShaders.Add(tech.NativeShader as NativeShaderProgram);
			}
		}
		/// <summary>
		/// Applies the specified parameter values to all currently active shaders.
		/// </summary>
		/// <param name="sharedParams"></param>
		/// <seealso cref="RetrieveActiveShaders"/>
		private void SetupSharedParameters(ShaderParameterCollection sharedParams)
		{
			this.sharedSamplerBindings = 0;
			this.sharedShaderParameters.Clear();
			if (sharedParams == null) return;

			foreach (NativeShaderProgram shader in this.activeShaders)
			{
				NativeShaderProgram.Bind(shader);

				ShaderFieldInfo[] varInfo = shader.Fields;
				int[] locations = shader.FieldLocations;

				// Setup shared sampler bindings and uniform data
				for (int i = 0; i < varInfo.Length; i++)
				{
					ShaderFieldInfo field = varInfo[i];
					int location = locations[i];

					if (field.Scope == ShaderFieldScope.Attribute) continue;
					if (field.Type == ShaderFieldType.Sampler2D)
					{
						ContentRef<Texture> texRef;
						if (!sharedParams.TryGetInternal(field.Name, out texRef)) continue;

						NativeTexture.Bind(texRef, this.sharedSamplerBindings);
						GL.Uniform1(location, this.sharedSamplerBindings);

						this.sharedSamplerBindings++;
					}
					else
					{
						float[] data;
						if (!sharedParams.TryGetInternal(field.Name, out data)) continue;

						NativeShaderProgram.SetUniform(field, location, data);
					}

					this.sharedShaderParameters.Add(field.Name);
				}
			}

			NativeShaderProgram.Bind(null);
		}
		private void SetupVertexFormat(BatchInfo material, VertexDeclaration vertexDeclaration)
		{
			DrawTechnique technique = material.Technique.Res ?? DrawTechnique.Solid.Res;
			NativeShaderProgram nativeProgram = (technique.NativeShader ?? DrawTechnique.Solid.Res.NativeShader) as NativeShaderProgram;

			VertexElement[] elements = vertexDeclaration.Elements;
			for (int elementIndex = 0; elementIndex < elements.Length; elementIndex++)
			{
				int fieldIndex = nativeProgram.SelectField(ref elements[elementIndex]);
				if (fieldIndex == -1) continue;

				VertexAttribPointerType attribType;
				switch (elements[elementIndex].Type)
				{
					default:
					case VertexElementType.Float: attribType = VertexAttribPointerType.Float; break;
					case VertexElementType.Byte: attribType = VertexAttribPointerType.UnsignedByte; break;
				}

				int fieldLocation = nativeProgram.FieldLocations[fieldIndex];
				GL.EnableVertexAttribArray(fieldLocation);
				GL.VertexAttribPointer(
					fieldLocation, 
					elements[elementIndex].Count, 
					attribType, 
					true, 
					vertexDeclaration.Size, 
					elements[elementIndex].Offset);
			}
		}
		private void SetupMaterial(BatchInfo material, BatchInfo lastMaterial)
		{
			DrawTechnique tech = material.Technique.Res ?? DrawTechnique.Solid.Res;
			DrawTechnique lastTech = lastMaterial != null ? lastMaterial.Technique.Res : null;

			// Setup BlendType
			if (lastTech == null || tech.Blending != lastTech.Blending)
				this.SetupBlendState(tech.Blending);

			// Bind Shader
			NativeShaderProgram nativeShader = tech.NativeShader as NativeShaderProgram;
			NativeShaderProgram.Bind(nativeShader);

			// Setup shader data
			ShaderFieldInfo[] varInfo = nativeShader.Fields;
			int[] locations = nativeShader.FieldLocations;

			// Setup sampler bindings and uniform data
			int curSamplerIndex = this.sharedSamplerBindings;
			for (int i = 0; i < varInfo.Length; i++)
			{
				ShaderFieldInfo field = varInfo[i];
				int location = locations[i];

				if (field.Scope == ShaderFieldScope.Attribute) continue;
				if (this.sharedShaderParameters.Contains(field.Name)) continue;

				if (field.Type == ShaderFieldType.Sampler2D)
				{
					ContentRef<Texture> texRef = material.GetInternalTexture(field.Name);
					if (texRef == null) this.internalShaderState.TryGetInternal(field.Name, out texRef);

					NativeTexture.Bind(texRef, curSamplerIndex);
					GL.Uniform1(location, curSamplerIndex);

					curSamplerIndex++;
				}
				else
				{
					float[] data = material.GetInternalData(field.Name);
					if (data == null && !this.internalShaderState.TryGetInternal(field.Name, out data))
						continue;

					NativeShaderProgram.SetUniform(field, location, data);
				}
			}
			NativeTexture.ResetBinding(curSamplerIndex);
		}
		private void SetupBlendState(BlendMode mode)
		{
			bool depthWrite = this.renderOptions.DepthWrite;
			bool useAlphaTesting = false;
			switch (mode)
			{
				default:
				case BlendMode.Solid:
					GL.DepthMask(depthWrite);
					GL.Disable(EnableCap.Blend);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					break;
				case BlendMode.Mask:
					GL.DepthMask(depthWrite);
					GL.Disable(EnableCap.Blend);
					if (this.useAlphaToCoverageBlend)
						GL.Enable(EnableCap.SampleAlphaToCoverage);
					else
						useAlphaTesting = true;
					break;
				case BlendMode.Alpha:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
					break;
				case BlendMode.AlphaPre:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
					break;
				case BlendMode.Add:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One, BlendingFactorSrc.One, BlendingFactorDest.One);
					break;
				case BlendMode.Light:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFuncSeparate(BlendingFactorSrc.DstColor, BlendingFactorDest.One, BlendingFactorSrc.Zero, BlendingFactorDest.One);
					break;
				case BlendMode.Multiply:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.Zero);
					break;
				case BlendMode.Invert:
					GL.DepthMask(false);
					GL.Enable(EnableCap.Blend);
					GL.Disable(EnableCap.SampleAlphaToCoverage);
					GL.BlendFunc(BlendingFactorSrc.OneMinusDstColor, BlendingFactorDest.OneMinusSrcColor);
					break;
			}

			this.internalShaderState.Set(
				BuiltinShaderFields.AlphaTestThreshold, 
				useAlphaTesting  ? 0.5f : -1.0f);
		}

		/// <summary>
		/// Draws the vertices of a single <see cref="DrawBatch"/>, after all other rendering state
		/// has been set up accordingly outside this method.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="ranges"></param>
		/// <param name="mode"></param>
		private void DrawVertexBatch(VertexBuffer buffer, RawList<VertexDrawRange> ranges, VertexMode mode)
		{
			NativeGraphicsBuffer indexBuffer = (buffer.IndexCount > 0 ? buffer.NativeIndex : null) as NativeGraphicsBuffer;
			IndexDataElementType indexType = buffer.IndexType;

			// Since the QUADS primitive is deprecated in OpenGL 3.0 and not available in OpenGL ES,
			// we'll emulate this with an ad-hoc index buffer object that we generate here.
			if (mode == VertexMode.Quads)
			{
				if (indexBuffer != null)
				{
					Logs.Core.WriteWarning(
						"Rendering {0} instances that use index buffers is not supported for quads geometry. " +
						"To use index buffers, use any other geometry type listed in {1}. Skipping batch.",
						typeof(DrawBatch).Name,
						typeof(VertexMode).Name);
					return;
				}

				NativeGraphicsBuffer.Bind(GraphicsBufferType.Index, this.sharedBatchIBO);

				this.GenerateQuadIndices(ranges, this.sharedBatchIndices);
				this.sharedBatchIBO.LoadData(
					this.sharedBatchIndices.Data,
					0,
					this.sharedBatchIndices.Count);
				
				GL.DrawElements(
					PrimitiveType.Triangles, 
					this.sharedBatchIndices.Count, 
					DrawElementsType.UnsignedShort, 
					IntPtr.Zero);

				this.sharedBatchIndices.Clear();
			}
			// Otherwise, run the regular rendering path
			else
			{
				// Rendering using index buffer
				if (indexBuffer != null)
				{
					if (ranges != null && ranges.Count > 0)
					{
						Logs.Core.WriteWarning(
							"Rendering {0} instances that use index buffers do not support specifying vertex ranges, " +
							"since the two features are mutually exclusive.",
							typeof(DrawBatch).Name,
							typeof(VertexMode).Name);
					}

					NativeGraphicsBuffer.Bind(GraphicsBufferType.Index, indexBuffer);

					PrimitiveType openTkMode = GetOpenTKVertexMode(mode);
					DrawElementsType openTkIndexType = GetOpenTKIndexType(indexType);
					GL.DrawElements(
						openTkMode,
						buffer.IndexCount,
						openTkIndexType,
						IntPtr.Zero);
				}
				// Rendering using an array of vertex ranges
				else
				{
					NativeGraphicsBuffer.Bind(GraphicsBufferType.Index, null);

					PrimitiveType openTkMode = GetOpenTKVertexMode(mode);
					VertexDrawRange[] rangeData = ranges.Data;
					int rangeCount = ranges.Count;
					for (int r = 0; r < rangeCount; r++)
					{
						GL.DrawArrays(
							openTkMode,
							rangeData[r].Index,
							rangeData[r].Count);
					}
				}
			}
		}
		/// <summary>
		/// Given a list of vertex drawing ranges using <see cref="VertexMode.Quads"/>, this method
		/// generates a single list of indices for <see cref="VertexMode.Triangles"/> that draws the same.
		/// </summary>
		/// <param name="ranges"></param>
		/// <param name="indices"></param>
		private void GenerateQuadIndices(RawList<VertexDrawRange> ranges, RawList<ushort> indices)
		{
			VertexDrawRange[] rangeData = ranges.Data;
			int rangeCount = ranges.Count;

			int elementIndex = indices.Count;
			for (int r = 0; r < rangeCount; r++)
			{
				int quadCount = rangeData[r].Count / 4;
				int elementCount = quadCount * 6;

				indices.Count += elementCount;
				ushort[] indexData = indices.Data;

				int vertexIndex = rangeData[r].Index;
				for (int quadIndex = 0; quadIndex < quadCount; quadIndex++)
				{
					// First triangle
					indexData[elementIndex + 0] = (ushort)(vertexIndex + 0);
					indexData[elementIndex + 1] = (ushort)(vertexIndex + 1);
					indexData[elementIndex + 2] = (ushort)(vertexIndex + 2);

					// Second triangle
					indexData[elementIndex + 3] = (ushort)(vertexIndex + 2);
					indexData[elementIndex + 4] = (ushort)(vertexIndex + 3);
					indexData[elementIndex + 5] = (ushort)(vertexIndex + 0);

					elementIndex += 6;
					vertexIndex += 4;
				}
			}
		}

		private void FinishSharedParameters()
		{
			NativeTexture.ResetBinding();

			this.sharedSamplerBindings = 0;
			this.sharedShaderParameters.Clear();
			this.activeShaders.Clear();
		}
		private void FinishVertexFormat(BatchInfo material, VertexDeclaration vertexDeclaration)
		{
			DrawTechnique technique = material.Technique.Res ?? DrawTechnique.Solid.Res;
			NativeShaderProgram nativeProgram = (technique.NativeShader ?? DrawTechnique.Solid.Res.NativeShader) as NativeShaderProgram;

			VertexElement[] elements = vertexDeclaration.Elements;
			for (int elementIndex = 0; elementIndex < elements.Length; elementIndex++)
			{
				int fieldIndex = nativeProgram.SelectField(ref elements[elementIndex]);
				if (fieldIndex == -1) break;
							
				int fieldLocation = nativeProgram.FieldLocations[fieldIndex];
				GL.DisableVertexAttribArray(fieldLocation);
			}
		}
		private void FinishMaterial(BatchInfo material)
		{
			DrawTechnique tech = material.Technique.Res;
			this.FinishBlendState();
			NativeShaderProgram.Bind(null);
			NativeTexture.ResetBinding(this.sharedSamplerBindings);
		}
		private void FinishBlendState()
		{
			GL.DepthMask(true);
			GL.Disable(EnableCap.Blend);
			GL.Disable(EnableCap.SampleAlphaToCoverage);
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
			}
		}
		private static DrawElementsType GetOpenTKIndexType(IndexDataElementType indexType)
		{
			switch (indexType)
			{
				default:
				case IndexDataElementType.UnsignedByte: return DrawElementsType.UnsignedByte;
				case IndexDataElementType.UnsignedShort: return DrawElementsType.UnsignedShort;
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

		public static void LogDisplayDevices()
		{
			Logs.Core.Write("Available display devices:");
			Logs.Core.PushIndent();
			foreach (DisplayIndex index in new[] { DisplayIndex.First, DisplayIndex.Second, DisplayIndex.Third, DisplayIndex.Fourth, DisplayIndex.Fifth, DisplayIndex.Sixth } )
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
		public static void LogOpenGLContextSpecs(IGraphicsContext context)
		{
			Logs.Core.Write(
				"Context specs: " + Environment.NewLine +
				"  Buffers: {0}" + Environment.NewLine +
				"  Samples: {1}" + Environment.NewLine +
				"  ColorFormat: {2}" + Environment.NewLine +
				"  AccumFormat: {3}" + Environment.NewLine +
				"  Depth: {4}" + Environment.NewLine +
				"  Stencil: {5}" + Environment.NewLine +
				"  SwapInterval: {6}",
				context.GraphicsMode.Buffers,
				context.GraphicsMode.Samples,
				context.GraphicsMode.ColorFormat,
				context.GraphicsMode.AccumulatorFormat,
				context.GraphicsMode.Depth,
				context.GraphicsMode.Stencil,
				context.SwapInterval);
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
