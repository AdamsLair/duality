﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FarseerPhysics.Collision;
using Duality;

namespace FarseerPhysics.Common
{
	public enum PolygonError
	{
		/// <summary>
		/// There were no errors in the polygon
		/// </summary>
		NoError,

		/// <summary>
		/// Polygon must have between 3 and Settings.MaxPolygonVertices vertices.
		/// </summary>
		InvalidAmountOfVertices,

		/// <summary>
		/// Polygon must be simple. This means no overlapping edges.
		/// </summary>
		NotSimple,

		/// <summary>
		/// Polygon must have a counter clockwise winding.
		/// </summary>
		NotCounterClockWise,

		/// <summary>
		/// The polygon is concave, it needs to be convex.
		/// </summary>
		NotConvex,

		/// <summary>
		/// Polygon area is too small.
		/// </summary>
		AreaTooSmall,

		/// <summary>
		/// The polygon has a side that is too short.
		/// </summary>
		SideTooSmall
	}

	[DebuggerDisplay("Count = {Count} Vertices = {ToString()}")]
	public class Vertices : List<Vector2>
	{
		public Vertices()
		{
		}

		public Vertices(int capacity)
		{
			this.Capacity = capacity;
		}

		public Vertices(Vector2[] vector2)
		{
			for (int i = 0; i < vector2.Length; i++)
			{
				Add(vector2[i]);
			}
		}

		public Vertices(IList<Vector2> vertices)
		{
			for (int i = 0; i < vertices.Count; i++)
			{
				Add(vertices[i]);
			}
		}

		/// <summary>
		/// Nexts the index.
		/// </summary>
		/// <param name="index">The index.</param>
		public int NextIndex(int index)
		{
			if (index == this.Count - 1)
			{
				return 0;
			}
			return index + 1;
		}

		public Vector2 NextVertex(int index)
		{
			return this[NextIndex(index)];
		}

		/// <summary>
		/// Gets the previous index.
		/// </summary>
		/// <param name="index">The index.</param>
		public int PreviousIndex(int index)
		{
			if (index == 0)
			{
				return this.Count - 1;
			}
			return index - 1;
		}

		public Vector2 PreviousVertex(int index)
		{
			return this[PreviousIndex(index)];
		}

		/// <summary>
		/// Gets the signed area.
		/// </summary>
		public float GetSignedArea()
		{
			int i;
			float area = 0;

			for (i = 0; i < this.Count; i++)
			{
				int j = (i + 1) % this.Count;
				area += this[i].X * this[j].Y;
				area -= this[i].Y * this[j].X;
			}
			area /= 2.0f;
			return area;
		}

		/// <summary>
		/// Gets the area.
		/// </summary>
		public float GetArea()
		{
			int i;
			float area = 0;

			for (i = 0; i < this.Count; i++)
			{
				int j = (i + 1) % this.Count;
				area += this[i].X * this[j].Y;
				area -= this[i].Y * this[j].X;
			}
			area /= 2.0f;
			return (area < 0 ? -area : area);
		}

		/// <summary>
		/// Gets the centroid.
		/// </summary>
		public Vector2 GetCentroid()
		{
			// Same algorithm is used by Box2D

			Vector2 c = Vector2.Zero;
			float area = 0.0f;

			const float inv3 = 1.0f / 3.0f;
			Vector2 pRef = Vector2.Zero;
			for (int i = 0; i < this.Count; ++i)
			{
				// Triangle vertices.
				Vector2 p1 = pRef;
				Vector2 p2 = this[i];
				Vector2 p3 = i + 1 < this.Count ? this[i + 1] : this[0];

				Vector2 e1 = p2 - p1;
				Vector2 e2 = p3 - p1;

				float D = MathUtils.Cross(e1, e2);

				float triangleArea = 0.5f * D;
				area += triangleArea;

				// Area weighted centroid
				c += triangleArea * inv3 * (p1 + p2 + p3);
			}

			// Centroid
			c *= 1.0f / area;
			return c;
		}

		/// <summary>
		/// Gets the radius based on area.
		/// </summary>
		public float GetRadius()
		{
			float area = GetSignedArea();

			double radiusSqrd = (double)area / MathHelper.Pi;
			if (radiusSqrd < 0)
			{
				radiusSqrd *= -1;
			}

			return (float)Math.Sqrt(radiusSqrd);
		}

		/// <summary>
		/// Returns an AABB for vertex.
		/// </summary>
		public AABB GetCollisionBox()
		{
			AABB aabb;
			Vector2 lowerBound = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 upperBound = new Vector2(float.MinValue, float.MinValue);

			for (int i = 0; i < this.Count; ++i)
			{
				if (this[i].X < lowerBound.X)
				{
					lowerBound.X = this[i].X;
				}
				if (this[i].X > upperBound.X)
				{
					upperBound.X = this[i].X;
				}

				if (this[i].Y < lowerBound.Y)
				{
					lowerBound.Y = this[i].Y;
				}
				if (this[i].Y > upperBound.Y)
				{
					upperBound.Y = this[i].Y;
				}
			}

			aabb.LowerBound = lowerBound;
			aabb.UpperBound = upperBound;

			return aabb;
		}

		public void Translate(Vector2 vector)
		{
			Translate(ref vector);
		}

		/// <summary>
		/// Translates the vertices with the specified vector.
		/// </summary>
		/// <param name="vector">The vector.</param>
		public void Translate(ref Vector2 vector)
		{
			for (int i = 0; i < this.Count; i++)
				this[i] = this[i] + vector;
		}

		/// <summary>
		/// Scales the vertices with the specified vector.
		/// </summary>
		/// <param name="value">The Value.</param>
		public void Scale(ref Vector2 value)
		{
			for (int i = 0; i < this.Count; i++)
				this[i] = this[i] * value;
		}

		/// <summary>
		/// Rotate the vertices with the defined value in radians.
		/// </summary>
		/// <param name="value">The amount to rotate by in radians.</param>
		public void Rotate(float value)
		{
			Matrix4 rotationMatrix;
			Matrix4.CreateRotationZ(value, out rotationMatrix);

			for (int i = 0; i < this.Count; i++)
				this[i] = Vector2.Transform(this[i], rotationMatrix);
		}

		/// <summary>
		/// Assuming the polygon is simple; determines whether the polygon is convex.
		/// NOTE: It will also return false if the input contains colinear edges.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if it is convex; otherwise, <c>false</c>.
		/// </returns>
		public bool IsConvex()
		{
			// Ensure the polygon is convex and the interior
			// is to the left of each edge.
			for (int i = 0; i < this.Count; ++i)
			{
				int i1 = i;
				int i2 = i + 1 < this.Count ? i + 1 : 0;
				Vector2 edge = this[i2] - this[i1];

				for (int j = 0; j < this.Count; ++j)
				{
					// Don't check vertices on the current edge.
					if (j == i1 || j == i2)
					{
						continue;
					}

					Vector2 r = this[j] - this[i1];

					float s = edge.X * r.Y - edge.Y * r.X;

					if (s <= 0.0f)
						return false;
				}
			}
			return true;
		}

		public bool IsCounterClockWise()
		{
			//We just return true for lines
			if (this.Count < 3)
				return true;

			return (GetSignedArea() > 0.0f);
		}

		/// <summary>
		/// Forces counter clock wise order.
		/// </summary>
		public void ForceCounterClockWise()
		{
			if (!IsCounterClockWise())
			{
				Reverse();
			}
		}

		/// <summary>
		/// Check for edge crossings
		/// </summary>
		public bool IsSimple()
		{
			for (int i = 0; i < this.Count; ++i)
			{
				int iplus = (i + 1 > this.Count - 1) ? 0 : i + 1;
				Vector2 a1 = new Vector2(this[i].X, this[i].Y);
				Vector2 a2 = new Vector2(this[iplus].X, this[iplus].Y);
				for (int j = i + 1; j < this.Count; ++j)
				{
					int jplus = (j + 1 > this.Count - 1) ? 0 : j + 1;
					Vector2 b1 = new Vector2(this[j].X, this[j].Y);
					Vector2 b2 = new Vector2(this[jplus].X, this[jplus].Y);

					Vector2 temp;

					if (LineTools.LineIntersect2(a1, a2, b1, b2, out temp))
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Checks if the polygon is valid for use in the engine.
		///
		/// Performs a full check, for simplicity, convexity,
		/// orientation, minimum angle, and volume.
		/// 
		/// From Eric Jordan's convex decomposition library
		/// </summary>
		/// <returns>PolygonError.NoError if there were no error.</returns>
		public PolygonError CheckPolygon()
		{
			if (this.Count < 3 || this.Count > Settings.MaxPolygonVertices)
				return PolygonError.InvalidAmountOfVertices;

			if (!IsSimple())
				return PolygonError.NotSimple;

			if (GetArea() <= Settings.Epsilon)
				return PolygonError.AreaTooSmall;

			if (!IsConvex())
				return PolygonError.NotConvex;

			//Check if the sides are of adequate length.
			for (int i = 0; i < this.Count; ++i)
			{
				int next = i + 1 < this.Count ? i + 1 : 0;
				Vector2 edge = this[next] - this[i];
				if (edge.LengthSquared <= Settings.Epsilon * Settings.Epsilon)
				{
					return PolygonError.SideTooSmall;
				}
			}

			if (!IsCounterClockWise())
				return PolygonError.NotCounterClockWise;

			return PolygonError.NoError;
		}

		// From Eric Jordan's convex decomposition library

		/// <summary>
		/// Trace the edge of a non-simple polygon and return a simple polygon.
		/// 
		/// Method:
		/// Start at vertex with minimum y (pick maximum x one if there are two).
		/// We aim our "lastDir" vector at (1.0, 0)
		/// We look at the two rays going off from our start vertex, and follow whichever
		/// has the smallest angle (in -Pi . Pi) wrt lastDir ("rightest" turn)
		/// Loop until we hit starting vertex:
		/// We add our current vertex to the list.
		/// We check the seg from current vertex to next vertex for intersections
		/// - if no intersections, follow to next vertex and continue
		/// - if intersections, pick one with minimum distance
		/// - if more than one, pick one with "rightest" next point (two possibilities for each)
		/// </summary>
		/// <param name="verts">The vertices.</param>
		public Vertices TraceEdge(Vertices verts)
		{
			PolyNode[] nodes = new PolyNode[verts.Count * verts.Count];
			//overkill, but sufficient (order of mag. is right)
			int nNodes = 0;

			//Add base nodes (raw outline)
			for (int i = 0; i < verts.Count; ++i)
			{
				Vector2 pos = new Vector2(verts[i].X, verts[i].Y);
				nodes[i].Position = pos;
				++nNodes;
				int iplus = (i == verts.Count - 1) ? 0 : i + 1;
				int iminus = (i == 0) ? verts.Count - 1 : i - 1;
				nodes[i].AddConnection(nodes[iplus]);
				nodes[i].AddConnection(nodes[iminus]);
			}

			//Process intersection nodes - horribly inefficient
			bool dirty = true;
			int counter = 0;
			while (dirty)
			{
				dirty = false;
				for (int i = 0; i < nNodes; ++i)
				{
					for (int j = 0; j < nodes[i].NConnected; ++j)
					{
						for (int k = 0; k < nNodes; ++k)
						{
							if (k == i || nodes[k] == nodes[i].Connected[j]) continue;
							for (int l = 0; l < nodes[k].NConnected; ++l)
							{
								if (nodes[k].Connected[l] == nodes[i].Connected[j] ||
									nodes[k].Connected[l] == nodes[i]) continue;

								//Check intersection
								Vector2 intersectPt;

								bool crosses = LineTools.LineIntersect(nodes[i].Position, nodes[i].Connected[j].Position,
																	   nodes[k].Position, nodes[k].Connected[l].Position,
																	   out intersectPt);
								if (crosses)
								{
									dirty = true;
									//Destroy and re-hook connections at crossing point
									PolyNode connj = nodes[i].Connected[j];
									PolyNode connl = nodes[k].Connected[l];
									nodes[i].Connected[j].RemoveConnection(nodes[i]);
									nodes[i].RemoveConnection(connj);
									nodes[k].Connected[l].RemoveConnection(nodes[k]);
									nodes[k].RemoveConnection(connl);
									nodes[nNodes] = new PolyNode(intersectPt);
									nodes[nNodes].AddConnection(nodes[i]);
									nodes[i].AddConnection(nodes[nNodes]);
									nodes[nNodes].AddConnection(nodes[k]);
									nodes[k].AddConnection(nodes[nNodes]);
									nodes[nNodes].AddConnection(connj);
									connj.AddConnection(nodes[nNodes]);
									nodes[nNodes].AddConnection(connl);
									connl.AddConnection(nodes[nNodes]);
									++nNodes;
									goto SkipOut;
								}
							}
						}
					}
				}
				SkipOut:
				++counter;
			}

			//Collapse duplicate points
			bool foundDupe = true;
			int nActive = nNodes;
			while (foundDupe)
			{
				foundDupe = false;
				for (int i = 0; i < nNodes; ++i)
				{
					if (nodes[i].NConnected == 0) continue;
					for (int j = i + 1; j < nNodes; ++j)
					{
						if (nodes[j].NConnected == 0) continue;
						Vector2 diff = nodes[i].Position - nodes[j].Position;
						if (diff.LengthSquared <= Settings.Epsilon * Settings.Epsilon)
						{
							if (nActive <= 3)
								return new Vertices();

							//printf("Found dupe, %d left\n",nActive);
							--nActive;
							foundDupe = true;
							PolyNode inode = nodes[i];
							PolyNode jnode = nodes[j];
							//Move all of j's connections to i, and orphan j
							int njConn = jnode.NConnected;
							for (int k = 0; k < njConn; ++k)
							{
								PolyNode knode = jnode.Connected[k];
								Debug.Assert(knode != jnode);
								if (knode != inode)
								{
									inode.AddConnection(knode);
									knode.AddConnection(inode);
								}
								knode.RemoveConnection(jnode);
							}
							jnode.NConnected = 0;
						}
					}
				}
			}

			//Now walk the edge of the list

			//Find node with minimum y value (max x if equal)
			float minY = float.MaxValue;
			float maxX = -float.MaxValue;
			int minYIndex = -1;
			for (int i = 0; i < nNodes; ++i)
			{
				if (nodes[i].Position.Y < minY && nodes[i].NConnected > 1)
				{
					minY = nodes[i].Position.Y;
					minYIndex = i;
					maxX = nodes[i].Position.X;
				}
				else if (nodes[i].Position.Y == minY && nodes[i].Position.X > maxX && nodes[i].NConnected > 1)
				{
					minYIndex = i;
					maxX = nodes[i].Position.X;
				}
			}

			Vector2 origDir = new Vector2(1.0f, 0.0f);
			Vector2[] resultVecs = new Vector2[4 * nNodes];
			// nodes may be visited more than once, unfortunately - change to growable array!
			int nResultVecs = 0;
			PolyNode currentNode = nodes[minYIndex];
			PolyNode startNode = currentNode;
			Debug.Assert(currentNode.NConnected > 0);
			PolyNode nextNode = currentNode.GetRightestConnection(origDir);
			if (nextNode == null)
			{
				Vertices vertices = new Vertices(nResultVecs);

				for (int i = 0; i < nResultVecs; ++i)
				{
					vertices.Add(resultVecs[i]);
				}

				return vertices;
			}

			// Borked, clean up our mess and return
			resultVecs[0] = startNode.Position;
			++nResultVecs;
			while (nextNode != startNode)
			{
				if (nResultVecs > 4 * nNodes)
				{
					Debug.Assert(false);
				}
				resultVecs[nResultVecs++] = nextNode.Position;
				PolyNode oldNode = currentNode;
				currentNode = nextNode;
				nextNode = currentNode.GetRightestConnection(oldNode);
				if (nextNode == null)
				{
					Vertices vertices = new Vertices(nResultVecs);
					for (int i = 0; i < nResultVecs; ++i)
					{
						vertices.Add(resultVecs[i]);
					}
					return vertices;
				}
				// There was a problem, so jump out of the loop and use whatever garbage we've generated so far
			}

			return new Vertices();
		}

		private class PolyNode
		{
			private const int MaxConnected = 32;

			/*
             * Given sines and cosines, tells if A's angle is less than B's on -Pi, Pi
             * (in other words, is A "righter" than B)
             */
			public PolyNode[] Connected = new PolyNode[MaxConnected];
			public int NConnected;
			public Vector2 Position;

			public PolyNode(Vector2 pos)
			{
				this.Position = pos;
				this.NConnected = 0;
			}

			private bool IsRighter(float sinA, float cosA, float sinB, float cosB)
			{
				if (sinA < 0)
				{
					if (sinB > 0 || cosA <= cosB) return true;
					else return false;
				}
				else
				{
					if (sinB < 0 || cosA <= cosB) return false;
					else return true;
				}
			}

			public void AddConnection(PolyNode toMe)
			{
				Debug.Assert(this.NConnected < MaxConnected);

				// Ignore duplicate additions
				for (int i = 0; i < this.NConnected; ++i)
				{
					if (this.Connected[i] == toMe) return;
				}
				this.Connected[this.NConnected] = toMe;
				++this.NConnected;
			}

			public void RemoveConnection(PolyNode fromMe)
			{
				bool isFound = false;
				int foundIndex = -1;
				for (int i = 0; i < this.NConnected; ++i)
				{
					if (fromMe == this.Connected[i])
					{
						//.position == connected[i].position){
						isFound = true;
						foundIndex = i;
						break;
					}
				}
				Debug.Assert(isFound);
				--this.NConnected;
				for (int i = foundIndex; i < this.NConnected; ++i)
				{
					this.Connected[i] = this.Connected[i + 1];
				}
			}

			public PolyNode GetRightestConnection(PolyNode incoming)
			{
				if (this.NConnected == 0) Debug.Assert(false); // This means the connection graph is inconsistent
				if (this.NConnected == 1)
				{
					//b2Assert(false);
					// Because of the possibility of collapsing nearby points,
					// we may end up with "spider legs" dangling off of a region.
					// The correct behavior here is to turn around.
					return incoming;
				}
				Vector2 inDir = this.Position - incoming.Position;

				float inLength = inDir.Length;
				inDir.Normalize();

				Debug.Assert(inLength > Settings.Epsilon);

				PolyNode result = null;
				for (int i = 0; i < this.NConnected; ++i)
				{
					if (this.Connected[i] == incoming) continue;
					Vector2 testDir = this.Connected[i].Position - this.Position;
					float testLengthSqr = testDir.LengthSquared;
					testDir.Normalize();
					Debug.Assert(testLengthSqr >= Settings.Epsilon * Settings.Epsilon);
					float myCos = Vector2.Dot(inDir, testDir);
					float mySin = MathUtils.Cross(inDir, testDir);
					if (result != null)
					{
						Vector2 resultDir = result.Position - this.Position;
						resultDir.Normalize();
						float resCos = Vector2.Dot(inDir, resultDir);
						float resSin = MathUtils.Cross(inDir, resultDir);
						if (IsRighter(mySin, myCos, resSin, resCos))
						{
							result = this.Connected[i];
						}
					}
					else
					{
						result = this.Connected[i];
					}
				}

				Debug.Assert(result != null);

				return result;
			}

			public PolyNode GetRightestConnection(Vector2 incomingDir)
			{
				Vector2 diff = this.Position - incomingDir;
				PolyNode temp = new PolyNode(diff);
				PolyNode res = GetRightestConnection(temp);
				Debug.Assert(res != null);
				return res;
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < this.Count; i++)
			{
				builder.Append(this[i].ToString());
				if (i < this.Count - 1)
				{
					builder.Append(" ");
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Projects to axis.
		/// </summary>
		/// <param name="axis">The axis.</param>
		/// <param name="min">The min.</param>
		/// <param name="max">The max.</param>
		public void ProjectToAxis(ref Vector2 axis, out float min, out float max)
		{
			// To project a point on an axis use the dot product
			float dotProduct = Vector2.Dot(axis, this[0]);
			min = dotProduct;
			max = dotProduct;

			for (int i = 0; i < this.Count; i++)
			{
				dotProduct = Vector2.Dot(this[i], axis);
				if (dotProduct < min)
				{
					min = dotProduct;
				}
				else
				{
					if (dotProduct > max)
					{
						max = dotProduct;
					}
				}
			}
		}

		/// <summary>
		/// Winding number test for a point in a polygon.
		/// </summary>
		/// See more info about the algorithm here: http://softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm
		/// <param name="point">The point to be tested.</param>
		/// <returns>-1 if the winding number is zero and the point is outside
		/// the polygon, 1 if the point is inside the polygon, and 0 if the point
		/// is on the polygons edge.</returns>
		public int PointInPolygon(ref Vector2 point)
		{
			// Winding number
			int wn = 0;

			// Iterate through polygon's edges
			for (int i = 0; i < this.Count; i++)
			{
				// Get points
				Vector2 p1 = this[i];
				Vector2 p2 = this[NextIndex(i)];

				// Test if a point is directly on the edge
				Vector2 edge = p2 - p1;
				float area = MathUtils.Area(ref p1, ref p2, ref point);
				if (area == 0f && Vector2.Dot(point - p1, edge) >= 0f && Vector2.Dot(point - p2, edge) <= 0f)
				{
					return 0;
				}
				// Test edge for intersection with ray from point
				if (p1.Y <= point.Y)
				{
					if (p2.Y > point.Y && area > 0f)
					{
						++wn;
					}
				}
				else
				{
					if (p2.Y <= point.Y && area < 0f)
					{
						--wn;
					}
				}
			}
			return (wn == 0 ? -1 : 1);
		}

		/// <summary>
		/// Compute the sum of the angles made between the test point and each pair of points making up the polygon. 
		/// If this sum is 2pi then the point is an interior point, if 0 then the point is an exterior point. 
		/// ref: http://ozviz.wasp.uwa.edu.au/~pbourke/geometry/insidepoly/  - Solution 2 
		/// </summary>
		public bool PointInPolygonAngle(ref Vector2 point)
		{
			double angle = 0;

			// Iterate through polygon's edges
			for (int i = 0; i < this.Count; i++)
			{
				// Get points
				Vector2 p1 = this[i] - point;
				Vector2 p2 = this[NextIndex(i)] - point;

				angle += MathUtils.VectorAngle(ref p1, ref p2);
			}

			if (Math.Abs(angle) < Math.PI)
			{
				return false;
			}

			return true;
		}
	}
}