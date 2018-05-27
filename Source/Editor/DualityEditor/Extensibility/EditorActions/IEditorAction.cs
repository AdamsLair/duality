using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Duality.Editor
{
	/// <summary>
	/// A generic action that can be executed in the editor 
	/// (context menus, opening resources, etc.)
	/// </summary>
	/// <seealso cref="EditorAction{T}"/>
	/// <seealso cref="EditorSingleAction{T}"/>
	public interface IEditorAction
	{
		/// <summary>
		/// [GET] The display friendly name of this action.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// [GET] The display friendly description of  this action.
		/// </summary>
		string Description { get; }
		/// <summary>
		/// [GET] The <see cref="Image"/> to use as an icon for this action.
		/// </summary>
		Image Icon { get; }
		/// <summary>
		/// [GET] The type of object that this action can be performed on.
		/// </summary>
		Type SubjectType { get; }
		/// <summary>
		/// [GET] The priority of this action. Controls display order
		/// when multiple relevant actions are displayed together.
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// Performs this action on the given object.
		/// </summary>
		void Perform(object obj);
		/// <summary>
		/// Performs this action on the given sequence of objects.
		/// </summary>
		void Perform(IEnumerable<object> obj);
		/// <summary>
		/// Determines whether or not this action can be applied 
		/// to the given sequence of objects. Actions returning false
		/// from this method will not be presented to the user.
		/// </summary>
		bool CanPerformOn(IEnumerable<object> obj);
		/// <summary>
		/// Returns whether or not this action matches the 
		/// given action context. Actions returning false
		/// from this method will not be presented to the user.
		/// </summary>
		/// <param name="context">
		/// A custom string coming from a plugin 
		/// or one of the constant Action strings defined 
		/// in <see cref="DualityEditorApp"/>
		/// </param>
		bool MatchesContext(string context);
	}
}
