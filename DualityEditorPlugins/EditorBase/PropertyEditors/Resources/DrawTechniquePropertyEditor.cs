using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Resources;
using Duality.Drawing;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(DrawTechnique), PropertyEditorAssignmentAttribute.PriorityGeneral + 1)]
	public class DrawTechniquePropertyEditor : ResourcePropertyEditor
	{
		protected override PropertyEditor AutoCreateMemberEditor(MemberInfo info)
		{
			if (ReflectionHelper.MemberInfoEquals(info, ReflectionInfo.Property_DrawTechnique_PreferredVertexFormat))
			{
				List<VertexFormatDefinition> formats = new List<VertexFormatDefinition>();
				formats.Add(null);
				foreach (Type vertexType in DualityApp.GetAvailDualityTypes(typeof(IVertexData)))
				{
					if (vertexType.IsClass) continue;
					if (vertexType.IsAbstract) continue;
					if (vertexType.IsInterface) continue;

					IVertexData vertex = vertexType.CreateInstanceOf() as IVertexData;
					formats.Add(vertex.Format);
				}

				ObjectSelectorPropertyEditor e = new ObjectSelectorPropertyEditor();
				e.EditedType = (info as PropertyInfo).PropertyType;
				e.Items = formats.Select(format => new ObjectItem(format, format != null ? format.DataType.Name : "None"));
				this.ParentGrid.ConfigureEditor(e);
				return e;
			}
			return base.AutoCreateMemberEditor(info);
		}
	}
}
