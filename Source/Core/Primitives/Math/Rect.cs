using System;

namespace Duality
{
	/// <summary>
	/// Describes a rectangular area.
	/// </summary>
	public struct Rect : IEquatable<Rect>
	{
		/// <summary>
		/// An empty Rect.
		/// </summary>
		public static readonly Rect Empty = new Rect(0, 0, 0, 0);

		/// <summary>
		/// The Rects x-Coordinate.
		/// </summary>
		public	float	X;
		/// <summary>
		/// The Rects y-Coordinate.
		/// </summary>
		public	float	Y;
		/// <summary>
		/// The Rects width.
		/// </summary>
		public	float	W;
		/// <summary>
		/// The Rects height.
		/// </summary>
		public	float	H;

		/// <summary>
		/// [GET / SET] The Rects position
		/// </summary>
		public Vector2 Pos
		{
			get { return new Vector2(this.X, this.Y); }
			set { this.X = value.X; this.Y = value.Y; }
		}
		/// <summary>
		/// [GET / SET] The Rects size.
		/// </summary>
		public Vector2 Size
		{
			get { return new Vector2(W, H); }
			set { this.W = value.X; this.H = value.Y; }
		}

		/// <summary>
		/// [GET] The minimum x-Coordinate occupied by the Rect. Accounts for negative sizes.
		/// </summary>
		public float LeftX
		{
			get { return MathF.Min(X, X + W); }
		}
		/// <summary>
		/// [GET] The minimum y-Coordinate occupied by the Rect. Accounts for negative sizes.
		/// </summary>
		public float TopY
		{
			get { return MathF.Min(Y, Y + H); }
		}
		/// <summary>
		/// [GET] The maximum y-Coordinate occupied by the Rect. Accounts for negative sizes.
		/// </summary>
		public float BottomY
		{
			get { return MathF.Max(Y, Y + H); }
		}
		/// <summary>
		/// [GET] The maximum x-Coordinate occupied by the Rect. Accounts for negative sizes.
		/// </summary>
		public float RightX
		{
			get { return MathF.Max(X, X + W); }
		}
		/// <summary>
		/// [GET] The center x-Coordinate occupied by the Rect.
		/// </summary>
		public float CenterX
		{
			get { return X + W * 0.5f; }
		}
		/// <summary>
		/// [GET] The center y-Coordinate occupied by the Rect.
		/// </summary>
		public float CenterY
		{
			get { return Y + H * 0.5f; }
		}

		/// <summary>
		/// [GET] The Rects top left coordinates
		/// </summary>
		public Vector2 TopLeft
		{
			get { return new Vector2(this.LeftX, this.TopY); }
		}
		/// <summary>
		/// [GET] The Rects top right coordinates
		/// </summary>
		public Vector2 TopRight
		{
			get { return new Vector2(this.RightX, this.TopY); }
		}
		/// <summary>
		/// [GET] The Rects bottom left coordinates
		/// </summary>
		public Vector2 BottomLeft
		{
			get { return new Vector2(this.LeftX, this.BottomY); }
		}
		/// <summary>
		/// [GET] The Rects bottom right coordinates
		/// </summary>
		public Vector2 BottomRight
		{
			get { return new Vector2(this.RightX, this.BottomY); }
		}
		/// <summary>
		/// [GET] The Rects center coordinates
		/// </summary>
		public Vector2 Center
		{
			get { return new Vector2(this.CenterX, this.CenterY); }
		}

		/// <summary>
		/// [GET] If this Rect was to fit inside a bounding circle originating from [0,0],
		/// this would be its radius.
		/// </summary>
		public float BoundingRadius
		{
			get 
			{ 
				return MathF.Max(
					MathF.Distance(this.RightX, this.BottomY),
					MathF.Distance(this.LeftX, this.TopY),
					MathF.Distance(this.RightX, this.TopY),
					MathF.Distance(this.LeftX, this.BottomY)); 
			}
		}
		
		/// <summary>
		/// Creates a Rect of the given size.
		/// </summary>
		/// <param name="size"></param>
		public Rect(Vector2 size)
		{
			this.X = 0;
			this.Y = 0;
			this.W = size.X;
			this.H = size.Y;
		}
		/// <summary>
		/// Creates a Rect of the given size.
		/// </summary>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public Rect(float w, float h)
		{
			this.X = 0;
			this.Y = 0;
			this.W = w;
			this.H = h;
		}
		/// <summary>
		/// Creates a Rect of the given size and position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public Rect(float x, float y, float w, float h)
		{
			this.X = x;
			this.Y = y;
			this.W = w;
			this.H = h;
		}

		/// <summary>
		/// Returns a new version of this Rect that has been moved by the specified offset.
		/// </summary>
		/// <param name="x">Movement in x-Direction.</param>
		/// <param name="y">Movement in y-Direction.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect WithOffset(float x, float y)
		{
			Rect newRect = this;
			newRect.X += x;
			newRect.Y += y;
			return newRect;
		}
		/// <summary>
		/// Returns a new version of this Rect that has been moved by the specified offset.
		/// </summary>
		/// <param name="offset">Movement vector.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect WithOffset(Vector2 offset)
		{
			Rect newRect = this;
			newRect.X += offset.X;
			newRect.Y += offset.Y;
			return newRect;
		}

		/// <summary>
		/// Returns a new version of this Rect that has been scaled by the specified factor.
		/// Scaling only affects a Rects size, not its position.
		/// </summary>
		/// <param name="x">x-Scale factor.</param>
		/// <param name="y">y-Scale factor.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect Scaled(float x, float y)
		{
			Rect newRect = this;
			newRect.W *= x;
			newRect.H *= y;
			return newRect;
		}
		/// <summary>
		/// Returns a new version of this Rect that has been scaled by the specified factor.
		/// Scaling only affects a Rects size, not its position.
		/// </summary>
		/// <param name="factor">Scale factor.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect Scaled(Vector2 factor)
		{
			Rect newRect = this;
			newRect.W *= factor.X;
			newRect.H *= factor.Y;
			return newRect;
		}
		/// <summary>
		/// Returns a new version of this Rect that has been transformed by the specified scale factor.
		/// Transforming both affects a Rects size and position.
		/// </summary>
		/// <param name="x">x-Scale factor.</param>
		/// <param name="y">y-Scale factor.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect Transformed(float x, float y)
		{
			Rect newRect = this;
			newRect.X *= x;
			newRect.Y *= y;
			newRect.W *= x;
			newRect.H *= y;
			return newRect;
		}
		/// <summary>
		/// Returns a new version of this Rect that has been transformed by the specified scale factor.
		/// Transforming both affects a Rects size and position.
		/// </summary>
		/// <param name="scale">Scale factor.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect Transformed(Vector2 scale)
		{
			Rect newRect = this;
			newRect.X *= scale.X;
			newRect.Y *= scale.Y;
			newRect.W *= scale.X;
			newRect.H *= scale.Y;
			return newRect;
		}

		/// <summary>
		/// Returns a new version of this Rect that has been expanded to contain
		/// the specified rectangular area.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect to contain.</param>
		/// <param name="y">y-Coordinate of the Rect to contain.</param>
		/// <param name="w">Width of the Rect to contain.</param>
		/// <param name="h">Height of the Rect to contain.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect ExpandedToContain(float x, float y, float w, float h)
		{
			return this.ExpandedToContain(x, y).ExpandedToContain(x + w, y + h);
		}
		/// <summary>
		/// Returns a new version of this Rect that has been expanded to contain
		/// the specified Rect.
		/// </summary>
		/// <param name="other">The Rect to contain.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect ExpandedToContain(Rect other)
		{
			return this.ExpandedToContain(other.X, other.Y).ExpandedToContain(other.X + other.W, other.Y + other.H);
		}
		/// <summary>
		/// Returns a new version of this Rect that has been expanded to contain
		/// the specified point.
		/// </summary>
		/// <param name="x">x-Coordinate of the point to contain.</param>
		/// <param name="y">y-Coordinate of the point to contain.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect ExpandedToContain(float x, float y)
		{
			Rect newRect = this;
			if (x < newRect.X)
			{
				newRect.W += newRect.X - x;
				newRect.X = x;
			}
			if (y < newRect.Y)
			{
				newRect.H += newRect.Y - y;
				newRect.Y = y;
			}
			if (x > newRect.X + newRect.W) newRect.W = x - newRect.X;
			if (y > newRect.Y + newRect.H) newRect.H = y - newRect.Y;
			return newRect;
		}
		/// <summary>
		/// Returns a new version of this Rect that has been expanded to contain
		/// the specified point.
		/// </summary>
		/// <param name="p">The point to contain.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect ExpandedToContain(Vector2 p)
		{
			return this.ExpandedToContain(p.X, p.Y);
		}

		/// <summary>
		/// Returns a normalized version of the rect, i.e. one with a positive width and height.
		/// </summary>
		/// <returns></returns>
		public Rect Normalized()
		{
			Rect normalized = this;
			if (normalized.W < 0)
			{
				normalized.X += normalized.W;
				normalized.W = -normalized.W;
			}
			if (normalized.H < 0)
			{
				normalized.Y += normalized.H;
				normalized.H = -normalized.H;
			}
			return normalized;
		}

		/// <summary>
		/// Returns whether this Rect contains a given point.
		/// </summary>
		/// <param name="x">x-Coordinate of the point to test.</param>
		/// <param name="y">y-Coordinate of the point to test.</param>
		/// <returns>True, if the Rect contains the point, false if not.</returns>
		public bool Contains(float x, float y)
		{
			return x >= this.LeftX && x <= this.RightX && y >= this.TopY && y <= this.BottomY;
		}
		/// <summary>
		/// Returns whether this Rect contains a given point.
		/// </summary>
		/// <param name="pos">The point to test.</param>
		/// <returns>True, if the Rect contains the point, false if not.</returns>
		public bool Contains(Vector2 pos)
		{
			return pos.X >= this.LeftX && pos.X <= this.RightX && pos.Y >= this.TopY && pos.Y <= this.BottomY;
		}
		/// <summary>
		/// Returns whether this Rect contains a given rectangular area.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect to test.</param>
		/// <param name="y">y-Coordinate of the Rect to test.</param>
		/// <param name="w">Width of the Rect to test.</param>
		/// <param name="h">Height of the Rect to test.</param>
		/// <returns>True, if the Rect contains the other Rect, false if not.</returns>
		public bool Contains(float x, float y, float w, float h)
		{
			return this.Contains(x, y) && this.Contains(x + w, y + h);
		}
		/// <summary>
		/// Returns whether this Rect contains a given rectangular area.
		/// </summary>
		/// <param name="rect">The Rect to test.</param>
		/// <returns>True, if the Rect contains the other Rect, false if not.</returns>
		public bool Contains(Rect rect)
		{
			return this.Contains(rect.X, rect.Y) && this.Contains(rect.X + rect.W, rect.Y + rect.H);
		}
		
		/// <summary>
		/// Returns whether this Rect intersects a given rectangular area.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect to test.</param>
		/// <param name="y">y-Coordinate of the Rect to test.</param>
		/// <param name="w">Width of the Rect to test.</param>
		/// <param name="h">Height of the Rect to test.</param>
		/// <returns>True, if the Rect intersects the other Rect, false if not.</returns>
		public bool Intersects(float x, float y, float w, float h)
		{
			return this.Intersects(new Rect(x, y, w, h));
		}
		/// <summary>
		/// Returns whether this Rect intersects a given rectangular area.
		/// </summary>
		/// <param name="rect">The Rect to test.</param>
		/// <returns>True, if the Rect intersects the other Rect, false if not.</returns>
		public bool Intersects(Rect rect)
		{
			rect = rect.Normalized();
			Rect norm = this.Normalized();
			if (norm.X > (rect.X + rect.W) || (norm.X + norm.W) < rect.X) return false;
			if (norm.Y > (rect.Y + rect.H) || (norm.Y + norm.H) < rect.Y) return false;
			return true;
		}
		/// <summary>
		/// Returns a Rect that equals this Rects intersection with another Rect.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect to intersect with.</param>
		/// <param name="y">y-Coordinate of the Rect to intersect with.</param>
		/// <param name="w">Width of the Rect to intersect with.</param>
		/// <param name="h">Height of the Rect to intersect with.</param>
		/// <returns>A new Rect that describes both Rects intersection area. <see cref="Empty"/>, if there is no intersection.</returns>
		public Rect Intersection(float x, float y, float w, float h)
		{
			return this.Intersection(new Rect(x, y, w, h));
		}
		/// <summary>
		/// Returns a Rect that equals this Rects intersection with another Rect.
		/// </summary>
		/// <param name="rect">The other Rect to intersect with.</param>
		/// <returns>A new Rect that describes both Rects intersection area. <see cref="Empty"/>, if there is no intersection.</returns>
		public Rect Intersection(Rect rect)
		{
			rect = rect.Normalized();
			Rect norm = this.Normalized();

			float tempWidth = Math.Min(rect.W, norm.W - (rect.X - norm.X));
			float tempHeight = Math.Min(rect.H, norm.H - (rect.Y - norm.Y));
			if ((norm.X - rect.X) > 0.0f) tempWidth -= (norm.X - rect.X);
			if ((norm.Y - rect.Y) > 0.0f) tempHeight -= (norm.Y - rect.Y);

			Rect result = new Rect(
				Math.Max(norm.X, rect.X),
				Math.Max(norm.Y, rect.Y),
				Math.Min(norm.W, tempWidth),
				Math.Min(norm.H, tempHeight));

			return (result.W == 0 || result.H == 0) ? Rect.Empty : result;
		}

		/// <summary>
		/// Tests if two Rects are equal.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(Rect other)
		{
			return 
				this.X == other.X &&
				this.Y == other.Y &&
				this.W == other.W &&
				this.H == other.H;
		}
		public override bool Equals(object obj)
		{
			if (!(obj is Rect))
				return false;
			else
				return this.Equals((Rect)obj);
		}
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.W.GetHashCode() ^ this.H.GetHashCode();
		}
		public override string ToString()
		{
			return string.Format("Rect ({0}, {1}, {2}, {3})", this.X, this.Y, this.W, this.H);
		}

		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned as specified.
		/// </summary>
		/// <param name="align">The alignment of the Rects x and y Coordinates.</param>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect Align(Alignment align, float x, float y, float w, float h)
		{
			switch (align)
			{
				default:
				case Alignment.TopLeft:		return new Rect(x, y, w, h);
				case Alignment.TopRight:	return new Rect(x - w, y, w, h);
				case Alignment.BottomLeft:	return new Rect(x, y - h, w, h);
				case Alignment.BottomRight:	return new Rect(x - w, y - h, w, h);
				case Alignment.Center:		return new Rect(x - w * 0.5f, y - h * 0.5f, w, h);
				case Alignment.Bottom:		return new Rect(x - w * 0.5f, y - h, w, h);
				case Alignment.Left:		return new Rect(x, y - h * 0.5f, w, h);
				case Alignment.Right:		return new Rect(x - w, y - h * 0.5f, w, h);
				case Alignment.Top:			return new Rect(x - w * 0.5f, y, w, h);
			}
		}

		/// <summary>
		/// Returns whether two Rects are equal.
		/// </summary>
		/// <param name="left">The first Rect.</param>
		/// <param name="right">The second Rect.</param>
		/// <returns></returns>
		public static bool operator ==(Rect left, Rect right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Returns whether two Rects are unequal.
		/// </summary>
		/// <param name="left">The first Rect.</param>
		/// <param name="right">The second Rect.</param>
		/// <returns></returns>
		public static bool operator !=(Rect left, Rect right)
		{
			return !left.Equals(right);
		}
	}
}
