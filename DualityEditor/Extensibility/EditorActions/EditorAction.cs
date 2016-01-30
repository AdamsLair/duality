using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Duality.Editor
{
	public abstract class EditorAction<T> : IEditorAction
	{
		public virtual string Name
		{
			get { return null; }
		}
		public virtual string Description
		{
			get { return null; }
		}
		public virtual Image Icon
		{
			get { return null; }
		}
		public Type SubjectType
		{
			get { return typeof(T); }
		}
		public virtual int Priority
		{
			get { return 0; }
		}

		public void Perform(T obj)
		{
			this.Perform(new[] { obj });
		}
		public abstract void Perform(IEnumerable<T> objEnum);
		public virtual bool CanPerformOn(IEnumerable<T> objEnum)
		{
			return true;
		}
		public virtual bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextMenu;
		}

		void IEditorAction.Perform(object obj)
		{
			this.Perform((T)obj);
		}
		void IEditorAction.Perform(IEnumerable<object> obj)
		{
			this.Perform(obj.Cast<T>());
		}
		bool IEditorAction.CanPerformOn(IEnumerable<object> obj)
		{
			if (obj == null) return true;
			return this.CanPerformOn(obj.Cast<T>());
		}
	}
}
