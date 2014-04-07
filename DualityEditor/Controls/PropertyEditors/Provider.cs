using System;
using System.Linq;
using System.Collections.Generic;

using AdamsLair.WinForms.PropertyEditing;
using OpenTK;

using Duality.Drawing;


namespace Duality.Editor.Controls.PropertyEditors
{
	public class DualityPropertyEditorProvider : IPropertyEditorProvider
	{
		public int IsResponsibleFor(Type baseType, ProviderContext context)
		{
			IEnumerable<Type> propertyEditorTypes = DualityEditorApp.GetAvailDualityEditorTypes(typeof(PropertyEditor));
			if (propertyEditorTypes.Any())
			{
				int bestScore = PropertyGrid.EditorPriority_None;
				foreach (Type editorType in propertyEditorTypes)
				{
					var assignment = editorType.GetCustomAttributes<PropertyEditorAssignmentAttribute>().FirstOrDefault();
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
			IEnumerable<Type> propertyEditorTypes = DualityEditorApp.GetAvailDualityEditorTypes(typeof(PropertyEditor));
			if (propertyEditorTypes.Any())
			{
				int bestScore = PropertyGrid.EditorPriority_None;
				Type bestType = null;
				foreach (Type editorType in propertyEditorTypes)
				{
					var assignment = editorType.GetCustomAttributes<PropertyEditorAssignmentAttribute>().FirstOrDefault();
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
