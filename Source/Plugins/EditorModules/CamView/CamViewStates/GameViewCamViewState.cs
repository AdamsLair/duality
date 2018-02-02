using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using AdamsLair.WinForms.ItemModels;
using AdamsLair.WinForms.ItemViews;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.CamView.Properties;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Provides a full preview of the game within the editor. 
	/// This state renders the games actual audiovisual output and reroutes user input to the game.
	/// </summary>
	public class GameViewCamViewState : CamViewState
	{
		/// <summary>
		/// Describes rendering size preset roles that may have special behaviour.
		/// </summary>
		private enum SpecialRenderSize
		{
			/// <summary>
			/// A fixed size is used, regardless of game settings or window sizes.
			/// </summary>
			Fixed,
			/// <summary>
			/// Matches the <see cref="CamView"/> client size.
			/// </summary>
			CamView,
			/// <summary>
			/// Matches the target size of the game, based on forced rendering size
			/// settings and default screen resolution.
			/// </summary>
			GameTarget
		}


		private List<ToolStripItem>     toolbarItems        = new List<ToolStripItem>();
		private ToolStripTextBoxAdv     textBoxRenderWidth  = null;
		private ToolStripTextBoxAdv     textBoxRenderHeight = null;
		private ToolStripDropDownButton dropdownResolution  = null;
		private MenuModel               resolutionMenuModel = new MenuModel();
		private MenuStripMenuView       resolutionMenuView  = null;

		private Point2            targetRenderSize        = Point2.Zero;
		private SpecialRenderSize targetRenderSizeMode    = SpecialRenderSize.CamView;
		private List<Point2>      recentTargetRenderSizes = new List<Point2>();
		private bool              isUpdatingUI            = false;
		private RenderTarget      outputTarget            = null;
		private Texture           outputTexture           = null;
		private DrawDevice        blitDevice              = null;


		/// <inheritdoc />
		public override string StateName
		{
			get { return Properties.CamViewRes.CamViewState_GameView_Name; }
		}
		/// <inheritdoc />
		public override Rect RenderedViewport
		{
			get { return this.LocalGameWindowRect; }
		}
		/// <inheritdoc />
		public override Point2 RenderedImageSize
		{
			get { return this.TargetRenderSize; }
		}
		/// <summary>
		/// [GET] The antialiasing quality that is preferred by the game.
		/// </summary>
		private AAQuality GameAntialiasingQuality
		{
			get
			{
				return DualityApp.AppData.MultisampleBackBuffer ?
					DualityApp.UserData.AntialiasingQuality : 
					AAQuality.Off;
			}
		}
		/// <summary>
		/// [GET] The target rendering size that is preferred by the game.
		/// Depends on default window size and forced resolution settings.
		/// </summary>
		private Point2 GameTargetSize
		{
			get
			{
				bool isUsingForcedSize = 
					DualityApp.AppData.ForcedRenderResizeMode != TargetResize.None &&
					DualityApp.AppData.ForcedRenderSize.X != 0 && 
					DualityApp.AppData.ForcedRenderSize.Y != 0;
				return isUsingForcedSize ? 
					DualityApp.AppData.ForcedRenderSize : 
					DualityApp.UserData.WindowSize;
			}
		}
		/// <summary>
		/// [GET] The target rendering size that is preferred by the <see cref="CamView"/>
		/// client area.
		/// </summary>
		private Point2 CamViewTargetSize
		{
			get
			{
				return new Point2(
					this.RenderableControl.ClientSize.Width,
					this.RenderableControl.ClientSize.Height);
			}
		}
		/// <summary>
		/// [GET / SET] The rendering size that will be used for displaying the
		/// game in this <see cref="CamViewState"/>.
		/// </summary>
		private Point2 TargetRenderSize
		{
			get { return this.targetRenderSize; }
			set
			{
				if (this.targetRenderSize != value)
				{
					this.targetRenderSize = value;
					this.UpdateTargetRenderSizeUI();
					this.Invalidate();
				}
			}
		}
		/// <summary>
		/// [GET / SET] The special rendering size role that is currently active
		/// for dynamically choosing rendering size in this <see cref="CamViewState"/>.
		/// </summary>
		private SpecialRenderSize TargetRenderSizeMode
		{
			get { return this.targetRenderSizeMode; }
			set
			{
				this.targetRenderSizeMode = value;
				this.ApplyTargetRenderSizeMode();
			}
		}
		/// <summary>
		/// [GET] Whether the currently used <see cref="TargetRenderSize"/> fits
		/// completely inside the available client area without having to downscale.
		/// </summary>
		private bool TargetSizeFitsClientArea
		{
			get
			{
				return 
					this.targetRenderSize.X <= this.RenderableControl.ClientSize.Width &&
					this.targetRenderSize.Y <= this.RenderableControl.ClientSize.Height;
			}
		}
		/// <summary>
		/// [GET] Whether an offscreen buffer should be used for rendering the game.
		/// </summary>
		private bool UseOffscreenBuffer
		{
			get
			{
				return 
					!this.TargetSizeFitsClientArea || 
					this.RenderableSite.AntialiasingQuality != this.GameAntialiasingQuality;
			}
		}
		/// <summary>
		/// [GET] The rect inside the local <see cref="CamView"/> client area that
		/// will be occupied by the game rendering. Pixels outside this rect will
		/// not be rendered to by the game.
		/// </summary>
		private Rect LocalGameWindowRect
		{
			get
			{
				TargetResize resizeMode = this.TargetSizeFitsClientArea ? 
					TargetResize.None : 
					TargetResize.Fit;
				
				Vector2 clientSize = new Vector2(this.ClientSize.Width, this.ClientSize.Height);
				Vector2 localWindowSize = resizeMode.Apply(this.TargetRenderSize, clientSize);

				return Rect.Align(
					Alignment.Center,
					clientSize.X * 0.5f,
					clientSize.Y * 0.5f,
					localWindowSize.X,
					localWindowSize.Y);
			}
		}


		public GameViewCamViewState()
		{
			this.CameraActionAllowed = false;
			this.EngineUserInput = true;
		}

		/// <summary>
		/// Creates the <see cref="CamView"/> main toolbar items that are only available while
		/// the <see cref="GameViewCamViewState"/> is active.
		/// </summary>
		private void AddToolbarItems()
		{
			this.textBoxRenderWidth = new ToolStripTextBoxAdv("textBoxRenderWidth");
			this.textBoxRenderWidth.BackColor = Color.FromArgb(196, 196, 196);
			this.textBoxRenderWidth.AutoSize = false;
			this.textBoxRenderWidth.Width = 35;
			this.textBoxRenderWidth.MaxLength = 4;
			this.textBoxRenderWidth.AcceptsOnlyNumbers = true;
			this.textBoxRenderWidth.EditingFinished += this.textBoxRenderWidth_EditingFinished;
			this.textBoxRenderWidth.ProceedRequested += this.textBoxRenderWidth_ProceedRequested;

			this.textBoxRenderHeight = new ToolStripTextBoxAdv("textBoxRenderHeight");
			this.textBoxRenderHeight.BackColor = Color.FromArgb(196, 196, 196);
			this.textBoxRenderHeight.AutoSize = false;
			this.textBoxRenderHeight.Width = 35;
			this.textBoxRenderHeight.MaxLength = 4;
			this.textBoxRenderHeight.AcceptsOnlyNumbers = true;
			this.textBoxRenderHeight.EditingFinished += this.textBoxRenderHeight_EditingFinished;
			this.textBoxRenderHeight.ProceedRequested += this.textBoxRenderHeight_ProceedRequested;

			this.dropdownResolution = new ToolStripDropDownButton(CamViewResCache.IconMonitor);
			this.dropdownResolution.DropDownOpening += this.dropdownResolution_DropDownOpening;

			this.toolbarItems.Add(new ToolStripLabel("Window Size "));
			this.toolbarItems.Add(this.textBoxRenderWidth);
			this.toolbarItems.Add(new ToolStripLabel("x"));
			this.toolbarItems.Add(this.textBoxRenderHeight);
			this.toolbarItems.Add(this.dropdownResolution);
			
			this.resolutionMenuView = new MenuStripMenuView(this.dropdownResolution.DropDownItems);
			this.resolutionMenuView.Model = this.resolutionMenuModel;

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
		/// <summary>
		/// Disposes any <see cref="CamView"/> main toolbar items that have been created
		/// during <see cref="AddToolbarItems"/>.
		/// </summary>
		private void RemoveToolbarItems()
		{
			this.View.ToolbarCamera.SuspendLayout();
			foreach (ToolStripItem item in this.toolbarItems)
			{
				this.View.ToolbarCamera.Items.Remove(item);
			}
			this.View.ToolbarCamera.ResumeLayout();

			this.textBoxRenderWidth.EditingFinished -= this.textBoxRenderWidth_EditingFinished;
			this.textBoxRenderWidth.ProceedRequested -= this.textBoxRenderWidth_ProceedRequested;
			this.textBoxRenderHeight.EditingFinished -= this.textBoxRenderHeight_EditingFinished;
			this.textBoxRenderHeight.ProceedRequested -= this.textBoxRenderHeight_ProceedRequested;
			this.dropdownResolution.DropDownOpening -= this.dropdownResolution_DropDownOpening;
			
			this.resolutionMenuView.Model = null;
			this.resolutionMenuView = null;
			this.resolutionMenuModel.ClearItems();

			this.toolbarItems.Clear();
			this.textBoxRenderWidth = null;
			this.textBoxRenderHeight = null;
		}
		/// <summary>
		/// Initializes dropdown menu items for the rendering size preset toolbar button.
		/// Will clean up or update previous contents when called multiple times.
		/// </summary>
		private void InitResolutionDropDownItems()
		{
			// Remove old items
			this.resolutionMenuModel.ClearItems();

			// Add dynamic presets
			MenuModelItem gameViewItem = this.resolutionMenuModel.RequestItem("GameView Size");
			gameViewItem.ActionHandler = this.dropdownResolution_GameViewSizeClicked;
			gameViewItem.SortValue = MenuModelItem.SortValue_Top;
			gameViewItem.Checked = (this.targetRenderSizeMode == SpecialRenderSize.CamView);
			MenuModelItem targetSizeItem = this.resolutionMenuModel.RequestItem("Target Size");
			targetSizeItem.ActionHandler = this.dropdownResolution_TargetSizeClicked;
			targetSizeItem.SortValue = MenuModelItem.SortValue_Top;
			targetSizeItem.Checked = (this.targetRenderSizeMode == SpecialRenderSize.GameTarget);
			this.resolutionMenuModel.AddItem(new MenuModelItem
			{
				Name      = "TopSeparator",
				TypeHint  = MenuItemTypeHint.Separator,
				SortValue = MenuModelItem.SortValue_Top + 1
			});

			// Add fixed presets
			Point2[] fixedPresets = new Point2[]
			{
				new Point2(1920, 1080),
				new Point2(1280, 1024),
				new Point2(800, 600),
				new Point2(320, 300)
			};
			for (int i = 0; i < fixedPresets.Length; i++)
			{
				Point2 size = fixedPresets[i];
				string itemName = string.Format("{0} x {1}", size.X, size.Y);
				MenuModelItem item = this.resolutionMenuModel.RequestItem(itemName);
				item.Tag = size;
				item.ActionHandler = this.dropdownResolution_FixedSizeClicked;
				item.SortValue = MenuModelItem.SortValue_UnderTop + i;
				item.Checked = (this.TargetRenderSize == size);
			}
			this.resolutionMenuModel.AddItem(new MenuModelItem
			{
				Name      = "UnderTopSeparator",
				TypeHint  = MenuItemTypeHint.Separator,
				SortValue = MenuModelItem.SortValue_UnderTop + fixedPresets.Length + 1
			});

			// Add recently used custom fixed resolutions
			int addedItemCount = 0;
			for (int i = 0; i < this.recentTargetRenderSizes.Count; i++)
			{
				Point2 size = this.recentTargetRenderSizes[i];

				// Skip those that are already part of the fixed presets
				if (Array.IndexOf(fixedPresets, size) != -1)
					continue;

				string itemName = string.Format("{0} x {1}", size.X, size.Y);
				MenuModelItem item = this.resolutionMenuModel.RequestItem(itemName);
				item.Tag = size;
				item.ActionHandler = this.dropdownResolution_FixedSizeClicked;
				item.SortValue = MenuModelItem.SortValue_Main + i;
				item.Checked = (this.TargetRenderSize == size);

				// Add a maximum of three custom resolutions
				addedItemCount++;
				if (addedItemCount >= 3) break;
			}
		}

		/// <summary>
		/// Applies the dynamic rendering size that is defined by the currently
		/// used <see cref="TargetRenderSizeMode"/>. Does nothing if that mode is
		/// <see cref="SpecialRenderSize.Fixed"/>.
		/// </summary>
		private void ApplyTargetRenderSizeMode()
		{
			if (this.targetRenderSizeMode == SpecialRenderSize.CamView)
				this.TargetRenderSize = this.CamViewTargetSize;
			else if (this.targetRenderSizeMode == SpecialRenderSize.GameTarget)
				this.TargetRenderSize = this.GameTargetSize;
		}
		/// <summary>
		/// Updates the render size UI textboxes to display the current internal
		/// rendering size value.
		/// </summary>
		private void UpdateTargetRenderSizeUI()
		{
			this.isUpdatingUI = true;

			Color normalColor = Color.FromArgb(196, 196, 196);
			Color customSizeColor = Color.FromArgb(196, 224, 255);
			Color overSizedColor = Color.FromArgb(255, 196, 196);

			Color backColor;
			if (this.targetRenderSizeMode == SpecialRenderSize.CamView)
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
		/// <summary>
		/// Parses the render size UI textbox contents, and applies the validated
		/// and clamped values to the internal rendering size value.
		/// </summary>
		private void ParseAndValidateTargetRenderSize()
		{
			int width;
			int height;
			if (!int.TryParse(this.textBoxRenderWidth.Text, out width))
				width = this.TargetRenderSize.X;
			if (!int.TryParse(this.textBoxRenderHeight.Text, out height))
				height = this.TargetRenderSize.Y;

			width = MathF.Clamp(width, 1, 3840);
			height = MathF.Clamp(height, 1, 2160);

			this.TargetRenderSizeMode = SpecialRenderSize.Fixed;
			this.TargetRenderSize = new Point2(width, height);
		}
		/// <summary>
		/// Flags the currently applied rendering size as recently used. Shouldn't
		/// be called on every change, because users generally do multiple changes
		/// (modify X and Y, potentially mistype, etc.) until the final value is entered.
		/// </summary>
		private void SampleRecentTargetRenderSize()
		{
			if (this.targetRenderSizeMode != SpecialRenderSize.Fixed)
				return;

			this.recentTargetRenderSizes.Remove(this.targetRenderSize);
			this.recentTargetRenderSizes.Insert(0, this.targetRenderSize);

			if (this.recentTargetRenderSizes.Count > 10)
				this.recentTargetRenderSizes.RemoveRange(10, this.recentTargetRenderSizes.Count - 10);
		}

		/// <summary>
		/// Disposes any potentially allocated internal offscreen rendering
		/// target for rendering the game.
		/// </summary>
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
		/// <summary>
		/// Sets up an offscreen rendering target for rendering the game. If
		/// one was already available, the existing target is updated to match
		/// the current rendering size and settings.
		/// </summary>
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
			if (this.outputTarget.Size != outputSize || this.outputTarget.Multisampling != this.GameAntialiasingQuality)
			{
				this.outputTexture.Size = outputSize;
				this.outputTexture.ReloadData();

				this.outputTarget.Multisampling = this.GameAntialiasingQuality;
				this.outputTarget.SetupTarget();
			}
		}
		/// <summary>
		/// Sets up a <see cref="DrawDevice"/> to be used for blitting an internal
		/// offscreen rendering target to the actual window surface.
		/// </summary>
		private void SetupBlitDevice()
		{
			if (this.blitDevice == null)
			{
				this.blitDevice = new DrawDevice();
				this.blitDevice.ClearFlags = ClearFlag.Depth;
				this.blitDevice.Projection = ProjectionMode.Screen;
			}
		}
		
		/// <inheritdoc />
		protected internal override void SaveUserData(XElement node)
		{
			base.SaveUserData(node);
			if (this.targetRenderSizeMode == SpecialRenderSize.Fixed)
			{
				XElement renderSizeElement = new XElement("RenderSize");
				renderSizeElement.SetElementValue("X", this.targetRenderSize.X);
				renderSizeElement.SetElementValue("Y", this.targetRenderSize.Y);
				node.Add(renderSizeElement);
			}
			else
			{
				node.Add(new XElement(
					"SpecialRenderSize", 
					this.targetRenderSizeMode));
			}

			XElement recentSizesElement = new XElement("RecentRenderSizes");
			foreach (Point2 recentSize in this.recentTargetRenderSizes)
			{
				recentSizesElement.Add(new XElement("RenderSize", 
					new XElement("X", recentSize.X),
					new XElement("Y", recentSize.Y)));
			}
			node.Add(recentSizesElement);
		}
		/// <inheritdoc />
		protected internal override void LoadUserData(XElement node)
		{
			base.LoadUserData(node);

			XElement renderSizeElement = node.Element("RenderSize");
			SpecialRenderSize specialSize = SpecialRenderSize.CamView;
			if (node.TryGetElementValue("SpecialRenderSize", ref specialSize) && specialSize != SpecialRenderSize.Fixed)
			{
				this.targetRenderSizeMode = specialSize;
			}
			else if (renderSizeElement != null)
			{
				this.targetRenderSizeMode = SpecialRenderSize.Fixed;
				this.targetRenderSize = new Point2(
					renderSizeElement.GetElementValue("X", this.targetRenderSize.X),
					renderSizeElement.GetElementValue("Y", this.targetRenderSize.Y));
			}

			XElement recentSizesElement = node.Element("RecentRenderSizes");
			if (recentSizesElement != null)
			{
				this.recentTargetRenderSizes.Clear();
				foreach (XElement item in recentSizesElement.Elements("RenderSize"))
				{
					Point2 recentSize = new Point2(
						item.GetElementValue("X", 0),
						item.GetElementValue("Y", 0));
					if (recentSize.X == 0) continue;
					if (recentSize.Y == 0) continue;

					this.recentTargetRenderSizes.Insert(0, recentSize);
				}
			}
		}

		/// <inheritdoc />
		protected internal override void OnEnterState()
		{
			base.OnEnterState();

			// Disable all regular editing functionality
			this.View.SetEditingToolsAvailable(false);
			this.CameraObj.Active = false;

			this.AddToolbarItems();
			this.ApplyTargetRenderSizeMode();
		}
		/// <inheritdoc />
		protected internal override void OnLeaveState()
		{
			base.OnLeaveState();

			this.RemoveToolbarItems();

			// Enable regular editing functionality again
			this.View.SetEditingToolsAvailable(true);
			this.CameraObj.Active = true;
		}
		/// <inheritdoc />
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

			if (this.UseOffscreenBuffer)
			{
				// Render the scene to an offscreen buffer of matching size first
				this.SetupOutputRenderTarget();
				DualityApp.Render(this.outputTarget, viewportRect, imageSize);

				// Blit the offscreen buffer to the window area
				this.SetupBlitDevice();
				this.blitDevice.TargetSize = clientSize;
				this.blitDevice.ViewportRect = new Rect(clientSize);

				BatchInfo blitMaterial = this.blitDevice.RentMaterial();
				blitMaterial.Technique = DrawTechnique.Solid;
				blitMaterial.MainTexture = this.outputTexture;

				TargetResize blitResize = this.TargetSizeFitsClientArea ? 
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
		/// <inheritdoc />
		protected override void OnResize()
		{
			base.OnResize();

			// Update target size when fitting to cam view size
			if (this.targetRenderSizeMode == SpecialRenderSize.CamView)
				this.TargetRenderSize = this.CamViewTargetSize;
			// Otherwise update the UI, because whether or not we're rendering at superresolution may have changed.
			else
				this.UpdateTargetRenderSizeUI();
		}
		/// <inheritdoc />
		protected override void OnGotFocus()
		{
			base.OnGotFocus();
			this.SampleRecentTargetRenderSize();
		}

		private void textBoxRenderWidth_ProceedRequested(object sender, EventArgs e)
		{
			this.textBoxRenderHeight.Focus();
		}
		private void textBoxRenderWidth_EditingFinished(object sender, EventArgs e)
		{
			if (this.isUpdatingUI) return;
			this.ParseAndValidateTargetRenderSize();
		}
		private void textBoxRenderHeight_ProceedRequested(object sender, EventArgs e)
		{
			this.textBoxRenderWidth.Focus();
		}
		private void textBoxRenderHeight_EditingFinished(object sender, EventArgs e)
		{
			if (this.isUpdatingUI) return;
			this.ParseAndValidateTargetRenderSize();
		}
		private void dropdownResolution_DropDownOpening(object sender, EventArgs e)
		{
			this.InitResolutionDropDownItems();
		}
		private void dropdownResolution_GameViewSizeClicked(object sender, EventArgs e)
		{
			this.TargetRenderSizeMode = SpecialRenderSize.CamView;
		}
		private void dropdownResolution_TargetSizeClicked(object sender, EventArgs e)
		{
			this.TargetRenderSizeMode = SpecialRenderSize.GameTarget;
		}
		private void dropdownResolution_FixedSizeClicked(object sender, EventArgs e)
		{
			MenuModelItem item = sender as MenuModelItem;
			Point2 size = (Point2)item.Tag;
			this.TargetRenderSizeMode = SpecialRenderSize.Fixed;
			this.TargetRenderSize = size;
		}
	}
}
