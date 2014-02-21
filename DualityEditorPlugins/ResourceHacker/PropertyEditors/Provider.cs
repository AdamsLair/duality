using System;
using AdamsLair.PropertyGrid;
using Duality.Serialization.MetaFormat;

namespace Duality.Editor.Plugins.ResourceHacker.PropertyEditors
{
	public class PropertyEditorProvider : IPropertyEditorProvider
	{
		public int IsResponsibleFor(Type baseType, ProviderContext context)
		{
			if (baseType == typeof(ArrayNode))			return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(PrimitiveNode))	return PropertyGrid.EditorPriority_Specialized;
			else return PropertyGrid.EditorPriority_None;
		}
		public PropertyEditor CreateEditor(Type baseType, ProviderContext context)
		{
			PropertyEditor e = null;

			if (baseType == typeof(ArrayNode))			e = new ArrayNodePropertyEditor();
			else if (baseType == typeof(PrimitiveNode))	e = new PrimitiveNodePropertyEditor();

			return e;
		}
	}
}
