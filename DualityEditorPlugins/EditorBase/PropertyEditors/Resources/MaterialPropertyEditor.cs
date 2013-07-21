using System.Collections.Generic;
using System.Reflection;
using Duality;
using DualityEditor;

namespace EditorBase.PropertyEditors
{
	public class MaterialPropertyEditor : ResourcePropertyEditor
	{
		protected override void BeforeAutoCreateEditors()
		{
			base.BeforeAutoCreateEditors();
			BatchInfoPropertyEditor e = this.AddEditorForField(ReflectionInfo.Field_Material_Info) as BatchInfoPropertyEditor;
			e.PropertyName = null;
			e.Hints = HintFlags.None;
			e.HeaderIcon = null;
			e.HeaderValueText = null;
			e.HeaderHeight = 0;
			e.Indent = 0;
			e.Expanded = true;
		}
		protected override bool IsAutoCreateMember(MemberInfo info)
		{
			return false;
		}
	}
}
