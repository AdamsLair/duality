using System;
using System.Collections.Generic;
using System.Linq;

using Duality.IO;
using Duality.Editor;
using Duality.Cloning;
using Duality.Drawing;
using Duality.Resources;
using Duality.Properties;

namespace Duality.Components
{
	/// <summary>
	/// A Camera is responsible for rendering the current <see cref="Duality.Resources.Scene"/>.
	/// </summary>
	[RequiredComponent(typeof(Transform))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageCamera)]
	public sealed class Camera : Component, ICmpInitializable
	{
		private float                    nearZ                 = 0.0f;
		private float                    farZ                  = 10000.0f;
		private float                    focusDist             = DrawDevice.DefaultFocusDist;
		private PerspectiveMode          perspective           = PerspectiveMode.Parallax;
		private VisibilityFlag           visibilityMask        = VisibilityFlag.All;
		private ColorRgba                clearColor            = ColorRgba.TransparentBlack;
		private ContentRef<RenderTarget> renderTarget          = null;
		private ContentRef<RenderSetup>  renderSetup           = null;
		private List<RenderStepAddition> additionalRenderSteps = new List<RenderStepAddition>();

		[DontSerialize] private DrawDevice                    drawDevice         = null;
		[DontSerialize] private List<RenderStep>              renderSteps        = new List<RenderStep>();
		[DontSerialize] private List<ICmpRenderer>            pickingMap         = null;
		[DontSerialize] private RenderTarget                  pickingRT          = null;
		[DontSerialize] private Texture                       pickingTex         = null;
		[DontSerialize] private byte[]                        pickingBuffer      = null;
		[DontSerialize] private List<Predicate<ICmpRenderer>> editorRenderFilter = new List<Predicate<ICmpRenderer>>();
		
		[DontSerialize] 
		private EventHandler<CollectDrawcallEventArgs> eventCollectDrawcalls = null;
		/// <summary>
		/// Fired when a <see cref="RenderStep"/> is collecting drawcalls.
		/// </summary>
		public event EventHandler<CollectDrawcallEventArgs> EventCollectDrawcalls
		{
			add { this.eventCollectDrawcalls += value; }
			remove { this.eventCollectDrawcalls -= value; }
		}

		/// <summary>
		/// [GET / SET] The lowest Z value that can be displayed by the device.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		[EditorHintIncrement(10.0f)]
		public float NearZ
		{
			get { return this.nearZ; }
			set { this.nearZ = value; }
		}
		/// <summary>
		/// [GET / SET] The highest Z value that can be displayed by the device.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		[EditorHintIncrement(10.0f)]
		public float FarZ
		{
			get { return this.farZ; }
			set { this.farZ = value; }
		}
		/// <summary>
		/// [GET / SET] Reference distance for calculating the view projection. When using <see cref="PerspectiveMode.Parallax"/>, 
		/// an object this far away from the Camera will always appear in its original size and without offset.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		[EditorHintIncrement(10.0f)]
		[EditorHintRange(0.0f,float.MaxValue)]
		public float FocusDist
		{
			get { return this.focusDist; }
			set { this.focusDist = MathF.Max(value, 0.01f); }
		}
		/// <summary>
		/// [GET / SET] Specified the perspective effect that is applied when rendering the world.
		/// </summary>
		public PerspectiveMode Perspective
		{
			get { return this.perspective; }
			set { this.perspective = value; }
		}
		/// <summary>
		/// [GET / SET] A bitmask flagging all visibility groups that are considered visible to this drawing device.
		/// </summary>
		public VisibilityFlag VisibilityMask
		{
			get { return this.visibilityMask; }
			set { this.visibilityMask = value; }
		}
		/// <summary>
		/// [GET / SET] The default background color of the rendered image.
		/// </summary>
		public ColorRgba ClearColor
		{
			get { return this.clearColor; }
			set { this.clearColor = value; }
		}
		/// <summary>
		/// [GET / SET] When set, the camera will render all output that would normally end up
		/// on screen to the specified <see cref="RenderTarget"/> instead.
		/// </summary>
		public ContentRef<RenderTarget> Target
		{
			get { return this.renderTarget; }
			set { this.renderTarget = value; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="RenderSetup"/> that should be used by this camera. Will
		/// fall back to the application-default <see cref="DualityAppData.RenderingSetup"/> when unavailable.
		/// </summary>
		public ContentRef<RenderSetup> RenderingSetup
		{
			get { return this.renderSetup; }
			set { this.renderSetup = value; }
		}
		/// <summary>
		/// [GET / SET] A list of all rendering steps that will be executed in addition to 
		/// the ones defined in the referenced or default <see cref="RenderingSetup"/>.
		/// </summary>
		public List<RenderStepAddition> AdditionalRenderSteps
		{
			get { return this.additionalRenderSteps; }
			set { this.additionalRenderSteps = value ?? new List<RenderStepAddition>(); }
		}


		/// <summary>
		/// Adds an additional rendering step inside the sequence of rendering steps that is 
		/// defined by the <see cref="RenderingSetup"/>.
		/// </summary>
		/// <param name="anchorId">Id of the existing rendering step to which the new step will be anchored.</param>
		/// <param name="anchorPos">Position of the new rendering step relative to the one it is anchored to.</param>
		/// <param name="step">The new rendering step that should be inserted into the rendering step sequence.</param>
		public void AddRenderStep(string anchorId, RenderStepPosition anchorPos, RenderStep step)
		{
			this.additionalRenderSteps.Add(new RenderStepAddition
			{
				AnchorStepId = anchorId,
				AnchorPosition = anchorPos,
				AddedRenderStep = step
			});
		}
		/// <summary>
		/// Adds an additional rendering step inside the sequence of rendering steps that is 
		/// defined by the <see cref="RenderingSetup"/>.
		/// </summary>
		/// <param name="anchorPos">Position of the new rendering step relative to the one it is anchored to.</param>
		/// <param name="step">The new rendering step that should be inserted into the rendering step sequence.</param>
		public void AddRenderStep(RenderStepPosition anchorPos, RenderStep step)
		{
			this.additionalRenderSteps.Add(new RenderStepAddition
			{
				AnchorPosition = anchorPos,
				AddedRenderStep = step
			});
		}
		/// <summary>
		/// Removes an additional rendering step inside the sequence of rendering steps that is 
		/// defined by the <see cref="RenderingSetup"/>.
		/// </summary>
		/// <param name="step"></param>
		public void RemoveRenderStep(RenderStep step)
		{
			this.additionalRenderSteps.RemoveAll(item => item.AddedRenderStep == step);
		}

		/// <summary>
		/// Renders the current <see cref="Duality.Resources.Scene"/>.
		/// </summary>
		/// <param name="viewportRect">The viewport area to which will be rendered.</param>
		public void Render(Rect viewportRect)
		{
			this.UpdateRenderSteps();
			this.UpdateDeviceConfig();

			string counterName = PathOp.Combine("Cameras", this.gameobj.Name);
			Profile.BeginMeasure(counterName);
			Profile.TimeRender.BeginMeasure();

			this.RenderAllSteps(viewportRect);
			this.drawDevice.VisibilityMask = this.visibilityMask;
			this.drawDevice.RenderMode = RenderMatrix.WorldSpace;
			this.drawDevice.UpdateMatrices(); // Reset matrices for projection calculations during update

			Profile.TimeRender.EndMeasure();
			Profile.EndMeasure(counterName);
		}
		/// <summary>
		/// Renders a picking map of the current <see cref="Duality.Resources.Scene"/>.
		/// This method needs to be called each frame a picking operation is to be performed.
		/// </summary>
		/// <param name="viewportSize">Size of the viewport area to which will be rendered.</param>
		/// <param name="renderOverlay">Whether or not to render screen overlay renderers onto the picking target.</param>
		public void RenderPickingMap(Point2 viewportSize, bool renderOverlay)
		{
			Profile.TimeVisualPicking.BeginMeasure();

			// Render picking map
			{
				this.UpdateRenderSteps();
				this.UpdateDeviceConfig();
				this.SetupPickingRT(viewportSize);

				if (this.pickingMap == null) this.pickingMap = new List<ICmpRenderer>();
				this.pickingMap.Clear();

				// Setup DrawDevice
				this.drawDevice.PickingIndex = 1;
				this.drawDevice.Target = this.pickingRT;
				this.drawDevice.ViewportRect = new Rect(this.pickingTex.ContentSize);

				// Render the world
				{
					this.drawDevice.VisibilityMask = this.visibilityMask & VisibilityFlag.AllGroups;
					this.drawDevice.RenderMode = RenderMatrix.WorldSpace;

					this.drawDevice.PrepareForDrawcalls();
					this.CollectDrawcalls();
					this.drawDevice.Render(ClearFlag.All, ColorRgba.Black, 1.0f);
				}

				// Render screen overlays
				if (renderOverlay)
				{
					this.drawDevice.VisibilityMask = this.visibilityMask;
					this.drawDevice.RenderMode = RenderMatrix.ScreenSpace;

					this.drawDevice.PrepareForDrawcalls();
					this.CollectDrawcalls();
					this.drawDevice.Render(ClearFlag.None, ColorRgba.Black, 1.0f);
				}

				this.drawDevice.PickingIndex = 0;
			}

			// Move data to local buffer
			int pxNum = this.pickingTex.ContentWidth * this.pickingTex.ContentHeight;
			int pxByteNum = pxNum * 4;

			if (this.pickingBuffer == null)
				this.pickingBuffer = new byte[pxByteNum];
			else if (pxByteNum > this.pickingBuffer.Length)
				Array.Resize(ref this.pickingBuffer, Math.Max(this.pickingBuffer.Length * 2, pxByteNum));

			this.pickingRT.GetPixelData(this.pickingBuffer);

			Profile.TimeVisualPicking.EndMeasure();
		}
		/// <summary>
		/// Picks the <see cref="Duality.ICmpRenderer"/> that owns the pixel at the specified position.
		/// The resulting information is only accurate if <see cref="RenderPickingMap"/> has been called this frame.
		/// </summary>
		/// <param name="x">x-Coordinate of the pixel to check.</param>
		/// <param name="y">y-Coordinate of the pixel to check.</param>
		/// <returns>The <see cref="Duality.ICmpRenderer"/> that owns the pixel.</returns>
		public ICmpRenderer PickRendererAt(int x, int y)
		{
			if (this.pickingBuffer == null) return null;
			if (x < 0 || x >= this.pickingTex.ContentWidth) return null;
			if (y < 0 || y >= this.pickingTex.ContentHeight) return null;

			x = MathF.Clamp(x, 0, this.pickingTex.ContentWidth - 1);
			y = MathF.Clamp(y, 0, this.pickingTex.ContentHeight - 1);

			int baseIndex = 4 * (x + y * this.pickingTex.ContentWidth);
			if (baseIndex + 4 >= this.pickingBuffer.Length) return null;

			int rendererId = 
				(this.pickingBuffer[baseIndex + 0] << 16) |
				(this.pickingBuffer[baseIndex + 1] << 8) |
				(this.pickingBuffer[baseIndex + 2] << 0);
			if (rendererId > this.pickingMap.Count)
			{
				Logs.Core.WriteWarning("Unexpected picking result: {0}", ColorRgba.FromIntArgb(rendererId));
				return null;
			}
			else if (rendererId != 0)
			{
				if ((this.pickingMap[rendererId - 1] as Component).Disposed)
					return null;
				else
					return this.pickingMap[rendererId - 1];
			}
			else
				return null;
		}
		/// <summary>
		/// Picks all <see cref="Duality.ICmpRenderer">ICmpRenderers</see> contained within the specified
		/// rectangular area.
		/// The resulting information is only accurate if <see cref="RenderPickingMap"/> has been called this frame.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect.</param>
		/// <param name="y">y-Coordinate of the Rect.</param>
		/// <param name="w">Width of the Rect.</param>
		/// <param name="h">Height of the Rect.</param>
		/// <returns>A set of all <see cref="Duality.ICmpRenderer">ICmpRenderers</see> that have been picked.</returns>
		public IEnumerable<ICmpRenderer> PickRenderersIn(int x, int y, int w, int h)
		{
			if (this.pickingBuffer == null)
				return Enumerable.Empty<ICmpRenderer>();
			if ((x + w) + (y + h) * this.pickingTex.ContentWidth >= this.pickingBuffer.Length)
				return Enumerable.Empty<ICmpRenderer>();

			Rect dstRect = new Rect(x, y, w, h);
			Rect availRect = new Rect(this.pickingTex.ContentSize);

			if (!dstRect.Intersects(availRect)) return Enumerable.Empty<ICmpRenderer>();
			dstRect = dstRect.Intersection(availRect);

			x = Math.Max((int)dstRect.X, 0);
			y = Math.Max((int)dstRect.Y, 0);
			w = Math.Min((int)dstRect.W, this.pickingTex.ContentWidth - x);
			h = Math.Min((int)dstRect.H, this.pickingTex.ContentHeight - y);

			HashSet<ICmpRenderer> result = new HashSet<ICmpRenderer>();
			int rendererIdLast = 0;
			for (int j = 0; j < h; ++j)
			{
				int offset = 4 * (x + (y + j) * this.pickingTex.ContentWidth);
				for (int i = 0; i < w; ++i)
				{
					int rendererId =
						(this.pickingBuffer[offset]		<< 16) |
						(this.pickingBuffer[offset + 1] << 8) |
						(this.pickingBuffer[offset + 2] << 0);

					if (rendererId != rendererIdLast)
					{
						if (rendererId - 1 > this.pickingMap.Count)
							Logs.Core.WriteWarning("Unexpected picking result: {0}", ColorRgba.FromIntArgb(rendererId));
						else if (rendererId != 0 && !(this.pickingMap[rendererId - 1] as Component).Disposed)
							result.Add(this.pickingMap[rendererId - 1]);
						rendererIdLast = rendererId;
					}
					offset += 4;
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the scale factor of objects that are located at the specified (world space) z-Coordinate.
		/// </summary>
		/// <param name="z"></param>
		/// <returns></returns>
		public float GetScaleAtZ(float z)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetScaleAtZ(z);
		}
		/// <summary>
		/// Transforms screen space coordinates to world space coordinates. The screen positions Z coordinate is
		/// interpreted as the target world Z coordinate.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public Vector3 GetSpaceCoord(Vector3 screenPos)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetSpaceCoord(screenPos);
		}
		/// <summary>
		/// Transforms screen space coordinates to world space coordinates.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public Vector3 GetSpaceCoord(Vector2 screenPos)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetSpaceCoord(screenPos);
		}
		/// <summary>
		/// Transforms world space coordinates to screen space coordinates.
		/// </summary>
		/// <param name="spacePos"></param>
		/// <returns></returns>
		public Vector3 GetScreenCoord(Vector3 spacePos)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetScreenCoord(spacePos);
		}
		/// <summary>
		/// Transforms world space coordinates to screen space coordinates.
		/// </summary>
		/// <param name="spacePos"></param>
		/// <returns></returns>
		public Vector3 GetScreenCoord(Vector2 spacePos)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetScreenCoord(spacePos);
		}
		/// <summary>
		/// Returns whether the specified world-space position is visible in the Cameras view space.
		/// </summary>
		/// <param name="c">The position to test.</param>
		/// <param name="boundRad">The visual bounding radius to assume for the specified position.</param>
		/// <returns>True, if the position or a portion of its bounding circle is visible, false if not.</returns>
		public bool IsCoordInView(Vector3 c, float boundRad = 1.0f)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.IsCoordInView(c, boundRad);
		}

		private void SetupDevice()
		{
			if (this.drawDevice != null && !this.drawDevice.Disposed) return;
			this.drawDevice = new DrawDevice();
		}
		private void ReleaseDevice()
		{
			if (this.drawDevice == null) return;
			this.drawDevice.Dispose();
			this.drawDevice = null;
		}
		private void UpdateDeviceConfig()
		{
			// Lazy setup, in case someone uses this Camera despite being inactive. (Editor)
			if (this.drawDevice == null) this.SetupDevice();

			this.drawDevice.RefCoord = this.gameobj.Transform.Pos;
			this.drawDevice.RefAngle = this.gameobj.Transform.Angle;
			this.drawDevice.NearZ = this.nearZ;
			this.drawDevice.FarZ = this.farZ;
			this.drawDevice.FocusDist = this.focusDist;
			this.drawDevice.Perspective = this.perspective;
		}

		private void UpdateRenderSteps()
		{
			// Decide which rendering setup to use
			RenderSetup setup = 
				this.renderSetup.Res ?? 
				DualityApp.AppData.RenderingSetup.Res ?? 
				RenderSetup.Default.Res;

			// Retrieve all rendering steps from the setup
			this.renderSteps.Clear();
			foreach (RenderStep step in setup.Steps)
				this.renderSteps.Add(step);

			// Insert additional rendering steps as defined locally
			foreach (RenderStepAddition addition in this.additionalRenderSteps)
			{
				if (addition.AddedRenderStep == null) continue;

				int anchorIndex = -1;
				switch (addition.AnchorPosition)
				{
					case RenderStepPosition.Before:
						anchorIndex = this.renderSteps.FindIndex(step => step.Id == addition.AnchorStepId);
						if (anchorIndex == -1)
							anchorIndex = 0;
						break;
					case RenderStepPosition.After:
						anchorIndex = this.renderSteps.FindIndex(step => step.Id == addition.AnchorStepId);
						if (anchorIndex == -1)
							anchorIndex = this.renderSteps.Count;
						else
							anchorIndex++;
						break;
					case RenderStepPosition.First:
						anchorIndex = 0;
						break;
					case RenderStepPosition.Last:
						anchorIndex = this.renderSteps.Count;
						break;
				}
				if (anchorIndex != -1)
				{
					this.renderSteps.Insert(anchorIndex, addition.AddedRenderStep);
				}
			}
		}
		private void RenderAllSteps(Rect viewportRect)
		{
			// Determine which rendering setup is active
			RenderSetup setup = 
				this.renderSetup.Res ?? 
				DualityApp.AppData.RenderingSetup.Res ?? 
				RenderSetup.Default.Res;
			setup.ApplyOutputAutoResize((Point2)viewportRect.Size);

			// Execute all steps in the rendering setup, as well as those that were added in this camera
			foreach (RenderStep step in this.renderSteps)
			{
				this.RenderSingleStep(viewportRect, step);
			}
		}
		private void RenderSingleStep(Rect viewportRect, RenderStep step)
		{
			ContentRef<RenderTarget> renderTarget = step.Output.IsExplicitNull ? this.renderTarget : step.Output;

			this.drawDevice.VisibilityMask = this.visibilityMask & step.VisibilityMask;
			this.drawDevice.RenderMode = step.MatrixMode;
			this.drawDevice.Target = renderTarget;
			this.drawDevice.ViewportRect = renderTarget.IsAvailable ? new Rect(renderTarget.Res.Size) : viewportRect;

			if (step.Input == null)
			{
				// Collect drawcalls from all the renderers the want to display
				this.drawDevice.PrepareForDrawcalls();
				try
				{
					// Collect renderer drawcalls
					this.CollectDrawcalls();

					// Collect additional drawcalls by external sources subscribed to the event handler
					if (this.eventCollectDrawcalls != null)
						this.eventCollectDrawcalls(this, new CollectDrawcallEventArgs(step.Id, this.drawDevice));
				}
				catch (Exception e)
				{
					Logs.Core.WriteError("There was an error while {0} was collecting drawcalls: {1}", this.ToString(), LogFormat.Exception(e));
				}

				// Submit the collected drawcalls and perform rendering operations
				this.drawDevice.Render(
					step.ClearFlags, 
					step.DefaultClearColor ? this.clearColor : step.ClearColor, 
					step.ClearDepth);
			}
			else
			{
				Profile.TimePostProcessing.BeginMeasure();
				this.drawDevice.PrepareForDrawcalls();

				Texture mainTex = step.Input.MainTexture.Res;
				Vector2 uvRatio = mainTex != null ? mainTex.UVRatio : Vector2.One;
				Vector2 inputSize = mainTex != null ? mainTex.ContentSize : Vector2.One;

				// Fit the input material rect to the output size according to rendering step config
				Vector2 targetSize = step.InputResize.Apply(inputSize, this.drawDevice.TargetSize);
				Rect targetRect = Rect.Align(
					Alignment.Center, 
					this.drawDevice.TargetSize.X * 0.5f, 
					this.drawDevice.TargetSize.Y * 0.5f, 
					targetSize.X, 
					targetSize.Y);

				IDrawDevice device = this.drawDevice;
				{
					VertexC1P3T2[] vertices = new VertexC1P3T2[4];

					vertices[0].Pos = new Vector3(targetRect.LeftX, targetRect.TopY, 0.0f);
					vertices[1].Pos = new Vector3(targetRect.RightX, targetRect.TopY, 0.0f);
					vertices[2].Pos = new Vector3(targetRect.RightX, targetRect.BottomY, 0.0f);
					vertices[3].Pos = new Vector3(targetRect.LeftX, targetRect.BottomY, 0.0f);

					vertices[0].TexCoord = new Vector2(0.0f, 0.0f);
					vertices[1].TexCoord = new Vector2(uvRatio.X, 0.0f);
					vertices[2].TexCoord = new Vector2(uvRatio.X, uvRatio.Y);
					vertices[3].TexCoord = new Vector2(0.0f, uvRatio.Y);

					vertices[0].Color = ColorRgba.White;
					vertices[1].Color = ColorRgba.White;
					vertices[2].Color = ColorRgba.White;
					vertices[3].Color = ColorRgba.White;

					device.AddVertices(step.Input, VertexMode.Quads, vertices);
				}

				this.drawDevice.Render(step.ClearFlags, step.ClearColor, step.ClearDepth);
				Profile.TimePostProcessing.EndMeasure();
			}
		}

		private void CollectDrawcalls()
		{
			// If no visibility groups are met, don't bother looking for renderers.
			// This is important to allow efficient drawcall injection with additional
			// "dummy" renderpasses. CamViewStates render their overlays by temporarily 
			// adding 3 - 4 of these passes. Iterating over all objects again would be 
			// devastating for performance and at the same time pointless.
			if ((this.drawDevice.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None) return;

			// Query renderers
			IRendererVisibilityStrategy visibilityStrategy = Scene.Current.VisibilityStrategy;
			RawList<ICmpRenderer> visibleRenderers;
			{
				if (visibilityStrategy == null) return;
				Profile.TimeQueryVisibleRenderers.BeginMeasure();

				visibleRenderers = new RawList<ICmpRenderer>();
				visibilityStrategy.QueryVisibleRenderers(this.drawDevice, visibleRenderers);
				if (this.editorRenderFilter.Count > 0)
				{
					visibleRenderers.RemoveAll(r =>
					{
						for (int i = 0; i < this.editorRenderFilter.Count; i++)
						{
							if (!this.editorRenderFilter[i](r)) return true;
						}
						return false;
					});
				}

				Profile.TimeQueryVisibleRenderers.EndMeasure();
			}

			// Collect drawcalls
			if (this.drawDevice.IsPicking)
			{
				this.pickingMap.AddRange(visibleRenderers);
				foreach (ICmpRenderer r in visibleRenderers)
				{
					r.Draw(this.drawDevice);
					this.drawDevice.PickingIndex++;
				}
			}
			else
			{
				bool profilePerType = visibilityStrategy.IsRendererQuerySorted;
				Profile.TimeCollectDrawcalls.BeginMeasure();

				Type lastRendererType = null;
				Type rendererType = null;
				TimeCounter activeProfiler = null;
				ICmpRenderer[] data = visibleRenderers.Data;
				for (int i = 0; i < data.Length; i++)
				{
					if (i >= visibleRenderers.Count) break;

					// Manage profilers per Component type
					if (profilePerType)
					{
						rendererType = data[i].GetType();
						if (rendererType != lastRendererType)
						{
							if (activeProfiler != null)
								activeProfiler.EndMeasure();
							activeProfiler = Profile.RequestCounter<TimeCounter>(Profile.TimeCollectDrawcalls.FullName + @"\" + rendererType.Name);
							activeProfiler.BeginMeasure();
							lastRendererType = rendererType;
						}
					}

					// Collect Drawcalls from this Component
					data[i].Draw(this.drawDevice);
				}
				
				if (activeProfiler != null)
					activeProfiler.EndMeasure();

				Profile.TimeCollectDrawcalls.EndMeasure();
			}
		}
		private void SetupPickingRT(Point2 size)
		{
			if (this.pickingTex == null || this.pickingTex.ContentSize != size)
			{
				if (this.pickingTex != null) this.pickingTex.Dispose();
				if (this.pickingRT != null) this.pickingRT.Dispose();
				this.pickingTex = new Texture(
					size.X, size.Y, TextureSizeMode.Default, 
					TextureMagFilter.Nearest, TextureMinFilter.Nearest);
				this.pickingRT = new RenderTarget(AAQuality.Off, this.pickingTex);
			}
		}

		internal void AddEditorRendererFilter(Predicate<ICmpRenderer> filter)
		{
			if (this.editorRenderFilter.Contains(filter)) return;
			this.editorRenderFilter.Add(filter);
		}
		internal void RemoveEditorRendererFilter(Predicate<ICmpRenderer> filter)
		{
			this.editorRenderFilter.Remove(filter);
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				this.SetupDevice();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				this.ReleaseDevice();
			}
		}
	}
}
