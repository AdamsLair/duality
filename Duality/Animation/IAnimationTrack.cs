using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	/// <summary>
	/// Describes the animation of a value as a set of <see cref="IAnimationKeyFrame">keyframes</see>.
	/// </summary>
	public interface IAnimationTrack : IEnumerable<IAnimationKeyFrame>
	{
		/// <summary>
		/// [GET / SET] The set of keyframes that is used to describe the animation.
		/// </summary>
		IEnumerable<IAnimationKeyFrame> KeyFrames { get; set; }
		/// <summary>
		/// [GET] The total number of keyframes in this track.
		/// </summary>
		int KeyFrameCount { get; }
		/// <summary>
		/// [GET / SET] Describes whether or not this track is considered to be a loop.
		/// </summary>
		bool IsLooping { get; set; }
		/// <summary>
		/// [GET] Returns the total duration of this track in seconds, i.e. the last keyframes time value.
		/// </summary>
		float Duration { get; }
		/// <summary>
		/// [GET / SET] The animated value at a certain time. Setting this value may
		/// introduce new keyframes or modify existing ones.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <returns></returns>
		object this[float time] { get; set; }
		
		/// <summary>
		/// Adds a range of keyframes to the track. They do not need to be sorted 
		/// or at the end of the track, as they will be sorted anyway.
		/// </summary>
		/// <param name="frames"></param>
		void AddRange(IEnumerable<IAnimationKeyFrame> frames);
		/// <summary>
		/// Adds a single keyframe to the track. You may use this method to add
		/// keyframes at arbitrary times.
		/// </summary>
		/// <param name="frame"></param>
		void Add(IAnimationKeyFrame frame);
		/// <summary>
		/// Adds a single keyframe to the track. You may use this method to add
		/// keyframes at arbitrary times.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <param name="value"></param>
		void Add<T>(float time, T value);
		/// <summary>
		/// Removes a single keyframe from the track.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		void Remove(float time);
		/// <summary>
		/// Clears the track from all keyframes.
		/// </summary>
		void Clear();
		
		/// <summary>
		/// Returns the animated value at the specified time, converted to Type T.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <returns></returns>
		T GetValue<T>(float time);
		/// <summary>
		/// Assigns a value at the specified time, converted from Type T. This may introduce new keyframes or modify existing ones.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <param name="value"></param>
		void SetValue<T>(float time, T value);
	}
}
