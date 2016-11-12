﻿using System.Linq;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Provides a full preview of the game within the editor. 
	/// This state renders the games actual audiovisual output and reroutes user input to the game.
	/// </summary>
	public class GameViewCamViewState : CamViewState
	{
		public override string StateName
		{
			get { return Properties.CamViewRes.CamViewState_GameView_Name; }
		}

		public GameViewCamViewState()
		{
			this.CameraActionAllowed = false;
			this.EngineUserInput = true;
		}

		protected internal override void OnEnterState()
		{
			base.OnEnterState();
			this.View.SetToolbarCamSettingsEnabled(false);
			this.CameraObj.Active = false;
		}
		protected internal override void OnLeaveState()
		{
			base.OnLeaveState();
			this.View.SetToolbarCamSettingsEnabled(true);
			this.CameraObj.Active = true;
		}
		protected override void OnRenderState()
		{
			// Render game pov
			Rect viewportRect = new Rect(this.ClientSize.Width, this.ClientSize.Height);
			if (!Scene.Current.FindComponents<Camera>().Any())	DrawDevice.RenderVoid(viewportRect);
			else												DualityApp.Render(viewportRect);
		}
	}
}
