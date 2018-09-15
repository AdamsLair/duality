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
		private string fieldName;
		private IntPtr offset;
		private VertexElementType type;
		private int count;

		/// <summary>
		/// [GET] The vertex elements preferred shader field name to map to.
		/// </summary>
		public string FieldName
		{
			get { return this.fieldName; }
		}
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

		internal VertexElement(string fieldName, IntPtr offset, VertexElementType type, int count)
		{
			this.fieldName = fieldName;
			this.offset = offset;
			this.type = type;
			this.count = count;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}[{2}] at {3}", this.fieldName, this.type, this.count, this.offset);
		}
	}
}