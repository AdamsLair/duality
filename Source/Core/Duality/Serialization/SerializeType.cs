using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.Serialization
{
	/// <summary>
	/// The SerializeType class is essentially caching serialization-relevant information
	/// that has been generated basing on a <see cref="System.Type"/>.
	/// </summary>
	[DontSerialize]
	public sealed class SerializeType
	{
		private TypeInfo            type;
		private FieldInfo[]         fields;
		private string              typeString;
		private DataType            dataType;
		private bool                dontSerialize;
		private object              defaultValue;
		private ISerializeSurrogate surrogate;

		/// <summary>
		/// [GET] The <see cref="System.Reflection.TypeInfo"/> that is described.
		/// </summary>
		public TypeInfo Type
		{
			get { return this.type; }
		}
		/// <summary>
		/// [GET] An array of <see cref="System.Reflection.FieldInfo">fields</see> which are serialized.
		/// </summary>
		public FieldInfo[] Fields
		{
			get { return this.fields; }
		}
		/// <summary>
		/// [GET] A string referring to the <see cref="System.Type"/> that is described.
		/// </summary>
		/// <seealso cref="ReflectionHelper.GetTypeId"/>
		public string TypeString
		{
			get { return this.typeString; }
		}
		/// <summary>
		/// [GET] The <see cref="Duality.Serialization.DataType"/> associated with the described <see cref="System.Type"/>.
		/// </summary>
		public DataType DataType
		{
			get { return this.dataType; }
		}
		/// <summary>
		/// [GET] Returns whether objects of this Type are viable for serialization. 
		/// </summary>
		public bool IsSerializable
		{
			get { return !this.dontSerialize; }
		}
		/// <summary>
		/// [GET] Returns whether object of this Type can be referenced by other serialized objects.
		/// </summary>
		public bool CanBeReferenced
		{
			get
			{
				return !this.type.IsValueType && (
					this.dataType == DataType.Array || 
					this.dataType == DataType.Struct || 
					this.dataType == DataType.Delegate || 
					this.dataType.IsMemberInfoType());
			}
		}
		/// <summary>
		/// [GET] Returns the default instance for objects of this type. This is a cached instance
		/// of <see cref="ObjectCreator.GetDefaultOf"/>.
		/// </summary>
		public object DefaultValue
		{
			get { return this.defaultValue; }
		}
		/// <summary>
		/// [GET] When assigned, this property returns the serialization surrogate
		/// for the type it represents.
		/// </summary>
		public ISerializeSurrogate Surrogate
		{
			get { return this.surrogate; }
		}

		/// <summary>
		/// Creates a new SerializeType based on a <see cref="System.Type"/>, gathering all the information that is necessary for serialization.
		/// </summary>
		/// <param name="t"></param>
		public SerializeType(Type t)
		{
			this.type = t.GetTypeInfo();
			this.typeString = t.GetTypeId();
			this.dataType = GetDataType(this.type);
			this.dontSerialize = this.type.HasAttributeCached<DontSerializeAttribute>();
			this.defaultValue = this.type.GetDefaultOf();
			this.surrogate = Serializer.GetSurrogateFor(this.type);

			if (this.dataType == DataType.Struct && this.surrogate == null)
			{
				// Retrieve all fields that are not flagged not to be serialized
				List<FieldInfo> filteredFields = this.type
					.DeclaredFieldsDeep()
					.Where(f => !f.IsStatic && !f.HasAttributeCached<DontSerializeAttribute>())
					.ToList();

				// Hack: Remove some specific fields based on an internal blacklist
				this.RemoveBlacklistedFields(filteredFields);

				// Store the filtered fields in a fixed form
				this.fields = filteredFields.ToArray();
				this.fields.StableSort((a, b) => string.Compare(a.Name, b.Name));
			}
			else
			{
				this.fields = new FieldInfo[0];
			}
		}

		/// <summary>
		/// Retrieves a serialized field of this type by name.
		/// </summary>
		/// <param name="name"></param>
		public FieldInfo GetFieldByName(string name)
		{
			for (int i = 0; i < this.fields.Length; i++)
			{
				if (this.fields[i].Name == name)
					return this.fields[i];
			}
			return null;
		}
		private void RemoveBlacklistedFields(List<FieldInfo> fields)
		{
			TypeInfo collectionBase = typeof(System.Collections.ICollection).GetTypeInfo();
			if (collectionBase.IsAssignableFrom(this.type) && this.type.Namespace.StartsWith("System.Collections."))
			{
				// Ugly hack to skip .Net collection _syncRoot fields. 
				// Can't use field.IsNonSerialized, because that doesn't exist in the PCL profile,
				// and implementing a whole filtering system just for this would be overkill.
				for (int i = fields.Count - 1; i >= 0; i--)
				{
					FieldInfo field = fields[i];
					if (field.Name != "_syncRoot" && field.Name != "m_syncRoot") continue;
					if (field.FieldType != typeof(object)) continue;
					if (!collectionBase.IsAssignableFrom(field.DeclaringType.GetTypeInfo())) continue;
					fields.RemoveAt(i);
				}

				// Another ugly hack to skip the .Net collection _version fields.
				// They're not even flagged as IsNonSerialized, but they don't do
				// any good for persistent serialization.
				for (int i = fields.Count - 1; i >= 0; i--)
				{
					FieldInfo field = fields[i];
					if (field.Name != "_version" && field.Name != "m_version") continue;
					if (field.FieldType != typeof(int)) continue;
					if (!collectionBase.IsAssignableFrom(field.DeclaringType.GetTypeInfo())) continue;
					fields.RemoveAt(i);
				}

				// A proper solution to both of the above would be to define serialization surrogates
				// for the affected types. However, since we can't access the internals of List<T>
				// we'd end up copying and boxing lots of data and likely losing performance.
				//
				// It may be a viable solution for HashSet, Stack and Queue though, as they tend to be used
				// with smaller quantities of items.
			}
		}

		private static DataType GetDataType(TypeInfo typeInfo)
		{
			Type type = typeInfo.AsType();
			if (typeInfo.IsEnum)
				return DataType.Enum;
			else if (typeInfo.IsPrimitive)
			{
				if		(type == typeof(bool))		return DataType.Bool;
				else if (type == typeof(byte))		return DataType.Byte;
				else if (type == typeof(char))		return DataType.Char;
				else if (type == typeof(sbyte))		return DataType.SByte;
				else if (type == typeof(short))		return DataType.Short;
				else if (type == typeof(ushort))	return DataType.UShort;
				else if (type == typeof(int))		return DataType.Int;
				else if (type == typeof(uint))		return DataType.UInt;
				else if (type == typeof(long))		return DataType.Long;
				else if (type == typeof(ulong))		return DataType.ULong;
				else if (type == typeof(float))		return DataType.Float;
				else if (type == typeof(double))	return DataType.Double;
				else if (type == typeof(decimal))	return DataType.Decimal;
			}
			else if (typeof(Type).GetTypeInfo().IsAssignableFrom(typeInfo))
				return DataType.Type;
			else if (typeof(MemberInfo).GetTypeInfo().IsAssignableFrom(typeInfo))
				return DataType.MemberInfo;
			else if (typeof(Delegate).GetTypeInfo().IsAssignableFrom(typeInfo))
				return DataType.Delegate;
			else if (type == typeof(string))
				return DataType.String;
			else if (typeInfo.IsArray)
				return DataType.Array;
			else if (typeInfo.IsClass)
				return DataType.Struct;
			else if (typeInfo.IsValueType)
				return DataType.Struct;

			// Should never happen in theory
			return DataType.Unknown;
		}
	}
}
