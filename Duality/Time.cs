using System;
using System.Diagnostics;

using Duality.Profiling;

namespace Duality
{
	/// <summary>
	/// The Time class provides a global interface for time measurement and control. It affects all time-dependent computations. 
	/// Use the <see cref="TimeMult"/> Property to make your own computations time-dependent instead of frame-dependent. Otherwise, your
	/// game logic will depend on how many FPS the player's machine achieves and mit behave differently on very slow or fast machines.
	/// </summary>
	public static class Time
	{
		/// <summary>
		/// Milliseconds a frame takes at the desired refresh rate of 60 FPS
		/// </summary>
		public const	float	MsPFMult	= 1000.0f / 60.0f;
		/// <summary>
		/// Seconds a frame takes at the desired refresh rate of 60 FPS
		/// </summary>
		public const	float	SPFMult		= 1.0f / 60.0f;

		private	static	DateTime	startup		= DateTime.Now;
		private	static	Stopwatch	watch		= new Stopwatch();
		private	static	TimeSpan	gameTimer	= TimeSpan.Zero;
		private	static	double		frameBegin	= 0.0d;
		private	static	float		lastDelta	= 0.0f;
		private	static	float		timeMult	= 0.0f;
		private	static	float		timeScale	= 1.0f;
		private	static	int			timeFreeze	= 0;
		private	static	int			frameCount	= 0;
		private	static	int			fps			= 0;
		private	static	int			fps_frames	= 0;
		private	static	double		fps_last	= 0.0d;

		/// <summary>
		/// [GET] Returns the date and time of engine startup.
		/// </summary>
		public static DateTime StartupTime
		{
			get { return startup; }
		}	//	G
		/// <summary>
		/// [GET] Returns the real time that has passed since engine startup.
		/// </summary>
		public static TimeSpan MainTimer
		{
			get { return watch.Elapsed; }
		}		//	G
		/// <summary>
		/// [GET] Time in milliseconds the last frame took
		/// </summary>
		public static float LastDelta
		{
			get { return lastDelta; }
		}		//	G
		/// <summary>
		/// [GET] Frames per Second
		/// </summary>
		public static float Fps
		{
			get { return fps; }
		}				//	G
		/// <summary>
		/// [GET] Returns the game time that has passed since engine startup. Since it's game time, this timer will stop
		/// when pausing or freezing and also run slower or faster according to <see cref="TimeScale"/>.
		/// </summary>
		public static TimeSpan GameTimer
		{
			get { return gameTimer; }
		}		//	G
		/// <summary>
		/// [GET] Multiply any frame-independend movement or change with this factor.
		/// It also applies the time scale you set.
		/// </summary>
		public static float TimeMult
		{
			get { return timeMult; }
		}			//	G
		/// <summary>
		/// [GET / SET] Specifies how fast game time runs compared to real time i.e. how
		/// fast the game runs. May be used for slow motion effects.
		/// </summary>
		public static float TimeScale
		{
			get { return timeScale; }
			set { timeScale = value; }
		}		//	GS
		/// <summary>
		/// [GET] The number of frames passed since startup
		/// </summary>
		public static int FrameCount
		{
			get { return frameCount; }
		}			//	G

		/// <summary>
		/// Freezes game time. This will cause the GameTimer to stop and TimeMult to equal zero.
		/// </summary>
		public static void Freeze()
		{
			timeFreeze++;
		}
		/// <summary>
		/// Unfreezes game time. TimeMult resumes to its normal value and GameTimer starts running again.
		/// </summary>
		public static void Resume()
		{
			if (timeFreeze == 0) return;
			timeFreeze--;
		}
		internal static void Resume(bool hardReset)
		{
			if (hardReset)
				timeFreeze = 0;
			else
				Resume();
		}

		internal static void FrameTick(bool forceFixedStep = false)
		{
			// Initial timer start
			if (!watch.IsRunning) watch.Restart();

			Profile.TimeFrame.EndMeasure();
			Profile.TimeFrame.BeginMeasure();

			frameCount++;

			double mainTimer = Time.MainTimer.TotalMilliseconds;
			lastDelta = forceFixedStep ? MsPFMult : MathF.Min((float)(mainTimer - frameBegin), MsPFMult * 2); // Don't skip more than 2 frames / fall below 30 fps
			frameBegin = mainTimer;

			if (timeFreeze == 0)
			{
				if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
					gameTimer += TimeSpan.FromMilliseconds(lastDelta * timeScale);
				timeMult = timeScale * lastDelta / MsPFMult;
			}
			else
			{
				timeMult = 0.0f;
			}

			fps_frames++;
			if (mainTimer - fps_last >= 1000.0f)
			{
				fps = fps_frames;
				fps_frames = 0;
				fps_last = mainTimer;
				//Log.Core.Write("FPS: {0},\tms: {1}", fps, lastDelta);
			}
		}
	}
}
