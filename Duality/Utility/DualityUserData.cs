using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Describes the way a Duality window is set up.
	/// </summary>
	public enum ScreenMode
	{
		/// <summary>
		/// Duality runs in windowed mode. The window can be resized by the user.
		/// </summary>
		Window,
		/// <summary>
		/// Duality runs in windowed mode. The window has a fixed size.
		/// </summary>
		FixedWindow,
		/// <summary>
		/// Duality runs in windowed mode. The window is borderless and covers the whole screen.
		/// </summary>
		FullWindow,
		/// <summary>
		/// Duality runs in fullscreen mode, using whatever screen resolution is currently active on the users desktop.
		/// </summary>
		Native,
		/// <summary>
		/// Duality runs in fullscreen mode and changes desktop resolution whenever necesary.
		/// </summary>
		Fullscreen
	}

	/// <summary>
	/// Specifies the quality level of the anti-aliasing used for rendering.
	/// </summary>
	public enum AAQuality
	{
		/// <summary>
		/// Highest possible quality. Sacrifices performance for smooth edges. Can be a problem on older machines.
		/// </summary>
		High,
		/// <summary>
		/// Medium quality. A tradeoff between looks and Profile.
		/// </summary>
		Medium,
		/// <summary>
		/// Low quality. Favors Profile.
		/// </summary>
		Low,
		/// <summary>
		/// No hardware anti-aliasing is used at all.
		/// </summary>
		Off
	}

	/// <summary>
	/// Specifies intervals and modes to refresh the screen and update the game.
	/// </summary>
	public enum RefreshMode
	{
		/// <summary>
		/// Refreshes occur as fast as possible with no wait inbetween.
		/// </summary>
		NoSync,
		/// <summary>
		/// Refreshes target 60 FPS and will use wait for each frame to use its entire available time.
		/// Doesn't use hardware / driver VSync, but prevents 100% CPU usage.
		/// </summary>
		ManualSync,
		/// <summary>
		/// Refreshes wait for the hardware / driver VSync.
		/// </summary>
		VSync
	}

	/// <summary>
	/// Provides information about user settings for this Duality application / game.
	/// It is persistent beyond installing or deleting this Duality game.
	/// </summary>
	public class DualityUserData
	{
		private	string		userName			= "Unknown";
		private	int			gfxWidth			= 800;
		private	int			gfxHeight			= 600;
		private	ScreenMode	gfxMode				= ScreenMode.Window;
		private	AAQuality	gfxAAQuality		= AAQuality.High;
		private	RefreshMode	gfxRefreshMode		= RefreshMode.VSync;
		private	float		sfxEffectVol		= 1.0f;
		private	float		sfxSpeechVol		= 1.0f;
		private	float		sfxMusicVol			= 1.0f;
		private	float		sfxMasterVol		= 1.0f;
		private	bool		systemCursorVisible	= false;
		private	object		customData			= null;

		/// <summary>
		/// [GET / SET] The player's name. This may be his main character's name or simply remain unused.
		/// </summary>
		public string UserName
		{
			get { return this.userName; }
			set { this.userName = value; }
		}
		/// <summary>
		/// [GET / SET] Width of the game's display area.
		/// </summary>
		public int GfxWidth
		{
			get { return this.gfxWidth; }
			set { this.gfxWidth = value; }
		}
		/// <summary>
		/// [GET / SET] Height of the game's display area.
		/// </summary>
		public int GfxHeight
		{
			get { return this.gfxHeight; }
			set { this.gfxHeight = value; }
		}
		/// <summary>
		/// [GET / SET] Describes the way the game window is set up.
		/// </summary>
		public ScreenMode GfxMode
		{
			get { return this.gfxMode; }
			set { this.gfxMode = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies the quality of anti-aliasing used in rendering.
		/// </summary>
		public AAQuality AntialiasingQuality
		{
			get { return this.gfxAAQuality; }
			set { this.gfxAAQuality = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies the way in which Duality switches and refreshes visual and update frames.
		/// </summary>
		public RefreshMode RefreshMode
		{
			get { return this.gfxRefreshMode; }
			set { this.gfxRefreshMode = value; }
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
		/// [GET / SET] Volume factor of sound effects. This is applied automatically by the <see cref="SoundDevice"/> based on the <see cref="SoundType"/>.
		/// </summary>
		public float SfxEffectVol
		{
			get { return this.sfxEffectVol; }
			set { this.sfxEffectVol = value; }
		}
		/// <summary>
		/// [GET / SET] Volume factor of speech / vocals. This is applied automatically by the <see cref="SoundDevice"/> based on the <see cref="SoundType"/>.
		/// </summary>
		public float SfxSpeechVol
		{
			get { return this.sfxSpeechVol; }
			set { this.sfxSpeechVol = value; }
		}
		/// <summary>
		/// [GET / SET] Volume factor of music. This is applied automatically by the <see cref="SoundDevice"/> based on the <see cref="SoundType"/>.
		/// </summary>
		public float SfxMusicVol
		{
			get { return this.sfxMusicVol; }
			set { this.sfxMusicVol = value; }
		}
		/// <summary>
		/// [GET / SET] Volume master factor for sound in general. This is applied automatically by the <see cref="SoundDevice"/>.
		/// </summary>
		public float SfxMasterVol
		{
			get { return this.sfxMasterVol; }
			set { this.sfxMasterVol = value; }
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
