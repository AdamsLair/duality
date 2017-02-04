using System.Linq;

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
