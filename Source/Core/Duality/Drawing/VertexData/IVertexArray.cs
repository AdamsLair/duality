using System;
using System.Collections.Generic;

namespace Duality.Drawing
{
	public interface IVertexArray
	{
		int Count { get; }
		VertexDeclaration Declaration { get; }

		void Clear();
		RawList<T> GetTypedData<T>();
		PinnedArrayHandle Lock();
	}
}
