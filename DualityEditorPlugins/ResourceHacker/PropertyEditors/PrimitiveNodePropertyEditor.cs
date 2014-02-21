using System;
using System.Linq;
using System.Reflection;

using AdamsLair.PropertyGrid;

using Duality;
using Duality.Serialization;
using Duality.Serialization.MetaFormat;

namespace Duality.Editor.Plugins.ResourceHacker.PropertyEditors
{
	public class PrimitiveNodePropertyEditor : MemberwisePropertyEditor
	{
		protected	PropertyEditor	editorPrimitiveValue	= null;
		protected	bool			isInitializingContent	= false;
		

		public override void PerformGetValue()
		{
			if (this.isInitializingContent) return;

			this.isInitializingContent = true;
			this.InitContent();
			this.isInitializingContent = false;

			base.PerformGetValue();
		}
		protected override PropertyEditor AutoCreateMemberEditor(MemberInfo info)
		{
			if (ReflectionHelper.MemberInfoEquals(info, typeof(PrimitiveNode).GetProperty("PrimitiveValue")))
			{
				PrimitiveNode primitiveNode = this.GetValue().NotNull().FirstOrDefault() as PrimitiveNode;
				Type actualType = primitiveNode.NodeType.ToActualType();
				if (actualType == null) actualType = (info as PropertyInfo).PropertyType;

				this.editorPrimitiveValue = this.ParentGrid.CreateEditor(actualType, this);
				return this.editorPrimitiveValue;
			}
			else
				return base.AutoCreateMemberEditor(info);
		}
	}
}
