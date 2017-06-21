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
		private ToolStrip toolstrip = null;
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

		private void AddToolbar()
		{
			this.View.SuspendLayout();
			this.toolstrip = new ToolStrip();
			this.toolstrip.SuspendLayout();

			this.toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolstrip.Name = "toolstrip";
			this.toolstrip.Text = "GameView Tools";
			this.toolstrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
			this.toolstrip.BackColor = Color.FromArgb(212, 212, 212);

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

			this.toolstrip.Items.Add(new ToolStripLabel("Screen Size "));
			this.toolstrip.Items.Add(this.textBoxRenderWidth);
			this.toolstrip.Items.Add(new ToolStripLabel("x"));
			this.toolstrip.Items.Add(this.textBoxRenderHeight);

			this.View.Controls.Add(this.toolstrip);
			this.View.Controls.SetChildIndex(this.toolstrip, this.View.Controls.IndexOf(this.View.ToolbarCamera));
			this.toolstrip.ResumeLayout(true);
			this.View.ResumeLayout(true);
		}
		private void RemoveToolbar()
		{
			this.toolstrip.Dispose();
			this.toolstrip = null;
			this.textBoxRenderWidth = null;
			this.textBoxRenderHeight = null;
		}

		protected internal override void OnEnterState()
		{
			base.OnEnterState();
			this.View.SetToolbarCamSettingsEnabled(false);
			this.CameraObj.Active = false;
			this.AddToolbar();
		}
		protected internal override void OnLeaveState()
		{
			base.OnLeaveState();
			this.RemoveToolbar();
			this.View.SetToolbarCamSettingsEnabled(true);
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
