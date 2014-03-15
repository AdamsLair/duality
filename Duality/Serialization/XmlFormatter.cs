using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace Duality.Serialization
{
	/// <summary>
	/// De/Serializes object data.
	/// </summary>
	public class XmlFormatter : Formatter
	{
		private class CustomSerialIO : CustomSerialIOBase<XmlFormatter>
		{
			public const string HeaderElement = "header";
			public const string BodyElement = "body";

			public void Serialize(XmlFormatter formatter, XElement element)
			{
				foreach (var pair in this.data)
				{
					XElement fieldElement = new XElement(GetXmlElementName(pair.Key));
					element.Add(fieldElement);
					formatter.WriteObjectData(fieldElement, pair.Value);
				}
				this.Clear();
			}
			public void Deserialize(XmlFormatter formatter, XElement element)
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

		private	Stream		stream	= null;
		private	XDocument	doc		= null;
		

		public override bool CanWrite
		{
			get { return this.stream != null && this.stream.CanWrite; }
		}
		public override bool CanRead
		{
			get { return this.stream != null && this.stream.CanRead; }
		}


		public XmlFormatter(Stream stream)
		{
			this.stream = stream;
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

		protected override void BeginReadOperation()
		{
			if (this.stream == null) throw new InvalidOperationException("Can't read data, because the Stream is unavailable.");
			if (!this.stream.CanRead) throw new InvalidOperationException("Can't read data, because the Stream doesn't support it.");

			base.BeginReadOperation();

			using (XmlReader reader = XmlReader.Create(ReadSingleDocument(this.stream), GetReaderSettings()))
			{
				this.doc = XDocument.Load(reader);
			}
		}
		protected override void EndReadOperation()
		{
			base.EndReadOperation();
			this.doc = null;
		}
		protected override void BeginWriteOperation()
		{
			if (this.stream == null) throw new InvalidOperationException("Can't write data, because the Stream is unavailable.");
			if (!this.stream.CanWrite) throw new InvalidOperationException("Can't write data, because the Stream doesn't support it.");

			base.BeginWriteOperation();
			this.doc = new XDocument(new XElement("root"));
		}
		protected override void EndWriteOperation()
		{
			base.EndWriteOperation();
			using (XmlWriter writer = XmlWriter.Create(this.stream, GetWriterSettings()))
			{
				this.doc.Save(writer);
			}

			// Insert "stop token" separator, so reading Xml data won't screw up 
			// the underlying streams position when reading it again later.
			using (StreamWriter writer = new StreamWriter(this.stream.NonClosing()))
			{
				writer.WriteLine();
				writer.WriteLine(DocumentSeparator);
			}
		}


		private void WriteObjectData(XElement element, object obj)
		{
			// Null? Empty Element.
			if (obj == null)
				return;
			
			// Retrieve type data
			ObjectHeader header = this.PrepareWriteObject(obj);

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
				this.LocalLog.WriteError("Error writing object: {0}", e is ApplicationException ? e.Message : Log.Exception(e));
			}
			finally
			{
				this.idManager.PopIdLevel();
			}
		}
		private void WriteObjectBody(XElement element, object obj, ObjectHeader header)
		{
			if (header.IsPrimitive)							this.WritePrimitive		(element, obj);
			else if (header.DataType == DataType.Enum)		this.WriteEnum			(element, obj as Enum, header);
			else if (header.DataType == DataType.String)	element.Value = obj as string;
			else if (header.DataType == DataType.Struct)	this.WriteStruct		(element, obj, header);
			else if (header.DataType == DataType.ObjectRef)	element.Value = XmlConvert.ToString(header.ObjectId);
			else if	(header.DataType == DataType.Array)		this.WriteArray			(element, obj, header);
			else if (header.DataType == DataType.Delegate)	this.WriteDelegate		(element, obj, header);
			else if (header.DataType.IsMemberInfoType())	this.WriteMemberInfo	(element, obj, header);
		}
		private void WritePrimitive(XElement element, object obj)
		{
			if		(obj is bool)		element.Value = XmlConvert.ToString((bool)obj);
			else if (obj is byte)		element.Value = XmlConvert.ToString((byte)obj);
			else if (obj is char)		element.Value = XmlConvert.ToString((char)obj);
			else if (obj is string)		element.Value = (string)obj;
			else if (obj is sbyte)		element.Value = XmlConvert.ToString((sbyte)obj);
			else if (obj is short)		element.Value = XmlConvert.ToString((short)obj);
			else if (obj is ushort)		element.Value = XmlConvert.ToString((ushort)obj);
			else if (obj is int)		element.Value = XmlConvert.ToString((int)obj);
			else if (obj is uint)		element.Value = XmlConvert.ToString((uint)obj);
			else if (obj is long)		element.Value = XmlConvert.ToString((long)obj);
			else if (obj is ulong)		element.Value = XmlConvert.ToString((decimal)(ulong)obj);
			else if (obj is float)		element.Value = XmlConvert.ToString((float)obj);
			else if (obj is double)		element.Value = XmlConvert.ToString((double)obj);
			else if (obj is decimal)	element.Value = XmlConvert.ToString((decimal)obj);
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
		private void WriteArray(XElement element, object obj, ObjectHeader header)
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
		private void WriteStruct(XElement element, object obj, ObjectHeader header)
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
		private void WriteEnum(XElement element, Enum obj, ObjectHeader header)
		{
			element.SetAttributeValue("name", obj.ToString());
			element.SetAttributeValue("value", XmlConvert.ToString(Convert.ToInt64(obj)));
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
				else 
					dataType = DataType.Unknown;
			}

			Type type = null;
			if (typeStr != null) type = this.ResolveType(typeStr, objId);

			ObjectHeader header = new ObjectHeader(objId, dataType, type != null ? type.GetSerializeType() : null);
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
				this.LocalLog.WriteError("Error reading object: {0}", e is ApplicationException ? e.Message : Log.Exception(e));
			}

			return result;
		}
		private object ReadObjectBody(XElement element, ObjectHeader header)
		{
			object result = null;

			if (header.IsPrimitive)							result = this.ReadPrimitive(element, header.DataType);
			else if (header.DataType == DataType.String)	result = element.Value;
			else if (header.DataType == DataType.Enum)		result = this.ReadEnum(element, header);
			else if (header.DataType == DataType.Struct)	result = this.ReadStruct(element, header);
			else if (header.DataType == DataType.ObjectRef)	result = this.ReadObjectRef(element);
			else if (header.DataType == DataType.Array)		result = this.ReadArray(element, header);
			else if (header.DataType == DataType.Delegate)	result = this.ReadDelegate(element, header);
			else if (header.DataType.IsMemberInfoType())	result = this.ReadMemberInfo(element, header);

			return result;
		}
		private object ReadPrimitive(XElement element, DataType dataType)
		{
			string val = element.Value;
			switch (dataType)
			{
				case DataType.Bool:			return XmlConvert.ToBoolean(val);
				case DataType.Byte:			return XmlConvert.ToByte(val);
				case DataType.SByte:		return XmlConvert.ToSByte(val);
				case DataType.Short:		return XmlConvert.ToInt16(val);
				case DataType.UShort:		return XmlConvert.ToUInt16(val);
				case DataType.Int:			return XmlConvert.ToInt32(val);
				case DataType.UInt:			return XmlConvert.ToUInt32(val);
				case DataType.Long:			return XmlConvert.ToInt64(val);
				case DataType.ULong:		return XmlConvert.ToUInt64(val);
				case DataType.Float:		return XmlConvert.ToSingle(val);
				case DataType.Double:		return XmlConvert.ToDouble(val);
				case DataType.Decimal:		return XmlConvert.ToDecimal(val);
				case DataType.Char:			return XmlConvert.ToChar(val);
				case DataType.String:		return val;
				default:
					throw new ArgumentException(string.Format("DataType '{0}' is not a primitive.", dataType));
			}
		}
		private Array ReadArray(XElement element, ObjectHeader header)
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
		private object ReadStruct(XElement element, ObjectHeader header)
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
					this.LocalLog.WriteWarning(
						"Object data (Id {0}) is flagged for custom deserialization, yet the objects Type ('{1}') does not support it. Guessing associated fields...",
						header.ObjectId,
						Log.Type(header.ObjectType));
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

			if (!this.idManager.Lookup(objId, out obj)) throw new ApplicationException(string.Format("Can't resolve object reference '{0}'.", objId));

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
				this.LocalLog.WriteError(
					"An error occurred in deserializing MemberInfo object Id {0} of type '{1}': {2}",
					header.ObjectId,
					Log.Type(header.ObjectType),
					Log.Exception(e));
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
		private Enum ReadEnum(XElement element, ObjectHeader header)
		{
			string name = element.GetAttributeValue("name");
			string valueString = element.GetAttributeValue("value");
			long val = valueString == null ? 0 : XmlConvert.ToInt64(valueString);
			return (header.ObjectType == null) ? null : this.ResolveEnumValue(header.ObjectType, name, val);
		}

		
		/// <summary>
		/// Determines the length of the longest Array element sequence that contains
		/// non-default values, beginning at index zero. It is the number of elements
		/// that actually needs to be serialized.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="elementType"></param>
		/// <returns></returns>
		private int GetArrayNonDefaultElementCount(Array array, Type elementType)
		{
			if (array.Length == 0) return 0;

			int omitElementCount = 0;
			object defaultValue = elementType.GetDefaultInstanceOf();
			while (omitElementCount < array.Length && object.Equals(array.GetValue(array.Length - omitElementCount - 1), defaultValue))
			{
				omitElementCount++;
			}

			return array.Length - omitElementCount;
		}

		/// <summary>
		/// Returns whether the specified stream is an XML stream. The check requires a stream that is both readable and seekable.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		[System.Diagnostics.DebuggerStepThrough]
		public static bool IsXmlStream(Stream stream)
		{
			if (!stream.CanRead) throw new InvalidOperationException("The specified stream is not readable.");
			if (!stream.CanSeek) throw new InvalidOperationException("The specified stream is not seekable. XML check aborted to maintain stream state.");
			if (stream.Length == 0) throw new InvalidOperationException("The specified stream is empty.");
			long oldPos = stream.Position;

			bool isXml = true;
			try
			{
				using (XmlReader xmlRead = XmlReader.Create(stream, GetReaderSettings()))
				{
					xmlRead.Read();
				}
			} catch (Exception) { isXml = false; }
			stream.Seek(oldPos, SeekOrigin.Begin);

			return isXml;
		}
		
		/// <summary>
		/// Wraps the specified <see cref="Stream"/> in a sub-stream that can only access the next available XML document section.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
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
						byteOffset += Encoding.Default.GetBytes(DocumentSeparator).Length;
					}
					// Stop at the separator when it occurs last
					else
					{
						docDataBuilder.Append(line.Substring(0, indexOf));
						byteOffset += Encoding.Default.GetBytes(DocumentSeparator + Environment.NewLine).Length;
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

		private static byte[] DecodeByteArray(string str)
		{
			return Convert.FromBase64String(str);
		}
		private static string EncodeByteArray(byte[] arr)
		{
			return Convert.ToBase64String(arr, Base64FormattingOptions.None);
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
