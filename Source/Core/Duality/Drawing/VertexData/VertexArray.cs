using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
		public VertexArrayLock Lock()
		{
			return new VertexArrayLock(this.vertices.Data);
		}

		RawList<U> IVertexArray.GetTypedData<U>()
		{
			return this.vertices as RawList<U>;
		}
	}
}
