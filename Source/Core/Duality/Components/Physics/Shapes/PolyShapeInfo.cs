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
		[DontSerialize]
		private List<Fixture> fixtures;
		private Vector2[] vertices;
		private List<Vector2[]> convexPolygons;


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
		/// <summary>
		/// [GET] A read-only list of convex polygons that were generated 
		/// from the shapes <see cref="Vertices"/>. Do not modify any of the
		/// returned values.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IReadOnlyList<Vector2[]> ConvexPolygons
		{
			get { return this.convexPolygons; }
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
			get { return this.fixtures != null && this.fixtures.Count > 0; }
		}

			
		public PolyShapeInfo() {}
		public PolyShapeInfo(IEnumerable<Vector2> vertices, float density)
		{
			this.vertices = vertices.ToArray();
			this.density = density;
		}

		protected override void DestroyFixtures()
		{
			if (this.convexPolygons != null)
				this.convexPolygons.Clear();

			if (this.fixtures != null)
			{
				foreach (Fixture fixture in this.fixtures)
				{
					if (fixture.Body != null)
						fixture.Body.DestroyFixture(fixture);
				}
				this.fixtures.Clear();
			}
		}
		protected override void SyncFixtures()
		{
			this.EnsureDecomposedPolygons();
			if (!this.EnsureFixtures()) return;

			foreach (Fixture fixture in this.fixtures)
			{
				fixture.IsSensor = this.sensor;
				fixture.Restitution = this.restitution;
				fixture.Friction = this.friction;
			
				PolygonShape shape = fixture.Shape as PolygonShape;
				shape.Density = this.density;
			}
		}

		private void EnsureDecomposedPolygons()
		{
			if (this.convexPolygons != null && this.convexPolygons.Count > 0) return;
			if (this.convexPolygons == null)
				this.convexPolygons = new List<Vector2[]>();

			// No valid polygon defined at all: Nothing to generate.
			if (this.vertices == null || this.vertices.Length < 3) return;

			Vertices fullPolygon = VerticesToFarseer(this.vertices, 1.0f);

			// Do not allow neighbor vertices that are too close to each other.
			for (int i = 1; i < fullPolygon.Count; i++)
			{
				float distance = (fullPolygon[i - 1] - fullPolygon[i]).Length;
				if (distance < 0.01f) return;
			}

			// Discard non-simple and micro area polygons early, as there
			// is nothing that decomposition can do in this case.
			if (!fullPolygon.IsSimple()) return;
			if (fullPolygon.GetArea() < 0.0001f) return;

			// If the polygon is small enough and convex, use it as-is.
			if (this.vertices.Length <= FarseerPhysics.Settings.MaxPolygonVertices)
			{
				fullPolygon.ForceCounterClockWise();
				if (fullPolygon.IsConvex())
				{
					this.convexPolygons.Add(VerticesToDuality(fullPolygon));
					return;
				}
			}

			// Decompose non-convex polygons and save them persistently,
			// so we don't need to decompose them again unless modified.
			List<Vertices> decomposed = Triangulate.ConvexPartition(fullPolygon, TriangulationAlgorithm.Delauny);
			foreach (Vertices polygon in decomposed)
			{
				this.convexPolygons.Add(VerticesToDuality(polygon));
			}
		}
		private bool EnsureFixtures()
		{
			if (this.convexPolygons == null || this.convexPolygons.Count == 0) return false;

			if (this.fixtures == null)
				this.fixtures = new List<Fixture>();

			if (this.fixtures.Count == 0)
			{
				Body body = this.Parent.PhysicsBody;
				if (body != null)
				{
					this.Parent.CheckValidTransform();
					float scale = this.ParentScale;
					foreach (Vector2[] polygon in this.convexPolygons)
					{
						Vertices farseerPolygon = VerticesToFarseer(polygon, scale);
						Fixture fixture = new Fixture(
							body, 
							new PolygonShape(farseerPolygon, this.density), 
							this);
						this.fixtures.Add(fixture);
						this.Parent.CheckValidTransform();
					}
				}
			}

			return this.fixtures.Count > 0;
		}

		private static Vector2[] VerticesToDuality(Vertices vertices)
		{
			Vector2[] transformed = new Vector2[vertices.Count];
			for (int i = 0; i < transformed.Length; i++)
				transformed[i] = PhysicsUnit.LengthToDuality * vertices[i];
			return transformed;
		}
		private static Vertices VerticesToFarseer(Vector2[] vertices, float scale)
		{
			Vertices transformed = new Vertices(vertices.Length);
			for (int i = 0; i < vertices.Length; i++)
				transformed.Add(PhysicsUnit.LengthToPhysical * vertices[i] * scale);
			return transformed;
		}
	}
}
