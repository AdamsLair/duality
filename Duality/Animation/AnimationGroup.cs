using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Duality.Animation
{
	public class AnimationGroup : IEnumerable<IAnimation>
	{
		private	List<IAnimation>	children	= new List<IAnimation>();
		private	float				time		= 0.0f;

		public IEnumerable<IAnimation> Children
		{
			get { return this.children; }
			set { this.children = (value != null) ? value.ToList() : new List<IAnimation>(); }
		}
		public float Time
		{
			get { return this.time; }
			set { this.time = value; this.UpdateAnimatedValue(); }
		}
		
		public AnimationGroup() {}
		public AnimationGroup(IEnumerable<IAnimation> animations)
		{
			this.AddRange(animations);
		}

		public void AddRange(IEnumerable<IAnimation> animations)
		{
			this.children.AddRange(animations);
		}
		public void Add(IAnimation animation)
		{
			this.children.Add(animation);
		}
		public void Remove(IAnimation animation)
		{
			this.children.Remove(animation);
		}
		public void Clear()
		{
			this.children.Clear();
		}

		public void UpdateAnimatedValue()
		{
			foreach (IAnimation anim in this.children)
			{
				anim.Time = this.time;
			}
		}

		IEnumerator<IAnimation> IEnumerable<IAnimation>.GetEnumerator()
		{
			return this.children.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.children.GetEnumerator();
		}
	}
}
