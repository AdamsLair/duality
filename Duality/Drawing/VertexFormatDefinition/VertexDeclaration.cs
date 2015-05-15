using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace Duality.Drawing
{
	[DontSerialize]
	public class VertexDeclaration
	{
		private static class Cache<T> where T : struct, IVertexData
		{
			public static readonly VertexDeclaration Instance = new VertexDeclaration(typeof(T));
		}
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

		public static VertexDeclaration Get<T>() where T : struct, IVertexData
		{
			return Cache<T>.Instance;
		}
		public static VertexDeclaration Get(Type vertexType)
		{
			IVertexData dummyVertex = vertexType.CreateInstanceOf() as IVertexData;
			return dummyVertex != null ? dummyVertex.Declaration : null;
		}

		private Type dataType;
		private int typeIndex;
		private int size;
		private VertexElement[] elements;

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
		public VertexElement[] Elements
		{
			get { return this.elements; }
		}

		private VertexDeclaration(Type dataType)
		{
			if (dataType.IsClass) throw new InvalidOperationException("Vertex formats need to be structs. Classes are not supported.");

			FieldInfo[] fields = dataType.GetFields(ReflectionHelper.BindInstanceAll);

			this.dataType = dataType;
			this.typeIndex = GetVertexTypeIndex(dataType);
			this.size = Marshal.SizeOf(dataType);
			this.elements = new VertexElement[fields.Length];

			for (int i = 0; i < fields.Length; i++)
			{
				VertexElementRole role = VertexElementRole.Unknown;
				VertexElementType type = VertexElementType.Unknown;
				int count = 0;

				VertexElementAttribute attrib = fields[i].GetAttributesCached<VertexElementAttribute>().FirstOrDefault();
				if (attrib != null)
				{
					role = attrib.Role;
					type = attrib.Type;
					count = attrib.Count;
				}

				if (type == VertexElementType.Unknown || count == 0)
				{
					DetermineElement(fields[i].FieldType, out type, out count);
				}
				if (type == VertexElementType.Unknown || count == 0)
				{
					throw new InvalidOperationException(string.Format("Unable to determine type of field {2}, vertex format {1}. Add a {0} to specify it explicitly",
						typeof(VertexElementAttribute).Name,
						dataType.Name,
						fields[i].Name));
				}

				this.elements[i] = new VertexElement(Marshal.OffsetOf(dataType, fields[i].Name), type, count, role);
			}
		}

		private static bool DetermineElement(Type dataType, out VertexElementType type, out int count)
		{
			if (dataType == typeof(float))
			{
				type = VertexElementType.Float;
				count = 1;
				return true;
			}
			else if (dataType == typeof(byte))
			{
				type = VertexElementType.Byte;
				count = 1;
				return true;
			}
			else if (!dataType.IsClass && !dataType.IsEnum && !dataType.IsPrimitive)
			{
				type = VertexElementType.Unknown;
				count = 0;

				FieldInfo[] fields = dataType.GetFields(ReflectionHelper.BindInstanceAll);
				for (int i = 0; i < fields.Length; i++)
				{
					VertexElementType fieldType;
					int fieldCount;
					if (!DetermineElement(fields[i].FieldType, out fieldType, out fieldCount))
						return false;

					if (type == VertexElementType.Unknown)
						type = fieldType;
					else if (fieldType != type)
						return false;

					count += fieldCount;
				}

				return true;
			}
			
			type = VertexElementType.Unknown;
			count = 0;
			return false;
		}
	}
}