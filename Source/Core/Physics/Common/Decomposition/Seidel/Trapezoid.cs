using System.Collections.Generic;

namespace FarseerPhysics.Common.Decomposition.Seidel
{
	internal class Trapezoid
	{
		public Edge Bottom;
		public bool Inside;
		public Point LeftPoint;

		// Neighbor pointers
		public Trapezoid LowerLeft;
		public Trapezoid LowerRight;

		public Point RightPoint;
		public Sink Sink;

		public Edge Top;
		public Trapezoid UpperLeft;
		public Trapezoid UpperRight;

		public Trapezoid(Point leftPoint, Point rightPoint, Edge top, Edge bottom)
		{
			this.LeftPoint = leftPoint;
			this.RightPoint = rightPoint;
			this.Top = top;
			this.Bottom = bottom;
			this.UpperLeft = null;
			this.UpperRight = null;
			this.LowerLeft = null;
			this.LowerRight = null;
			this.Inside = true;
			this.Sink = null;
		}

		// Update neighbors to the left
		public void UpdateLeft(Trapezoid ul, Trapezoid ll)
		{
			this.UpperLeft = ul;
			if (ul != null) ul.UpperRight = this;
			this.LowerLeft = ll;
			if (ll != null) ll.LowerRight = this;
		}

		// Update neighbors to the right
		public void UpdateRight(Trapezoid ur, Trapezoid lr)
		{
			this.UpperRight = ur;
			if (ur != null) ur.UpperLeft = this;
			this.LowerRight = lr;
			if (lr != null) lr.LowerLeft = this;
		}

		// Update neighbors on both sides
		public void UpdateLeftRight(Trapezoid ul, Trapezoid ll, Trapezoid ur, Trapezoid lr)
		{
			this.UpperLeft = ul;
			if (ul != null) ul.UpperRight = this;
			this.LowerLeft = ll;
			if (ll != null) ll.LowerRight = this;
			this.UpperRight = ur;
			if (ur != null) ur.UpperLeft = this;
			this.LowerRight = lr;
			if (lr != null) lr.LowerLeft = this;
		}

		// Recursively trim outside neighbors
		public void TrimNeighbors()
		{
			if (this.Inside)
			{
				this.Inside = false;
				if (this.UpperLeft != null) this.UpperLeft.TrimNeighbors();
				if (this.LowerLeft != null) this.LowerLeft.TrimNeighbors();
				if (this.UpperRight != null) this.UpperRight.TrimNeighbors();
				if (this.LowerRight != null) this.LowerRight.TrimNeighbors();
			}
		}

		// Determines if this point lies inside the trapezoid
		public bool Contains(Point point)
		{
			return (point.X > this.LeftPoint.X && point.X < this.RightPoint.X && this.Top.IsAbove(point) && this.Bottom.IsBelow(point));
		}

		public List<Point> GetVertices()
		{
			List<Point> verts = new List<Point>(4);
			verts.Add(LineIntersect(this.Top, this.LeftPoint.X));
			verts.Add(LineIntersect(this.Bottom, this.LeftPoint.X));
			verts.Add(LineIntersect(this.Bottom, this.RightPoint.X));
			verts.Add(LineIntersect(this.Top, this.RightPoint.X));
			return verts;
		}

		private Point LineIntersect(Edge edge, float x)
		{
			float y = edge.Slope * x + edge.B;
			return new Point(x, y);
		}

		// Add points to monotone mountain
		public void AddPoints()
		{
			if (this.LeftPoint != this.Bottom.P)
			{
				this.Bottom.AddMpoint(this.LeftPoint);
			}
			if (this.RightPoint != this.Bottom.Q)
			{
				this.Bottom.AddMpoint(this.RightPoint);
			}
			if (this.LeftPoint != this.Top.P)
			{
				this.Top.AddMpoint(this.LeftPoint);
			}
			if (this.RightPoint != this.Top.Q)
			{
				this.Top.AddMpoint(this.RightPoint);
			}
		}
	}
}