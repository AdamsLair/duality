using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Duality.Drawing
{
	public interface IVertexArray
	{
		int Count { get; }
		VertexDeclaration Declaration { get; }

		void Clear();
		RawList<T> GetTypedData<T>();
		VertexArrayLock Lock();
	}
}
