using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Duality.Editor
{
	/// <summary>
	/// Base class for editor actions that can be performed on a set of objects.
	/// It can be regarded as a less verbose implementation shortcut for an <see cref="IEditorAction"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class EditorAction<T> : IEditorAction
	{
		private HelpInfo info;

		/// <summary>
		/// [GET] The user-friendly name of this action that will be displayed in the user interface.
		/// </summary>
		public virtual string Name
		{
			get { return null; }
		}
		/// <summary>
		/// [GET] The icon of this action when displayed to the user.
		/// </summary>
		public virtual Image Icon
		{
			get { return null; }
		}
		/// <summary>
		/// [GET] An optional hint about this action that is displayed in the user interface.
		/// </summary>
		public virtual HelpInfo HelpInfo
		{
			get
			{
				if (this.info == null)
				{
					Type type = this.GetType();
					string name = this.Name ?? type.GetTypeCSCodeName(true);
					if (HelpSystem.GetXmlCodeDoc(type) != null)
					{
						this.info = HelpInfo.FromMember(type);
						this.info.Topic = name;
					}
					else
					{
						this.info = HelpInfo.CreateNotAvailable(name);
					}
				}
				return this.info;
			}
		}
		/// <summary>
		/// [GET] The type of object that this editor action deals with, i.e. the kind of
		/// object that the action is able to operate on.
		/// </summary>
		public Type SubjectType
		{
			get { return typeof(T); }
		}
		/// <summary>
		/// [GET] When multiple actions are available for the same object or set of objects,
		/// the one with the highest priority will be used.
		/// </summary>
		public virtual int Priority
		{
			get { return 0; }
		}

		/// <summary>
		/// Performs the action on the specified object.
		/// </summary>
		/// <param name="obj"></param>
		public void Perform(T obj)
		{
			this.Perform(new[] { obj });
		}
		/// <summary>
		/// Performs the action on the specified set of objects.
		/// </summary>
		/// <param name="objEnum"></param>
		public abstract void Perform(IEnumerable<T> objEnum);
		/// <summary>
		/// Returns whether the action can be performed on the specified set of objects.
		/// </summary>
		/// <param name="objEnum"></param>
		public virtual bool CanPerformOn(IEnumerable<T> objEnum)
		{
			return true;
		}
		/// <summary>
		/// Returns whether the action matches the specified context.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
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
