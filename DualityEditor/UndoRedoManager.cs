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
	public static class UndoRedoManager
	{
		public enum MacroDeriveName
		{
			UseGeneric,
			FromFirst,
			FromLast
		}

		private static	List<UndoRedoAction>	actionStack			= new List<UndoRedoAction>();
		private static	int						actionIndex			= -1;
		private	static	string					macroName			= null;
		private	static	int						macroBeginCount		= 0;
		private static	List<UndoRedoAction>	macroList			= new List<UndoRedoAction>();
		private	static	int						maxActions			= 50;
		private	static	bool					lastActionFinished	= false;


		public static event EventHandler StackChanged = null;


		public static int MaxUndoActions
		{
			get { return maxActions; }
			set { maxActions = MathF.Max(value, 1); }
		}
		public static bool CanUndo
		{
			get { return PrevAction != null; }
		}
		public static bool CanRedo
		{
			get { return NextAction != null; }
		}
		public static IUndoRedoActionInfo PrevActionInfo
		{
			get { return PrevAction; }
		}
		public static IUndoRedoActionInfo NextActionInfo
		{
			get { return NextAction; }
		}
		private static UndoRedoAction PrevAction
		{
			get { return actionIndex < actionStack.Count && actionIndex >= 0 ? actionStack[actionIndex] : null; }
		}
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


		public static void Clear()
		{
			actionStack.Clear();
			actionIndex = -1;
			lastActionFinished = false;
			OnStackChanged();
		}
		public static void Do(string macroName, params UndoRedoAction[] macro)
		{
			Do(new UndoRedoMacroAction(macroName, macro));
		}
		public static void Do(UndoRedoAction action)
		{
			AppendAction(action, true);
		}
		public static void Finish()
		{
			if (macroBeginCount != 0) return;
			lastActionFinished = true;
		}
		public static void Redo()
		{
			UndoRedoAction action = NextAction;
			if (action == null) return;
			actionIndex++;
			action.Do();
			OnStackChanged();
		}
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

		public static void BeginMacro(string name = null)
		{
			if (macroBeginCount == 0 && name != null) macroName = name;
			macroBeginCount++;
		}
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

	public interface IUndoRedoActionInfo
	{
		string Name { get; }
		HelpInfo Help { get; }
	}

	public abstract class UndoRedoAction : IUndoRedoActionInfo
	{
		protected static readonly CloneProviderContext BackupCloneContext = new CloneProviderContext(false);

		public abstract string Name { get; }
		public virtual bool IsVoid
		{
			get { return false; }
		}
		public virtual HelpInfo Help
		{
			get { return null; }
		}

		public virtual bool CanAppend(UndoRedoAction action)
		{
			return false;
		}
		public virtual void Append(UndoRedoAction action, bool performAction) {}
		public abstract void Do();
		public abstract void Undo();
	}
}
