using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;

using Duality;
using Duality.Resources;
using Duality.Editor.Controls.PropertyEditors;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	public class TextureContentPropertyEditor : ResourcePropertyEditor
	{
		public TextureContentPropertyEditor()
		{
			this.Hints = HintFlags.None;
			this.HeaderHeight = 0;
			this.HeaderValueText = null;
			this.Expanded = true;
		}
	}
}
