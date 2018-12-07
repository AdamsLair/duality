using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Duality.Editor
{
	/// <summary>
	/// Base class for editor actions that can be performed on a a single object, or iteratively on a set of objects.
	/// It can be regarded as a less verbose implementation shortcut for an <see cref="IEditorAction"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class EditorSingleAction<T> : EditorAction<T>
	{
		/// <summary>
		/// Performs the action on the specified object.
		/// </summary>
		/// <param name="obj"></param>
		public abstract void Perform(T obj);
		/// <summary>
		/// Returns whether the action can be performed on the specified object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public virtual bool CanPerformOn(T obj)
		{
			return true;
		}
		
		/// <summary>
		/// Performs the action on the specified set of objects.
		/// </summary>
		/// <param name="objEnum"></param>
		public override void Perform(IEnumerable<T> objEnum)
		{
			foreach (T o in objEnum)
			{
				this.Perform(o);
			}
		}
		/// <summary>
		/// Returns whether the action can be performed on the specified set of objects.
		/// </summary>
		/// <param name="objEnum"></param>
		public override bool CanPerformOn(IEnumerable<T> objEnum)
		{
			return objEnum.All(o => this.CanPerformOn(o));
		}
	}
}
