using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Resources;
using Duality.Cloning;

namespace Duality.Drawing
{
	/// <summary>
	/// This class handles buffering and reusing vertex arrays created by a <see cref="Duality.Drawing.Canvas"/> and is a measure of
	/// performance and memory footprint improvement when using <see cref="Duality.Drawing.Canvas"/> on a regular basis.
	/// </summary>
	public class CanvasBuffer
	{
		private RawListPool<VertexC1P3T2> pool = null;


		public CanvasBuffer() : this(false) {}
		internal CanvasBuffer(bool isDummy)
		{
			if (!isDummy)
			{
				this.pool = new RawListPool<VertexC1P3T2>();
			}
		}


		/// <summary>
		/// Creates or retrieves a vertex array of at least the specified size.
		/// </summary>
		/// <param name="desiredSize">Minimum size of the new or reused vertex array.</param>
		/// <returns></returns>
		public VertexC1P3T2[] RequestVertexArray(int desiredSize)
		{
			if (this.pool == null)
				return new VertexC1P3T2[desiredSize];
			else
				return this.pool.Rent(desiredSize).Data;
		}

		/// <summary>
		/// Resets the buffer for being re-used in future rendering frames.
		/// </summary>
		public void Reset()
		{
			if (this.pool == null) return;
			this.pool.Reset();
		}
	}
}