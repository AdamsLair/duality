using System;
using OpenTK;

namespace Duality
{
	/// <summary>
	/// Describes a rectangular area.
	/// </summary>
	[Serializable]
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
		public float MinimumX
		{
			get { return MathF.Min(X, X + W); }
		}
		/// <summary>
		/// [GET] The minimum y-Coordinate occupied by the Rect. Accounts for negative sizes.
		/// </summary>
		public float MinimumY
		{
			get { return MathF.Min(Y, Y + H); }
		}
		/// <summary>
		/// [GET] The maximum y-Coordinate occupied by the Rect. Accounts for negative sizes.
		/// </summary>
		public float MaximumY
		{
			get { return MathF.Max(Y, Y + H); }
		}
		/// <summary>
		/// [GET] The maximum x-Coordinate occupied by the Rect. Accounts for negative sizes.
		/// </summary>
		public float MaximumX
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
			get { return new Vector2(this.MinimumX, this.MinimumY); }
		}
		/// <summary>
		/// [GET] The Rects top right coordinates
		/// </summary>
		public Vector2 TopRight
		{
			get { return new Vector2(this.MaximumX, this.MinimumY); }
		}
		/// <summary>
		/// [GET] The Rects top coordinates
		/// </summary>
		public Vector2 Top
		{
			get { return new Vector2(this.CenterX, this.MinimumY); }
		}
		/// <summary>
		/// [GET] The Rects bottom left coordinates
		/// </summary>
		public Vector2 BottomLeft
		{
			get { return new Vector2(this.MinimumX, this.MaximumY); }
		}
		/// <summary>
		/// [GET] The Rects bottom right coordinates
		/// </summary>
		public Vector2 BottomRight
		{
			get { return new Vector2(this.MaximumX, this.MaximumY); }
		}
		/// <summary>
		/// [GET] The Rects bottom coordinates
		/// </summary>
		public Vector2 Bottom
		{
			get { return new Vector2(this.CenterX, this.MaximumY); }
		}
		/// <summary>
		/// [GET] The Rects left coordinates
		/// </summary>
		public Vector2 Left
		{
			get { return new Vector2(this.MinimumX, this.CenterY); }
		}
		/// <summary>
		/// [GET] The Rects right coordinates
		/// </summary>
		public Vector2 Right
		{
			get { return new Vector2(this.MaximumX, this.CenterY); }
		}
		/// <summary>
		/// [GET] The Rects center coordinates
		/// </summary>
		public Vector2 Center
		{
			get { return new Vector2(this.CenterX, this.CenterY); }
		}

		/// <summary>
		/// [GET] The area that is occupied by the Rect.
		/// </summary>
		public float Area
		{
			get { return MathF.Abs(W * H); }
		}
		/// <summary>
		/// [GET] The Rects perimeter i.e. sum of all edge lengths.
		/// </summary>
		public float Perimeter
		{
			get { return 2 * MathF.Abs(W) + 2 * MathF.Abs(H); }
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
					MathF.Distance(this.MaximumX, this.MaximumY),
					MathF.Distance(this.MinimumX, this.MinimumY),
					MathF.Distance(this.MaximumX, this.MinimumY),
					MathF.Distance(this.MinimumX, this.MaximumY)); 
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
		public Rect Offset(float x, float y)
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
		public Rect Offset(Vector2 offset)
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
		public Rect Scale(float x, float y)
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
		public Rect Scale(Vector2 factor)
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
		public Rect Transform(float x, float y)
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
		public Rect Transform(Vector2 scale)
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
		public Rect ExpandToContain(float x, float y, float w, float h)
		{
			return this.ExpandToContain(x, y).ExpandToContain(x + w - 1, y + h - 1);
		}
		/// <summary>
		/// Returns a new version of this Rect that has been expanded to contain
		/// the specified Rect.
		/// </summary>
		/// <param name="other">The Rect to contain.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect ExpandToContain(Rect other)
		{
			return this.ExpandToContain(other.X, other.Y).ExpandToContain(other.X + other.W - 1, other.Y + other.H - 1);
		}
		/// <summary>
		/// Returns a new version of this Rect that has been expanded to contain
		/// the specified point.
		/// </summary>
		/// <param name="x">x-Coordinate of the point to contain.</param>
		/// <param name="y">y-Coordinate of the point to contain.</param>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect ExpandToContain(float x, float y)
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
		public Rect ExpandToContain(Vector2 p)
		{
			return this.ExpandToContain(p.X, p.Y);
		}

		/// <summary>
		/// Returns a new version of this Rect with integer coordinates and size.
		/// They have been <see cref="MathF.Round(float)">rounded</see>.
		/// </summary>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect Round()
		{
			return new Rect(MathF.Round(X), MathF.Round(Y), MathF.Round(W), MathF.Round(H));
		}
		/// <summary>
		/// Returns a new version of this Rect with integer coordinates and size.
		/// They have been <see cref="MathF.Ceiling">ceiled</see>.
		/// </summary>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect Ceiling()
		{
			return new Rect(MathF.Ceiling(X), MathF.Ceiling(Y), MathF.Ceiling(W), MathF.Ceiling(H));
		}
		/// <summary>
		/// Returns a new version of this Rect with integer coordinates and size.
		/// They have been <see cref="MathF.Floor">floored</see>.
		/// </summary>
		/// <returns>A new Rect with the specified adjustments.</returns>
		public Rect Floor()
		{
			return new Rect(MathF.Floor(X), MathF.Floor(Y), MathF.Floor(W), MathF.Floor(H));
		}

		/// <summary>
		/// Returns whether this Rect contains a given point.
		/// </summary>
		/// <param name="x">x-Coordinate of the point to test.</param>
		/// <param name="y">y-Coordinate of the point to test.</param>
		/// <returns>True, if the Rect contains the point, false if not.</returns>
		public bool Contains(float x, float y)
		{
			return x >= this.MinimumX && x <= this.MaximumX && y >= this.MinimumY && y <= this.MaximumY;
		}
		/// <summary>
		/// Returns whether this Rect contains a given point.
		/// </summary>
		/// <param name="pos">The point to test.</param>
		/// <returns>True, if the Rect contains the point, false if not.</returns>
		public bool Contains(Vector2 pos)
		{
			return pos.X >= this.MinimumX && pos.X <= this.MaximumX && pos.Y >= this.MinimumY && pos.Y <= this.MaximumY;
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
			if (this.X > (x + w) || (this.X + this.W) < x) return false;
			if (this.Y > (y + h) || (this.Y + this.H) < y) return false;
			return true;
		}
		/// <summary>
		/// Returns whether this Rect intersects a given rectangular area.
		/// </summary>
		/// <param name="rect">The Rect to test.</param>
		/// <returns>True, if the Rect intersects the other Rect, false if not.</returns>
		public bool Intersects(Rect rect)
		{
			if (this.X > (rect.X + rect.W) || (this.X + this.W) < rect.X) return false;
			if (this.Y > (rect.Y + rect.H) || (this.Y + this.H) < rect.Y) return false;
			return true;
		}
		/// <summary>
		/// Returns a Rect that equals this Rects intersection with another Rect.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect to intersect with.</param>
		/// <param name="y">y-Coordinate of the Rect to intersect with.</param>
		/// <param name="w">Width of the Rect to intersect with.</param>
		/// <param name="h">Height of the Rect to intersect with.</param>
		/// <returns>A new Rect that describes both Rects intersection area.</returns>
		public Rect Intersection(float x, float y, float w, float h)
		{
			return this.Intersection(new Rect(x, y, w, h));
		}
		/// <summary>
		/// Returns a Rect that equals this Rects intersection with another Rect.
		/// </summary>
		/// <param name="rect">The other Rect to intersect with.</param>
		/// <returns>A new Rect that describes both Rects intersection area.</returns>
		public Rect Intersection(Rect rect)
		{
			float tempWidth = Math.Min(rect.W, this.W - (rect.X - this.X));
			float tempHeight = Math.Min(rect.H, this.H - (rect.Y - this.Y));
			if ((this.X - rect.X) > 0.0f) tempWidth -= (this.X - rect.X);
			if ((this.Y - rect.Y) > 0.0f) tempHeight -= (this.Y - rect.Y);

			return new Rect(
				Math.Max(this.X, rect.X),
				Math.Max(this.Y, rect.Y),
				Math.Min(this.W, tempWidth),
				Math.Min(this.H, tempHeight));
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
				case Alignment.TopLeft:		return AlignTopLeft		(x, y, w, h);
				case Alignment.TopRight:	return AlignTopRight	(x, y, w, h);
				case Alignment.BottomLeft:	return AlignBottomLeft	(x, y, w, h);
				case Alignment.BottomRight:	return AlignBottomRight	(x, y, w, h);
				case Alignment.Center:		return AlignCenter		(x, y, w, h);
				case Alignment.Bottom:		return AlignBottom		(x, y, w, h);
				case Alignment.Left:		return AlignLeft		(x, y, w, h);
				case Alignment.Right:		return AlignRight		(x, y, w, h);
				case Alignment.Top:			return AlignTop			(x, y, w, h);
			}
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned centered.
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignCenter(float x, float y, float w, float h)
		{
			return new Rect(x - w * 0.5f, y - h * 0.5f, w, h);
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned at the top.
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignTop(float x, float y, float w, float h)
		{
			return new Rect(x - w * 0.5f, y, w, h);
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned at the bottom.
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignBottom(float x, float y, float w, float h)
		{
			return new Rect(x - w * 0.5f, y - h, w, h);
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned left
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignLeft(float x, float y, float w, float h)
		{
			return new Rect(x, y - h * 0.5f, w, h);
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned right
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignRight(float x, float y, float w, float h)
		{
			return new Rect(x - w, y - h * 0.5f, w, h);
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned top left.
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignTopLeft(float x, float y, float w, float h)
		{
			return new Rect(x, y, w, h);
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned top right.
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignTopRight(float x, float y, float w, float h)
		{
			return new Rect(x - w, y, w, h);
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned bottom left.
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignBottomLeft(float x, float y, float w, float h)
		{
			return new Rect(x, y - h, w, h);
		}
		/// <summary>
		/// Creates a Rect using x and y Coordinates that are assumed to be aligned bottom right.
		/// </summary>
		/// <param name="x">The Rects x-Coordinate.</param>
		/// <param name="y">The Rects y-Coordinate.</param>
		/// <param name="w">The Rects width.</param>
		/// <param name="h">The Rects height.</param>
		/// <returns></returns>
		public static Rect AlignBottomRight(float x, float y, float w, float h)
		{
			return new Rect(x - w, y - h, w, h);
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

		public static implicit operator System.Drawing.Rectangle(Rect r)
		{
			return new System.Drawing.Rectangle((int)r.X, (int)r.Y, (int)r.W, (int)r.H);
		}
		public static implicit operator Rect(System.Drawing.Rectangle r)
		{
			return new Rect(r.X, r.Y, r.Width, r.Height);
		}
	}
}
