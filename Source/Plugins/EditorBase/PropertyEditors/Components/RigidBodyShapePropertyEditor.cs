﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AdamsLair.WinForms.PropertyEditing;

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

			var shapes = targets.OfType<ShapeInfo>().ToArray();
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, property, shapes, null));

			var parentBodies = shapes.Select(c => c.Parent).NotNull().ToArray();
			foreach (var body in parentBodies) body.AwakeBody();
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, ReflectionInfo.Property_RigidBody_Shapes, parentBodies, null));
		}
	}
}
