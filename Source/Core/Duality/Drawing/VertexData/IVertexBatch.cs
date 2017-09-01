using System;
using System.Collections.Generic;

namespace Duality.Drawing
{
	public interface IVertexBatch
	{
		int Count { get; }
		VertexDeclaration Declaration { get; }

		void Clear();
		RawList<T> GetTypedData<T>();
		PinnedArrayHandle Lock();
	}
}
