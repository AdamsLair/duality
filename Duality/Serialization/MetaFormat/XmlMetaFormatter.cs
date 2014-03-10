using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Duality.Serialization.MetaFormat;

namespace Duality.Serialization.MetaFormat
{
	/// <summary>
	/// De/Serializes abstract object data using <see cref="Duality.Serialization.MetaFormat.DataNode">DataNodes</see> instead of the object itsself.
	/// </summary>
	/// <seealso cref="Duality.Serialization.XmlFormatter"/>
	public class XmlMetaFormatter : XmlFormatterBase
	{
		public XmlMetaFormatter(Stream stream) : base(stream) {}

		protected override ObjectHeader PrepareWriteObject(object obj)
		{
			DataNode node = obj as DataNode;
			if (node == null) throw new InvalidOperationException("The XmlMetaFormatter can't serialize objects that do not derive from DataNode");
			return new ObjectHeader(node);
		}
		protected override void WriteObjectBody(XElement element, object obj, ObjectHeader header)
		{
			if (header.IsPrimitive)							this.WritePrimitive	(element, (obj as PrimitiveNode).PrimitiveValue);
			else if (header.DataType == DataType.Enum)		this.WriteEnum		(element, obj as EnumNode);
			else if (header.DataType == DataType.Struct)	this.WriteStruct	(element, obj as StructNode);
			else if (header.DataType == DataType.ObjectRef)	element.Value = XmlConvert.ToString((obj as ObjectRefNode).ObjRefId);
			else if	(header.DataType == DataType.Array)		this.WriteArray		(element, obj as ArrayNode);
			else if (header.DataType == DataType.Delegate)	this.WriteDelegate	(element, obj as DelegateNode);
			else if (header.DataType.IsMemberInfoType())	this.WriteMemberInfo(element, obj as MemberInfoNode);
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.MemberInfoNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected void WriteMemberInfo(XElement element, MemberInfoNode node)
		{
			element.SetAttributeValue("value", node.TypeString);
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.ArrayNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected void WriteArray(XElement element, ArrayNode node)
		{
			if (node.Rank != 1) throw new ArgumentException("Non single-Rank arrays are not supported");
			
			if (node.PrimitiveData != null)
			{
				Array objAsArray = node.PrimitiveData;
				if (objAsArray is byte[])
				{
					byte[] byteArr = objAsArray as byte[];
					element.Value = EncodeByteArray(byteArr);
				}
				else
				{
					SerializeType elemType = objAsArray.GetType().GetElementType().GetSerializeType();
					int nonDefaultElementCount = this.GetArrayNonDefaultElementCount(objAsArray, elemType.Type);
					for (long l = 0; l < nonDefaultElementCount; l++)
					{
						XElement itemElement = new XElement("item");
						element.Add(itemElement);
						this.WriteObjectData(itemElement, new PrimitiveNode(elemType.DataType, objAsArray.GetValue(l)));
					}

					// Write original length, in case trailing elements were omitted.
					if (nonDefaultElementCount != objAsArray.Length)
					{
						element.SetAttributeValue("length", XmlConvert.ToString(node.PrimitiveData.Length));
					}
				}
			}
			else
			{
				foreach (DataNode subNode in node.SubNodes)
				{
					XElement itemElement = new XElement("item");
					element.Add(itemElement);
					this.WriteObjectData(itemElement, subNode);
				}

				// Write original length, in case trailing elements were omitted.
				if (node.SubNodes.Count() != node.Length)
				{
					element.SetAttributeValue("length", XmlConvert.ToString(node.Length));
				}
			}
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.StructNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected void WriteStruct(XElement element, StructNode node)
		{
			// Write the structs data type
			if (node.CustomSerialization)		element.SetAttributeValue("custom", XmlConvert.ToString(true));
			if (node.SurrogateSerialization)	element.SetAttributeValue("surrogate", XmlConvert.ToString(true));

			if (node.SurrogateSerialization)
			{
				CustomSerialIO customIO = new CustomSerialIO();
				DummyNode surrogateConstructor = node.SubNodes.FirstOrDefault() as DummyNode;
				if (surrogateConstructor != null)
				{
					var enumerator = surrogateConstructor.SubNodes.GetEnumerator();
					while (enumerator.MoveNext())
					{
						PrimitiveNode key = enumerator.Current as PrimitiveNode;
						if (enumerator.MoveNext() && key != null)
						{
							DataNode value = enumerator.Current;
							customIO.WriteValue(key.PrimitiveValue as string, value);
						}
					}
				}

				XElement customHeaderElement = new XElement(CustomSerialIO.HeaderElement);
				element.Add(customHeaderElement);
				customIO.Serialize(this, customHeaderElement);
			}

			if (node.CustomSerialization || node.SurrogateSerialization)
			{
				CustomSerialIO customIO = new CustomSerialIO();
				var enumerator = node.SubNodes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					PrimitiveNode key = enumerator.Current as PrimitiveNode;
					if (key != null && enumerator.MoveNext())
					{
						DataNode value = enumerator.Current;
						customIO.WriteValue(key.PrimitiveValue as string, value);
					}
				}

				XElement customBodyElement = new XElement(CustomSerialIO.BodyElement);
				element.Add(customBodyElement);
				customIO.Serialize(this, customBodyElement);
			}
			else
			{
				// Write the structs fields
				foreach (DataNode subNode in node.SubNodes)
				{
					if (subNode is DummyNode) continue;
					if (subNode is TypeDataLayoutNode) continue;

					XElement fieldElement = new XElement(GetXmlElementName(subNode.Name));
					element.Add(fieldElement);

					this.WriteObjectData(fieldElement, subNode);
				}
			}
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.DelegateNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected void WriteDelegate(XElement element, DelegateNode node)
		{
			// Write the delegates type
			if (node.InvokeList != null) element.SetAttributeValue("multi", XmlConvert.ToString(true));
			
			XElement methodElement = new XElement("method");
			XElement targetElement = new XElement("target");
			element.Add(methodElement);
			element.Add(targetElement);

			this.WriteObjectData(methodElement, node.Method);
			this.WriteObjectData(targetElement, node.Target);

			if (node.InvokeList != null)
			{
				XElement invocationListElement = new XElement("invocationList");
				element.Add(invocationListElement);
				this.WriteObjectData(invocationListElement, node.InvokeList);
			}
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.EnumNode"/>.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected void WriteEnum(XElement element, EnumNode node)
		{
			element.SetAttributeValue("name", node.ValueName);
			element.SetAttributeValue("value", XmlConvert.ToString(node.Value));
		}
		
		protected override object GetNullObject()
		{
			return new PrimitiveNode(DataType.Unknown, null);
		}
		protected override ObjectHeader ParseObjectHeader(uint objId, DataType dataType, string typeString)
		{
			return new ObjectHeader(objId, dataType, typeString);
		}
		protected override object ReadObjectBody(XElement element, ObjectHeader header)
		{
			DataNode result = null;

			if (header.IsPrimitive)							result = new PrimitiveNode(header.DataType, this.ReadPrimitive(element, header));
			else if (header.DataType == DataType.Enum)		result = this.ReadEnum(element, header);
			else if (header.DataType == DataType.Struct)	result = this.ReadStruct(element, header);
			else if (header.DataType == DataType.ObjectRef)	result = this.ReadObjectRef(element);
			else if (header.DataType == DataType.Array)		result = this.ReadArray(element, header);
			else if (header.DataType == DataType.Delegate)	result = this.ReadDelegate(element, header);
			else if (header.DataType.IsMemberInfoType())	result = this.ReadMemberInfo(element, header);

			return result;
		}
		/// <summary>
		/// Reads a <see cref="Duality.Serialization.MetaFormat.MemberInfoNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected MemberInfoNode ReadMemberInfo(XElement element, ObjectHeader header)
		{
			string typeString = element.GetAttributeValue("value");
			MemberInfoNode result = new MemberInfoNode(header.DataType, typeString, header.ObjectId);
			
			// Prepare object reference
			this.idManager.Inject(result, header.ObjectId);

			return result;
		}
		/// <summary>
		/// Reads an <see cref="Duality.Serialization.MetaFormat.ArrayNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected ArrayNode ReadArray(XElement element, ObjectHeader header)
		{
			string	arrLengthString	= element.GetAttributeValue("length");
			int		arrLength		= arrLengthString == null ? element.Elements().Count() : XmlConvert.ToInt32(arrLengthString);
			Type	arrType			= ReflectionHelper.ResolveType(header.TypeString, false);

			ArrayNode result = new ArrayNode(header.TypeString, header.ObjectId, 1, arrLength);
			
			// Prepare object reference
			this.idManager.Inject(result, header.ObjectId);

			// Store primitive data block
			bool nonPrimitive;
			if (arrType != null)
			{
				Array arrObj = Array.CreateInstance(arrType.GetElementType(), arrLength);

				if (arrObj is bool[] || 
					arrObj is byte[] || arrObj is sbyte[] || 
					arrObj is short[] || arrObj is ushort[] || 
					arrObj is int[] || arrObj is uint[] || 
					arrObj is long[] || arrObj is ulong[] || 
					arrObj is float[] || arrObj is double[] || 
					arrObj is decimal[] || 
					arrObj is char[] || arrObj is string[])
					nonPrimitive = false;
				else
					nonPrimitive = true;

				if (!nonPrimitive)
				{
					if (arrObj is byte[])
					{
						byte[] byteArr = arrObj as byte[];
						string binHexString = element.Value;
						byte[] byteArr2 = DecodeByteArray(binHexString);
						for (int l = 0; l < arrLength; l++)
							byteArr[l] = byteArr2[l];
					}
					else
					{
						int itemIndex = 0;
						foreach (XElement itemElement in element.Elements())
						{
							PrimitiveNode elemNode = this.ReadObjectData(itemElement) as PrimitiveNode;
							if (arrObj != null)
							{
								arrObj.SetValue(elemNode.PrimitiveValue, itemIndex);
							}

							itemIndex++;
							if (itemIndex >= arrLength) break;
						}
					}
					result.PrimitiveData = arrObj;
				}
			}
			else
				nonPrimitive = true;

			// Store other data as sub-nodes
			if (nonPrimitive)
			{
				int itemIndex = 0;
				foreach (XElement itemElement in element.Elements())
				{
					DataNode child = this.ReadObjectData(itemElement) as DataNode;
					child.Parent = result;

					itemIndex++;
					if (itemIndex >= arrLength) break;
				}
			}

			return result;
		}
		/// <summary>
		/// Reads a <see cref="Duality.Serialization.MetaFormat.StructNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected StructNode ReadStruct(XElement element, ObjectHeader header)
		{
			// Read struct type
			string	customString	= element.GetAttributeValue("custom");
			string	surrogateString	= element.GetAttributeValue("surrogate");
			bool	custom			= customString != null && XmlConvert.ToBoolean(customString);
			bool	surrogate		= surrogateString != null && XmlConvert.ToBoolean(surrogateString);

			StructNode result = new StructNode(header.TypeString, header.ObjectId, custom, surrogate);
			
			// Read surrogate constructor data
			if (surrogate)
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
				if (customIO.Data.Any())
				{
					DummyNode surrogateConstructor = new DummyNode();
					surrogateConstructor.Parent = result;
					foreach (var pair in customIO.Data)
					{
						PrimitiveNode key = new PrimitiveNode(DataType.String, pair.Key);
						DataNode value = pair.Value as DataNode;
						key.Parent = surrogateConstructor;
						value.Parent = surrogateConstructor;
					}
				}
			}

			// Prepare object reference
			this.idManager.Inject(result, header.ObjectId);
			
			// Read custom object data
			if (custom)
			{
				CustomSerialIO customIO = new CustomSerialIO();
				XElement customBodyElement = element.Element(CustomSerialIO.BodyElement) ?? element.Elements().ElementAtOrDefault(1);
				if (customBodyElement != null)
				{
					customIO.Deserialize(this, customBodyElement);
				}
				foreach (var pair in customIO.Data)
				{
					PrimitiveNode key = new PrimitiveNode(DataType.String, pair.Key);
					DataNode value = pair.Value as DataNode;
					key.Parent = result;
					value.Parent = result;
				}
			}
			// Red non-custom object data
			else if (!element.IsEmpty)
			{
				// Read fields
				DataNode fieldValue;
				foreach (XElement fieldElement in element.Elements())
				{
					fieldValue = this.ReadObjectData(fieldElement) as DataNode;
					fieldValue.Name = GetCodeElementName(fieldElement.Name.LocalName);
					fieldValue.Parent = result;
				}
			}

			return result;
		}
		/// <summary>
		/// Reads a <see cref="Duality.Serialization.MetaFormat.DelegateNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected DelegateNode ReadDelegate(XElement element, ObjectHeader header)
		{
			string multiString= element.GetAttributeValue("multi");
			bool multi = multiString != null && XmlConvert.ToBoolean(multiString);
			
			XElement methodElement = element.Element("method") ?? element.Elements().FirstOrDefault();
			XElement targetElement = element.Element("target") ?? element.Elements().ElementAtOrDefault(1);
			XElement invocationListElement = element.Element("invocationList") ?? element.Elements().ElementAtOrDefault(2);

			DataNode method	= this.ReadObjectData(methodElement) as DataNode;
			DataNode target	= null;

			// Create the delegate without target and fix it later, so we don't load its target object before setting this object id
			DelegateNode result = new DelegateNode(header.TypeString, header.ObjectId, method, target, null);

			// Prepare object reference
			this.idManager.Inject(result, header.ObjectId);

			// Load & fix the target object
			target = this.ReadObjectData(targetElement) as DataNode;
			target.Parent = result;
			result.Target = target;

			// Combine multicast delegates
			if (multi)
			{
				DataNode invokeList = this.ReadObjectData(invocationListElement) as DataNode;
				result.InvokeList = invokeList;
			}

			return result;
		}
		/// <summary>
		/// Reads an <see cref="Duality.Serialization.MetaFormat.EnumNode"/>.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected EnumNode ReadEnum(XElement element, ObjectHeader header)
		{
			string name = element.GetAttributeValue("name");
			string valueString = element.GetAttributeValue("value");
			long val = valueString == null ? 0 : XmlConvert.ToInt64(valueString);
			return new EnumNode(header.TypeString, name, val);
		}
		/// <summary>
		/// Reads an <see cref="Duality.Serialization.MetaFormat.ObjectRefNode"/>.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected ObjectRefNode ReadObjectRef(XElement element)
		{
			uint objId = XmlConvert.ToUInt32(element.Value);
			ObjectRefNode result = new ObjectRefNode(objId);
			return result;
		}
	}
}
