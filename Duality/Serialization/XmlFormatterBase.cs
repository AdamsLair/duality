using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
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

			private string elementName = "customSerialIO";

			public CustomSerialIO(string elementName)
			{
				this.elementName = elementName;
			}

			/// <summary>
			/// Writes the contained data to the specified serializer.
			/// </summary>
			/// <param name="formatter">The serializer to write data to.</param>
			public override void Serialize(XmlFormatterBase formatter)
			{
				formatter.writer.WriteStartElement(this.elementName);
				try
				{
					foreach (var pair in this.data)
						formatter.WriteObjectData(pair.Value, pair.Key);
				}
				finally
				{
					formatter.writer.WriteEndElement();
				}
				this.Clear();
			}
			/// <summary>
			/// Reads data from the specified serializer
			/// </summary>
			/// <param name="formatter">The serializer to read data from.</param>
			public override void Deserialize(XmlFormatterBase formatter)
			{
				this.Clear();

				int elementDepth = formatter.ReadUntilElementStart();

				try
				{
					if (!formatter.reader.IsEmptyElement)
					{
						bool scopeChanged;
						string key;
						object value;
						while (true)
						{
							value = formatter.ReadObjectData(out key, out scopeChanged);
							if (scopeChanged) break;
							else
							{
								this.data.Add(key, value);
							}
						}
					}
				}
				finally
				{
					formatter.ReadUntilElementEnd(elementDepth);
				}
			}
		}

		
		protected const string DocumentSeparator = "<!-- XmlFormatterBase Document Separator -->";

		protected	Stream		stream	= null;
		protected	XmlWriter	writer	= null;
		protected	XmlReader	reader	= null;
		

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
		protected override void OnDisposed(bool manually)
		{
			base.OnDisposed(manually);
			this.DisposeWriter();
			this.DisposeReader();
		}

		protected void DisposeWriter()
		{
			if (this.writer == null) return;
			this.writer.Flush();
			this.writer.Close();
			this.writer = null;
		}
		protected void DisposeReader()
		{
			if (this.reader == null) return;
			this.reader.Close();
			this.reader = null;
		}
		protected void CreateWriter()
		{
			this.DisposeWriter();

			this.writer = (this.stream != null && this.stream.CanWrite) ? 
				XmlTextWriter.Create(this.stream, GetWriterSettings()) : 
				null;
		}
		protected void CreateReader()
		{
			this.DisposeReader();

			this.reader = (stream != null && stream.CanRead) ? 
				XmlTextReader.Create(ReadSingleDocument(stream), GetReaderSettings()) : 
				null;
		}

		protected override object ReadObjectData()
		{
			string objName;
			bool scopeChanged;
			object obj;

			do
			{
				obj = this.ReadObjectData(out objName, out scopeChanged);
			} while (scopeChanged);

			return obj;
		}
		protected object ReadObjectData(out string objName, out bool scopeChanged)
		{
			if (this.reader.ReadState == ReadState.EndOfFile) throw new EndOfStreamException("No more data to read.");
			if (this.reader.ReadState == ReadState.Error) throw new EndOfStreamException("An XML Error occurred.");

			int elementDepth = this.ReadUntilElementStart();
			objName = GetCodeElementName(this.reader.Name);
			scopeChanged = elementDepth == -1;

			// Moved outside of current scope? Return null
			if (scopeChanged)
			{
				this.ReadUntilElementEnd(elementDepth);
				return this.GetNullObject();
			}

			// Empty element without type data? Return null
			if (this.reader.IsEmptyElement && !this.reader.HasAttributes)
			{
				this.ReadUntilElementEnd(elementDepth);
				return this.GetNullObject();
			}

			// Read data type header
			string dataTypeStr = this.reader.GetAttribute("dataType");
			DataType dataType;
			if (!Enum.TryParse<DataType>(dataTypeStr, out dataType))
			{
				dataType = DataType.Unknown;
				this.SerializationLog.WriteError("Can't resolve DataType: {0}. Returning null reference.", dataTypeStr);
				this.ReadUntilElementEnd(elementDepth);
				return this.GetNullObject();
			}

			// Read object
			object result = null;
			try
			{
				// Read the objects body
				result = this.ReadObjectBody(dataType);
			}
			catch (Exception e)
			{
				this.SerializationLog.WriteError("Error reading object: {0}", e is ApplicationException ? e.Message : Log.Exception(e));
			}
			finally
			{
				this.ReadUntilElementEnd(elementDepth);
			}

			return result ?? this.GetNullObject();
		}
		/// <summary>
		/// Reads the body of an object.
		/// </summary>
		/// <param name="dataType">The <see cref="Duality.Serialization.DataType"/> that is assumed.</param>
		/// <returns>The object that has been read.</returns>
		protected abstract object ReadObjectBody(DataType dataType);
		/// <summary>
		/// Reads a single primitive value, assuming the specified <see cref="Duality.Serialization.DataType"/>.
		/// </summary>
		/// <param name="dataType"></param>
		/// <returns></returns>
		protected object ReadPrimitive(DataType dataType)
		{
			string val = this.reader.ReadString();
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
				default:
					throw new ArgumentException(string.Format("DataType '{0}' is not a primitive.", dataType));
			}
		}

		protected int ReadUntilElementStart()
		{
			int oldDepth = this.reader.Depth;
			while (this.reader.Read())
			{
				if (this.reader.Depth < oldDepth) return -1;
				if (this.reader.NodeType == XmlNodeType.Element) return this.reader.Depth;
			}
			return -1;
		}
		protected void ReadUntilElementEnd(int depth)
		{
			if (depth == -1) return;
			if (this.reader.IsEmptyElement && this.reader.Depth == depth) return;
			do
			{
				if (this.reader.NodeType == XmlNodeType.EndElement && this.reader.Depth == depth) return;
			} while (this.reader.Read());
		}
		

		protected override void WriteObjectData(object obj)
		{
			this.WriteObjectData(obj, "object");
		}
		protected void WriteObjectData(object obj, string elementName)
		{
			elementName = GetXmlElementName(elementName);
			this.writer.WriteStartElement(elementName);

			// Null? Empty Element.
			if (obj == this.GetNullObject())
			{
				this.writer.WriteEndElement();
				return;
			}
			
			// Retrieve type data
			SerializeType objSerializeType;
			uint objId;
			DataType dataType;
			this.GetWriteObjectData(obj, out objSerializeType, out dataType, out objId);
			
			// Unknown DataType? Empty Element.
			if (dataType == DataType.Unknown)
			{
				this.writer.WriteEndElement();
				return;
			}

			// Write data type header
			this.writer.WriteAttributeString("dataType", dataType.ToString());

			// Write object
			try 
			{
				this.idManager.PushIdLevel();
				this.WriteObjectBody(dataType, obj, objSerializeType, objId);
			}
			catch (Exception e)
			{
				// Log the error
				this.SerializationLog.WriteError("Error writing object: {0}", e is ApplicationException ? e.Message : Log.Exception(e));
			}
			finally
			{
				this.writer.WriteEndElement();
				this.idManager.PopIdLevel();
			}
		}
		/// <summary>
		/// Writes the body of a given object.
		/// </summary>
		/// <param name="dataType">The <see cref="Duality.Serialization.DataType"/> as which the object will be written.</param>
		/// <param name="obj">The object to be written.</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> that describes the specified object.</param>
		/// <param name="objId">An object id that is assigned to the specified object.</param>
		protected abstract void WriteObjectBody(DataType dataType, object obj, SerializeType objSerializeType, uint objId);
		/// <summary>
		/// Writes a single primitive value.
		/// </summary>
		/// <param name="obj">The primitive value to write.</param>
		protected void WritePrimitive(object obj)
		{
			if		(obj is bool)		this.writer.WriteValue((bool)obj);
			else if (obj is byte)		this.writer.WriteValue((byte)obj);
			else if (obj is char)		this.writer.WriteValue((char)obj);
			else if (obj is sbyte)		this.writer.WriteValue((sbyte)obj);
			else if (obj is short)		this.writer.WriteValue((short)obj);
			else if (obj is ushort)		this.writer.WriteValue((ushort)obj);
			else if (obj is int)		this.writer.WriteValue((int)obj);
			else if (obj is uint)		this.writer.WriteValue((uint)obj);
			else if (obj is long)		this.writer.WriteValue((long)obj);
			else if (obj is ulong)		this.writer.WriteValue((decimal)(ulong)obj);
			else if (obj is float)		this.writer.WriteValue((float)obj);
			else if (obj is double)		this.writer.WriteValue((double)obj);
			else if (obj is decimal)	this.writer.WriteValue((decimal)obj);
			else if (obj == null)
				throw new ArgumentNullException("obj");
			else
				throw new ArgumentException(string.Format("Type '{0}' is not a primitive.", obj.GetType()));
		}


		protected override void BeginReadOperation()
		{
			base.BeginReadOperation();
			this.CreateReader();
			this.reader.Read();
			this.reader.MoveToContent();
		}
		protected override void EndReadOperation()
		{
			base.EndReadOperation();
			this.DisposeReader();
		}
		protected override void BeginWriteOperation()
		{
			base.BeginWriteOperation();
			this.CreateWriter();
			this.writer.WriteStartElement("root");
		}
		protected override void EndWriteOperation()
		{
			base.EndWriteOperation();
			this.writer.WriteEndDocument();
			this.writer.Flush();
			this.DisposeWriter();

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
			byte[] reducedData = Encoding.Default.GetBytes(reducedDoc);
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
