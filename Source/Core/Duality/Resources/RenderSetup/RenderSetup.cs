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
				DefaultClearColor = true
			});
			defaultSetup.Steps.Add(new RenderStep
			{
				Id = "ScreenOverlay",
				MatrixMode = RenderMatrix.ScreenSpace,
				ClearFlags = ClearFlag.None,
				VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay
			});

			InitDefaultContent<RenderSetup>(new Dictionary<string,RenderSetup>
			{
				{ "Default", defaultSetup },
			});
		}


		private List<RenderStep>               steps                = new List<RenderStep>();
		private TargetResize                   autoResizeTargetMode = TargetResize.None;
		private List<ContentRef<RenderTarget>> autoResizeTargets    = new List<ContentRef<RenderTarget>>();

		[DontSerialize] private Dictionary<ContentRef<RenderTarget>,Point2> originalTargetSizes = new Dictionary<ContentRef<RenderTarget>,Point2>();


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
		/// [GET / SET] Specifies whether and how <see cref="RenderTarget"/> resources used by 
		/// this <see cref="RenderSetup"/> should be automatically resized to fit the game's window size.
		/// Which render targets are affected is defined by the <see cref="AutoResizeTargets"/> property.
		/// 
		/// Usually, this should be set to <see cref="TargetResize.None"/>, <see cref="TargetResize.Stretch"/>
		/// or <see cref="TargetResize.Fit"/>.
		/// </summary>
		public TargetResize AutoResizeTargetMode
		{
			get { return this.autoResizeTargetMode; }
			set { this.autoResizeTargetMode = value; }
		}
		/// <summary>
		/// [GET / SET] A list of <see cref="RenderTarget"/> resources that should be automatically resized to 
		/// fit the desired rendering output size.
		/// </summary>
		public List<ContentRef<RenderTarget>> AutoResizeTargets
		{
			get { return this.autoResizeTargets; }
			set { this.autoResizeTargets = value ?? new List<ContentRef<RenderTarget>>(); }
		}


		/// <summary>
		/// Applies auto-resizing rules to all <see cref="RenderTarget"/> resources that are in the resize list
		/// of this <see cref="RenderSetup"/>.
		/// </summary>
		/// <param name="outputSize"></param>
		public void ApplyOutputAutoResize(Point2 outputSize)
		{
			foreach (ContentRef<RenderTarget> targetRef in this.autoResizeTargets)
			{
				RenderTarget target = targetRef.Res;
				if (target == null) continue;

				// Determine the target's original size
				Point2 originalTargetSize;
				if (!this.originalTargetSizes.TryGetValue(targetRef, out originalTargetSize))
					originalTargetSize = target.Size;

				// Determine the target's desired size based on output size and resize mode.
				Point2 desiredTargetSize = (Point2)this.autoResizeTargetMode.Apply(originalTargetSize, outputSize);
				if (target.Size != desiredTargetSize)
				{
					// If there's no record of the target's original size yet, create one
					if (!this.originalTargetSizes.ContainsKey(targetRef))
						this.originalTargetSizes.Add(targetRef, target.Size);

					// Resize all textures that are bound to the render target
					foreach (ContentRef<Texture> texRef in target.Targets)
					{
						Texture tex = texRef.Res;
						if (tex == null) continue;

						tex.Size = desiredTargetSize;
						tex.ReloadData();
					}

					// Rebind the render target
					target.SetupTarget();
				}

				// If the render target has been reset to its original size, remove the record for resetting it
				if (target.Size == originalTargetSize)
				{
					this.originalTargetSizes.Remove(targetRef);
				}
			}
		}
		/// <summary>
		/// Renders the specified <see cref="Scene"/>.
		/// </summary>
		/// <param name="scene">The <see cref="Scene"/> that should be rendered.</param>
		/// <param name="viewportRect">The viewport to render to, in pixel coordinates.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		internal protected virtual void RenderScene(Scene scene, Rect viewportRect, Vector2 imageSize)
		{
			Camera[] activeCams = scene.FindComponents<Camera>()
				.Where(c => c.Active)
				.ToArray();

			// Maybe sort / process list first later on.
			foreach (Camera c in activeCams)
				c.Render(viewportRect, imageSize);
		}
	}
}
