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
		private	BatchInfo			blendMaterial		= null;
		
		[NonSerialized] private	bool			gameStarted			= false;
		[NonSerialized] private	bool			gameOver			= false;
		[NonSerialized] private	bool			gameWin				= false;
		[NonSerialized] private float			lastTimeAnyAlive	= 0.0f;
		[NonSerialized] private CanvasBuffer	buffer				= null;


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
			// If the game has ended, nothing to do here
			if (this.gameOver) return;
			if (this.gameWin) return;

			// Determine whether the game has started / ended
			if (Player.IsAnyPlayerAlive)
			{
				this.gameStarted = true;
				this.lastTimeAnyAlive = (float)Time.MainTimer.TotalMilliseconds;
			}
			if (this.gameStarted)
			{
				if (Player.AllPlayers.All(p => !p.Active || !p.IsPlaying || p.HasReachedGoal))
					this.gameWin = true;
				if (!Player.IsAnyPlayerAlive)
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

			// If the game is over or won, display "game over" screen
			if (this.gameOver || this.gameWin)
			{
				// Various animation timing variables.
				float animOffset = this.gameWin ? 0.0f : 2500.0f;
				float animTime = this.gameWin ? 10000.0f : 4500.0f;
				float blendDurationRatio = this.gameWin ? 0.6f : 0.5f;
				float textOffsetRatio = this.gameWin ? 0.2f : 0.0f;

				float timeSinceGameOver = (float)Time.MainTimer.TotalMilliseconds - this.lastTimeAnyAlive;
				float gameOverAnimProgress = MathF.Clamp((timeSinceGameOver - animOffset) / animTime, 0.0f, 1.0f);
				float blendAnimProgress = MathF.Clamp(gameOverAnimProgress / blendDurationRatio, 0.0f, 1.0f);
				float textAnimProgress = MathF.Clamp((gameOverAnimProgress - blendDurationRatio - textOffsetRatio) / (1.0f - blendDurationRatio - textOffsetRatio), 0.0f, 1.0f);

				if (this.blendMaterial != null && blendAnimProgress > 0.0f)
				{
					canvas.PushState();

					if (this.gameOver)
					{
						// Set up our special blending Material and specify the threshold to blend to
						this.blendMaterial.SetUniform("threshold", 1.0f - blendAnimProgress);
						canvas.State.SetMaterial(this.blendMaterial);
						canvas.State.ColorTint = ColorRgba.Black;

						// Specify a texture coordinate rect so it spans the entire screen repeating itself, instead of being stretched
						if (this.blendMaterial.MainTexture != null)
						{
							Random rnd = new Random((int)this.lastTimeAnyAlive);
							Vector2 randomTranslate = rnd.NextVector2(0.0f, 0.0f, canvas.State.TextureBaseSize.X, canvas.State.TextureBaseSize.Y);
							canvas.State.TextureCoordinateRect = new Rect(
								randomTranslate.X, 
								randomTranslate.Y, 
								device.TargetSize.X / canvas.State.TextureBaseSize.X, 
								device.TargetSize.Y / canvas.State.TextureBaseSize.Y);
						}
					}
					else
					{
						// If we won, simply fade to white
						canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Add, ColorRgba.White));
						canvas.State.ColorTint = ColorRgba.White.WithAlpha(blendAnimProgress);
					}

					// Fill the screen with a rect of our Material
					canvas.FillRect(0, 0, device.TargetSize.X, device.TargetSize.Y);

					canvas.PopState();
				}

				if (this.font != null && textAnimProgress > 0.0f)
				{
					canvas.PushState();

					// Determine which text to draw to screen and where to draw it
					string gameOverText = this.gameWin ? "is it over..?" : "darkness...";
					Vector2 fullTextSize = canvas.MeasureText(gameOverText);
					Vector2 textPos = device.TargetSize * 0.5f - fullTextSize * 0.5f;
					gameOverText = gameOverText.Substring(0, MathF.RoundToInt(gameOverText.Length * textAnimProgress));

					// Make sure not to draw inbetween pixels, so the text is perfectly sharp.
					textPos.X = MathF.Round(textPos.X);
					textPos.Y = MathF.Round(textPos.Y);

					// Draw the text to screen
					canvas.State.ColorTint = this.gameWin ? ColorRgba.Black : ColorRgba.White;
					canvas.DrawText(gameOverText, textPos.X, textPos.Y);

					canvas.PopState();
				}
			}
		}
	}
}
