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
using FarseerPhysics.Common.Decomposition;
using Duality;

namespace FarseerPhysics.Collision.Shapes
{
	/// <summary>
	/// Represents a simple non-selfintersecting convex polygon.
	/// If you want to have concave polygons, you will have to use the <see cref="BayazitDecomposer"/> or the <see cref="EarclipDecomposer"/>
	/// to decompose the concave polygon into 2 or more convex polygons.
	/// </summary>
	public class PolygonShape : Shape
	{
		public Vertices Normals;
		public Vertices Vertices;

		/// <summary>
		/// Initializes a new instance of the <see cref="PolygonShape"/> class.
		/// </summary>
		/// <param name="vertices">The vertices.</param>
		/// <param name="density">The density.</param>
		public PolygonShape(Vertices vertices, float density)
			: base(density)
		{
			this.ShapeType = ShapeType.Polygon;
			this._radius = Settings.PolygonRadius;

			Set(vertices);
		}

		public PolygonShape(float density)
			: base(density)
		{
			this.ShapeType = ShapeType.Polygon;
			this._radius = Settings.PolygonRadius;
			this.Normals = new Vertices();
			this.Vertices = new Vertices();
		}

		internal PolygonShape()
			: base(0)
		{
			this.ShapeType = ShapeType.Polygon;
			this._radius = Settings.PolygonRadius;
			this.Normals = new Vertices();
			this.Vertices = new Vertices();
		}

		public override int ChildCount
		{
			get { return 1; }
		}

		public override Shape Clone()
		{
			PolygonShape clone = new PolygonShape();
			clone.ShapeType = this.ShapeType;
			clone._radius = this._radius;
			clone._density = this._density;

			if (Settings.ConserveMemory)
			{
				clone.Vertices = this.Vertices;
				clone.Normals = this.Normals;
			}
			else
			{
				clone.Vertices = new Vertices(this.Vertices);
				clone.Normals = new Vertices(this.Normals);
			}

			clone.MassData = this.MassData;
			return clone;
		}

		/// <summary>
		/// Copy vertices. This assumes the vertices define a convex polygon.
		/// It is assumed that the exterior is the the right of each edge.
		/// </summary>
		/// <param name="vertices">The vertices.</param>
		public void Set(Vertices vertices)
		{
			Debug.Assert(vertices.Count >= 3 && vertices.Count <= Settings.MaxPolygonVertices);

			if (Settings.ConserveMemory)
				this.Vertices = vertices;
			else
				// Copy vertices.
				this.Vertices = new Vertices(vertices);

			this.Normals = new Vertices(vertices.Count);

			// Compute normals. Ensure the edges have non-zero length.
			for (int i = 0; i < vertices.Count; ++i)
			{
				int i1 = i;
				int i2 = i + 1 < vertices.Count ? i + 1 : 0;
				Vector2 edge = this.Vertices[i2] - this.Vertices[i1];
				Debug.Assert(edge.LengthSquared > Settings.Epsilon * Settings.Epsilon);

				Vector2 temp = new Vector2(edge.Y, -edge.X);
				temp.Normalize();
				this.Normals.Add(temp);
			}

			// Compute the polygon mass data
			ComputeProperties();
		}

		/// <summary>
		/// Compute the mass properties of this shape using its dimensions and density.
		/// The inertia tensor is computed about the local origin, not the centroid.
		/// </summary>
		public override void ComputeProperties()
		{
			// Polygon mass, centroid, and inertia.
			// Let rho be the polygon density in mass per unit area.
			// Then:
			// mass = rho * int(dA)
			// centroid.X = (1/mass) * rho * int(x * dA)
			// centroid.Y = (1/mass) * rho * int(y * dA)
			// I = rho * int((x*x + y*y) * dA)
			//
			// We can compute these integrals by summing all the integrals
			// for each triangle of the polygon. To evaluate the integral
			// for a single triangle, we make a change of variables to
			// the (u,v) coordinates of the triangle:
			// x = x0 + e1x * u + e2x * v
			// y = y0 + e1y * u + e2y * v
			// where 0 <= u && 0 <= v && u + v <= 1.
			//
			// We integrate u from [0,1-v] and then v from [0,1].
			// We also need to use the Jacobian of the transformation:
			// D = cross(e1, e2)
			//
			// Simplification: triangle centroid = (1/3) * (p1 + p2 + p3)
			//
			// The rest of the derivation is handled by computer algebra.

			Debug.Assert(this.Vertices.Count >= 3);

			if (this._density <= 0)
				return;

			Vector2 center = Vector2.Zero;
			float area = 0.0f;
			float I = 0.0f;

			// pRef is the reference point for forming triangles.
			// It's location doesn't change the result (except for rounding error).
			Vector2 pRef = Vector2.Zero;

#if false
    // This code would put the reference point inside the polygon.
	        for (int i = 0; i < count; ++i)
	        {
		        pRef += vs[i];
	        }
	        pRef *= 1.0f / count;
#endif

			const float inv3 = 1.0f / 3.0f;

			for (int i = 0; i < this.Vertices.Count; ++i)
			{
				// Triangle vertices.
				Vector2 p1 = pRef;
				Vector2 p2 = this.Vertices[i];
				Vector2 p3 = i + 1 < this.Vertices.Count ? this.Vertices[i + 1] : this.Vertices[0];

				Vector2 e1 = p2 - p1;
				Vector2 e2 = p3 - p1;

				float d;
				MathUtils.Cross(ref e1, ref e2, out d);

				float triangleArea = 0.5f * d;
				area += triangleArea;

				// Area weighted centroid
				center += triangleArea * inv3 * (p1 + p2 + p3);

				float px = p1.X, py = p1.Y;
				float ex1 = e1.X, ey1 = e1.Y;
				float ex2 = e2.X, ey2 = e2.Y;

				float intx2 = inv3 * (0.25f * (ex1 * ex1 + ex2 * ex1 + ex2 * ex2) + (px * ex1 + px * ex2)) +
							  0.5f * px * px;
				float inty2 = inv3 * (0.25f * (ey1 * ey1 + ey2 * ey1 + ey2 * ey2) + (py * ey1 + py * ey2)) +
							  0.5f * py * py;

				I += d * (intx2 + inty2);
			}

			//The area is too small for the engine to handle.
			Debug.Assert(area > Settings.Epsilon);

			// We save the area
			this.MassData.Area = area;

			// Total mass
			this.MassData.Mass = this._density * area;

			// Center of mass
			center *= 1.0f / area;
			this.MassData.Centroid = center;

			// Inertia tensor relative to the local origin.
			this.MassData.Inertia = this._density * I;
		}

		/// <summary>
		/// Build vertices to represent an axis-aligned box.
		/// </summary>
		/// <param name="halfWidth">The half-width.</param>
		/// <param name="halfHeight">The half-height.</param>
		public void SetAsBox(float halfWidth, float halfHeight)
		{
			Set(PolygonTools.CreateRectangle(halfWidth, halfHeight));
		}

		/// <summary>
		/// Build vertices to represent an oriented box.
		/// </summary>
		/// <param name="halfWidth">The half-width..</param>
		/// <param name="halfHeight">The half-height.</param>
		/// <param name="center">The center of the box in local coordinates.</param>
		/// <param name="angle">The rotation of the box in local coordinates.</param>
		public void SetAsBox(float halfWidth, float halfHeight, Vector2 center, float angle)
		{
			Set(PolygonTools.CreateRectangle(halfWidth, halfHeight, center, angle));
		}

		/// <summary>
		/// Test a point for containment in this shape. This only works for convex shapes.
		/// </summary>
		/// <param name="transform">The shape world transform.</param>
		/// <param name="point">a point in world coordinates.</param>
		/// <returns>True if the point is inside the shape</returns>
		public override bool TestPoint(ref Transform transform, ref Vector2 point)
		{
			Vector2 pLocal = MathUtils.MultiplyT(ref transform.R, point - transform.Position);

			for (int i = 0; i < this.Vertices.Count; ++i)
			{
				float dot = Vector2.Dot(this.Normals[i], pLocal - this.Vertices[i]);
				if (dot > 0.0f)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Cast a ray against a child shape.
		/// </summary>
		/// <param name="output">The ray-cast results.</param>
		/// <param name="input">The ray-cast input parameters.</param>
		/// <param name="transform">The transform to be applied to the shape.</param>
		/// <param name="childIndex">The child shape index.</param>
		/// <returns>True if the ray-cast hits the shape</returns>
		public override bool RayCast(out RayCastOutput output, ref RayCastInput input, ref Transform transform,
									 int childIndex)
		{
			output = new RayCastOutput();

			// Put the ray into the polygon's frame of reference.
			Vector2 p1 = MathUtils.MultiplyT(ref transform.R, input.Point1 - transform.Position);
			Vector2 p2 = MathUtils.MultiplyT(ref transform.R, input.Point2 - transform.Position);
			Vector2 d = p2 - p1;

			float lower = 0.0f, upper = input.MaxFraction;

			int index = -1;

			for (int i = 0; i < this.Vertices.Count; ++i)
			{
				// p = p1 + a * d
				// dot(normal, p - v) = 0
				// dot(normal, p1 - v) + a * dot(normal, d) = 0
				float numerator = Vector2.Dot(this.Normals[i], this.Vertices[i] - p1);
				float denominator = Vector2.Dot(this.Normals[i], d);

				if (denominator == 0.0f)
				{
					if (numerator < 0.0f)
					{
						return false;
					}
				}
				else
				{
					// Note: we want this predicate without division:
					// lower < numerator / denominator, where denominator < 0
					// Since denominator < 0, we have to flip the inequality:
					// lower < numerator / denominator <==> denominator * lower > numerator.
					if (denominator < 0.0f && numerator < lower * denominator)
					{
						// Increase lower.
						// The segment enters this half-space.
						lower = numerator / denominator;
						index = i;
					}
					else if (denominator > 0.0f && numerator < upper * denominator)
					{
						// Decrease upper.
						// The segment exits this half-space.
						upper = numerator / denominator;
					}
				}

				// The use of epsilon here causes the assert on lower to trip
				// in some cases. Apparently the use of epsilon was to make edge
				// shapes work, but now those are handled separately.
				//if (upper < lower - b2_epsilon)
				if (upper < lower)
				{
					return false;
				}
			}

			Debug.Assert(0.0f <= lower && lower <= input.MaxFraction);

			if (index >= 0)
			{
				output.Fraction = lower;
				output.Normal = MathUtils.Multiply(ref transform.R, this.Normals[index]);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Given a transform, compute the associated axis aligned bounding box for a child shape.
		/// </summary>
		/// <param name="aabb">The aabb results.</param>
		/// <param name="transform">The world transform of the shape.</param>
		/// <param name="childIndex">The child shape index.</param>
		public override void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex)
		{
			Vector2 lower = MathUtils.Multiply(ref transform, this.Vertices[0]);
			Vector2 upper = lower;

			for (int i = 1; i < this.Vertices.Count; ++i)
			{
				Vector2 v = MathUtils.Multiply(ref transform, this.Vertices[i]);
				lower = Vector2.Min(lower, v);
				upper = Vector2.Max(upper, v);
			}

			Vector2 r = new Vector2(this.Radius, this.Radius);
			aabb.LowerBound = lower - r;
			aabb.UpperBound = upper + r;
		}

		public bool CompareTo(PolygonShape shape)
		{
			if (this.Vertices.Count != shape.Vertices.Count)
				return false;

			for (int i = 0; i < this.Vertices.Count; i++)
			{
				if (this.Vertices[i] != shape.Vertices[i])
					return false;
			}

			return (this.Radius == shape.Radius &&
					this.MassData == shape.MassData);
		}

		public override float ComputeSubmergedArea(Vector2 normal, float offset, Transform xf, out Vector2 sc)
		{
			sc = Vector2.Zero;

			//Transform plane into shape co-ordinates
			Vector2 normalL = MathUtils.MultiplyT(ref xf.R, normal);
			float offsetL = offset - Vector2.Dot(normal, xf.Position);

			float[] depths = new float[Settings.MaxPolygonVertices];
			int diveCount = 0;
			int intoIndex = -1;
			int outoIndex = -1;

			bool lastSubmerged = false;
			int i;
			for (i = 0; i < this.Vertices.Count; i++)
			{
				depths[i] = Vector2.Dot(normalL, this.Vertices[i]) - offsetL;
				bool isSubmerged = depths[i] < -Settings.Epsilon;
				if (i > 0)
				{
					if (isSubmerged)
					{
						if (!lastSubmerged)
						{
							intoIndex = i - 1;
							diveCount++;
						}
					}
					else
					{
						if (lastSubmerged)
						{
							outoIndex = i - 1;
							diveCount++;
						}
					}
				}
				lastSubmerged = isSubmerged;
			}
			switch (diveCount)
			{
				case 0:
					if (lastSubmerged)
					{
						//Completely submerged
						sc = MathUtils.Multiply(ref xf, this.MassData.Centroid);
						return this.MassData.Mass / this.Density;
					}
					else
					{
						//Completely dry
						return 0;
					}
					break;
				case 1:
					if (intoIndex == -1)
					{
						intoIndex = this.Vertices.Count - 1;
					}
					else
					{
						outoIndex = this.Vertices.Count - 1;
					}
					break;
			}
			int intoIndex2 = (intoIndex + 1) % this.Vertices.Count;
			int outoIndex2 = (outoIndex + 1) % this.Vertices.Count;

			float intoLambda = (0 - depths[intoIndex]) / (depths[intoIndex2] - depths[intoIndex]);
			float outoLambda = (0 - depths[outoIndex]) / (depths[outoIndex2] - depths[outoIndex]);

			Vector2 intoVec = new Vector2(
				this.Vertices[intoIndex].X * (1 - intoLambda) + this.Vertices[intoIndex2].X * intoLambda,
				this.Vertices[intoIndex].Y * (1 - intoLambda) + this.Vertices[intoIndex2].Y * intoLambda);
			Vector2 outoVec = new Vector2(
				this.Vertices[outoIndex].X * (1 - outoLambda) + this.Vertices[outoIndex2].X * outoLambda,
				this.Vertices[outoIndex].Y * (1 - outoLambda) + this.Vertices[outoIndex2].Y * outoLambda);

			//Initialize accumulator
			float area = 0;
			Vector2 center = new Vector2(0, 0);
			Vector2 p2 = this.Vertices[intoIndex2];
			Vector2 p3;

			float k_inv3 = 1.0f / 3.0f;

			//An awkward loop from intoIndex2+1 to outIndex2
			i = intoIndex2;
			while (i != outoIndex2)
			{
				i = (i + 1) % this.Vertices.Count;
				if (i == outoIndex2)
					p3 = outoVec;
				else
					p3 = this.Vertices[i];
				//Add the triangle formed by intoVec,p2,p3
				{
					Vector2 e1 = p2 - intoVec;
					Vector2 e2 = p3 - intoVec;

					float D = MathUtils.Cross(e1, e2);

					float triangleArea = 0.5f * D;

					area += triangleArea;

					// Area weighted centroid
					center += triangleArea * k_inv3 * (intoVec + p2 + p3);
				}
				//
				p2 = p3;
			}

			//Normalize and transform centroid
			center *= 1.0f / area;

			sc = MathUtils.Multiply(ref xf, center);

			return area;
		}
	}
}