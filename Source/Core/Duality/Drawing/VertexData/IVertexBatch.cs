using System;
using System.Collections.Generic;

namespace Duality.Drawing
{
	/// <summary>
	/// Represents a single batch of dynamically gathered vertex data.
	/// </summary>
	public interface IVertexBatch
	{
		/// <summary>
		/// [GET] The number of vertices in this batch.
		/// </summary>
		int Count { get; }
		/// <summary>
		/// [GET] The <see cref="VertexDeclaration"/> that is associated with 
		/// the type of vertex data in this batch.
		/// </summary>
		VertexDeclaration Declaration { get; }
		
		/// <summary>
		/// Clears the batch of all vertex data.
		/// </summary>
		void Clear();
		/// <summary>
		/// Locks the batch in order to access its vertex data directly using an <see cref="IntPtr"/>.
		/// </summary>
		/// <returns></returns>
		PinnedArrayHandle Lock();
	}
}
