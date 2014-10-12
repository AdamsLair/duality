using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;

using Duality;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	[Serializable]
	public class GameOverScreen : Component, ICmpRenderer, ICmpUpdatable
	{
		private ContentRef<Font>	font				= null;
		private	bool				gameStarted			= false;
		private	bool				gameOver			= false;
		private float				lastTimeAnyAlive	= 0.0f;
		private	BatchInfo			blendMaterial		= null;

		[NonSerialized] private CanvasBuffer buffer = null;


		float ICmpRenderer.BoundRadius
		{
			get { return float.MaxValue; }
		}
		public ContentRef<Font> Font
		{
			get { return this.font; }
			set { this.font = value; }
		}
		public BatchInfo BlendMaterial
		{
			get { return this.blendMaterial; }
			set { this.blendMaterial = value; }
		}
		public bool IsGameOver
		{
			get { return this.gameOver; }
		}
		

		void ICmpUpdatable.OnUpdate()
		{
			if (Player.IsAnyPlayerAlive)
			{
				this.gameStarted = true;
				this.lastTimeAnyAlive = (float)Time.MainTimer.TotalMilliseconds;
			}
			else if (this.gameStarted)
			{
				this.gameOver = true;
			}
		}
		bool ICmpRenderer.IsVisible(IDrawDevice device)
		{
			// Only render when in screen overlay mode and the visibility mask is non-empty.
			return 
				(device.VisibilityMask & VisibilityFlag.AllGroups) != VisibilityFlag.None &&
				(device.VisibilityMask & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			// Create a buffer to cache and re-use vertices. Not required, but will boost performance.
			if (this.buffer == null) this.buffer = new CanvasBuffer();

			// Create a Canvas to auto-generate vertices from high-level drawing commands.
			Canvas canvas = new Canvas(device, this.buffer);
			canvas.State.TextFont = this.font;

			// If no player is left alive, display "game over" screen
			if (this.gameOver)
			{
				float timeSinceGameOver = (float)Time.MainTimer.TotalMilliseconds - this.lastTimeAnyAlive;
				float gameOverAnimProgress = MathF.Clamp(timeSinceGameOver / 5000.0f, 0.0f, 1.0f);
				float blendAnimProgress = MathF.Clamp((gameOverAnimProgress - 0.5f) / 0.25f, 0.0f, 1.0f);
				float textAnimProgress = MathF.Clamp((gameOverAnimProgress - 0.75f) / 0.25f, 0.0f, 1.0f);

				if (this.blendMaterial != null && blendAnimProgress > 0.0f)
				{
					canvas.PushState();

					this.blendMaterial.SetUniform("threshold", 1.0f - blendAnimProgress);
					canvas.State.SetMaterial(this.blendMaterial);
					if (this.blendMaterial.MainTexture != null)
					{
						canvas.State.TextureCoordinateRect = new Rect(
							0, 
							0, 
							device.TargetSize.X / canvas.State.TextureBaseSize.X, 
							device.TargetSize.Y / canvas.State.TextureBaseSize.Y);
					}
					canvas.FillRect(0, 0, device.TargetSize.X, device.TargetSize.Y);

					canvas.PopState();
				}

				if (this.font != null && textAnimProgress > 0.0f)
				{
					canvas.PushState();

					string gameOverText = "Game Over";
					Vector2 fullTextSize = canvas.MeasureText(gameOverText);
					Vector2 textPos = device.TargetSize * 0.5f - fullTextSize * 0.5f;
					gameOverText = gameOverText.Substring(0, MathF.RoundToInt(gameOverText.Length * textAnimProgress));

					canvas.State.ColorTint = ColorRgba.White;
					canvas.DrawText(gameOverText, textPos.X, textPos.Y);

					canvas.PopState();
				}
			}
		}
	}
}
