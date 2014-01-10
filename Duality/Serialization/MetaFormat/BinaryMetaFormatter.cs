using System;
using System.Linq;
using System.IO;
using Duality.Serialization.MetaFormat;

namespace Duality.Serialization.MetaFormat
{
	/// <summary>
	/// De/Serializes abstract object data using <see cref="Duality.Serialization.MetaFormat.DataNode">DataNodes</see> instead of the object itsself.
	/// </summary>
	/// <seealso cref="Duality.Serialization.BinaryFormatter"/>
	public class BinaryMetaFormatter : BinaryFormatterBase
	{
		public BinaryMetaFormatter(Stream stream) : base(stream) {}
		
		protected override void GetWriteObjectData(object obj, out SerializeType objSerializeType, out DataType dataType, out uint objId)
		{
			DataNode node = obj as DataNode;
			if (node == null) throw new InvalidOperationException("The BinaryMetaFormatter can't serialize objects that do not derive from DataNode");

			objSerializeType = null;
			objId = 0;
			dataType = node.NodeType;

			if		(node is ObjectNode)	objId = (node as ObjectNode).ObjId;
			else if (node is ObjectRefNode) objId = (node as ObjectRefNode).ObjRefId;
		}
		protected override void WriteObjectBody(DataType dataType, object obj, SerializeType objSerializeType, uint objId)
		{
			if (dataType.IsPrimitiveType())				this.WritePrimitive((obj as PrimitiveNode).PrimitiveValue);
			else if (dataType == DataType.String)		this.WriteString((obj as StringNode).StringValue);
			else if (dataType == DataType.Enum)			this.WriteEnum(obj as EnumNode);
			else if (dataType == DataType.Struct)		this.WriteStruct(obj as StructNode);
			else if (dataType == DataType.ObjectRef)	this.writer.Write((obj as ObjectRefNode).ObjRefId);
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

			this.writer.Write(node.ObjId);
			this.writer.Write(node.TypeString);
		}
		/// <summary>
		/// Writes the specified <see cref="Duality.Serialization.MetaFormat.ArrayNode"/>, including possible child nodes.
		/// </summary>
		/// <param name="node"></param>
		protected void WriteArray(ArrayNode node)
		{
			if (node.Rank != 1) throw new ArgumentException("Non single-Rank arrays are not supported");

			this.writer.Write(node.TypeString);
			this.writer.Write(node.ObjId);
			this.writer.Write(node.Rank);

			
			if (node.PrimitiveData != null)
			{
				this.writer.Write(node.PrimitiveData.Length);
				Array objAsArray = node.PrimitiveData;
				if		(objAsArray is bool[])		this.WriteArrayData(objAsArray as bool[]);
				else if (objAsArray is byte[])		this.WriteArrayData(objAsArray as byte[]);
				else if (objAsArray is sbyte[])		this.WriteArrayData(objAsArray as sbyte[]);
				else if (objAsArray is short[])		this.WriteArrayData(objAsArray as short[]);
				else if (objAsArray is ushort[])	this.WriteArrayData(objAsArray as ushort[]);
				else if (objAsArray is int[])		this.WriteArrayData(objAsArray as int[]);
				else if (objAsArray is uint[])		this.WriteArrayData(objAsArray as uint[]);
				else if (objAsArray is long[])		this.WriteArrayData(objAsArray as long[]);
				else if (objAsArray is ulong[])		this.WriteArrayData(objAsArray as ulong[]);
				else if (objAsArray is float[])		this.WriteArrayData(objAsArray as float[]);
				else if (objAsArray is double[])	this.WriteArrayData(objAsArray as double[]);
				else if (objAsArray is decimal[])	this.WriteArrayData(objAsArray as decimal[]);
				else if (objAsArray is char[])		this.WriteArrayData(objAsArray as char[]);
				else if (objAsArray is string[])	this.WriteArrayData(objAsArray as string[]);
			}
			else
			{
				this.writer.Write(node.SubNodes.Count());
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
			this.writer.Write(node.TypeString);
			this.writer.Write(node.ObjId);
			this.writer.Write(node.CustomSerialization);
			this.writer.Write(node.SurrogateSerialization);

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
				customIO.Serialize(this);
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
				customIO.Serialize(this);
			}
			else
			{
				bool skipLayout = false;
				TypeDataLayout layout = null;
				if (node.SubNodes.FirstOrDefault() is TypeDataLayoutNode)
				{
					TypeDataLayoutNode typeDataLayout = node.SubNodes.FirstOrDefault() as TypeDataLayoutNode;
					this.WriteTypeDataLayout(typeDataLayout.Layout, node.TypeString);
					layout = typeDataLayout.Layout;
					skipLayout = true;
				}
				else
				{
					this.WriteTypeDataLayout(node.TypeString);
					layout = this.GetCachedTypeDataLayout(node.TypeString);
				}

				// Write the structs omitted mask
				bool[] fieldOmitted = new bool[layout.Fields.Length];
				for (int i = 0; i < layout.Fields.Length; i++)
				{
					fieldOmitted[i] = !node.SubNodes.Any(n => !(n is DummyNode) && n.Name == layout.Fields[i].name);
				}
				this.WriteArrayData(fieldOmitted);

				// Write the structs fields
				foreach (DataNode subNode in node.SubNodes)
				{
					if (skipLayout)
					{
						skipLayout = false;
						continue;
					}
					if (subNode is DummyNode) continue;
					this.WriteObject(subNode);
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
			this.writer.Write(node.TypeString);
			this.writer.Write(node.ObjId);
			this.writer.Write(node.InvokeList != null);

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
			this.writer.Write(node.EnumType);
			this.writer.Write(node.ValueName);
			this.writer.Write(node.Value);
		}

		protected override object GetNullObject()
		{
			return new PrimitiveNode(DataType.Unknown, null);
		}
		protected override object ReadObjectBody(DataType dataType)
		{
			DataNode result = null;

			if (dataType.IsPrimitiveType())				result = new PrimitiveNode(dataType, this.ReadPrimitive(dataType));
			else if (dataType == DataType.String)		result = new StringNode(this.ReadString());
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
			uint objId = this.reader.ReadUInt32();

			string typeString = this.reader.ReadString();
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
			string	arrTypeString	= this.reader.ReadString();
			uint	objId			= this.reader.ReadUInt32();
			int		arrRank			= this.reader.ReadInt32();
			int		arrLength		= this.reader.ReadInt32();
			Type	arrType			= ReflectionHelper.ResolveType(arrTypeString, false);

			ArrayNode result = new ArrayNode(arrTypeString, objId, arrRank, arrLength);
			
			// Prepare object reference
			this.idManager.Inject(result, objId);

			// Store primitive data block
			bool nonPrimitive = false;
			if (arrType != null)
			{
				Array arrObj = Array.CreateInstance(arrType.GetElementType(), arrLength);
				if		(arrObj is bool[])		this.ReadArrayData(arrObj as bool[]);
				else if (arrObj is byte[])		this.ReadArrayData(arrObj as byte[]);
				else if (arrObj is sbyte[])		this.ReadArrayData(arrObj as sbyte[]);
				else if (arrObj is short[])		this.ReadArrayData(arrObj as short[]);
				else if (arrObj is ushort[])	this.ReadArrayData(arrObj as ushort[]);
				else if (arrObj is int[])		this.ReadArrayData(arrObj as int[]);
				else if (arrObj is uint[])		this.ReadArrayData(arrObj as uint[]);
				else if (arrObj is long[])		this.ReadArrayData(arrObj as long[]);
				else if (arrObj is ulong[])		this.ReadArrayData(arrObj as ulong[]);
				else if (arrObj is float[])		this.ReadArrayData(arrObj as float[]);
				else if (arrObj is double[])	this.ReadArrayData(arrObj as double[]);
				else if (arrObj is decimal[])	this.ReadArrayData(arrObj as decimal[]);
				else if (arrObj is char[])		this.ReadArrayData(arrObj as char[]);
				else if (arrObj is string[])	this.ReadArrayData(arrObj as string[]);
				else
					nonPrimitive = true;

				if (!nonPrimitive) result.PrimitiveData = arrObj;
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
			string	objTypeString	= this.reader.ReadString();
			uint	objId			= this.reader.ReadUInt32();
			bool	custom			= this.reader.ReadBoolean();
			bool	surrogate		= this.reader.ReadBoolean();

			StructNode result = new StructNode(classType, objTypeString, objId, custom, surrogate);
			
			// Read surrogate constructor data
			if (surrogate)
			{
				custom = true;

				// Set fake object reference for surrogate constructor: No self-references allowed here.
				this.idManager.Inject(null, objId);

				CustomSerialIO customIO = new CustomSerialIO();
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

			if (custom)
			{
				CustomSerialIO customIO = new CustomSerialIO();
				customIO.Deserialize(this);
				foreach (var pair in customIO.Data)
				{
					StringNode key = new StringNode(pair.Key);
					DataNode value = pair.Value as DataNode;
					key.Parent = result;
					value.Parent = result;
				}
			}
			else
			{
				// Determine data layout
				bool wasThereBefore = this.GetCachedTypeDataLayout(objTypeString) != null;
				TypeDataLayout layout = this.ReadTypeDataLayout(objTypeString);
				if (!wasThereBefore)
				{
					TypeDataLayoutNode layoutNode = new TypeDataLayoutNode(new TypeDataLayout(layout));
					layoutNode.Parent = result;
				}

				// Read fields
				if (this.dataVersion <= 2)
				{
					for (int i = 0; i < layout.Fields.Length; i++)
					{
						DataNode fieldValue = this.ReadObject() as DataNode;
						fieldValue.Parent = result;
						fieldValue.Name = layout.Fields[i].name;
					}
				}
				else if (this.dataVersion >= 3)
				{
					bool[] fieldOmitted = new bool[layout.Fields.Length];
					this.ReadArrayData(fieldOmitted);
					
					for (int i = 0; i < layout.Fields.Length; i++)
					{
						if (fieldOmitted[i]) continue;
						DataNode fieldValue = this.ReadObject() as DataNode;
						fieldValue.Parent = result;
						fieldValue.Name = layout.Fields[i].name;
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
			string		delegateTypeString	= this.reader.ReadString();
			uint		objId				= this.reader.ReadUInt32();
			bool		multi				= this.reader.ReadBoolean();

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
			string typeName = this.reader.ReadString();
			string name = this.reader.ReadString();
			long val = this.reader.ReadInt64();
			return new EnumNode(typeName, name, val);
		}
		/// <summary>
		/// Reads an <see cref="Duality.Serialization.MetaFormat.ObjectRefNode"/>.
		/// </summary>
		/// <param name="node"></param>
		protected ObjectRefNode ReadObjectRef()
		{
			uint objId = this.reader.ReadUInt32();
			ObjectRefNode result = new ObjectRefNode(objId);
			return result;
		}
	}
}
