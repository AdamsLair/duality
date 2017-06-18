using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public class RigidBodyEditorSelVertexShape : RigidBodyEditorSelShape
	{
		private Vector2 center;
		private float   boundRad;
		private float   angle;
		private Vector2 scale;

		public override Vector3 Pos
		{
			get
			{
				return this.Body.GameObj.Transform.GetWorldPoint(new Vector3(this.center));
			}
			set
			{
				value.Z = this.Body.GameObj.Transform.Pos.Z;
				this.MoveCenterTo(this.Body.GameObj.Transform.GetLocalPoint(value).Xy);
			}
		}
		public override Vector3 Scale
		{
			get { return new Vector3(this.scale); }
			set { this.ScaleTo(value.Xy); }
		}
		public override float Angle
		{
			get { return this.angle; }
			set { this.RotateTo(value); }
		}
		public override float BoundRadius
		{
			get { return this.boundRad * this.Body.GameObj.Transform.Scale; }
		}
		public override string DisplayObjectName
		{
			get { return Properties.CamViewRes.RigidBodyCamViewState_SelPolyShapeName; }
		}
		private VertexBasedShapeInfo VertexShape
		{
			get { return this.Shape as VertexBasedShapeInfo; }
		}


		public RigidBodyEditorSelVertexShape(VertexBasedShapeInfo shape) : base(shape)
		{
			this.UpdateShapeStats();
		}

		public override string UpdateActionText(ObjectEditorAction action, bool performing)
		{
			if (action == ObjectEditorAction.Move)
			{
				return
					string.Format("Center X:{0,9:0.00}/n", this.center.X) +
					string.Format("Center Y:{0,9:0.00}", this.center.Y);
			}
			else if (action == ObjectEditorAction.Scale)
			{
				if (MathF.Abs(this.scale.X - this.scale.Y) >= 0.01f)
				{
					return
						string.Format("Scale X:{0,8:0.00}/n", this.scale.X) +
						string.Format("Scale Y:{0,8:0.00}", this.scale.Y);
				}
				else
				{
					return string.Format("Scale:{0,8:0.00}", this.scale.X);
				}
			}
			else if (action == ObjectEditorAction.Rotate)
			{
				return string.Format("Angle:{0,6:0.0}°", MathF.RadToDeg(this.angle));
			}
			return base.UpdateActionText(action, performing);
		}

		public override void UpdateShapeStats()
		{
			Vector2[] vertices = this.VertexShape.Vertices;

			this.center = Vector2.Zero;
			for (int i = 0; i < vertices.Length; i++)
				this.center += vertices[i];
			this.center /= vertices.Length;

			this.scale = Vector2.Zero;
			for (int i = 0; i < vertices.Length; i++)
			{
				this.scale.X = MathF.Max(this.scale.X, MathF.Abs(vertices[i].X - this.center.X));
				this.scale.Y = MathF.Max(this.scale.Y, MathF.Abs(vertices[i].Y - this.center.Y));
			}

			this.boundRad = 0.0f;
			for (int i = 0; i < vertices.Length; i++)
				this.boundRad = MathF.Max(this.boundRad, (vertices[i] - this.center).Length);

			this.angle = MathF.Angle(this.center.X, this.center.Y, vertices[0].X, vertices[0].Y);
		}
		private void MoveCenterTo(Vector2 newPos)
		{
			Vector2 mov = newPos - this.center;

			Vector2[] movedVertices = this.VertexShape.Vertices.ToArray();
			for (int i = 0; i < movedVertices.Length; i++)
				movedVertices[i] += mov;

			this.VertexShape.Vertices = movedVertices;
			this.UpdateShapeStats();
		}
		private void ScaleTo(Vector2 newScale)
		{
			Vector2 scaleRatio = newScale / this.scale;

			Vector2[] scaledVertices = this.VertexShape.Vertices.ToArray();
			for (int i = 0; i < scaledVertices.Length; i++)
			{
				scaledVertices[i].X = (scaledVertices[i].X - this.center.X) * scaleRatio.X + this.center.X;
				scaledVertices[i].Y = (scaledVertices[i].Y - this.center.Y) * scaleRatio.Y + this.center.Y;
			}

			this.VertexShape.Vertices = scaledVertices;
			this.UpdateShapeStats();
		}
		private void RotateTo(float newAngle)
		{
			float rot = newAngle - this.angle;

			Vector2[] rotatedVertices = this.VertexShape.Vertices.ToArray();
			for (int i = 0; i < rotatedVertices.Length; i++)
				MathF.TransformCoord(ref rotatedVertices[i].X, ref rotatedVertices[i].Y, rot, 1.0f, this.center.X, this.center.Y);

			this.VertexShape.Vertices = rotatedVertices;
			this.UpdateShapeStats();
		}
	}
}
