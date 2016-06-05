using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public partial class RigidBodyEditorCamViewState
	{
		public class SelBody : SelObj
		{
			private GameObject bodyObj;

			public override object ActualObject
			{
				get { return this.bodyObj == null || this.bodyObj.Disposed ? null : this.bodyObj; }
			}
			public override string DisplayObjectName
			{
				get { return Properties.CamViewRes.RigidBodyCamViewState_SelBodyName; }
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
					ICmpRenderer r = this.bodyObj.GetComponent<ICmpRenderer>();
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
			private ShapeInfo shape;
			
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
				get { return Properties.CamViewRes.RigidBodyCamViewState_SelShapeName; }
			}
			public RigidBody Body
			{
				get { return this.shape.Parent; }
			}
			public ShapeInfo Shape
			{
				get { return this.shape; }
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
			public virtual void UpdateShapeStats() { }

			public static SelShape Create(ShapeInfo shape)
			{
				if      (shape is CircleShapeInfo) return new SelCircleShape(shape as CircleShapeInfo);
				else if (shape is PolyShapeInfo  ) return new SelPolyShape  (shape as PolyShapeInfo  );
				else if (shape is ChainShapeInfo ) return new SelChainShape (shape as ChainShapeInfo );
				else if (shape is LoopShapeInfo  ) return new SelLoopShape  (shape as LoopShapeInfo  );
				else                               return null;
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
				get { return Properties.CamViewRes.RigidBodyCamViewState_SelCircleShapeName; }
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
		public abstract class SelPolyLikeShape : SelShape
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
				get { return Properties.CamViewRes.RigidBodyCamViewState_SelPolyShapeName; }
			}
			protected abstract Vector2[] Vertices { get; set; }

			public SelPolyLikeShape(ShapeInfo shape) : base(shape)
			{
				this.UpdateShapeStats();
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

			public override void UpdateShapeStats()
			{
				this.center = Vector2.Zero;
				for (int i = 0; i < this.Vertices.Length; i++)
					this.center += this.Vertices[i];
				this.center /= this.Vertices.Length;

				this.scale = Vector2.Zero;
				for (int i = 0; i < this.Vertices.Length; i++)
				{
					this.scale.X = MathF.Max(this.scale.X, MathF.Abs(this.Vertices[i].X - this.center.X));
					this.scale.Y = MathF.Max(this.scale.Y, MathF.Abs(this.Vertices[i].Y - this.center.Y));
				}

				this.boundRad = 0.0f;
				for (int i = 0; i < this.Vertices.Length; i++)
					this.boundRad = MathF.Max(this.boundRad, (this.Vertices[i] - this.center).Length);

				this.angle = MathF.Angle(this.center.X, this.center.Y, this.Vertices[0].X, this.Vertices[0].Y);
			}
			private void MoveCenterTo(Vector2 newPos)
			{
				Vector2 mov = newPos - this.center;

				Vector2[] movedVertices = this.Vertices.ToArray();
				for (int i = 0; i < movedVertices.Length; i++)
					movedVertices[i] += mov;

				this.Vertices = movedVertices;
				this.UpdateShapeStats();
			}
			private void ScaleTo(Vector2 newScale)
			{
				Vector2 scaleRatio = newScale / this.scale;

				Vector2[] scaledVertices = this.Vertices.ToArray();
				for (int i = 0; i < scaledVertices.Length; i++)
				{
					scaledVertices[i].X = (scaledVertices[i].X - this.center.X) * scaleRatio.X + this.center.X;
					scaledVertices[i].Y = (scaledVertices[i].Y - this.center.Y) * scaleRatio.Y + this.center.Y;
				}

				this.Vertices = scaledVertices;
				this.UpdateShapeStats();
			}
			private void RotateTo(float newAngle)
			{
				float rot = newAngle - this.angle;

				Vector2[] rotatedVertices = this.Vertices.ToArray();
				for (int i = 0; i < rotatedVertices.Length; i++)
					MathF.TransformCoord(ref rotatedVertices[i].X, ref rotatedVertices[i].Y, rot, 1.0f, this.center.X, this.center.Y);

				this.Vertices = rotatedVertices;
				this.UpdateShapeStats();
			}
		}
		public class SelPolyShape : SelPolyLikeShape
		{
			protected override Vector2[] Vertices
			{
				get { return (this.Shape as PolyShapeInfo).Vertices; }
				set { (this.Shape as PolyShapeInfo).Vertices = value; }
			}
			public SelPolyShape(PolyShapeInfo shape) : base(shape) { }
		}
		public class SelLoopShape : SelPolyLikeShape
		{
			protected override Vector2[] Vertices
			{
				get { return (this.Shape as LoopShapeInfo).Vertices; }
				set { (this.Shape as LoopShapeInfo).Vertices = value; }
			}
			public SelLoopShape(LoopShapeInfo shape) : base(shape) { }
		}
		public class SelChainShape : SelPolyLikeShape
		{
			protected override Vector2[] Vertices
			{
				get { return (this.Shape as ChainShapeInfo).Vertices; }
				set { (this.Shape as ChainShapeInfo).Vertices = value; }
			}
			public SelChainShape(ChainShapeInfo shape) : base(shape) { }
		}
	}
}
