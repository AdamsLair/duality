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
			Vector2 clientSize = new Vector2(this.ClientSize.Width, this.ClientSize.Height);
			bool isRenderableScene = Scene.Current.FindComponents<Camera>().Any();

			Vector2 imageSize = clientSize;
			Rect viewportRect = new Rect(clientSize);
			Point2 forcedSize = DualityApp.AppData.ForcedRenderSize;
			if (forcedSize.X > 0 && forcedSize.Y > 0 && forcedSize != imageSize)
			{
				Vector2 adjustedViewportSize = DualityApp.AppData.ForcedRenderResizeMode.Apply(forcedSize, viewportRect.Size);
				imageSize = forcedSize;
				viewportRect = Rect.Align(
					Alignment.Center, 
					viewportRect.Size.X * 0.5f, 
					viewportRect.Size.Y * 0.5f, 
					adjustedViewportSize.X, 
					adjustedViewportSize.Y);
			}

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
