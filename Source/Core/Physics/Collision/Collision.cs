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
using System.Runtime.InteropServices;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Duality;

namespace FarseerPhysics.Collision
{
	internal enum ContactFeatureType : byte
	{
		Vertex = 0,
		Face = 1,
	}

	/// <summary>
	/// The features that intersect to form the contact point
	/// This must be 4 bytes or less.
	/// </summary>
	public struct ContactFeature
	{
		/// <summary>
		/// Feature index on ShapeA
		/// </summary>
		public byte IndexA;

		/// <summary>
		/// Feature index on ShapeB
		/// </summary>
		public byte IndexB;

		/// <summary>
		/// The feature type on ShapeA
		/// </summary>
		public byte TypeA;

		/// <summary>
		/// The feature type on ShapeB
		/// </summary>
		public byte TypeB;
	}

	/// <summary>
	/// Contact ids to facilitate warm starting.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct ContactID
	{
		/// <summary>
		/// The features that intersect to form the contact point
		/// </summary>
		[FieldOffset(0)]
		public ContactFeature Features;

		/// <summary>
		/// Used to quickly compare contact ids.
		/// </summary>
		[FieldOffset(0)]
		public uint Key;
	}

	/// <summary>
	/// A manifold point is a contact point belonging to a contact
	/// manifold. It holds details related to the geometry and dynamics
	/// of the contact points.
	/// The local point usage depends on the manifold type:
	/// -ShapeType.Circles: the local center of circleB
	/// -SeparationFunction.FaceA: the local center of cirlceB or the clip point of polygonB
	/// -SeparationFunction.FaceB: the clip point of polygonA
	/// This structure is stored across time steps, so we keep it small.
	/// Note: the impulses are used for internal caching and may not
	/// provide reliable contact forces, especially for high speed collisions.
	/// </summary>
	public struct ManifoldPoint
	{
		/// <summary>
		/// Uniquely identifies a contact point between two Shapes
		/// </summary>
		public ContactID Id;

		public Vector2 LocalPoint;

		public float NormalImpulse;

		public float TangentImpulse;
	}

	public enum ManifoldType
	{
		Circles,
		FaceA,
		FaceB
	}

	/// <summary>
	/// A manifold for two touching convex Shapes.
	/// Box2D supports multiple types of contact:
	/// - clip point versus plane with radius
	/// - point versus point with radius (circles)
	/// The local point usage depends on the manifold type:
	/// -ShapeType.Circles: the local center of circleA
	/// -SeparationFunction.FaceA: the center of faceA
	/// -SeparationFunction.FaceB: the center of faceB
	/// Similarly the local normal usage:
	/// -ShapeType.Circles: not used
	/// -SeparationFunction.FaceA: the normal on polygonA
	/// -SeparationFunction.FaceB: the normal on polygonB
	/// We store contacts in this way so that position correction can
	/// account for movement, which is critical for continuous physics.
	/// All contact scenarios must be expressed in one of these types.
	/// This structure is stored across time steps, so we keep it small.
	/// </summary>
	public struct Manifold
	{
		/// <summary>
		/// Not use for Type.SeparationFunction.Points
		/// </summary>
		public Vector2 LocalNormal;

		/// <summary>
		/// Usage depends on manifold type
		/// </summary>
		public Vector2 LocalPoint;

		/// <summary>
		/// The number of manifold points
		/// </summary>
		public int PointCount;

		/// <summary>
		/// The points of contact
		/// </summary>
		public FixedArray2<ManifoldPoint> Points;

		public ManifoldType Type;
	}

	/// <summary>
	/// This is used for determining the state of contact points.
	/// </summary>
	public enum PointState
	{
		/// <summary>
		/// Point does not exist
		/// </summary>
		Null,

		/// <summary>
		/// Point was added in the update
		/// </summary>
		Add,

		/// <summary>
		/// Point persisted across the update
		/// </summary>
		Persist,

		/// <summary>
		/// Point was removed in the update
		/// </summary>
		Remove,
	}

	/// <summary>
	/// Used for computing contact manifolds.
	/// </summary>
	public struct ClipVertex
	{
		public ContactID ID;
		public Vector2 V;
	}

	/// <summary>
	/// Ray-cast input data. The ray extends from p1 to p1 + maxFraction * (p2 - p1).
	/// </summary>
	public struct RayCastInput
	{
		public float MaxFraction;
		public Vector2 Point1, Point2;
	}

	/// <summary>
	/// Ray-cast output data.  The ray hits at p1 + fraction * (p2 - p1), where p1 and p2
	/// come from RayCastInput. 
	/// </summary>
	public struct RayCastOutput
	{
		public float Fraction;
		public Vector2 Normal;
	}

	/// <summary>
	/// An axis aligned bounding box.
	/// </summary>
	public struct AABB
	{
		private static DistanceInput _input = new DistanceInput();

		/// <summary>
		/// The lower vertex
		/// </summary>
		public Vector2 LowerBound;

		/// <summary>
		/// The upper vertex
		/// </summary>
		public Vector2 UpperBound;

		public AABB(Vector2 min, Vector2 max)
			: this(ref min, ref max)
		{
		}

		public AABB(ref Vector2 min, ref Vector2 max)
		{
			this.LowerBound = min;
			this.UpperBound = max;
		}

		public AABB(Vector2 center, float width, float height)
		{
			this.LowerBound = center - new Vector2(width / 2, height / 2);
			this.UpperBound = center + new Vector2(width / 2, height / 2);
		}

		/// <summary>
		/// Get the center of the AABB.
		/// </summary>
		/// <value></value>
		public Vector2 Center
		{
			get { return 0.5f * (this.LowerBound + this.UpperBound); }
		}

		/// <summary>
		/// Get the extents of the AABB (half-widths).
		/// </summary>
		/// <value></value>
		public Vector2 Extents
		{
			get { return 0.5f * (this.UpperBound - this.LowerBound); }
		}

		/// <summary>
		/// Get the perimeter length
		/// </summary>
		/// <value></value>
		public float Perimeter
		{
			get
			{
				float wx = this.UpperBound.X - this.LowerBound.X;
				float wy = this.UpperBound.Y - this.LowerBound.Y;
				return 2.0f * (wx + wy);
			}
		}

		/// <summary>
		/// Gets the vertices of the AABB.
		/// </summary>
		/// <value>The corners of the AABB</value>
		public Vertices Vertices
		{
			get
			{
				Vertices vertices = new Vertices();
				vertices.Add(this.LowerBound);
				vertices.Add(new Vector2(this.LowerBound.X, this.UpperBound.Y));
				vertices.Add(this.UpperBound);
				vertices.Add(new Vector2(this.UpperBound.X, this.LowerBound.Y));
				return vertices;
			}
		}

		/// <summary>
		/// first quadrant
		/// </summary>
		public AABB Q1
		{
			get { return new AABB(this.Center, this.UpperBound); }
		}

		public AABB Q2
		{
			get
			{
				return new AABB(new Vector2(this.LowerBound.X, this.Center.Y), new Vector2(this.Center.X, this.UpperBound.Y));
				;
			}
		}

		public AABB Q3
		{
			get { return new AABB(this.LowerBound, this.Center); }
		}

		public AABB Q4
		{
			get { return new AABB(new Vector2(this.Center.X, this.LowerBound.Y), new Vector2(this.UpperBound.X, this.Center.Y)); }
		}

		public Vector2[] GetVertices()
		{
			Vector2 p1 = this.UpperBound;
			Vector2 p2 = new Vector2(this.UpperBound.X, this.LowerBound.Y);
			Vector2 p3 = this.LowerBound;
			Vector2 p4 = new Vector2(this.LowerBound.X, this.UpperBound.Y);
			return new[] { p1, p2, p3, p4 };
		}

		/// <summary>
		/// Verify that the bounds are sorted.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValid()
		{
			Vector2 d = this.UpperBound - this.LowerBound;
			bool valid = d.X >= 0.0f && d.Y >= 0.0f;
			valid = valid && this.LowerBound.IsValid() && this.UpperBound.IsValid();
			return valid;
		}

		/// <summary>
		/// Combine an AABB into this one.
		/// </summary>
		/// <param name="aabb">The aabb.</param>
		public void Combine(ref AABB aabb)
		{
			this.LowerBound = Vector2.Min(this.LowerBound, aabb.LowerBound);
			this.UpperBound = Vector2.Max(this.UpperBound, aabb.UpperBound);
		}

		/// <summary>
		/// Combine two AABBs into this one.
		/// </summary>
		/// <param name="aabb1">The aabb1.</param>
		/// <param name="aabb2">The aabb2.</param>
		public void Combine(ref AABB aabb1, ref AABB aabb2)
		{
			this.LowerBound = Vector2.Min(aabb1.LowerBound, aabb2.LowerBound);
			this.UpperBound = Vector2.Max(aabb1.UpperBound, aabb2.UpperBound);
		}

		/// <summary>
		/// Does this aabb contain the provided AABB.
		/// </summary>
		/// <param name="aabb">The aabb.</param>
		/// <returns>
		/// 	<c>true</c> if it contains the specified aabb; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(ref AABB aabb)
		{
			bool result = true;
			result = result && this.LowerBound.X <= aabb.LowerBound.X;
			result = result && this.LowerBound.Y <= aabb.LowerBound.Y;
			result = result && aabb.UpperBound.X <= this.UpperBound.X;
			result = result && aabb.UpperBound.Y <= this.UpperBound.Y;
			return result;
		}

		/// <summary>
		/// Determines whether the AAABB contains the specified point.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>
		/// 	<c>true</c> if it contains the specified point; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(ref Vector2 point)
		{
			//using epsilon to try and gaurd against float rounding errors.
			if ((point.X > (this.LowerBound.X + Settings.Epsilon) && point.X < (this.UpperBound.X - Settings.Epsilon) &&
				 (point.Y > (this.LowerBound.Y + Settings.Epsilon) && point.Y < (this.UpperBound.Y - Settings.Epsilon))))
			{
				return true;
			}
			return false;
		}

		public static bool TestOverlap(AABB a, AABB b)
		{
			return TestOverlap(ref a, ref b);
		}

		public static bool TestOverlap(ref AABB a, ref AABB b)
		{
			Vector2 d1 = b.LowerBound - a.UpperBound;
			Vector2 d2 = a.LowerBound - b.UpperBound;

			if (d1.X > 0.0f || d1.Y > 0.0f)
				return false;

			if (d2.X > 0.0f || d2.Y > 0.0f)
				return false;

			return true;
		}

		public static bool TestOverlap(Shape shapeA, int indexA,
									   Shape shapeB, int indexB,
									   ref Transform xfA, ref Transform xfB)
		{
			_input.ProxyA.Set(shapeA, indexA);
			_input.ProxyB.Set(shapeB, indexB);
			_input.TransformA = xfA;
			_input.TransformB = xfB;
			_input.UseRadii = true;

			SimplexCache cache;
			DistanceOutput output;
			Distance.ComputeDistance(out output, out cache, _input);

			return output.Distance < 10.0f * Settings.Epsilon;
		}


		// From Real-time Collision Detection, p179.
		public bool RayCast(out RayCastOutput output, ref RayCastInput input)
		{
			output = new RayCastOutput();

			float tmin = -Settings.MaxFloat;
			float tmax = Settings.MaxFloat;

			Vector2 p = input.Point1;
			Vector2 d = input.Point2 - input.Point1;
			Vector2 absD = MathUtils.Abs(d);

			Vector2 normal = Vector2.Zero;

			for (int i = 0; i < 2; ++i)
			{
				float absD_i = i == 0 ? absD.X : absD.Y;
				float lowerBound_i = i == 0 ? this.LowerBound.X : this.LowerBound.Y;
				float upperBound_i = i == 0 ? this.UpperBound.X : this.UpperBound.Y;
				float p_i = i == 0 ? p.X : p.Y;

				if (absD_i < Settings.Epsilon)
				{
					// Parallel.
					if (p_i < lowerBound_i || upperBound_i < p_i)
					{
						return false;
					}
				}
				else
				{
					float d_i = i == 0 ? d.X : d.Y;

					float inv_d = 1.0f / d_i;
					float t1 = (lowerBound_i - p_i) * inv_d;
					float t2 = (upperBound_i - p_i) * inv_d;

					// Sign of the normal vector.
					float s = -1.0f;

					if (t1 > t2)
					{
						MathUtils.Swap(ref t1, ref t2);
						s = 1.0f;
					}

					// Push the min up
					if (t1 > tmin)
					{
						if (i == 0)
						{
							normal.X = s;
						}
						else
						{
							normal.Y = s;
						}

						tmin = t1;
					}

					// Pull the max down
					tmax = Math.Min(tmax, t2);

					if (tmin > tmax)
					{
						return false;
					}
				}
			}

			// Does the ray start inside the box?
			// Does the ray intersect beyond the max fraction?
			if (tmin < 0.0f || input.MaxFraction < tmin)
			{
				return false;
			}

			// Intersection.
			output.Fraction = tmin;
			output.Normal = normal;
			return true;
		}
	}

	/// <summary>
	/// Edge shape plus more stuff.
	/// </summary>
	public struct FatEdge
	{
		public bool HasVertex0, HasVertex3;
		public Vector2 Normal;
		public Vector2 V0, V1, V2, V3;
	}

	/// <summary>
	/// This lets us treate and edge shape and a polygon in the same
	/// way in the SAT collider.
	/// </summary>
	public class EPProxy
	{
		public Vector2 Centroid;
		public int Count;
		public Vector2[] Normals = new Vector2[Settings.MaxPolygonVertices];
		public Vector2[] Vertices = new Vector2[Settings.MaxPolygonVertices];
	}

	public struct EPAxis
	{
		public int Index;
		public float Separation;
		public EPAxisType Type;
	}

	public enum EPAxisType
	{
		Unknown,
		EdgeA,
		EdgeB,
	}

	public static class Collision
	{
		private static FatEdge _edgeA;

		private static EPProxy _proxyA = new EPProxy();
		private static EPProxy _proxyB = new EPProxy();

		private static Transform _xf;
		private static Vector2 _limit11, _limit12;
		private static Vector2 _limit21, _limit22;
		private static float _radius;
		private static Vector2[] _tmpNormals = new Vector2[2];

		/// <summary>
		/// Evaluate the manifold with supplied transforms. This assumes
		/// modest motion from the original state. This does not change the
		/// point count, impulses, etc. The radii must come from the Shapes
		/// that generated the manifold.
		/// </summary>
		/// <param name="manifold">The manifold.</param>
		/// <param name="transformA">The transform for A.</param>
		/// <param name="radiusA">The radius for A.</param>
		/// <param name="transformB">The transform for B.</param>
		/// <param name="radiusB">The radius for B.</param>
		/// <param name="normal">World vector pointing from A to B</param>
		/// <param name="points">Torld contact point (point of intersection).</param>
		public static void GetWorldManifold(ref Manifold manifold,
											ref Transform transformA, float radiusA,
											ref Transform transformB, float radiusB, out Vector2 normal,
											out FixedArray2<Vector2> points)
		{
			points = new FixedArray2<Vector2>();
			normal = Vector2.Zero;

			if (manifold.PointCount == 0)
			{
				normal = Vector2.UnitY;
				return;
			}

			switch (manifold.Type)
			{
				case ManifoldType.Circles:
					{
						Vector2 tmp = manifold.Points[0].LocalPoint;
						float pointAx = transformA.Position.X + transformA.R.Col1.X * manifold.LocalPoint.X +
										transformA.R.Col2.X * manifold.LocalPoint.Y;

						float pointAy = transformA.Position.Y + transformA.R.Col1.Y * manifold.LocalPoint.X +
										transformA.R.Col2.Y * manifold.LocalPoint.Y;

						float pointBx = transformB.Position.X + transformB.R.Col1.X * tmp.X +
										transformB.R.Col2.X * tmp.Y;

						float pointBy = transformB.Position.Y + transformB.R.Col1.Y * tmp.X +
										transformB.R.Col2.Y * tmp.Y;

						normal.X = 1;
						normal.Y = 0;

						float result = (pointAx - pointBx) * (pointAx - pointBx) +
									   (pointAy - pointBy) * (pointAy - pointBy);
						if (result > Settings.Epsilon * Settings.Epsilon)
						{
							float tmpNormalx = pointBx - pointAx;
							float tmpNormaly = pointBy - pointAy;
							float factor = 1f / (float)Math.Sqrt(tmpNormalx * tmpNormalx + tmpNormaly * tmpNormaly);
							normal.X = tmpNormalx * factor;
							normal.Y = tmpNormaly * factor;
						}

						Vector2 c = Vector2.Zero;
						c.X = (pointAx + radiusA * normal.X) + (pointBx - radiusB * normal.X);
						c.Y = (pointAy + radiusA * normal.Y) + (pointBy - radiusB * normal.Y);

						points[0] = 0.5f * c;
					}
					break;

				case ManifoldType.FaceA:
					{
						normal.X = transformA.R.Col1.X * manifold.LocalNormal.X +
								   transformA.R.Col2.X * manifold.LocalNormal.Y;
						normal.Y = transformA.R.Col1.Y * manifold.LocalNormal.X +
								   transformA.R.Col2.Y * manifold.LocalNormal.Y;

						float planePointx = transformA.Position.X + transformA.R.Col1.X * manifold.LocalPoint.X +
											transformA.R.Col2.X * manifold.LocalPoint.Y;

						float planePointy = transformA.Position.Y + transformA.R.Col1.Y * manifold.LocalPoint.X +
											transformA.R.Col2.Y * manifold.LocalPoint.Y;

						for (int i = 0; i < manifold.PointCount; ++i)
						{
							Vector2 tmp = manifold.Points[i].LocalPoint;

							float clipPointx = transformB.Position.X + transformB.R.Col1.X * tmp.X +
											   transformB.R.Col2.X * tmp.Y;

							float clipPointy = transformB.Position.Y + transformB.R.Col1.Y * tmp.X +
											   transformB.R.Col2.Y * tmp.Y;

							float value = (clipPointx - planePointx) * normal.X + (clipPointy - planePointy) * normal.Y;

							Vector2 c = Vector2.Zero;
							c.X = (clipPointx + (radiusA - value) * normal.X) + (clipPointx - radiusB * normal.X);
							c.Y = (clipPointy + (radiusA - value) * normal.Y) + (clipPointy - radiusB * normal.Y);

							points[i] = 0.5f * c;
						}
					}
					break;

				case ManifoldType.FaceB:
					{
						normal.X = transformB.R.Col1.X * manifold.LocalNormal.X +
								   transformB.R.Col2.X * manifold.LocalNormal.Y;
						normal.Y = transformB.R.Col1.Y * manifold.LocalNormal.X +
								   transformB.R.Col2.Y * manifold.LocalNormal.Y;

						float planePointx = transformB.Position.X + transformB.R.Col1.X * manifold.LocalPoint.X +
											transformB.R.Col2.X * manifold.LocalPoint.Y;

						float planePointy = transformB.Position.Y + transformB.R.Col1.Y * manifold.LocalPoint.X +
											transformB.R.Col2.Y * manifold.LocalPoint.Y;

						for (int i = 0; i < manifold.PointCount; ++i)
						{
							Vector2 tmp = manifold.Points[i].LocalPoint;

							float clipPointx = transformA.Position.X + transformA.R.Col1.X * tmp.X +
											   transformA.R.Col2.X * tmp.Y;

							float clipPointy = transformA.Position.Y + transformA.R.Col1.Y * tmp.X +
											   transformA.R.Col2.Y * tmp.Y;

							float value = (clipPointx - planePointx) * normal.X + (clipPointy - planePointy) * normal.Y;

							Vector2 c = Vector2.Zero;
							c.X = (clipPointx - radiusA * normal.X) + (clipPointx + (radiusB - value) * normal.X);
							c.Y = (clipPointy - radiusA * normal.Y) + (clipPointy + (radiusB - value) * normal.Y);

							points[i] = 0.5f * c;
						}
						// Ensure normal points from A to B.
						normal *= -1;
					}
					break;
				default:
					normal = Vector2.UnitY;
					break;
			}
		}

		public static void GetPointStates(out FixedArray2<PointState> state1, out FixedArray2<PointState> state2,
										  ref Manifold manifold1, ref Manifold manifold2)
		{
			state1 = new FixedArray2<PointState>();
			state2 = new FixedArray2<PointState>();

			// Detect persists and removes.
			for (int i = 0; i < manifold1.PointCount; ++i)
			{
				ContactID id = manifold1.Points[i].Id;

				state1[i] = PointState.Remove;

				for (int j = 0; j < manifold2.PointCount; ++j)
				{
					if (manifold2.Points[j].Id.Key == id.Key)
					{
						state1[i] = PointState.Persist;
						break;
					}
				}
			}

			// Detect persists and adds.
			for (int i = 0; i < manifold2.PointCount; ++i)
			{
				ContactID id = manifold2.Points[i].Id;

				state2[i] = PointState.Add;

				for (int j = 0; j < manifold1.PointCount; ++j)
				{
					if (manifold1.Points[j].Id.Key == id.Key)
					{
						state2[i] = PointState.Persist;
						break;
					}
				}
			}
		}


		/// Compute the collision manifold between two circles.
		public static void CollideCircles(ref Manifold manifold,
										  CircleShape circleA, ref Transform xfA,
										  CircleShape circleB, ref Transform xfB)
		{
			manifold.PointCount = 0;

			float pAx = xfA.Position.X + xfA.R.Col1.X * circleA.Position.X + xfA.R.Col2.X * circleA.Position.Y;
			float pAy = xfA.Position.Y + xfA.R.Col1.Y * circleA.Position.X + xfA.R.Col2.Y * circleA.Position.Y;
			float pBx = xfB.Position.X + xfB.R.Col1.X * circleB.Position.X + xfB.R.Col2.X * circleB.Position.Y;
			float pBy = xfB.Position.Y + xfB.R.Col1.Y * circleB.Position.X + xfB.R.Col2.Y * circleB.Position.Y;

			float distSqr = (pBx - pAx) * (pBx - pAx) + (pBy - pAy) * (pBy - pAy);
			float radius = circleA.Radius + circleB.Radius;
			if (distSqr > radius * radius)
			{
				return;
			}

			manifold.Type = ManifoldType.Circles;
			manifold.LocalPoint = circleA.Position;
			manifold.LocalNormal = Vector2.Zero;
			manifold.PointCount = 1;

			ManifoldPoint p0 = manifold.Points[0];

			p0.LocalPoint = circleB.Position;
			p0.Id.Key = 0;

			manifold.Points[0] = p0;
		}

		/// <summary>
		/// Compute the collision manifold between a polygon and a circle.
		/// </summary>
		/// <param name="manifold">The manifold.</param>
		/// <param name="polygonA">The polygon A.</param>
		/// <param name="transformA">The transform of A.</param>
		/// <param name="circleB">The circle B.</param>
		/// <param name="transformB">The transform of B.</param>
		public static void CollidePolygonAndCircle(ref Manifold manifold,
												   PolygonShape polygonA, ref Transform transformA,
												   CircleShape circleB, ref Transform transformB)
		{
			manifold.PointCount = 0;

			// Compute circle position in the frame of the polygon.
			Vector2 c =
				new Vector2(
					transformB.Position.X + transformB.R.Col1.X * circleB.Position.X +
					transformB.R.Col2.X * circleB.Position.Y,
					transformB.Position.Y + transformB.R.Col1.Y * circleB.Position.X +
					transformB.R.Col2.Y * circleB.Position.Y);
			Vector2 cLocal =
				new Vector2(
					(c.X - transformA.Position.X) * transformA.R.Col1.X +
					(c.Y - transformA.Position.Y) * transformA.R.Col1.Y,
					(c.X - transformA.Position.X) * transformA.R.Col2.X +
					(c.Y - transformA.Position.Y) * transformA.R.Col2.Y);

			// Find the min separating edge.
			int normalIndex = 0;
			float separation = -Settings.MaxFloat;
			float radius = polygonA.Radius + circleB.Radius;
			int vertexCount = polygonA.Vertices.Count;

			for (int i = 0; i < vertexCount; ++i)
			{
				Vector2 value1 = polygonA.Normals[i];
				Vector2 value2 = cLocal - polygonA.Vertices[i];
				float s = value1.X * value2.X + value1.Y * value2.Y;

				if (s > radius)
				{
					// Early out.
					return;
				}

				if (s > separation)
				{
					separation = s;
					normalIndex = i;
				}
			}

			// Vertices that subtend the incident face.
			int vertIndex1 = normalIndex;
			int vertIndex2 = vertIndex1 + 1 < vertexCount ? vertIndex1 + 1 : 0;
			Vector2 v1 = polygonA.Vertices[vertIndex1];
			Vector2 v2 = polygonA.Vertices[vertIndex2];

			// If the center is inside the polygon ...
			if (separation < Settings.Epsilon)
			{
				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalNormal = polygonA.Normals[normalIndex];
				manifold.LocalPoint = 0.5f * (v1 + v2);

				ManifoldPoint p0 = manifold.Points[0];

				p0.LocalPoint = circleB.Position;
				p0.Id.Key = 0;

				manifold.Points[0] = p0;

				return;
			}

			// Compute barycentric coordinates
			float u1 = (cLocal.X - v1.X) * (v2.X - v1.X) + (cLocal.Y - v1.Y) * (v2.Y - v1.Y);
			float u2 = (cLocal.X - v2.X) * (v1.X - v2.X) + (cLocal.Y - v2.Y) * (v1.Y - v2.Y);

			if (u1 <= 0.0f)
			{
				float r = (cLocal.X - v1.X) * (cLocal.X - v1.X) + (cLocal.Y - v1.Y) * (cLocal.Y - v1.Y);
				if (r > radius * radius)
				{
					return;
				}

				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalNormal = cLocal - v1;
				float factor = 1f /
							   (float)
							   Math.Sqrt(manifold.LocalNormal.X * manifold.LocalNormal.X +
										 manifold.LocalNormal.Y * manifold.LocalNormal.Y);
				manifold.LocalNormal.X = manifold.LocalNormal.X * factor;
				manifold.LocalNormal.Y = manifold.LocalNormal.Y * factor;
				manifold.LocalPoint = v1;

				ManifoldPoint p0b = manifold.Points[0];

				p0b.LocalPoint = circleB.Position;
				p0b.Id.Key = 0;

				manifold.Points[0] = p0b;
			}
			else if (u2 <= 0.0f)
			{
				float r = (cLocal.X - v2.X) * (cLocal.X - v2.X) + (cLocal.Y - v2.Y) * (cLocal.Y - v2.Y);
				if (r > radius * radius)
				{
					return;
				}

				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalNormal = cLocal - v2;
				float factor = 1f /
							   (float)
							   Math.Sqrt(manifold.LocalNormal.X * manifold.LocalNormal.X +
										 manifold.LocalNormal.Y * manifold.LocalNormal.Y);
				manifold.LocalNormal.X = manifold.LocalNormal.X * factor;
				manifold.LocalNormal.Y = manifold.LocalNormal.Y * factor;
				manifold.LocalPoint = v2;

				ManifoldPoint p0c = manifold.Points[0];

				p0c.LocalPoint = circleB.Position;
				p0c.Id.Key = 0;

				manifold.Points[0] = p0c;
			}
			else
			{
				Vector2 faceCenter = 0.5f * (v1 + v2);
				Vector2 value1 = cLocal - faceCenter;
				Vector2 value2 = polygonA.Normals[vertIndex1];
				float separation2 = value1.X * value2.X + value1.Y * value2.Y;
				if (separation2 > radius)
				{
					return;
				}

				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalNormal = polygonA.Normals[vertIndex1];
				manifold.LocalPoint = faceCenter;

				ManifoldPoint p0d = manifold.Points[0];

				p0d.LocalPoint = circleB.Position;
				p0d.Id.Key = 0;

				manifold.Points[0] = p0d;
			}
		}

		/// <summary>
		/// Compute the collision manifold between two polygons.
		/// </summary>
		/// <param name="manifold">The manifold.</param>
		/// <param name="polyA">The poly A.</param>
		/// <param name="transformA">The transform A.</param>
		/// <param name="polyB">The poly B.</param>
		/// <param name="transformB">The transform B.</param>
		public static void CollidePolygons(ref Manifold manifold,
										   PolygonShape polyA, ref Transform transformA,
										   PolygonShape polyB, ref Transform transformB)
		{
			manifold.PointCount = 0;
			float totalRadius = polyA.Radius + polyB.Radius;

			int edgeA = 0;
			float separationA = FindMaxSeparation(out edgeA, polyA, ref transformA, polyB, ref transformB);
			if (separationA > totalRadius)
				return;

			int edgeB = 0;
			float separationB = FindMaxSeparation(out edgeB, polyB, ref transformB, polyA, ref transformA);
			if (separationB > totalRadius)
				return;

			PolygonShape poly1; // reference polygon
			PolygonShape poly2; // incident polygon
			Transform xf1, xf2;
			int edge1; // reference edge
			bool flip;
			const float k_relativeTol = 0.98f;
			const float k_absoluteTol = 0.001f;

			if (separationB > k_relativeTol * separationA + k_absoluteTol)
			{
				poly1 = polyB;
				poly2 = polyA;
				xf1 = transformB;
				xf2 = transformA;
				edge1 = edgeB;
				manifold.Type = ManifoldType.FaceB;
				flip = true;
			}
			else
			{
				poly1 = polyA;
				poly2 = polyB;
				xf1 = transformA;
				xf2 = transformB;
				edge1 = edgeA;
				manifold.Type = ManifoldType.FaceA;
				flip = false;
			}

			FixedArray2<ClipVertex> incidentEdge;
			FindIncidentEdge(out incidentEdge, poly1, ref xf1, edge1, poly2, ref xf2);

			int count1 = poly1.Vertices.Count;

			int iv1 = edge1;
			int iv2 = edge1 + 1 < count1 ? edge1 + 1 : 0;

			Vector2 v11 = poly1.Vertices[iv1];
			Vector2 v12 = poly1.Vertices[iv2];

			float localTangentX = v12.X - v11.X;
			float localTangentY = v12.Y - v11.Y;

			float factor = 1f / (float)Math.Sqrt(localTangentX * localTangentX + localTangentY * localTangentY);
			localTangentX = localTangentX * factor;
			localTangentY = localTangentY * factor;

			Vector2 localNormal = new Vector2(localTangentY, -localTangentX);
			Vector2 planePoint = 0.5f * (v11 + v12);

			Vector2 tangent = new Vector2(xf1.R.Col1.X * localTangentX + xf1.R.Col2.X * localTangentY,
										  xf1.R.Col1.Y * localTangentX + xf1.R.Col2.Y * localTangentY);
			float normalx = tangent.Y;
			float normaly = -tangent.X;

			v11 = new Vector2(xf1.Position.X + xf1.R.Col1.X * v11.X + xf1.R.Col2.X * v11.Y,
							  xf1.Position.Y + xf1.R.Col1.Y * v11.X + xf1.R.Col2.Y * v11.Y);
			v12 = new Vector2(xf1.Position.X + xf1.R.Col1.X * v12.X + xf1.R.Col2.X * v12.Y,
							  xf1.Position.Y + xf1.R.Col1.Y * v12.X + xf1.R.Col2.Y * v12.Y);

			// Face offset.
			float frontOffset = normalx * v11.X + normaly * v11.Y;

			// Side offsets, extended by polytope skin thickness.
			float sideOffset1 = -(tangent.X * v11.X + tangent.Y * v11.Y) + totalRadius;
			float sideOffset2 = tangent.X * v12.X + tangent.Y * v12.Y + totalRadius;

			// Clip incident edge against extruded edge1 side edges.
			FixedArray2<ClipVertex> clipPoints1;
			FixedArray2<ClipVertex> clipPoints2;

			// Clip to box side 1
			int np = ClipSegmentToLine(out clipPoints1, ref incidentEdge, -tangent, sideOffset1, iv1);

			if (np < 2)
				return;

			// Clip to negative box side 1
			np = ClipSegmentToLine(out clipPoints2, ref clipPoints1, tangent, sideOffset2, iv2);

			if (np < 2)
			{
				return;
			}

			// Now clipPoints2 contains the clipped points.
			manifold.LocalNormal = localNormal;
			manifold.LocalPoint = planePoint;

			int pointCount = 0;
			for (int i = 0; i < Settings.MaxManifoldPoints; ++i)
			{
				Vector2 value = clipPoints2[i].V;
				float separation = normalx * value.X + normaly * value.Y - frontOffset;

				if (separation <= totalRadius)
				{
					ManifoldPoint cp = manifold.Points[pointCount];
					Vector2 tmp = clipPoints2[i].V;
					float tmp1X = tmp.X - xf2.Position.X;
					float tmp1Y = tmp.Y - xf2.Position.Y;
					cp.LocalPoint.X = tmp1X * xf2.R.Col1.X + tmp1Y * xf2.R.Col1.Y;
					cp.LocalPoint.Y = tmp1X * xf2.R.Col2.X + tmp1Y * xf2.R.Col2.Y;
					cp.Id = clipPoints2[i].ID;

					if (flip)
					{
						// Swap features
						ContactFeature cf = cp.Id.Features;
						cp.Id.Features.IndexA = cf.IndexB;
						cp.Id.Features.IndexB = cf.IndexA;
						cp.Id.Features.TypeA = cf.TypeB;
						cp.Id.Features.TypeB = cf.TypeA;
					}

					manifold.Points[pointCount] = cp;

					++pointCount;
				}
			}

			manifold.PointCount = pointCount;
		}

		/// <summary>
		/// Compute contact points for edge versus circle.
		/// This accounts for edge connectivity.
		/// </summary>
		/// <param name="manifold">The manifold.</param>
		/// <param name="edgeA">The edge A.</param>
		/// <param name="transformA">The transform A.</param>
		/// <param name="circleB">The circle B.</param>
		/// <param name="transformB">The transform B.</param>
		public static void CollideEdgeAndCircle(ref Manifold manifold,
												EdgeShape edgeA, ref Transform transformA,
												CircleShape circleB, ref Transform transformB)
		{
			manifold.PointCount = 0;

			// Compute circle in frame of edge
			Vector2 Q = MathUtils.MultiplyT(ref transformA, MathUtils.Multiply(ref transformB, ref circleB._position));

			Vector2 A = edgeA.Vertex1, B = edgeA.Vertex2;
			Vector2 e = B - A;

			// Barycentric coordinates
			float u = Vector2.Dot(e, B - Q);
			float v = Vector2.Dot(e, Q - A);

			float radius = edgeA.Radius + circleB.Radius;

			ContactFeature cf;
			cf.IndexB = 0;
			cf.TypeB = (byte)ContactFeatureType.Vertex;

			Vector2 P, d;

			// Region A
			if (v <= 0.0f)
			{
				P = A;
				d = Q - P;
				float dd;
				Vector2.Dot(ref d, ref d, out dd);
				if (dd > radius * radius)
				{
					return;
				}

				// Is there an edge connected to A?
				if (edgeA.HasVertex0)
				{
					Vector2 A1 = edgeA.Vertex0;
					Vector2 B1 = A;
					Vector2 e1 = B1 - A1;
					float u1 = Vector2.Dot(e1, B1 - Q);

					// Is the circle in Region AB of the previous edge?
					if (u1 > 0.0f)
					{
						return;
					}
				}

				cf.IndexA = 0;
				cf.TypeA = (byte)ContactFeatureType.Vertex;
				manifold.PointCount = 1;
				manifold.Type = ManifoldType.Circles;
				manifold.LocalNormal = Vector2.Zero;
				manifold.LocalPoint = P;
				ManifoldPoint mp = new ManifoldPoint();
				mp.Id.Key = 0;
				mp.Id.Features = cf;
				mp.LocalPoint = circleB.Position;
				manifold.Points[0] = mp;
				return;
			}

			// Region B
			if (u <= 0.0f)
			{
				P = B;
				d = Q - P;
				float dd;
				Vector2.Dot(ref d, ref d, out dd);
				if (dd > radius * radius)
				{
					return;
				}

				// Is there an edge connected to B?
				if (edgeA.HasVertex3)
				{
					Vector2 B2 = edgeA.Vertex3;
					Vector2 A2 = B;
					Vector2 e2 = B2 - A2;
					float v2 = Vector2.Dot(e2, Q - A2);

					// Is the circle in Region AB of the next edge?
					if (v2 > 0.0f)
					{
						return;
					}
				}

				cf.IndexA = 1;
				cf.TypeA = (byte)ContactFeatureType.Vertex;
				manifold.PointCount = 1;
				manifold.Type = ManifoldType.Circles;
				manifold.LocalNormal = Vector2.Zero;
				manifold.LocalPoint = P;
				ManifoldPoint mp = new ManifoldPoint();
				mp.Id.Key = 0;
				mp.Id.Features = cf;
				mp.LocalPoint = circleB.Position;
				manifold.Points[0] = mp;
				return;
			}

			// Region AB
			float den;
			Vector2.Dot(ref e, ref e, out den);
			Debug.Assert(den > 0.0f);
			P = (1.0f / den) * (u * A + v * B);
			d = Q - P;
			float dd2;
			Vector2.Dot(ref d, ref d, out dd2);
			if (dd2 > radius * radius)
			{
				return;
			}

			Vector2 n = new Vector2(-e.Y, e.X);
			if (Vector2.Dot(n, Q - A) < 0.0f)
			{
				n = new Vector2(-n.X, -n.Y);
			}
			n.Normalize();

			cf.IndexA = 0;
			cf.TypeA = (byte)ContactFeatureType.Face;
			manifold.PointCount = 1;
			manifold.Type = ManifoldType.FaceA;
			manifold.LocalNormal = n;
			manifold.LocalPoint = A;
			ManifoldPoint mp2 = new ManifoldPoint();
			mp2.Id.Key = 0;
			mp2.Id.Features = cf;
			mp2.LocalPoint = circleB.Position;
			manifold.Points[0] = mp2;
		}

		/// <summary>
		/// Collides and edge and a polygon, taking into account edge adjacency.
		/// </summary>
		/// <param name="manifold">The manifold.</param>
		/// <param name="edgeA">The edge A.</param>
		/// <param name="xfA">The xf A.</param>
		/// <param name="polygonB">The polygon B.</param>
		/// <param name="xfB">The xf B.</param>
		public static void CollideEdgeAndPolygon(ref Manifold manifold,
												 EdgeShape edgeA, ref Transform xfA,
												 PolygonShape polygonB, ref Transform xfB)
		{
			MathUtils.MultiplyT(ref xfA, ref xfB, out _xf);

			// Edge geometry
			_edgeA.V0 = edgeA.Vertex0;
			_edgeA.V1 = edgeA.Vertex1;
			_edgeA.V2 = edgeA.Vertex2;
			_edgeA.V3 = edgeA.Vertex3;
			Vector2 e = _edgeA.V2 - _edgeA.V1;

			// Normal points outwards in CCW order.
			_edgeA.Normal = new Vector2(e.Y, -e.X);
			_edgeA.Normal.Normalize();
			_edgeA.HasVertex0 = edgeA.HasVertex0;
			_edgeA.HasVertex3 = edgeA.HasVertex3;

			// Proxy for edge
			_proxyA.Vertices[0] = _edgeA.V1;
			_proxyA.Vertices[1] = _edgeA.V2;
			_proxyA.Normals[0] = _edgeA.Normal;
			_proxyA.Normals[1] = -_edgeA.Normal;
			_proxyA.Centroid = 0.5f * (_edgeA.V1 + _edgeA.V2);
			_proxyA.Count = 2;

			// Proxy for polygon
			_proxyB.Count = polygonB.Vertices.Count;
			_proxyB.Centroid = MathUtils.Multiply(ref _xf, ref polygonB.MassData.Centroid);
			for (int i = 0; i < polygonB.Vertices.Count; ++i)
			{
				_proxyB.Vertices[i] = MathUtils.Multiply(ref _xf, polygonB.Vertices[i]);
				_proxyB.Normals[i] = MathUtils.Multiply(ref _xf.R, polygonB.Normals[i]);
			}

			_radius = 2.0f * Settings.PolygonRadius;

			_limit11 = Vector2.Zero;
			_limit12 = Vector2.Zero;
			_limit21 = Vector2.Zero;
			_limit22 = Vector2.Zero;

			//Collide(ref manifold); inline start
			manifold.PointCount = 0;

			//ComputeAdjacency(); inline start
			Vector2 v0 = _edgeA.V0;
			Vector2 v1 = _edgeA.V1;
			Vector2 v2 = _edgeA.V2;
			Vector2 v3 = _edgeA.V3;

			// Determine allowable the normal regions based on adjacency.
			// Note: it may be possible that no normal is admissable.
			Vector2 centerB = _proxyB.Centroid;
			if (_edgeA.HasVertex0)
			{
				Vector2 e0 = v1 - v0;
				Vector2 e1 = v2 - v1;
				Vector2 n0 = new Vector2(e0.Y, -e0.X);
				Vector2 n1 = new Vector2(e1.Y, -e1.X);
				n0.Normalize();
				n1.Normalize();

				bool convex = MathUtils.Cross(n0, n1) >= 0.0f;
				bool front0 = Vector2.Dot(n0, centerB - v0) >= 0.0f;
				bool front1 = Vector2.Dot(n1, centerB - v1) >= 0.0f;

				if (convex)
				{
					if (front0 || front1)
					{
						_limit11 = n1;
						_limit12 = n0;
					}
					else
					{
						_limit11 = -n1;
						_limit12 = -n0;
					}
				}
				else
				{
					if (front0 && front1)
					{
						_limit11 = n0;
						_limit12 = n1;
					}
					else
					{
						_limit11 = -n0;
						_limit12 = -n1;
					}
				}
			}
			else
			{
				_limit11 = Vector2.Zero;
				_limit12 = Vector2.Zero;
			}

			if (_edgeA.HasVertex3)
			{
				Vector2 e1 = v2 - v1;
				Vector2 e2 = v3 - v2;
				Vector2 n1 = new Vector2(e1.Y, -e1.X);
				Vector2 n2 = new Vector2(e2.Y, -e2.X);
				n1.Normalize();
				n2.Normalize();

				bool convex = MathUtils.Cross(n1, n2) >= 0.0f;
				bool front1 = Vector2.Dot(n1, centerB - v1) >= 0.0f;
				bool front2 = Vector2.Dot(n2, centerB - v2) >= 0.0f;

				if (convex)
				{
					if (front1 || front2)
					{
						_limit21 = n2;
						_limit22 = n1;
					}
					else
					{
						_limit21 = -n2;
						_limit22 = -n1;
					}
				}
				else
				{
					if (front1 && front2)
					{
						_limit21 = n1;
						_limit22 = n2;
					}
					else
					{
						_limit21 = -n1;
						_limit22 = -n2;
					}
				}
			}
			else
			{
				_limit21 = Vector2.Zero;
				_limit22 = Vector2.Zero;
			}

			//ComputeAdjacency(); inline end

			//EPAxis edgeAxis = ComputeEdgeSeparation(); inline start
			EPAxis edgeAxis = ComputeEdgeSeparation();

			// If no valid normal can be found than this edge should not collide.
			// This can happen on the middle edge of a 3-edge zig-zag chain.
			if (edgeAxis.Type == EPAxisType.Unknown)
			{
				return;
			}

			if (edgeAxis.Separation > _radius)
			{
				return;
			}

			EPAxis polygonAxis = ComputePolygonSeparation();
			if (polygonAxis.Type != EPAxisType.Unknown && polygonAxis.Separation > _radius)
			{
				return;
			}

			// Use hysteresis for jitter reduction.
			const float k_relativeTol = 0.98f;
			const float k_absoluteTol = 0.001f;

			EPAxis primaryAxis;
			if (polygonAxis.Type == EPAxisType.Unknown)
			{
				primaryAxis = edgeAxis;
			}
			else if (polygonAxis.Separation > k_relativeTol * edgeAxis.Separation + k_absoluteTol)
			{
				primaryAxis = polygonAxis;
			}
			else
			{
				primaryAxis = edgeAxis;
			}

			EPProxy proxy1;
			EPProxy proxy2;
			FixedArray2<ClipVertex> incidentEdge = new FixedArray2<ClipVertex>();
			if (primaryAxis.Type == EPAxisType.EdgeA)
			{
				proxy1 = _proxyA;
				proxy2 = _proxyB;
				manifold.Type = ManifoldType.FaceA;
			}
			else
			{
				proxy1 = _proxyB;
				proxy2 = _proxyA;
				manifold.Type = ManifoldType.FaceB;
			}

			int edge1 = primaryAxis.Index;

			FindIncidentEdge(ref incidentEdge, proxy1, primaryAxis.Index, proxy2);
			int count1 = proxy1.Count;

			int iv1 = edge1;
			int iv2 = edge1 + 1 < count1 ? edge1 + 1 : 0;

			Vector2 v11 = proxy1.Vertices[iv1];
			Vector2 v12 = proxy1.Vertices[iv2];

			Vector2 tangent = v12 - v11;
			tangent.Normalize();

			Vector2 normal = MathUtils.Cross(tangent, 1.0f);
			Vector2 planePoint = 0.5f * (v11 + v12);

			// Face offset.
			float frontOffset = Vector2.Dot(normal, v11);

			// Side offsets, extended by polytope skin thickness.
			float sideOffset1 = -Vector2.Dot(tangent, v11) + _radius;
			float sideOffset2 = Vector2.Dot(tangent, v12) + _radius;

			// Clip incident edge against extruded edge1 side edges.
			FixedArray2<ClipVertex> clipPoints1;
			FixedArray2<ClipVertex> clipPoints2;
			int np;

			// Clip to box side 1
			np = ClipSegmentToLine(out clipPoints1, ref incidentEdge, -tangent, sideOffset1, iv1);

			if (np < Settings.MaxManifoldPoints)
			{
				return;
			}

			// Clip to negative box side 1
			np = ClipSegmentToLine(out clipPoints2, ref clipPoints1, tangent, sideOffset2, iv2);

			if (np < Settings.MaxManifoldPoints)
			{
				return;
			}

			// Now clipPoints2 contains the clipped points.
			if (primaryAxis.Type == EPAxisType.EdgeA)
			{
				manifold.LocalNormal = normal;
				manifold.LocalPoint = planePoint;
			}
			else
			{
				manifold.LocalNormal = MathUtils.MultiplyT(ref _xf.R, ref normal);
				manifold.LocalPoint = MathUtils.MultiplyT(ref _xf, ref planePoint);
			}

			int pointCount = 0;
			for (int i1 = 0; i1 < Settings.MaxManifoldPoints; ++i1)
			{
				float separation = Vector2.Dot(normal, clipPoints2[i1].V) - frontOffset;

				if (separation <= _radius)
				{
					ManifoldPoint cp = manifold.Points[pointCount];

					if (primaryAxis.Type == EPAxisType.EdgeA)
					{
						cp.LocalPoint = MathUtils.MultiplyT(ref _xf, clipPoints2[i1].V);
						cp.Id = clipPoints2[i1].ID;
					}
					else
					{
						cp.LocalPoint = clipPoints2[i1].V;
						cp.Id.Features.TypeA = clipPoints2[i1].ID.Features.TypeB;
						cp.Id.Features.TypeB = clipPoints2[i1].ID.Features.TypeA;
						cp.Id.Features.IndexA = clipPoints2[i1].ID.Features.IndexB;
						cp.Id.Features.IndexB = clipPoints2[i1].ID.Features.IndexA;
					}

					manifold.Points[pointCount] = cp;

					++pointCount;
				}
			}

			manifold.PointCount = pointCount;

			//Collide(ref manifold); inline end
		}

		private static EPAxis ComputeEdgeSeparation()
		{
			// EdgeA separation
			EPAxis bestAxis;
			bestAxis.Type = EPAxisType.Unknown;
			bestAxis.Index = -1;
			bestAxis.Separation = -Settings.MaxFloat;
			_tmpNormals[0] = _edgeA.Normal;
			_tmpNormals[1] = -_edgeA.Normal;

			for (int i = 0; i < 2; ++i)
			{
				Vector2 n = _tmpNormals[i];

				// Adjacency
				bool valid1 = MathUtils.Cross(n, _limit11) >= -Settings.AngularSlop &&
							  MathUtils.Cross(_limit12, n) >= -Settings.AngularSlop;
				bool valid2 = MathUtils.Cross(n, _limit21) >= -Settings.AngularSlop &&
							  MathUtils.Cross(_limit22, n) >= -Settings.AngularSlop;

				if (valid1 == false || valid2 == false)
				{
					continue;
				}

				EPAxis axis;
				axis.Type = EPAxisType.EdgeA;
				axis.Index = i;
				axis.Separation = Settings.MaxFloat;

				for (int j = 0; j < _proxyB.Count; ++j)
				{
					float s = Vector2.Dot(n, _proxyB.Vertices[j] - _edgeA.V1);
					if (s < axis.Separation)
					{
						axis.Separation = s;
					}
				}

				if (axis.Separation > _radius)
				{
					return axis;
				}

				if (axis.Separation > bestAxis.Separation)
				{
					bestAxis = axis;
				}
			}

			return bestAxis;
		}

		private static EPAxis ComputePolygonSeparation()
		{
			EPAxis axis;
			axis.Type = EPAxisType.Unknown;
			axis.Index = -1;
			axis.Separation = -Settings.MaxFloat;
			for (int i = 0; i < _proxyB.Count; ++i)
			{
				Vector2 n = -_proxyB.Normals[i];

				// Adjacency
				bool valid1 = MathUtils.Cross(n, _limit11) >= -Settings.AngularSlop &&
							  MathUtils.Cross(_limit12, n) >= -Settings.AngularSlop;
				bool valid2 = MathUtils.Cross(n, _limit21) >= -Settings.AngularSlop &&
							  MathUtils.Cross(_limit22, n) >= -Settings.AngularSlop;

				if (valid1 == false && valid2 == false)
				{
					continue;
				}

				float s1 = Vector2.Dot(n, _proxyB.Vertices[i] - _edgeA.V1);
				float s2 = Vector2.Dot(n, _proxyB.Vertices[i] - _edgeA.V2);
				float s = Math.Min(s1, s2);

				if (s > _radius)
				{
					axis.Type = EPAxisType.EdgeB;
					axis.Index = i;
					axis.Separation = s;
				}

				if (s > axis.Separation)
				{
					axis.Type = EPAxisType.EdgeB;
					axis.Index = i;
					axis.Separation = s;
				}
			}

			return axis;
		}

		private static void FindIncidentEdge(ref FixedArray2<ClipVertex> c, EPProxy proxy1, int edge1, EPProxy proxy2)
		{
			int count2 = proxy2.Count;

			Debug.Assert(0 <= edge1 && edge1 < proxy1.Count);

			// Get the normal of the reference edge in proxy2's frame.
			Vector2 normal1 = proxy1.Normals[edge1];

			// Find the incident edge on proxy2.
			int index = 0;
			float minDot = float.MaxValue;
			for (int i = 0; i < count2; ++i)
			{
				float dot = Vector2.Dot(normal1, proxy2.Normals[i]);
				if (dot < minDot)
				{
					minDot = dot;
					index = i;
				}
			}

			// Build the clip vertices for the incident edge.
			int i1 = index;
			int i2 = i1 + 1 < count2 ? i1 + 1 : 0;

			ClipVertex cTemp = new ClipVertex();
			cTemp.V = proxy2.Vertices[i1];
			cTemp.ID.Features.IndexA = (byte)edge1;
			cTemp.ID.Features.IndexB = (byte)i1;
			cTemp.ID.Features.TypeA = (byte)ContactFeatureType.Face;
			cTemp.ID.Features.TypeB = (byte)ContactFeatureType.Vertex;
			c[0] = cTemp;

			cTemp.V = proxy2.Vertices[i2];
			cTemp.ID.Features.IndexA = (byte)edge1;
			cTemp.ID.Features.IndexB = (byte)i2;
			cTemp.ID.Features.TypeA = (byte)ContactFeatureType.Face;
			cTemp.ID.Features.TypeB = (byte)ContactFeatureType.Vertex;
			c[1] = cTemp;
		}

		/// <summary>
		/// Clipping for contact manifolds.
		/// </summary>
		/// <param name="vOut">The v out.</param>
		/// <param name="vIn">The v in.</param>
		/// <param name="normal">The normal.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="vertexIndexA">The vertex index A.</param>
		private static int ClipSegmentToLine(out FixedArray2<ClipVertex> vOut, ref FixedArray2<ClipVertex> vIn,
											 Vector2 normal, float offset, int vertexIndexA)
		{
			vOut = new FixedArray2<ClipVertex>();

			ClipVertex v0 = vIn[0];
			ClipVertex v1 = vIn[1];

			// Start with no output points
			int numOut = 0;

			// Calculate the distance of end points to the line
			float distance0 = normal.X * v0.V.X + normal.Y * v0.V.Y - offset;
			float distance1 = normal.X * v1.V.X + normal.Y * v1.V.Y - offset;

			// If the points are behind the plane
			if (distance0 <= 0.0f) vOut[numOut++] = v0;
			if (distance1 <= 0.0f) vOut[numOut++] = v1;

			// If the points are on different sides of the plane
			if (distance0 * distance1 < 0.0f)
			{
				// Find intersection point of edge and plane
				float interp = distance0 / (distance0 - distance1);

				ClipVertex cv = vOut[numOut];

				cv.V.X = v0.V.X + interp * (v1.V.X - v0.V.X);
				cv.V.Y = v0.V.Y + interp * (v1.V.Y - v0.V.Y);

				// VertexA is hitting edgeB.
				cv.ID.Features.IndexA = (byte)vertexIndexA;
				cv.ID.Features.IndexB = v0.ID.Features.IndexB;
				cv.ID.Features.TypeA = (byte)ContactFeatureType.Vertex;
				cv.ID.Features.TypeB = (byte)ContactFeatureType.Face;

				vOut[numOut] = cv;

				++numOut;
			}

			return numOut;
		}

		/// <summary>
		/// Find the separation between poly1 and poly2 for a give edge normal on poly1.
		/// </summary>
		/// <param name="poly1">The poly1.</param>
		/// <param name="xf1">The XF1.</param>
		/// <param name="edge1">The edge1.</param>
		/// <param name="poly2">The poly2.</param>
		/// <param name="xf2">The XF2.</param>
		private static float EdgeSeparation(PolygonShape poly1, ref Transform xf1, int edge1,
											PolygonShape poly2, ref Transform xf2)
		{
			int count2 = poly2.Vertices.Count;

			Debug.Assert(0 <= edge1 && edge1 < poly1.Vertices.Count);

			// Convert normal from poly1's frame into poly2's frame.
			Vector2 p1n = poly1.Normals[edge1];

			float normalWorldx = xf1.R.Col1.X * p1n.X + xf1.R.Col2.X * p1n.Y;
			float normalWorldy = xf1.R.Col1.Y * p1n.X + xf1.R.Col2.Y * p1n.Y;

			Vector2 normal = new Vector2(normalWorldx * xf2.R.Col1.X + normalWorldy * xf2.R.Col1.Y,
										 normalWorldx * xf2.R.Col2.X + normalWorldy * xf2.R.Col2.Y);

			// Find support vertex on poly2 for -normal.
			int index = 0;
			float minDot = Settings.MaxFloat;

			for (int i = 0; i < count2; ++i)
			{
				float dot = Vector2.Dot(poly2.Vertices[i], normal);

				if (dot < minDot)
				{
					minDot = dot;
					index = i;
				}
			}

			Vector2 p1ve = poly1.Vertices[edge1];
			Vector2 p2vi = poly2.Vertices[index];

			return ((xf2.Position.X + xf2.R.Col1.X * p2vi.X + xf2.R.Col2.X * p2vi.Y) -
					(xf1.Position.X + xf1.R.Col1.X * p1ve.X + xf1.R.Col2.X * p1ve.Y)) * normalWorldx +
				   ((xf2.Position.Y + xf2.R.Col1.Y * p2vi.X + xf2.R.Col2.Y * p2vi.Y) -
					(xf1.Position.Y + xf1.R.Col1.Y * p1ve.X + xf1.R.Col2.Y * p1ve.Y)) * normalWorldy;
		}

		/// <summary>
		/// Find the max separation between poly1 and poly2 using edge normals from poly1.
		/// </summary>
		/// <param name="edgeIndex">Index of the edge.</param>
		/// <param name="poly1">The poly1.</param>
		/// <param name="xf1">The XF1.</param>
		/// <param name="poly2">The poly2.</param>
		/// <param name="xf2">The XF2.</param>
		private static float FindMaxSeparation(out int edgeIndex,
											   PolygonShape poly1, ref Transform xf1,
											   PolygonShape poly2, ref Transform xf2)
		{
			int count1 = poly1.Vertices.Count;

			// Vector pointing from the centroid of poly1 to the centroid of poly2.
			float dx = (xf2.Position.X + xf2.R.Col1.X * poly2.MassData.Centroid.X +
						xf2.R.Col2.X * poly2.MassData.Centroid.Y) -
					   (xf1.Position.X + xf1.R.Col1.X * poly1.MassData.Centroid.X +
						xf1.R.Col2.X * poly1.MassData.Centroid.Y);
			float dy = (xf2.Position.Y + xf2.R.Col1.Y * poly2.MassData.Centroid.X +
						xf2.R.Col2.Y * poly2.MassData.Centroid.Y) -
					   (xf1.Position.Y + xf1.R.Col1.Y * poly1.MassData.Centroid.X +
						xf1.R.Col2.Y * poly1.MassData.Centroid.Y);
			Vector2 dLocal1 = new Vector2(dx * xf1.R.Col1.X + dy * xf1.R.Col1.Y, dx * xf1.R.Col2.X + dy * xf1.R.Col2.Y);

			// Find edge normal on poly1 that has the largest projection onto d.
			int edge = 0;
			float maxDot = -Settings.MaxFloat;
			for (int i = 0; i < count1; ++i)
			{
				float dot = Vector2.Dot(poly1.Normals[i], dLocal1);
				if (dot > maxDot)
				{
					maxDot = dot;
					edge = i;
				}
			}

			// Get the separation for the edge normal.
			float s = EdgeSeparation(poly1, ref xf1, edge, poly2, ref xf2);

			// Check the separation for the previous edge normal.
			int prevEdge = edge - 1 >= 0 ? edge - 1 : count1 - 1;
			float sPrev = EdgeSeparation(poly1, ref xf1, prevEdge, poly2, ref xf2);

			// Check the separation for the next edge normal.
			int nextEdge = edge + 1 < count1 ? edge + 1 : 0;
			float sNext = EdgeSeparation(poly1, ref xf1, nextEdge, poly2, ref xf2);

			// Find the best edge and the search direction.
			int bestEdge;
			float bestSeparation;
			int increment;
			if (sPrev > s && sPrev > sNext)
			{
				increment = -1;
				bestEdge = prevEdge;
				bestSeparation = sPrev;
			}
			else if (sNext > s)
			{
				increment = 1;
				bestEdge = nextEdge;
				bestSeparation = sNext;
			}
			else
			{
				edgeIndex = edge;
				return s;
			}

			// Perform a local search for the best edge normal.
			for (; ; )
			{
				if (increment == -1)
					edge = bestEdge - 1 >= 0 ? bestEdge - 1 : count1 - 1;
				else
					edge = bestEdge + 1 < count1 ? bestEdge + 1 : 0;

				s = EdgeSeparation(poly1, ref xf1, edge, poly2, ref xf2);

				if (s > bestSeparation)
				{
					bestEdge = edge;
					bestSeparation = s;
				}
				else
				{
					break;
				}
			}

			edgeIndex = bestEdge;
			return bestSeparation;
		}

		private static void FindIncidentEdge(out FixedArray2<ClipVertex> c,
											 PolygonShape poly1, ref Transform xf1, int edge1,
											 PolygonShape poly2, ref Transform xf2)
		{
			c = new FixedArray2<ClipVertex>();

			int count2 = poly2.Vertices.Count;

			Debug.Assert(0 <= edge1 && edge1 < poly1.Vertices.Count);

			// Get the normal of the reference edge in poly2's frame.
			Vector2 v = poly1.Normals[edge1];
			float tmpx = xf1.R.Col1.X * v.X + xf1.R.Col2.X * v.Y;
			float tmpy = xf1.R.Col1.Y * v.X + xf1.R.Col2.Y * v.Y;
			Vector2 normal1 = new Vector2(tmpx * xf2.R.Col1.X + tmpy * xf2.R.Col1.Y,
										  tmpx * xf2.R.Col2.X + tmpy * xf2.R.Col2.Y);

			// Find the incident edge on poly2.
			int index = 0;
			float minDot = Settings.MaxFloat;
			for (int i = 0; i < count2; ++i)
			{
				float dot = Vector2.Dot(normal1, poly2.Normals[i]);
				if (dot < minDot)
				{
					minDot = dot;
					index = i;
				}
			}

			// Build the clip vertices for the incident edge.
			int i1 = index;
			int i2 = i1 + 1 < count2 ? i1 + 1 : 0;

			ClipVertex cv0 = c[0];

			Vector2 v1 = poly2.Vertices[i1];
			cv0.V.X = xf2.Position.X + xf2.R.Col1.X * v1.X + xf2.R.Col2.X * v1.Y;
			cv0.V.Y = xf2.Position.Y + xf2.R.Col1.Y * v1.X + xf2.R.Col2.Y * v1.Y;
			cv0.ID.Features.IndexA = (byte)edge1;
			cv0.ID.Features.IndexB = (byte)i1;
			cv0.ID.Features.TypeA = (byte)ContactFeatureType.Face;
			cv0.ID.Features.TypeB = (byte)ContactFeatureType.Vertex;

			c[0] = cv0;

			ClipVertex cv1 = c[1];
			Vector2 v2 = poly2.Vertices[i2];
			cv1.V.X = xf2.Position.X + xf2.R.Col1.X * v2.X + xf2.R.Col2.X * v2.Y;
			cv1.V.Y = xf2.Position.Y + xf2.R.Col1.Y * v2.X + xf2.R.Col2.Y * v2.Y;
			cv1.ID.Features.IndexA = (byte)edge1;
			cv1.ID.Features.IndexB = (byte)i2;
			cv1.ID.Features.TypeA = (byte)ContactFeatureType.Face;
			cv1.ID.Features.TypeB = (byte)ContactFeatureType.Vertex;

			c[1] = cv1;
		}
	}
}