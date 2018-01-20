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
	public class GraphicsBackend : IGraphicsBackend
	{
		private static readonly Version MinOpenGLVersion = new Version(3, 0);

		private static GraphicsBackend activeInstance = null;
		public static GraphicsBackend ActiveInstance
		{
			get { return activeInstance; }
		}

		private IDrawDevice                  currentDevice           = null;
		private RenderOptions                renderOptions           = null;
		private RenderStats                  renderStats             = null;
		private HashSet<GraphicsMode>        availGraphicsModes      = null;
		private GraphicsMode                 defaultGraphicsMode     = null;
		private RawList<uint>                perVertexTypeVBO        = new RawList<uint>();
		private int                          sharedBatchIBO          = 0;
		private RawList<uint>                sharedBatchIndices      = new RawList<uint>();
		private NativeWindow                 activeWindow            = null;
		private Point2                       externalBackbufferSize  = Point2.Zero;
		private bool                         useAlphaToCoverageBlend = false;
		private bool                         msaaIsDriverDisabled    = false;
		private bool                         contextCapsRetrieved    = false;
		private HashSet<NativeShaderProgram> activeShaders           = new HashSet<NativeShaderProgram>();
		private HashSet<string>              sharedShaderParameters  = new HashSet<string>();
		private int                          sharedSamplerBindings   = 0;
		private ShaderParameterCollection    internalShaderState     = new ShaderParameterCollection();
		

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

			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
			{
				DefaultOpenTKBackendPlugin.GuardSingleThreadState();
				for (int i = 0; i < this.perVertexTypeVBO.Count; i++)
				{
					uint handle = this.perVertexTypeVBO[i];
					if (handle != 0)
					{
						GL.DeleteBuffers(1, ref handle);
					}
				}
				this.perVertexTypeVBO.Clear();

				if (this.sharedBatchIBO != 0)
				{
					GL.DeleteBuffers(1, ref this.sharedBatchIBO);
					this.sharedBatchIBO = 0;
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

		void IGraphicsBackend.BeginRendering(IDrawDevice device, VertexBatchStore vertexData, RenderOptions options, RenderStats stats)
		{
			DebugCheckOpenGLErrors();
			this.CheckContextCaps();

			this.currentDevice = device;
			this.renderOptions = options;
			this.renderStats = stats;

			// Upload all vertex data that we'll need during rendering
			if (vertexData != null)
			{
				this.perVertexTypeVBO.Count = Math.Max(this.perVertexTypeVBO.Count, vertexData.Batches.Count);
				for (int typeIndex = 0; typeIndex < vertexData.Batches.Count; typeIndex++)
				{
					// Filter out unused vertex types
					IVertexBatch vertexBatch = vertexData.Batches[typeIndex];
					if (vertexBatch == null) continue;
					if (vertexBatch.Count == 0) continue;

					// Generate a VBO for this vertex type if it didn't exist yet
					if (this.perVertexTypeVBO[typeIndex] == 0)
					{
						GL.GenBuffers(1, out this.perVertexTypeVBO.Data[typeIndex]);
					}
					GL.BindBuffer(BufferTarget.ArrayBuffer, this.perVertexTypeVBO[typeIndex]);
				
					// Upload all data of this vertex type as a single block
					int vertexDataLength = vertexBatch.Declaration.Size * vertexBatch.Count;
					using (PinnedArrayHandle pinned = vertexBatch.Lock())
					{
						GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)vertexDataLength, IntPtr.Zero, BufferUsageHint.StreamDraw);
						GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)vertexDataLength, pinned.Address, BufferUsageHint.StreamDraw);
					}
				}
			}
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			// Prepare a shared index buffer object, in case we don't have one yet
			if (this.sharedBatchIBO == 0)
			{
				GL.GenBuffers(1, out this.sharedBatchIBO);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.sharedBatchIBO);
				GL.BufferData(BufferTarget.ElementArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StreamDraw);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			}

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
			if (options.RenderMode == RenderMode.Screen)
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

			// Prepare shared matrix stack for rendering
			Matrix4 viewMatrix = options.ViewMatrix;
			Matrix4 projectionMatrix = options.ProjectionMatrix;

			if (NativeRenderTarget.BoundRT != null)
			{
				Matrix4 flipOutput = Matrix4.CreateScale(1.0f, -1.0f, 1.0f);
				projectionMatrix = flipOutput * projectionMatrix;

				if (options.RenderMode == RenderMode.Screen)
				{
					Matrix4 offsetOutput = Matrix4.CreateTranslation(0.0f, -device.TargetSize.Y, 0.0f);
					projectionMatrix = offsetOutput * projectionMatrix;
				}
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
				VertexDeclaration vertexType = batch.VertexType;

				GL.BindBuffer(BufferTarget.ArrayBuffer, this.perVertexTypeVBO[vertexType.TypeIndex]);
				
				bool first = (i == 0);
				bool sameMaterial = 
					lastRendered != null && 
					lastRendered.Material.Equals(batch.Material);

				// Setup vertex bindings. Note that the setup differs based on the 
				// materials shader, so material changes can be vertex binding changes.
				if (lastRendered != null)
				{
					this.FinishVertexFormat(lastRendered.Material, lastRendered.VertexType);
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
				this.DrawVertexBatch(batch.VertexRanges, batch.VertexMode);

				drawCalls++;
				lastRendered = batch;
			}
			
			// Cleanup after rendering
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			if (lastRendered != null)
			{
				this.FinishMaterial(lastRendered.Material);
				this.FinishVertexFormat(lastRendered.Material, lastRendered.VertexType);
			}

			if (this.renderStats != null)
			{
				this.renderStats.DrawCalls += drawCalls;
			}

			this.FinishSharedParameters();
		}
		void IGraphicsBackend.EndRendering()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			this.currentDevice = null;
			this.renderOptions = null;
			this.renderStats = null;

			DebugCheckOpenGLErrors();
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
				ShaderProgram shader = tech.Shader.Res ?? ShaderProgram.Minimal.Res;
				this.activeShaders.Add(shader.Native as NativeShaderProgram);
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

				// Setup shared sampler bindings
				for (int i = 0; i < varInfo.Length; i++)
				{
					if (locations[i] == -1) continue;
					if (varInfo[i].Type != ShaderFieldType.Sampler2D) continue;

					ContentRef<Texture> texRef;
					if (!sharedParams.TryGetInternal(varInfo[i].Name, out texRef)) continue;

					NativeTexture.Bind(texRef, this.sharedSamplerBindings);
					GL.Uniform1(locations[i], this.sharedSamplerBindings);

					this.sharedSamplerBindings++;
					this.sharedShaderParameters.Add(varInfo[i].Name);
				}

				// Setup shared uniform data
				for (int i = 0; i < varInfo.Length; i++)
				{
					if (locations[i] == -1) continue;
					if (varInfo[i].Type == ShaderFieldType.Sampler2D) continue;

					float[] data;
					if (!sharedParams.TryGetInternal(varInfo[i].Name, out data)) continue;
		
					NativeShaderProgram.SetUniform(ref varInfo[i], locations[i], data);

					this.sharedShaderParameters.Add(varInfo[i].Name);
				}
			}

			NativeShaderProgram.Bind(null);
		}
		private void SetupVertexFormat(BatchInfo material, VertexDeclaration vertexDeclaration)
		{
			DrawTechnique technique = material.Technique.Res ?? DrawTechnique.Solid.Res;
			ShaderProgram program = technique.Shader.Res ?? ShaderProgram.Minimal.Res;
			NativeShaderProgram nativeProgram = (program.Native ?? ShaderProgram.Minimal.Res.Native) as NativeShaderProgram;

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
				this.SetupBlendState(tech.Blending, this.currentDevice.DepthWrite);

			// Bind Shader
			ShaderProgram shader = tech.Shader.Res ?? ShaderProgram.Minimal.Res;
			NativeShaderProgram nativeShader = shader.Native as NativeShaderProgram;
			NativeShaderProgram.Bind(nativeShader);

			// Setup shader data
			ShaderFieldInfo[] varInfo = nativeShader.Fields;
			int[] locations = nativeShader.FieldLocations;

			// Setup sampler bindings
			int curSamplerIndex = this.sharedSamplerBindings;
			for (int i = 0; i < varInfo.Length; i++)
			{
				if (locations[i] == -1) continue;
				if (varInfo[i].Type != ShaderFieldType.Sampler2D) continue;
				if (this.sharedShaderParameters.Contains(varInfo[i].Name)) continue;

				ContentRef<Texture> texRef = material.GetInternalTexture(varInfo[i].Name);
				if (texRef == null) this.internalShaderState.TryGetInternal(varInfo[i].Name, out texRef);

				NativeTexture.Bind(texRef, curSamplerIndex);
				GL.Uniform1(locations[i], curSamplerIndex);

				curSamplerIndex++;
			}
			NativeTexture.ResetBinding(curSamplerIndex);

			// Setup uniform data
			for (int i = 0; i < varInfo.Length; i++)
			{
				if (locations[i] == -1) continue;
				if (varInfo[i].Type == ShaderFieldType.Sampler2D) continue;
				if (this.sharedShaderParameters.Contains(varInfo[i].Name)) continue;

				float[] data = material.GetInternalData(varInfo[i].Name);
				if (data == null && !this.internalShaderState.TryGetInternal(varInfo[i].Name, out data))
					continue;

				NativeShaderProgram.SetUniform(ref varInfo[i], locations[i], data);
			}
		}
		private void SetupBlendState(BlendMode mode, bool depthWrite)
		{
			bool useAlphaTesting = false;
			switch (mode)
			{
				default:
				case BlendMode.Reset:
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

		private void DrawVertexBatch(RawList<VertexDrawRange> ranges, VertexMode mode)
		{
			VertexDrawRange[] rangeData = ranges.Data;
			int rangeCount = ranges.Count;

			// Since the QUADS primitive is deprecated in OpenGL 3.0 and not available in OpenGL ES,
			// we'll emulate this with an ad-hoc index buffer object that we generate here.
			if (mode == VertexMode.Quads)
			{
				this.GenerateQuadIndices(ranges, this.sharedBatchIndices);

				GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.sharedBatchIBO);

				GL.BufferData(BufferTarget.ElementArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StreamDraw);
				GL.BufferData(BufferTarget.ElementArrayBuffer, 
					this.sharedBatchIndices.Count * sizeof(uint), 
					this.sharedBatchIndices.Data, 
					BufferUsageHint.StreamDraw);

				GL.DrawElements(
					PrimitiveType.Triangles, 
					this.sharedBatchIndices.Count, 
					DrawElementsType.UnsignedInt, 
					IntPtr.Zero);

				this.sharedBatchIndices.Clear();
			}
			// In all other cases, we can just forward to a regular DrawArrays call.
			else
			{
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

				PrimitiveType openTkMode = GetOpenTKVertexMode(mode);
				for (int r = 0; r < rangeCount; r++)
				{
					GL.DrawArrays(
						openTkMode,
						rangeData[r].Index,
						rangeData[r].Count);
				}
			}
		}
		/// <summary>
		/// Given a list of vertex drawing ranges using <see cref="VertexMode.Quads"/>, this method
		/// generates a single list of indices for <see cref="VertexMode.Triangles"/> that draws the same.
		/// </summary>
		/// <param name="ranges"></param>
		/// <param name="indices"></param>
		private void GenerateQuadIndices(RawList<VertexDrawRange> ranges, RawList<uint> indices)
		{
			VertexDrawRange[] rangeData = ranges.Data;
			int rangeCount = ranges.Count;

			int elementIndex = indices.Count;
			for (int r = 0; r < rangeCount; r++)
			{
				int quadCount = rangeData[r].Count / 4;
				int elementCount = quadCount * 6;

				indices.Count += elementCount;
				uint[] indexData = indices.Data;

				int vertexIndex = rangeData[r].Index;
				for (int quadIndex = 0; quadIndex < quadCount; quadIndex++)
				{
					// First triangle
					indexData[elementIndex + 0] = (uint)(vertexIndex + 0);
					indexData[elementIndex + 1] = (uint)(vertexIndex + 1);
					indexData[elementIndex + 2] = (uint)(vertexIndex + 2);

					// Second triangle
					indexData[elementIndex + 3] = (uint)(vertexIndex + 2);
					indexData[elementIndex + 4] = (uint)(vertexIndex + 3);
					indexData[elementIndex + 5] = (uint)(vertexIndex + 0);

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
			ShaderProgram program = technique.Shader.Res ?? ShaderProgram.Minimal.Res;
			NativeShaderProgram nativeProgram = (program.Native ?? ShaderProgram.Minimal.Res.Native) as NativeShaderProgram;

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
			this.SetupBlendState(BlendMode.Reset, true);
			NativeShaderProgram.Bind(null);
			NativeTexture.ResetBinding(this.sharedSamplerBindings);
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
