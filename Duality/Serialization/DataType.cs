using System;
using System.Reflection;

namespace Duality.Serialization
{
	/// <summary>
	/// This enum is used by Dualitys serializers to distinguish between certain kinds of data.
	/// </summary>
	public enum DataType : ushort
	{
		/// <summary>
		/// Unknown data type
		/// </summary>
		Unknown,

		/// <summary>
		/// A <see cref="System.Boolean"/> value
		/// </summary>
		Bool,
		/// <summary>
		/// A <see cref="System.Byte"/> value
		/// </summary>
		Byte,
		/// <summary>
		/// A <see cref="System.SByte"/> value
		/// </summary>
		SByte,
		/// <summary>
		/// A <see cref="System.Int16"/> value
		/// </summary>
		Short,
		/// <summary>
		/// A <see cref="System.UInt16"/> value
		/// </summary>
		UShort,
		/// <summary>
		/// A <see cref="System.Int32"/> value
		/// </summary>
		Int,
		/// <summary>
		/// A <see cref="System.UInt32"/> value
		/// </summary>
		UInt,
		/// <summary>
		/// A <see cref="System.Int64"/> value
		/// </summary>
		Long,
		/// <summary>
		/// A <see cref="System.UInt64"/> value
		/// </summary>
		ULong,
		/// <summary>
		/// A <see cref="System.Single"/> value
		/// </summary>
		Float,
		/// <summary>
		/// A <see cref="System.Double"/> value
		/// </summary>
		Double,
		/// <summary>
		/// A <see cref="System.Decimal"/> value
		/// </summary>
		Decimal,
		/// <summary>
		/// A <see cref="System.Char"/> value
		/// </summary>
		Char,

		/// <summary>
		/// A <see cref="System.Type"/> value
		/// </summary>
		Type,
		/// <summary>
		/// A <see cref="System.Reflection.FieldInfo"/> value
		/// </summary>
		FieldInfo,
		/// <summary>
		/// A <see cref="System.Reflection.PropertyInfo"/> value
		/// </summary>
		PropertyInfo,
		/// <summary>
		/// A <see cref="System.Reflection.MethodInfo"/> value
		/// </summary>
		MethodInfo,
		/// <summary>
		/// A <see cref="System.Reflection.ConstructorInfo"/> value
		/// </summary>
		ConstructorInfo,
		/// <summary>
		/// A <see cref="System.Reflection.EventInfo"/> value
		/// </summary>
		EventInfo,

		/// <summary>
		/// A <see cref="System.Delegate"/> value
		/// </summary>
		Delegate,

		/// <summary>
		/// A <see cref="System.Enum"/> value
		/// </summary>
		Enum,
		/// <summary>
		/// A <see cref="System.String"/> value
		/// </summary>
		String,
		/// <summary>
		/// A <see cref="System.Array"/> value
		/// </summary>
		Array,
		/// <summary>
		/// A class object
		/// </summary>
		Class,
		/// <summary>
		/// A struct object
		/// </summary>
		Struct,

		/// <summary>
		/// The reference to an object
		/// </summary>
		ObjectRef
	}

	/// <summary>
	/// Extension methods for <see cref="Duality.Serialization.DataType"/>
	/// </summary>
	public static class ExtMethodsDataType
	{
		/// <summary>
		/// Returns whether the <see cref="Duality.Serialization.DataType"/> represents a primitive data type.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static bool IsPrimitiveType(this DataType dt)
		{
			return (ushort)dt >= (ushort)DataType.Bool && (ushort)dt <= (ushort)DataType.Char;
		}
		/// <summary>
		/// Returns whether the <see cref="Duality.Serialization.DataType"/> represents a <see cref="System.Reflection.MemberInfo"/> type.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static bool IsMemberInfoType(this DataType dt)
		{
			return (ushort)dt >= (ushort)DataType.Type && (ushort)dt <= (ushort)DataType.EventInfo;
		}
		/// <summary>
		/// Returns the actual <see cref="System.Type"/> that is associated with the <see cref="Duality.Serialization.DataType"/>.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static Type ToActualType(this DataType dt)
		{
			switch (dt)
			{
				case DataType.Array:			return typeof(Array);
				case DataType.Bool:				return typeof(bool);
				case DataType.Byte:				return typeof(byte);
				case DataType.Char:				return typeof(char);
				case DataType.Class:			return typeof(object);
				case DataType.ConstructorInfo:	return typeof(ConstructorInfo);
				case DataType.Decimal:			return typeof(decimal);
				case DataType.Delegate:			return typeof(Delegate);
				case DataType.Double:			return typeof(double);
				case DataType.EventInfo:		return typeof(EventInfo);
				case DataType.FieldInfo:		return typeof(FieldInfo);
				case DataType.Float:			return typeof(float);
				case DataType.Int:				return typeof(int);
				case DataType.Long:				return typeof(long);
				case DataType.MethodInfo:		return typeof(MethodInfo);
				case DataType.ObjectRef:		return typeof(object);
				case DataType.PropertyInfo:		return typeof(PropertyInfo);
				case DataType.SByte:			return typeof(sbyte);
				case DataType.Short:			return typeof(short);
				case DataType.String:			return typeof(string);
				case DataType.Struct:			return typeof(object);
				case DataType.Type:				return typeof(Type);
				case DataType.UInt:				return typeof(uint);
				case DataType.ULong:			return typeof(ulong);
				case DataType.Unknown:			return null;
				case DataType.UShort:			return typeof(ushort);
				default:						return null;
			}
		}
	}
}
