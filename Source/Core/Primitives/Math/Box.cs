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
			return Contains(lineStart) || 
				   Contains(lineEnd) ||
				   LineIntersectsLine(lineStart, lineEnd, this.P1, this.P2) ||
				   LineIntersectsLine(lineStart, lineEnd, this.P2, this.P3) ||
				   LineIntersectsLine(lineStart, lineEnd, this.P3, this.P4) ||
				   LineIntersectsLine(lineStart, lineEnd, this.P4, this.P1);
		}

		private static bool LineIntersectsLine(Vector2 startLine1, Vector2 endLine1, Vector2 startLine2, Vector2 endLine2)
		{
			float d = Vector2.Cross(endLine1.X - startLine1.X, endLine1.Y - startLine1.Y, endLine2.X - startLine2.X, endLine2.Y - startLine2.Y);
			if (d == 0) return false;

			float r = Vector2.Cross(startLine1.Y - startLine2.Y, startLine1.X - startLine2.X, endLine2.Y - startLine2.Y, endLine2.X - startLine2.X) / d;
			if (r < 0 || r > 1) return false;

			float s = Vector2.Cross(startLine1.Y - startLine2.Y, startLine1.X - startLine2.X, endLine1.Y - startLine1.Y, endLine1.X - startLine1.X) / d;
			if (s < 0 || s > 1) return false;

			return true;
		}
	}
}
