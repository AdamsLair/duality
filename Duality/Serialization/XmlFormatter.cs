using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
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

		protected override void WriteObjectBody(XElement element, object obj, ObjectHeader header)
		{
			if (header.IsPrimitive)							this.WritePrimitive		(element, obj);
			else if (header.DataType == DataType.Enum)		this.WriteEnum			(element, obj as Enum, header);
			else if (header.DataType == DataType.String)	element.Value = obj as string;
			else if (header.DataType == DataType.Struct)	this.WriteStruct		(element, obj, header);
			else if (header.DataType == DataType.ObjectRef)	element.Value = XmlConvert.ToString(header.ObjectId);
			else if	(header.DataType == DataType.Array)		this.WriteArray			(element, obj, header);
			else if (header.DataType == DataType.Class)		this.WriteStruct		(element, obj, header);
			else if (header.DataType == DataType.Delegate)	this.WriteDelegate		(element, obj, header);
			else if (header.DataType.IsMemberInfoType())	this.WriteMemberInfo	(element, obj, header);
		}
		/// <summary>
		/// Writes the specified <see cref="System.Reflection.MemberInfo"/>, including references objects.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="obj">The object to write.</param>
		/// <param name="id">The objects id.</param>
		protected void WriteMemberInfo(XElement element, object obj, ObjectHeader header)
		{
			if (obj is Type)
			{
				Type type = obj as Type;
				SerializeType cachedType = type.GetSerializeType();
				element.SetAttributeValue("value", cachedType.TypeString);
			}
			else if (obj is MemberInfo)
			{
				MemberInfo member = obj as MemberInfo;
				element.SetAttributeValue("value", member.GetMemberId());
			}
			else if (obj == null)
				throw new ArgumentNullException("obj");
			else
				throw new ArgumentException(string.Format("Type '{0}' is not a supported MemberInfo.", obj.GetType()));
		}
		/// <summary>
		/// Writes the specified <see cref="System.Array"/>, including references objects.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="obj">The object to write.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> describing the object.</param>
		/// <param name="id">The objects id.</param>
		protected void WriteArray(XElement element, object obj, ObjectHeader header)
		{
			Array objAsArray = obj as Array;

			if (objAsArray.Rank != 1) throw new ArgumentException("Non single-Rank arrays are not supported");
			if (objAsArray.GetLowerBound(0) != 0) throw new ArgumentException("Non zero-based arrays are not supported");

			if (objAsArray is byte[])
			{
				byte[] byteArr = objAsArray as byte[];
				element.Value = EncodeByteArray(byteArr);
			}
			else
			{
				// Write Array elements
				int nonDefaultElementCount = this.GetArrayNonDefaultElementCount(objAsArray, header.ObjectType.GetElementType());
				for (int i = 0; i < nonDefaultElementCount; i++)
				{
					XElement itemElement = new XElement("item");
					element.Add(itemElement);

					this.WriteObjectData(itemElement, objAsArray.GetValue(i));
				}

				// Write original length, in case trailing elements were omitted.
				if (nonDefaultElementCount != objAsArray.Length)
				{
					element.SetAttributeValue("length", XmlConvert.ToString(objAsArray.Length));
				}
			}
		}
		/// <summary>
		/// Writes the specified structural object, including references objects.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="obj">The object to write.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> describing the object.</param>
		/// <param name="id">The objects id.</param>
		protected void WriteStruct(XElement element, object obj, ObjectHeader header)
		{
			ISerializeExplicit objAsCustom = obj as ISerializeExplicit;
			ISerializeSurrogate objSurrogate = GetSurrogateFor(header.ObjectType);

			// Write the structs data type
			if (objAsCustom != null)	element.SetAttributeValue("custom", XmlConvert.ToString(true));
			if (objSurrogate != null)	element.SetAttributeValue("surrogate", XmlConvert.ToString(true));

			if (objSurrogate != null)
			{
				objSurrogate.RealObject = obj;
				objAsCustom = objSurrogate.SurrogateObject;

				CustomSerialIO customIO = new CustomSerialIO();
				try { objSurrogate.WriteConstructorData(customIO); }
				catch (Exception e) { this.LogCustomSerializationError(header.ObjectId, header.ObjectType, e); }

				XElement customHeaderElement = new XElement(CustomSerialIO.HeaderElement);
				element.Add(customHeaderElement);
				customIO.Serialize(this, customHeaderElement);
			}

			if (objAsCustom != null)
			{
				CustomSerialIO customIO = new CustomSerialIO();
				try { objAsCustom.WriteData(customIO); }
				catch (Exception e) { this.LogCustomSerializationError(header.ObjectId, header.ObjectType, e); }
				
				XElement customBodyElement = new XElement(CustomSerialIO.BodyElement);
				element.Add(customBodyElement);
				customIO.Serialize(this, customBodyElement);
			}
			else
			{
				// Write the structs fields
				foreach (FieldInfo field in header.SerializeType.Fields)
				{
					if (this.IsFieldBlocked(field, obj)) continue;

					XElement fieldElement = new XElement(GetXmlElementName(field.Name));
					element.Add(fieldElement);

					this.WriteObjectData(fieldElement, field.GetValue(obj));
				}
			}
		}
		/// <summary>
		/// Writes the specified <see cref="System.Delegate"/>, including references objects.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="obj">The object to write.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> describing the object.</param>
		/// <param name="id">The objects id.</param>
		protected void WriteDelegate(XElement element, object obj, ObjectHeader header)
		{
			bool multi = obj is MulticastDelegate;

			// Write the delegates type
			if (multi) element.SetAttributeValue("multi", XmlConvert.ToString(multi));

			if (!multi)
			{
				XElement methodElement = new XElement("method");
				XElement targetElement = new XElement("target");
				element.Add(methodElement);
				element.Add(targetElement);

				Delegate objAsDelegate = obj as Delegate;
				this.WriteObjectData(methodElement, objAsDelegate.Method);
				this.WriteObjectData(targetElement, objAsDelegate.Target);
			}
			else
			{
				XElement methodElement = new XElement("method");
				XElement targetElement = new XElement("target");
				XElement invocationListElement = new XElement("invocationList");
				element.Add(methodElement);
				element.Add(targetElement);
				element.Add(invocationListElement);

				MulticastDelegate objAsDelegate = obj as MulticastDelegate;
				Delegate[] invokeList = objAsDelegate.GetInvocationList();
				this.WriteObjectData(methodElement, objAsDelegate.Method);
				this.WriteObjectData(targetElement, objAsDelegate.Target);
				this.WriteObjectData(invocationListElement, invokeList);
			}
		}
		/// <summary>
		/// Writes the specified <see cref="System.Enum"/>.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="obj">The object to write.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> describing the object.</param>
		protected void WriteEnum(XElement element, Enum obj, ObjectHeader header)
		{
			element.SetAttributeValue("name", obj.ToString());
			element.SetAttributeValue("value", XmlConvert.ToString(Convert.ToInt64(obj)));
		}

		protected override object ReadObjectBody(XElement element, ObjectHeader header)
		{
			object result = null;

			if (header.IsPrimitive)							result = this.ReadPrimitive(element, header);
			else if (header.DataType == DataType.String)	result = element.Value;
			else if (header.DataType == DataType.Enum)		result = this.ReadEnum(element, header);
			else if (header.DataType == DataType.Struct)	result = this.ReadStruct(element, header);
			else if (header.DataType == DataType.ObjectRef)	result = this.ReadObjectRef(element);
			else if (header.DataType == DataType.Array)		result = this.ReadArray(element, header);
			else if (header.DataType == DataType.Class)		result = this.ReadStruct(element, header);
			else if (header.DataType == DataType.Delegate)	result = this.ReadDelegate(element, header);
			else if (header.DataType.IsMemberInfoType())	result = this.ReadMemberInfo(element, header);

			return result;
		}
		/// <summary>
		/// Reads an <see cref="System.Array"/>, including referenced objects.
		/// </summary>
		/// <param name="element"></param>
		/// <returns>The object that has been read.</returns>
		protected Array ReadArray(XElement element, ObjectHeader header)
		{
			string arrLengthString = element.GetAttributeValue("length");
			int arrLength = arrLengthString == null	? element.Elements().Count() : XmlConvert.ToInt32(arrLengthString);

			Array arrObj;
			if (header.ObjectType == typeof(byte[]))
			{
				string binHexString = element.Value;
				byte[] byteArr = DecodeByteArray(binHexString);

				// Set object reference
				this.idManager.Inject(byteArr, header.ObjectId);
				arrObj = byteArr;
			}
			else
			{
				// Prepare object reference
				arrObj = Array.CreateInstance(header.ObjectType.GetElementType(), arrLength);
				this.idManager.Inject(arrObj, header.ObjectId);

				int itemIndex = 0;
				foreach (XElement itemElement in element.Elements())
				{
					object item = this.ReadObjectData(itemElement);
					if (arrObj != null) arrObj.SetValue(item, itemIndex);

					itemIndex++;
					if (itemIndex >= arrLength) break;
				}
			}

			return arrObj;
		}
		/// <summary>
		/// Reads a structural object, including referenced objects.
		/// </summary>
		/// <param name="element"></param>
		/// <returns>The object that has been read.</returns>
		protected object ReadStruct(XElement element, ObjectHeader header)
		{
			// Read struct type
			string	customString	= element.GetAttributeValue("custom");
			string	surrogateString	= element.GetAttributeValue("surrogate");
			bool	custom			= customString != null && XmlConvert.ToBoolean(customString);
			bool	surrogate		= surrogateString != null && XmlConvert.ToBoolean(surrogateString);

			// Retrieve surrogate if requested
			ISerializeSurrogate objSurrogate = null;
			if (surrogate && header.ObjectType != null) objSurrogate = GetSurrogateFor(header.ObjectType);

			// Construct object
			object obj = null;
			if (header.ObjectType != null)
			{
				if (objSurrogate != null)
				{
					custom = true;

					// Set fake object reference for surrogate constructor: No self-references allowed here.
					this.idManager.Inject(null, header.ObjectId);

					CustomSerialIO customIO = new CustomSerialIO();
					XElement customHeaderElement = element.Element(CustomSerialIO.HeaderElement) ?? element.Elements().FirstOrDefault();
					if (customHeaderElement != null)
					{
						customIO.Deserialize(this, customHeaderElement);
					}
					try { obj = objSurrogate.ConstructObject(customIO, header.ObjectType); }
					catch (Exception e) { this.LogCustomDeserializationError(header.ObjectId, header.ObjectType, e); }
				}
				if (obj == null) obj = header.ObjectType.CreateInstanceOf();
				if (obj == null) obj = header.ObjectType.CreateInstanceOf(true);
			}

			// Prepare object reference
			this.idManager.Inject(obj, header.ObjectId);

			// Read custom object data
			if (custom)
			{
				CustomSerialIO customIO = new CustomSerialIO();
				XElement customBodyElement = element.Element(CustomSerialIO.BodyElement) ?? element.Elements().ElementAtOrDefault(1);
				if (customBodyElement != null)
				{
					customIO.Deserialize(this, customBodyElement);
				}

				ISerializeExplicit objAsCustom;
				if (objSurrogate != null)
				{
					objSurrogate.RealObject = obj;
					objAsCustom = objSurrogate.SurrogateObject;
				}
				else
					objAsCustom = obj as ISerializeExplicit;

				if (objAsCustom != null)
				{
					try { objAsCustom.ReadData(customIO); }
					catch (Exception e) { this.LogCustomDeserializationError(header.ObjectId, header.ObjectType, e); }
				}
				else if (obj != null && header.ObjectType != null)
				{
					this.SerializationLog.WriteWarning(
						"Object data (Id {0}) is flagged for custom deserialization, yet the objects Type ('{1}') does not support it. Guessing associated fields...",
						header.ObjectId,
						Log.Type(header.ObjectType));
					this.SerializationLog.PushIndent();
					foreach (var pair in customIO.Data)
					{
						this.AssignValueToField(header.SerializeType, obj, pair.Key, pair.Value);
					}
					this.SerializationLog.PopIndent();
				}
			}
			// Red non-custom object data
			else if (!element.IsEmpty)
			{
				// Read fields
				object fieldValue;
				foreach (XElement fieldElement in element.Elements())
				{
					fieldValue = this.ReadObjectData(fieldElement);
					this.AssignValueToField(header.SerializeType, obj, GetCodeElementName(fieldElement.Name.LocalName), fieldValue);
				}
			}

			return obj;
		}
		/// <summary>
		/// Reads an object reference.
		/// </summary>
		/// <param name="element"></param>
		/// <returns>The object that has been read.</returns>
		protected object ReadObjectRef(XElement element)
		{
			object obj;
			uint objId = XmlConvert.ToUInt32(element.Value);

			if (!this.idManager.Lookup(objId, out obj)) throw new ApplicationException(string.Format("Can't resolve object reference '{0}'.", objId));

			return obj;
		}
		/// <summary>
		/// Reads a <see cref="System.Reflection.MemberInfo"/>, including referenced objects.
		/// </summary>
		/// <param name="dataType">The <see cref="Duality.Serialization.DataType"/> of the object to read.</param>
		/// <param name="element"></param>
		/// <returns>The object that has been read.</returns>
		protected MemberInfo ReadMemberInfo(XElement element, ObjectHeader header)
		{
			MemberInfo result;
			try
			{
				if (header.DataType == DataType.Type)
				{
					string typeString = element.GetAttributeValue("value");
					result = this.ResolveType(typeString, header.ObjectId);
				}
				else
				{
					string memberString = element.GetAttributeValue("value");
					result = this.ResolveMember(memberString, header.ObjectId);
				}
			}
			catch (Exception e)
			{
				result = null;
				this.SerializationLog.WriteError(
					"An error occurred in deserializing MemberInfo object Id {0} of type '{1}': {2}",
					header.ObjectId,
					Log.Type(header.ObjectType),
					Log.Exception(e));
			}

			// Prepare object reference
			this.idManager.Inject(result, header.ObjectId);

			return result;
		}
		/// <summary>
		/// Reads a <see cref="System.Delegate"/>, including referenced objects.
		/// </summary>
		/// <param name="element"></param>
		/// <returns>The object that has been read.</returns>
		protected Delegate ReadDelegate(XElement element, ObjectHeader header)
		{
			string multiString = element.GetAttributeValue("multi");
			bool multi = multiString != null && XmlConvert.ToBoolean(multiString);

			XElement methodElement = element.Element("method") ?? element.Elements().FirstOrDefault();
			XElement targetElement = element.Element("target") ?? element.Elements().ElementAtOrDefault(1);
			XElement invocationListElement = element.Element("invocationList") ?? element.Elements().ElementAtOrDefault(2);

			// Create the delegate without target and fix it later, so we can register its object id before loading its target object
			MethodInfo	method	= this.ReadObjectData(methodElement) as MethodInfo;
			object		target	= null;
			Delegate	del		= header.ObjectType != null && method != null ? Delegate.CreateDelegate(header.ObjectType, target, method) : null;

			// Prepare object reference
			this.idManager.Inject(del, header.ObjectId);

			// Read the target object now and replace the dummy
			target = this.ReadObjectData(targetElement);
			if (del != null && target != null)
			{
				FieldInfo targetField = header.ObjectType.GetField("_target", ReflectionHelper.BindInstanceAll);
				targetField.SetValue(del, target);
			}

			// Combine multicast delegates
			if (multi)
			{
				Delegate[] invokeList = (this.ReadObjectData(invocationListElement) as Delegate[]).NotNull().ToArray();
				del = invokeList.Length > 0 ? Delegate.Combine(invokeList) : null;
			}

			return del;
		}
		/// <summary>
		/// Reads an <see cref="System.Enum"/>.
		/// </summary>
		/// <param name="element"></param>
		/// <returns>The object that has been read.</returns>
		protected Enum ReadEnum(XElement element, ObjectHeader header)
		{
			string name = element.GetAttributeValue("name");
			string valueString = element.GetAttributeValue("value");
			long val = valueString == null ? 0 : XmlConvert.ToInt64(valueString);
			return (header.ObjectType == null) ? null : this.ResolveEnumValue(header.ObjectType, name, val);
		}
	}
}
