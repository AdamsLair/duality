using System.Collections.Generic;
using System.Reflection;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Material), PropertyEditorAssignmentAttribute.PrioritySpecialized)]
	public class MaterialPropertyEditor : ResourcePropertyEditor
	{
		protected override void BeforeAutoCreateEditors()
		{
			base.BeforeAutoCreateEditors();
			BatchInfoPropertyEditor e = this.AddEditorForMember(ReflectionInfo.Field_Material_Info) as BatchInfoPropertyEditor;
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
