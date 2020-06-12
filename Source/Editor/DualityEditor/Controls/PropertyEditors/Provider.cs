using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using AdamsLair.WinForms.PropertyEditing;

using Duality.Drawing;


namespace Duality.Editor.Controls.PropertyEditors
{
	/// <summary>
	/// The <see cref="DualityPropertyEditorProvider"/> is responsible for selecting a
	/// matching <see cref="PropertyEditor"/> for each edited type inside a <see cref="DualitorPropertyGrid"/>,
	/// such as the one used by the defautl Object Inspector. It uses reflection to retrieve all
	/// available <see cref="PropertyEditor"/> classes as well as their <see cref="PropertyEditorAssignmentAttribute"/>
	/// values, which are then used to determine the priority of the assignment.
	/// </summary>
	public class DualityPropertyEditorProvider : IPropertyEditorProvider
	{
		private struct EditorItem
		{
			public TypeInfo EditorType;
			public PropertyEditorAssignmentAttribute Assignment;
		}

		private List<EditorItem> propertyEditorAssignments = null;

		/// <summary>
		/// Determines whether this provider is responsible for creating a <see cref="PropertyEditor"/>
		/// that is able to edit objects of the specified base type.
		/// </summary>
		/// <param name="baseType"></param>
		/// <param name="context"></param>
		public int IsResponsibleFor(Type baseType, ProviderContext context)
		{
			if (this.propertyEditorAssignments == null)
				this.RetrieveAssignments();

			int bestScore = PropertyGrid.EditorPriority_None;
			foreach (EditorItem item in this.propertyEditorAssignments)
			{
				int score = item.Assignment.MatchToProperty(baseType, context);
				if (score > bestScore)
				{
					bestScore = score;
				}
			}
			return bestScore;
		}
		/// <summary>
		/// Creates a <see cref="PropertyEditor"/> that is able to edit objects
		/// of the specified base type.
		/// </summary>
		/// <param name="baseType"></param>
		/// <param name="context"></param>
		public PropertyEditor CreateEditor(Type baseType, ProviderContext context)
		{
			if (this.propertyEditorAssignments == null)
				this.RetrieveAssignments();

			int bestScore = PropertyGrid.EditorPriority_None;
			TypeInfo bestType = null;
			foreach (EditorItem item in this.propertyEditorAssignments)
			{
				int score = item.Assignment.MatchToProperty(baseType, context);
				if (score > bestScore)
				{
					bestScore = score;
					bestType = item.EditorType;
				}
			}

			if (bestType != null)
				return bestType.CreateInstanceOf() as PropertyEditor;
			else
				return null;
		}

		/// <summary>
		/// Aggregates the cached list of all available <see cref="PropertyEditor"/>
		/// instances, as well as their matching <see cref="PropertyEditorAssignmentAttribute"/>.
		/// 
		/// Unlike cached core plugin type data, this list doesn't have to be cleared
		/// when reloading plugins, since <see cref="PropertyEditor"/> classes can only be defined
		/// in editor plugins - which require an editor restart anyway.
		/// </summary>
		private void RetrieveAssignments()
		{
			this.propertyEditorAssignments = new List<EditorItem>();

			IEnumerable<TypeInfo> propertyEditorTypes = DualityEditorApp.GetAvailDualityEditorTypes(typeof(PropertyEditor));
			foreach (TypeInfo editorType in propertyEditorTypes)
			{
				// Note: We don't want inherited attributes here!
				var assignment = editorType.GetCustomAttribute<PropertyEditorAssignmentAttribute>(false);
				if (assignment != null)
				{
					this.propertyEditorAssignments.Add(new EditorItem
					{
						EditorType = editorType,
						Assignment = assignment
					});
				}
			}
		}
	}
}
