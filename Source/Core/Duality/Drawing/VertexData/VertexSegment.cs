using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines a writable segment in a shared vertex array. This type sacrifices
	/// validation and parameter checking for some extra performance - when specifying
	/// parameters, reading and writing data, do so with the same caution that you
	/// would apply to unsafe code.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct VertexSegment<T> where T : struct, IVertexData
	{
		/// <summary>
		/// The shared vertex array that is addressed in this segment.
		/// </summary>
		public T[] Data;
		/// <summary>
		/// The index offset to the beginning of <see cref="Data"/> at which this segments starts.
		/// </summary>
		public int Offset;
		/// <summary>
		/// The length of this segment.
		/// </summary>
		public int Length;

		/// <summary>
		/// Addresses the underlying <see cref="Data"/> starting at the <see cref="Offset"/> of this segment.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			// No range checking for performance reasons.
			// Using a throw helper allows inlining, but
			// not checking at all reduces instructions from
			// ~16 to ~11.
			get
			{
#if DEBUG
				if (index < 0 || index >= this.Length)
					throw new IndexOutOfRangeException();
#endif
				return this.Data[this.Offset + index];
			}
			set
			{
#if DEBUG
				if (index < 0 || index >= this.Length)
					throw new IndexOutOfRangeException();
#endif
				this.Data[this.Offset + index] = value;
			}
		}

		public VertexSegment(T[] data, int offset, int length)
		{
			// No argument validation for performance reasons.
			// Keeping it simple allows the JIT compiler to inline
			// the entire constructor with a few mov instructions.
#if DEBUG
			if (data == null) throw new ArgumentNullException("data");
			if (offset < 0 || offset >= data.Length) throw new ArgumentOutOfRangeException("offset");
			if (length < 0 || length > data.Length - offset) throw new ArgumentOutOfRangeException("length");
#endif
			this.Data = data;
			this.Offset = offset;
			this.Length = length;
		}
	}
}
