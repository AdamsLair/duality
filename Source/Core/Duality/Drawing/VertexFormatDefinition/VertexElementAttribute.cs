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
		private string name;
		private int count;

		public VertexElementType Type
		{
			get { return this.type; }
		}
		public string Name
		{
			get { return this.name; }
		}
		public int Count
		{
			get { return this.count; }
		}

		public VertexElementAttribute(string name) : this(VertexElementType.Unknown, 0, name) { }
		public VertexElementAttribute(VertexElementType type, int count, string name = null)
		{
			this.type = type;
			this.count = count;
			this.name = name;
		}
	}
}