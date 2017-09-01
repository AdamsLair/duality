using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Describes a single vertex element in a <see cref="VertexDeclaration"/>.
	/// </summary>
	public struct VertexElement
	{
		private IntPtr offset;
		private VertexElementType type;
		private int count;
		private VertexElementRole role;

		/// <summary>
		/// [GET] Byte offset of this element relative to the vertex start address.
		/// </summary>
		public IntPtr Offset
		{
			get { return this.offset; }
		}
		/// <summary>
		/// [GET] Field type of this element.
		/// </summary>
		public VertexElementType Type
		{
			get { return this.type; }
		}
		/// <summary>
		/// [GET] Number of fields of the specified <see cref="Type"/> in this element.
		/// </summary>
		public int Count
		{
			get { return this.count; }
		}
		/// <summary>
		/// [GET] The rendering role that this vertex element is assigned to.
		/// </summary>
		public VertexElementRole Role
		{
			get { return this.role; }
		}

		internal VertexElement(IntPtr offset, VertexElementType type, int count, VertexElementRole role)
		{
			this.offset = offset;
			this.type = type;
			this.count = count;
			this.role = role;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1} x {2} at {3}", this.role, this.type, this.count, this.offset);
		}
	}
}