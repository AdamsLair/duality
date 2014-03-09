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
			this.dataType = this.type.GetDataType();

			this.fields.StableSort((a, b) => string.Compare(a.Name, b.Name));
		}
	}
}
