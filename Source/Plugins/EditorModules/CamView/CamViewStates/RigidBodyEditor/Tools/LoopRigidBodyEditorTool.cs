using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Components.Physics;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.UndoRedoActions;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Creates a new loop shape in the selected <see cref="RigidBody"/>.
	/// </summary>
	public class LoopRigidBodyEditorTool : VertexShapeRigidBodyEditorTool
	{
		public override string Name
		{
			get { return CamViewRes.RigidBodyCamViewState_ItemName_CreateLoop; }
		}
		public override Image Icon
		{
			get { return CamViewResCache.IconCmpLoopCollider; }
		}
		public override Cursor ActionCursor
		{
			get { return CamViewResCache.CursorArrowCreateLoop; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.L; }
		}
		public override int SortOrder
		{
			get { return 3; }
		}
		public override bool IsAvailable
		{
			get { return base.IsAvailable && this.Environment.ActiveBody.BodyType == BodyType.Static; }
		}

		protected override VertexBasedShapeInfo CreateShapeInfo(Vector2[] vertices)
		{
			return new LoopShapeInfo(vertices);
		}
	}
}
