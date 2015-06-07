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
		private	TypeInfo	type;
		private	FieldInfo[]	fields;
		private	string		typeString;
		private	DataType	dataType;
		private bool		dontSerialize;

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
		/// Creates a new SerializeType based on a <see cref="System.Type"/>, gathering all the information that is necessary for serialization.
		/// </summary>
		/// <param name="t"></param>
		public SerializeType(Type t)
		{
			this.type = t.GetTypeInfo();
			this.dataType = GetDataType(this.type);
			this.typeString = this.type.GetTypeId();
			this.dontSerialize = this.type.HasAttributeCached<DontSerializeAttribute>();

			if (this.dataType == DataType.Struct)
			{
				// Retrieve all fields that are not flagged not to be serialized
				IEnumerable<FieldInfo> filteredFields = this.type
					.DeclaredFieldsDeep()
					.Where(f => !f.IsStatic && !f.HasAttributeCached<DontSerializeAttribute>());

				// Ugly hack to skip .Net collection _syncRoot fields. 
				// Can't use field.IsNonSerialized, because that doesn't exist in the PCL profile,
				// and implementing a whole filtering system just for this would be overkill.
				filteredFields = filteredFields
					.Where(f => !(
						f.FieldType == typeof(object) && 
						f.Name == "_syncRoot" && 
						typeof(System.Collections.ICollection).IsAssignableFrom(f.DeclaringType)));

				// Store the filtered fields in a fixed form
				this.fields = filteredFields.ToArray();
				this.fields.StableSort((a, b) => string.Compare(a.Name, b.Name));
			}
			else
			{
				this.fields = new FieldInfo[0];
			}
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
			else if (typeof(Type).IsAssignableFrom(t))
				return DataType.Type;
			else if (typeof(MemberInfo).IsAssignableFrom(t))
				return DataType.MemberInfo;
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
