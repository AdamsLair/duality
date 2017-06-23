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
		private Size targetRenderSize = Size.Empty;
		private bool isNativeRenderSize = false;
		private bool isUpdatingUI = false;


		public override string StateName
		{
			get { return Properties.CamViewRes.CamViewState_GameView_Name; }
		}
		private Size TargetRenderSize
		{
			get { return this.targetRenderSize; }
			set
			{
				if (this.targetRenderSize != value)
				{
					this.isNativeRenderSize = (this.ClientSize == value);
					this.targetRenderSize = value;
					this.UpdateTargetRenderSizeUI();
				}
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
			this.textBoxRenderWidth.ProceedRequested += (sender, e) => this.textBoxRenderHeight.Focus();
			this.textBoxRenderWidth.ValueChanged += (sender, e) => this.OnTargetRenderSizeUIValueChanged();

			this.textBoxRenderHeight = new ToolStripTextBoxAdv("textBoxRenderHeight");
			this.textBoxRenderHeight.BackColor = Color.FromArgb(196, 196, 196);
			this.textBoxRenderHeight.AutoSize = false;
			this.textBoxRenderHeight.Width = 35;
			this.textBoxRenderHeight.MaxLength = 4;
			this.textBoxRenderHeight.ProceedRequested += (sender, e) => this.textBoxRenderWidth.Focus();
			this.textBoxRenderHeight.ValueChanged += (sender, e) => this.OnTargetRenderSizeUIValueChanged();

			this.toolbarItems.Add(new ToolStripLabel("Screen Size "));
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
			this.TargetRenderSize = this.ClientSize;
		}
		private void UpdateTargetRenderSizeUI()
		{
			this.isUpdatingUI = true;

			Color backColor = this.isNativeRenderSize ? 
				Color.FromArgb(196, 196, 196) : 
				Color.FromArgb(196, 224, 255);

			this.textBoxRenderWidth.Text = this.targetRenderSize.Width.ToString();
			this.textBoxRenderHeight.Text = this.targetRenderSize.Height.ToString();
			this.textBoxRenderWidth.BackColor = backColor;
			this.textBoxRenderHeight.BackColor = backColor;

			this.isUpdatingUI = false;
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

			Point2 clientSize = new Point2(this.ClientSize.Width, this.ClientSize.Height);
			bool isRenderableScene = Scene.Current.FindComponents<Camera>().Any();
			
			Vector2 imageSize;
			Rect viewportRect;
			DualityApp.CalculateGameViewport(clientSize, out viewportRect, out imageSize);

			// In case we're not using the entire viewport area, no cameras are
			// rendering on screen or similar, clear the entire available area first.
			DrawDevice.RenderVoid(new Rect(clientSize));

			if (isRenderableScene)
			{
				DualityApp.Render(viewportRect, imageSize);
			}
		}
		protected override void OnResize()
		{
			base.OnResize();
			if (this.isNativeRenderSize)
				this.ResetTargetRenderSize();
		}
		private void OnTargetRenderSizeUIValueChanged()
		{
			if (this.isUpdatingUI) return;

			int width;
			int height;
			if (!int.TryParse(this.textBoxRenderWidth.Text, out width)) return;
			if (!int.TryParse(this.textBoxRenderHeight.Text, out height)) return;

			this.TargetRenderSize = new Size(width, height);
		}
	}
}
