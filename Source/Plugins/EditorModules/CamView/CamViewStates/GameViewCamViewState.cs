using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

using Duality.Editor.Controls.ToolStrip;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Provides a full preview of the game within the editor. 
	/// This state renders the games actual audiovisual output and reroutes user input to the game.
	/// </summary>
	public class GameViewCamViewState : CamViewState
	{
		private List<ToolStripItem> toolbarItems = new List<ToolStripItem>();
		private ToolStripTextBoxAdv textBoxRenderWidth = null;
		private ToolStripTextBoxAdv textBoxRenderHeight = null;
		private Point2 targetRenderSize = Point2.Zero;
		private bool isNativeRenderSize = false;
		private bool isUpdatingUI = false;
		private RenderTarget outputTarget = null;
		private Texture outputTexture = null;
		private DrawDevice blitDevice = null;


		public override string StateName
		{
			get { return Properties.CamViewRes.CamViewState_GameView_Name; }
		}
		public override Rect RenderedViewport
		{
			get { return this.LocalGameWindowRect; }
		}
		public override Point2 RenderedImageSize
		{
			get { return this.TargetRenderSize; }
		}
		private Point2 TargetRenderSize
		{
			get { return this.targetRenderSize; }
			set
			{
				if (this.targetRenderSize != value)
				{
					Point2 nativeSize = new Point2(
						this.RenderableControl.ClientSize.Width, 
						this.RenderableControl.ClientSize.Height);

					this.isNativeRenderSize = (nativeSize == value);
					this.targetRenderSize = value;
					this.UpdateTargetRenderSizeUI();
					this.Invalidate();
				}
			}
		}
		private bool TargetSizeFitsClientArea
		{
			get
			{
				return 
					this.targetRenderSize.X <= this.RenderableControl.ClientSize.Width &&
					this.targetRenderSize.Y <= this.RenderableControl.ClientSize.Height;
			}
		}
		private bool UseOffscreenBuffer
		{
			get { return !this.TargetSizeFitsClientArea; }
		}
		private Rect LocalGameWindowRect
		{
			get
			{
				return Rect.Align(
					Alignment.Center,
					this.ClientSize.Width * 0.5f,
					this.ClientSize.Height * 0.5f,
					this.TargetRenderSize.X,
					this.TargetRenderSize.Y);
			}
		}


		public GameViewCamViewState()
		{
			this.CameraActionAllowed = false;
			this.EngineUserInput = true;
		}

		private void AddToolbarItems()
		{
			this.textBoxRenderWidth = new ToolStripTextBoxAdv("textBoxRenderWidth");
			this.textBoxRenderWidth.BackColor = Color.FromArgb(196, 196, 196);
			this.textBoxRenderWidth.AutoSize = false;
			this.textBoxRenderWidth.Width = 35;
			this.textBoxRenderWidth.MaxLength = 4;
			this.textBoxRenderWidth.AcceptsOnlyNumbers = true;
			this.textBoxRenderWidth.EditingFinished += (sender, e) => this.OnTargetRenderSizeUIEditingFinished();
			this.textBoxRenderWidth.ProceedRequested += (sender, e) => this.textBoxRenderHeight.Focus();

			this.textBoxRenderHeight = new ToolStripTextBoxAdv("textBoxRenderHeight");
			this.textBoxRenderHeight.BackColor = Color.FromArgb(196, 196, 196);
			this.textBoxRenderHeight.AutoSize = false;
			this.textBoxRenderHeight.Width = 35;
			this.textBoxRenderHeight.MaxLength = 4;
			this.textBoxRenderHeight.AcceptsOnlyNumbers = true;
			this.textBoxRenderHeight.EditingFinished += (sender, e) => this.OnTargetRenderSizeUIEditingFinished();
			this.textBoxRenderHeight.ProceedRequested += (sender, e) => this.textBoxRenderWidth.Focus();

			this.toolbarItems.Add(new ToolStripLabel("Window Size "));
			this.toolbarItems.Add(this.textBoxRenderWidth);
			this.toolbarItems.Add(new ToolStripLabel("x"));
			this.toolbarItems.Add(this.textBoxRenderHeight);

			this.View.ToolbarCamera.SuspendLayout();
			for (int i = this.toolbarItems.Count - 1; i >= 0; i--)
			{
				ToolStripItem item = this.toolbarItems[i];
				item.Alignment = ToolStripItemAlignment.Right;
				this.View.ToolbarCamera.Items.Add(item);
			}
			this.View.ToolbarCamera.ResumeLayout();

			this.UpdateTargetRenderSizeUI();
		}
		private void RemoveToolbarItems()
		{
			this.View.ToolbarCamera.SuspendLayout();
			foreach (ToolStripItem item in this.toolbarItems)
			{
				this.View.ToolbarCamera.Items.Remove(item);
			}
			this.View.ToolbarCamera.ResumeLayout();

			this.toolbarItems.Clear();
			this.textBoxRenderWidth = null;
			this.textBoxRenderHeight = null;
		}

		private void ResetTargetRenderSize()
		{
			this.TargetRenderSize = new Point2(
				this.RenderableControl.ClientSize.Width,
				this.RenderableControl.ClientSize.Height);
		}
		private void UpdateTargetRenderSizeUI()
		{
			this.isUpdatingUI = true;

			Color normalColor = Color.FromArgb(196, 196, 196);
			Color customSizeColor = Color.FromArgb(196, 224, 255);
			Color overSizedColor = Color.FromArgb(255, 196, 196);

			Color backColor;
			if (this.isNativeRenderSize)
				backColor = normalColor;
			else if (this.TargetSizeFitsClientArea)
				backColor = customSizeColor;
			else
				backColor = overSizedColor;

			this.textBoxRenderWidth.Text = this.targetRenderSize.X.ToString();
			this.textBoxRenderHeight.Text = this.targetRenderSize.Y.ToString();
			this.textBoxRenderWidth.BackColor = backColor;
			this.textBoxRenderHeight.BackColor = backColor;

			this.isUpdatingUI = false;
		}
		private void ParseAndValidateTargetRenderSize()
		{
			int width;
			int height;
			if (!int.TryParse(this.textBoxRenderWidth.Text, out width))
				width = this.TargetRenderSize.X;
			if (!int.TryParse(this.textBoxRenderHeight.Text, out height))
				height = this.TargetRenderSize.Y;

			width = MathF.Clamp(width, 1, 7680);
			height = MathF.Clamp(height, 1, 4320);

			this.TargetRenderSize = new Point2(width, height);
		}

		private void CleanupRenderTarget()
		{
			if (this.outputTarget != null)
			{
				this.outputTarget.Dispose();
				this.outputTarget = null;
			}
			if (this.outputTexture != null)
			{
				this.outputTexture.Dispose();
				this.outputTexture = null;
			}
		}
		private void SetupOutputRenderTarget()
		{
			if (this.outputTarget == null)
			{
				this.outputTexture = new Texture(
					1, 
					1, 
					TextureSizeMode.NonPowerOfTwo, 
					TextureMagFilter.Nearest, 
					TextureMinFilter.Linear);
				this.outputTarget = new RenderTarget();
				this.outputTarget.DepthBuffer = true;
				this.outputTarget.Targets = new ContentRef<Texture>[]
				{
					this.outputTexture
				};
			}

			Point2 outputSize = new Point2(this.TargetRenderSize.X, this.TargetRenderSize.Y);
			if (this.outputTarget.Size != outputSize)
			{
				this.outputTexture.Size = outputSize;
				this.outputTexture.ReloadData();

				this.outputTarget.Multisampling = DualityApp.UserData.AntialiasingQuality;
				this.outputTarget.SetupTarget();
			}
		}
		private void SetupBlitDevice()
		{
			if (this.blitDevice == null)
			{
				this.blitDevice = new DrawDevice();
				this.blitDevice.ClearFlags = ClearFlag.Depth;
				this.blitDevice.Perspective = PerspectiveMode.Flat;
				this.blitDevice.RenderMode = RenderMatrix.ScreenSpace;
			}
		}

		protected internal override void OnEnterState()
		{
			base.OnEnterState();

			// Disable all regular editing functionality
			this.View.SetEditingToolsAvailable(false);
			this.CameraObj.Active = false;

			this.AddToolbarItems();
			this.ResetTargetRenderSize();
		}
		protected internal override void OnLeaveState()
		{
			base.OnLeaveState();

			this.RemoveToolbarItems();

			// Enable regular editing functionality again
			this.View.SetEditingToolsAvailable(true);
			this.CameraObj.Active = true;
		}
		protected override void OnRenderState()
		{
			// We're not calling the base implementation, because the default is to
			// render the scene from the view of an editing camera. The Game View, however, 
			// is in the special position to render the actual game and completely ignore 
			// any editing camera.
			//
			// base.OnRenderState();

			Point2 clientSize = new Point2(this.RenderableControl.ClientSize.Width, this.RenderableControl.ClientSize.Height);
			Point2 targetSize = this.TargetRenderSize;
			Rect windowRect = this.LocalGameWindowRect;
			
			Vector2 imageSize;
			Rect viewportRect;
			DualityApp.CalculateGameViewport(targetSize, out viewportRect, out imageSize);

			// Render the game view background using a background color matching editor UI,
			// so users can discern between an area that isn't rendered to and a rendered
			// area of the game that happens to be black or outside the game viewport.
			DrawDevice.RenderVoid(new Rect(clientSize), new ColorRgba(64, 64, 64));

			bool isRenderableScene = Scene.Current.FindComponents<Camera>().Any();
			if (!isRenderableScene)
			{
				// If there is nothing to render, fill the window area with emptiness
				DrawDevice.RenderVoid(windowRect);
			}
			else if (this.UseOffscreenBuffer)
			{
				// Render the scene to an offscreen buffer of matching size first
				this.SetupOutputRenderTarget();
				DualityApp.Render(this.outputTarget, viewportRect, imageSize);

				// Blit the offscreen buffer to the window area
				this.SetupBlitDevice();
				this.blitDevice.TargetSize = clientSize;
				this.blitDevice.ViewportRect = new Rect(clientSize);

				BatchInfo blitMaterial = new BatchInfo(
					DrawTechnique.Solid, 
					ColorRgba.White, 
					this.outputTexture);
				bool isSmallerThanWindow = 
					this.outputTarget.Size.X < clientSize.X && 
					this.outputTarget.Size.Y < clientSize.Y;
				TargetResize blitResize = isSmallerThanWindow ? 
					TargetResize.None : 
					TargetResize.Fit;

				this.blitDevice.PrepareForDrawcalls();
				this.blitDevice.AddFullscreenQuad(blitMaterial, blitResize);
				this.blitDevice.Render();
			}
			else
			{
				Rect windowViewportRect = new Rect(
					windowRect.X + viewportRect.X, 
					windowRect.Y + viewportRect.Y, 
					viewportRect.W, 
					viewportRect.H);

				// Render the scene centered into the designated viewport area
				this.CleanupRenderTarget();
				DrawDevice.RenderVoid(windowRect);
				DualityApp.Render(null, windowViewportRect, imageSize);
			}
		}
		protected override void OnResize()
		{
			base.OnResize();

			if (this.isNativeRenderSize)
				this.ResetTargetRenderSize();
		}

		private void OnTargetRenderSizeUIEditingFinished()
		{
			if (this.isUpdatingUI) return;
			this.ParseAndValidateTargetRenderSize();
		}
	}
}
