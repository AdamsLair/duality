using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Describes memory layout and semantics of a vertex type struct.
	/// </summary>
	[DontSerialize]
	public class VertexDeclaration
	{
		private static class Cache<T> where T : struct, IVertexData
		{
			public static readonly VertexDeclaration Instance = new VertexDeclaration(typeof(T), vertexTypeCounter++);
		}

		private static int vertexTypeCounter = 0;
		private static List<VertexDeclaration> delcarationByIndex = new List<VertexDeclaration>();
		private static MethodInfo genericGetDeclarationMethod = null;

		/// <summary>
		/// A prefix that is added to a vertex element name in order to derive its
		/// corresponding shader field name.
		/// </summary>
		public static readonly string ShaderFieldPrefix = "vertex";
		
		/// <summary>
		/// Retrieves the <see cref="VertexDeclaration"/> for the vertex type specified
		/// via generic parameter. This is a very efficient compile-time lookup.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static VertexDeclaration Get<T>() where T : struct, IVertexData
		{
			return Cache<T>.Instance;
		}
		/// <summary>
		/// Retrieves the <see cref="VertexDeclaration"/> for the specified type index.
		/// Returns null if the type index is not valid.
		/// </summary>
		public static VertexDeclaration Get(int typeIndex)
		{
			if (typeIndex < 0 || typeIndex >= delcarationByIndex.Count)
				return null;
			else
				return delcarationByIndex[typeIndex];
		}
		/// <summary>
		/// Retrieves the <see cref="VertexDeclaration"/> for the vertex type specified
		/// via reflection <see cref="Type"/> parameter. Slow and reflection-based. When
		/// possible, use the generic <see cref="Get{T}()"/> method instead.
		/// </summary>
		/// <param name="vertexType"></param>
		public static VertexDeclaration Get(Type vertexType)
		{
			if (vertexType == null) return null;

			TypeInfo vertexTypeInfo = vertexType.GetTypeInfo();
			if (!typeof(IVertexData).GetTypeInfo().IsAssignableFrom(vertexTypeInfo)) return null;

			if (genericGetDeclarationMethod == null)
			{
				TypeInfo declarationTypeInfo = typeof(VertexDeclaration).GetTypeInfo();
				foreach (MethodInfo method in declarationTypeInfo.DeclaredMethods)
				{
					if (!method.IsStatic) continue;
					if (!method.IsGenericMethodDefinition) continue;
					if (method.Name != "Get") continue;

					genericGetDeclarationMethod = method;
					break;
				}
			}

			MethodInfo specializedGet = genericGetDeclarationMethod.MakeGenericMethod(vertexType);
			VertexDeclaration declaration = specializedGet.Invoke(null, null) as VertexDeclaration;
			return declaration;
		}


		private Type dataType;
		private int typeIndex;
		private int size;
		private VertexElement[] elements;

		/// <summary>
		/// [GET] The vertex type that is described by this <see cref="VertexDeclaration"/>.
		/// </summary>
		public Type DataType
		{
			get { return this.dataType; }
		}
		/// <summary>
		/// [GET] A unique, but non-persistent type index that represents the described vertex type.
		/// Type indices start at zero and increase to a maximum of the number of different known
		/// vertex types.
		/// </summary>
		public int TypeIndex
		{
			get { return this.typeIndex; }
		}
		/// <summary>
		/// [GET] Size of a single vertex in bytes.
		/// </summary>
		public int Size
		{
			get { return this.size; }
		}
		/// <summary>
		/// [GET] Descriptions of the vertices individual elements.
		/// </summary>
		public VertexElement[] Elements
		{
			get { return this.elements; }
		}

		private VertexDeclaration(Type dataType, int typeIndex)
		{
			TypeInfo dataTypeInfo = dataType.GetTypeInfo();
			if (dataTypeInfo.IsClass) throw new InvalidOperationException("Vertex formats need to be structs. Classes are not supported.");

			FieldInfo[] fields = dataTypeInfo.DeclaredFieldsDeep().Where(m => !m.IsStatic).ToArray();

			this.dataType = dataType;
			this.typeIndex = typeIndex;
			this.size = Marshal.SizeOf(dataType);
			this.elements = new VertexElement[fields.Length];

			for (int i = 0; i < fields.Length; i++)
			{
				string name = null;
				VertexElementType type = VertexElementType.Unknown;
				int count = 0;

				VertexElementAttribute attrib = fields[i].GetAttributesCached<VertexElementAttribute>().FirstOrDefault();
				if (attrib != null)
				{
					name = attrib.Name;
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

				// When not specified, use the structs field name as a vertex element name
				if (name == null)
				{
					name = fields[i].Name;
				}

				this.elements[i] = new VertexElement(
					ShaderFieldPrefix + name,
					Marshal.OffsetOf(dataType, fields[i].Name), 
					type, 
					count);
			}

			// Add this declaration to the static by-TypeIndex lookup
			while (delcarationByIndex.Count <= this.typeIndex)
				delcarationByIndex.Add(null);
			delcarationByIndex[this.typeIndex] = this;
		}

		public override string ToString()
		{
			return string.Format("{0}", this.dataType.GetTypeCSCodeName(true));
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
			
			TypeInfo dataTypeInfo = dataType.GetTypeInfo();
			if (!dataTypeInfo.IsClass && !dataTypeInfo.IsEnum && !dataTypeInfo.IsPrimitive)
			{
				type = VertexElementType.Unknown;
				count = 0;

				FieldInfo[] fields = dataTypeInfo.DeclaredFieldsDeep().Where(m => !m.IsStatic).ToArray();
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