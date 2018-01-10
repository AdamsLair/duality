using System;
using System.Runtime.InteropServices;

namespace Duality
{
	/// <summary>
	/// Represents a 2D point using two integer values. For vector math, see <see cref="Vector2"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Point2 : IEquatable<Point2>
	{
		/// <summary>
		/// A point at the origin (0, 0).
		/// </summary>
		public static readonly Point2 Zero = new Point2(0, 0);

		/// <summary>
		/// The X component of the Point.
		/// </summary>
		public int X;
		/// <summary>
		/// The Y component of the Point.
		/// </summary>
		public int Y;
		
		/// <summary>
		/// Gets or sets the value at the index of the Point.
		/// </summary>
		public int this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return this.X;
					case 1: return this.Y;
					default: throw new IndexOutOfRangeException("Point2 access at index: " + index);
				}
			}
			set
			{
				switch (index)
				{
					case 0: this.X = value; return;
					case 1: this.Y = value; return;
					default: throw new IndexOutOfRangeException("Point2 access at index: " + index);
				}
			}
		}

		/// <summary>
		/// Constructs a new Point.
		/// </summary>
		/// <param name="x">The x coordinate of the Point.</param>
		/// <param name="y">The y coordinate of the Point.</param>
		public Point2(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Calculate the component-wise minimum of two points.
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise minimum</returns>
		public static Point2 Min(Point2 a, Point2 b)
		{
			a.X = a.X < b.X ? a.X : b.X;
			a.Y = a.Y < b.Y ? a.Y : b.Y;
			return a;
		}
		/// <summary>
		/// Calculate the component-wise maximum of two points.
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise maximum</returns>
		public static Point2 Max(Point2 a, Point2 b)
		{
			a.X = a.X > b.X ? a.X : b.X;
			a.Y = a.Y > b.Y ? a.Y : b.Y;
			return a;
		}
		/// <summary>
		/// Calculates the distance between two points described by two points. 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static float Distance(Point2 left, Point2 right)
		{
			Point2 diff;
			diff.X = left.X - right.X;
			diff.Y = left.Y - right.Y;
			return MathF.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
		}
		
		/// <summary>
		/// Adds the specified points component-wise.
		/// </summary>
		public static Point2 operator +(Point2 left, Point2 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}
		/// <summary>
		/// Subtracts the specified points component-wise.
		/// </summary>
		public static Point2 operator -(Point2 left, Point2 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}
		/// <summary>
		/// Inverts the specified point component-wise,
		/// </summary>
		public static Point2 operator -(Point2 point)
		{
			point.X = -point.X;
			point.Y = -point.Y;
			return point;
		}
		/// <summary>
		/// Multiplies the specified point component-wise with the specified factor.
		/// </summary>
		public static Point2 operator *(Point2 left, int right)
		{
			left.X *= right;
			left.Y *= right;
			return left;
		}
		/// <summary>
		/// Multiplies the specified point component-wise with the specified factor.
		/// </summary>
		public static Point2 operator *(int left, Point2 right)
		{
			right.X *= left;
			right.Y *= left;
			return right;
		}
		/// <summary>
		/// Multiplies the specified points component-wise.
		/// </summary>
		public static Point2 operator *(Point2 left, Point2 right)
		{
			left.X *= right.X;
			left.Y *= right.Y;
			return left;
		}
		/// <summary>
		/// Divides the specified point component-wise with the specified value.
		/// </summary>
		public static Point2 operator /(Point2 left, int right)
		{
			left.X /= right;
			left.Y /= right;
			return left;
		}
		/// <summary>
		/// Divides the specified points component-wise.
		/// </summary>
		public static Point2 operator /(Point2 left, Point2 right)
		{
			left.X /= right.X;
			left.Y /= right.Y;
			return left;
		}

		/// <summary>
		/// Multiplies the specified point component-wise with the specified factor.
		/// </summary>
		public static Vector2 operator *(Point2 left, float right)
		{
			Vector2 result = left;
			result.X *= right;
			result.Y *= right;
			return result;
		}
		/// <summary>
		/// Multiplies the specified point component-wise with the specified factor.
		/// </summary>
		public static Vector2 operator *(float left, Point2 right)
		{
			Vector2 result = right;
			result.X *= left;
			result.Y *= left;
			return result;
		}
		/// <summary>
		/// Divides the specified point component-wise with the specified value.
		/// </summary>
		public static Vector2 operator /(Point2 left, float right)
		{
			float mult = 1.0f / right;
			Vector2 result = left;
			result.X *= mult;
			result.Y *= mult;
			return result;
		}

		/// <summary>
		/// Compares the specified instances for equality.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True if both instances are equal; false otherwise.</returns>
		public static bool operator ==(Point2 left, Point2 right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Compares the specified instances for inequality.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True if both instances are not equal; false otherwise.</returns>
		public static bool operator !=(Point2 left, Point2 right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a System.String that represents the current Point.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("({0}, {1})", this.X, this.Y);
		}
		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}
		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Point2))
				return false;

			return this.Equals((Point2)obj);
		}

		/// <summary>
		/// Indicates whether the current point is equal to another point.
		/// </summary>
		/// <param name="other">A point to compare with this point.</param>
		/// <returns>true if the current point is equal to the point parameter; otherwise, false.</returns>
		public bool Equals(Point2 other)
		{
			return
				this.X == other.X &&
				this.Y == other.Y;
		}

		public static implicit operator Vector2(Point2 r)
		{
			return new Vector2(r.X, r.Y);
		}
		public static explicit operator Point2(Vector2 r)
		{
			return new Point2(MathF.RoundToInt(r.X), MathF.RoundToInt(r.Y));
		}
	}
}
