using System;
using System.Collections.Generic;

namespace FarseerPhysics.Common.Decomposition.Seidel
{
	internal class Triangulator
	{
		// Trapezoid decomposition list
		public List<Trapezoid> Trapezoids;
		public List<List<Point>> Triangles;

		// Initialize trapezoidal map and query structure
		private Trapezoid _boundingBox;
		private List<Edge> _edgeList;
		private QueryGraph _queryGraph;
		private float _sheer = 0.001f;
		private TrapezoidalMap _trapezoidalMap;
		private List<MonotoneMountain> _xMonoPoly;

		public Triangulator(List<Point> polyLine, float sheer)
		{
			this._sheer = sheer;
			this.Triangles = new List<List<Point>>();
			this.Trapezoids = new List<Trapezoid>();
			this._xMonoPoly = new List<MonotoneMountain>();
			this._edgeList = InitEdges(polyLine);
			this._trapezoidalMap = new TrapezoidalMap();
			this._boundingBox = this._trapezoidalMap.BoundingBox(this._edgeList);
			this._queryGraph = new QueryGraph(Sink.Isink(this._boundingBox));

			Process();
		}

		// Build the trapezoidal map and query graph
		private void Process()
		{
			foreach (Edge edge in this._edgeList)
			{
				List<Trapezoid> traps = this._queryGraph.FollowEdge(edge);

				// Remove trapezoids from trapezoidal Map
				foreach (Trapezoid t in traps)
				{
					this._trapezoidalMap.Map.Remove(t);

					bool cp = t.Contains(edge.P);
					bool cq = t.Contains(edge.Q);
					Trapezoid[] tList;

					if (cp && cq)
					{
						tList = this._trapezoidalMap.Case1(t, edge);
						this._queryGraph.Case1(t.Sink, edge, tList);
					}
					else if (cp && !cq)
					{
						tList = this._trapezoidalMap.Case2(t, edge);
						this._queryGraph.Case2(t.Sink, edge, tList);
					}
					else if (!cp && !cq)
					{
						tList = this._trapezoidalMap.Case3(t, edge);
						this._queryGraph.Case3(t.Sink, edge, tList);
					}
					else
					{
						tList = this._trapezoidalMap.Case4(t, edge);
						this._queryGraph.Case4(t.Sink, edge, tList);
					}
					// Add new trapezoids to map
					foreach (Trapezoid y in tList)
					{
						this._trapezoidalMap.Map.Add(y);
					}
				}
				this._trapezoidalMap.Clear();
			}

			// Mark outside trapezoids
			foreach (Trapezoid t in this._trapezoidalMap.Map)
			{
				MarkOutside(t);
			}

			// Collect interior trapezoids
			foreach (Trapezoid t in this._trapezoidalMap.Map)
			{
				if (t.Inside)
				{
					this.Trapezoids.Add(t);
					t.AddPoints();
				}
			}

			// Generate the triangles
			CreateMountains();
		}

		// Build a list of x-monotone mountains
		private void CreateMountains()
		{
			foreach (Edge edge in this._edgeList)
			{
				if (edge.MPoints.Count > 2)
				{
					MonotoneMountain mountain = new MonotoneMountain();

					// Sorting is a perfromance hit. Literature says this can be accomplised in
					// linear time, although I don't see a way around using traditional methods
					// when using a randomized incremental algorithm

					// Insertion sort is one of the fastest algorithms for sorting arrays containing 
					// fewer than ten elements, or for lists that are already mostly sorted.

					List<Point> points = new List<Point>(edge.MPoints);
					points.Sort((p1, p2) => p1.X.CompareTo(p2.X));

					foreach (Point p in points)
						mountain.Add(p);

					// Triangulate monotone mountain
					mountain.Process();

					// Extract the triangles into a single list
					foreach (List<Point> t in mountain.Triangles)
					{
						this.Triangles.Add(t);
					}

					this._xMonoPoly.Add(mountain);
				}
			}
		}

		// Mark the outside trapezoids surrounding the polygon
		private void MarkOutside(Trapezoid t)
		{
			if (t.Top == this._boundingBox.Top || t.Bottom == this._boundingBox.Bottom)
				t.TrimNeighbors();
		}

		// Create segments and connect end points; update edge event pointer
		private List<Edge> InitEdges(List<Point> points)
		{
			List<Edge> edges = new List<Edge>();

			for (int i = 0; i < points.Count - 1; i++)
			{
				edges.Add(new Edge(points[i], points[i + 1]));
			}
			edges.Add(new Edge(points[0], points[points.Count - 1]));
			return OrderSegments(edges);
		}

		private List<Edge> OrderSegments(List<Edge> edgeInput)
		{
			// Ignore vertical segments!
			List<Edge> edges = new List<Edge>();

			foreach (Edge e in edgeInput)
			{
				Point p = ShearTransform(e.P);
				Point q = ShearTransform(e.Q);

				// Point p must be to the left of point q
				if (p.X > q.X)
				{
					edges.Add(new Edge(q, p));
				}
				else if (p.X < q.X)
				{
					edges.Add(new Edge(p, q));
				}
			}

			// Randomized triangulation improves performance
			// See Seidel's paper, or O'Rourke's book, p. 57 
			Shuffle(edges);
			return edges;
		}

		private static void Shuffle<T>(IList<T> list)
		{
			Random rng = new Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		// Prevents any two distinct endpoints from lying on a common vertical line, and avoiding
		// the degenerate case. See Mark de Berg et al, Chapter 6.3
		private Point ShearTransform(Point point)
		{
			return new Point(point.X + this._sheer * point.Y, point.Y);
		}
	}
}