using System;
using System.Linq;
using System.Reflection;

using AdamsLair.PropertyGrid;

using Duality;
using Duality.Serialization.MetaFormat;

namespace Duality.Editor.Plugins.ResourceHacker.PropertyEditors
{
	public class ArrayNodePropertyEditor : MemberwisePropertyEditor
	{
		protected	PropertyEditor	editorPrimitiveData	= null;

		protected override PropertyEditor AutoCreateMemberEditor(MemberInfo info)
		{
			if (ReflectionHelper.MemberInfoEquals(info, typeof(ArrayNode).GetProperty("PrimitiveData")))
			{
				ArrayNode arrayNode = this.GetValue().NotNull().FirstOrDefault() as ArrayNode;
				Type actualType = ReflectionHelper.ResolveType(arrayNode.TypeString);
				if (actualType == null || !actualType.IsArray || actualType.GetElementType() == null) actualType = (info as PropertyInfo).PropertyType;

				bool primitiveDataEditable = actualType != null && actualType.IsArray && (actualType.GetElementType().IsPrimitive || actualType.GetElementType() == typeof(string));

				this.editorPrimitiveData = this.ParentGrid.CreateEditor(actualType, this);
				if (!primitiveDataEditable) this.editorPrimitiveData.Setter = null;
				return this.editorPrimitiveData;
			}
			else
				return base.AutoCreateMemberEditor(info);
		}
	}
}
