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
		/// [GET] When multiple actions are available for the same object or set of objects,
		/// the one with the highest priority will be used.
		/// </summary>
		int Priority { get; }
		
		/// <summary>
		/// Performs the action on the specified object.
		/// </summary>
		/// <param name="obj"></param>
		void Perform(object obj);
		/// <summary>
		/// Performs the action on the specified set of objects.
		/// </summary>
		/// <param name="objEnum"></param>
		void Perform(IEnumerable<object> obj);
		/// <summary>
		/// Returns whether the action can be performed on the specified set of objects.
		/// </summary>
		/// <param name="objEnum"></param>
		bool CanPerformOn(IEnumerable<object> obj);
		/// <summary>
		/// Returns whether the action matches the specified context.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		bool MatchesContext(string context);
	}
}
