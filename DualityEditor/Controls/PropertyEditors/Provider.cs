using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using AdamsLair.WinForms.PropertyEditing;

using Duality.Drawing;


namespace Duality.Editor.Controls.PropertyEditors
{
	public class DualityPropertyEditorProvider : IPropertyEditorProvider
	{
		public int IsResponsibleFor(Type baseType, ProviderContext context)
		{
			IEnumerable<TypeInfo> propertyEditorTypes = DualityEditorApp.GetAvailDualityEditorTypes(typeof(PropertyEditor));
			if (propertyEditorTypes.Any())
			{
				int bestScore = PropertyGrid.EditorPriority_None;
				foreach (TypeInfo editorType in propertyEditorTypes)
				{
					var assignment = editorType.GetAttributesCached<PropertyEditorAssignmentAttribute>().FirstOrDefault();
					if (assignment == null) continue;
					int score = assignment.MatchToProperty(baseType, context);
					if (score > bestScore)
					{
						bestScore = score;
					}
				}
				return bestScore;
			}
			else
			{
				return PropertyGrid.EditorPriority_None;
			}
		}
		public PropertyEditor CreateEditor(Type baseType, ProviderContext context)
		{
			IEnumerable<TypeInfo> propertyEditorTypes = DualityEditorApp.GetAvailDualityEditorTypes(typeof(PropertyEditor));
			if (propertyEditorTypes.Any())
			{
				int bestScore = PropertyGrid.EditorPriority_None;
				TypeInfo bestType = null;
				foreach (TypeInfo editorType in propertyEditorTypes)
				{
					var assignment = editorType.GetAttributesCached<PropertyEditorAssignmentAttribute>().FirstOrDefault();
					if (assignment == null) continue;
					int score = assignment.MatchToProperty(baseType, context);
					if (score > bestScore)
					{
						bestScore = score;
						bestType = editorType;
					}
				}
				if (bestType != null) return bestType.CreateInstanceOf() as PropertyEditor;
			}

			return null;
		}
	}
}
