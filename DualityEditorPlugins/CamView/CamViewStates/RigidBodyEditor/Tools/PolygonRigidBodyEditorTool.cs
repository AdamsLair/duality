using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Components.Physics;
using Duality.Editor.Plugins.CamView.Properties;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Creates a new polygon shape in the selected <see cref="RigidBody"/>.
	/// </summary>
	public class PolygonRigidBodyEditorTool : PolyLikeRigidBodyEditorTool
	{
		public override string Name
		{
			get { return CamViewRes.RigidBodyCamViewState_ItemName_CreatePolygon; }
		}
		public override Image Icon
		{
			get { return CamViewResCache.IconCmpRectCollider; }
		}
		public override Cursor ActionCursor
		{
			get { return CamViewResCache.CursorArrowCreatePolygon; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.P; }
		}
		public override int SortOrder
		{
			get { return 2; }
		}
		protected override int MaxVertexCount
		{
			get { return PolyShapeInfo.MaxVertices; }
		}
		
		protected override ShapeInfo CreateShapeInfo(Vector2[] vertices)
		{
			return new PolyShapeInfo(vertices, 1.0f);
		}
		protected override Vector2[] GetVertices(ShapeInfo shape)
		{
			return (shape as PolyShapeInfo).Vertices;
		}
		protected override void SetVertices(ShapeInfo shape, Vector2[] vertices)
		{
			(shape as PolyShapeInfo).Vertices = vertices;
		}
		protected override bool IsValidPolyon(Vector2[] vertices)
		{
			return MathF.IsPolygonConvex(vertices);
		}
	}
}
