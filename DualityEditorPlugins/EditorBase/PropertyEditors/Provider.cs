using System;
using AdamsLair.PropertyGrid;

using Duality;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Components.Renderers;

namespace EditorBase.PropertyEditors
{
	public class PropertyEditorProvider : IPropertyEditorProvider
	{
		public int IsResponsibleFor(Type baseType, ProviderContext context)
		{
			// -------- Specialized area --------
			if (baseType == typeof(GameObject))			return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(Transform))		return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(SoundEmitter))	return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(BatchInfo))		return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(Material))		return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(Texture))		return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(AudioData))		return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(Pixmap))		return PropertyGrid.EditorPriority_Specialized;
			else if (baseType == typeof(Font))			return PropertyGrid.EditorPriority_Specialized;

			// -------- Semi-Specialized area --------
			else if (typeof(RigidBody).IsAssignableFrom(baseType))			return PropertyGrid.EditorPriority_General + 1;
			else if (typeof(DrawTechnique).IsAssignableFrom(baseType))		return PropertyGrid.EditorPriority_General + 1;

			// -------- General area --------
			else if (typeof(ShapeInfo).IsAssignableFrom(baseType))			return PropertyGrid.EditorPriority_General;
			else if (typeof(Component).IsAssignableFrom(baseType))			return PropertyGrid.EditorPriority_General;
			else if (typeof(Resource).IsAssignableFrom(baseType))			return PropertyGrid.EditorPriority_General;
			else if (typeof(IContentRef).IsAssignableFrom(baseType))		return PropertyGrid.EditorPriority_General;
			
			else return PropertyGrid.EditorPriority_None;
		}
		public PropertyEditor CreateEditor(Type baseType, ProviderContext context)
		{
			PropertyEditor e = null;
			bool compRef = !(context.ParentEditor is GameObjectOverviewPropertyEditor);

			// -------- Specialized area --------
			if (baseType == typeof(GameObject))
			{
				if (context.ParentEditor == null)
					e = new GameObjectOverviewPropertyEditor();
				else
					e = new GameObjectRefPropertyEditor();
			}
			else if (baseType == typeof(Transform) && !compRef)		e = new TransformPropertyEditor();
			else if (baseType == typeof(SoundEmitter) && !compRef)	e = new SoundEmitterPropertyEditor();
			else if (baseType == typeof(BatchInfo))					e = new BatchInfoPropertyEditor();
			else if (baseType == typeof(Material))					e = new MaterialPropertyEditor();
			else if (baseType == typeof(Texture))					e = new TexturePropertyEditor();
			else if (baseType == typeof(AudioData))					e = new AudioDataPropertyEditor();
			else if (baseType == typeof(Pixmap))					e = new PixmapPropertyEditor();
			else if (baseType == typeof(Font))						e = new FontPropertyEditor();

			//// -------- Semi-Specialized area --------
			else if (typeof(RigidBody).IsAssignableFrom(baseType) && !compRef)	e = new RigidBodyPropertyEditor();
			else if (typeof(DrawTechnique).IsAssignableFrom(baseType))			e = new DrawTechniquePropertyEditor();

			// -------- General area --------
			else if (typeof(ShapeInfo).IsAssignableFrom(baseType))				e = new RigidBodyShapePropertyEditor();
			else if (typeof(Component).IsAssignableFrom(baseType) && compRef)	e = new ComponentRefPropertyEditor();
			else if (typeof(Component).IsAssignableFrom(baseType) && !compRef)	e = new ComponentPropertyEditor();
			else if (typeof(Resource).IsAssignableFrom(baseType))				e = new ResourcePropertyEditor();
			else if (typeof(IContentRef).IsAssignableFrom(baseType))			e = new IContentRefPropertyEditor();

			return e;
		}
	}
}
