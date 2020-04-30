using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines a writable slice in a shared vertex array. This type sacrifices
	/// validation and parameter checking for some extra performance - when specifying
	/// parameters, reading and writing data, do so with the same caution that you
	/// would apply to unsafe code.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DebuggerTypeProxy(typeof(VertexSlice<>.DebuggerTypeProxy))]
	[DebuggerDisplay("Length = {Length}")]
	public struct VertexSlice<T> where T : struct, IVertexData
	{
		/// <summary>
		/// The shared vertex array that is addressed in this slice.
		/// </summary>
		public T[] Data;
		/// <summary>
		/// The index offset to the beginning of <see cref="Data"/> at which this slice starts.
		/// </summary>
		public int Offset;
		/// <summary>
		/// The length of this slice.
		/// </summary>
		public int Length;

		/// <summary>
		/// Addresses the underlying <see cref="Data"/> starting at the <see cref="Offset"/> of this slice.
		/// </summary>
		/// <param name="index"></param>
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

		public VertexSlice(T[] data, int offset, int length)
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

		public override string ToString()
		{
			return string.Format(
				"{0}, Offset {1}, Length {2}", 
				(this.Data != null) ? this.Data.ToString() : "null", 
				this.Offset, 
				this.Length);
		}

		internal sealed class DebuggerTypeProxy
		{
			private VertexSlice<T> slice;

			public DebuggerTypeProxy(VertexSlice<T> slice)
			{
				this.slice = slice;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public T[] Items
			{
				get
				{
					if (this.slice.Data == null) return null;
					if (this.slice.Offset < 0 || this.slice.Offset >= this.slice.Data.Length) return null;
					if (this.slice.Length < 0 || this.slice.Length > this.slice.Data.Length - this.slice.Offset) return null;

					T[] array = new T[this.slice.Length];
					for (int i = 0; i < this.slice.Length; i++)
					{
						array[i] = this.slice[i];
					}
					return array;
				}
			}
		}
	}
}
