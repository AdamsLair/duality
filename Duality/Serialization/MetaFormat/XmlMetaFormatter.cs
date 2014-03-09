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

		protected override void PrepareWriteObject(object obj, out ObjectHeader header)
		{
			DataNode node = obj as DataNode;
			if (node == null) throw new InvalidOperationException("The XmlMetaFormatter can't serialize objects that do not derive from DataNode");
			
			uint objId = 0;
			if (node is ObjectNode) objId = (node as ObjectNode).ObjId;
			else if (node is ObjectRefNode) objId = (node as ObjectRefNode).ObjRefId;

			header = new ObjectHeader(objId, node.NodeType, null);
		}
		protected override void WriteObjectBody(XElement element, object obj, ObjectHeader header)
		{
			if (header.IsPrimitive)							this.WritePrimitive	(element, (obj as PrimitiveNode).PrimitiveValue);
			else if (header.DataType == DataType.String)	element.Value = (obj as StringNode).StringValue;
			else if (header.DataType == DataType.Enum)		this.WriteEnum		(element, obj as EnumNode);
			else if (header.DataType == DataType.Struct)	this.WriteStruct	(element, obj as StructNode);
			else if (header.DataType == DataType.ObjectRef)	element.Value = XmlConvert.ToString((obj as ObjectRefNode).ObjRefId);
			else if	(header.DataType == DataType.Array)		this.WriteArray		(element, obj as ArrayNode);
			else if (header.DataType == DataType.Class)		this.WriteStruct	(element, obj as StructNode);
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
			if (node == null) throw new ArgumentNullException("node");

			if (node.ObjId != 0)	element.SetAttributeValue("id", XmlConvert.ToString(node.ObjId));
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
			
			element.SetAttributeValue("type", node.TypeString);
			if (node.ObjId != 0)	element.SetAttributeValue("id", XmlConvert.ToString(node.ObjId));
			if (node.Rank != 1)		element.SetAttributeValue("rank", XmlConvert.ToString(node.Rank));

			
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
					if (elemType.DataType == DataType.String)
					{
						for (long l = 0; l < nonDefaultElementCount; l++)
						{
							XElement itemElement = new XElement("item");
							element.Add(itemElement);
							string str = (string)objAsArray.GetValue(l);
							this.WriteObjectData(itemElement, str != null ? new StringNode(str) : this.GetNullObject());
						}
					}
					else
					{
						for (long l = 0; l < nonDefaultElementCount; l++)
						{
							XElement itemElement = new XElement("item");
							element.Add(itemElement);
							this.WriteObjectData(itemElement, new PrimitiveNode(elemType.DataType, objAsArray.GetValue(l)));
						}
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
												element.SetAttributeValue("type", node.TypeString);
			if (node.ObjId != 0)				element.SetAttributeValue("id", XmlConvert.ToString(node.ObjId));
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
						StringNode key = enumerator.Current as StringNode;
						if (enumerator.MoveNext() && key != null)
						{
							DataNode value = enumerator.Current;
							customIO.WriteValue(key.StringValue, value);
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
					StringNode key = enumerator.Current as StringNode;
					if (key != null && enumerator.MoveNext())
					{
						DataNode value = enumerator.Current;
						customIO.WriteValue(key.StringValue, value);
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
											element.SetAttributeValue("type", node.TypeString);
			if (node.ObjId != 0)			element.SetAttributeValue("id", XmlConvert.ToString(node.ObjId));
			if (node.InvokeList != null)	element.SetAttributeValue("multi", XmlConvert.ToString(true));
			
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
			element.SetAttributeValue("type", node.EnumType);
			element.SetAttributeValue("name", node.ValueName);
			element.SetAttributeValue("value", XmlConvert.ToString(node.Value));
		}
		
		protected override object GetNullObject()
		{
			return new PrimitiveNode(DataType.Unknown, null);
		}
		protected override object ReadObjectBody(XElement element, DataType dataType)
		{
			DataNode result = null;

			if (dataType.IsPrimitiveType())				result = new PrimitiveNode(dataType, this.ReadPrimitive(element, dataType));
			else if (dataType == DataType.String)		result = new StringNode(element.Value);
			else if (dataType == DataType.Enum)			result = this.ReadEnum(element);
			else if (dataType == DataType.Struct)		result = this.ReadStruct(element, false);
			else if (dataType == DataType.ObjectRef)	result = this.ReadObjectRef(element);
			else if (dataType == DataType.Array)		result = this.ReadArray(element);
			else if (dataType == DataType.Class)		result = this.ReadStruct(element, true);
			else if (dataType == DataType.Delegate)		result = this.ReadDelegate(element);
			else if (dataType.IsMemberInfoType())		result = this.ReadMemberInfo(element, dataType);

			return result;
		}
		/// <summary>
		/// Reads a <see cref="Duality.Serialization.MetaFormat.MemberInfoNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected MemberInfoNode ReadMemberInfo(XElement element, DataType dataType)
		{
			string	objIdString		= element.GetAttributeValue("id");
			uint	objId			= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);

			string typeString = element.GetAttributeValue("value");
			MemberInfoNode result = new MemberInfoNode(dataType, typeString, objId);
			
			// Prepare object reference
			this.idManager.Inject(result, objId);

			return result;
		}
		/// <summary>
		/// Reads an <see cref="Duality.Serialization.MetaFormat.ArrayNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="node"></param>
		protected ArrayNode ReadArray(XElement element)
		{
			string	arrTypeString	= element.GetAttributeValue("type");
			string	objIdString		= element.GetAttributeValue("id");
			string	arrRankString	= element.GetAttributeValue("rank");
			string	arrLengthString	= element.GetAttributeValue("length");
			uint	objId			= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			int		arrRank			= arrRankString == null ? 1 : XmlConvert.ToInt32(arrRankString);
			int		arrLength		= arrLengthString == null ? element.Elements().Count() : XmlConvert.ToInt32(arrLengthString);
			Type	arrType			= ReflectionHelper.ResolveType(arrTypeString, false);

			ArrayNode result = new ArrayNode(arrTypeString, objId, arrRank, arrLength);
			
			// Prepare object reference
			this.idManager.Inject(result, objId);

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
							DataNode elemNode = this.ReadObjectData(itemElement) as DataNode;
							if (arrObj != null)
							{
								if (elemNode is PrimitiveNode)
									arrObj.SetValue((elemNode as PrimitiveNode).PrimitiveValue, itemIndex);
								else if (elemNode is StringNode)
									arrObj.SetValue((elemNode as StringNode).StringValue, itemIndex);
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
		protected StructNode ReadStruct(XElement element, bool classType)
		{
			// Read struct type
			string	objTypeString	= element.GetAttributeValue("type");
			string	objIdString		= element.GetAttributeValue("id");
			string	customString	= element.GetAttributeValue("custom");
			string	surrogateString	= element.GetAttributeValue("surrogate");
			uint	objId			= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			bool	custom			= customString != null && XmlConvert.ToBoolean(customString);
			bool	surrogate		= surrogateString != null && XmlConvert.ToBoolean(surrogateString);

			StructNode result = new StructNode(classType, objTypeString, objId, custom, surrogate);
			
			// Read surrogate constructor data
			if (surrogate)
			{
				custom = true;

				// Set fake object reference for surrogate constructor: No self-references allowed here.
				this.idManager.Inject(null, objId);
				
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
						StringNode key = new StringNode(pair.Key);
						DataNode value = pair.Value as DataNode;
						key.Parent = surrogateConstructor;
						value.Parent = surrogateConstructor;
					}
				}
			}

			// Prepare object reference
			this.idManager.Inject(result, objId);
			
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
					StringNode key = new StringNode(pair.Key);
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
		protected DelegateNode ReadDelegate(XElement element)
		{
			string	delegateTypeString	= element.GetAttributeValue("type");
			string	objIdString			= element.GetAttributeValue("id");
			string	multiString			= element.GetAttributeValue("multi");
			uint	objId				= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			bool	multi				= objIdString != null && XmlConvert.ToBoolean(multiString);
			
			XElement methodElement = element.Element("method") ?? element.Elements().FirstOrDefault();
			XElement targetElement = element.Element("target") ?? element.Elements().ElementAtOrDefault(1);
			XElement invocationListElement = element.Element("invocationList") ?? element.Elements().ElementAtOrDefault(2);

			DataNode method	= this.ReadObjectData(methodElement) as DataNode;
			DataNode target	= null;

			// Create the delegate without target and fix it later, so we don't load its target object before setting this object id
			DelegateNode result = new DelegateNode(delegateTypeString, objId, method, target, null);

			// Prepare object reference
			this.idManager.Inject(result, objId);

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
		protected EnumNode ReadEnum(XElement element)
		{
			string typeName		= element.GetAttributeValue("type");
			string name			= element.GetAttributeValue("name");
			string valueString	= element.GetAttributeValue("value");
			long val = valueString == null ? 0 : XmlConvert.ToInt64(valueString);
			return new EnumNode(typeName, name, val);
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
