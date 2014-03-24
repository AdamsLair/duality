using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace Duality
{
	/// <summary>
	/// Represents two-dimensional grid-aligned data.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	[DebuggerTypeProxy(typeof(Grid<>.DebuggerTypeProxy))]
	[DebuggerDisplay("Width = {Width}, Height = {Height}")]
	public class Grid<T> : IEnumerable<T>
	{
		/// <summary>
		/// Specifies flags on how to shrink a grid to its minimal size.
		/// </summary>
		[Flags]
		public enum ShrinkMode
		{
			/// <summary>
			/// Don't shrink the grid at all.
			/// </summary>
			None	= 0x0,
			/// <summary>
			/// Shrink the grid horizontally.
			/// </summary>
			X		= 0x1,
			/// <summary>
			/// Shrink the grid vertically.
			/// </summary>
			Y		= 0x2,
			/// <summary>
			/// Shrink the grid both horizontally and vertically.
			/// </summary>
			Both	= X | Y
		}


		private RawList<T>	sequence;
		private	int			width;
		private	int			height;


		/// <summary>
		/// [GET] The underlying raw data of the grid. You shouldn't need to access this in regular usage - it's just there to
		/// allow to operate directly on the data block for higher performance on large batch operations.
		/// </summary>
		public T[] RawData
		{
			get { return this.sequence.Data; }
		}
		/// <summary>
		/// [GET / SET] The grids width. Setting this will perform a resize operation.
		/// </summary>
		public int Width
		{
			get { return this.width; }
			set { this.Resize(value, this.height); }
		}
		/// <summary>
		/// [GET / SET] The grids height. Setting this will perform a resize operation.
		/// </summary>
		public int Height
		{
			get { return this.height; }
			set { this.Resize(this.width, value); }
		}
		/// <summary>
		/// [GET / SET] The grids total capacity for elements. Equals <see cref="Width"/> times <see cref="Height"/>.
		/// </summary>
		public int Capacity
		{
			get { return this.width * this.height; }
		}

		/// <summary>
		/// [GET / SET] Accesses a grid element at the specified position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public T this[int x, int y]
		{
			get
			{
				if (x < 0 || x >= width) throw new IndexOutOfRangeException();
				if (y < 0 || y >= height) throw new IndexOutOfRangeException();
				return this.sequence.Data[x + y * width];
			}
			set
			{
				if (x < 0 || x >= width) throw new IndexOutOfRangeException();
				if (y < 0 || y >= height) throw new IndexOutOfRangeException();
				this.sequence.Data[x + y * width] = value;
			}
		}


		/// <summary>
		/// Creates a new, empty grid.
		/// </summary>
		public Grid() : this(0, 0, null) {}
		/// <summary>
		/// Creates a new grid of the specified size.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Grid(int width, int height) : this(width, height, null) {}
		/// <summary>
		/// Creates a new grid based on the specified raw data array. It will not be copied, but directly used.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="wrapAround"></param>
		public Grid(int width, int height, T[] wrapAround)
		{
			if (wrapAround == null) wrapAround = new T[width * height];

			this.width = width;
			this.height = height;
			this.sequence = new RawList<T>(wrapAround, width * height);
		}
		/// <summary>
		/// Creates a copy of the specified grid.
		/// </summary>
		/// <param name="other"></param>
		public Grid(Grid<T> other)
		{
			this.width = other.width;
			this.height = other.height;
			this.sequence = new RawList<T>(other.sequence.Data.ToArray(), other.sequence.Count);
		}
		
		/// <summary>
		/// Determines the two-dimensional grid index from the specified raw data index.
		/// </summary>
		/// <param name="dataIndex"></param>
		/// <returns></returns>
		public Point GetGridIndex(int dataIndex)
		{
			return new Point(dataIndex % this.width, dataIndex / this.width);
		}
		/// <summary>
		/// Determines the raw data index from the specified two-dimensional grid index.
		/// </summary>
		/// <param name="gridX"></param>
		/// <param name="gridY"></param>
		/// <returns></returns>
		public int GetDataIndex(int gridX, int gridY)
		{
			return gridX + gridY * this.width;
		}

		/// <summary>
		/// Determines the index of the specified item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public Point IndexOf(T item)
		{
			int index = Array.IndexOf(this.sequence.Data, item, 0, this.sequence.Count);
			if (index == -1)
				return new Point(-1, -1);
			else
				return this.GetGridIndex(index);
		}
		/// <summary>
		/// Finds an item that matches the specified predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public T Find(Predicate<T> predicate)
		{
			T[] data = this.sequence.Data;
			int count = this.sequence.Count;
			for (int i = 0; i < count; i++)
			{
				if (predicate(data[i]))
				{
					return data[i];
				}
			}
			return default(T);
		}
		/// <summary>
		/// Finds all items that match the specified predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public IEnumerable<T> FindAll(Predicate<T> predicate)
		{
			T[] data = this.sequence.Data;
			int count = this.sequence.Count;
			for (int i = 0; i < count; i++)
			{
				if (predicate(data[i]))
				{
					yield return data[i];
				}
			}
			yield break;
		}
		/// <summary>
		/// Finds the index of an item that matches the specified predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public Point FindIndex(Predicate<T> predicate)
		{
			T[] data = this.sequence.Data;
			int count = this.sequence.Count;
			for (int i = 0; i < count; i++)
			{
				if (predicate(data[i]))
				{
					return this.GetGridIndex(i);
				}
			}
			return new Point(-1, -1);
		}
		/// <summary>
		/// Finds all indices of items that match the specified predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public IEnumerable<Point> FindAllIndices(Predicate<T> predicate)
		{
			T[] data = this.sequence.Data;
			int count = this.sequence.Count;
			for (int i = 0; i < count; i++)
			{
				if (predicate(data[i]))
				{
					yield return this.GetGridIndex(i);
				}
			}
			yield break;
		}
		/// <summary>
		/// Determines whether the specified item is contained within this grid.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			int index = Array.IndexOf(this.sequence.Data, item, 0, this.sequence.Count);
			return index >= 0;
		}
		/// <summary>
		/// Determines the boundaries of the grids non-null content.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetContentBoundaries()
		{
			Rectangle bounds = new Rectangle(this.width, this.height, 0, 0);

			int count = this.sequence.Count;
			T[] data = this.sequence.Data;
			for (int i = 0; i < count; i++)
			{
				if (GenericOperator.Equal(data[i], default(T))) continue;
				int cX = i % this.width;
				int cY = i / this.width;
				bounds.X = Math.Min(bounds.X, cX);
				bounds.Y = Math.Min(bounds.Y, cY);
				bounds.Width = Math.Max(bounds.Width, cX);
				bounds.Height = Math.Max(bounds.Height, cY);
			}
			bounds.Width = 1 + Math.Max(0, bounds.Width - bounds.X);
			bounds.Height = 1 + Math.Max(0, bounds.Height - bounds.Y);

			return bounds;
		}

		/// <summary>
		/// Fills a rectangular region of the grid with the specified value.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Fill(T item, int x, int y, int width, int height)
		{
			if (width < 0)
			{
				x += width;
				width = -width;
			}
			if (height < 0)
			{
				y += height;
				height = -height;
			}

			if (x < 0 || x + width - 1 >= this.width) throw new IndexOutOfRangeException();
			if (y < 0 || y + height - 1 >= this.height) throw new IndexOutOfRangeException();
			
			T[] data = this.sequence.Data;
			for (int i = x; i < x + width; i++)
			{
				for (int j = y; j < y + height; j++)
				{
					int n = i + this.width * j;
					data[n] = item;
				}
			}
		}
		/// <summary>
		/// Removes the specified item from the grid.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			int index = Array.IndexOf(this.sequence.Data, item, 0, this.sequence.Count);
			if (index >= 0)
			{
				this.sequence.Data[index] = default(T);
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// Removes all items from the grid that match the specified criteria.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public int RemoveAll(Predicate<T> predicate)
		{
			int matchCount = 0;
			T[] data = this.sequence.Data;
			for (int i = this.sequence.Count - 1; i >= 0; i--)
			{
				if (predicate(data[i]))
				{
					data[i] = default(T);
					matchCount++;
				}
			}
			return matchCount;
		}
		/// <summary>
		/// Clears the grid by overwriting it with null values.
		/// </summary>
		public void Clear()
		{
			T[] data = this.sequence.Data;
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = default(T);
			}
		}

		/// <summary>
		/// Resizes the grid and translates its contents, so it assumes the specified rectangular region,
		/// relative to its former shape. X and Y values may be negative, to grow the grid on its left or top side.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void AssumeRect(int x, int y, int width, int height)
		{
			if (width < 0) throw new ArgumentException("Width needs to be greater than or equal to zero.", "width");
			if (height < 0) throw new ArgumentException("Height needs to be greater than or equal to zero.", "height");
			if (x == 0 && y == 0 && width == this.width && height == this.height) return;

			// Create a new grid of the requried target size.
			Grid<T> tempGrid = new Grid<T>(width, height);
			this.CopyTo(tempGrid, -x, -y);

			// Become the temporary grid
			this.sequence = tempGrid.sequence;
			this.width = tempGrid.width;
			this.height = tempGrid.height;
		}
		/// <summary>
		/// Resizes the grid.
		/// </summary>
		/// <param name="newWidth"></param>
		/// <param name="newHeight"></param>
		/// <param name="origin"></param>
		public void Resize(int newWidth, int newHeight, Alignment origin = Alignment.TopLeft)
		{
			if (newWidth == this.width && newHeight == this.height) return;
			
			int x = 0;
			int y = 0;

			if (origin == Alignment.Right || 
				origin == Alignment.TopRight || 
				origin == Alignment.BottomRight)
				x = newWidth - this.width;
			else if (
				origin == Alignment.Center || 
				origin == Alignment.Top || 
				origin == Alignment.Bottom)
				x = (newWidth - this.width) / 2;

			if (origin == Alignment.Bottom || 
				origin == Alignment.BottomLeft || 
				origin == Alignment.BottomRight)
				y = newHeight - this.height;
			else if (
				origin == Alignment.Center || 
				origin == Alignment.Left || 
				origin == Alignment.Right)
				y = (newHeight - this.height) / 2;

			this.AssumeRect(-x, -y, newWidth, newHeight);
		}
		/// <summary>
		/// Shrinks the grid to match its non-null content boundaries.
		/// </summary>
		/// <param name="mode"></param>
		public void ShrinkToFit(ShrinkMode mode = ShrinkMode.Both)
		{
			if (mode == ShrinkMode.None) return;
			Rectangle bounds = this.GetContentBoundaries();
			this.AssumeRect(
				mode.HasFlag(ShrinkMode.X) ? bounds.X : 0, 
				mode.HasFlag(ShrinkMode.Y) ? bounds.Y : 0, 
				mode.HasFlag(ShrinkMode.X) ? bounds.Width : this.width, 
				mode.HasFlag(ShrinkMode.Y) ? bounds.Height : this.height);
		}
		
		/// <summary>
		/// Copies the grids contents to the specified other grid.
		/// </summary>
		/// <typeparam name="U"></typeparam>
		/// <param name="target"></param>
		/// <param name="destX"></param>
		/// <param name="destY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="srcX"></param>
		/// <param name="srcY"></param>
		/// <param name="selector"></param>
		public void CopyTo<U>(Grid<U> target, int destX = 0, int destY = 0, int width = -1, int height = -1, int srcX = 0, int srcY = 0, Func<T,U,U> selector = null)
		{
			if (width == -1) width = this.width;
			if (height == -1) height = this.height;

			int beginX = MathF.Max(0, -destX, -srcX);
			int beginY = MathF.Max(0, -destY, -srcY);
			int endX = MathF.Min(width, this.width, target.width - destX, this.width - srcX);
			int endY = MathF.Min(height, this.height, target.height - destY, this.height - srcY);
			if (endX - beginX < 1) return;
			if (endY - beginY < 1) return;

			U[] targetData = target.sequence.Data;
			T[] sourceData = this.sequence.Data;
			if (selector != null)
			{
				for (int i = beginX; i < endX; i++)
				{
					for (int j = beginY; j < endY; j++)
					{
						int sourceN = srcX + i + this.width * (srcY + j);
						int targetN = destX + i + target.width * (destY + j);
						targetData[targetN] = selector(sourceData[sourceN], targetData[targetN]);
					}
				}
			}
			else
			{
				for (int i = beginX; i < endX; i++)
				{
					for (int j = beginY; j < endY; j++)
					{
						int sourceN = srcX + i + this.width * (srcY + j);
						int targetN = destX + i + target.width * (destY + j);
						targetData[targetN] = GenericOperator.Convert<T,U>(sourceData[sourceN]);
					}
				}
			}
		}
		/// <summary>
		/// Copies the grids contents to the specified array, line by line.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.sequence.Data, 0, array, arrayIndex, this.sequence.Count);
		}
		public IEnumerator<T> GetEnumerator()
		{
			int count = this.sequence.Count;
			T[] data = this.sequence.Data;
			for (int i = 0; i < count; i++)
			{
				yield return data[i];
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}


		internal sealed class DebuggerTypeProxy
		{
			private Grid<T> grid;

			public DebuggerTypeProxy(Grid<T> grid)
			{
				if (grid == null) throw new ArgumentNullException("grid");
				this.grid = grid;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerTypeProxyItem[] Items
			{
				get
				{
					DebuggerTypeProxyItem[] array = new DebuggerTypeProxyItem[this.grid.sequence.Count];
					int count = this.grid.sequence.Count;
					T[] data = this.grid.sequence.Data;
					for (int i = 0; i < count; i++)
					{
						array[i] = new DebuggerTypeProxyItem(i % this.grid.width, i / this.grid.width, data[i]);
					}
					return array;
				}
			}
		}
		[DebuggerDisplay("{Value}", Name = "[{X}, {Y}]")]
		internal sealed class DebuggerTypeProxyItem
		{
			private int X;
			private int Y;
			private T Value;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public T Item
			{
				get { return this.Value; }
			}

			public DebuggerTypeProxyItem(int posX, int posY, T value)
			{
				this.Value = value;
				this.X = posX;
				this.Y = posY;
			}
		}
	}
}
