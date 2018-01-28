using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdamsLair.WinForms.PropertyEditing;

namespace Duality.Editor.UndoRedoActions
{
	public class ResizeListAction : UndoRedoAction
	{
		private PropertyEditor editor;
		private IList<IList> targetLists;
		private IList<IList> originalLists;
		private int targetSize;
		private Type elementType;

		// TODO: Look this up in resources. See other UndoRedoActions for example
		public override string Name
		{
			get { return "Resize"; }
		}

		public ResizeListAction(PropertyEditor editor, IList<IList> targetLists, int size, Type elementType)
		{
			this.editor = editor;
			this.targetLists = targetLists;
			this.targetSize = size;
			this.elementType = elementType;
		}

		public override void Do()
		{
			bool writeback = false;
			
			for (int i = 0; i < this.targetLists.Count; i++)
			{
				IList target = this.targetLists[i];
				if (target == null) continue;
				if (!target.IsFixedSize && !target.IsReadOnly)
				{
					while (target.Count < this.targetSize)
					{
						object added = this.elementType.IsValueType ? this.elementType.GetTypeInfo().CreateInstanceOf() : null;
						target.Add(added);
					}
					while (target.Count > this.targetSize)
						target.RemoveAt(target.Count - 1);
				}
				else if (target is Array)
				{
					Array newTarget = Array.CreateInstance(this.elementType, this.targetSize);
					for (int j = 0; j < Math.Min(this.targetSize, target.Count); j++) newTarget.SetValue(target[j], j);
					this.targetLists[i] = newTarget;
					writeback = true;
				}
				else
				{
					// a read only container we can't do anything about
				}
			}

			if (writeback)
			{
				PropertyInfo property = this.editor.EditedMember as PropertyInfo;
				if (property != null)
				{
					//foreach (object obj in (IEnumerable<object>)this.editor.DisplayedValue)
						//Log.Editor.Write(obj.ToString());
					// Successful when editing a single object, but when multiple objects are selected,
					// DisplayedValue is only a single object and this fails to update all selected objects
					object targetObject = this.editor.ParentEditor.DisplayedValue;
					property.SetValue(targetObject, this.targetLists[0]);
				}
			}
			//return writeback;
		}

		public override void Undo()
		{
			Log.Editor.Write("Test");
			throw new NotImplementedException();
		}
	}
}
