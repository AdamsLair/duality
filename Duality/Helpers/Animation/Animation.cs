using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	public class Animation<TObject,TValue> : IAnimation where TObject : class
	{
		private	TObject					target		= null;
		private	AnimationTrack<TValue>	track		= null;
		private	Action<TObject,TValue>	setter		= null;
		private	float					time		= 0.0f;

		public TObject Target
		{
			get { return this.target; }
		}
		public AnimationTrack<TValue> Track
		{
			get { return this.track; }
		}
		public float Time
		{
			get { return this.time; }
			set { this.time = value; this.UpdateAnimatedValue(); }
		}
		public TValue CurrentValue
		{
			get { return this.track.GetValue(this.time); }
		}

		public Animation(TObject target, Action<TObject,TValue> setter, AnimationTrack<TValue> track)
		{
			if (setter == null)	throw new ArgumentNullException("setter");
			if (track == null)	throw new ArgumentNullException("track");

			this.target = target;
			this.setter = setter;
			this.track = track;
		}

		public void UpdateAnimatedValue()
		{
			this.setter(this.target, this.track.GetValue(this.time));
		}

		object IAnimation.Target
		{
			get { return this.target; }
		}
		IAnimationTrack IAnimation.Track
		{
			get { return this.track; }
		}
		object IAnimation.CurrentValue
		{
			get { return this.CurrentValue; }
		}
		T IAnimation.GetCurrentValue<T>()
		{
			return GenericOperator.Convert<TValue,T>(this.CurrentValue);
		}
	}
}
