using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	/// <summary>
	/// The keyframe of an <see cref="IAnimationTrack"/>, which is essentially a key/value pair mapped from time to value.
	/// </summary>
	public interface IAnimationKeyFrame
	{
		/// <summary>
		/// [GET / SET] Time in seconds, at which this keyframe is set.
		/// </summary>
		float Time { get; set; }
		/// <summary>
		/// [GET / SET] Value of the animated entity at the specified time.
		/// </summary>
		object Value { get; set; }

		/// <summary>
		/// Returns the <see cref="Value"/>, converted to a value of Type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetValue<T>();
		/// <summary>
		/// Sets the <see cref="Value"/>, converted from a value of Type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		void SetValue<T>(T value);
	}
}
