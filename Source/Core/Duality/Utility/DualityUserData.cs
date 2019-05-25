using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Provides information about user settings for this Duality application.
	/// It is persistent and usually tied to the game folder.
	/// </summary>
	public class DualityUserData
	{
		private Point2      windowSize          = new Point2(800, 600);
		private ScreenMode  windowMode          = ScreenMode.Window;
		private RefreshMode windowRefreshMode   = RefreshMode.AdaptiveVSync;
		private AAQuality   antialiasingQuality = AAQuality.High;
		private float       soundEffectVol      = 1.0f;
		private float       soundSpeechVol      = 1.0f;
		private float       soundMusicVol       = 1.0f;
		private float       soundMasterVol      = 1.0f;
		private bool        systemCursorVisible = false;
		private object      customData          = null;

		/// <summary>
		/// [GET / SET] Size of the game's display area when in windowed mode.
		/// </summary>
		public Point2 WindowSize
		{
			get { return this.windowSize; }
			set { this.windowSize = value; }
		}
		/// <summary>
		/// [GET / SET] Describes the way the game window is set up with regard to the screen on which the game runs.
		/// </summary>
		public ScreenMode WindowMode
		{
			get { return this.windowMode; }
			set { this.windowMode = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies the quality of anti-aliasing used in rendering.
		/// </summary>
		public AAQuality AntialiasingQuality
		{
			get { return this.antialiasingQuality; }
			set { this.antialiasingQuality = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies the way in which Duality switches and refreshes visual and update frames.
		/// </summary>
		public RefreshMode WindowRefreshMode
		{
			get { return this.windowRefreshMode; }
			set { this.windowRefreshMode = value; }
		}
		/// <summary>
		/// [GET / SET] Determines whether or not the system cursor should be visible in windowed mode.
		/// </summary>
		public bool SystemCursorVisible
		{
			get { return this.systemCursorVisible; }
			set { this.systemCursorVisible = value; }
		}
		/// <summary>
		/// [GET / SET] Volume factor of sound effects. This is applied automatically by the <see cref="Duality.Audio.SoundDevice"/> based on the <see cref="Duality.Audio.SoundType"/>.
		/// </summary>
		public float SoundEffectVol
		{
			get { return this.soundEffectVol; }
			set { this.soundEffectVol = value; }
		}
		/// <summary>
		/// [GET / SET] Volume factor of speech / vocals. This is applied automatically by the <see cref="Duality.Audio.SoundDevice"/> based on the <see cref="Duality.Audio.SoundType"/>.
		/// </summary>
		public float SoundSpeechVol
		{
			get { return this.soundSpeechVol; }
			set { this.soundSpeechVol = value; }
		}
		/// <summary>
		/// [GET / SET] Volume factor of music. This is applied automatically by the <see cref="Duality.Audio.SoundDevice"/> based on the <see cref="Duality.Audio.SoundType"/>.
		/// </summary>
		public float SoundMusicVol
		{
			get { return this.soundMusicVol; }
			set { this.soundMusicVol = value; }
		}
		/// <summary>
		/// [GET / SET] Volume master factor for sound in general. This is applied automatically by the <see cref="Duality.Audio.SoundDevice"/>.
		/// </summary>
		public float SoundMasterVol
		{
			get { return this.soundMasterVol; }
			set { this.soundMasterVol = value; }
		}
		/// <summary>
		/// [GET / SET] Use this property to store custom user data.
		/// </summary>
		public object CustomData
		{
			get { return this.customData; }
			set { this.customData = value; }
		}
	}
}
