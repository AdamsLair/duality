using System.Linq;
using System.Reflection;

using Duality.Resources;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Font), PropertyEditorAssignmentAttribute.PrioritySpecialized)]
	public class FontPropertyEditor : ResourcePropertyEditor
	{
		public FontPropertyEditor()
		{
			this.Indent = 0;
		}

		protected override bool IsAutoCreateMember(MemberInfo info)
		{
			return false;
		}
		protected override void BeforeAutoCreateEditors()
		{
			base.BeforeAutoCreateEditors();
			FontPreviewPropertyEditor preview = new FontPreviewPropertyEditor();
			preview.EditedType = this.EditedType;
			preview.Getter = this.GetValue;
			this.ParentGrid.ConfigureEditor(preview);
			this.AddPropertyEditor(preview);
			FontContentPropertyEditor content = new FontContentPropertyEditor();
			content.EditedType = this.EditedType;
			content.Getter = this.GetValue;
			content.Setter = this.SetValues;
			content.Hints = HintFlags.None;
			content.HeaderHeight = 0;
			content.HeaderValueText = null;
			content.PreventFocus = true;
			this.ParentGrid.ConfigureEditor(content);
			this.AddPropertyEditor(content);
			content.Expanded = true;
		}
	}
}
