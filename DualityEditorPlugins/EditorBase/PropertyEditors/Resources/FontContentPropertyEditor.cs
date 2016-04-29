using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public class FontContentPropertyEditor : ResourcePropertyEditor
	{
		private bool canReRenderGlyphs = false;

		protected override void OnGetValue()
		{
			base.OnGetValue();
			Font font = this.GetValue().FirstOrDefault() as Font;

			// Can we re-render this font dynamically when needed? This will determine whether
			// users are allowed to change properties like Size or Style.
			this.canReRenderGlyphs = (font != null && font.EmbeddedTrueTypeFont != null);
			this.UpdateReadOnlyState();
		}

		private void UpdateReadOnlyState()
		{
			foreach (PropertyEditor memberEditor in this.ChildEditors)
			{
				bool willRequireReRender = 
					memberEditor.EditedMember == ReflectionInfo.Property_Font_Size ||
					memberEditor.EditedMember == ReflectionInfo.Property_Font_Style ||
					memberEditor.EditedMember == ReflectionInfo.Property_Font_GlyphRenderMode ||
					memberEditor.EditedMember == ReflectionInfo.Property_Font_MonoSpace;

				if (willRequireReRender)
					memberEditor.ReadOnly = !this.canReRenderGlyphs;
			}
		}
	}
}
