using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// A custom data structure for efficiently storing and querying vertex / corner node connectivity in a <see cref="Tilemap"/>.
	/// Each coordinate in a <see cref="TileEdgeMap"/> refers to a single vertex / corner node and stores a bitmask of all connections
	/// to its 8-tile neighbourhood. All connections are considered bidirectional.
	/// </summary>
	public class TileEdgeMap
	{
		[Flags]
		private enum Connection : byte
		{
			None      = 0x00,

			Up        = 0x01,
			RightUp   = 0x02,
			Right     = 0x04,
			RightDown = 0x08,
			Down      = 0x10,
			LeftDown  = 0x20,
			Left      = 0x40,
			LeftUp    = 0x80
		}


		private Grid<Connection> connections;


		/// <summary>
		/// [GET] The amount of vertices / corner nodes on the horizontal axis.
		/// </summary>
		public int Width
		{
			get { return this.connections.Width; }
		}
		/// <summary>
		/// [GET] The amount of vertices / corner nodes on the vertical axis
		/// </summary>
		public int Height
		{
			get { return this.connections.Height; }
		}


		public TileEdgeMap(int width, int height)
		{
			this.connections = new Grid<Connection>(width, height);
		}

		/// <summary>
		/// Adds a new edge connection to the <see cref="TileEdgeMap"/>.
		/// Both specified nodes need to be adjacent to each other.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void AddEdge(Point2 from, Point2 to)
		{
			Connection con = GetConnection(new Point2(to.X - from.X, to.Y - from.Y));
			this.connections[from.X, from.Y] |= con;
			this.connections[to.X, to.Y] |= InvertConnection(con);
		}
		/// <summary>
		/// Removes an existing edge connection from the <see cref="TileEdgeMap"/>.
		/// Both specified nodes need to be adjacent to each other.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void RemoveEdge(Point2 from, Point2 to)
		{
			Connection con = GetConnection(new Point2(to.X - from.X, to.Y - from.Y));
			this.connections[from.X, from.Y] &= ~con;
			this.connections[to.X, to.Y] &= ~InvertConnection(con);
		}
		/// <summary>
		/// Determines whether the specified edge is part of the <see cref="TileEdgeMap"/>.
		/// Both specified nodes need to be adjacent to each other.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public bool HasEdge(Point2 from, Point2 to)
		{
			Connection con = GetConnection(new Point2(to.X - from.X, to.Y - from.Y));
			return (this.connections[from.X, from.Y] & con) != Connection.None;
		}
		/// <summary>
		/// Starting from the specified node, this method returns the first neighbour
		/// that is connected to it, starting at the one directly above it and proceeding clockwise.
		/// Returns (-1, -1) if no neighbour is connected.
		/// </summary>
		/// <param name="from"></param>
		/// <returns></returns>
		public Point2 GetClockwiseNextFrom(Point2 from)
		{
			Connection con = this.connections[from.X, from.Y];

			if ((con & Connection.Up       ) != Connection.None) return new Point2(from.X    , from.Y - 1);
			if ((con & Connection.RightUp  ) != Connection.None) return new Point2(from.X + 1, from.Y - 1);
			if ((con & Connection.Right    ) != Connection.None) return new Point2(from.X + 1, from.Y    );
			if ((con & Connection.RightDown) != Connection.None) return new Point2(from.X + 1, from.Y + 1);
			if ((con & Connection.Down     ) != Connection.None) return new Point2(from.X    , from.Y + 1);
			if ((con & Connection.LeftDown ) != Connection.None) return new Point2(from.X - 1, from.Y + 1);
			if ((con & Connection.Left     ) != Connection.None) return new Point2(from.X - 1, from.Y    );
			if ((con & Connection.LeftUp   ) != Connection.None) return new Point2(from.X - 1, from.Y - 1);

			return new Point2(-1, -1);
		}
		/// <summary>
		/// Finds the first node that is connected to any other node.
		/// Returns (-1, -1) if no node is connected to any other.
		/// </summary>
		/// <returns></returns>
		public Point2 FindNonEmpty()
		{
			return this.connections.FindIndex(c => c != Connection.None);
		}
		/// <summary>
		/// Clears all edges / connections between nodes.
		/// </summary>
		public void Clear()
		{
			this.connections.Clear();
		}


		private static readonly Connection[] DiffHashToConnection = new Connection[]
		{
			Connection.LeftUp,
			Connection.Up,
			Connection.RightUp,
			Connection.None,     // Invalid (Unused)
			Connection.Left,
			Connection.None,     // Invalid (Center)
			Connection.Right,
			Connection.None,     // Invalid (Unused)
			Connection.LeftDown,
			Connection.Down,
			Connection.RightDown
		};
		private static Connection GetConnection(Point2 diff)
		{
			int diffHash = (diff.X + 1) + (diff.Y + 1) * 4;
			return (diffHash < 0 || diffHash > 10) ? Connection.None : DiffHashToConnection[diffHash];
		}
		private static Connection InvertConnection(Connection con)
		{
			switch (con)
			{
				default:                   return Connection.None;
				case Connection.Up:        return Connection.Down;
				case Connection.RightUp:   return Connection.LeftDown;
				case Connection.Right:     return Connection.Left;
				case Connection.RightDown: return Connection.LeftUp;
				case Connection.Down:      return Connection.Up;
				case Connection.LeftDown:  return Connection.RightUp;
				case Connection.Left:      return Connection.Right;
				case Connection.LeftUp:    return Connection.RightDown;
			}
		}
	}
}
