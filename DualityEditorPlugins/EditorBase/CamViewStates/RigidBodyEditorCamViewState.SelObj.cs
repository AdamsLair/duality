using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

using OpenTK;

namespace EditorBase.CamViewStates
{
	public partial class RigidBodyEditorCamViewState
	{
		public class SelBody : SelObj
		{
			private	GameObject	bodyObj;

			public override object ActualObject
			{
				get { return this.bodyObj == null || this.bodyObj.Disposed ? null : this.bodyObj; }
			}
			public override string DisplayObjectName
			{
				get { return PluginRes.EditorBaseRes.RigidBodyCamViewState_SelBodyName; }
			}
			public override bool HasTransform
			{
				get { return this.bodyObj != null && !this.bodyObj.Disposed && this.bodyObj.Transform != null; }
			}
			public override Vector3 Pos
			{
				get { return this.bodyObj.Transform.Pos; }
				set { }
			}
			public override float Angle
			{
				get { return this.bodyObj.Transform.Angle; }
				set { }
			}
			public override Vector3 Scale
			{
				get { return Vector3.One * this.bodyObj.Transform.Scale; }
				set { }
			}
			public override float BoundRadius
			{
				get
				{
					ICmpRenderer r = this.bodyObj.Renderer;
					return r == null ? CamView.DefaultDisplayBoundRadius : r.BoundRadius;
				}
			}
			public override bool ShowPos
			{
				get { return false; }
			}

			public SelBody(RigidBody obj)
			{
				this.bodyObj = obj != null ? obj.GameObj : null;
			}

			public override bool IsActionAvailable(ObjectAction action)
			{
				return false;
			}
		}

		public abstract class SelShape : SelObj
		{
			private		ShapeInfo	shape;
			
			public override bool HasTransform
			{
				get { return this.Body != null && !this.Body.Disposed && this.Body.GameObj.Transform != null; }
			}
			public override object ActualObject
			{
				get { return this.shape; }
			}
			public override string DisplayObjectName
			{
				get { return PluginRes.EditorBaseRes.RigidBodyCamViewState_SelShapeName; }
			}
			public RigidBody Body
			{
				get { return this.shape.Parent; }
			}

			protected SelShape(ShapeInfo shape)
			{
				this.shape = shape;
			}

			public override bool IsActionAvailable(ObjectAction action)
			{
				if (action == ObjectAction.Move) return true;
				if (action == ObjectAction.Rotate) return true;
				if (action == ObjectAction.Scale) return true;
				return false;
			}

			public static SelShape Create(ShapeInfo shape)
			{
				if (shape is CircleShapeInfo)		return new SelCircleShape(shape as CircleShapeInfo);
				else if (shape is PolyShapeInfo)	return new SelPolyShape(shape as PolyShapeInfo);
			//	else if (shape is EdgeShapeInfo)	return new SelEdgeShape(shape as EdgeShapeInfo);
				else if (shape is LoopShapeInfo)	return new SelLoopShape(shape as LoopShapeInfo);
				else								return null;
			}
		}
		public class SelCircleShape : SelShape
		{
			private	CircleShapeInfo	circle;
			
			public override Vector3 Pos
			{
				get
				{
					return this.Body.GameObj.Transform.GetWorldPoint(new Vector3(this.circle.Position));
				}
				set
				{
					value.Z = this.Body.GameObj.Transform.Pos.Z;
					this.circle.Position = this.Body.GameObj.Transform.GetLocalPoint(value).Xy;
				}
			}
			public override Vector3 Scale
			{
				get
				{
					return Vector3.One * this.circle.Radius;
				}
				set
				{
					this.circle.Radius = value.Length / MathF.Sqrt(3.0f);
				}
			}
			public override float BoundRadius
			{
				get { return this.circle.Radius * this.Body.GameObj.Transform.Scale; }
			}
			public override string DisplayObjectName
			{
				get { return PluginRes.EditorBaseRes.RigidBodyCamViewState_SelCircleShapeName; }
			}

			public SelCircleShape(CircleShapeInfo shape) : base(shape)
			{
				this.circle = shape;
			}

			public override bool IsActionAvailable(ObjectAction action)
			{
				if (action == ObjectAction.Rotate) return false;
				return base.IsActionAvailable(action);
			}
			public override string UpdateActionText(ObjectAction action, bool performing)
			{
				if (action == ObjectAction.Move)
				{
					return
						string.Format("Center X:{0,9:0.00}/n", this.circle.Position.X) +
						string.Format("Center Y:{0,9:0.00}", this.circle.Position.Y);
				}
				else if (action == ObjectAction.Scale)
				{
					return string.Format("Radius:{0,8:0.00}", this.circle.Radius);
				}
				return base.UpdateActionText(action, performing);
			}
		}
		public class SelPolyShape : SelShape
		{
			private	PolyShapeInfo	poly;
			private	Vector2	center;
			private	float	boundRad;
			private	float	angle;
			private	Vector2	scale;

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
				get
				{
					return new Vector3(this.scale);
				}
				set
				{
					this.ScaleTo(value.Xy);
				}
			}
			public override float Angle
			{
				get
				{
					return this.angle;
				}
				set
				{
					this.RotateTo(value);
				}
			}
			public override float BoundRadius
			{
				get { return this.boundRad * this.Body.GameObj.Transform.Scale; }
			}
			public override string DisplayObjectName
			{
				get { return PluginRes.EditorBaseRes.RigidBodyCamViewState_SelPolyShapeName; }
			}

			public SelPolyShape(PolyShapeInfo shape) : base(shape)
			{
				this.poly = shape;
				this.UpdatePolyStats();
			}

			public override string UpdateActionText(ObjectAction action, bool performing)
			{
				if (action == ObjectAction.Move)
				{
					return
						string.Format("Center X:{0,9:0.00}/n", this.center.X) +
						string.Format("Center Y:{0,9:0.00}", this.center.Y);
				}
				else if (action == ObjectAction.Scale)
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
				else if (action == ObjectAction.Rotate)
				{
					return string.Format("Angle:{0,6:0.0}°", MathF.RadToDeg(this.angle));
				}
				return base.UpdateActionText(action, performing);
			}

			public void UpdatePolyStats()
			{
				this.center = Vector2.Zero;
				for (int i = 0; i < this.poly.Vertices.Length; i++)
					this.center += this.poly.Vertices[i];
				this.center /= this.poly.Vertices.Length;

				this.scale = Vector2.Zero;
				for (int i = 0; i < this.poly.Vertices.Length; i++)
				{
					this.scale.X = MathF.Max(this.scale.X, MathF.Abs(this.poly.Vertices[i].X - this.center.X));
					this.scale.Y = MathF.Max(this.scale.Y, MathF.Abs(this.poly.Vertices[i].Y - this.center.Y));
				}

				this.boundRad = 0.0f;
				for (int i = 0; i < this.poly.Vertices.Length; i++)
					this.boundRad = MathF.Max(this.boundRad, (this.poly.Vertices[i] - this.center).Length);

				this.angle = MathF.Angle(this.center.X, this.center.Y, this.poly.Vertices[0].X, this.poly.Vertices[0].Y);
			}
			private void MoveCenterTo(Vector2 newPos)
			{
				Vector2 mov = newPos - this.center;

				Vector2[] movedVertices = this.poly.Vertices.ToArray();
				for (int i = 0; i < movedVertices.Length; i++)
					movedVertices[i] += mov;

				this.poly.Vertices = movedVertices;
				this.UpdatePolyStats();
			}
			private void ScaleTo(Vector2 newScale)
			{
				Vector2 scaleRatio = newScale / this.scale;

				Vector2[] scaledVertices = this.poly.Vertices.ToArray();
				for (int i = 0; i < scaledVertices.Length; i++)
				{
					scaledVertices[i].X = (scaledVertices[i].X - this.center.X) * scaleRatio.X + this.center.X;
					scaledVertices[i].Y = (scaledVertices[i].Y - this.center.Y) * scaleRatio.Y + this.center.Y;
				}

				this.poly.Vertices = scaledVertices;
				this.UpdatePolyStats();
			}
			private void RotateTo(float newAngle)
			{
				float rot = newAngle - this.angle;

				Vector2[] rotatedVertices = this.poly.Vertices.ToArray();
				for (int i = 0; i < rotatedVertices.Length; i++)
					MathF.TransformCoord(ref rotatedVertices[i].X, ref rotatedVertices[i].Y, rot, 1.0f, this.center.X, this.center.Y);

				this.poly.Vertices = rotatedVertices;
				this.UpdatePolyStats();
			}
		}
		public class SelLoopShape : SelShape
		{
			private	LoopShapeInfo	loop;
			private	Vector2	center;
			private	float	boundRad;
			private	float	angle;
			private	Vector2	scale;

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
				get
				{
					return new Vector3(this.scale);
				}
				set
				{
					this.ScaleTo(value.Xy);
				}
			}
			public override float Angle
			{
				get
				{
					return this.angle;
				}
				set
				{
					this.RotateTo(value);
				}
			}
			public override float BoundRadius
			{
				get { return this.boundRad * this.Body.GameObj.Transform.Scale; }
			}
			public override string DisplayObjectName
			{
				get { return PluginRes.EditorBaseRes.RigidBodyCamViewState_SelLoopShapeName; }
			}

			public SelLoopShape(LoopShapeInfo shape) : base(shape)
			{
				this.loop = shape;
				this.UpdateLoopStats();
			}

			public override string UpdateActionText(ObjectAction action, bool performing)
			{
				if (action == ObjectAction.Move)
				{
					return
						string.Format("Center X:{0,9:0.00}/n", this.center.X) +
						string.Format("Center Y:{0,9:0.00}", this.center.Y);
				}
				else if (action == ObjectAction.Scale)
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
				else if (action == ObjectAction.Rotate)
				{
					return string.Format("Angle:{0,6:0.0}°", MathF.RadToDeg(this.angle));
				}
				return base.UpdateActionText(action, performing);
			}

			public void UpdateLoopStats()
			{
				this.center = Vector2.Zero;
				for (int i = 0; i < this.loop.Vertices.Length; i++)
					this.center += this.loop.Vertices[i];
				this.center /= this.loop.Vertices.Length;

				this.scale = Vector2.Zero;
				for (int i = 0; i < this.loop.Vertices.Length; i++)
				{
					this.scale.X = MathF.Max(this.scale.X, MathF.Abs(this.loop.Vertices[i].X - this.center.X));
					this.scale.Y = MathF.Max(this.scale.Y, MathF.Abs(this.loop.Vertices[i].Y - this.center.Y));
				}

				this.boundRad = 0.0f;
				for (int i = 0; i < this.loop.Vertices.Length; i++)
					this.boundRad = MathF.Max(this.boundRad, (this.loop.Vertices[i] - this.center).Length);

				this.angle = MathF.Angle(this.center.X, this.center.Y, this.loop.Vertices[0].X, this.loop.Vertices[0].Y);
			}
			private void MoveCenterTo(Vector2 newPos)
			{
				Vector2 mov = newPos - this.center;

				Vector2[] movedVertices = this.loop.Vertices.ToArray();
				for (int i = 0; i < movedVertices.Length; i++)
					movedVertices[i] += mov;

				this.loop.Vertices = movedVertices;
				this.UpdateLoopStats();
			}
			private void ScaleTo(Vector2 newScale)
			{
				Vector2 scaleRatio = newScale / this.scale;

				Vector2[] scaledVertices = this.loop.Vertices.ToArray();
				for (int i = 0; i < scaledVertices.Length; i++)
				{
					scaledVertices[i].X = (scaledVertices[i].X - this.center.X) * scaleRatio.X + this.center.X;
					scaledVertices[i].Y = (scaledVertices[i].Y - this.center.Y) * scaleRatio.Y + this.center.Y;
				}

				this.loop.Vertices = scaledVertices;
				this.UpdateLoopStats();
			}
			private void RotateTo(float newAngle)
			{
				float rot = newAngle - this.angle;

				Vector2[] rotatedVertices = this.loop.Vertices.ToArray();
				for (int i = 0; i < rotatedVertices.Length; i++)
					MathF.TransformCoord(ref rotatedVertices[i].X, ref rotatedVertices[i].Y, rot, 1.0f, this.center.X, this.center.Y);

				this.loop.Vertices = rotatedVertices;
				this.UpdateLoopStats();
			}
		}
		//public class SelEdgeShape : SelShape
		//{
		//    private	EdgeShapeInfo	edge;
		//    private	Vector2	center;
		//    private	float	boundRad;
		//    private	float	angle;
		//    private	Vector2	scale;

		//    public override Vector3 Pos
		//    {
		//        get
		//        {
		//            return this.Collider.GameObj.Transform.GetWorldPoint(new Vector3(this.center));
		//        }
		//        set
		//        {
		//            value.Z = this.Collider.GameObj.Transform.Pos.Z;
		//            this.MoveCenterTo(this.Collider.GameObj.Transform.GetLocalPoint(value).Xy);
		//        }
		//    }
		//    public override Vector3 Scale
		//    {
		//        get
		//        {
		//            return new Vector3(this.scale);
		//        }
		//        set
		//        {
		//            this.ScaleTo(value.Xy);
		//        }
		//    }
		//    public override float Angle
		//    {
		//        get
		//        {
		//            return this.angle;
		//        }
		//        set
		//        {
		//            this.RotateTo(value);
		//        }
		//    }
		//    public override float BoundRadius
		//    {
		//        get { return this.boundRad * this.Collider.GameObj.Transform.Scale.Xy.Length / MathF.Sqrt(2.0f); }
		//    }

		//    public SelEdgeShape(EdgeShapeInfo shape) : base(shape)
		//    {
		//        this.edge = shape;
		//        this.UpdateEdgeStats();
		//    }

		//    public override void DrawActionGizmo(Canvas canvas, MouseAction action, Point beginLoc, Point curLoc)
		//    {
		//        base.DrawActionGizmo(canvas, action, beginLoc, curLoc);
		//        if (action == MouseAction.MoveObj)
		//        {
		//            canvas.DrawText(string.Format("Center X:{0,7:0.00}", this.center.X), curLoc.X + 30, curLoc.Y + 10);
		//            canvas.DrawText(string.Format("Center Y:{0,7:0.00}", this.center.Y), curLoc.X + 30, curLoc.Y + 18);
		//        }
		//        else if (action == MouseAction.ScaleObj)
		//        {
		//            if (MathF.Abs(this.scale.X - this.scale.Y) >= 0.01f)
		//            {
		//                canvas.DrawText(string.Format("Scale X:{0,7:0.00}", this.scale.X), curLoc.X + 30, curLoc.Y + 10);
		//                canvas.DrawText(string.Format("Scale Y:{0,7:0.00}", this.scale.Y), curLoc.X + 30, curLoc.Y + 18);
		//            }
		//            else
		//            {
		//                canvas.DrawText(string.Format("Scale:{0,7:0.00}", this.scale.X), curLoc.X + 30, curLoc.Y + 10);
		//            }
		//        }
		//        else if (action == MouseAction.RotateObj)
		//        {
		//            canvas.DrawText(string.Format("Angle:{0,6:0.0}", MathF.RadToDeg(this.angle)), curLoc.X + 30, curLoc.Y + 10);
		//        }
		//    }

		//    public void UpdateEdgeStats()
		//    {
		//        Vector2 connection = this.edge.VertexEnd - this.edge.VertexStart;
		//        this.center = (this.edge.VertexStart + this.edge.VertexEnd) * 0.5f;

		//        this.scale.X = MathF.Abs(connection.X);
		//        this.scale.Y = MathF.Abs(connection.Y);

		//        this.boundRad = connection.Length * 0.5f;

		//        this.angle = connection.PerpendicularLeft.Angle;
		//    }
		//    private void MoveCenterTo(Vector2 newPos)
		//    {
		//        Vector2 mov = newPos - this.center;

		//        this.edge.VertexStart += mov;
		//        this.edge.VertexEnd += mov;

		//        this.UpdateEdgeStats();
		//    }
		//    private void ScaleTo(Vector2 newScale)
		//    {
		//        Vector2 scaleRatio = newScale / this.scale;

		//        this.edge.VertexStart = (this.edge.VertexStart - this.center) * scaleRatio + this.center;
		//        this.edge.VertexEnd = (this.edge.VertexEnd - this.center) * scaleRatio + this.center;

		//        this.UpdateEdgeStats();
		//    }
		//    private void RotateTo(float newAngle)
		//    {
		//        float rot = newAngle - this.angle;

		//        Vector2 temp;
		//        temp = this.edge.VertexStart; MathF.TransformCoord(ref temp.X, ref temp.Y, rot, 1.0f, this.center.X, this.center.Y); this.edge.VertexStart = temp;
		//        temp = this.edge.VertexEnd; MathF.TransformCoord(ref temp.X, ref temp.Y, rot, 1.0f, this.center.X, this.center.Y); this.edge.VertexEnd = temp;

		//        this.UpdateEdgeStats();
		//    }
		//}
	}
}
