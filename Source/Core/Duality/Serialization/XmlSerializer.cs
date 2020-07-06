using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

using Duality.IO;
using Duality.Editor;
using Duality.Properties;

namespace Duality.Serialization
{
	/// <summary>
	/// De/Serializes objects in an XML format. Additional metadata is included in order to allow an extended degree of error tolerance when working under development conditions.
	/// </summary>
	[DontSerialize]
	[EditorHintImage(CoreResNames.ImageXmlSerializer)]
	public class XmlSerializer : Serializer
	{
		private class CustomSerialIO : CustomSerialIOBase<XmlSerializer>
		{
			public const string HeaderElement = "header";
			public const string BodyElement = "body";

			public void Serialize(XmlSerializer formatter, XElement element)
			{
				foreach (var pair in this.data)
				{
					XElement fieldElement = new XElement(GetXmlElementName(pair.Key));
					element.Add(fieldElement);
					formatter.WriteObjectData(fieldElement, pair.Value);
				}
				this.Clear();
			}
			public void Deserialize(XmlSerializer formatter, XElement element)
			{
				this.Clear();
				if (!element.IsEmpty)
				{
					object value;
					foreach (XElement fieldElement in element.Elements())
					{
						value = formatter.ReadObjectData(fieldElement);
						this.data.Add(GetCodeElementName(fieldElement.Name.LocalName), value);
					}
				}
			}
		}
		
		
		private const string DocumentSeparator = "<!-- XmlFormatterBase Document Separator -->";

		private	XDocument doc = null;

		
		[System.Diagnostics.DebuggerStepThrough]
		protected override bool MatchesStreamFormat(Stream stream)
		{
			try
			{
				using (XmlReader xmlRead = XmlReader.Create(stream, GetReaderSettings()))
				{
					xmlRead.Read();
				}
				return true;
			} 
			catch (Exception)
			{
				return false;
			}
		}
		protected override void WriteObjectData(object obj)
		{
			this.WriteObjectData(this.doc.Root, obj);
		}
		protected override object ReadObjectData()
		{
			XElement objElement = this.doc.Root;
			if (!objElement.HasAttributes) objElement = objElement.Elements().FirstOrDefault();
			return this.ReadObjectData(objElement);
		}

		protected override void OnBeginReadOperation()
		{
			base.OnBeginReadOperation();

			using (XmlReader reader = XmlReader.Create(ReadSingleDocument(this.TargetStream), GetReaderSettings()))
			{
				this.doc = XDocument.Load(reader);
			}
		}
		protected override void OnEndReadOperation()
		{
			base.OnEndReadOperation();
			this.doc = null;
		}
		protected override void OnBeginWriteOperation()
		{
			base.OnBeginWriteOperation();
			this.doc = new XDocument(new XElement("root"));
		}
		protected override void OnEndWriteOperation()
		{
			base.OnEndWriteOperation();
			using (XmlWriter writer = XmlWriter.Create(this.TargetStream, GetWriterSettings()))
			{
				this.doc.Save(writer);
			}

			// Insert "stop token" separator, so reading Xml data won't screw up 
			// the underlying streams position when reading it again later.
			using (StreamWriter writer = new StreamWriter(this.TargetStream.NonClosing()))
			{
				writer.WriteLine();
				writer.WriteLine(DocumentSeparator);
			}
		}


		private void WriteObjectData(XElement element, object obj)
		{
			// Null? Empty Element.
			if (obj == null) return;
			
			// Retrieve type data
			ObjectHeader header = this.PrepareWriteObject(obj);
			if (header == null) return;

			// Write data type header
			if (header.DataType != DataType.Unknown) element.SetAttributeValue("dataType", header.DataType.ToString());
			if (header.IsObjectTypeRequired && !string.IsNullOrEmpty(header.TypeString)) element.SetAttributeValue("type", header.TypeString);
			if (header.IsObjectIdRequired && header.ObjectId != 0) element.SetAttributeValue("id", XmlConvert.ToString(header.ObjectId));

			// Write object
			try 
			{
				this.idManager.PushIdLevel();
				this.WriteObjectBody(element, obj, header);
			}
			catch (Exception e)
			{
				// Log the error
				this.LocalLog.WriteError("Error writing object: {0}", LogFormat.Exception(e));
			}
			finally
			{
				this.idManager.PopIdLevel();
			}
		}
		private void WriteObjectBody(XElement element, object obj, ObjectHeader header)
		{
			if (header.IsPrimitive)							this.WritePrimitive		(element, obj);
			else if (obj is XElement childElement)          this.WriteXml(element, childElement);
			else if (header.DataType == DataType.Enum)		this.WriteEnum			(element, obj as Enum, header);
			else if (header.DataType == DataType.Struct)	this.WriteStruct		(element, obj, header);
			else if (header.DataType == DataType.ObjectRef)	element.Value = XmlConvert.ToString(header.ObjectId);
			else if	(header.DataType == DataType.Array)		this.WriteArray			(element, obj, header);
			else if (header.DataType == DataType.Delegate)	this.WriteDelegate		(element, obj, header);
			else if (header.DataType.IsMemberInfoType())	this.WriteMemberInfo	(element, obj, header);
		}

		/// <summary>
		/// This method is not strictly necessary to serialize <see cref="XElement"/> as that is already solved in <see cref="Surrogates.XElementSurrogate"/>
		/// However this method nicely embeds the xml in a readable way.
		/// See https://github.com/AdamsLair/duality/pull/861 for more info.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="childElement"></param>
		private void WriteXml(XElement element, XElement childElement)
		{
			// Simply add the XElement as a child.
			element.Add(childElement);
		}
		private void WritePrimitive(XElement element, object obj)
		{
			if      (obj is bool)    element.Value = XmlConvert.ToString((bool)obj);
			else if (obj is byte)    element.Value = XmlConvert.ToString((byte)obj);
			else if (obj is char)    element.Value = XmlConvert.EncodeName(XmlConvert.ToString((char)obj));
			else if (obj is sbyte)   element.Value = XmlConvert.ToString((sbyte)obj);
			else if (obj is short)   element.Value = XmlConvert.ToString((short)obj);
			else if (obj is ushort)  element.Value = XmlConvert.ToString((ushort)obj);
			else if (obj is int)     element.Value = XmlConvert.ToString((int)obj);
			else if (obj is uint)    element.Value = XmlConvert.ToString((uint)obj);
			else if (obj is long)    element.Value = XmlConvert.ToString((long)obj);
			else if (obj is ulong)   element.Value = XmlConvert.ToString((decimal)(ulong)obj);
			else if (obj is float)   element.Value = XmlConvert.ToString((float)obj);
			else if (obj is double)  element.Value = XmlConvert.ToString((double)obj);
			else if (obj is decimal) element.Value = XmlConvert.ToString((decimal)obj);
			else if (obj is string)
			{
				string strVal = obj as string;
				if (IsValidXmlString(strVal))
					element.Value = strVal;
				else
					element.Add(new XCData(Convert.ToBase64String(Encoding.UTF8.GetBytes(strVal))));
			}
			else if (obj == null)
				throw new ArgumentNullException("obj");
			else
				throw new ArgumentException(string.Format("Type '{0}' is not a primitive.", obj.GetType()));
		}
		private void WriteMemberInfo(XElement element, object obj, ObjectHeader header)
		{
			if (obj is Type)
			{
				Type type = obj as Type;
				SerializeType cachedType = GetSerializeType(type);
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
		private void WriteArray(XElement element, object obj, ObjectHeader header)
		{
			Array objAsArray = obj as Array;
			TypeInfo arrayTypeInfo = header.ObjectType;
			Type elementType = arrayTypeInfo.GetElementType();
			TypeInfo elementTypeInfo = elementType.GetTypeInfo();

			if (objAsArray.Rank != 1) throw new NotSupportedException("Non single-Rank arrays are not supported");
			if (objAsArray.GetLowerBound(0) != 0) throw new NotSupportedException("Non zero-based arrays are not supported");

			// If it's a primitive array, save the values as a comma-separated list
			if (elementType == typeof(bool))			this.WriteArrayData(element, objAsArray as bool[]);
			else if (elementType == typeof(byte))		this.WriteArrayData(element, objAsArray as byte[]);
			else if (elementType == typeof(sbyte))		this.WriteArrayData(element, objAsArray as sbyte[]);
			else if (elementType == typeof(short))		this.WriteArrayData(element, objAsArray as short[]);
			else if (elementType == typeof(ushort))		this.WriteArrayData(element, objAsArray as ushort[]);
			else if (elementType == typeof(int))		this.WriteArrayData(element, objAsArray as int[]);
			else if (elementType == typeof(uint))		this.WriteArrayData(element, objAsArray as uint[]);
			else if (elementType == typeof(long))		this.WriteArrayData(element, objAsArray as long[]);
			else if (elementType == typeof(ulong))		this.WriteArrayData(element, objAsArray as ulong[]);
			else if (elementType == typeof(float))		this.WriteArrayData(element, objAsArray as float[]);
			else if (elementType == typeof(double))		this.WriteArrayData(element, objAsArray as double[]);
			else if (elementType == typeof(decimal))	this.WriteArrayData(element, objAsArray as decimal[]);
			// Any non-trivial object data will be serialized recursively
			else
			{
				int nonDefaultElementCount = this.GetArrayNonDefaultElementCount(objAsArray, elementTypeInfo);

				// Write Array elements
				for (int i = 0; i < nonDefaultElementCount; i++)
				{
					XElement itemElement = new XElement("item");
					element.Add(itemElement);

					this.WriteObjectData(itemElement, objAsArray.GetValue(i));
				}

				// Write original length, in case trailing elements were omitted or we have an (XML-ambiguous) zero-element array.
				if (nonDefaultElementCount != objAsArray.Length || nonDefaultElementCount == 0)
				{
					element.SetAttributeValue("length", XmlConvert.ToString(objAsArray.Length));
				}
			}
		}
		private void WriteStruct(XElement element, object obj, ObjectHeader header)
		{
			ISerializeExplicit objAsCustom = obj as ISerializeExplicit;
			ISerializeSurrogate objSurrogate = header.SerializeType.Surrogate;
			
			// If we're serializing a value type, skip the entire object body if 
			// it equals the zero-init struct. This will keep struct-heavy data a lot
			// more concise.
			if (header.ObjectType.IsValueType && 
				object.Equals(obj, header.SerializeType.DefaultValue)) 
				return;

			// Write information about custom or surrogate serialization
			if (objAsCustom != null)  element.SetAttributeValue("custom"   , XmlConvert.ToString(true));
			if (objSurrogate != null) element.SetAttributeValue("surrogate", XmlConvert.ToString(true));

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
		private void WriteDelegate(XElement element, object obj, ObjectHeader header)
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
				this.WriteObjectData(methodElement, objAsDelegate.GetMethodInfo());
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
				this.WriteObjectData(methodElement, objAsDelegate.GetMethodInfo());
				this.WriteObjectData(targetElement, objAsDelegate.Target);
				this.WriteObjectData(invocationListElement, invokeList);
			}
		}
		private void WriteEnum(XElement element, Enum obj, ObjectHeader header)
		{
			element.SetAttributeValue("name", obj.ToString());
			element.SetAttributeValue("value", XmlConvert.ToString(Convert.ToInt64(obj)));
		}
		
		private void WriteArrayData(XElement element, bool[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, byte[] array)
		{
			element.Value = Convert.ToBase64String(array);
		}
		private void WriteArrayData(XElement element, sbyte[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, short[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, ushort[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, int[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, uint[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, long[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, ulong[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, float[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, double[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WriteArrayData(XElement element, decimal[] array)
		{
			this.WritePrimitiveArrayData(element, array, v => XmlConvert.ToString(v));
		}
		private void WritePrimitiveArrayData<T>(XElement element, T[] array, Func<T,string> toString) where T : struct
		{
			StringBuilder listBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (i != 0) listBuilder.Append(", ");
				listBuilder.Append(toString(array[i]));
			}
			element.Value = listBuilder.ToString();
		}


		private object ReadObjectData(XElement element)
		{
			// Empty element without type data? Return null
			if (element.IsEmpty && !element.HasAttributes)
				return null;

			// Read data type header
			string objIdString = element.GetAttributeValue("id");
			string dataTypeStr = element.GetAttributeValue("dataType");
			string typeStr = element.GetAttributeValue("type");

			uint objId = objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);

			DataType dataType = DataType.Unknown;
			if (!Enum.TryParse<DataType>(dataTypeStr, out dataType))
			{
				if (dataTypeStr == "Class") // Legacy support (Written 2014-03-10)
					dataType = DataType.Struct;
				if (dataTypeStr == "MethodInfo" || // Legacy support (Written 2015-06-07)
					dataTypeStr == "ConstructorInfo" ||
					dataTypeStr == "PropertyInfo" ||
					dataTypeStr == "FieldInfo" ||
					dataTypeStr == "EventInfo")
					dataType = DataType.MemberInfo;
				else 
					dataType = DataType.Unknown;
			}

			Type type = null;
			if (typeStr != null) type = this.ResolveType(typeStr, objId);

			ObjectHeader header = (type != null) ? 
				new ObjectHeader(objId, dataType, GetSerializeType(type)) :
				new ObjectHeader(objId, dataType, typeStr);
			if (header.DataType == DataType.Unknown)
			{
				this.LocalLog.WriteError("Unable to process DataType: {0}.", dataTypeStr);
				return null;
			}

			// Read object
			object result = null;
			try
			{
				// Read the objects body
				result = this.ReadObjectBody(element, header);
			}
			catch (Exception e)
			{
				this.LocalLog.WriteError("Error reading object: {0}", LogFormat.Exception(e));
			}

			return result;
		}
		private object ReadObjectBody(XElement element, ObjectHeader header)
		{
			object result = null;

			if (header.IsPrimitive)							result = this.ReadPrimitive(element, header.DataType);
			else if (header.DataType == DataType.Enum)		result = this.ReadEnum(element, header);
			else if (header.ObjectType == typeof(XElement)) result = this.ReadXml(element, header);
			else if (header.DataType == DataType.Struct)	result = this.ReadStruct(element, header);
			else if (header.DataType == DataType.ObjectRef)	result = this.ReadObjectRef(element);
			else if (header.DataType == DataType.Array)		result = this.ReadArray(element, header);
			else if (header.DataType == DataType.Delegate)	result = this.ReadDelegate(element, header);
			else if (header.DataType.IsMemberInfoType())	result = this.ReadMemberInfo(element, header);

			return result;
		}
		/// <summary>
		/// Counterpart of <see cref="WriteXml"/>
		/// </summary>
		/// <param name="element"></param>
		/// <param name="header"></param>
		/// <returns></returns>
		private object ReadXml(XElement element, ObjectHeader header)
		{
			return element.FirstNode;
		}
		private object ReadPrimitive(XElement element, DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Bool:    return XmlConvert.ToBoolean(element.Value);
				case DataType.Byte:    return XmlConvert.ToByte(element.Value);
				case DataType.SByte:   return XmlConvert.ToSByte(element.Value);
				case DataType.Short:   return XmlConvert.ToInt16(element.Value);
				case DataType.UShort:  return XmlConvert.ToUInt16(element.Value);
				case DataType.Int:     return XmlConvert.ToInt32(element.Value);
				case DataType.UInt:    return XmlConvert.ToUInt32(element.Value);
				case DataType.Long:    return XmlConvert.ToInt64(element.Value);
				case DataType.ULong:   return XmlConvert.ToUInt64(element.Value);
				case DataType.Float:   return XmlConvert.ToSingle(element.Value);
				case DataType.Double:  return XmlConvert.ToDouble(element.Value);
				case DataType.Decimal: return XmlConvert.ToDecimal(element.Value);
				case DataType.Char:    return XmlConvert.ToChar(XmlConvert.DecodeName(element.Value));
				case DataType.String:
					if (element.FirstNode is XCData)
					{
						byte[] stringData = Convert.FromBase64String((element.FirstNode as XCData).Value);
						return Encoding.UTF8.GetString(stringData, 0, stringData.Length);
					}
					else
					{ 
						return element.Value;
					}
				default:
					throw new ArgumentException(string.Format("DataType '{0}' is not a primitive.", dataType));
			}
		}
		private Array ReadArray(XElement element, ObjectHeader header)
		{
			Array arrObj;
			Type elementType = (header.ObjectType != null) ? header.ObjectType.GetElementType() : null;
			
			// Determine the array length based on child elements or explicit value
			string explicitLengthString = element.GetAttributeValue("length");
			int explicitLength = explicitLengthString == null ? -1 : XmlConvert.ToInt32(explicitLengthString);

			// Expect the "complex" array format, if there are child elements or an explicit length (children may be omitted)
			bool isComplex = element.Elements().Any() || explicitLength != -1;
			bool isEmpty = explicitLength == 0 || (!isComplex && string.IsNullOrEmpty(element.Value));

			// Early-out: Create an empty array
			if (isEmpty)
			{
				arrObj = elementType != null ? Array.CreateInstance(elementType, 0) : null;
				this.idManager.Inject(arrObj, header.ObjectId);
			}
			// Read a primitive value array
			else if (!isComplex)
			{
				if		(elementType == typeof(bool))		{ bool[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(byte))		{ byte[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(sbyte))		{ sbyte[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(short))		{ short[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(ushort))		{ ushort[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(int))		{ int[]		array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(uint))		{ uint[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(long))		{ long[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(ulong))		{ ulong[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(float))		{ float[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(double))		{ double[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else if (elementType == typeof(decimal))	{ decimal[]	array; this.ReadArrayData(element, out array); arrObj = array; }
				else
				{
					this.LocalLog.WriteWarning("Can't read primitive value array. Unknown element type '{0}'. Discarding data.", LogFormat.Type(elementType));
					arrObj = elementType != null ? Array.CreateInstance(elementType, 0) : null;
				}

				// Set object reference
				this.idManager.Inject(arrObj, header.ObjectId);
			}
			// Read a complex value array, where each item is an XML element
			else
			{
				SerializeType elementSerializeType = GetSerializeType(elementType);
				int arrLength = explicitLength != -1 ? explicitLength : element.Elements().Count();

				// Prepare object reference
				arrObj = elementType != null ? Array.CreateInstance(elementType, arrLength) : null;
				this.idManager.Inject(arrObj, header.ObjectId);

				int itemIndex = 0;
				foreach (XElement itemElement in element.Elements())
				{
					object item = this.ReadObjectData(itemElement);
					this.AssignValueToArray(elementSerializeType, arrObj, itemIndex, item);

					itemIndex++;
					if (itemIndex >= arrLength) break;
				}
			}

			return arrObj;
		}
		private object ReadStruct(XElement element, ObjectHeader header)
		{
			// Read struct type
			string	customString	= element.GetAttributeValue("custom");
			string	surrogateString	= element.GetAttributeValue("surrogate");
			bool	custom			= customString != null && XmlConvert.ToBoolean(customString);
			bool	surrogate		= surrogateString != null && XmlConvert.ToBoolean(surrogateString);

			// Retrieve surrogate if requested
			ISerializeSurrogate objSurrogate = null;
			if (surrogate && header.SerializeType != null)
			{
				custom = true;
				objSurrogate = header.SerializeType.Surrogate;
				if (objSurrogate == null)
				{
					this.LocalLog.WriteError(
						"Object type '{0}' was serialized using a surrogate, but no such surrogate was found for deserialization.",
						LogFormat.Type(header.SerializeType.Type));
				}
			}

			// If the object was serialized as a surrogate, deserialize its header first
			CustomSerialIO surrogateHeader = null;
			if (surrogate)
			{
				// Set fake object reference for surrogate constructor: No self-references allowed here.
				this.idManager.Inject(null, header.ObjectId);

				XElement headerElement = element.Element(CustomSerialIO.HeaderElement) ?? element.Elements().FirstOrDefault();
				surrogateHeader = new CustomSerialIO();
				if (headerElement != null)
				{
					surrogateHeader.Deserialize(this, headerElement);
				}
			}

			// Construct object
			object obj = null;
			if (header.ObjectType != null)
			{
				if (objSurrogate != null)
				{
					try { obj = objSurrogate.ConstructObject(surrogateHeader, header.ObjectType); }
					catch (Exception e) { this.LogCustomDeserializationError(header.ObjectId, header.ObjectType, e); }
				}
				if (obj == null) obj = header.ObjectType.CreateInstanceOf();
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
				{
					objAsCustom = obj as ISerializeExplicit;
				}

				if (objAsCustom != null)
				{
					try { objAsCustom.ReadData(customIO); }
					catch (Exception e) { this.LogCustomDeserializationError(header.ObjectId, header.ObjectType, e); }
				}
				else if (obj != null && header.ObjectType != null)
				{
					this.LocalLog.WriteWarning(
						"Object data (Id {0}) is flagged for custom deserialization, yet the objects Type ('{1}') does not support it. Guessing associated fields...",
						header.ObjectId,
						LogFormat.Type(header.ObjectType));
					this.LocalLog.PushIndent();
					foreach (var pair in customIO.Data)
					{
						this.AssignValueToField(header.SerializeType, obj, pair.Key, pair.Value);
					}
					this.LocalLog.PopIndent();
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
		private object ReadObjectRef(XElement element)
		{
			object obj;
			uint objId = XmlConvert.ToUInt32(element.Value);

			if (!this.idManager.Lookup(objId, out obj)) throw new Exception(string.Format("Can't resolve object reference '{0}'.", objId));

			return obj;
		}
		private MemberInfo ReadMemberInfo(XElement element, ObjectHeader header)
		{
			MemberInfo result;
			try
			{
				if (header.DataType == DataType.Type)
				{
					string typeString = element.GetAttributeValue("value");
					Type type = this.ResolveType(typeString, header.ObjectId);
					result = (type != null) ? type.GetTypeInfo() : null;
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
				this.LocalLog.WriteError(
					"An error occurred in deserializing MemberInfo object Id {0} of type '{1}': {2}",
					header.ObjectId,
					LogFormat.Type(header.ObjectType),
					LogFormat.Exception(e));
			}

			// Prepare object reference
			this.idManager.Inject(result, header.ObjectId);

			return result;
		}
		private Delegate ReadDelegate(XElement element, ObjectHeader header)
		{
			string multiString = element.GetAttributeValue("multi");
			bool multi = multiString != null && XmlConvert.ToBoolean(multiString);

			XElement methodElement = element.Element("method") ?? element.Elements().FirstOrDefault();
			XElement targetElement = element.Element("target") ?? element.Elements().ElementAtOrDefault(1);
			XElement invocationListElement = element.Element("invocationList") ?? element.Elements().ElementAtOrDefault(2);

			// Create the delegate without target and fix it later, so we can register its object id before loading its target object
			MethodInfo	method	= this.ReadObjectData(methodElement) as MethodInfo;
			object		target	= this.ReadObjectData(targetElement);
			Delegate	del		= header.ObjectType != null && method != null ? method.CreateDelegate(header.ObjectType.AsType(), target) : null;

			// Add object reference
			this.idManager.Inject(del, header.ObjectId);

			// Combine multicast delegates
			if (multi)
			{
				Delegate[] invokeList = (this.ReadObjectData(invocationListElement) as Delegate[]).NotNull().ToArray();
				del = invokeList.Length > 0 ? Delegate.Combine(invokeList) : null;
			}

			return del;
		}
		private Enum ReadEnum(XElement element, ObjectHeader header)
		{
			string name = element.GetAttributeValue("name");
			string valueString = element.GetAttributeValue("value");
			long val = valueString == null ? 0 : XmlConvert.ToInt64(valueString);
			return (header.ObjectType == null) ? null : this.ResolveEnumValue(header.ObjectType.AsType(), name, val);
		}
		
		private void ReadArrayData(XElement element, out bool[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToBoolean(s));
		}
		private void ReadArrayData(XElement element, out byte[] array)
		{
			array = Convert.FromBase64String(element.Value);
		}
		private void ReadArrayData(XElement element, out sbyte[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => (sbyte)XmlConvert.ToInt16(s));
		}
		private void ReadArrayData(XElement element, out short[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToInt16(s));
		}
		private void ReadArrayData(XElement element, out ushort[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToUInt16(s));
		}
		private void ReadArrayData(XElement element, out int[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToInt32(s));
		}
		private void ReadArrayData(XElement element, out uint[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToUInt32(s));
		}
		private void ReadArrayData(XElement element, out long[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToInt64(s));
		}
		private void ReadArrayData(XElement element, out ulong[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToUInt64(s));
		}
		private void ReadArrayData(XElement element, out float[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToSingle(s));
		}
		private void ReadArrayData(XElement element, out double[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToDouble(s));
		}
		private void ReadArrayData(XElement element, out decimal[] array)
		{
			this.ReadPrimitiveArrayData(element, out array, s => XmlConvert.ToDecimal(s));
		}
		private void ReadPrimitiveArrayData<T>(XElement element, out T[] array, Func<string,T> fromString) where T : struct
		{
			string[] valueStrings = element.Value.Split(new char[] { ',' });
			array = new T[valueStrings.Length];
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = fromString(valueStrings[i]);
				}
			}
			catch (Exception e)
			{
				this.LocalLog.WriteError("Error reading primitive value array of element type {0}: {1}", LogFormat.Type(typeof(T)), LogFormat.Exception(e));
			}
		}
		
		/// <summary>
		/// Determines the length of the longest Array element sequence that contains
		/// non-default values, beginning at index zero. It is the number of elements
		/// that actually needs to be serialized.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="elementType"></param>
		private int GetArrayNonDefaultElementCount(Array array, TypeInfo elementType)
		{
			if (array.Length == 0) return 0;

			int omitElementCount = 0;
			object defaultValue = elementType.GetDefaultOf();
			while (omitElementCount < array.Length && object.Equals(array.GetValue(array.Length - omitElementCount - 1), defaultValue))
			{
				omitElementCount++;
			}

			return array.Length - omitElementCount;
		}
		
		/// <summary>
		/// Wraps the specified <see cref="Stream"/> in a sub-stream that can only access the next available XML document section.
		/// </summary>
		/// <param name="stream"></param>
		private static Stream ReadSingleDocument(Stream stream)
		{
			if (!stream.CanSeek) throw new InvalidOperationException("The specified stream needs to be seekable.");

			long oldPos = stream.Position;

			Encoding encoding = null;
			StringBuilder docDataBuilder = new StringBuilder();
			int byteOffset = 0;

			using (StreamReader reader = new StreamReader(stream.NonClosing()))
			{
				// Determine Encoding and preamble length when at the beginning of the stream
				encoding = reader.CurrentEncoding;
				if (stream.Position == 0)
				{
					byte[] preamble = encoding.GetPreamble(); 
					byte[] preambleRead = new byte[preamble.Length]; 
					if (stream.Read(preambleRead, 0, preambleRead.Length) == preamble.Length)
					{
						if (preamble.SequenceEqual(preambleRead))
						{
							byteOffset += preamble.Length;
						}
					}
					stream.Position = 0;
				}

				// Read the appropriate XML document portion of the stream.
				bool firstContentLine = true;
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					int indexOf = line.IndexOf(DocumentSeparator);

					// Consume regular lines
					if (indexOf == -1)
					{
						docDataBuilder.AppendLine(line);
					}
					// Consume the separator when it occurs first
					else if (firstContentLine)
					{
						docDataBuilder.AppendLine(line.Remove(0, indexOf + DocumentSeparator.Length));
						byteOffset += reader.CurrentEncoding.GetBytes(DocumentSeparator).Length;
					}
					// Stop at the separator when it occurs last
					else
					{
						docDataBuilder.Append(line.Substring(0, indexOf));
						byteOffset += reader.CurrentEncoding.GetBytes(DocumentSeparator + Environment.NewLine).Length;
						break;
					}

					if (!string.IsNullOrWhiteSpace(line)) firstContentLine = false;
				}
			}

			// Create a MemoryStream from the desired subsection of the original Stream
			string reducedDoc = docDataBuilder.ToString();
			byte[] reducedData = encoding.GetBytes(reducedDoc);
			MemoryStream result = new MemoryStream(reducedData);

			// Reset the original Stream to the expected position and return the substream
			stream.Position = oldPos + byteOffset + reducedData.Length;
			return result;
		}

		[System.Diagnostics.DebuggerStepThrough]
		private static bool IsValidXmlString(string value)
		{
			try
			{
				// Note: XML will normalize all \r chars into \n chars, so we'll treat \r as invalid, 
				// triggering the verbatim / escaped / binary code path.
				return 
					value.IndexOf('\r') == -1 && 
					XmlConvert.VerifyXmlChars(value) != null;
			}
			catch (XmlException)
			{
				return false;
			}
		}
		private static string GetXmlElementName(string codeName)
		{
			return XmlConvert.EncodeName(codeName);
		}
		private static string GetCodeElementName(string xmlName)
		{
			// Legacy support. Remove later. (Written 2014-01-10)
			xmlName = xmlName.Replace("__sbo__", "<").Replace("__sbc__", ">");
			return XmlConvert.DecodeName(xmlName);
		}

		private static XmlReaderSettings GetReaderSettings()
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.DtdProcessing = DtdProcessing.Ignore;
			settings.IgnoreWhitespace = true;
			settings.IgnoreComments = true;
			settings.IgnoreProcessingInstructions = true;
			settings.CloseInput = false;
			return settings;
		}
		private static XmlWriterSettings GetWriterSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			settings.CloseOutput = false;
			return settings;
		}
	}
}
