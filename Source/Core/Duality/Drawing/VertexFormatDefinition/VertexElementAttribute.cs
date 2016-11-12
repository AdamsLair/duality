using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace Duality.Drawing
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class VertexElementAttribute : Attribute
	{
		private VertexElementType type;
		private VertexElementRole role;
		private int count;

		public VertexElementType Type
		{
			get { return this.type; }
		}
		public VertexElementRole Role
		{
			get { return this.role; }
		}
		public int Count
		{
			get { return this.count; }
		}

		public VertexElementAttribute(VertexElementRole role) : this(VertexElementType.Unknown, 0, role) { }
		public VertexElementAttribute(VertexElementType type, int count, VertexElementRole role = VertexElementRole.Unknown)
		{
			this.type = type;
			this.count = count;
			this.role = role;
		}
	}
}