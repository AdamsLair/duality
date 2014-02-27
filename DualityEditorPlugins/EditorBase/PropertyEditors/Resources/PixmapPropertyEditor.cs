using System.Reflection;
using System.Linq;

using Duality.Resources;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(Pixmap), PropertyEditorAssignmentAttribute.PrioritySpecialized)]
	public class PixmapPropertyEditor : ResourcePropertyEditor
	{
		public PixmapPropertyEditor()
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
			PixmapPreviewPropertyEditor preview = new PixmapPreviewPropertyEditor();
			preview.EditedType = this.EditedType;
			preview.Getter = this.GetValue;
			this.ParentGrid.ConfigureEditor(preview);
			this.AddPropertyEditor(preview);
			PixmapContentPropertyEditor content = new PixmapContentPropertyEditor();
			content.EditedType = this.EditedType;
			content.Getter = this.GetValue;
			content.Setter = this.SetValues;
			content.PreventFocus = true;
			this.ParentGrid.ConfigureEditor(content);
			this.AddPropertyEditor(content);
		}

		public void UpdatePreview()
		{
			this.Children.OfType<PixmapPreviewPropertyEditor>().First().PerformGetValue();
		}
	}
}
