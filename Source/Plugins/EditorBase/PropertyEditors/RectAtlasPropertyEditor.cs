using System;
using System.Collections.Generic;
using System.Linq;
using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(RectAtlas), PropertyGrid.EditorPriority_Specialized)]
	public class RectAtlasPropertyEditor : GroupedPropertyEditor
	{
		private IListPropertyEditor baseRectListEditor;
		private IListPropertyEditor pivotListEditor;

		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}

		public override void InitContent()
		{
			base.InitContent();
			if (this.baseRectListEditor == null)
			{
				this.baseRectListEditor = new IListPropertyEditor
				{
					PropertyName = "Rects",
					EditedType = this.EditedType,
					Getter = this.CreateRectsGetter(),
					Setter = this.CreateRectsSetter(),
					ForceWriteBack = true
				};
				this.AddPropertyEditor(this.baseRectListEditor);
			}
			if (this.pivotListEditor == null)
			{
				this.pivotListEditor = new IListPropertyEditor
				{
					PropertyName = "Pivots",
					EditedType = typeof(Vector2[]),
					Getter = this.CreatePivotsGetter(),
					Setter = this.CreatePivotsSetter()
				};
				this.AddPropertyEditor(this.pivotListEditor);
			}
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			foreach (PropertyEditor e in this.ChildEditors)
				e.PerformGetValue();
		}

		private Func<IEnumerable<object>> CreateRectsGetter()
		{
			return () => this.GetValue().Cast<IList<Rect>>();
		}

		private Action<IEnumerable<object>> CreateRectsSetter()
		{
			return values =>
			{
				this.SetValues(values);
				this.pivotListEditor.PerformGetValue();
			};
		}

		private Func<IEnumerable<object>> CreatePivotsGetter()
		{
			return () => this.GetValue().Select(o => o == null ? null : ((RectAtlas)o).Pivots);
		}

		private Action<IEnumerable<object>> CreatePivotsSetter()
		{
			return values =>
			{
				IEnumerator<Vector2[]> valuesEnum = values.Cast<Vector2[]>().GetEnumerator();

				Vector2[] curValue = null;
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;

				RectAtlas[] thisValueArray = this.GetValue().Cast<RectAtlas>().ToArray();
				for (int i = 0; i < thisValueArray.Length; i++)
				{
					if (thisValueArray[i] == null)
					{
						thisValueArray[i] = new RectAtlas();
					}

					if (curValue == null)
						continue;

					// Resize the rect atlas to match the pivots array
					while (thisValueArray[i].Count < curValue.Length)
						thisValueArray[i].Add(default(Rect));
					while (thisValueArray[i].Count > curValue.Length)
						thisValueArray[i].RemoveAt(thisValueArray[i].Count - 1);

					// Copy over the new pivot values
					for (int j = 0; j < thisValueArray[i].Count; j++)
					{
						thisValueArray[i].Pivots[j] = curValue[j];
					}

					if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
				}

				this.SetValues(thisValueArray);
				this.baseRectListEditor.PerformGetValue();
			};
		}
	}
}