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
	/// Creates a new chain shape in the selected <see cref="RigidBody"/>.
	/// </summary>
	public class ChainRigidBodyEditorTool : PolyLikeRigidBodyEditorTool
	{
		public override string Name
		{
			get { return CamViewRes.RigidBodyCamViewState_ItemName_CreateChain; }
		}
		public override Image Icon
		{
			get { return CamViewResCache.IconCmpEdgeCollider; }
		}
		public override Cursor ActionCursor
		{
			get { return CamViewResCache.CursorArrowCreateEdge; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.K; }
		}
		public override int SortOrder
		{
			get { return 4; }
		}
		public override bool IsAvailable
		{
			get { return base.IsAvailable && this.Environment.ActiveBody.BodyType == BodyType.Static; }
		}

		protected override Vector2[] GetInitialVertices(Vector2 basePos)
		{
			return new Vector2[] 
			{
				basePos, 
				basePos + Vector2.UnitX
			};
		}
		protected override ShapeInfo CreateShapeInfo(Vector2[] vertices)
		{
			return new ChainShapeInfo(vertices);
		}
		protected override Vector2[] GetVertices(ShapeInfo shape)
		{
			return (shape as ChainShapeInfo).Vertices;
		}
		protected override void SetVertices(ShapeInfo shape, Vector2[] vertices)
		{
			(shape as ChainShapeInfo).Vertices = vertices;
		}
	}
}
