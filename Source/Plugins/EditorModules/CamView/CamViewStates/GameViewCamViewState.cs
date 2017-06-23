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


		public override string StateName
		{
			get { return Properties.CamViewRes.CamViewState_GameView_Name; }
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

			this.textBoxRenderHeight = new ToolStripTextBoxAdv("textBoxRenderHeight");
			this.textBoxRenderHeight.BackColor = Color.FromArgb(196, 196, 196);
			this.textBoxRenderHeight.AutoSize = false;
			this.textBoxRenderHeight.Width = 35;
			this.textBoxRenderHeight.MaxLength = 4;
			this.textBoxRenderHeight.ProceedRequested += (sender, e) => this.textBoxRenderWidth.Focus();

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

		protected internal override void OnEnterState()
		{
			base.OnEnterState();
			this.View.SetEditingToolsAvailable(false);
			this.CameraObj.Active = false;
			this.AddToolbarItems();
		}
		protected internal override void OnLeaveState()
		{
			base.OnLeaveState();
			this.RemoveToolbarItems();
			this.View.SetEditingToolsAvailable(true);
			this.CameraObj.Active = true;
		}
		protected override void OnRenderState()
		{
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
	}
}
