using System.Linq;
using System.Reflection;

using Duality.Resources;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Texture), PropertyEditorAssignmentAttribute.PrioritySpecialized)]
	public class TexturePropertyEditor : ResourcePropertyEditor
	{
		public TexturePropertyEditor()
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
			TexturePreviewPropertyEditor preview = new TexturePreviewPropertyEditor();
			preview.EditedType = this.EditedType;
			preview.Getter = this.GetValue;
			this.ParentGrid.ConfigureEditor(preview);
			this.AddPropertyEditor(preview);
			TextureContentPropertyEditor content = new TextureContentPropertyEditor();
			content.EditedType = this.EditedType;
			content.Getter = this.GetValue;
			content.Setter = this.SetValues;
			content.PreventFocus = true;
			this.ParentGrid.ConfigureEditor(content);
			this.AddPropertyEditor(content);
		}
	}
}
