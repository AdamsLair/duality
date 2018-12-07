using System;
using System.Collections.Generic;

namespace Duality.Drawing
{
	/// <summary>
	/// Represents a single batch of dynamically gathered vertex data.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class VertexBatch<T> : IVertexBatch where T : struct, IVertexData
	{
		private RawList<T> vertices = new RawList<T>();

		/// <summary>
		/// [GET] The vertex data in this batch.
		/// </summary>
		public RawList<T> Vertices
		{
			get { return this.vertices; }
		}
		/// <summary>
		/// [GET] The number of vertices in this batch.
		/// </summary>
		public int Count
		{
			get { return this.vertices.Count; }
		}
		/// <summary>
		/// [GET] The <see cref="VertexDeclaration"/> that is associated with 
		/// the type of vertex data in this batch.
		/// </summary>
		public VertexDeclaration Declaration
		{
			get { return VertexDeclaration.Get<T>(); }
		}

		/// <summary>
		/// Clears the batch of all vertex data.
		/// </summary>
		public void Clear()
		{
			this.vertices.Clear();
		}
		/// <summary>
		/// Locks the batch in order to access its vertex data directly using an <see cref="IntPtr"/>.
		/// </summary>
		/// <returns></returns>
		public PinnedArrayHandle Lock()
		{
			return new PinnedArrayHandle(this.vertices.Data);
		}
	}
}
