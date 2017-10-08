using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	public class GameOverScreen : Component, ICmpRenderer, ICmpUpdatable
	{
		private ContentRef<Font>     font                     = null;
		private BatchInfo            blendMaterial            = null;
		private ContentRef<Material> controlInfoMouseKeyboard = null;
		private ContentRef<Material> controlInfoGamepad       = null;
		
		[DontSerialize] private bool   gameStarted      = false;
		[DontSerialize] private bool   gameOver         = false;
		[DontSerialize] private bool   gameWin          = false;
		[DontSerialize] private float  lastTimeAnyAlive = 0.0f;
		[DontSerialize] private Canvas canvas           = null;


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
		public ContentRef<Material> ControlsMouseKeyboard
		{
			get { return this.controlInfoMouseKeyboard; }
			set { this.controlInfoMouseKeyboard = value; }
		}
		public ContentRef<Material> ControlsGamepad
		{
			get { return this.controlInfoGamepad; }
			set { this.controlInfoGamepad = value; }
		}
		public bool HasGameEnded
		{
			get { return this.gameOver || this.gameWin; }
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
				{
					this.gameWin = true;
					SpawnPoint.LastVisitedIndex = -1;
				}
				if (!Player.IsAnyPlayerAlive)
				{
					this.gameOver = true;
				}
			}
		}
		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			// Create a Canvas for high-level drawing commands.
			// We'll re-use this to keep performance high and allocations low.
			if (this.canvas == null) this.canvas = new Canvas();

			// Prepare the canvas for drawing
			this.canvas.Begin(device);
			this.canvas.State.TextFont = this.font;

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
				float controlInfoAnimProgress = MathF.Clamp(((timeSinceGameOver - animOffset) - animTime - 2000.0f) / 2000.0f, 0.0f, 1.0f);
				float blendAnimProgress = MathF.Clamp(gameOverAnimProgress / blendDurationRatio, 0.0f, 1.0f);
				float textAnimProgress = MathF.Clamp((gameOverAnimProgress - blendDurationRatio - textOffsetRatio) / (1.0f - blendDurationRatio - textOffsetRatio), 0.0f, 1.0f);

				if (this.blendMaterial != null && blendAnimProgress > 0.0f)
				{
					this.canvas.PushState();

					if (this.gameOver)
					{
						// Set up our special blending Material and specify the threshold to blend to
						this.blendMaterial.SetValue("threshold", 1.0f - blendAnimProgress);
						this.canvas.State.SetMaterial(this.blendMaterial);
						this.canvas.State.ColorTint = ColorRgba.Black;

						// Specify a texture coordinate rect so it spans the entire screen repeating itself, instead of being stretched
						if (this.blendMaterial.MainTexture != null)
						{
							Random rnd = new Random((int)this.lastTimeAnyAlive);
							Vector2 randomTranslate = rnd.NextVector2(0.0f, 0.0f, this.canvas.State.TextureBaseSize.X, this.canvas.State.TextureBaseSize.Y);
							this.canvas.State.TextureCoordinateRect = new Rect(
								randomTranslate.X, 
								randomTranslate.Y, 
								device.TargetSize.X / this.canvas.State.TextureBaseSize.X, 
								device.TargetSize.Y / this.canvas.State.TextureBaseSize.Y);
						}
					}
					else
					{
						// If we won, simply fade to white
						this.canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Add));
						this.canvas.State.ColorTint = ColorRgba.White.WithAlpha(blendAnimProgress);
					}

					// Fill the screen with a rect of our Material
					this.canvas.FillRect(0, 0, device.TargetSize.X, device.TargetSize.Y);

					this.canvas.PopState();
				}

				if (this.font != null && textAnimProgress > 0.0f)
				{
					this.canvas.PushState();

					// Determine which text to draw to screen and where to draw it
					string gameOverText = this.gameWin ? "is it over..?" : "darkness...";
					Vector2 fullTextSize = this.canvas.MeasureText(gameOverText);
					Vector2 textPos = (Vector2)device.TargetSize * 0.5f - fullTextSize * 0.5f;
					gameOverText = gameOverText.Substring(0, MathF.RoundToInt(gameOverText.Length * textAnimProgress));

					// Make sure not to draw inbetween pixels, so the text is perfectly sharp.
					textPos.X = MathF.Round(textPos.X);
					textPos.Y = MathF.Round(textPos.Y);

					// Draw the text to screen
					this.canvas.State.ColorTint = this.gameWin ? ColorRgba.Black : ColorRgba.White;
					this.canvas.DrawText(gameOverText, textPos.X, textPos.Y);

					this.canvas.PopState();
				}

				if (controlInfoAnimProgress > 0.0f)
				{
					Vector2 infoBasePos = (Vector2)device.TargetSize * 0.5f + new Vector2(0.0f, device.TargetSize.Y * 0.25f);
					if (this.controlInfoMouseKeyboard != null)
					{
						this.canvas.PushState();

						Vector2 texSize = (Vector2)this.controlInfoMouseKeyboard.Res.MainTexture.Res.Size * 0.5f;

						this.canvas.State.SetMaterial(this.controlInfoMouseKeyboard);
						this.canvas.State.ColorTint = ColorRgba.White.WithAlpha(controlInfoAnimProgress);
						this.canvas.FillRect(
							infoBasePos.X - texSize.X * 0.5f,
							infoBasePos.Y - texSize.Y - 10,
							texSize.X,
							texSize.Y);

						this.canvas.PopState();
					}
					if (this.controlInfoGamepad != null)
					{
						this.canvas.PushState();

						Vector2 texSize = (Vector2)this.controlInfoGamepad.Res.MainTexture.Res.Size * 0.5f;

						this.canvas.State.SetMaterial(this.controlInfoGamepad);
						this.canvas.State.ColorTint = ColorRgba.White.WithAlpha(controlInfoAnimProgress);
						this.canvas.FillRect(
							infoBasePos.X - texSize.X * 0.5f,
							infoBasePos.Y + 10,
							texSize.X,
							texSize.Y);

						this.canvas.PopState();
					}
				}
			}

			this.canvas.End();
		}
	}
}
