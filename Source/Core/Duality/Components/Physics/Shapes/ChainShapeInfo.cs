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
	/// Describes a double-sided edge chain in a <see cref="RigidBody"/> shape.
	/// </summary>
	public sealed class ChainShapeInfo : ShapeInfo
	{
		[DontSerialize]
		private Fixture fixture;
		private Vector2[] vertices;


		/// <summary>
		/// [GET / SET] The edge chains vertices. While assinging the array will cause an automatic update, simply modifying it will require you to call <see cref="ShapeInfo.UpdateShape"/> manually.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		[EditorHintIncrement(1)]
		[EditorHintDecimalPlaces(1)]
		public Vector2[] Vertices
		{
			get { return this.vertices; }
			set
			{
				this.vertices = value ?? new Vector2[] { Vector2.Zero, Vector2.UnitX };
				this.UpdateInternalShape(true);
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
		protected override bool IsInternalShapeCreated
		{
			get { return this.fixture != null; }
		}

			
		public ChainShapeInfo() {}
		public ChainShapeInfo(IEnumerable<Vector2> vertices)
		{
			this.vertices = vertices.ToArray();
		}
		
		protected override void DestroyFixtures()
		{
			if (this.fixture == null) return;
			if (this.fixture.Body != null)
				this.fixture.Body.DestroyFixture(this.fixture);
			this.fixture = null;
		}
		protected override void SyncFixtures()
		{
			// This kind of shape is not allowed on non-static bodies.
			if (this.Parent.BodyType != BodyType.Static)
			{
				this.DestroyFixtures();
				return;
			}

			if (!this.EnsureFixtures()) return;

			this.fixture.IsSensor = this.sensor;
			this.fixture.Restitution = this.restitution;
			this.fixture.Friction = this.friction;
			
			ChainShape shape = this.fixture.Shape as ChainShape;
			this.UpdateVertices(shape, this.ParentScale);
			shape.Density = this.density;
		}

		private bool EnsureFixtures()
		{
			if (this.vertices == null || this.vertices.Length < 2) return false;

			if (this.fixture == null)
			{
				Body body = this.Parent.PhysicsBody;
				if (body != null)
				{
					if (!body.IsStatic) return false;

					ChainShape shape = new ChainShape();
					shape.Vertices = new Vertices();
					this.UpdateVertices(shape, this.ParentScale);

					this.fixture = new Fixture(
						body, 
						shape, 
						this);
				}
			}

			return this.fixture != null;
		}
		private void UpdateVertices(ChainShape shape, float scale)
		{
			shape.Vertices.Clear();

			for (int i = 0; i < this.vertices.Length; i++)
				shape.Vertices.Add(PhysicsUnit.LengthToPhysical * this.vertices[i] * scale);

			shape.MakeChain();
		}
	}
}
