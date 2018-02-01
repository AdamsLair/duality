using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Duality.Resources;

namespace Duality.Editor.UndoRedoActions
{
	public class RemoveDictionaryKeyAction : UndoRedoAction
	{
		private readonly IDictionary[] targetDictionaries;
		private readonly object[] removedValues;
		private readonly object key;

		// TODO: fetch from resources
		public override string Name
		{
			get { return "Remove key"; }
		}

		public RemoveDictionaryKeyAction(IEnumerable<IDictionary> targetDictionaries, object key)
		{
			this.targetDictionaries = targetDictionaries.Where(d => d != null).ToArray();
			this.key = key;
			this.removedValues = new object[this.targetDictionaries.Length];
		}

		public override void Do()
		{
			for (int i = 0; i < this.targetDictionaries.Length; i++)
			{
				IDictionary target = this.targetDictionaries[i];
				if (!target.IsFixedSize && !target.IsReadOnly)
				{
					if (target.Contains(this.key))
					{
						this.removedValues[i] = target[this.key];
						target.Remove(this.key);
					}
				}
				else
				{
					// a read only container we can't do anything about
				}
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}

		public override void Undo()
		{
			for (int i = 0; i < this.targetDictionaries.Length; i++)
			{
				IDictionary target = this.targetDictionaries[i];
				if (!target.IsFixedSize && !target.IsReadOnly)
				{
					if (!target.Contains(this.key))
					{
						target.Add(this.key, this.removedValues[i]);
					}
				}
				else
				{
					// a read only container we can't do anything about
				}
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
	}
}
