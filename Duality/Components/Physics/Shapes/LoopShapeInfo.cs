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
	/// Describes a double-sided edge loop (outline) in a <see cref="RigidBody">RigidBodies</see> shape.
	/// </summary>
	[Serializable]
	public sealed class LoopShapeInfo : ShapeInfo
	{
		private	Vector2[]	vertices;

		/// <summary>
		/// [GET / SET] The edge loops vertices. While assinging the array will cause an automatic update, simply modifying it will require you to call <see cref="UpdateShape"/> manually.
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
			var farseerVert = this.CreateVertices(1.0f);
			if (farseerVert == null) return null;

			this.Parent.CheckValidTransform();

			Fixture f = body.CreateFixture(new LoopShape(farseerVert), this);

			this.Parent.CheckValidTransform();
			return f;
		}
		internal override void UpdateFixture(bool updateShape = false)
		{
			// Loop shapes aren't allowed on nonstatic bodies.
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

			LoopShape poly = this.fixture.Shape as LoopShape;
			poly.Vertices = this.CreateVertices(scale);
		}
		private FarseerPhysics.Common.Vertices CreateVertices(float scale)
		{
			if (this.vertices == null || this.vertices.Length < 3) return null;
			Vector2[] vertices = this.vertices.ToArray();

			FarseerPhysics.Common.Vertices farseerVert = new FarseerPhysics.Common.Vertices(vertices.Length);
			for (int i = 0; i < vertices.Length; i++)
			{
				farseerVert.Add(new Vector2(
					PhysicsConvert.ToPhysicalUnit(vertices[i].X * scale), 
					PhysicsConvert.ToPhysicalUnit(vertices[i].Y * scale)));
			}
			return farseerVert;
		}

		protected override void OnCopyTo(ShapeInfo target)
		{
			base.OnCopyTo(target);
			LoopShapeInfo c = target as LoopShapeInfo;
			c.vertices = this.vertices != null ? (Vector2[])this.vertices.Clone() : null;
		}
	}
}
