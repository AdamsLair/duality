using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace Duality.Serialization
{
	/// <summary>
	/// Base class for Dualitys xml serializers.
	/// </summary>
	public abstract class XmlFormatterBase : Formatter
	{
		/// <summary>
		/// Buffer object for <see cref="Duality.Serialization.ISerializeExplicit">custom de/serialization</see>, 
		/// providing read and write functionality.
		/// </summary>
		protected class CustomSerialIO : CustomSerialIOBase<XmlFormatterBase>
		{
			public const string HeaderElement = "header";
			public const string BodyElement = "body";

			/// <summary>
			/// Writes the contained data to the specified serializer.
			/// </summary>
			/// <param name="formatter">The serializer to write data to.</param>
			public void Serialize(XmlFormatterBase formatter, XElement element)
			{
				foreach (var pair in this.data)
				{
					XElement fieldElement = new XElement(GetXmlElementName(pair.Key));
					element.Add(fieldElement);
					formatter.WriteObjectData(fieldElement, pair.Value);
				}
				this.Clear();
			}
			/// <summary>
			/// Reads data from the specified serializer
			/// </summary>
			/// <param name="formatter">The serializer to read data from.</param>
			public void Deserialize(XmlFormatterBase formatter, XElement element)
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

		
		protected const string DocumentSeparator = "<!-- XmlFormatterBase Document Separator -->";

		protected	Stream		stream	= null;
		protected	XDocument	doc		= null;
		

		public override bool CanWrite
		{
			get { return this.stream != null && this.stream.CanWrite; }
		}
		public override bool CanRead
		{
			get { return this.stream != null && this.stream.CanRead; }
		}


		protected XmlFormatterBase() : this(null) {}
		protected XmlFormatterBase(Stream stream)
		{
			this.stream = stream;
		}

		protected override object ReadObjectData()
		{
			XElement objElement = this.doc.Root;
			if (!objElement.HasAttributes) objElement = objElement.Elements().FirstOrDefault();
			return this.ReadObjectData(objElement);
		}
		protected object ReadObjectData(XElement element)
		{
			// Empty element without type data? Return null
			if (element.IsEmpty && !element.HasAttributes)
			{
				return this.GetNullObject();
			}

			// Read data type header
			string objIdString = element.GetAttributeValue("id");
			string dataTypeStr = element.GetAttributeValue("dataType");
			string typeStr = element.GetAttributeValue("type");
			uint objId = objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			DataType dataType;
			if (!Enum.TryParse<DataType>(dataTypeStr, out dataType))
			{
				if (dataTypeStr == "Class") // Legacy support (Written 2014-03-10)
					dataType = DataType.Struct;
				else 
					dataType = DataType.Unknown;
			}
			ObjectHeader header = this.ParseObjectHeader(objId, dataType, typeStr);
			if (header.DataType == DataType.Unknown)
			{
				this.SerializationLog.WriteError("Unable to process DataType: {0}.", dataTypeStr);
				return this.GetNullObject();
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
				this.SerializationLog.WriteError("Error reading object: {0}", e is ApplicationException ? e.Message : Log.Exception(e));
			}

			return result ?? this.GetNullObject();
		}
		/// <summary>
		/// Reads the body of an object.
		/// </summary>
		/// <param name="element">The XML element that describes the object.</param>
		/// <param name="dataType">The <see cref="Duality.Serialization.DataType"/> that is assumed.</param>
		/// <returns>The object that has been read.</returns>
		protected abstract object ReadObjectBody(XElement element, ObjectHeader header);
		/// <summary>
		/// Reads a single primitive value, assuming the specified <see cref="Duality.Serialization.DataType"/>.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="dataType"></param>
		/// <returns></returns>
		protected object ReadPrimitive(XElement element, ObjectHeader header)
		{
			string val = element.Value;
			switch (header.DataType)
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
					throw new ArgumentException(string.Format("DataType '{0}' is not a primitive.", header.DataType));
			}
		}

		protected override void WriteObjectData(object obj)
		{
			this.WriteObjectData(this.doc.Root, obj);
		}
		protected void WriteObjectData(XElement element, object obj)
		{
			// Null? Empty Element.
			if (object.Equals(obj, this.GetNullObject()))
			{
				return;
			}
			
			// Retrieve type data
			ObjectHeader header = this.PrepareWriteObject(obj);

			// Write data type header
			element.SetAttributeValue("dataType", header.DataType.ToString());
			if (header.IsObjectTypeRequired) element.SetAttributeValue("type", header.TypeString);
			if (header.ObjectId != 0) element.SetAttributeValue("id", XmlConvert.ToString(header.ObjectId));

			// Write object
			try 
			{
				this.idManager.PushIdLevel();
				this.WriteObjectBody(element, obj, header);
			}
			catch (Exception e)
			{
				// Log the error
				this.SerializationLog.WriteError("Error writing object: {0}", e is ApplicationException ? e.Message : Log.Exception(e));
			}
			finally
			{
				this.idManager.PopIdLevel();
			}
		}
		/// <summary>
		/// Writes the body of a given object.
		/// </summary>
		/// <param name="element">The XML element to write to.</param>
		/// <param name="dataType">The <see cref="Duality.Serialization.DataType"/> as which the object will be written.</param>
		/// <param name="obj">The object to be written.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> that describes the specified object.</param>
		/// <param name="objId">An object id that is assigned to the specified object.</param>
		protected abstract void WriteObjectBody(XElement element, object obj, ObjectHeader header);
		/// <summary>
		/// Writes a single primitive value.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="obj">The primitive value to write.</param>
		protected void WritePrimitive(XElement element, object obj)
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

		/// <summary>
		/// Determines the length of the longest Array element sequence that contains
		/// non-default values, beginning at index zero. It is the number of elements
		/// that actually needs to be serialized.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="elementType"></param>
		/// <returns></returns>
		protected int GetArrayNonDefaultElementCount(Array array, Type elementType)
		{
			if (array.Length == 0) return 0;

			int omitElementCount = 0;
			object defaultValue = elementType.GetDefaultInstanceOf();
			while (object.Equals(array.GetValue(array.Length - omitElementCount - 1), defaultValue))
			{
				omitElementCount++;
			}

			return array.Length - omitElementCount;
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
		protected static Stream ReadSingleDocument(Stream stream)
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

		protected static byte[] DecodeByteArray(string str)
		{
			return Convert.FromBase64String(str);
		}
		protected static string EncodeByteArray(byte[] arr)
		{
			return Convert.ToBase64String(arr, Base64FormattingOptions.None);
		}

		protected static string GetXmlElementName(string codeName)
		{
			return XmlConvert.EncodeName(codeName);
		}
		protected static string GetCodeElementName(string xmlName)
		{
			// Legacy support. Remove later. (Written 2014-01-10)
			xmlName = xmlName.Replace("__sbo__", "<").Replace("__sbc__", ">");
			return XmlConvert.DecodeName(xmlName);
		}

		protected static XmlReaderSettings GetReaderSettings()
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.DtdProcessing = DtdProcessing.Ignore;
			settings.IgnoreWhitespace = true;
			settings.IgnoreComments = true;
			settings.IgnoreProcessingInstructions = true;
			settings.CloseInput = false;
			return settings;
		}
		protected static XmlWriterSettings GetWriterSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			settings.CloseOutput = false;
			return settings;
		}
	}
}
