using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	public struct AnimationKeyFrame<T> : IComparable<AnimationKeyFrame<T>>, IAnimationKeyFrame
	{
		public float Time;
		public T Value;

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
