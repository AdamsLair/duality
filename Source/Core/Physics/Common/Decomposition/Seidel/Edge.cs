using System.Collections.Generic;

namespace FarseerPhysics.Common.Decomposition.Seidel
{
	internal class Edge
	{
		// Pointers used for building trapezoidal map
		public Trapezoid Above;
		public float B;
		public Trapezoid Below;

		// Montone mountain points
		public HashSet<Point> MPoints;
		public Point P;
		public Point Q;

		// Slope of the line (m)
		public float Slope;


		public Edge(Point p, Point q)
		{
			this.P = p;
			this.Q = q;

			if (q.X - p.X != 0)
				this.Slope = (q.Y - p.Y) / (q.X - p.X);
			else
				this.Slope = 0;

			this.B = p.Y - (p.X * this.Slope);
			this.Above = null;
			this.Below = null;
			this.MPoints = new HashSet<Point>();
			this.MPoints.Add(p);
			this.MPoints.Add(q);
		}

		public bool IsAbove(Point point)
		{
			return this.P.Orient2D(this.Q, point) < 0;
		}

		public bool IsBelow(Point point)
		{
			return this.P.Orient2D(this.Q, point) > 0;
		}

		public void AddMpoint(Point point)
		{
			foreach (Point mp in this.MPoints)
			{
				if (!mp.Neq(point))
					return;
			}

			this.MPoints.Add(point);
		}
	}
}