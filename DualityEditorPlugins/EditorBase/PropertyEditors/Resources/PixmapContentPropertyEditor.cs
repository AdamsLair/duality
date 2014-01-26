using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AdamsLair.PropertyGrid;
using AdamsLair.PropertyGrid.PropertyEditors;

using Duality;
using Duality.Resources;
using DualityEditor.Controls.PropertyEditors;

namespace EditorBase.PropertyEditors
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

		protected override void OnPropertySet(PropertyInfo property, IEnumerable<object> targets)
		{
			base.OnPropertySet(property, targets);
			if (ReflectionHelper.MemberInfoEquals(property, ReflectionInfo.Property_Pixmap_AnimCols) ||
				ReflectionHelper.MemberInfoEquals(property, ReflectionInfo.Property_Pixmap_AnimRows) ||
				ReflectionHelper.MemberInfoEquals(property, ReflectionInfo.Property_Pixmap_AnimFrameBorder) ||
				ReflectionHelper.MemberInfoEquals(property, ReflectionInfo.Property_Pixmap_Atlas))
			{
				this.PerformGetValue();
				(this.ParentEditor as PixmapPropertyEditor).UpdatePreview();
			}
		}
	}
}
