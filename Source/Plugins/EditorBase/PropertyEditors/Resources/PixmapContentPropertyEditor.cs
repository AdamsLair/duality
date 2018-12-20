﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;

using Duality;
using Duality.Resources;
using Duality.Editor.Controls.PropertyEditors;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public class PixmapContentPropertyEditor : ResourcePropertyEditor
	{
		public PixmapContentPropertyEditor()
		{
			this.Hints = HintFlags.None;
			this.HeaderHeight = 0;
			this.HeaderValueText = null;
			this.Expanded = true;
		}

		protected override bool IsAutoCreateMember(MemberInfo info)
		{
			if (info.IsEquivalent(ReflectionInfo.Property_Pixmap_Atlas))
				return true;
			return base.IsAutoCreateMember(info);
		}

		protected override PropertyEditor AutoCreateMemberEditor(MemberInfo info)
		{
			if (info.IsEquivalent(ReflectionInfo.Property_Pixmap_Atlas))
			{
				MemberwisePropertyEditor editor = new MemberwisePropertyEditor();
				editor.EditedType = typeof(RectAtlas);
				return editor;
			}
			return base.AutoCreateMemberEditor(info);
		}

		protected override void OnValueChanged(object sender, PropertyEditorValueEventArgs args)
		{
			base.OnValueChanged(sender, args);
			if (args.Editor.EditedMember != null &&
				args.Editor.EditedMember.IsEquivalent(ReflectionInfo.Property_Pixmap_Atlas))
			{
				this.PerformGetValue();
				(this.ParentEditor as PixmapPropertyEditor).UpdatePreview();
			}
		}
	}
}
