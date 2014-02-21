using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	/// <summary>
	/// Represents an animation that is being processed on a certain object.
	/// </summary>
	/// <typeparam name="TObject">Type of the animated object.</typeparam>
	/// <typeparam name="TValue">Type of the animated value.</typeparam>
	public class Animation<TObject,TValue> : IAnimation where TObject : class
	{
		private	TObject					target		= null;
		private	AnimationTrack<TValue>	track		= null;
		private	Action<TObject,TValue>	setter		= null;
		private	float					time		= 0.0f;

		/// <summary>
		/// [GET] The target object this animation runs on.
		/// </summary>
		public TObject Target
		{
			get { return this.target; }
		}
		/// <summary>
		/// [GET] The animation data that is used by this animation as a source of values.
		/// </summary>
		public AnimationTrack<TValue> Track
		{
			get { return this.track; }
		}
		/// <summary>
		/// [GET / SET] The current time value of this animation in seconds. Setting this will update the object.
		/// </summary>
		public float Time
		{
			get { return this.time; }
			set { this.time = value; this.UpdateAnimatedValue(); }
		}
		/// <summary>
		/// [GET] The total duration of this animation in seconds.
		/// </summary>
		public float Duration
		{
			get { return this.track.Duration; }
		}
		/// <summary>
		/// [GET] The current animated value, based on the animations current <see cref="Time"/>.
		/// </summary>
		public TValue CurrentValue
		{
			get { return this.track.GetValue(this.time); }
		}


		/// <summary>
		/// Creates a new animation.
		/// </summary>
		/// <param name="target">The animated object.</param>
		/// <param name="setter">The setter that is invoked when updating the animated value.</param>
		/// <param name="track">The animation track that is used as a data source.</param>
		public Animation(TObject target, Action<TObject,TValue> setter, AnimationTrack<TValue> track)
		{
			if (setter == null)	throw new ArgumentNullException("setter");
			if (track == null)	throw new ArgumentNullException("track");

			this.target = target;
			this.setter = setter;
			this.track = track;
		}

		/// <summary>
		/// Updates the <see cref="Target"/> object based on the animations current time and value.
		/// </summary>
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
