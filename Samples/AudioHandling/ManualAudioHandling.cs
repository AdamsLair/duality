using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Input;
using Duality.Components;
using Duality.Drawing;
using Duality.Audio;
using Duality.Resources;

namespace AudioHandling
{
	/// <summary>
	/// A sample Component demonstrating how to handle audio manually using code.
	/// </summary>
	public class ManualAudioHandling : Component, ICmpUpdatable, ICmpRenderer
	{
		private ContentRef<Sound>[] soundsOutside = new ContentRef<Sound>[0];
		private ContentRef<Sound>[] soundsInside  = new ContentRef<Sound>[0];

		[DontSerialize] private SoundInstance[] playingOutside = new SoundInstance[0];
		[DontSerialize] private SoundInstance[] playingInside  = new SoundInstance[0];

		[DontSerialize] private float         targetInside = 0.0f;
		[DontSerialize] private float         inside       = 0.0f;
		[DontSerialize] private FormattedText infoText     = null;
		[DontSerialize] private FormattedText stateText    = null;

		public ContentRef<Sound>[] SoundsOutside
		{
			get { return this.soundsOutside; }
			set { this.soundsOutside = value ?? new ContentRef<Sound>[0]; }
		}
		public ContentRef<Sound>[] SoundsInside
		{
			get { return this.soundsInside; }
			set { this.soundsInside = value ?? new ContentRef<Sound>[0]; }
		}

		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
		}
		void ICmpUpdatable.OnUpdate()
		{
			// Allow the user to input where to go
			if (DualityApp.Keyboard.KeyHit(Key.Number1))
				this.targetInside = 1.0f;
			if (DualityApp.Keyboard.KeyHit(Key.Number2))
				this.targetInside = 0.0f;

			// Is there a Gamepad we can use?
			GamepadInput gamepad = DualityApp.Gamepads.FirstOrDefault(g => g.IsAvailable);
			if (gamepad != null)
			{
				if (gamepad.ButtonHit(GamepadButton.A))
					this.targetInside = 1.0f;
				if (gamepad.ButtonHit(GamepadButton.B))
					this.targetInside = 0.0f;
			}

			// Walk around
			this.inside += (this.targetInside - this.inside) * 0.01f * Time.TimeMult;

			// Make sure we're playing what we're supposed to
			this.SyncSounds(this.soundsInside, ref this.playingInside);
			this.SyncSounds(this.soundsOutside, ref this.playingOutside);

			// Apply settings based on where we are
			for (int i = 0; i < this.playingInside.Length; i++)
			{
				if (this.playingInside[i] == null) continue;
				this.playingInside[i].Volume = MathF.Lerp(0.1f, 1.0f, this.inside);
				this.playingInside[i].Lowpass = MathF.Lerp(0.1f, 1.0f, this.inside);
			}
			for (int i = 0; i < this.playingOutside.Length; i++)
			{
				if (this.playingOutside[i] == null) continue;
				this.playingOutside[i].Volume = MathF.Lerp(1.0f, 0.5f, this.inside);
				this.playingOutside[i].Lowpass = MathF.Lerp(1.0f, 0.1f, this.inside);
			}
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Canvas canvas = new Canvas();
			canvas.Begin(device);
			
			Vector2 screenSize = device.TargetSize;

			// Draw some info text
			if (this.infoText == null)
			{
				this.infoText = new FormattedText();
				this.infoText.MaxWidth = 350;
			}
			this.infoText.SourceText = string.Format(
				"Manual Audio Handling Sample/n/n" +
				"Use /c44AAFFFFnumber key 1/cFFFFFFFF // Gamepad /c44AAFFFFbutton A/cFFFFFFFF to go inside and close the door./n" + 
				"Use /c44AAFFFFnumber key 2/cFFFFFFFF // Gamepad /c44AAFFFFbutton B/cFFFFFFFF to open the door and go back outside.");

			canvas.State.ColorTint = ColorRgba.White;
			canvas.DrawText(this.infoText, 10, 10, 0.0f, null, Alignment.TopLeft, true);

			// Draw state information on the current camera controller
			if (this.stateText == null) this.stateText = new FormattedText();
			this.stateText.SourceText = string.Format(
				"Where am I? /cFF8800FF{0}/cFFFFFFFF",
				this.inside > 0.5f ? "In my cozy room" : "Out in the cold");

			canvas.State.ColorTint = ColorRgba.White;
			canvas.DrawText(this.stateText, 10, screenSize.Y - 10, 0.0f, null, Alignment.BottomLeft, true);
			canvas.DrawText(string.Format("{0:F}", this.inside), 250, screenSize.Y - 10, 0.0f, Alignment.BottomLeft, true);

			canvas.End();
		}

		private void SyncSounds(IList<ContentRef<Sound>> sounds, ref SoundInstance[] playing)
		{
			if (playing.Length != sounds.Count)
				Array.Resize(ref playing, sounds.Count);

			for (int i = 0; i < sounds.Count; i++)
			{
				// If the sound has been changed, make sure to stop the old playing one
				if (playing[i] != null && sounds[i] != playing[i].Sound)
				{
					playing[i].FadeOut(1.0f);
					playing[i] = null;
				}

				// If we're not playing a sound yet, start
				if (sounds[i] != null && (playing[i] == null || playing[i].Disposed))
				{
					playing[i] = DualityApp.Sound.PlaySound(sounds[i]);
					playing[i].Looped = true;
					playing[i].BeginFadeIn(1.0f);
				}
			}
		}
	}
}
