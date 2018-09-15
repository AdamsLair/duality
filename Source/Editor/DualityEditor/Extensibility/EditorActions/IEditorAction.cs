using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Duality.Editor
{
	/// <summary>
	/// The public interface for editor actions. If a public type implements this
	/// interface, it will be available as an action throughout the editor.
	/// </summary>
	/// <seealso cref="EditorAction{T}"/>
	/// <seealso cref="EditorSingleAction{T}"/>
	public interface IEditorAction
	{
		/// <summary>
		/// [GET] The user-friendly name of this action that will be displayed in the user interface.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// [GET] The icon of this action when displayed to the user.
		/// </summary>
		Image Icon { get; }
		/// <summary>
		/// [GET] An optional hint about this action that is displayed in the user interface.
		/// </summary>
		HelpInfo HelpInfo { get; }
		/// <summary>
		/// [GET] The type of object that this editor action deals with, i.e. the kind of
		/// object that the action is able to operate on.
		/// </summary>
		Type SubjectType { get; }
		/// <summary>
		/// [GET] The priority of this action. Controls display order
		/// when multiple relevant actions are displayed together.
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// Performs this action on the given sequence of objects.
		/// </summary>
		void Perform(IEnumerable<object> obj);
		/// <summary>
		/// Returns whether the action can be performed on the specified set of objects.
		/// </summary>
		/// <param name="objEnum"></param>
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
