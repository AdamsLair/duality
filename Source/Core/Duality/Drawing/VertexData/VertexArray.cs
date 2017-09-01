using System;
using System.Collections.Generic;

namespace Duality.Drawing
{
	public class VertexArray<T> : IVertexArray where T : struct, IVertexData
	{
		private RawList<T> vertices = new RawList<T>();

		public RawList<T> Vertices
		{
			get { return this.vertices; }
		}
		public int Count
		{
			get { return this.vertices.Count; }
		}
		public VertexDeclaration Declaration
		{
			get { return VertexDeclaration.Get<T>(); }
		}

		public void Clear()
		{
			this.vertices.Clear();
		}
		public PinnedArrayHandle Lock()
		{
			return new PinnedArrayHandle(this.vertices.Data);
		}

		RawList<U> IVertexArray.GetTypedData<U>()
		{
			return this.vertices as RawList<U>;
		}
	}
}
