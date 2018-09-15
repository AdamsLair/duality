/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System.Diagnostics;
using FarseerPhysics.Common;
using Duality;

namespace FarseerPhysics.Collision.Shapes
{
	/// <summary>
	/// A chain shape is a free form sequence of line segments.
	/// The chain may cross upon itself, but this is not recommended for smooth collision.
	/// The chain has double sided collision, so you can use inside and outside collision.
	/// Therefore, you may use any winding order.
	/// </summary>
	public class ChainShape : Shape
	{
		private static EdgeShape _edgeShape = new EdgeShape();

		/// <summary>
		/// The vertices. These are not owned/freed by the loop Shape.
		/// </summary>
		public Vertices Vertices;
		public Vector2 PrevVertex;
		public Vector2 NextVertex;
		public bool HasPrevVertex;
		public bool HasNextVertex;

		public override int ChildCount
		{
			get { return this.Vertices.Count - 1; }
		}

		public ChainShape()
			: base(0)
		{
			this.ShapeType = ShapeType.Chain;
			this._radius = Settings.PolygonRadius;
		}
		/// <summary>
		/// Create a new chainshape from the vertices.
		/// </summary>
		/// <param name="vertices">The vertices to use. Must contain 2 or more vertices.</param>
		/// <param name="createLoop">Set to true to create a closed loop. It connects the first vertice to the last, and automatically adjusts connectivity to create smooth collisions along the chain.</param>
		public ChainShape(Vertices vertices, bool createLoop) : this()
		{
			Debug.Assert(vertices != null && vertices.Count >= 3);

			this.Vertices = new Vertices(vertices);

			if (createLoop)
				this.MakeLoop();
		}

		public override Shape Clone()
		{
			ChainShape clone = new ChainShape();
			clone._density = this._density;
			clone._radius = this._radius;
			clone.PrevVertex = this.PrevVertex;
			clone.NextVertex = this.NextVertex;
			clone.HasNextVertex = this.HasNextVertex;
			clone.HasPrevVertex = this.HasPrevVertex;
			clone.Vertices = this.Vertices;
			clone.MassData = this.MassData;
			return clone;
		}

		/// <summary>
		/// Adjusts the shapes previous and next vertex settings in order
		/// to form a loop shape. This requires the first vertex to equal
		/// the last one. If this is not the case, a new vertex will be
		/// added to close the loop.
		/// </summary>
		public void MakeLoop()
		{
			// Close the loop, if this is not done yet
			if (this.Vertices[0] != this.Vertices[this.Vertices.Count - 1])
			{
				this.Vertices.Add(this.Vertices[0]);
			}

			this.PrevVertex = this.Vertices[this.Vertices.Count - 2]; //FPE: We use the properties instead of the private fields here.
			this.NextVertex = this.Vertices[1]; //FPE: We use the properties instead of the private fields here.
			this.HasPrevVertex = true;
			this.HasNextVertex = true;
		}
		/// <summary>
		/// Adjusts the shapes previous and next vertex settings in order
		/// to form a chain shape. This requires the first vertex to not 
		/// equal the last one. If this is the case, the last vertex will be
		/// removed to break the loop.
		/// </summary>
		public void MakeChain()
		{
			// Break the loop, if required
			if (this.Vertices[0] == this.Vertices[this.Vertices.Count - 1])
			{
				this.Vertices.RemoveAt(this.Vertices.Count - 1);
			}

			this.HasPrevVertex = false;
			this.HasNextVertex = false;
		}

		public void GetChildEdge(ref EdgeShape edge, int index)
		{
			Debug.Assert(2 <= this.Vertices.Count);
			Debug.Assert(0 <= index && index < this.Vertices.Count - 1);
			edge.ShapeType = ShapeType.Edge;
			edge._radius = this._radius;

			edge.Vertex1 = this.Vertices[index + 0];
			edge.Vertex2 = this.Vertices[index + 1];

			if (index > 0)
			{
				edge.Vertex0 = this.Vertices[index - 1];
				edge.HasVertex0 = true;
			}
			else
			{
				edge.Vertex0 = this.PrevVertex;
				edge.HasVertex0 = this.HasPrevVertex;
			}

			if (index < this.Vertices.Count - 2)
			{
				edge.Vertex3 = this.Vertices[index + 2];
				edge.HasVertex3 = true;
			}
			else
			{
				edge.Vertex3 = this.NextVertex;
				edge.HasVertex3 = this.HasNextVertex;
			}

			// Old hack-fix for jittery collision at sharp (<90°) angles.
			// See here: https://github.com/AdamsLair/duality/commit/924b25119a6634c77e71175e7f275db3d3d4e9dd
			edge.HasVertex0 = edge.HasVertex0 && Vector2.AngleBetween(edge.Vertex0, edge.Vertex1) > MathHelper.PiOver2;
			edge.HasVertex3 = edge.HasVertex3 && Vector2.AngleBetween(edge.Vertex2, edge.Vertex3) > MathHelper.PiOver2;
		}

		/// <summary>
		/// Test a point for containment in this shape. This only works for convex shapes.
		/// </summary>
		/// <param name="transform">The shape world transform.</param>
		/// <param name="point">a point in world coordinates.</param>
		/// <returns>True if the point is inside the shape</returns>
		public override bool TestPoint(ref Transform transform, ref Vector2 point)
		{
			return false;
		}
		/// <summary>
		/// Cast a ray against a child shape.
		/// </summary>
		/// <param name="output">The ray-cast results.</param>
		/// <param name="input">The ray-cast input parameters.</param>
		/// <param name="transform">The transform to be applied to the shape.</param>
		/// <param name="childIndex">The child shape index.</param>
		/// <returns>True if the ray-cast hits the shape</returns>
		public override bool RayCast(out RayCastOutput output, ref RayCastInput input,
									 ref Transform transform, int childIndex)
		{
			Debug.Assert(childIndex < this.Vertices.Count);

			int i1 = childIndex;
			int i2 = childIndex + 1;
			if (i2 == this.Vertices.Count)
			{
				i2 = 0;
			}

			_edgeShape.Vertex1 = this.Vertices[i1];
			_edgeShape.Vertex2 = this.Vertices[i2];

			return _edgeShape.RayCast(out output, ref input, ref transform, 0);
		}
		/// <summary>
		/// Given a transform, compute the associated axis aligned bounding box for a child shape.
		/// </summary>
		/// <param name="aabb">The aabb results.</param>
		/// <param name="transform">The world transform of the shape.</param>
		/// <param name="childIndex">The child shape index.</param>
		public override void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex)
		{
			Debug.Assert(childIndex < this.Vertices.Count);

			int i1 = childIndex;
			int i2 = childIndex + 1;
			if (i2 == this.Vertices.Count)
			{
				i2 = 0;
			}

			Vector2 v1 = MathUtils.Multiply(ref transform, this.Vertices[i1]);
			Vector2 v2 = MathUtils.Multiply(ref transform, this.Vertices[i2]);

			aabb.LowerBound = Vector2.Min(v1, v2);
			aabb.UpperBound = Vector2.Max(v1, v2);
		}
		/// <summary>
		/// Chains have zero mass.
		/// </summary>
		public override void ComputeProperties()
		{
			// Does nothing. Loop shapes don't have properties.
		}
		public override float ComputeSubmergedArea(Vector2 normal, float offset, Transform xf, out Vector2 sc)
		{
			sc = Vector2.Zero;
			return 0;
		}
	}
}