using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Duality
{
	/// <summary>
	/// Represents a readonly interface for two-dimensional grid-aligned data.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IReadOnlyGrid<out T> : IEnumerable<T>
	{
		/// <summary>
		/// [GET] The grids width.
		/// </summary>
		int Width { get; }
		/// <summary>
		/// [GET] The grids height.
		/// </summary>
		int Height { get; }

		/// <summary>
		/// [GET] Accesses a grid element at the specified position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		T this[int x, int y] { get; }
	}
}
