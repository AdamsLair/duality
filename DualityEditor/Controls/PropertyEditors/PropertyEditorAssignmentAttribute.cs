using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AdamsLair.WinForms;


namespace Duality.Editor
{
	/// <summary>
	/// Calculates a PropertyEditorAssignment priority for the given property Type in a certain context.
	/// </summary>
	/// <param name="propertyType"></param>
	/// <param name="context"></param>
	/// <returns>The priority which connects the given Type to the PropertyEditor. Return <see cref="PropertyEditorAssignmentAttribute.PriorityNone"/> for no assignment.</returns>
	public delegate int PropertyEditorMatching(Type propertyType, ProviderContext context);

	/// <summary>
	/// This attribute is utilized to match PropertyEditors to the property Types they are supposed to edit.
	/// </summary>
	public class PropertyEditorAssignmentAttribute : Attribute
	{
		public const int PriorityGeneral		= PropertyGrid.EditorPriority_General;
		public const int PrioritySpecialized	= PropertyGrid.EditorPriority_Specialized;
		public const int PriorityOverride		= PropertyGrid.EditorPriority_Override;
		public const int PriorityNone			= PropertyGrid.EditorPriority_None;

		private	Type					assignToType	= null;
		private	int						assignPriority	= PropertyGrid.EditorPriority_General;
		private	PropertyEditorMatching	dynamicAssign	= null;

		/// <summary>
		/// Creates a static PropertyEditor assignment to the specified property Type.
		/// </summary>
		/// <param name="propertyType"></param>
		/// <param name="priority"></param>
		public PropertyEditorAssignmentAttribute(Type propertyType, int priority = PropertyGrid.EditorPriority_General)
		{
			this.assignToType = propertyType;
			this.assignPriority = priority;
		}
		/// <summary>
		/// Creates a dynamic PropertyEditor assignment based on the specified static <see cref="PropertyEditorMatching"/> method.
		/// </summary>
		/// <param name="methodHostType"></param>
		/// <param name="staticMethodName"></param>
		public PropertyEditorAssignmentAttribute(Type methodHostType, string staticMethodName)
		{
			MethodInfo methodInfo = methodHostType.GetMethod(staticMethodName, ReflectionHelper.BindStaticAll);
			this.dynamicAssign = Delegate.CreateDelegate(typeof(PropertyEditorMatching), methodInfo) as PropertyEditorMatching;
		}

		/// <summary>
		/// Determines the matching level between a PropertyEditor and the specified property Type.
		/// </summary>
		/// <param name="propertyType"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public int MatchToProperty(Type propertyType, ProviderContext context)
		{
			if (this.dynamicAssign != null)
				return this.dynamicAssign(propertyType, context);
			else if (this.assignToType.IsAssignableFrom(propertyType))
				return this.assignPriority;
			else
				return PropertyGrid.EditorPriority_None;
		}
	}
}
