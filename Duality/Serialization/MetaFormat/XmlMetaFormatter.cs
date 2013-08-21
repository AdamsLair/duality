using System;
using System.Linq;
using System.Xml;
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
		public XmlMetaFormatter() {}
		public XmlMetaFormatter(Stream stream) : base(stream) {}
		
		protected override void GetWriteObjectData(object obj, out SerializeType objSerializeType, out DataType dataType, out uint objId)
		{
			DataNode node = obj as DataNode;
			if (node == null) throw new InvalidOperationException("The XmlMetaFormatter can't serialize objects that do not derive from DataNode");

			objSerializeType = null;
			objId = 0;
			dataType = node.NodeType;

			if		(node is ObjectNode)	objId = (node as ObjectNode).ObjId;
			else if (node is ObjectRefNode) objId = (node as ObjectRefNode).ObjRefId;
		}
		protected override void WriteObjectBody(DataType dataType, object obj, SerializeType objSerializeType, uint objId)
		{
			if (dataType.IsPrimitiveType())				this.WritePrimitive((obj as PrimitiveNode).PrimitiveValue);
			else if (dataType == DataType.String)		this.writer.WriteString((obj as StringNode).StringValue);
			else if (dataType == DataType.Enum)			this.WriteEnum(obj as EnumNode);
			else if (dataType == DataType.Struct)		this.WriteStruct(obj as StructNode);
			else if (dataType == DataType.ObjectRef)	this.writer.WriteValue((obj as ObjectRefNode).ObjRefId);
			else if	(dataType == DataType.Array)		this.WriteArray(obj as ArrayNode);
			else if (dataType == DataType.Class)		this.WriteStruct(obj as StructNode);
			else if (dataType == DataType.Delegate)		this.WriteDelegate(obj as DelegateNode);
			else if (dataType.IsMemberInfoType())		this.WriteMemberInfo(obj as MemberInfoNode);
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.MemberInfoNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected void WriteMemberInfo(MemberInfoNode node)
		{
			if (node == null) throw new ArgumentNullException("node");

			if (node.ObjId != 0) this.writer.WriteAttributeString("id", XmlConvert.ToString(node.ObjId));
			this.writer.WriteAttributeString("value", node.TypeString);
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.ArrayNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected void WriteArray(ArrayNode node)
		{
			if (node.Rank != 1) throw new ArgumentException("Non single-Rank arrays are not supported");
			
			this.writer.WriteAttributeString("type", node.TypeString);
			if (node.ObjId != 0) this.writer.WriteAttributeString("id", XmlConvert.ToString(node.ObjId));
			if (node.Rank != 1) this.writer.WriteAttributeString("rank", XmlConvert.ToString(node.Rank));

			
			if (node.PrimitiveData != null)
			{
				this.writer.WriteAttributeString("length", XmlConvert.ToString(node.PrimitiveData.Length));
				Array objAsArray = node.PrimitiveData;
				if (objAsArray is byte[])
				{
					byte[] byteArr = objAsArray as byte[];
					this.writer.WriteString(this.ByteArrayToString(byteArr));
				}
				else
				{
					SerializeType elemType = objAsArray.GetType().GetElementType().GetSerializeType();
					for (long l = 0; l < objAsArray.Length; l++)
						this.WriteObject(new PrimitiveNode(elemType.DataType, objAsArray.GetValue(l)));
				}
			}
			else
			{
				this.writer.WriteAttributeString("length", XmlConvert.ToString(node.SubNodes.Count()));
				foreach (DataNode subNode in node.SubNodes)
					this.WriteObject(subNode);
			}
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.StructNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected void WriteStruct(StructNode node)
		{
			// Write the structs data type
			this.writer.WriteAttributeString("type", node.TypeString);
			if (node.ObjId != 0) this.writer.WriteAttributeString("id", XmlConvert.ToString(node.ObjId));
			if (node.CustomSerialization) this.writer.WriteAttributeString("custom", XmlConvert.ToString(true));
			if (node.SurrogateSerialization) this.writer.WriteAttributeString("surrogate", XmlConvert.ToString(true));

			if (node.SurrogateSerialization)
			{
				CustomSerialIO customIO = new CustomSerialIO(CustomSerialIO.HeaderElement);
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
				customIO.Serialize(this);
			}

			if (node.CustomSerialization || node.SurrogateSerialization)
			{
				CustomSerialIO customIO = new CustomSerialIO(CustomSerialIO.BodyElement);
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
				customIO.Serialize(this);
			}
			else
			{
				// Write the structs fields
				foreach (DataNode subNode in node.SubNodes)
				{
					if (subNode is DummyNode) continue;
					if (subNode is TypeDataLayoutNode) continue;
					this.WriteObject(subNode, subNode.Name);
				}
			}
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.DelegateNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected void WriteDelegate(DelegateNode node)
		{
			// Write the delegates type
			this.writer.WriteAttributeString("type", node.TypeString);
			if (node.ObjId != 0) this.writer.WriteAttributeString("id", XmlConvert.ToString(node.ObjId));
			if (node.InvokeList != null) this.writer.WriteAttributeString("multi", XmlConvert.ToString(true));

			this.WriteObject(node.Method);
			this.WriteObject(node.Target);
			if (node.InvokeList != null) this.WriteObject(node.InvokeList);
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.EnumNode"/>.
		/// </summary>
		/// <param name="node"></param>
		protected void WriteEnum(EnumNode node)
		{
			this.writer.WriteAttributeString("type", node.EnumType);
			this.writer.WriteAttributeString("name", node.ValueName);
			this.writer.WriteAttributeString("value", XmlConvert.ToString(node.Value));
		}
		
		protected override object GetNullObject()
		{
			return new PrimitiveNode(DataType.Unknown, null);
		}
		protected override object ReadObjectBody(DataType dataType)
		{
			DataNode result = null;

			if (dataType.IsPrimitiveType())				result = new PrimitiveNode(dataType, this.ReadPrimitive(dataType));
			else if (dataType == DataType.String)		result = new StringNode(this.reader.ReadString());
			else if (dataType == DataType.Enum)			result = this.ReadEnum();
			else if (dataType == DataType.Struct)		result = this.ReadStruct(false);
			else if (dataType == DataType.ObjectRef)	result = this.ReadObjectRef();
			else if (dataType == DataType.Array)		result = this.ReadArray();
			else if (dataType == DataType.Class)		result = this.ReadStruct(true);
			else if (dataType == DataType.Delegate)		result = this.ReadDelegate();
			else if (dataType.IsMemberInfoType())		result = this.ReadMemberInfo(dataType);

			return result;
		}
		/// <summary>
		/// Reads a <see cref="Duality.Serialization.MetaFormat.MemberInfoNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected MemberInfoNode ReadMemberInfo(DataType dataType)
		{
			string	objIdString		= this.reader.GetAttribute("id");
			uint	objId			= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);

			string typeString = this.reader.GetAttribute("value");
			MemberInfoNode result = new MemberInfoNode(dataType, typeString, objId);
			
			// Prepare object reference
			this.idManager.Inject(result, objId);

			return result;
		}
		/// <summary>
		/// Reads an <see cref="Duality.Serialization.MetaFormat.ArrayNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected ArrayNode ReadArray()
		{
			string	arrTypeString	= this.reader.GetAttribute("type");
			string	objIdString		= this.reader.GetAttribute("id");
			string	arrRankString	= this.reader.GetAttribute("rank");
			string	arrLengthString	= this.reader.GetAttribute("length");
			uint	objId			= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			int		arrRank			= arrRankString == null ? 1 : XmlConvert.ToInt32(arrRankString);
			int		arrLength		= arrLengthString == null ? 0 : XmlConvert.ToInt32(arrLengthString);
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
						string binHexString = this.reader.ReadString();
						byte[] byteArr2 = this.StringToByteArray(binHexString);
						for (int l = 0; l < arrLength; l++)
							byteArr[l] = byteArr2[l];
					}
					else
					{
						for (int l = 0; l < arrLength; l++)
						{
							PrimitiveNode elemNode = this.ReadObject() as PrimitiveNode;
							if (arrObj != null) arrObj.SetValue(elemNode.PrimitiveValue, l);
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
				for (int i = 0; i < arrLength; i++)
				{
					DataNode child = this.ReadObject() as DataNode;
					child.Parent = result;
				}
			}

			return result;
		}
		/// <summary>
		/// Reads a <see cref="Duality.Serialization.MetaFormat.StructNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected StructNode ReadStruct(bool classType)
		{
			// Read struct type
			string	objTypeString	= this.reader.GetAttribute("type");
			string	objIdString		= this.reader.GetAttribute("id");
			string	customString	= this.reader.GetAttribute("custom");
			string	surrogateString	= this.reader.GetAttribute("surrogate");
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

				CustomSerialIO customIO = new CustomSerialIO(CustomSerialIO.HeaderElement);
				customIO.Deserialize(this);
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
				CustomSerialIO customIO = new CustomSerialIO(CustomSerialIO.BodyElement);
				customIO.Deserialize(this);
				foreach (var pair in customIO.Data)
				{
					StringNode key = new StringNode(pair.Key);
					DataNode value = pair.Value as DataNode;
					key.Parent = result;
					value.Parent = result;
				}
			}
			// Red non-custom object data
			else if (!this.reader.IsEmptyElement)
			{
				// Read fields
				bool scopeChanged;
				string fieldName;
				DataNode fieldValue;
				while (true)
				{
					fieldValue = this.ReadObject(out fieldName, out scopeChanged) as DataNode;
					if (scopeChanged) break;
					else
					{
						fieldValue.Name = fieldName;
						fieldValue.Parent = result;
					}
				}
			}

			return result;
		}
		/// <summary>
		/// Reads a <see cref="Duality.Serialization.MetaFormat.DelegateNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected DelegateNode ReadDelegate()
		{
			string	delegateTypeString	= this.reader.GetAttribute("type");
			string	objIdString			= this.reader.GetAttribute("id");
			string	multiString			= this.reader.GetAttribute("multi");
			uint	objId				= objIdString == null ? 0 : XmlConvert.ToUInt32(objIdString);
			bool	multi				= objIdString != null && XmlConvert.ToBoolean(multiString);

			DataNode method	= this.ReadObject() as DataNode;
			DataNode target	= null;

			// Create the delegate without target and fix it later, so we don't load its target object before setting this object id
			DelegateNode result = new DelegateNode(delegateTypeString, objId, method, target, null);

			// Prepare object reference
			this.idManager.Inject(result, objId);

			// Load & fix the target object
			target = this.ReadObject() as DataNode;
			target.Parent = result;
			result.Target = target;

			// Combine multicast delegates
			if (multi)
			{
				DataNode invokeList = this.ReadObject() as DataNode;
				result.InvokeList = invokeList;
			}

			return result;
		}
		/// <summary>
		/// Reads an <see cref="Duality.Serialization.MetaFormat.EnumNode"/>.
		/// </summary>
		/// <param name="node"></param>
		protected EnumNode ReadEnum()
		{
			string typeName		= this.reader.GetAttribute("type");
			string name			= this.reader.GetAttribute("name");
			string valueString	= this.reader.GetAttribute("value");
			long val = valueString == null ? 0 : XmlConvert.ToInt64(valueString);
			return new EnumNode(typeName, name, val);
		}
		/// <summary>
		/// Reads an <see cref="Duality.Serialization.MetaFormat.ObjectRefNode"/>.
		/// </summary>
		/// <param name="node"></param>
		protected ObjectRefNode ReadObjectRef()
		{
			uint objId = XmlConvert.ToUInt32(this.reader.ReadString());
			ObjectRefNode result = new ObjectRefNode(objId);
			return result;
		}
	}
}
