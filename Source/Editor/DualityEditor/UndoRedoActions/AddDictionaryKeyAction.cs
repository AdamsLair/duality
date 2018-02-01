using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Duality.Resources;

namespace Duality.Editor.UndoRedoActions
{
	public class AddDictionaryKeyAction : UndoRedoAction
	{
		private readonly IDictionary[] targetDictionaries;
		private readonly object key;
		private readonly Type valueType;

		// TODO: fetch from resources
		public override string Name
		{
			get { return "Add key"; }
		}

		public AddDictionaryKeyAction(IEnumerable<IDictionary> targetDictionaries, object key, Type valueType)
		{
			this.targetDictionaries = targetDictionaries.Where(d => d != null).ToArray();
			this.key = key;
			this.valueType = valueType;
		}

		public override void Do()
		{
			foreach (IDictionary target in this.targetDictionaries)
			{
				if (!target.IsFixedSize && !target.IsReadOnly)
				{
					if (!target.Contains(this.key))
					{
						// Add a new key value pair
						object addedValue = this.valueType.IsValueType 
							? this.valueType.GetTypeInfo().CreateInstanceOf() 
							: null;
						target.Add(this.key, addedValue);
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
			foreach (IDictionary target in this.targetDictionaries)
			{
				if (!target.IsFixedSize && !target.IsReadOnly)
				{
					if (target.Contains(this.key))
					{
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
	}
}
