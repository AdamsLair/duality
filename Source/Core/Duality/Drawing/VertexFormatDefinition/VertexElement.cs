using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace Duality.Drawing
{
	public struct VertexElement
	{
		private IntPtr offset;
		private VertexElementType type;
		private int count;
		private VertexElementRole role;

		public IntPtr Offset
		{
			get { return this.offset; }
		}
		public VertexElementType Type
		{
			get { return this.type; }
		}
		public int Count
		{
			get { return this.count; }
		}
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