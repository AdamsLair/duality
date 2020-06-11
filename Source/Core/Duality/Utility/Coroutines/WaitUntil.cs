﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Utility.Coroutines
{
	/// <summary>
	/// Struct in charge of managing the timings of a Coroutine's execution
	/// </summary>
	public struct WaitUntil
	{
		private enum WaitType
		{
			Frames,
			GameTime,
			RealTime
		}

		/// <summary>
		/// Waits until the next frame
		/// </summary>
		public static readonly WaitUntil NextFrame = new WaitUntil(1, WaitType.Frames);

		private readonly WaitType type;
		private float internalValue;

		public bool IsComplete
		{
			get { return this.internalValue <= 0; }
		}

		private WaitUntil(float startingValue, WaitType type)
		{
			this.internalValue = startingValue;
			this.type = type;
		}

		internal void Update()
		{
			switch (this.type)
			{
				case WaitType.Frames:
					this.internalValue -= 1;
					break;

				case WaitType.GameTime:
					this.internalValue -= Time.DeltaTime;
					break;

				case WaitType.RealTime:
					this.internalValue -= Time.UnscaledDeltaTime;
					break;
			}
		}

		/// <summary>
		/// Waits until the desired number of frames
		/// </summary>
		/// <param name="frames">The number of frames to wait</param>
		/// <returns>A new WaitUntil struct</returns>
		public static WaitUntil Frames(int frames)
		{
			return new WaitUntil(frames, WaitType.Frames);
		}

		/// <summary>
		/// Waits until the desired number of seconds
		/// </summary>
		/// <param name="seconds">The amount of seconds to wait</param>
		/// <param name="realTime">If true, the countdown is made based on real time, game time (default) otherwise</param>
		/// <returns>A new WaitUntil struct</returns>
		public static WaitUntil Seconds(float seconds, bool realTime = false)
		{
			return new WaitUntil(seconds, realTime ? WaitType.RealTime : WaitType.GameTime);
		}

		/// <summary>
		/// Waits until the desired amount of time
		/// </summary>
		/// <param name="timeSpan">The amount of time to wait</param>
		/// <param name="realTime">If true, the countdown is made based on real time, game time (default) otherwise</param>
		/// <returns>A new WaitUntil struct</returns>
		public static WaitUntil TimeSpan(TimeSpan timeSpan, bool realTime = false)
		{
			return WaitUntil.Seconds((float)timeSpan.TotalSeconds, realTime);
		}
	}
}
