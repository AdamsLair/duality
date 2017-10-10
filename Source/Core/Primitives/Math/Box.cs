namespace Duality
{
	public class Box
	{
		public readonly Vector2 P1;
		public readonly Vector2 P2;
		public readonly Vector2 P3;
		public readonly Vector2 P4;
		public readonly Vector2 Size;

		public Box(float x, float y, float width, float height)
		{
			this.P1 = new Vector2(x, y);
			this.P2 = new Vector2(x + width, y);
			this.P3 = new Vector2(x + width, y + height);
			this.P4 = new Vector2(x, y + height);
			this.Size = new Vector2(width, height);
		}

		public bool Contains(Vector2 point)
		{
			return point.X >= this.P1.X && point.Y >= this.P1.Y && point.X <= this.P3.X && point.Y <= this.P3.Y;
		}

		public bool LineIntersects(Vector2 lineStart, Vector2 lineEnd)
		{
			return LineIntersectsLine(lineStart, lineEnd, this.P1, this.P2) ||
				   LineIntersectsLine(lineStart, lineEnd, this.P2, this.P3) ||
				   LineIntersectsLine(lineStart, lineEnd, this.P3, this.P4) ||
				   LineIntersectsLine(lineStart, lineEnd, this.P4, this.P1) ||
				   (Contains(lineStart) && Contains(lineEnd));
		}

		private static bool LineIntersectsLine(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2)
		{
			float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
			float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

			if (d == 0)
			{
				return false;
			}

			float r = q / d;

			q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
			float s = q / d;

			if (r < 0 || r > 1 || s < 0 || s > 1)
			{
				return false;
			}

			return true;
		}
	}
}
