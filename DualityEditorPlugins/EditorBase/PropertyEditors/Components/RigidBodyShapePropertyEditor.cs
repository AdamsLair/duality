using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AdamsLair.WinForms;

using Duality;
using Duality.Components.Physics;

using Duality.Editor;
using Duality.Editor.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(ShapeInfo))]
	public class RigidBodyShapePropertyEditor : MemberwisePropertyEditor
	{
		public RigidBodyShapePropertyEditor()
		{
			this.Hints &= ~HintFlags.HasButton;
			this.Hints &= ~HintFlags.ButtonEnabled;
		}

		protected override void OnPropertySet(PropertyInfo property, IEnumerable<object> targets)
		{
			base.OnPropertySet(property, targets);

			var colShapes = targets.OfType<ShapeInfo>().ToArray();
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, property, colShapes, null));

			var colliders = colShapes.Select(c => c.Parent).ToArray();
			foreach (var c in colliders) c.AwakeBody();
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, ReflectionInfo.Property_RigidBody_Shapes, colliders, null));
		}
	}
}
