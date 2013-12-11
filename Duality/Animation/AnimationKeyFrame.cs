using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	/// <summary>
	/// The keyframe of an <see cref="AnimationTrack{T}"/>, which is essentially a key/value pair mapped from time to value.
	/// </summary>
	/// <typeparam name="T">The Type of the animated value.</typeparam>
	public struct AnimationKeyFrame<T> : IComparable<AnimationKeyFrame<T>>, IAnimationKeyFrame
	{
		/// <summary>
		/// Time in seconds, at which this keyframe is set.
		/// </summary>
		public float Time;
		/// <summary>
		/// Value of the animated entity at the specified time.
		/// </summary>
		public T Value;


		/// <summary>
		/// Creates a new keyframe from time and value.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <param name="value"></param>
		public AnimationKeyFrame(float time, T value)
		{
			this.Time = time;
			this.Value = value;
		}


		public override string ToString()
		{
			return string.Format("[{0:F}: {1:F}]", this.Time, this.Value);
		}
		int IComparable<AnimationKeyFrame<T>>.CompareTo(AnimationKeyFrame<T> other)
		{
			return this.Time.CompareTo(other.Time);
		}
		float IAnimationKeyFrame.Time
		{
			get { return this.Time; }
			set { this.Time = value; }
		}
		object IAnimationKeyFrame.Value
		{
			get { return this.Value; }
			set { this.Value = (T)value; }
		}
		U IAnimationKeyFrame.GetValue<U>()
		{
			return GenericOperator.Convert<T,U>(this.Value);
		}
		void IAnimationKeyFrame.SetValue<U>(U value)
		{
			this.Value = GenericOperator.Convert<U,T>(value);
		}
	}
}
