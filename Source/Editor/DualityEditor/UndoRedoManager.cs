using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Resources;
using Duality.Cloning;

using Duality.Editor.UndoRedoActions;

namespace Duality.Editor
{
	/// <summary>
	/// Manages the editor's global Undo / Redo system.
	/// </summary>
	public static class UndoRedoManager
	{
		/// <summary>
		/// Describes different modes of deriving the grouped name of an Undo / Redo
		/// macro from its individual actions.
		/// </summary>
		public enum MacroDeriveName
		{
			/// <summary>
			/// Use a generic macro name, not describing contents any further.
			/// </summary>
			UseGeneric,
			/// <summary>
			/// The macro will assume the name of its first action item.
			/// </summary>
			FromFirst,
			/// <summary>
			/// The macro will assume the name of its last action item.
			/// </summary>
			FromLast
		}

		private static List<UndoRedoAction> actionStack        = new List<UndoRedoAction>();
		private static int                  actionIndex        = -1;
		private static string               macroName          = null;
		private static int                  macroBeginCount    = 0;
		private static List<UndoRedoAction> macroList          = new List<UndoRedoAction>();
		private static int                  maxActions         = 50;
		private static bool                 lastActionFinished = false;


		/// <summary>
		/// Event that is fired whenever the Undo / Redo stack is modified.
		/// </summary>
		public static event EventHandler StackChanged = null;


		/// <summary>
		/// [GET / SET] The maximum amount of Undo / Redo actions that is stored on the stack
		/// before removing the oldest actions permanently.
		/// </summary>
		public static int MaxUndoActions
		{
			get { return maxActions; }
			set { maxActions = MathF.Max(value, 1); }
		}
		/// <summary>
		/// [GET] Whether it is currently possible to perform an undo step.
		/// </summary>
		public static bool CanUndo
		{
			get { return PrevAction != null; }
		}
		/// <summary>
		/// [GET] Whether it is currently possible to perform a redo step.
		/// </summary>
		public static bool CanRedo
		{
			get { return NextAction != null; }
		}
		/// <summary>
		/// [GET] Provides user information on the previously performed action for Undo.
		/// </summary>
		public static IUndoRedoActionInfo PrevActionInfo
		{
			get { return PrevAction; }
		}
		/// <summary>
		/// [GET] Provides user information on the action that will be performed next on Redo.
		/// </summary>
		public static IUndoRedoActionInfo NextActionInfo
		{
			get { return NextAction; }
		}
		/// <summary>
		/// [GET] The action stack containing all currently logged Undo / Redo actions.
		/// New elements are added at the end while old elements are discarded at the beginning.
		/// </summary>
		public static IReadOnlyList<IUndoRedoActionInfo> ActionStack
		{
			get { return actionStack; }
		}
		/// <summary>
		/// [GET] The <see cref="ActionStack"/> index of the action that was performed last.
		/// </summary>
		public static int ActionIndex
		{
			get { return actionIndex; }
		}
		/// <summary>
		/// [GET] The previously performed action for Undo.
		/// </summary>
		private static UndoRedoAction PrevAction
		{
			get { return actionIndex < actionStack.Count && actionIndex >= 0 ? actionStack[actionIndex] : null; }
		}
		/// <summary>
		/// [GET] The action that will be performed next on Redo.
		/// </summary>
		private static UndoRedoAction NextAction
		{
			get { return actionIndex + 1 < actionStack.Count && actionIndex + 1 >= 0 ? actionStack[actionIndex + 1] : null; }
		}

		
		internal static void Init()
		{
			// Register events
			Scene.Leaving += Scene_Changed;
			Scene.Entered += Scene_Changed;
			Sandbox.StateChanged += Scene_Changed;
		}
		internal static void Terminate()
		{
			// Unregister events
			Scene.Leaving -= Scene_Changed;
			Scene.Entered -= Scene_Changed;
			Sandbox.StateChanged -= Scene_Changed;
		}


		/// <summary>
		/// Clears the entire Undo / Redo stack. This will discard all actions.
		/// </summary>
		public static void Clear()
		{
			actionStack.Clear();
			actionIndex = -1;
			lastActionFinished = false;
			OnStackChanged();
		}
		/// <summary>
		/// Performs a pre-defined macro action and puts it on the Undo / Redo stack
		/// as a single user operation.
		/// </summary>
		/// <param name="macroName"></param>
		/// <param name="macro"></param>
		public static void Do(string macroName, params UndoRedoAction[] macro)
		{
			Do(new UndoRedoMacroAction(macroName, macro));
		}
		/// <summary>
		/// Performs the specified user operation and puts it on the Undo / Redo stack.
		/// </summary>
		/// <param name="action"></param>
		public static void Do(UndoRedoAction action)
		{
			AppendAction(action, true);
		}
		/// <summary>
		/// Indicates that the last user operation that was put on the Undo / Redo stack
		/// is considered finished and thus cannot be merged with future user operations.
		/// </summary>
		public static void Finish()
		{
			if (macroBeginCount != 0) return;
			lastActionFinished = true;
		}
		/// <summary>
		/// Performs a Redo operation, i.e. applies the next action on the stack.
		/// </summary>
		public static void Redo()
		{
			UndoRedoAction action = NextAction;
			if (action == null) return;
			actionIndex++;
			action.Do();
			OnStackChanged();
		}
		/// <summary>
		/// Performs an Undo operation, i.e. reverts the last action on the stack.
		/// </summary>
		public static void Undo()
		{
			UndoRedoAction action = PrevAction;
			if (action == null) return;
			actionIndex--;
			action.Undo();
			OnStackChanged();
		}

		private static void AppendAction(UndoRedoAction action, bool performAction)
		{
			if (action.IsVoid) return;
			if (macroBeginCount > 0)
			{
				UndoRedoAction prev = macroList.Count > 0 ? macroList[macroList.Count - 1] : null;
				if (prev != null && prev.CanAppend(action))
				{
					prev.Append(action, performAction);
				}
				else
				{
					macroList.Add(action);
					if (performAction) action.Do();
				}
			}
			else
			{
				if (Sandbox.IsActive)
				{
					if (performAction) action.Do();
					return;
				}

				bool hadNext = false;
				if (actionStack.Count - actionIndex - 1 > 0)
				{
					actionStack.RemoveRange(actionIndex + 1, actionStack.Count - actionIndex - 1);
					hadNext = true;
				}

				UndoRedoAction prev = PrevAction;
				if (!lastActionFinished && !hadNext && prev != null && prev.CanAppend(action))
				{
					prev.Append(action, performAction);
				}
				else
				{
					lastActionFinished = false;
					actionStack.Add(action);
					actionIndex++;
					if (performAction) action.Do();
				}

				if (actionStack.Count > maxActions)
				{
					actionIndex -= actionStack.Count - maxActions;
					actionStack.RemoveRange(0, actionStack.Count - maxActions);
				}

				OnStackChanged();
			}
		}

		/// <summary>
		/// Begins a macro with the specified name.
		/// </summary>
		/// <param name="name"></param>
		public static void BeginMacro(string name = null)
		{
			if (macroBeginCount == 0 && name != null) macroName = name;
			macroBeginCount++;
		}
		/// <summary>
		/// Ends the currently active macro and merges all actions that have been
		/// performed since into a single Undo / Redo action on the stack.
		/// </summary>
		/// <param name="name"></param>
		public static void EndMacro(string name)
		{
			if (macroBeginCount == 0) throw new InvalidOperationException("Attempting to end a non-existent macro recording");

			macroBeginCount--;
			if (macroBeginCount == 0)
			{
				if (macroList.Count == 1)
					AppendAction(macroList[0], false);
				else
					AppendAction(new UndoRedoMacroAction(name ?? macroName, macroList), false);
				macroList.Clear();
			}
		}
		/// <summary>
		/// Ends the currently active macro and merges all actions that have been
		/// performed since into a single Undo / Redo action on the stack.
		/// </summary>
		/// <param name="name"></param>
		public static void EndMacro(MacroDeriveName name = MacroDeriveName.UseGeneric)
		{
			string nameStr = null;
			if (macroList.Count > 0)
			{
				if (name == MacroDeriveName.FromFirst) nameStr = macroList.First().Name;
				if (name == MacroDeriveName.FromLast) nameStr = macroList.Last().Name;
			}
			EndMacro(nameStr);
		}

		private static void OnStackChanged()
		{
			if (StackChanged != null)
				StackChanged(null, EventArgs.Empty);
		}

		private static void Scene_Changed(object sender, EventArgs e)
		{
			// Maybe reimplement later to only remove Scene-related actions?
			Clear();
		}
	}

	/// <summary>
	/// Provides user information about an Undo / Redo action.
	/// </summary>
	public interface IUndoRedoActionInfo
	{
		/// <summary>
		/// [GET] The name of the user action.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// [GET] An optional help topic dealing with the user action in detail.
		/// </summary>
		HelpInfo Help { get; }
	}

	/// <summary>
	/// Defines and implements an Undo / Redo user action.
	/// </summary>
	public abstract class UndoRedoAction : IUndoRedoActionInfo
	{
		/// <summary>
		/// A cloning context that doesn't preserve (i.e. clones and overwrites) identity-relevant 
		/// object fields because it is intended to be used for creating object backups.
		/// </summary>
		protected static readonly CloneProviderContext BackupCloneContext = new CloneProviderContext(false);
		
		/// <summary>
		/// [GET] The name of the user action.
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// [GET] Whether or not this user action should be considered devoid of content.
		/// If this action does nothing, this property should return true.
		/// </summary>
		public virtual bool IsVoid
		{
			get { return false; }
		}
		/// <summary>
		/// [GET] An optional help topic dealing with the user action in detail.
		/// </summary>
		public virtual HelpInfo Help
		{
			get { return null; }
		}

		/// <summary>
		/// Determines whether the specified action could be merged with this one.
		/// </summary>
		/// <param name="action"></param>
		public virtual bool CanAppend(UndoRedoAction action)
		{
			return false;
		}
		/// <summary>
		/// Merges the specified action with this one and performs it before doing so, if requested.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="performAction"></param>
		public virtual void Append(UndoRedoAction action, bool performAction) {}
		/// <summary>
		/// Applies the intended changes of this user action.
		/// </summary>
		public abstract void Do();
		/// <summary>
		/// Reverts the previously applied changes of this user action.
		/// </summary>
		public abstract void Undo();
	}
}
