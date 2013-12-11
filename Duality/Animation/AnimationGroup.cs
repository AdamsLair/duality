using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	/// <summary>
	/// Represents a synchronized group of <see cref="IAnimation">running animations</see>.
	/// </summary>
	public class AnimationGroup : IEnumerable<IAnimation>
	{
		private	List<IAnimation>	animations	= new List<IAnimation>();
		private	float				time		= 0.0f;

		/// <summary>
		/// [GET / SET] The collection of animations that are grouped here.
		/// </summary>
		public IEnumerable<IAnimation> Animations
		{
			get { return this.animations; }
			set { this.animations = (value != null) ? value.ToList() : new List<IAnimation>(); }
		}
		/// <summary>
		/// [GET / SET] The mutual time value of all grouped animations. Setting this will update
		/// all animated objects according to their new values.
		/// </summary>
		public float Time
		{
			get { return this.time; }
			set { this.time = value; this.UpdateAnimatedValue(); }
		}
		/// <summary>
		/// [GET] The total duration of this group of animations. Equals the <see cref="IAnimation.Duration"/> of the longest
		/// animation within the group.
		/// </summary>
		public float Duration
		{
			get { return this.animations.Max(anim => anim.Track.Duration); }
		}
		
		/// <summary>
		/// Creates an empty group of animations.
		/// </summary>
		public AnimationGroup() {}
		/// <summary>
		/// Creates a new group of animations based on the specified enumerable.
		/// </summary>
		/// <param name="animations"></param>
		public AnimationGroup(IEnumerable<IAnimation> animations)
		{
			this.AddRange(animations);
		}

		/// <summary>
		/// Adds a range of animations to this group.
		/// </summary>
		/// <param name="animations"></param>
		public void AddRange(IEnumerable<IAnimation> animations)
		{
			animations = animations.Where(anim => !this.animations.Contains(anim));
			this.animations.AddRange(animations);
		}
		/// <summary>
		/// Adds a single animation to this group.
		/// </summary>
		/// <param name="animation"></param>
		public void Add(IAnimation animation)
		{
			if (this.animations.Contains(animation)) return;
			this.animations.Add(animation);
		}
		/// <summary>
		/// Removes a single animation from this group.
		/// </summary>
		/// <param name="animation"></param>
		public void Remove(IAnimation animation)
		{
			this.animations.Remove(animation);
		}
		/// <summary>
		/// Removes all animations from this group at once.
		/// </summary>
		public void Clear()
		{
			this.animations.Clear();
		}

		/// <summary>
		/// Updates all animations and animated objects within this group.
		/// </summary>
		public void UpdateAnimatedValue()
		{
			foreach (IAnimation anim in this.animations)
			{
				anim.Time = this.time;
			}
		}

		IEnumerator<IAnimation> IEnumerable<IAnimation>.GetEnumerator()
		{
			return this.animations.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.animations.GetEnumerator();
		}
	}
}
