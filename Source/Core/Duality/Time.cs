using System;
using System.Diagnostics;

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
		/// The amount of frame per second at the desired refresh rate of 60 FPS.
		/// </summary>
		public const float FramesPerSecond = 60.0f;
		/// <summary>
		/// Milliseconds a frame takes at the desired refresh rate of 60 FPS
		/// </summary>
		public const float MillisecondsPerFrame = 1000.0f / FramesPerSecond;
		/// <summary>
		/// Seconds a frame takes at the desired refresh rate of 60 FPS
		/// </summary>
		public const float SecondsPerFrame = 1.0f / FramesPerSecond;

		private static DateTime  startup    = DateTime.Now;
		private static Stopwatch watch      = new Stopwatch();
		private static TimeSpan  gameTimer  = TimeSpan.Zero;
		private static double    frameBegin = 0.0d;
		private static float     gameDelta  = 0.0f;
		private static float     realDelta  = 0.0f;
		private static float     timeMult   = 0.0f;
		private static float     timeScale  = 1.0f;
		private static int       timeFreeze = 0;
		private static int       frameCount = 0;
		private static int       fps        = 0;
		private static int       fps_frames = 0;
		private static double    fps_last   = 0.0d;

		/// <summary>
		/// [GET] Returns the date and time of engine startup.
		/// </summary>
		public static DateTime StartupTime
		{
			get { return startup; }
		}
		/// <summary>
		/// [GET] Returns the real, unscaled time that has passed since engine startup.
		/// </summary>
		public static TimeSpan MainTimer
		{
			get { return watch.Elapsed; }
		}
		/// <summary>
		/// [GET] Returns the time passed since the last frame in seconds weighted by <see cref="TimeScale"/>.
		/// You can multiply your "per second" updates with this value to make them framerate independent.
		/// </summary>
		public static float DeltaTime
		{
			get { return gameDelta; }
		}
		/// <summary>
		/// [GET] Returns the real, unscaled and unclamped time passed since the last frame in seconds.
		/// </summary>
		public static float UnscaledDeltaTime
		{
			get { return realDelta; }
		}
		/// <summary>
		/// [GET] Frames per Second
		/// </summary>
		public static float Fps
		{
			get { return fps; }
		}
		/// <summary>
		/// [GET] Returns the game time that has passed since engine startup. Since it's game time, this timer will stop
		/// when pausing or freezing and also run slower or faster according to <see cref="TimeScale"/>.
		/// </summary>
		public static TimeSpan GameTimer
		{
			get { return gameTimer; }
		}
		/// <summary>
		/// [GET] A factor that represents how long the last frame took relative to the desired
		/// frame time. When your game runs at half the target frame rate, this factor will be 2.0f,
		/// when it runs at double the target frame rate, it will be 0.5f and so on. Similar to
		/// <see cref="DeltaTime"/>, except as an abstract factor, rather than passed time.
		/// 
		/// You can multiply your "per frame" updates with this value to make them framerate independent
		/// in the same way you can multiply your "per second" updates with <see cref="DeltaTime"/>.
		/// </summary>
		public static float TimeMult
		{
			get { return timeMult; }
		}
		/// <summary>
		/// [GET / SET] Specifies how fast game time runs compared to real time i.e. how
		/// fast the game runs. May be used for slow motion effects.
		/// </summary>
		public static float TimeScale
		{
			get { return timeScale; }
			set { timeScale = value; }
		}
		/// <summary>
		/// [GET] The number of frames passed since startup
		/// </summary>
		public static int FrameCount
		{
			get { return frameCount; }
		}

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

			double mainTimer = Time.MainTimer.TotalSeconds;
			realDelta = (float)(mainTimer - frameBegin);
			if (forceFixedStep)
				gameDelta = timeScale * SecondsPerFrame;
			else
				gameDelta = timeScale * MathF.Min(realDelta, SecondsPerFrame * 2); // Don't skip more than 2 frames / just simulate slower when below 30 fps
			frameBegin = mainTimer;

			if (timeFreeze == 0)
			{
				if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
					gameTimer += TimeSpan.FromTicks((long)(gameDelta * TimeSpan.TicksPerSecond));
				timeMult = gameDelta / SecondsPerFrame;
			}
			else
			{
				timeMult = 0.0f;
			}

			fps_frames++;
			if (mainTimer - fps_last >= 1.0f)
			{
				fps = fps_frames;
				fps_frames = 0;
				fps_last = mainTimer;
			}
		}
	}
}
