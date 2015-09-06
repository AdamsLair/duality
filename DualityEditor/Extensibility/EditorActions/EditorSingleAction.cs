using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Duality.Editor
{
	public abstract class EditorSingleAction<T> : EditorAction<T>
	{
		public abstract new void Perform(T obj);
		public virtual bool CanPerformOn(T obj)
		{
			return true;
		}

		public override void Perform(IEnumerable<T> objEnum)
		{
			foreach (T o in objEnum)
			{
				this.Perform(o);
			}
		}
		public override bool CanPerformOn(IEnumerable<T> objEnum)
		{
			return objEnum.All(o => this.CanPerformOn(o));
		}
	}
}
