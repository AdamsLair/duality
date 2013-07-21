using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace DualityEditor.CorePluginInterface
{
	public interface IEditorAction
	{
		string Name { get; }
		string Description { get; }
		Image Icon { get; }

		void Perform(object obj);
		void Perform(IEnumerable<object> obj);
		bool CanPerformOn(IEnumerable<object> obj);
	}
	public abstract class EditorActionBase<T> : IEditorAction
	{
		private	string		name;
		private	string		desc;
		private	Image		icon;

		public string Name
		{
			get { return this.name; }
		}
		public string Description
		{
			get { return this.desc; }
		}
		public Image Icon
		{
			get { return this.icon; }
		}

		protected EditorActionBase(string name, Image icon, string desc)
		{
			this.name = name;
			this.icon = icon;
			this.desc = desc;
		}

		public void Perform(T obj)
		{
			this.Perform(new[] { obj });
		}
		public abstract void Perform(IEnumerable<T> objEnum);
		public abstract bool CanPerformOn(IEnumerable<T> objEnum);

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
	public class EditorAction<T> : EditorActionBase<T>
	{
		private	Action<T>		action;
		private	Predicate<T>	actionPredicate;

		public EditorAction(string name, Image icon, Action<T> action, string desc = null, Predicate<T> predicate = null) : base(name, icon, desc)
		{
			this.action = action;
			this.actionPredicate = predicate;
		}

		public override void Perform(IEnumerable<T> objEnum)
		{
			foreach (T obj in objEnum)
			{
				if (this.actionPredicate != null && !this.actionPredicate(obj)) continue;
				this.action(obj);
			}
		}
		public override bool CanPerformOn(IEnumerable<T> objEnum)
		{
			if (objEnum == null) return true;
			if (this.actionPredicate == null) return true;
			return objEnum.Any(o => this.actionPredicate(o));
		}
	}
	public class EditorGroupAction<T> : EditorActionBase<T>
	{
		private	Action<IEnumerable<T>>		action;
		private	Predicate<IEnumerable<T>>	actionPredicate;

		public EditorGroupAction(string name, Image icon, Action<IEnumerable<T>> action, string desc = null, Predicate<IEnumerable<T>> predicate = null) : base(name, icon, desc)
		{
			this.action = action;
			this.actionPredicate = predicate;
		}

		public override void Perform(IEnumerable<T> objEnum)
		{
			this.action(objEnum);
		}
		public override bool CanPerformOn(IEnumerable<T> objEnum)
		{
			if (objEnum == null) return true;
			if (this.actionPredicate == null) return true;
			return this.actionPredicate(objEnum);
		}
	}
}
