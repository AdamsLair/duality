using System;
using System.Collections.Generic;
using System.Linq;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

using Duality.Editor;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Describes a double-sided edge loop (outline) in a <see cref="RigidBody"/> shape.
	/// </summary>
	public sealed class LoopShapeInfo : ShapeInfo
	{
		private Vector2[] vertices;

		/// <summary>
		/// [GET / SET] The edge loops vertices. While assinging the array will cause an automatic update, simply modifying it will require you to call <see cref="ShapeInfo.UpdateShape"/> manually.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		[EditorHintIncrement(1)]
		[EditorHintDecimalPlaces(1)]
		public Vector2[] Vertices
		{
			get { return this.vertices; }
			set
			{
				this.vertices = value ?? new Vector2[] { Vector2.Zero, Vector2.UnitX, Vector2.UnitY };
				this.UpdateFixture(true);
			}
		}
		[EditorHintFlags(MemberFlags.Invisible)]
		public override Rect AABB
		{
			get 
			{
				float minX = float.MaxValue;
				float minY = float.MaxValue;
				float maxX = float.MinValue;
				float maxY = float.MinValue;
				for (int i = 0; i < this.vertices.Length; i++)
				{
					minX = MathF.Min(minX, this.vertices[i].X);
					minY = MathF.Min(minY, this.vertices[i].Y);
					maxX = MathF.Max(maxX, this.vertices[i].X);
					maxY = MathF.Max(maxY, this.vertices[i].Y);
				}
				return new Rect(minX, minY, maxX - minX, maxY - minY);
			}
		}
			
		public LoopShapeInfo() {}
		public LoopShapeInfo(IEnumerable<Vector2> vertices) : base(1.0f)
		{
			this.vertices = vertices.ToArray();
		}

		protected override Fixture CreateFixture(Body body)
		{
			if (!body.IsStatic) return null; // Loop shapes aren't allowed on nonstatic bodies.
			if (this.vertices == null || this.vertices.Length < 3) return null;

			this.Parent.CheckValidTransform();

			ChainShape shape = new ChainShape();
			this.UpdateVertices(shape, 1.0f);
			Fixture f = body.CreateFixture(shape, this);

			this.Parent.CheckValidTransform();
			return f;
		}
		internal override void UpdateFixture(bool updateShape = false)
		{
			// Loop / Chain shapes aren't allowed on nonstatic bodies.
			if (this.Parent != null && this.Parent.BodyType != BodyType.Static)
			{
				this.DestroyFixture(this.Parent.PhysicsBody, false);
				return;
			}

			base.UpdateFixture(updateShape);
			if (this.fixture == null) return;
			if (this.Parent == null) return;
				
			float scale = 1.0f;
			if (this.Parent.GameObj != null && this.Parent.GameObj.Transform != null)
				scale = this.Parent.GameObj.Transform.Scale;

			ChainShape shape = this.fixture.Shape as ChainShape;
			this.UpdateVertices(shape, scale);
		}
		private void UpdateVertices(ChainShape shape, float scale)
		{
			if (this.vertices == null || this.vertices.Length < 3) return;

			if (shape.Vertices == null)
				shape.Vertices = new Vertices();
			else
				shape.Vertices.Clear();

			for (int i = 0; i < this.vertices.Length; i++)
			{
				shape.Vertices.Add(PhysicsUnit.LengthToPhysical * this.vertices[i] * scale);
			}
			shape.MakeLoop();
		}
	}
}
