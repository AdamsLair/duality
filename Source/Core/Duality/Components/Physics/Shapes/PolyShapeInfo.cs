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
			
		public PolyShapeInfo() {}
		public PolyShapeInfo(IEnumerable<Vector2> vertices, float density) : base(density)
		{
			this.vertices = vertices.ToArray();
		}

		protected override Fixture CreateFixture(Body body)
		{
			Vertices convexPolygon = this.CreateFarseerPolygon(this.vertices, 1.0f);
			if (convexPolygon == null) return null;

			this.Parent.CheckValidTransform();

			Fixture f = body.CreateFixture(new PolygonShape(convexPolygon, 1.0f), this);

			this.Parent.CheckValidTransform();
			return f;
		}
		internal override void UpdateFixture(bool updateShape = false)
		{
			base.UpdateFixture(updateShape);
			if (this.fixture == null) return;
			if (this.Parent == null) return;
				
			float scale = 1.0f;
			if (this.Parent.GameObj != null && this.Parent.GameObj.Transform != null)
				scale = this.Parent.GameObj.Transform.Scale;

			this.Parent.CheckValidTransform();

			Vertices convexPolygon = this.CreateFarseerPolygon(this.vertices, scale);
			if (convexPolygon != null)
			{
				PolygonShape poly = this.fixture.Shape as PolygonShape;
				poly.Set(convexPolygon);
			}

			this.Parent.CheckValidTransform();
		}

		private Vertices CreateFarseerPolygon(Vector2[] inputPolygon, float scale)
		{
			// Early-out, if there are no vertices to process, or more than are supported
			if (inputPolygon == null || inputPolygon.Length < 3 || inputPolygon.Length > MaxVertices)
				return null;

			// Translate input polygon into farseer space and API
			Vertices farseerPolygon = VerticesToFarseer(inputPolygon, scale);

			// Enforce counter-clockwise order of vertices
			farseerPolygon.ForceCounterClockWise();

			// Pass-through convex polygons
			if (!farseerPolygon.IsConvex())
				return null;

			return farseerPolygon;
		}

		private static Vector2[] VerticesToDuality(Vertices vertices)
		{
			Vector2[] transformed = new Vector2[vertices.Count];
			for (int i = 0; i < transformed.Length; i++)
				transformed[i] = PhysicsUnit.LengthToPhysical * vertices[i];
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
