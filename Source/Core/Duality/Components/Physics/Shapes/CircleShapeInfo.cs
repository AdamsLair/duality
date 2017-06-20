using System;
using System.Collections.Generic;
using System.Linq;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

using Duality.Editor;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Describes a <see cref="RigidBody">Colliders</see> circle shape.
	/// </summary>
	public sealed class CircleShapeInfo : ShapeInfo
	{
		[DontSerialize]
		private Fixture fixture;
		private float   radius;
		private Vector2 position;


		/// <summary>
		/// [GET / SET] The circles radius.
		/// </summary>
		[EditorHintIncrement(1)]
		[EditorHintDecimalPlaces(1)]
		public float Radius
		{
			get { return this.radius; }
			set { this.radius = value; this.UpdateInternalShape(true); }
		}
		/// <summary>
		/// [GET / SET] The circles position.
		/// </summary>
		[EditorHintIncrement(1)]
		[EditorHintDecimalPlaces(1)]
		public Vector2 Position
		{
			get { return this.position; }
			set { this.position = value; this.UpdateInternalShape(true); }
		}
		public override Rect AABB
		{
			get { return Rect.Align(Alignment.Center, position.X, position.Y, radius * 2, radius * 2); }
		}
		protected override bool IsInternalShapeCreated
		{
			get { return this.fixture != null; }
		}


		public CircleShapeInfo() {}
		public CircleShapeInfo(float radius, Vector2 position, float density)
		{
			this.radius = radius;
			this.position = position;
			this.density = density;
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
			if (!this.EnsureFixtures()) return;

			this.fixture.IsSensor = this.sensor;
			this.fixture.Restitution = this.restitution;
			this.fixture.Friction = this.friction;

			CircleShape circle = this.fixture.Shape as CircleShape;
			circle.Density = this.density * PhysicsUnit.DensityToPhysical / (10.0f * 10.0f);
		}

		private bool EnsureFixtures()
		{
			if (this.fixture == null)
			{
				Body body = this.Parent.PhysicsBody;
				if (body != null)
				{
					float scale = this.ParentScale;
					CircleShape circle = new CircleShape(
						PhysicsUnit.LengthToPhysical * this.radius * scale, 
						this.density);
					circle.Position = PhysicsUnit.LengthToPhysical * this.position * scale;

					this.fixture = new Fixture(
						body, 
						circle, 
						this);
				}
			}

			return this.fixture != null;
		}
	}
}
