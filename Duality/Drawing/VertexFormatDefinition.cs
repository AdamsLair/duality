using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace Duality.Drawing
{
	[DontSerialize]
	public class VertexFormatDefinition
	{
		private static Dictionary<string,int> vertexTypeIndexMap = new Dictionary<string,int>();
		private static int GetVertexTypeIndex(Type dataType)
		{
			int index;
			string name = dataType.FullName;
			if (vertexTypeIndexMap.TryGetValue(name, out index)) return index;

			index = 0;
			while (vertexTypeIndexMap.Values.Contains(index))
			{
				index++;
			}

			vertexTypeIndexMap[name] = index;
			return index;
		}

		private Type dataType;
		private int typeIndex;
		private int size;
		private VertexField[] elements;

		public Type DataType
		{
			get { return this.dataType; }
		}
		public int TypeIndex
		{
			get { return this.typeIndex; }
		}
		public int Size
		{
			get { return this.size; }
		}
		public VertexField[] Elements
		{
			get { return this.elements; }
		}

		public VertexFormatDefinition(Type dataType)
		{
			if (dataType.IsClass) throw new InvalidOperationException("Vertex formats need to be structs. Classes are not supported.");

			FieldInfo[] fields = dataType.GetFields(ReflectionHelper.BindInstanceAll);

			this.dataType = dataType;
			this.typeIndex = GetVertexTypeIndex(dataType);
			this.size = Marshal.SizeOf(dataType);
			this.elements = new VertexField[fields.Length];

			for (int i = 0; i < fields.Length; i++)
			{
				VertexFieldRole role = VertexFieldRole.Unknown;
				VertexFieldType type = VertexFieldType.Unknown;
				int count = 0;

				VertexFieldAttribute attrib = fields[i].GetAttributesCached<VertexFieldAttribute>().FirstOrDefault();
				if (attrib != null)
				{
					role = attrib.Role;
					type = attrib.Type;
					count = attrib.Count;
				}

				if (type == VertexFieldType.Unknown || count == 0)
				{
					DetermineElement(fields[i].FieldType, out type, out count);
				}
				if (type == VertexFieldType.Unknown || count == 0)
				{
					throw new InvalidOperationException(string.Format("Unable to determine type of field {2}, vertex format {1}. Add a {0} to specify it explicitly",
						typeof(VertexFieldAttribute).Name,
						dataType.Name,
						fields[i].Name));
				}

				this.elements[i] = new VertexField(Marshal.OffsetOf(dataType, fields[i].Name), type, count, role);
			}
		}

		private static bool DetermineElement(Type dataType, out VertexFieldType type, out int count)
		{
			if (dataType == typeof(float))
			{
				type = VertexFieldType.Float;
				count = 1;
				return true;
			}
			else if (dataType == typeof(byte))
			{
				type = VertexFieldType.Byte;
				count = 1;
				return true;
			}
			else if (!dataType.IsClass && !dataType.IsEnum && !dataType.IsPrimitive)
			{
				type = VertexFieldType.Unknown;
				count = 0;

				FieldInfo[] fields = dataType.GetFields(ReflectionHelper.BindInstanceAll);
				for (int i = 0; i < fields.Length; i++)
				{
					VertexFieldType fieldType;
					int fieldCount;
					if (!DetermineElement(fields[i].FieldType, out fieldType, out fieldCount))
						return false;

					if (type == VertexFieldType.Unknown)
						type = fieldType;
					else if (fieldType != type)
						return false;

					count += fieldCount;
				}

				return true;
			}
			
			type = VertexFieldType.Unknown;
			count = 0;
			return false;
		}
	}

	public struct VertexField
	{
		private IntPtr offset;
		private VertexFieldType type;
		private int count;
		private VertexFieldRole role;

		public IntPtr Offset
		{
			get { return this.offset; }
		}
		public VertexFieldType Type
		{
			get { return this.type; }
		}
		public int Count
		{
			get { return this.count; }
		}
		public VertexFieldRole Role
		{
			get { return this.role; }
		}

		internal VertexField(IntPtr offset, VertexFieldType type, int count, VertexFieldRole role)
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

	public enum VertexFieldType
	{
		Unknown,

		Byte,
		Float
	}

	public enum VertexFieldRole
	{
		Unknown,

		Position,
		TexCoord,
		Color
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class VertexFieldAttribute : Attribute
	{
		private VertexFieldType type;
		private VertexFieldRole role;
		private int count;

		public VertexFieldType Type
		{
			get { return this.type; }
		}
		public VertexFieldRole Role
		{
			get { return this.role; }
		}
		public int Count
		{
			get { return this.count; }
		}

		public VertexFieldAttribute(VertexFieldRole role) : this(VertexFieldType.Unknown, 0, role) { }
		public VertexFieldAttribute(VertexFieldType type, int count, VertexFieldRole role = VertexFieldRole.Unknown)
		{
			this.type = type;
			this.count = count;
			this.role = role;
		}
	}
}