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
			if (info.IsEquivalent(ReflectionInfo.Property_DrawTechnique_PreferredVertexFormat))
			{
				List<VertexDeclaration> vertexTypes = new List<VertexDeclaration>();
				vertexTypes.Add(null);
				foreach (TypeInfo vertexType in DualityApp.GetAvailDualityTypes(typeof(IVertexData)))
				{
					if (vertexType.IsClass) continue;
					if (vertexType.IsAbstract) continue;
					if (vertexType.IsInterface) continue;

					vertexTypes.Add(VertexDeclaration.Get(vertexType.AsType()));
				}

				ObjectSelectorPropertyEditor e = new ObjectSelectorPropertyEditor();
				e.EditedType = (info as PropertyInfo).PropertyType;
				e.Items = vertexTypes.Select(decl => new ObjectItem(decl, decl != null ? decl.DataType.Name : "None"));
				this.ParentGrid.ConfigureEditor(e);
				return e;
			}
			return base.AutoCreateMemberEditor(info);
		}
	}
}
