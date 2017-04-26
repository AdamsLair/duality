using System.Linq;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	public class SetupRigidBodyForEditing : EditorSingleAction<RigidBody>
	{
		public override bool CanPerformOn(RigidBody body)
		{
			return base.CanPerformOn(body) && !body.Shapes.Any();
		}
				
		public override void Perform(RigidBody body)
		{
			if (body.Shapes.Any()) return;

			// Add a default shape when creating a new RigidBody in the editor
			body.AddShape(new CircleShapeInfo(128.0f, Vector2.Zero, 1.0f));
		}
				
		public override bool MatchesContext(string context)
		{
			return context == DualityEditorApp.ActionContextSetupObjectForEditing;
		}
	}
}
