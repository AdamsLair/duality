using System;

using AdamsLair.PropertyGrid;

using OpenTK;

using Duality;
using Duality.ColorFormat;

namespace DualityEditor.Controls.PropertyEditors
{
	public class DualityPropertyEditorProvider : IPropertyEditorProvider
	{
		public int IsResponsibleFor(Type baseType, ProviderContext context)
		{
			if (baseType == typeof(Vector2))		return PropertyGrid.EditorPriority_General;
			else if (baseType == typeof(Vector3))	return PropertyGrid.EditorPriority_General;
			else if (baseType == typeof(Vector4))	return PropertyGrid.EditorPriority_General;
			else if (baseType == typeof(Rect))		return PropertyGrid.EditorPriority_General;
			else if (baseType == typeof(Range))		return PropertyGrid.EditorPriority_General;

			else if (typeof(IColorData).IsAssignableFrom(baseType))		return PropertyGrid.EditorPriority_General;

			else return PropertyGrid.EditorPriority_None;
		}
		public PropertyEditor CreateEditor(Type baseType, ProviderContext context)
		{
			PropertyEditor e = null;

			if (baseType == typeof(Vector2))		e = new Vector2PropertyEditor();
			else if (baseType == typeof(Vector3))	e = new Vector3PropertyEditor();
			else if (baseType == typeof(Vector4))	e = new Vector4PropertyEditor();
			else if (baseType == typeof(Rect))		e = new RectPropertyEditor();
			else if (baseType == typeof(Range))		e = new RangePropertyEditor();

			else if (typeof(IColorData).IsAssignableFrom(baseType))		e = new IColorDataPropertyEditor();

			return e;
		}
	}
}
