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
	public sealed class LoopShapeInfo : VertexBasedShapeInfo
	{
		[DontSerialize]
		private Fixture fixture;


		/// <inheritdoc />
		public override VertexShapeTrait ShapeTraits
		{
			get { return VertexShapeTrait.IsLoop; }
		}
		protected override bool IsInternalShapeCreated
		{
			get { return this.fixture != null; }
		}

		
		/// <summary>
		/// Creates a new, empty loop shape.
		/// </summary>
		public LoopShapeInfo() {}
		/// <summary>
		/// Creates a new loop shape. Note that it will assume ownership of
		/// the specified vertex array, so no copy will be made.
		/// </summary>
		/// <param name="vertices"></param>
		public LoopShapeInfo(Vector2[] vertices) : base(vertices) { }


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
			shape.Density = this.density * PhysicsUnit.DensityToPhysical;
		}

		private bool EnsureFixtures()
		{
			if (this.vertices == null || this.vertices.Length < 3) return false;

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

			shape.MakeLoop();
		}
	}
}
