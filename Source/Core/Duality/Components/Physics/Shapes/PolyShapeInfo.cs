using System;
using System.Collections.Generic;
using System.Linq;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common;

using Duality.Editor;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Describes a <see cref="RigidBody">Colliders</see> polygon shape.
	/// </summary>
	public sealed class PolyShapeInfo : ShapeInfo
	{
		public const int MaxVertices = FarseerPhysics.Settings.MaxPolygonVertices;
		
		[DontSerialize]
		private Fixture fixture;
		private Vector2[] vertices;


		/// <summary>
		/// [GET / SET] The polygons vertices. While assinging the array will cause an automatic update, simply modifying it will require you to call <see cref="ShapeInfo.UpdateShape"/> manually.
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

			
		public PolyShapeInfo() {}
		public PolyShapeInfo(IEnumerable<Vector2> vertices, float density)
		{
			this.vertices = vertices.ToArray();
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

			float scale = this.ParentScale;
			
			this.fixture.IsSensor = this.sensor;
			this.fixture.Restitution = this.restitution;
			this.fixture.Friction = this.friction;
			
			PolygonShape shape = this.fixture.Shape as PolygonShape;
			this.UpdateVertices(shape.Vertices, scale);
			shape.Density = this.density;
			shape.Set(shape.Vertices);
		}

		private bool EnsureFixtures()
		{
			if (this.vertices == null || this.vertices.Length < 3) return false;
			if (this.vertices.Length > MaxVertices) return false;

			if (this.fixture == null)
			{
				Body body = this.Parent.PhysicsBody;
				if (body != null)
				{
					Vertices shapeVertices = new Vertices(this.vertices.Length);
					this.UpdateVertices(shapeVertices, this.ParentScale);
					if (!shapeVertices.IsConvex()) return false;

					this.fixture = new Fixture(
						body, 
						new PolygonShape(shapeVertices, this.density), 
						this);
				}
			}

			return this.fixture != null;
		}
		private void UpdateVertices(Vertices targetVertices, float scale)
		{
			targetVertices.Clear();

			// Translate input polygon into Farseer space and API
			VerticesToFarseer(this.vertices, scale, targetVertices);

			// Enforce counter-clockwise order of vertices
			targetVertices.ForceCounterClockWise();
		}
	}
}
