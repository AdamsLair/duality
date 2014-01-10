using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.IO;
using System.Reflection;

namespace Duality.Serialization
{
	/// <summary>
	/// De/Serializes object data.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.XmlMetaFormatter"/>
	public class XmlFormatter : XmlFormatterBase
	{
		public XmlFormatter(Stream stream) : base(stream) {}

		protected override void WriteObjectBody(DataType dataType, object obj, SerializeType objSerializeType, uint objId)
		{
			if (dataType.IsPrimitiveType())				this.WritePrimitive(obj);
			else if (dataType == DataType.Enum)			this.WriteEnum(obj as Enum, objSerializeType);
			else if (dataType == DataType.String)		this.writer.WriteString(obj as string);
			else if (dataType == DataType.Struct)		this.WriteStruct(obj, objSerializeType);
			else if (dataType == DataType.ObjectRef)	this.writer.WriteValue(objId);
			else if	(dataType == DataType.Array)		this.WriteArray(obj, objSerializeType, objId);
			else if (dataType == DataType.Class)		this.WriteStruct(obj, objSerializeType, objId);
			else if (dataType == DataType.Delegate)		this.WriteDelegate(obj, objSerializeType, objId);
			else if (dataType.IsMemberInfoType())		this.WriteMemberInfo(obj, objId);
		}
		/// <summary>
		/// Writes the specified <see cref="System.Reflection.MemberInfo"/>, including references objects.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		/// <param name="id">The objects id.</param>
		protected void WriteMemberInfo(object obj, uint id = 0)
		{
			if (id != 0) this.writer.WriteAttributeString("id", XmlConvert.ToString(id));

			if (obj is Type)
			{
				Type type = obj as Type;
				SerializeType cachedType = type.GetSerializeType();
				this.writer.WriteAttributeString("value", cachedType.TypeString);
			}
			else if (obj is MemberInfo)
			{
				MemberInfo member = obj as MemberInfo;
				this.writer.WriteAttributeString("value", member.GetMemberId());
			}
			else if (obj == null)
				throw new ArgumentNullException("obj");
			else
				throw new ArgumentException(string.Format("Type '{0}' is not a supported MemberInfo.", obj.GetType()));
		}
		/// <summary>
		/// Writes the specified <see cref="System.Array"/>, including references objects.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> describing the object.</param>
		/// <param name="id">The objects id.</param>
		protected void WriteArray(object obj, SerializeType objSerializeType, uint id = 0)
		{
			Array objAsArray = obj as Array;

			if (objAsArray.Rank != 1) throw new ArgumentException("Non single-Rank arrays are not supported");
			if (objAsArray.GetLowerBound(0) != 0) throw new ArgumentException("Non zero-based arrays are not supported");

			this.writer.WriteAttributeString("type", objSerializeType.TypeString);
			if (id != 0) this.writer.WriteAttributeString("id", XmlConvert.ToString(id));
			if (objAsArray.Rank != 1) this.writer.WriteAttributeString("rank", XmlConvert.ToString(objAsArray.Rank));
			this.writer.WriteAttributeString("length", XmlConvert.ToString(objAsArray.Length));

			if (objAsArray is byte[])
			{
				byte[] byteArr = objAsArray as byte[];
				this.writer.WriteString(this.ByteArrayToString(byteArr));
				//for (int l = 0; l < byteArr.Length; l++)
				//	this.writer.WriteString(byteArr[l].ToString("X2"));
			}
			else
			{
				for (long l = 0; l < objAsArray.Length; l++)
					this.WriteObject(objAsArray.GetValue(l));
			}
		}
		/// <summary>
		/// Writes the specified structural object, including references objects.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> describing the object.</param>
		/// <param name="id">The objects id.</param>
		protected void WriteStruct(object obj, SerializeType objSerializeType, uint id = 0)
		{
			ISerializable objAsCustom = obj as ISerializable;
			ISurrogate objSurrogate = this.GetSurrogateFor(objSerializeType.Type);

			// Write the structs data type
			this.writer.WriteAttributeString("type", objSerializeType.TypeString);
			if (id != 0) this.writer.WriteAttributeString("id", XmlConvert.ToString(id));
			if (objAsCustom != null) this.writer.WriteAttributeString("custom", XmlConvert.ToString(true));
			if (objSurrogate != null) this.writer.WriteAttributeString("surrogate", XmlConvert.ToString(true));

			if (objSurrogate != null)
			{
				objSurrogate.RealObject = obj;
				objAsCustom = objSurrogate.SurrogateObject;

				CustomSerialIO customIO = new CustomSerialIO(CustomSerialIO.HeaderElement);
				try { objSurrogate.WriteConstructorData(customIO); }
				catch (Exception e) { this.LogCustomSerializationError(id, objSerializeType.Type, e); }
				customIO.Serialize(this);
			}

			if (objAsCustom != null)
			{
				CustomSerialIO customIO = new CustomSerialIO(CustomSerialIO.BodyElement);
				try { objAsCustom.WriteData(customIO); }
				catch (Exception e) { this.LogCustomSerializationError(id, objSerializeType.Type, e); }
				customIO.Serialize(this);
			}
			else
			{
				// Write the structs fields
				foreach (FieldInfo field in objSerializeType.Fields)
				{
					if (this.IsFieldBlocked(field, obj)) continue;
					this.WriteObject(field.GetValue(obj), field.Name);
				}
			}
		}
		/// <summary>
		/// Writes the specified <see cref="System.Delegate"/>, including references objects.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> describing the object.</param>
		/// <param name="id">The objects id.</param>
		protected void WriteDelegate(object obj, SerializeType objSerializeType, uint id = 0)
		{
			bool multi = obj is MulticastDelegate;

			// Write the delegates type
			this.writer.WriteAttributeString("type", objSerializeType.TypeString);
			if (id != 0) this.writer.WriteAttributeString("id", XmlConvert.ToString(id));
			if (multi) this.writer.WriteAttributeString("multi", XmlConvert.ToString(multi));

			if (!multi)
			{
				Delegate objAsDelegate = obj as Delegate;
				this.WriteObject(objAsDelegate.Method);
				this.WriteObject(objAsDelegate.Target);
			}
			else
			{
				MulticastDelegate objAsDelegate = obj as MulticastDelegate;
				Delegate[] invokeList = objAsDelegate.GetInvocationList();
				this.WriteObject(objAsDelegate.Method);
				this.WriteObject(objAsDelegate.Target);
				this.WriteObject(invokeList);
			}
		}
		/// <summary>
		/// Writes the specified <see cref="System.Enum"/>.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> describing the object.</param>
		protected void WriteEnum(Enum obj, SerializeType objSerializeType)
		{
			this.writer.WriteAttributeString("type", objSerializeType.TypeString);
			this.writer.WriteAttributeString("name", obj.ToString());
			this.writer.WriteAttributeString("value", XmlConvert.ToString(Convert.ToInt64(obj)));
		}
		
		protected override object ReadObjectBody(DataType dataType)
		{
			object result = null;

			if (dataType.IsPrimitiveType())				result = this.ReadPrimitive(dataType);
			else if (dataType == DataType.String)		result = this.reader.ReadString();
			else if (dataType == DataType.Enum)			result = this.ReadEnum();
			else if (dataType == DataType.Struct)		result = this.ReadStruct();
			else if (dataType == DataType.ObjectRef)	result = this.ReadObjectRef();
			else if (dataType == DataType.Array)		result = this.ReadArray();
			else if (dataType == DataType.Class)		result = this.ReadStruct();
			else if (dataType == DataType.Delegate)		result = this.ReadDelegate();
			else if (dataType.IsMemberInfoType())		result = this.ReadMemberInfo(dataType);

			return result;
		}
		/// <summary>
		/// Reads an <see cref="System.Array"/>, including referenced objects.
		/// </summary>
		/// <returns>The object that has been read.</returns>
		protected Array ReadArray()
		{
			string	arrTypeString	= this.reader.GetAttribute("type");
			string	objIdString		= this.reader.GetAttribute("id");
			string	arrRankString	= this.reader.GetAttribute("rank");
			string	arrLengthString	= this.reader.GetAttribute("length");
			uint	objId			= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			int		arrRank			= arrRankString == null ? 1 : XmlConvert.ToInt32(arrRankString);
			int		arrLength		= arrLengthString == null ? 0 : XmlConvert.ToInt32(arrLengthString);
			Type	arrType			= this.ResolveType(arrTypeString, objId);

			Array arrObj;
			if (arrType == typeof(byte[]))
			{
			    string binHexString = this.reader.ReadString();
			    byte[] byteArr = this.StringToByteArray(binHexString);

			    // Set object reference
			    this.idManager.Inject(byteArr, objId);
			    arrObj = byteArr;
			}
			else
			{
			    // Prepare object reference
			    arrObj = arrType != null ? Array.CreateInstance(arrType.GetElementType(), arrLength) : null;
			    this.idManager.Inject(arrObj, objId);

			    for (int l = 0; l < arrLength; l++)
			    {
			        object elem = this.ReadObject();
			        if (arrObj != null) arrObj.SetValue(elem, l);
			    }
			}

			return arrObj;
		}
		/// <summary>
		/// Reads a structural object, including referenced objects.
		/// </summary>
		/// <returns>The object that has been read.</returns>
		protected object ReadStruct()
		{
			// Read struct type
			string	objTypeString	= this.reader.GetAttribute("type");
			string	objIdString		= this.reader.GetAttribute("id");
			string	customString	= this.reader.GetAttribute("custom");
			string	surrogateString	= this.reader.GetAttribute("surrogate");
			uint	objId			= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			bool	custom			= customString != null && XmlConvert.ToBoolean(customString);
			bool	surrogate		= surrogateString != null && XmlConvert.ToBoolean(surrogateString);
			Type	objType			= this.ResolveType(objTypeString, objId);

			SerializeType objSerializeType = null;
			if (objType != null) objSerializeType = objType.GetSerializeType();
			
			// Retrieve surrogate if requested
			ISurrogate objSurrogate = null;
			if (surrogate && objType != null) objSurrogate = this.GetSurrogateFor(objType);

			// Construct object
			object obj = null;
			if (objType != null)
			{
				if (objSurrogate != null)
				{
					custom = true;

					// Set fake object reference for surrogate constructor: No self-references allowed here.
					this.idManager.Inject(null, objId);

					CustomSerialIO customIO = new CustomSerialIO(CustomSerialIO.HeaderElement);
					customIO.Deserialize(this);
					try { obj = objSurrogate.ConstructObject(customIO, objType); }
					catch (Exception e) { this.LogCustomDeserializationError(objId, objType, e); }
				}
				if (obj == null) obj = objType.CreateInstanceOf();
				if (obj == null) obj = objType.CreateInstanceOf(true);
			}

			// Prepare object reference
			this.idManager.Inject(obj, objId);

			// Read custom object data
			if (custom)
			{
				CustomSerialIO customIO = new CustomSerialIO(CustomSerialIO.BodyElement);
				customIO.Deserialize(this);

				ISerializable objAsCustom;
				if (objSurrogate != null)
				{
					objSurrogate.RealObject = obj;
					objAsCustom = objSurrogate.SurrogateObject;
				}
				else
					objAsCustom = obj as ISerializable;

				if (objAsCustom != null)
				{
					try { objAsCustom.ReadData(customIO); }
					catch (Exception e) { this.LogCustomDeserializationError(objId, objType, e); }
				}
				else if (obj != null && objType != null)
				{
					this.SerializationLog.WriteWarning(
						"Object data (Id {0}) is flagged for custom deserialization, yet the objects Type ('{1}') does not support it. Guessing associated fields...",
						objId,
						Log.Type(objType));
					this.SerializationLog.PushIndent();
					foreach (var pair in customIO.Data)
					{
						this.AssignValueToField(objSerializeType, obj, pair.Key, pair.Value);
					}
					this.SerializationLog.PopIndent();
				}
			}
			// Red non-custom object data
			else if (!this.reader.IsEmptyElement)
			{
				// Read fields
				bool scopeChanged;
				string fieldName;
				object fieldValue;
				while (true)
				{
					fieldValue = this.ReadObject(out fieldName, out scopeChanged);
					if (scopeChanged) break;
					this.AssignValueToField(objSerializeType, obj, fieldName, fieldValue);
				}
			}

			return obj;
		}
		/// <summary>
		/// Reads an object reference.
		/// </summary>
		/// <returns>The object that has been read.</returns>
		protected object ReadObjectRef()
		{
			object obj;
			uint objId = XmlConvert.ToUInt32(this.reader.ReadString());

			if (!this.idManager.Lookup(objId, out obj)) throw new ApplicationException(string.Format("Can't resolve object reference '{0}'.", objId));

			return obj;
		}
		/// <summary>
		/// Reads a <see cref="System.Reflection.MemberInfo"/>, including referenced objects.
		/// </summary>
		/// <param name="dataType">The <see cref="Duality.Serialization.DataType"/> of the object to read.</param>
		/// <returns>The object that has been read.</returns>
		protected MemberInfo ReadMemberInfo(DataType dataType)
		{
			string	objIdString		= this.reader.GetAttribute("id");
			uint	objId			= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			MemberInfo result;

			try
			{
				if (dataType == DataType.Type)
				{
					string typeString = this.reader.GetAttribute("value");
					result = this.ResolveType(typeString, objId);
				}
				else
				{
					string memberString = this.reader.GetAttribute("value");
					result = this.ResolveMember(memberString, objId);
				}
			}
			catch (Exception e)
			{
				result = null;
				this.SerializationLog.WriteError(
					"An error occurred in deserializing MemberInfo object Id {0} of type '{1}': {2}",
					objId,
					Log.Type(dataType.ToActualType()),
					Log.Exception(e));
			}

			// Prepare object reference
			this.idManager.Inject(result, objId);

			return result;
		}
		/// <summary>
		/// Reads a <see cref="System.Delegate"/>, including referenced objects.
		/// </summary>
		/// <returns>The object that has been read.</returns>
		protected Delegate ReadDelegate()
		{
			string	delegateTypeString	= this.reader.GetAttribute("type");
			string	objIdString			= this.reader.GetAttribute("id");
			string	multiString			= this.reader.GetAttribute("multi");
			uint	objId				= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			bool	multi				= objIdString != null && XmlConvert.ToBoolean(multiString);
			Type	delType				= this.ResolveType(delegateTypeString, objId);

			// Create the delegate without target and fix it later, so we can register its object id before loading its target object
			MethodInfo	method	= this.ReadObject() as MethodInfo;
			object		target	= null;
			Delegate	del		= delType != null && method != null ? Delegate.CreateDelegate(delType, target, method) : null;

			// Prepare object reference
			this.idManager.Inject(del, objId);

			// Read the target object now and replace the dummy
			target = this.ReadObject();
			if (del != null && target != null)
			{
				FieldInfo targetField = delType.GetField("_target", ReflectionHelper.BindInstanceAll);
				targetField.SetValue(del, target);
			}

			// Combine multicast delegates
			if (multi)
			{
				Delegate[] invokeList = (this.ReadObject() as Delegate[]).NotNull().ToArray();
				del = invokeList.Length > 0 ? Delegate.Combine(invokeList) : null;
			}

			return del;
		}
		/// <summary>
		/// Reads an <see cref="System.Enum"/>.
		/// </summary>
		/// <returns>The object that has been read.</returns>
		protected Enum ReadEnum()
		{
			string typeName		= this.reader.GetAttribute("type");
			string name			= this.reader.GetAttribute("name");
			string valueString	= this.reader.GetAttribute("value");
			long val = valueString == null ? 0 : XmlConvert.ToInt64(valueString);
			Type enumType = this.ResolveType(typeName);

			return (enumType == null) ? null : this.ResolveEnumValue(enumType, name, val);
		}
	}
}
