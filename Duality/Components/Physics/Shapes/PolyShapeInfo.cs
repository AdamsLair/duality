using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

using Duality.EditorHints;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Describes a <see cref="RigidBody">Colliders</see> polygon shape.
	/// </summary>
	[Serializable]
	public sealed class PolyShapeInfo : ShapeInfo
	{
		public const int MaxVertices = FarseerPhysics.Settings.MaxPolygonVertices;

		private	Vector2[]	vertices;

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
			var farseerVert = this.CreateVertices(1.0f);
			if (farseerVert == null) return null;

			this.Parent.CheckValidTransform();

			Fixture f = body.CreateFixture(new PolygonShape(farseerVert, 1.0f), this);

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

			PolygonShape poly = this.fixture.Shape as PolygonShape;
			poly.Set(this.CreateVertices(scale));

			this.Parent.CheckValidTransform();
		}
		private FarseerPhysics.Common.Vertices CreateVertices(float scale)
		{
			if (this.vertices == null || this.vertices.Length < 3) return null;
			if (!MathF.IsPolygonConvex(this.vertices)) return null;
			
			// Be sure to not exceed the maximum vertex count
			Vector2[] sortedVertices = this.vertices.ToArray();
			if (sortedVertices.Length > MaxVertices)
			{
				Array.Resize(ref sortedVertices, MaxVertices);
				Log.Core.WriteWarning("Maximum Polygon Shape vertex count exceeded: {0} > {1}", this.vertices.Length, MaxVertices);
			}

			// Don't let all vertices be aligned on one axis (zero-area polygons)
			if (sortedVertices.Length > 0)
			{
				Vector2 firstVertex = sortedVertices[0];
				bool alignX = true;
				bool alignY = true;
				for (int i = 0; i < sortedVertices.Length; i++)
				{
					if (sortedVertices[i].X != firstVertex.X)
						alignX = false;
					if (sortedVertices[i].Y != firstVertex.Y)
						alignY = false;
					if (!alignX && !alignY)
						break;
				}
				if (alignX) sortedVertices[0].X += 0.01f;
				if (alignY) sortedVertices[0].Y += 0.01f;
			}

			// Sort vertices clockwise before submitting them to Farseer
			Vector2 centroid = Vector2.Zero;
			for (int i = 0; i < sortedVertices.Length; i++)
				centroid += sortedVertices[i];
			centroid /= sortedVertices.Length;
			sortedVertices.StableSort(delegate(Vector2 first, Vector2 second)
			{
				return MathF.RoundToInt(
					1000000.0f * MathF.Angle(centroid.X, centroid.Y, first.X, first.Y) - 
					1000000.0f * MathF.Angle(centroid.X, centroid.Y, second.X, second.Y));
			});

			// Shrink a little bit
			//for (int i = 0; i < sortedVertices.Length; i++)
			//{
			//    Vector2 rel = (sortedVertices[i] - centroid);
			//    float len = rel.Length;
			//    sortedVertices[i] = centroid + rel.Normalized * MathF.Max(0.0f, len - 1.5f);
			//}

			// Submit vertices
			FarseerPhysics.Common.Vertices v = new FarseerPhysics.Common.Vertices(sortedVertices.Length);
			for (int i = 0; i < sortedVertices.Length; i++)
			{
				v.Add(new Vector2(
					PhysicsConvert.ToPhysicalUnit(sortedVertices[i].X * scale), 
					PhysicsConvert.ToPhysicalUnit(sortedVertices[i].Y * scale)));
			}
			return v;
		}

		protected override void OnCopyTo(ShapeInfo target)
		{
			base.OnCopyTo(target);
			PolyShapeInfo c = target as PolyShapeInfo;
			c.vertices = this.vertices != null ? (Vector2[])this.vertices.Clone() : null;
		}
	}
}
