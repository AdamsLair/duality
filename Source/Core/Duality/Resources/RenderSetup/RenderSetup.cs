using System;
using System.Linq;
using System.Collections.Generic;

using Duality.Editor;
using Duality.Properties;
using Duality.Backend;
using Duality.Drawing;
using Duality.Components;


namespace Duality.Resources
{
	[ExplicitResourceReference(typeof(RenderTarget), typeof(Texture), typeof(Material))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageRenderSetup)]
	public class RenderSetup : Resource
	{
		/// <summary>
		/// The default rendering setup with one world-space step and one screen-space overlay step.
		/// </summary>
		public static ContentRef<RenderSetup> Default { get; private set; }

		internal static void InitDefaultContent()
		{
			RenderSetup defaultSetup = new RenderSetup();
			defaultSetup.Steps.Add(new RenderStep
			{
				Id = "World",
				DefaultClearColor = true,
				DefaultProjection = true
			});
			defaultSetup.Steps.Add(new RenderStep
			{
				Id = "ScreenOverlay",
				Projection = ProjectionMode.Screen,
				ClearFlags = ClearFlag.None,
				VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay
			});

			DefaultContent.InitType<RenderSetup>(new Dictionary<string,RenderSetup>
			{
				{ "Default", defaultSetup },
			});
		}


		private List<RenderStep>              steps             = new List<RenderStep>();
		private List<RenderSetupTargetResize> autoResizeTargets = new List<RenderSetupTargetResize>();

		[DontSerialize] private Dictionary<ContentRef<RenderTarget>,Point2> originalTargetSizes = new Dictionary<ContentRef<RenderTarget>,Point2>();
		[DontSerialize] private RawList<ICmpRenderer> collectRendererBuffer = new RawList<ICmpRenderer>();
		[DontSerialize] private List<Predicate<ICmpRenderer>> rendererFilter = new List<Predicate<ICmpRenderer>>();
		[DontSerialize] private EventHandler<CollectDrawcallEventArgs> eventCollectDrawcalls = null;


		/// <summary>
		/// Fired when a <see cref="RenderStep"/> is collecting drawcalls.
		/// </summary>
		public event EventHandler<CollectDrawcallEventArgs> EventCollectDrawcalls
		{
			add { this.eventCollectDrawcalls += value; }
			remove { this.eventCollectDrawcalls -= value; }
		}


		/// <summary>
		/// [GET / SET] A set of rendering steps that describes the rendering process. Is never null nor empty.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public List<RenderStep> Steps
		{
			get { return this.steps; }
			set 
			{ 
				if (value != null)
					this.steps = value.Select(v => v ?? new RenderStep()).ToList();
				else
					this.steps = new List<RenderStep>();
			}
		}
		/// <summary>
		/// [GET / SET] A list of <see cref="RenderTarget"/> resources that should be automatically resized to 
		/// fit the desired rendering output size.
		/// </summary>
		public List<RenderSetupTargetResize> AutoResizeTargets
		{
			get { return this.autoResizeTargets; }
			set { this.autoResizeTargets = value ?? new List<RenderSetupTargetResize>(); }
		}

		
		/// <summary>
		/// Adds an additional rendering step to <see cref="Steps"/>.
		/// </summary>
		/// <param name="anchorId">Id of the existing rendering step to which the new step will be anchored.</param>
		/// <param name="anchorPos">Position of the new rendering step relative to the one it is anchored to.</param>
		/// <param name="step">The new rendering step that should be inserted into the rendering step sequence.</param>
		public void AddRenderStep(string anchorId, RenderStepPosition anchorPos, RenderStep step)
		{
			int anchorIndex = -1;
			switch (anchorPos)
			{
				case RenderStepPosition.Before:
					anchorIndex = this.steps.FindIndex(existingStep => existingStep.Id == anchorId);
					if (anchorIndex == -1)
						anchorIndex = 0;
					break;
				case RenderStepPosition.After:
					anchorIndex = this.steps.FindIndex(existingStep => existingStep.Id == anchorId);
					if (anchorIndex == -1)
						anchorIndex = this.steps.Count;
					else
						anchorIndex++;
					break;
				case RenderStepPosition.First:
					anchorIndex = 0;
					break;
				case RenderStepPosition.Last:
					anchorIndex = this.steps.Count;
					break;
			}
			if (anchorIndex != -1)
			{
				this.steps.Insert(anchorIndex, step);
			}
		}
		/// <summary>
		/// Adds an additional rendering step to <see cref="Steps"/>.
		/// </summary>
		/// <param name="anchorPos">Position of the new rendering step relative to the one it is anchored to.</param>
		/// <param name="step">The new rendering step that should be inserted into the rendering step sequence.</param>
		public void AddRenderStep(RenderStepPosition anchorPos, RenderStep step)
		{
			this.AddRenderStep(null, anchorPos, step);
		}
		
		/// <summary>
		/// Adds a temporary filter to remove certain renderers from the drawing queue entirely.
		/// The list of active renderer filters is not serialized.
		/// </summary>
		/// <param name="filter"></param>
		public void AddRendererFilter(Predicate<ICmpRenderer> filter)
		{
			if (this.rendererFilter.Contains(filter)) return;
			this.rendererFilter.Add(filter);
		}
		/// <summary>
		/// Removes a temporary filter that was previously added using <see cref="AddRendererFilter"/>.
		/// </summary>
		/// <param name="filter"></param>
		public void RemoveRendererFilter(Predicate<ICmpRenderer> filter)
		{
			this.rendererFilter.Remove(filter);
		}


		/// <summary>
		/// Renders the specified <see cref="Scene"/> completely, including the viewports of 
		/// all <see cref="Camera"/> objects contained within.
		/// </summary>
		/// <param name="scene">The <see cref="Scene"/> that should be rendered.</param>
		/// <param name="target">
		/// The <see cref="RenderTarget"/> which will be used for all rendering output. 
		/// "null" means rendering directly to the output buffer of the game window / screen.
		/// </param>
		/// <param name="viewportRect">The viewport to render to, in pixel coordinates.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		public void RenderScene(Scene scene, ContentRef<RenderTarget> target, Rect viewportRect, Vector2 imageSize)
		{
			try
			{
				this.OnRenderScene(scene, target, viewportRect, imageSize);
			}
			catch (Exception e)
			{
				Logs.Core.WriteError("There was an error while {0} was rendering {1}: {2}", this, scene, LogFormat.Exception(e));
			}
		}
		/// <summary>
		/// Renders a scene from the perspective of a single, pre-configured drawing device.
		/// </summary>
		public void RenderPointOfView(Scene scene, DrawDevice drawDevice, Rect viewportRect, Vector2 imageSize)
		{
			// Memorize projection matrix settings, so the drawing device can be properly reset later
			ProjectionMode oldDeviceProjection = drawDevice.Projection;
			Vector2 oldDeviceTargetSize = drawDevice.TargetSize;

			try
			{
				this.OnRenderPointOfView(scene, drawDevice, viewportRect, imageSize);
			}
			catch (Exception e)
			{
				Logs.Core.WriteError("There was an error while {0} was rendering a point of view in {1}: {2}", this, scene, LogFormat.Exception(e));
			}

			// Reset matrices for projection calculations to their previous state
			drawDevice.Projection = oldDeviceProjection;
			drawDevice.TargetSize = oldDeviceTargetSize;
		}


		/// <summary>
		/// Applies auto-resizing rules to all <see cref="RenderTarget"/> resources that are in the resize list
		/// of this <see cref="RenderSetup"/>.
		/// </summary>
		protected void ApplyOutputAutoResize(Point2 outputSize)
		{
			foreach (RenderSetupTargetResize autoResize in this.autoResizeTargets)
			{
				ContentRef<RenderTarget> targetRef = autoResize.Target;
				RenderTarget target = targetRef.Res;
				if (target == null) continue;

				// Determine the target's original size
				Point2 originalTargetSize;
				if (!this.originalTargetSizes.TryGetValue(targetRef, out originalTargetSize))
					originalTargetSize = target.Size;

				// Determine the target's desired size based on output size and resize mode.
				Point2 desiredTargetSize = (Point2)(autoResize.ResizeMode.Apply(originalTargetSize, outputSize) * autoResize.Scale);
				desiredTargetSize = Point2.Max(desiredTargetSize, new Point2(1, 1));
				if (target.Size != desiredTargetSize)
				{
					// If there's no record of the target's original size yet, create one
					if (!this.originalTargetSizes.ContainsKey(targetRef))
						this.originalTargetSizes.Add(targetRef, target.Size);

					this.ResizeRenderTarget(target, desiredTargetSize);
				}

				// If the render target has been reset to its original size, remove the record for resetting it
				if (target.Size == originalTargetSize)
				{
					this.originalTargetSizes.Remove(targetRef);
				}
			}
		}
		/// <summary>
		/// Resizes the specified <see cref="RenderTarget"/> to its new target size.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="targetSize"></param>
		protected void ResizeRenderTarget(ContentRef<RenderTarget> target, Point2 targetSize)
		{
			RenderTarget targetRes = target.Res;
			if (targetRes.Size == targetSize) return;

			// Resize all textures that are bound to the render target
			foreach (ContentRef<Texture> texRef in targetRes.Targets)
			{
				Texture tex = texRef.Res;
				if (tex == null) continue;

				tex.Size = targetSize;
				tex.ReloadData();
			}

			// Rebind the render target
			targetRes.SetupTarget();
		}
		
		/// <summary>
		/// Performs the specified <see cref="RenderStep"/>. This method will do some basic, localized configuration on
		/// the drawing device and then invoke <see cref="OnRenderSingleStep"/> for running the actual rendering operations.
		/// </summary>
		protected void RenderSingleStep(RenderStep step, Scene scene, DrawDevice drawDevice, Rect viewportRect, Vector2 imageSize)
		{
			// Memorize old draw device settings to reset them later
			VisibilityFlag oldDeviceMask = drawDevice.VisibilityMask;
			ColorRgba oldDeviceClearColor = drawDevice.ClearColor;
			ProjectionMode oldDeviceProjection = drawDevice.Projection;
			ContentRef<RenderTarget> oldDeviceTarget = drawDevice.Target;
			
			Rect localViewport;
			Vector2 localTargetSize;
			ContentRef<RenderTarget> renderTarget;

			// If this step is using a custom render target, override image and viewport sizes
			if (step.Output.IsAvailable)
			{
				renderTarget = step.Output;
				localTargetSize = step.Output.Res.Size;
				localViewport = new Rect(step.Output.Res.Size);
			}
			// Otherwise, use the provided parameter values
			else
			{
				renderTarget = oldDeviceTarget;
				localTargetSize = imageSize;
				localViewport = viewportRect;
			}

			// Regardless of rendering targets, adjust the local render size and viewport 
			// according to the rendering step target rect
			localViewport.Pos += localViewport.Size * step.TargetRect.Pos;
			localViewport.Size *= step.TargetRect.Size;
			localTargetSize *= step.TargetRect.Size;

			// Set up the draw device with rendering step settings
			drawDevice.Projection = step.DefaultProjection ? oldDeviceProjection : step.Projection;
			drawDevice.Target = renderTarget;
			drawDevice.TargetSize = localTargetSize;
			drawDevice.ViewportRect = localViewport;
			drawDevice.ClearFlags = step.ClearFlags;
			drawDevice.ClearColor = step.DefaultClearColor ? oldDeviceClearColor : step.ClearColor;
			drawDevice.ClearDepth = step.ClearDepth;
			
			// ScreenOverlay is a special flag that is set on a per-rendering-step basis
			// and that shouldn't be affected by overall device settings. Keep it separate.
			drawDevice.VisibilityMask = 
				(drawDevice.VisibilityMask & step.VisibilityMask & ~VisibilityFlag.ScreenOverlay) | 
				(step.VisibilityMask & VisibilityFlag.ScreenOverlay);
			
			try
			{
				this.OnRenderSingleStep(step, scene, drawDevice);
			}
			catch (Exception e)
			{
				Logs.Core.WriteError("There was an error while {0} was processing rendering step '{1}': {2}", this, step.Id, LogFormat.Exception(e));
			}

			// Restore old draw device state
			drawDevice.VisibilityMask = oldDeviceMask;
			drawDevice.ClearColor = oldDeviceClearColor;
			drawDevice.Projection = oldDeviceProjection;
			drawDevice.Target = oldDeviceTarget;
		}
		/// <summary>
		/// Uses the specified <see cref="DrawDevice"/> to collect renderer drawcalls in the specified <see cref="Scene"/>.
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="drawDevice"></param>
		protected void CollectRendererDrawcalls(Scene scene, DrawDevice drawDevice)
		{
			Profile.TimeCollectDrawcalls.BeginMeasure();
			try
			{
				// If no visibility groups are met, don't bother looking for renderers.
				// This is important to allow efficient drawcall injection with additional
				// "dummy" renderpasses. CamViewStates render their overlays by temporarily 
				// adding 3 - 4 of these passes. Iterating over all objects again would be 
				// devastating for performance and at the same time pointless.
				if ((drawDevice.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None) return;

				// Query renderers
				IRendererVisibilityStrategy visibilityStrategy = scene.VisibilityStrategy;
				if (visibilityStrategy == null) return;

				Profile.TimeQueryVisibleRenderers.BeginMeasure();

				if (this.collectRendererBuffer == null)
					this.collectRendererBuffer = new RawList<ICmpRenderer>();
				this.collectRendererBuffer.Clear();

				visibilityStrategy.QueryVisibleRenderers(drawDevice, this.collectRendererBuffer);
				if (this.rendererFilter.Count > 0)
				{
					this.collectRendererBuffer.RemoveAll(r =>
					{
						for (int i = 0; i < this.rendererFilter.Count; i++)
						{
							if (!this.rendererFilter[i](r)) return true;
						}
						return false;
					});
				}

				Profile.TimeQueryVisibleRenderers.EndMeasure();

				this.OnCollectRendererDrawcalls(drawDevice, this.collectRendererBuffer, visibilityStrategy.IsRendererQuerySorted);
			}
			catch (Exception e)
			{
				Logs.Core.WriteError("There was an error while {0} was collecting renderer drawcalls: {1}", this, LogFormat.Exception(e));
			}
			Profile.TimeCollectDrawcalls.EndMeasure();
		}
		/// <summary>
		/// Collects drawcalls that are submitted by external sources which are 
		/// subscribed to <see cref="EventCollectDrawcalls"/>.
		/// </summary>
		protected void CollectExternalDrawcalls(RenderStep step, DrawDevice drawDevice)
		{
			Profile.TimeCollectDrawcalls.BeginMeasure();
			try
			{
				if (this.eventCollectDrawcalls != null)
					this.eventCollectDrawcalls(this, new CollectDrawcallEventArgs(step.Id, drawDevice));
			}
			catch (Exception e)
			{
				Logs.Core.WriteError("There was an error while {0} was collecting external drawcalls: {1}", this, LogFormat.Exception(e));
			}
			Profile.TimeCollectDrawcalls.EndMeasure();
		}
		/// <summary>
		/// Collects all drawcalls from both external sources and renderers in the scene.
		/// </summary>
		/// <param name="step"></param>
		/// <param name="scene"></param>
		/// <param name="drawDevice"></param>
		protected void CollectDrawcalls(RenderStep step, Scene scene, DrawDevice drawDevice)
		{
			this.CollectRendererDrawcalls(scene, drawDevice);
			this.CollectExternalDrawcalls(step, drawDevice);
		}

		/// <summary>
		/// Called to render the specified <see cref="Scene"/>, including the viewports of 
		/// all <see cref="Camera"/> objects contained within.
		/// </summary>
		/// <param name="scene">The <see cref="Scene"/> that should be rendered.</param>
		/// <param name="target">
		/// The <see cref="RenderTarget"/> which will be used for all rendering output. 
		/// "null" means rendering directly to the output buffer of the game window / screen.
		/// </param>
		/// <param name="viewportRect">The viewport to render to, in pixel coordinates.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		protected virtual void OnRenderScene(Scene scene, ContentRef<RenderTarget> target, Rect viewportRect, Vector2 imageSize)
		{
			Camera[] activeSceneCameras = scene.FindComponents<Camera>()
				.Where(c => c.Active)
				.OrderByDescending(c => c.Priority)
				.ToArray();
			
			foreach (Camera camera in activeSceneCameras)
			{
				Vector2 cameraImageSize = imageSize;
				Rect cameraViewport = viewportRect;
				bool isOutputCamera = false;

				// Cameras with a custom render target will use its size to override image and viewport size
				if (camera.Target.IsAvailable)
				{
					cameraImageSize = camera.Target.Res.Size;
					cameraViewport = new Rect(camera.Target.Res.Size);
				}
				// Cameras without a custom render target will use the provided parameters
				else
				{
					camera.Target = target;
					isOutputCamera = true;
				}
				
				try
				{
					camera.Render(cameraViewport, cameraImageSize);
				}
				finally
				{
					if (isOutputCamera)
						camera.Target = null;
				}
			}
		}
		/// <summary>
		/// Called to render a scene from the perspective of a single, pre-configured drawing device.
		/// </summary>
		protected virtual void OnRenderPointOfView(Scene scene, DrawDevice drawDevice, Rect viewportRect, Vector2 imageSize)
		{
			// Resize all render targets to the viewport size we're dealing with
			this.ApplyOutputAutoResize((Point2)viewportRect.Size);

			// Execute all steps in the rendering setup, as well as those that were added in this camera
			foreach (RenderStep step in this.steps)
			{
				this.RenderSingleStep(step, scene, drawDevice, viewportRect, imageSize);
			}
		}
		/// <summary>
		/// Called to process the specified <see cref="RenderStep"/>.
		/// </summary>
		protected virtual void OnRenderSingleStep(RenderStep step, Scene scene, DrawDevice drawDevice)
		{
			drawDevice.PrepareForDrawcalls();
			if (step.Input == null)
			{
				this.CollectDrawcalls(step, scene, drawDevice);
			}
			else
			{
				drawDevice.AddFullscreenQuad(step.Input, step.InputResize);
			}
			drawDevice.Render();
		}
		/// <summary>
		/// Called in order to let the specified <see cref="DrawDevice"/> collect all drawcalls from
		/// a set of renderers that was previously determined to be potentially visible.
		/// </summary>
		/// <param name="drawDevice"></param>
		/// <param name="visibleRenderers"></param>
		/// <param name="renderersSortedByType"></param>
		protected virtual void OnCollectRendererDrawcalls(DrawDevice drawDevice, RawList<ICmpRenderer> visibleRenderers, bool renderersSortedByType)
		{
			Type lastRendererType = null;
			Type rendererType = null;
			TimeCounter activeProfiler = null;
			ICmpRenderer[] data = visibleRenderers.Data;
			for (int i = 0; i < data.Length; i++)
			{
				if (i >= visibleRenderers.Count) break;

				// Manage profilers per Component type
				if (renderersSortedByType)
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
				data[i].Draw(drawDevice);
			}
				
			if (activeProfiler != null)
				activeProfiler.EndMeasure();
		}
	}
}
