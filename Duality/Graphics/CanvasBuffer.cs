using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;
using OpenTK;

using Duality.VertexFormat;
using Duality.ColorFormat;
using Duality.Resources;

namespace Duality
{
	/// <summary>
	/// This class handles buffering and reusing vertex arrays created by a <see cref="Duality.Canvas"/> and is a measure of
	/// performance and memory footprint improvement when using <see cref="Duality.Canvas"/> on a regular basis.
	/// </summary>
	public class CanvasBuffer
	{
		private	bool							dummy				= false;
		private	List<RawList<VertexC1P3T2>>		vertexArraysFree	= null;
		private	List<RawList<VertexC1P3T2>>		vertexArraysUsed	= null;


		public CanvasBuffer() : this(false) {}
		internal CanvasBuffer(bool isDummy)
		{
			if (!isDummy)
			{
				this.vertexArraysFree = new List<RawList<VertexC1P3T2>>();
				this.vertexArraysUsed = new List<RawList<VertexC1P3T2>>();
				this.dummy = false;
			}
			else
			{
				this.dummy = true;
			}
		}


		/// <summary>
		/// Adds the specified vertex arrays to the Canvas' buffering mechanism. As long as buffers are available, the
		/// Canvas will prefer re-using one of them over creating a new vertex array. Every vertex array will only be used once.
		/// </summary>
		/// <param name="arrays"></param>
		public void AddVertexArrays(IEnumerable<RawList<VertexC1P3T2>> arrays)
		{
			if (arrays == null) throw new ArgumentNullException("buffer");
			if (this.dummy) return;
			foreach (var buffer in arrays)
			{
				this.AddVertexArray(buffer);
			}
		}
		/// <summary>
		/// Adds the specified vertex array to the Canvas' buffering mechanism. As long as buffers are available, the
		/// Canvas will prefer re-using one of them over creating a new vertex array. Every vertex array will only be used once.
		/// </summary>
		/// <param name="array"></param>
		public void AddVertexArray(RawList<VertexC1P3T2> array)
		{
			if (array == null) throw new ArgumentNullException("buffer");
			if (this.dummy) return;
			if (this.vertexArraysFree.Contains(array)) return;
			if (this.vertexArraysUsed.Contains(array)) return;
			this.vertexArraysFree.Add(array);
		}
		/// <summary>
		/// Removes the specified vertex array from the Canvas' buffering mechanism.
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public bool RemoveVertexArray(RawList<VertexC1P3T2> array)
		{
			if (this.dummy) return false;
			return this.vertexArraysUsed.Remove(array) || this.vertexArraysFree.Remove(array);
		}
		/// <summary>
		/// Creates or retrieves a vertex array of at least the specified size.
		/// </summary>
		/// <param name="desiredSize">Minimum size of the new or reused vertex array.</param>
		/// <returns></returns>
		public VertexC1P3T2[] RequestVertexArray(int desiredSize)
		{
			// Dummy buffer? Default to regular array allocation
			if (this.dummy) return new VertexC1P3T2[desiredSize];

			// No buffers available? Create perfectly fitting array on-the-fly.
			if (this.vertexArraysFree.Count == 0)
			{
				RawList<VertexC1P3T2> vertexArray = new RawList<VertexC1P3T2>(desiredSize);
				this.vertexArraysUsed.Add(vertexArray);
				return vertexArray.Data;
			}

			// Determine the smallest fitting buffer regarding our desired buffer size
			RawList<VertexC1P3T2> bestBuffer = null;
			int bestSizeDiff = int.MaxValue;
			for (int i = 0; i < this.vertexArraysFree.Count; i++)
			{
				int sizeDiff = this.vertexArraysFree[i].Capacity - desiredSize;
				if (sizeDiff >= 0 && sizeDiff < bestSizeDiff)
				{
					bestBuffer = this.vertexArraysFree[i];
					bestSizeDiff = sizeDiff;
					if (sizeDiff == 0) break;
				}
			}

			// No buffer found? Use the smallest available buffer.
			if (bestBuffer == null)
			{
				bestBuffer = this.vertexArraysFree[0];
				for (int i = 1; i < this.vertexArraysFree.Count; i++)
				{
					if (this.vertexArraysFree[i].Capacity < bestBuffer.Capacity)
					{
						bestBuffer = this.vertexArraysFree[i];
						if (bestBuffer.Capacity == 0) break;
					}
				}
			}

			// Make sure the buffer has the desired size
			bestBuffer.Count = desiredSize;

			// Remove buffer from available list and return raw array for usage
			this.vertexArraysFree.Remove(bestBuffer);
			this.vertexArraysUsed.Add(bestBuffer);
			return bestBuffer.Data;
		}

		/// <summary>
		/// Resets the buffer for being re-used in future rendering frames.
		/// </summary>
		public void Reset()
		{
			if (this.dummy) return;
			this.vertexArraysFree.AddRange(this.vertexArraysUsed);
			this.vertexArraysUsed.Clear();
		}
	}
}