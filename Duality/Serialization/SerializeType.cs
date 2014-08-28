using System;
using System.Linq;
using System.Reflection;

namespace Duality.Serialization
{
	/// <summary>
	/// The SerializeType class is essentially caching serialization-relevant information
	/// that has been generated basing on a <see cref="System.Type"/>. It is cached in the
	/// <see cref="ReflectionHelper"/> to avoid redundant information gathering.
	/// </summary>
	public sealed class SerializeType
	{
		private	Type		type;
		private	FieldInfo[]	fields;
		private	string		typeString;
		private	DataType	dataType;

		/// <summary>
		/// [GET] The <see cref="System.Type"/> that is described.
		/// </summary>
		public Type Type
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
		/// Creates a new SerializeType based on a <see cref="System.Type"/>, gathering all the information that is necessary for serialization.
		/// </summary>
		/// <param name="t"></param>
		public SerializeType(Type t)
		{
			this.type = t;
			this.fields = this.type.GetAllFields(ReflectionHelper.BindInstanceAll).Where(f => !f.IsNotSerialized).ToArray();
			this.typeString = this.type.GetTypeId();
			this.dataType = GetDataType(this.type);

			this.fields.StableSort((a, b) => string.Compare(a.Name, b.Name));
		}

		private static DataType GetDataType(Type t)
		{
			if (t.IsEnum)
				return DataType.Enum;
			else if (t.IsPrimitive)
			{
				if		(t == typeof(bool))		return DataType.Bool;
				else if (t == typeof(byte))		return DataType.Byte;
				else if (t == typeof(char))		return DataType.Char;
				else if (t == typeof(sbyte))	return DataType.SByte;
				else if (t == typeof(short))	return DataType.Short;
				else if (t == typeof(ushort))	return DataType.UShort;
				else if (t == typeof(int))		return DataType.Int;
				else if (t == typeof(uint))		return DataType.UInt;
				else if (t == typeof(long))		return DataType.Long;
				else if (t == typeof(ulong))	return DataType.ULong;
				else if (t == typeof(float))	return DataType.Float;
				else if (t == typeof(double))	return DataType.Double;
				else if (t == typeof(decimal))	return DataType.Decimal;
			}
			else if (typeof(MemberInfo).IsAssignableFrom(t))
			{
				if		(typeof(Type).IsAssignableFrom(t))				return DataType.Type;
				else if (typeof(FieldInfo).IsAssignableFrom(t))			return DataType.FieldInfo;
				else if (typeof(PropertyInfo).IsAssignableFrom(t))		return DataType.PropertyInfo;
				else if (typeof(MethodInfo).IsAssignableFrom(t))		return DataType.MethodInfo;
				else if (typeof(ConstructorInfo).IsAssignableFrom(t))	return DataType.ConstructorInfo;
				else if (typeof(EventInfo).IsAssignableFrom(t))			return DataType.EventInfo;
			}
			else if (typeof(Delegate).IsAssignableFrom(t))
				return DataType.Delegate;
			else if (t == typeof(string))
				return DataType.String;
			else if (t.IsArray)
				return DataType.Array;
			else if (t.IsClass)
				return DataType.Struct;
			else if (t.IsValueType)
				return DataType.Struct;

			// Should never happen in theory
			return DataType.Unknown;
		}
	}
}
