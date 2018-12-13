using System;
using System.Collections.Generic;
using System.Linq;
using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(RectAtlas), PropertyGrid.EditorPriority_Specialized)]
	public class RectAtlasPropertyEditor : MemberwisePropertyEditor
	{
	}
}