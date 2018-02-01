using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Duality.Resources;

namespace Duality.Editor.UndoRedoActions
{
	public class ResizeListAction : UndoRedoAction
	{
		private readonly IList<IList> targetLists;
		private readonly int[] originalListSizes;
		private readonly int targetSize;
		private readonly Type elementType;

		// TODO: Look this up in resources. See other UndoRedoActions for example
		public override string Name
		{
			get { return "Resize"; }
		}

		public ResizeListAction(IList<IList> targetLists, int size, Type elementType)
		{
			this.targetLists = targetLists;
			this.targetSize = size;
			this.elementType = elementType;

			this.originalListSizes = new int[this.targetLists.Count];
			for (int i = 0; i < this.targetLists.Count; i++)
			{
				IList target = this.targetLists[i];
				if (target == null) continue;
				this.originalListSizes[i] = target.Count;
			}
		}

		public override void Do()
		{
			bool writeback = false;

			for (int i = 0; i < this.targetLists.Count; i++)
			{
				IList target = this.targetLists[i];
				if (target == null) continue;
				bool needsWriteback = EnsureIListSize(ref target, this.targetSize, this.elementType);
				if (needsWriteback)
				{
					this.targetLists[i] = target;
					writeback = true;
				}
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));

			if (writeback)
			{
				// If writeback is supported in the future, implement here
			}
		}

		public override void Undo()
		{
			bool writeback = false;

			for (int i = 0; i < this.targetLists.Count; i++)
			{
				IList target = this.targetLists[i];
				if (target == null) continue;
				bool needsWriteback = EnsureIListSize(ref target, this.originalListSizes[i], this.elementType);
				if (needsWriteback)
				{
					this.targetLists[i] = target;
					writeback = true;
				}
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));

			if (writeback)
			{
				// If writeback is supported in the future, implement here
			}
		}

		private static bool EnsureIListSize(ref IList list, int size, Type elementType)
		{
			bool writeback = false;

			if (!list.IsFixedSize && !list.IsReadOnly)
			{
				while (list.Count < size)
				{
					object added = elementType.IsValueType ? elementType.GetTypeInfo().CreateInstanceOf() : null;
					list.Add(added);
				}
				while (list.Count > size)
				{
					list.RemoveAt(list.Count - 1);
				}
			}
			else if (list is Array)
			{
				Array newTarget = Array.CreateInstance(elementType, size);
				for (int j = 0; j < Math.Min(size, list.Count); j++) newTarget.SetValue(list[j], j);
				list = newTarget;
				writeback = true;
			}
			else
			{
				// a read only container we can't do anything about
			}

			return writeback;
		}
	}
}
