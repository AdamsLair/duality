using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor.Properties;

namespace Duality.Editor.UndoRedoActions
{
	public class UndoRedoMacroAction : UndoRedoAction
	{
		private	UndoRedoAction[]	macro	= null;
		private	string				name	= null;

		public override string Name
		{
			get { return this.name; }
		}
		public override bool IsVoid
		{
			get { return this.macro == null || this.macro.Length == 0; }
		}

		public UndoRedoMacroAction(string name, IEnumerable<UndoRedoAction> macro)
		{
			if (macro == null) throw new ArgumentNullException("macro");
			this.macro = macro.Where(o => o != null && !o.IsVoid).ToArray();

			if (this.macro.Length == 1)
				this.name = this.macro[0].Name;
			else
				this.name = name ?? string.Format(GeneralRes.UndoRedo_Macro, this.macro.Length);
		}
		public UndoRedoMacroAction(string name, params UndoRedoAction[] macro) : this(name, macro as IEnumerable<UndoRedoAction>) {}

		public override bool CanAppend(UndoRedoAction action)
		{
			UndoRedoMacroAction castAction = action as UndoRedoMacroAction;

			if (castAction == null) return false;
			if (castAction.macro.Length != this.macro.Length) return false;
			for (int i = 0; i < this.macro.Length; i++)
			{
				if (!this.macro[i].CanAppend(castAction.macro[i])) return false;
			}

			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			UndoRedoMacroAction castAction = action as UndoRedoMacroAction;
			
			for (int i = 0; i < this.macro.Length; i++)
			{
				this.macro[i].Append(castAction.macro[i], performAction);
			}
		}
		public override void Do()
		{
			foreach (UndoRedoAction action in this.macro)
				action.Do();
		}
		public override void Undo()
		{
			foreach (UndoRedoAction action in this.macro.Reverse())
				action.Undo();
		}
	}
}
