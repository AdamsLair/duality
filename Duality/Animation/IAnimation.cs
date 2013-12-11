using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	/// <summary>
	/// Represents an animation that is being processed on a certain object.
	/// </summary>
	public interface IAnimation
	{
		/// <summary>
		/// [GET] The target object this animation runs on.
		/// </summary>
		object Target { get; }
		/// <summary>
		/// [GET] The animation data that is used by this animation as a source of values.
		/// </summary>
		IAnimationTrack Track { get; }
		/// <summary>
		/// [GET / SET] The current time value of this animation in seconds. Setting this will update the object.
		/// </summary>
		float Time { get; set; }
		/// <summary>
		/// [GET] The total duration of this animation in seconds.
		/// </summary>
		float Duration { get; }
		/// <summary>
		/// [GET] The current animated value, based on the animations current <see cref="Time"/>.
		/// </summary>
		object CurrentValue { get; }
		
		/// <summary>
		/// Returns the <see cref="CurrentValue"/>, converted to a value of Type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
        T GetCurrentValue<T>();
		/// <summary>
		/// Updates the <see cref="Target"/> object based on the animations current time and value.
		/// </summary>
		void UpdateAnimatedValue();
	}
}
