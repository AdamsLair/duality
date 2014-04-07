using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor.Properties;
using Duality.Editor.Controls;

using AdamsLair.WinForms.PropertyEditing;

namespace Duality.Editor.UndoRedoActions
{
	public class EditFieldAction : UndoRedoAction
	{
		protected	PropertyGrid	parent	= null;
		protected	bool			firstDo			= true;
		protected	FieldInfo		targetField		= null;
		protected	object[]		targetObj		= null;
		protected	object[]		targetValue		= null;
		protected	object[]		backupValue		= null;
		
		public override string Name
		{
			get { return string.Format(GeneralRes.UndoRedo_EditProperty, this.targetField.Name); }
		}
		public override bool IsVoid
		{
			get { return this.targetObj == null || this.targetObj.Length == 0; }
		}

		public EditFieldAction(PropertyGrid parent, FieldInfo field, IEnumerable<object> target, IEnumerable<object> value)
		{
			if (field == null) throw new ArgumentNullException("field");
			if (target == null) throw new ArgumentNullException("target");
			this.targetField = field;
			this.targetObj = target.Where(o => o != null).ToArray();
			this.targetValue = value != null ? value.ToArray() : null;
			this.parent = parent;
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			EditFieldAction castAction = action as EditFieldAction;

			if (castAction == null) return false;
			if (castAction.targetField != this.targetField) return false;
			if (!castAction.targetObj.SequenceEqual(this.targetObj)) return false;

			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			EditFieldAction castAction = action as EditFieldAction;

			if (performAction)
			{
				castAction.backupValue = this.backupValue;
				castAction.Do();
			}
			this.targetValue = castAction.targetValue ?? this.targetValue;
		}
		public override void Do()
		{
			if (this.targetValue != null && this.targetValue.Length > 0)
			{
				if (this.backupValue == null)
				{
					this.backupValue = new object[this.targetObj.Length];
					for (int i = 0; i < this.targetObj.Length; i++)
						this.backupValue[i] = this.targetField.GetValue(this.targetObj[i]);
				}

				for (int i = 0; i < this.targetObj.Length; i++)
					this.targetField.SetValue(this.targetObj[i], this.targetValue[Math.Min(i, this.targetValue.Length - 1)]);
			}
			
			// This is a bad workaround for having a purely Property-based change event system: Simply notify "something" changed.
			// Change to something better in the future.
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj));
			if (!this.firstDo && this.parent != null)
				this.parent.UpdateFromObjects();
			this.firstDo = false;
		}
		public override void Undo()
		{
			if (this.targetValue != null)
			{
				if (this.backupValue == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");

				for (int i = 0; i < this.targetObj.Length; i++)
					this.targetField.SetValue(this.targetObj[i], this.backupValue[i]);
			}
			
			// This is a bad workaround for having a purely Property-based change event system: Simply notify "something" changed.
			// Change to something better in the future.
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj));
			if (this.parent != null) this.parent.UpdateFromObjects();
		}
	}
}
