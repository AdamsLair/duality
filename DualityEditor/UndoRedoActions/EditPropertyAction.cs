using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor.Controls;
using Duality.Editor.Properties;

using AdamsLair.WinForms.PropertyEditing;

namespace Duality.Editor.UndoRedoActions
{
	public class EditPropertyAction : UndoRedoAction
	{
		protected	PropertyGrid	parent	= null;
		protected	bool			firstDo			= true;
		protected	PropertyInfo	targetProperty	= null;
		protected	object[]		targetObj		= null;
		protected	object[]		targetValue		= null;
		protected	object[]		backupValue		= null;
		protected	object[]		targetIndices	= null;
		
		public override string Name
		{
			get { return string.Format(GeneralRes.UndoRedo_EditProperty, this.targetProperty.Name); }
		}
		public override bool IsVoid
		{
			get { return this.targetObj == null || this.targetObj.Length == 0; }
		}

		public EditPropertyAction(PropertyGrid parent, PropertyInfo property, IEnumerable<object> target, IEnumerable<object> value, object[] indices = null)
		{
			if (property == null) throw new ArgumentNullException("property");
			if (target == null) throw new ArgumentNullException("target");
			this.targetProperty = property;
			this.targetObj = target.Where(o => o != null).ToArray();
			this.targetValue = value != null ? value.ToArray() : null;
			this.targetIndices = indices;
			this.parent = parent;
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			EditPropertyAction castAction = action as EditPropertyAction;

			if (castAction == null) return false;
			if (castAction.targetProperty != this.targetProperty) return false;
			if (!castAction.targetObj.SequenceEqual(this.targetObj)) return false;

			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			EditPropertyAction castAction = action as EditPropertyAction;

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
						this.backupValue[i] = this.targetProperty.GetValue(this.targetObj[i], this.targetIndices);
				}

				for (int i = 0; i < this.targetObj.Length; i++)
					this.targetProperty.SetValue(this.targetObj[i], this.targetValue[Math.Min(i, this.targetValue.Length - 1)], this.targetIndices);
			}

			DualityEditorApp.NotifyObjPropChanged(this.parent, new ObjectSelection(this.targetObj), this.targetProperty);
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
					this.targetProperty.SetValue(this.targetObj[i], this.backupValue[i], this.targetIndices);
			}

			DualityEditorApp.NotifyObjPropChanged(this.parent, new ObjectSelection(this.targetObj), this.targetProperty);
			if (this.parent != null) this.parent.UpdateFromObjects();
		}
	}
}
