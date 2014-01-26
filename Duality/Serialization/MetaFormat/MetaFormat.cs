using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality.EditorHints;

namespace Duality.Serialization.MetaFormat
{
	public static class MetaFormatHelper
	{
		public static List<DataNode> FileReadAll(string filePath)
		{
			List<DataNode> data = new List<DataNode>();

			// Load data
			using (FileStream fileStream = File.OpenRead(filePath))
			{
				using (var formatter = Formatter.CreateMeta(fileStream))
				{
					DataNode dataNode;
					try
					{
						while ((dataNode = formatter.ReadObject<DataNode>()) != null)
							data.Add(dataNode);
					}
					catch (EndOfStreamException) {}
					catch (Exception e)
					{
						Log.Editor.WriteError("Can't perform meta format action on {0} because an error occurred in the process: \n{1}",
							filePath,
							Log.Exception(e));
						return data;
					}
				}
			}

			return data;
		}
		public static void FileSaveAll(string filePath, List<DataNode> data)
		{
			using (FileStream fileStream = File.Open(filePath, FileMode.Create))
			{
				using (var formatter = Formatter.CreateMeta(fileStream))
				{
					foreach (DataNode dataNode in data)
						formatter.WriteObject(dataNode);
				}
			}
		}
		public static void FilePerformAction(string filePath, Action<DataNode> action)
		{
			// Load data
			List<DataNode> data = FileReadAll(filePath);

			// Process data
			foreach (DataNode dataNode in data)
				action(dataNode);

			// Save data
			FileSaveAll(filePath, data);
		}
		public static bool FilePerformAction(string filePath, Func<DataNode,bool> action)
		{
			// Load data
			List<DataNode> data = FileReadAll(filePath);

			// Process data
			bool saveData = false;
			foreach (DataNode dataNode in data)
			{
				if (action(dataNode))
					saveData = true;
			}

			// Save data
			if (saveData)
			{
				FileSaveAll(filePath, data);
			}

			return saveData;
		}
	}

	/// <summary>
	/// Describes a single serialization data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public abstract class DataNode
	{
		protected	DataType		dataType;
		protected	DataNode		parent;
		protected	List<DataNode>	subNodes;
		protected	string			name;

		/// <summary>
		/// [GET] Enumerates this nodes child nodes.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IEnumerable<DataNode> SubNodes
		{
			get { return this.subNodes; }
		}
		/// <summary>
		/// [GET / SET] This nodes parent node.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public DataNode Parent
		{
			get { return this.parent; }
			set
			{
				if (this.parent == value) return;

				if (this.parent != null) this.parent.subNodes.Remove(this);
				this.parent = value;
				if (this.parent != null) this.parent.subNodes.Add(this);
			}
		}
		/// <summary>
		/// [GET] The <see cref="Duality.Serialization.DataType"/> that is associated with this data node.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public DataType NodeType
		{
			get { return this.dataType; }
		}
		/// <summary>
		/// [GET] The node's name.
		/// </summary>
		public string Name
		{
			get { return this.name; }
			internal set { this.name = value; }
		}

		protected DataNode(DataType dataType)
		{
			this.dataType = dataType;
			this.subNodes = new List<DataNode>();
		}

		/// <summary>
		/// Returns a list of all <see cref="ReflectionHelper.GetTypeId">type strings</see> in this data node.
		/// </summary>
		/// <param name="deep">If true, both this node and all of its children are searched.</param>
		/// <returns>A list of <see cref="ReflectionHelper.GetTypeId">type strings</see>.</returns>
		public List<string> GetTypeStrings(bool deep)
		{
			List<string> result = new List<string>();
			this.GetTypeStrings(ref result, deep);
			return result;
		}
		/// <summary>
		/// DataNodes may override this method to append their <see cref="ReflectionHelper.GetTypeId">type strings</see> to the
		/// specified list. The base version iterates over its children, if a deep search is performed.
		/// </summary>
		/// <param name="list">The <see cref="System.Collections.Generic.List{T}"/> to append type strings to.</param>
		/// <param name="deep">If true, both this node and all of its children are searched.</param>
		protected virtual void GetTypeStrings(ref List<string> list, bool deep)
		{
			if (!deep) return;
			foreach (DataNode n in this.subNodes)
				n.GetTypeStrings(ref list, deep);
		}
		/// <summary>
		/// Determines whether this data node contains an <see cref="ObjectNode">object</see> with the specified
		/// object id. DataNodes may override this method to check for their own object id. The base version iterates
		/// over its children.
		/// </summary>
		/// <param name="objId"></param>
		/// <returns></returns>
		public virtual bool IsObjectIdDefined(uint objId)
		{
			return this.subNodes.Any(n => n.IsObjectIdDefined(objId));
		}

		/// <summary>
		/// Searches for one <see cref="ReflectionHelper.GetTypeId">type string</see> and replaces it with another.
		/// DataNodes may override this method to rename their own type strings. The base version iterates
		/// over its children.
		/// </summary>
		/// <param name="oldTypeString">The old type string that is to be replaced.</param>
		/// <param name="newTypeString">The new type string that is to be used instead of the other.</param>
		/// <returns>The number of occurences that have been replaced.</returns>
		public virtual int ReplaceTypeStrings(string oldTypeString, string newTypeString)
		{
			return this.subNodes.Sum(n => n.ReplaceTypeStrings(oldTypeString, newTypeString));
		}

		protected bool PerformReplaceTypeString(ref string typeString, string oldTypeString, string newTypeString)
		{
			if (typeString == oldTypeString)
			{
				typeString = newTypeString;
				return true;
			}
			else if (typeString.StartsWith(oldTypeString) && (typeString[oldTypeString.Length] == '.' || typeString[oldTypeString.Length] == '+'))
			{
				typeString = newTypeString + typeString.Remove(0, oldTypeString.Length);
				return true;
			}
			return false;
		}
	}
	/// <summary>
	/// Describes a serialization primitive data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class PrimitiveNode : DataNode, IEquatable<PrimitiveNode>
	{
		protected	object	value;

		/// <summary>
		/// [GET / SET] This nodes primitive data value.
		/// </summary>
		public object PrimitiveValue
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public PrimitiveNode(DataType dataType, object value) : base(dataType)
		{
			this.value = value;
		}

		public bool Equals(PrimitiveNode other)
		{
			return other.value == this.value;
		}
		public override bool Equals(object other)
		{
			if (other is PrimitiveNode)
				return (other as PrimitiveNode).Equals(this);
			else
				return false;
		}
		public override int GetHashCode()
		{
			return this.value != null ? this.value.GetHashCode() : 0;
		}
		public static bool operator ==(PrimitiveNode a, PrimitiveNode b)
		{
			if (object.ReferenceEquals(a, b)) return true;
			if (object.ReferenceEquals(a, null)) return false;
			if (object.ReferenceEquals(b, null)) return false;
			return a.Equals(b);
		}
		public static bool operator !=(PrimitiveNode a, PrimitiveNode b)
		{
			return !(a == b);
		}
	}
	/// <summary>
	/// Describes a serialization <see cref="System.String"/> data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class StringNode : DataNode
	{
		protected	string	value;

		/// <summary>
		/// [GET / SET] This nodes <see cref="System.String"/> value.
		/// </summary>
		public string StringValue
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public StringNode(string value) : base(DataType.String)
		{
			this.value = value;
		}
	}
	/// <summary>
	/// Describes a serialization <see cref="System.Enum"/> data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class EnumNode : DataNode
	{
		protected	string	enumType;
		protected	string	valueName;
		protected	long	value;

		/// <summary>
		/// [GET / SET] A string referring to the <see cref="System.Enum">Enums</see> Tyoe.
		/// </summary>
		public string EnumType
		{
			get { return this.enumType; }
			set { this.enumType = value; }
		}
		/// <summary>
		/// [GET / SET] The values name in the <see cref="System.Enum"/>.
		/// </summary>
		public string ValueName
		{
			get { return this.valueName; }
			set { this.valueName = value; }
		}
		/// <summary>
		/// [GET / SET] The numeric enum value
		/// </summary>
		public long Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public EnumNode(string enumType, string name, long value) : base(DataType.Enum)
		{
			this.enumType = enumType;
			this.valueName = name;
			this.value = value;
		}
		protected override void GetTypeStrings(ref List<string> list, bool deep)
		{
			list.Add(this.enumType);
			base.GetTypeStrings(ref list, deep);
		}
		public override int ReplaceTypeStrings(string oldTypeString, string newTypeString)
		{
			int count = base.ReplaceTypeStrings(oldTypeString, newTypeString);
			if (this.PerformReplaceTypeString(ref this.enumType, oldTypeString, newTypeString)) count++;
			return count;
		}
	}
	/// <summary>
	/// Describes a serialization object data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public abstract class ObjectNode : DataNode
	{
		protected	string	typeString;
		protected	uint	objId;
		
		/// <summary>
		/// [GET / SET] A string referring to the objects <see cref="System.Type"/>.
		/// </summary>
		public string TypeString
		{
			get { return this.typeString; }
			set { this.typeString = value; }
		}
		/// <summary>
		/// [GET / SET] The objects id.
		/// </summary>
		public uint ObjId
		{
			get { return this.objId; }
			set { this.objId = value; }
		}

		protected ObjectNode(DataType dataType, string typeString, uint objId) : base(dataType)
		{
			this.typeString = typeString;
			this.objId = objId;
		}

		protected override void GetTypeStrings(ref List<string> list, bool deep)
		{
			list.Add(this.typeString);
			base.GetTypeStrings(ref list, deep);
		}
		public override bool IsObjectIdDefined(uint objId)
		{
			if (this.objId == objId) return true;
			return base.IsObjectIdDefined(objId);
		}
		public override int ReplaceTypeStrings(string oldTypeString, string newTypeString)
		{
			int count = base.ReplaceTypeStrings(oldTypeString, newTypeString);
			if (this.PerformReplaceTypeString(ref this.typeString, oldTypeString, newTypeString)) count++;
			return count;
		}
	}
	/// <summary>
	/// Describes a serialization array data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class ArrayNode : ObjectNode
	{
		protected	int		rank;
		protected	int		length;
		protected	Array	primitiveData;

		/// <summary>
		/// [GET / SET] An <see cref="System.Array"/> storing this nodes primitive data. Null, if
		/// the data stored in this ArrayNode is not primitive.
		/// </summary>
		public Array PrimitiveData
		{
			get { return this.primitiveData; }
			set
			{ 
				this.primitiveData = value;
				if (this.primitiveData != null)
				{
					this.rank = this.primitiveData.Rank;
					this.length = this.primitiveData.Length;
				}
				else
				{
					this.rank = 0;
					this.length = 0;
				}
			}
		}
		/// <summary>
		/// [GET / SET] The rank of the array.
		/// </summary>
		public int Rank
		{
			get { return this.rank; }
		}
		/// <summary>
		/// [GET / SET] The arrays length.
		/// </summary>
		public int Length
		{
			get { return this.length; }
		}

		public ArrayNode(string typeString, uint objId, int rank, int length) : base(DataType.Array, typeString, objId)
		{
			this.rank = rank;
			this.length = length;
		}
	}
	/// <summary>
	/// Describes a serialization structural object data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class StructNode : ObjectNode
	{
		protected	bool	customSerialization;
		protected	bool	surrogateSerialization;

		/// <summary>
		/// [GET / SET] Whether this structural object uses <see cref="Duality.Serialization.ISerializable">custom serialization</see>.
		/// </summary>
		public bool CustomSerialization
		{
			get { return this.customSerialization; }
			set { this.customSerialization = value; }
		}
		/// <summary>
		/// [GET / SET] Whether this structural object uses an <see cref="Duality.Serialization.ISurrogate"/>.
		/// </summary>
		public bool SurrogateSerialization
		{
			get { return this.surrogateSerialization; }
			set { this.surrogateSerialization = value; }
		}

		public StructNode(bool isClass, string typeString, uint objId, bool customSerialization, bool surrogateSerialization) : base(isClass ? DataType.Class : DataType.Struct, typeString, objId)
		{
			this.customSerialization = customSerialization;
			this.surrogateSerialization = surrogateSerialization;
		}
	}
	/// <summary>
	/// Describes a serialization object reference data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class ObjectRefNode : DataNode
	{
		protected uint objId;

		/// <summary>
		/// [GET / SET] The id of the referenced object.
		/// </summary>
		public uint ObjRefId
		{
			get { return this.objId; }
			set { this.objId = value; }
		}

		public ObjectRefNode(uint objId) : base(DataType.ObjectRef)
		{
			this.objId = objId;
		}
	}
	/// <summary>
	/// Describes a serialization <see cref="System.Reflection.MemberInfo"/> data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class MemberInfoNode : ObjectNode
	{
		public MemberInfoNode(DataType dataType, string memberString, uint objId) : base(dataType, memberString, objId)
		{
		}
	}
	/// <summary>
	/// Describes a serialization <see cref="System.Delegate"/> data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class DelegateNode : ObjectNode
	{
		protected	DataNode	method;
		protected	DataNode	target;
		protected	DataNode	invokeList;

		/// <summary>
		/// [GET / SET] A reference to the DataNode containing the <see cref="System.Delegate">Delegates</see> invokation list.
		/// </summary>
		public DataNode InvokeList
		{
			get { return this.invokeList; }
			set 
			{
				if (this.invokeList != null) this.invokeList.Parent = null;
				this.invokeList = value;
				if (this.invokeList != null) this.invokeList.Parent = this;
			}
		}
		/// <summary>
		/// [GET / SET] A reference to the DataNode containing the <see cref="System.Delegate">Delegates</see> method.
		/// </summary>
		public DataNode Method
		{
			get { return this.method; }
		}
		/// <summary>
		/// [GET / SET] A reference to the DataNode containing the <see cref="System.Delegate">Delegates</see> target object.
		/// </summary>
		public DataNode Target
		{
			get { return this.target; }
			internal set { this.target = value; }
		}

		public DelegateNode(string typeString, uint objId, DataNode method, DataNode target, DataNode invokeList) : base(DataType.Delegate, typeString, objId) 
		{
			this.method = method;
			this.target = target;
			this.invokeList = invokeList;

			if (this.method != null) this.method.Parent = this;
			if (this.target != null) this.target.Parent = this;
			if (this.invokeList != null) this.invokeList.Parent = this;
		}
	}
	/// <summary>
	/// Describes a serialization <see cref="Duality.Serialization.TypeDataLayout"/> data node.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class TypeDataLayoutNode : DataNode
	{
		protected	TypeDataLayout	layout;

		/// <summary>
		/// [GET / SET] The TypeDataLayout that is stored in this node.
		/// </summary>
		public TypeDataLayout Layout
		{
			get { return this.layout; }
			set { this.layout = value; }
		}

		public TypeDataLayoutNode(TypeDataLayout layout) : base(DataType.Unknown)
		{
			this.layout = layout;
		}
			
		protected override void GetTypeStrings(ref List<string> list, bool deep)
		{
			list.AddRange(this.layout.Fields.Select(f => f.typeString));
			base.GetTypeStrings(ref list, deep);
		}
		public override int ReplaceTypeStrings(string oldTypeString, string newTypeString)
		{
			int count = base.ReplaceTypeStrings(oldTypeString, newTypeString);
			for (int i = 0; i < this.layout.Fields.Length; i++)
			{
				if (this.PerformReplaceTypeString(ref this.layout.Fields[i].typeString, oldTypeString, newTypeString))
					count++;
			}
			return count;
		}
	}
	/// <summary>
	/// Describes a serialization dummy data node. It does not contain any information but might be used by meta-formatters
	/// to group and organize other nodes that have actually been read.
	/// </summary>
	/// <seealso cref="Duality.Serialization.MetaFormat.BinaryMetaFormatter"/>
	public class DummyNode : DataNode
	{
		public DummyNode() : base(DataType.Unknown) {}
	}
}
