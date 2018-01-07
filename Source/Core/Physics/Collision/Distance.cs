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

using System;
using System.Diagnostics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Duality;

namespace FarseerPhysics.Collision
{
	/// <summary>
	/// A distance proxy is used by the GJK algorithm.
	/// It encapsulates any shape.
	/// </summary>
	public class DistanceProxy
	{
		internal float Radius;
		internal Vertices Vertices = new Vertices();

		/// <summary>
		/// Initialize the proxy using the given shape. The shape
		/// must remain in scope while the proxy is in use.
		/// </summary>
		/// <param name="shape">The shape.</param>
		/// <param name="index">The index.</param>
		public void Set(Shape shape, int index)
		{
			switch (shape.ShapeType)
			{
				case ShapeType.Circle:
					{
						CircleShape circle = (CircleShape)shape;
						this.Vertices.Clear();
						this.Vertices.Add(circle.Position);
						this.Radius = circle.Radius;
					}
					break;

				case ShapeType.Polygon:
					{
						PolygonShape polygon = (PolygonShape)shape;
						this.Vertices.Clear();
						for (int i = 0; i < polygon.Vertices.Count; i++)
						{
							this.Vertices.Add(polygon.Vertices[i]);
						}
						this.Radius = polygon.Radius;
					}
					break;

				case ShapeType.Chain:
					{
						ChainShape chain = (ChainShape)shape;
						Debug.Assert(0 <= index && index < chain.Vertices.Count);
						this.Vertices.Clear();
						this.Vertices.Add(chain.Vertices[index]);
						this.Vertices.Add(index + 1 < chain.Vertices.Count ? chain.Vertices[index + 1] : chain.Vertices[0]);

						this.Radius = chain.Radius;
					}
					break;

				case ShapeType.Edge:
					{
						EdgeShape edge = (EdgeShape)shape;
						this.Vertices.Clear();
						this.Vertices.Add(edge.Vertex1);
						this.Vertices.Add(edge.Vertex2);
						this.Radius = edge.Radius;
					}
					break;

				default:
					Debug.Assert(false);
					break;
			}
		}

		/// <summary>
		/// Get the supporting vertex index in the given direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		public int GetSupport(Vector2 direction)
		{
			int bestIndex = 0;
			float bestValue = Vector2.Dot(this.Vertices[0], direction);
			for (int i = 1; i < this.Vertices.Count; ++i)
			{
				float value = Vector2.Dot(this.Vertices[i], direction);
				if (value > bestValue)
				{
					bestIndex = i;
					bestValue = value;
				}
			}

			return bestIndex;
		}

		/// <summary>
		/// Get the supporting vertex in the given direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		public Vector2 GetSupportVertex(Vector2 direction)
		{
			int bestIndex = 0;
			float bestValue = Vector2.Dot(this.Vertices[0], direction);
			for (int i = 1; i < this.Vertices.Count; ++i)
			{
				float value = Vector2.Dot(this.Vertices[i], direction);
				if (value > bestValue)
				{
					bestIndex = i;
					bestValue = value;
				}
			}

			return this.Vertices[bestIndex];
		}
	}

	/// <summary>
	/// Used to warm start ComputeDistance.
	/// Set count to zero on first call.
	/// </summary>
	public struct SimplexCache
	{
		/// <summary>
		/// Length or area
		/// </summary>
		public ushort Count;

		/// <summary>
		/// Vertices on shape A
		/// </summary>
		public FixedArray3<byte> IndexA;

		/// <summary>
		/// Vertices on shape B
		/// </summary>
		public FixedArray3<byte> IndexB;

		public float Metric;
	}

	/// <summary>
	/// Input for ComputeDistance.
	/// You have to option to use the shape radii
	/// in the computation. 
	/// </summary>
	public class DistanceInput
	{
		public DistanceProxy ProxyA = new DistanceProxy();
		public DistanceProxy ProxyB = new DistanceProxy();
		public Transform TransformA;
		public Transform TransformB;
		public bool UseRadii;
	}

	/// <summary>
	/// Output for ComputeDistance.
	/// </summary>
	public struct DistanceOutput
	{
		public float Distance;

		/// <summary>
		/// Number of GJK iterations used
		/// </summary>
		public int Iterations;

		/// <summary>
		/// Closest point on shapeA
		/// </summary>
		public Vector2 PointA;

		/// <summary>
		/// Closest point on shapeB
		/// </summary>
		public Vector2 PointB;
	}

	internal struct SimplexVertex
	{
		/// <summary>
		/// Barycentric coordinate for closest point 
		/// </summary>
		public float A;

		/// <summary>
		/// wA index
		/// </summary>
		public int IndexA;

		/// <summary>
		/// wB index
		/// </summary>
		public int IndexB;

		/// <summary>
		/// wB - wA
		/// </summary>
		public Vector2 W;

		/// <summary>
		/// Support point in proxyA
		/// </summary>
		public Vector2 WA;

		/// <summary>
		/// Support point in proxyB
		/// </summary>
		public Vector2 WB;
	}

	internal struct Simplex
	{
		internal int Count;
		internal FixedArray3<SimplexVertex> V;

		internal void ReadCache(ref SimplexCache cache,
								DistanceProxy proxyA, ref Transform transformA,
								DistanceProxy proxyB, ref Transform transformB)
		{
			Debug.Assert(cache.Count <= 3);

			// Copy data from cache.
			this.Count = cache.Count;
			for (int i = 0; i < this.Count; ++i)
			{
				SimplexVertex v = this.V[i];
				v.IndexA = cache.IndexA[i];
				v.IndexB = cache.IndexB[i];
				Vector2 wALocal = proxyA.Vertices[v.IndexA];
				Vector2 wBLocal = proxyB.Vertices[v.IndexB];
				v.WA = MathUtils.Multiply(ref transformA, wALocal);
				v.WB = MathUtils.Multiply(ref transformB, wBLocal);
				v.W = v.WB - v.WA;
				v.A = 0.0f;
				this.V[i] = v;
			}

			// Compute the new simplex metric, if it is substantially different than
			// old metric then flush the simplex.
			if (this.Count > 1)
			{
				float metric1 = cache.Metric;
				float metric2 = GetMetric();
				if (metric2 < 0.5f * metric1 || 2.0f * metric1 < metric2 || metric2 < Settings.Epsilon)
				{
					// Reset the simplex.
					this.Count = 0;
				}
			}

			// If the cache is empty or invalid ...
			if (this.Count == 0)
			{
				SimplexVertex v = this.V[0];
				v.IndexA = 0;
				v.IndexB = 0;
				Vector2 wALocal = proxyA.Vertices[0];
				Vector2 wBLocal = proxyB.Vertices[0];
				v.WA = MathUtils.Multiply(ref transformA, wALocal);
				v.WB = MathUtils.Multiply(ref transformB, wBLocal);
				v.W = v.WB - v.WA;
				this.V[0] = v;
				this.Count = 1;
			}
		}

		internal void WriteCache(ref SimplexCache cache)
		{
			cache.Metric = GetMetric();
			cache.Count = (ushort)this.Count;
			for (int i = 0; i < this.Count; ++i)
			{
				cache.IndexA[i] = (byte)(this.V[i].IndexA);
				cache.IndexB[i] = (byte)(this.V[i].IndexB);
			}
		}

		internal Vector2 GetSearchDirection()
		{
			switch (this.Count)
			{
				case 1:
					return -this.V[0].W;

				case 2:
					{
						Vector2 e12 = this.V[1].W - this.V[0].W;
						float sgn = MathUtils.Cross(e12, -this.V[0].W);
						if (sgn > 0.0f)
						{
							// Origin is left of e12.
							return new Vector2(-e12.Y, e12.X);
						}
						else
						{
							// Origin is right of e12.
							return new Vector2(e12.Y, -e12.X);
						}
					}

				default:
					Debug.Assert(false);
					return Vector2.Zero;
			}
		}

		internal Vector2 GetClosestPoint()
		{
			switch (this.Count)
			{
				case 0:
					Debug.Assert(false);
					return Vector2.Zero;

				case 1:
					return this.V[0].W;

				case 2:
					return this.V[0].A * this.V[0].W + this.V[1].A * this.V[1].W;

				case 3:
					return Vector2.Zero;

				default:
					Debug.Assert(false);
					return Vector2.Zero;
			}
		}

		internal void GetWitnessPoints(out Vector2 pA, out Vector2 pB)
		{
			switch (this.Count)
			{
				case 0:
					pA = Vector2.Zero;
					pB = Vector2.Zero;
					Debug.Assert(false);
					break;

				case 1:
					pA = this.V[0].WA;
					pB = this.V[0].WB;
					break;

				case 2:
					pA = this.V[0].A * this.V[0].WA + this.V[1].A * this.V[1].WA;
					pB = this.V[0].A * this.V[0].WB + this.V[1].A * this.V[1].WB;
					break;

				case 3:
					pA = this.V[0].A * this.V[0].WA + this.V[1].A * this.V[1].WA + this.V[2].A * this.V[2].WA;
					pB = pA;
					break;

				default:
					throw new Exception();
			}
		}

		internal float GetMetric()
		{
			switch (this.Count)
			{
				case 0:
					Debug.Assert(false);
					return 0.0f;

				case 1:
					return 0.0f;

				case 2:
					return (this.V[0].W - this.V[1].W).Length;

				case 3:
					return MathUtils.Cross(this.V[1].W - this.V[0].W, this.V[2].W - this.V[0].W);

				default:
					Debug.Assert(false);
					return 0.0f;
			}
		}

		// Solve a line segment using barycentric coordinates.
		//
		// p = a1 * w1 + a2 * w2
		// a1 + a2 = 1
		//
		// The vector from the origin to the closest point on the line is
		// perpendicular to the line.
		// e12 = w2 - w1
		// dot(p, e) = 0
		// a1 * dot(w1, e) + a2 * dot(w2, e) = 0
		//
		// 2-by-2 linear system
		// [1      1     ][a1] = [1]
		// [w1.e12 w2.e12][a2] = [0]
		//
		// Define
		// d12_1 =  dot(w2, e12)
		// d12_2 = -dot(w1, e12)
		// d12 = d12_1 + d12_2
		//
		// Solution
		// a1 = d12_1 / d12
		// a2 = d12_2 / d12

		internal void Solve2()
		{
			Vector2 w1 = this.V[0].W;
			Vector2 w2 = this.V[1].W;
			Vector2 e12 = w2 - w1;

			// w1 region
			float d12_2 = -Vector2.Dot(w1, e12);
			if (d12_2 <= 0.0f)
			{
				// a2 <= 0, so we clamp it to 0
				SimplexVertex v0 = this.V[0];
				v0.A = 1.0f;
				this.V[0] = v0;
				this.Count = 1;
				return;
			}

			// w2 region
			float d12_1 = Vector2.Dot(w2, e12);
			if (d12_1 <= 0.0f)
			{
				// a1 <= 0, so we clamp it to 0
				SimplexVertex v1 = this.V[1];
				v1.A = 1.0f;
				this.V[1] = v1;
				this.Count = 1;
				this.V[0] = this.V[1];
				return;
			}

			// Must be in e12 region.
			float inv_d12 = 1.0f / (d12_1 + d12_2);
			SimplexVertex v0_2 = this.V[0];
			SimplexVertex v1_2 = this.V[1];
			v0_2.A = d12_1 * inv_d12;
			v1_2.A = d12_2 * inv_d12;
			this.V[0] = v0_2;
			this.V[1] = v1_2;
			this.Count = 2;
		}

		// Possible regions:
		// - points[2]
		// - edge points[0]-points[2]
		// - edge points[1]-points[2]
		// - inside the triangle
		internal void Solve3()
		{
			Vector2 w1 = this.V[0].W;
			Vector2 w2 = this.V[1].W;
			Vector2 w3 = this.V[2].W;

			// Edge12
			// [1      1     ][a1] = [1]
			// [w1.e12 w2.e12][a2] = [0]
			// a3 = 0
			Vector2 e12 = w2 - w1;
			float w1e12 = Vector2.Dot(w1, e12);
			float w2e12 = Vector2.Dot(w2, e12);
			float d12_1 = w2e12;
			float d12_2 = -w1e12;

			// Edge13
			// [1      1     ][a1] = [1]
			// [w1.e13 w3.e13][a3] = [0]
			// a2 = 0
			Vector2 e13 = w3 - w1;
			float w1e13 = Vector2.Dot(w1, e13);
			float w3e13 = Vector2.Dot(w3, e13);
			float d13_1 = w3e13;
			float d13_2 = -w1e13;

			// Edge23
			// [1      1     ][a2] = [1]
			// [w2.e23 w3.e23][a3] = [0]
			// a1 = 0
			Vector2 e23 = w3 - w2;
			float w2e23 = Vector2.Dot(w2, e23);
			float w3e23 = Vector2.Dot(w3, e23);
			float d23_1 = w3e23;
			float d23_2 = -w2e23;

			// Triangle123
			float n123 = MathUtils.Cross(e12, e13);

			float d123_1 = n123 * MathUtils.Cross(w2, w3);
			float d123_2 = n123 * MathUtils.Cross(w3, w1);
			float d123_3 = n123 * MathUtils.Cross(w1, w2);

			// w1 region
			if (d12_2 <= 0.0f && d13_2 <= 0.0f)
			{
				SimplexVertex v0_1 = this.V[0];
				v0_1.A = 1.0f;
				this.V[0] = v0_1;
				this.Count = 1;
				return;
			}

			// e12
			if (d12_1 > 0.0f && d12_2 > 0.0f && d123_3 <= 0.0f)
			{
				float inv_d12 = 1.0f / (d12_1 + d12_2);
				SimplexVertex v0_2 = this.V[0];
				SimplexVertex v1_2 = this.V[1];
				v0_2.A = d12_1 * inv_d12;
				v1_2.A = d12_2 * inv_d12;
				this.V[0] = v0_2;
				this.V[1] = v1_2;
				this.Count = 2;
				return;
			}

			// e13
			if (d13_1 > 0.0f && d13_2 > 0.0f && d123_2 <= 0.0f)
			{
				float inv_d13 = 1.0f / (d13_1 + d13_2);
				SimplexVertex v0_3 = this.V[0];
				SimplexVertex v2_3 = this.V[2];
				v0_3.A = d13_1 * inv_d13;
				v2_3.A = d13_2 * inv_d13;
				this.V[0] = v0_3;
				this.V[2] = v2_3;
				this.Count = 2;
				this.V[1] = this.V[2];
				return;
			}

			// w2 region
			if (d12_1 <= 0.0f && d23_2 <= 0.0f)
			{
				SimplexVertex v1_4 = this.V[1];
				v1_4.A = 1.0f;
				this.V[1] = v1_4;
				this.Count = 1;
				this.V[0] = this.V[1];
				return;
			}

			// w3 region
			if (d13_1 <= 0.0f && d23_1 <= 0.0f)
			{
				SimplexVertex v2_5 = this.V[2];
				v2_5.A = 1.0f;
				this.V[2] = v2_5;
				this.Count = 1;
				this.V[0] = this.V[2];
				return;
			}

			// e23
			if (d23_1 > 0.0f && d23_2 > 0.0f && d123_1 <= 0.0f)
			{
				float inv_d23 = 1.0f / (d23_1 + d23_2);
				SimplexVertex v1_6 = this.V[1];
				SimplexVertex v2_6 = this.V[2];
				v1_6.A = d23_1 * inv_d23;
				v2_6.A = d23_2 * inv_d23;
				this.V[1] = v1_6;
				this.V[2] = v2_6;
				this.Count = 2;
				this.V[0] = this.V[2];
				return;
			}

			// Must be in triangle123
			float inv_d123 = 1.0f / (d123_1 + d123_2 + d123_3);
			SimplexVertex v0_7 = this.V[0];
			SimplexVertex v1_7 = this.V[1];
			SimplexVertex v2_7 = this.V[2];
			v0_7.A = d123_1 * inv_d123;
			v1_7.A = d123_2 * inv_d123;
			v2_7.A = d123_3 * inv_d123;
			this.V[0] = v0_7;
			this.V[1] = v1_7;
			this.V[2] = v2_7;
			this.Count = 3;
		}
	}

	public static class Distance
	{
		public static int GJKCalls, GJKIters, GJKMaxIters;

		public static void ComputeDistance(out DistanceOutput output,
										   out SimplexCache cache,
										   DistanceInput input)
		{
			cache = new SimplexCache();
			++GJKCalls;

			// Initialize the simplex.
			Simplex simplex = new Simplex();
			simplex.ReadCache(ref cache, input.ProxyA, ref input.TransformA, input.ProxyB, ref input.TransformB);

			// Get simplex vertices as an array.
			const int k_maxIters = 20;

			// These store the vertices of the last simplex so that we
			// can check for duplicates and prevent cycling.
			FixedArray3<int> saveA = new FixedArray3<int>();
			FixedArray3<int> saveB = new FixedArray3<int>();

			Vector2 closestPoint = simplex.GetClosestPoint();
			float distanceSqr1 = closestPoint.LengthSquared;
			float distanceSqr2 = distanceSqr1;

			// Main iteration loop.
			int iter = 0;
			while (iter < k_maxIters)
			{
				// Copy simplex so we can identify duplicates.
				int saveCount = simplex.Count;
				for (int i = 0; i < saveCount; ++i)
				{
					saveA[i] = simplex.V[i].IndexA;
					saveB[i] = simplex.V[i].IndexB;
				}

				switch (simplex.Count)
				{
					case 1:
						break;

					case 2:
						simplex.Solve2();
						break;

					case 3:
						simplex.Solve3();
						break;

					default:
						Debug.Assert(false);
						break;
				}

				// If we have 3 points, then the origin is in the corresponding triangle.
				if (simplex.Count == 3)
				{
					break;
				}

				// Compute closest point.
				Vector2 p = simplex.GetClosestPoint();
				distanceSqr2 = p.LengthSquared;

				// Ensure progress
				if (distanceSqr2 >= distanceSqr1)
				{
					//break;
				}
				distanceSqr1 = distanceSqr2;

				// Get search direction.
				Vector2 d = simplex.GetSearchDirection();

				// Ensure the search direction is numerically fit.
				if (d.LengthSquared < Settings.Epsilon * Settings.Epsilon)
				{
					// The origin is probably contained by a line segment
					// or triangle. Thus the shapes are overlapped.

					// We can't return zero here even though there may be overlap.
					// In case the simplex is a point, segment, or triangle it is difficult
					// to determine if the origin is contained in the CSO or very close to it.
					break;
				}

				// Compute a tentative new simplex vertex using support points.
				SimplexVertex vertex = simplex.V[simplex.Count];
				vertex.IndexA = input.ProxyA.GetSupport(MathUtils.MultiplyT(ref input.TransformA.R, -d));
				vertex.WA = MathUtils.Multiply(ref input.TransformA, input.ProxyA.Vertices[vertex.IndexA]);

				vertex.IndexB = input.ProxyB.GetSupport(MathUtils.MultiplyT(ref input.TransformB.R, d));
				vertex.WB = MathUtils.Multiply(ref input.TransformB, input.ProxyB.Vertices[vertex.IndexB]);
				vertex.W = vertex.WB - vertex.WA;
				simplex.V[simplex.Count] = vertex;

				// Iteration count is equated to the number of support point calls.
				++iter;
				++GJKIters;

				// Check for duplicate support points. This is the main termination criteria.
				bool duplicate = false;
				for (int i = 0; i < saveCount; ++i)
				{
					if (vertex.IndexA == saveA[i] && vertex.IndexB == saveB[i])
					{
						duplicate = true;
						break;
					}
				}

				// If we found a duplicate support point we must exit to avoid cycling.
				if (duplicate)
				{
					break;
				}

				// New vertex is ok and needed.
				++simplex.Count;
			}

			GJKMaxIters = Math.Max(GJKMaxIters, iter);

			// Prepare output.
			simplex.GetWitnessPoints(out output.PointA, out output.PointB);
			output.Distance = (output.PointA - output.PointB).Length;
			output.Iterations = iter;

			// Cache the simplex.
			simplex.WriteCache(ref cache);

			// Apply radii if requested.
			if (input.UseRadii)
			{
				float rA = input.ProxyA.Radius;
				float rB = input.ProxyB.Radius;

				if (output.Distance > rA + rB && output.Distance > Settings.Epsilon)
				{
					// Shapes are still no overlapped.
					// Move the witness points to the outer surface.
					output.Distance -= rA + rB;
					Vector2 normal = output.PointB - output.PointA;
					normal.Normalize();
					output.PointA += rA * normal;
					output.PointB -= rB * normal;
				}
				else
				{
					// Shapes are overlapped when radii are considered.
					// Move the witness points to the middle.
					Vector2 p = 0.5f * (output.PointA + output.PointB);
					output.PointA = p;
					output.PointB = p;
					output.Distance = 0.0f;
				}
			}
		}
	}
}