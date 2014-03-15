using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;

namespace Duality.Serialization
{
	/// <summary>
	/// De/Serializes object data.
	/// </summary>
	public class BinaryFormatter : Formatter
	{
		private class CustomSerialIO : CustomSerialIOBase<BinaryFormatter>
		{
			public void Serialize(BinaryFormatter formatter)
			{
				formatter.WritePrimitive(this.data.Count);
				foreach (var pair in this.data)
				{
					formatter.WriteString(pair.Key);
					formatter.WriteObjectData(pair.Value);
				}
				this.Clear();
			}
			public void Deserialize(BinaryFormatter formatter)
			{
				this.Clear();
				int count = (int)formatter.ReadPrimitive(DataType.Int);
				for (int i = 0; i < count; i++)
				{
					string key = formatter.ReadString();
					object value = formatter.ReadObjectData();
					this.data.Add(key, value);
				}
			}
		}

		
		private	const	string	HeaderId	= "BinaryFormatterHeader";
		private	const	ushort	Version		= 3;


		private BinaryWriter	writer		= null;
		private BinaryReader	reader		= null;
		private ushort			dataVersion	= 0;

		private Stack<long>							offsetStack			= new Stack<long>();
		private Dictionary<string,TypeDataLayout>	typeDataLayout		= new Dictionary<string,TypeDataLayout>();
		private Dictionary<string,long>				typeDataLayoutMap	= new Dictionary<string,long>();

		
		public override bool CanWrite
		{
			get { return this.writer != null; }
		}
		public override bool CanRead
		{
			get { return this.reader != null; }
		}


		public BinaryFormatter(Stream stream)
		{
			stream = stream.NonClosing();
			this.writer = (stream != null && stream.CanWrite) ? new BinaryWriter(stream) : null;
			this.reader = (stream != null && stream.CanRead) ? new BinaryReader(stream) : null;
		}
		protected override void OnDisposed(bool manually)
		{
			base.OnDisposed(manually);

			if (this.writer != null)
			{
				this.writer.Flush();
				this.writer.Dispose();
				this.writer = null;
			}

			if (this.reader != null)
			{
				this.reader.Dispose();
				this.reader = null;
			}
		}
		
		protected override void WriteObjectData(object obj)
		{
			// NotNull flag
			if (obj == null)
			{
				this.writer.Write(false);
				return;
			}
			else
				this.writer.Write(true);
			
			// Retrieve type data
			ObjectHeader header = this.PrepareWriteObject(obj);

			// Write data type header
			this.WriteDataType(header.DataType);
			this.WritePushOffset();

			if (header.IsObjectTypeRequired) this.writer.Write(header.TypeString);
			if (header.IsObjectIdRequired) this.writer.Write(header.ObjectId);

			try
			{
				// Write object
				this.idManager.PushIdLevel();
				this.WriteObjectBody(obj, header);
			}
			finally
			{
				// Write object footer
				this.WritePopOffset();
				this.idManager.PopIdLevel();
			}
		}
		protected override object ReadObjectData()
		{
			if (this.reader.BaseStream.Position == this.reader.BaseStream.Length) throw new EndOfStreamException("No more data to read.");

			// Not null flag
			bool isNotNull = this.reader.ReadBoolean();
			if (!isNotNull) return null;

			// Read data type header
			DataType dataType = this.ReadDataType();
			long lastPos = this.reader.BaseStream.Position;
			long offset = this.reader.ReadInt64();
			
			string typeStr = null;
			uint objId = 0;
			if (dataType.HasTypeName()) typeStr = this.reader.ReadString();
			if (dataType.HasObjectId()) objId = this.reader.ReadUInt32();
			
			Type type = null;
			if (typeStr != null) type = this.ResolveType(typeStr, objId);
			ObjectHeader header = new ObjectHeader(objId, dataType, type != null ? type.GetSerializeType() : null);
			if (header.DataType == DataType.Unknown)
			{
				this.LocalLog.WriteError("Unable to process DataType: {0}.", typeStr);
				return null;
			}

			// Read object
			object result = null;
			try
			{
				// Read the objects body
				result = this.ReadObjectBody(header);

				// If we read the object properly and aren't where we're supposed to be, something went wrong
				if (this.reader.BaseStream.Position != lastPos + offset) throw new ApplicationException(string.Format("Wrong dataset offset: '{0}' instead of expected value '{1}'.", this.reader.BaseStream.Position - lastPos, offset));
			}
			catch (Exception e)
			{
				// If anything goes wrong, assure the stream position is valid and points to the next data entry
				this.reader.BaseStream.Seek(lastPos + offset, SeekOrigin.Begin);
				// Log the error
				this.LocalLog.WriteError("Error reading object at '{0:X8}'-'{1:X8}': {2}", 
					lastPos,
					lastPos + offset, 
					e is ApplicationException ? e.Message : Log.Exception(e));
			}

			return result;
		}
		
		protected override void BeginReadOperation()
		{
			base.BeginReadOperation();
			this.ReadFormatterHeader();
		}
		protected override void BeginWriteOperation()
		{
			base.BeginWriteOperation();
			this.WriteFormatterHeader();
		}
		protected override void EndReadOperation()
		{
			base.EndReadOperation();
			this.typeDataLayout.Clear();
			this.typeDataLayoutMap.Clear();
			this.offsetStack.Clear();
		}
		protected override void EndWriteOperation()
		{
			base.EndWriteOperation();
			this.typeDataLayout.Clear();
			this.typeDataLayoutMap.Clear();
			this.offsetStack.Clear();
			this.writer.Flush();
		}

		
		private void WriteObjectBody(object obj, ObjectHeader header)
		{
			if (header.IsPrimitive)							this.WritePrimitive(obj);
			else if (header.DataType == DataType.Enum)		this.WriteEnum(obj as Enum, header);
			else if (header.DataType == DataType.Struct)	this.WriteStruct(obj, header);
			else if (header.DataType == DataType.ObjectRef)	this.writer.Write(header.ObjectId);
			else if	(header.DataType == DataType.Array)		this.WriteArray(obj, header);
			else if (header.DataType == DataType.Delegate)	this.WriteDelegate(obj, header);
			else if (header.DataType.IsMemberInfoType())	this.WriteMemberInfo(obj, header);
		}
		private void WriteMemberInfo(object obj, ObjectHeader header)
		{
			if (obj is Type)
			{
				Type type = obj as Type;
				SerializeType cachedType = type.GetSerializeType();

				this.writer.Write(cachedType.TypeString);
			}
			else if (obj is MemberInfo)
			{
				MemberInfo member = obj as MemberInfo;

				this.writer.Write(member.GetMemberId());
			}
			else if (obj == null)
				throw new ArgumentNullException("obj");
			else
				throw new ArgumentException(string.Format("Type '{0}' is not a supported MemberInfo.", obj.GetType()));
		}
		private void WriteArray(object obj, ObjectHeader header)
		{
			Array objAsArray = obj as Array;

			if (objAsArray.Rank != 1) throw new ArgumentException("Non single-Rank arrays are not supported");
			if (objAsArray.GetLowerBound(0) != 0) throw new ArgumentException("Non zero-based arrays are not supported");

			this.writer.Write(objAsArray.Rank);
			this.writer.Write(objAsArray.Length);

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
			else
			{
				for (long l = 0; l < objAsArray.Length; l++)
					this.WriteObjectData(objAsArray.GetValue(l));
			}
		}
		private void WriteStruct(object obj, ObjectHeader header)
		{
			ISerializeExplicit objAsCustom = obj as ISerializeExplicit;
			ISerializeSurrogate objSurrogate = GetSurrogateFor(header.ObjectType);

			this.writer.Write(objAsCustom != null);
			this.writer.Write(objSurrogate != null);

			if (objSurrogate != null)
			{
				objSurrogate.RealObject = obj;
				objAsCustom = objSurrogate.SurrogateObject;

				CustomSerialIO customIO = new CustomSerialIO();
				try { objSurrogate.WriteConstructorData(customIO); }
				catch (Exception e) { this.LogCustomSerializationError(header.ObjectId, header.ObjectType, e); }
				customIO.Serialize(this);
			}

			if (objAsCustom != null)
			{
				CustomSerialIO customIO = new CustomSerialIO();
				try { objAsCustom.WriteData(customIO); }
				catch (Exception e) { this.LogCustomSerializationError(header.ObjectId, header.ObjectType, e); }
				customIO.Serialize(this);
			}
			else
			{
				// Assure the type data layout has bee written (only once per file)
				this.WriteTypeDataLayout(header.SerializeType);

				// Write omitted field bitmask
				bool[] fieldOmitted = new bool[header.SerializeType.Fields.Length];
				for (int i = 0; i < fieldOmitted.Length; i++)
				{
					fieldOmitted[i] = this.IsFieldBlocked(header.SerializeType.Fields[i], obj);
				}
				this.WriteArrayData(fieldOmitted);

				// Write the structs fields
				for (int i = 0; i < header.SerializeType.Fields.Length; i++)
				{
					if (fieldOmitted[i]) continue;
					this.WriteObjectData(header.SerializeType.Fields[i].GetValue(obj));
				}
			}
		}
		private void WriteDelegate(object obj, ObjectHeader header)
		{
			bool multi = obj is MulticastDelegate;
			this.writer.Write(multi);

			if (!multi)
			{
				Delegate objAsDelegate = obj as Delegate;
				this.WriteObjectData(objAsDelegate.Method);
				this.WriteObjectData(objAsDelegate.Target);
			}
			else
			{
				MulticastDelegate objAsDelegate = obj as MulticastDelegate;
				Delegate[] invokeList = objAsDelegate.GetInvocationList();
				this.WriteObjectData(objAsDelegate.Method);
				this.WriteObjectData(objAsDelegate.Target);
				this.WriteObjectData(invokeList);
			}
		}
		private void WriteEnum(Enum obj, ObjectHeader header)
		{
			this.writer.Write(obj.ToString());
			this.writer.Write(Convert.ToInt64(obj));
		}
		
		private void WriteFormatterHeader()
		{
			this.writer.Write(HeaderId);
			this.writer.Write(Version);
			this.WritePushOffset();

			// --[ Insert writing additional header data here ]--

			this.WritePopOffset();
		}
		private void WriteTypeDataLayout(SerializeType objSerializeType)
		{
			if (this.typeDataLayout.ContainsKey(objSerializeType.TypeString))
			{
				long backRef = this.typeDataLayoutMap[objSerializeType.TypeString];
				this.writer.Write(backRef);
				return;
			}

			this.WriteTypeDataLayout(new TypeDataLayout(objSerializeType), objSerializeType.TypeString);
		}
		private void WriteTypeDataLayout(string typeString)
		{
			if (this.typeDataLayout.ContainsKey(typeString))
			{
				long backRef = this.typeDataLayoutMap[typeString];
				this.writer.Write(backRef);
				return;
			}

			Type resolved = this.ResolveType(typeString);
			SerializeType cached = resolved != null ? resolved.GetSerializeType() : null;
			TypeDataLayout layout = cached != null ? new TypeDataLayout(cached) : null;
			this.WriteTypeDataLayout(layout, typeString);
		}
		private void WriteTypeDataLayout(TypeDataLayout layout, string typeString)
		{
			this.typeDataLayout[typeString] = layout;
			this.writer.Write(-1L);
			this.typeDataLayoutMap[typeString] = this.writer.BaseStream.Position;
			layout.Write(this.writer);
		}
		private void WritePrimitive(object obj)
		{
			if		(obj is bool)		this.writer.Write((bool)obj);
			else if (obj is byte)		this.writer.Write((byte)obj);
			else if (obj is char)		this.writer.Write((char)obj);
			else if (obj is string)		this.writer.Write((string)obj);
			else if (obj is sbyte)		this.writer.Write((sbyte)obj);
			else if (obj is short)		this.writer.Write((short)obj);
			else if (obj is ushort)		this.writer.Write((ushort)obj);
			else if (obj is int)		this.writer.Write((int)obj);
			else if (obj is uint)		this.writer.Write((uint)obj);
			else if (obj is long)		this.writer.Write((long)obj);
			else if (obj is ulong)		this.writer.Write((ulong)obj);
			else if (obj is float)		this.writer.Write((float)obj);
			else if (obj is double)		this.writer.Write((double)obj);
			else if (obj is decimal)	this.writer.Write((decimal)obj);
			else if (obj == null)
				throw new ArgumentNullException("obj");
			else
				throw new ArgumentException(string.Format("Type '{0}' is not a primitive.", obj.GetType()));
		}
		private void WriteString(string obj)
		{
			this.writer.Write(obj);
		}

		private void WriteArrayData(bool[] obj)
		{
			for (long l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(byte[] obj)
		{
			this.writer.Write(obj);
		}
		private void WriteArrayData(sbyte[] obj)
		{
			for (long l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(short[] obj)
		{
			for (long l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(ushort[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(int[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(uint[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(long[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(ulong[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(float[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(double[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(decimal[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(char[] obj)
		{
			for (int l = 0; l < obj.Length; l++) this.writer.Write(obj[l]);
		}
		private void WriteArrayData(string[] obj)
		{
			for (int l = 0; l < obj.Length; l++)
			{
				if (obj[l] == null)
					this.writer.Write(false);
				else
				{
					this.writer.Write(true);
					this.writer.Write(obj[l]);
				}
			}
		}

		private void WriteDataType(DataType dt)
		{
			this.writer.Write((ushort)dt);
		}
		private DataType ReadDataType()
		{
			DataType dataType = (DataType)this.reader.ReadUInt16();
			if (dataType == (DataType)24) dataType = DataType.Struct; // Legacy support (Written 2014-03-10)
			return dataType;
		}
		private void WritePushOffset()
		{
			this.offsetStack.Push(this.writer.BaseStream.Position);
			this.writer.Write(0L);
		}
		private void WritePopOffset()
		{
			long curPos = this.writer.BaseStream.Position;
			long lastPos = this.offsetStack.Pop();
			long offset = curPos - lastPos;
			
			this.writer.BaseStream.Seek(lastPos, SeekOrigin.Begin);
			this.writer.Write(offset);
			this.writer.BaseStream.Seek(curPos, SeekOrigin.Begin);
		}

		
		private object ReadObjectBody(ObjectHeader header)
		{
			object result = null;

			if (header.IsPrimitive)							result = this.ReadPrimitive(header.DataType);
			else if (header.DataType == DataType.Enum)		result = this.ReadEnum(header);
			else if (header.DataType == DataType.Struct)	result = this.ReadStruct(header);
			else if (header.DataType == DataType.ObjectRef)	result = this.ReadObjectRef();
			else if (header.DataType == DataType.Array)		result = this.ReadArray(header);
			else if (header.DataType == DataType.Delegate)	result = this.ReadDelegate(header);
			else if (header.DataType.IsMemberInfoType())	result = this.ReadMemberInfo(header);

			return result;
		}
		private Array ReadArray(ObjectHeader header)
		{
			int		arrRang			= this.reader.ReadInt32();
			int		arrLength		= this.reader.ReadInt32();

			Array arrObj = header.ObjectType != null ? Array.CreateInstance(header.ObjectType.GetElementType(), arrLength) : null;
			
			// Prepare object reference
			this.idManager.Inject(arrObj, header.ObjectId);

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
			{
				for (int l = 0; l < arrLength; l++)
				{
					object elem = this.ReadObjectData();
					if (arrObj != null) arrObj.SetValue(elem, l);
				}
			}

			return arrObj;
		}
		private object ReadStruct(ObjectHeader header)
		{
			// Read struct type
			bool	custom			= this.reader.ReadBoolean();
			bool	surrogate		= this.reader.ReadBoolean();

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
					customIO.Deserialize(this);
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
				customIO.Deserialize(this);

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
			else
			{
				// Determine data layout
				TypeDataLayout layout	= this.ReadTypeDataLayout(header.TypeString);

				// Read fields
				if (this.dataVersion <= 2)
				{
					for (int i = 0; i < layout.Fields.Length; i++)
					{
						object fieldValue = this.ReadObjectData();
						this.AssignValueToField(header.SerializeType, obj, layout.Fields[i].name, fieldValue);
					}
				}
				else if (this.dataVersion >= 3)
				{
					bool[] fieldOmitted = new bool[layout.Fields.Length];
					this.ReadArrayData(fieldOmitted);
					for (int i = 0; i < layout.Fields.Length; i++)
					{
						if (fieldOmitted[i]) continue;
						object fieldValue = this.ReadObjectData();
						this.AssignValueToField(header.SerializeType, obj, layout.Fields[i].name, fieldValue);
					}
				}
			}

			return obj;
		}
		private object ReadObjectRef()
		{
			object obj;
			uint objId = this.reader.ReadUInt32();

			if (!this.idManager.Lookup(objId, out obj)) throw new ApplicationException(string.Format("Can't resolve object reference '{0}'.", objId));

			return obj;
		}
		private MemberInfo ReadMemberInfo(ObjectHeader header)
		{
			MemberInfo result = null;

			try
			{
				#region Version 1
				if (this.dataVersion <= 1)
				{
					if (header.DataType == DataType.Type)
					{
						string typeString = this.reader.ReadString();
						Type type = this.ResolveType(typeString);
						result = type;
					}
					else if (header.DataType == DataType.FieldInfo)
					{
						bool isStatic = this.reader.ReadBoolean();
						string declaringTypeString = this.reader.ReadString();
						string fieldName = this.reader.ReadString();

						Type declaringType = this.ResolveType(declaringTypeString);
						FieldInfo field = declaringType.GetField(fieldName, isStatic ? ReflectionHelper.BindStaticAll : ReflectionHelper.BindInstanceAll);
						result = field;
					}
					else if (header.DataType == DataType.PropertyInfo)
					{
						bool isStatic = this.reader.ReadBoolean();
						string declaringTypeString = this.reader.ReadString();
						string propertyName = this.reader.ReadString();
						string propertyTypeString = this.reader.ReadString();

						int paramCount = this.reader.ReadInt32();
						string[] paramTypeStrings = new string[paramCount];
						for (int i = 0; i < paramCount; i++)
							paramTypeStrings[i] = this.reader.ReadString();

						Type declaringType = this.ResolveType(declaringTypeString);
						Type propertyType = this.ResolveType(propertyTypeString);
						Type[] paramTypes = new Type[paramCount];
						for (int i = 0; i < paramCount; i++)
							paramTypes[i] = this.ResolveType(paramTypeStrings[i]);

						PropertyInfo property = declaringType.GetProperty(
							propertyName, 
							isStatic ? ReflectionHelper.BindStaticAll : ReflectionHelper.BindInstanceAll, 
							null, 
							propertyType, 
							paramTypes, 
							null);

						result = property;
					}
					else if (header.DataType == DataType.MethodInfo)
					{
						bool isStatic = this.reader.ReadBoolean();
						string declaringTypeString = this.reader.ReadString();
						string methodName = this.reader.ReadString();

						int paramCount = this.reader.ReadInt32();
						string[] paramTypeStrings = new string[paramCount];
						for (int i = 0; i < paramCount; i++)
							paramTypeStrings[i] = this.reader.ReadString();

						Type declaringType = this.ResolveType(declaringTypeString);
						Type[] paramTypes = new Type[paramCount];
						for (int i = 0; i < paramCount; i++)
							paramTypes[i] = this.ResolveType(paramTypeStrings[i]);

						MethodInfo method = declaringType.GetMethod(methodName, isStatic ? ReflectionHelper.BindStaticAll : ReflectionHelper.BindInstanceAll, null, paramTypes, null);
						result = method;
					}
					else if (header.DataType == DataType.ConstructorInfo)
					{
						bool isStatic = this.reader.ReadBoolean();
						string declaringTypeString = this.reader.ReadString();

						int paramCount = this.reader.ReadInt32();
						string[] paramTypeStrings = new string[paramCount];
						for (int i = 0; i < paramCount; i++)
							paramTypeStrings[i] = this.reader.ReadString();

						Type declaringType = this.ResolveType(declaringTypeString);
						Type[] paramTypes = new Type[paramCount];
						for (int i = 0; i < paramCount; i++)
							paramTypes[i] = this.ResolveType(paramTypeStrings[i]);

						ConstructorInfo method = declaringType.GetConstructor(isStatic ? ReflectionHelper.BindStaticAll : ReflectionHelper.BindInstanceAll, null, paramTypes, null);
						result = method;
					}
					else if (header.DataType == DataType.EventInfo)
					{
						bool isStatic = this.reader.ReadBoolean();
						string declaringTypeString = this.reader.ReadString();
						string eventName = this.reader.ReadString();

						Type declaringType = this.ResolveType(declaringTypeString);
						EventInfo e = declaringType.GetEvent(eventName, isStatic ? ReflectionHelper.BindStaticAll : ReflectionHelper.BindInstanceAll);
						result = e;
					}
					else
						throw new ApplicationException(string.Format("Invalid DataType '{0}' in ReadMemberInfo method.", header.DataType));
				}
				#endregion
				else if (this.dataVersion >= 2)
				{
					if (header.DataType == DataType.Type)
					{
						string typeString = this.reader.ReadString();
						result = this.ResolveType(typeString, header.ObjectId);
					}
					else
					{
						string memberString = this.reader.ReadString();
						result = this.ResolveMember(memberString, header.ObjectId);
					}
				}
			}
			catch (Exception e)
			{
				result = null;
				this.LocalLog.WriteError(
					"An error occurred in deserializing MemberInfo object Id {0} of type '{1}': {2}",
					header.ObjectId,
					Log.Type(header.DataType.ToActualType()),
					Log.Exception(e));
			}
			
			// Prepare object reference
			this.idManager.Inject(result, header.ObjectId);

			return result;
		}
		private Delegate ReadDelegate(ObjectHeader header)
		{
			bool multi = this.reader.ReadBoolean();

			// Create the delegate without target and fix it later, so we can register its object id before loading its target object
			MethodInfo	method	= this.ReadObjectData() as MethodInfo;
			object		target	= null;
			Delegate	del		= header.ObjectType != null && method != null ? Delegate.CreateDelegate(header.ObjectType, target, method) : null;

			// Prepare object reference
			this.idManager.Inject(del, header.ObjectId);

			// Read the target object now and replace the dummy
			target = this.ReadObjectData();
			if (del != null && target != null)
			{
				FieldInfo targetField = header.ObjectType.GetField("_target", ReflectionHelper.BindInstanceAll);
				targetField.SetValue(del, target);
			}

			// Combine multicast delegates
			if (multi)
			{
				Delegate[] invokeList = (this.ReadObjectData() as Delegate[]).NotNull().ToArray();
				del = invokeList.Length > 0 ? Delegate.Combine(invokeList) : null;
			}

			return del;
		}
		private Enum ReadEnum(ObjectHeader header)
		{
			string name = this.reader.ReadString();
			long val = this.reader.ReadInt64();
			return (header.ObjectType == null) ? null : this.ResolveEnumValue(header.ObjectType, name, val);
		}
		
		private TypeDataLayout GetCachedTypeDataLayout(string t)
		{
			TypeDataLayout result;
			if (!this.typeDataLayout.TryGetValue(t, out result)) return null;
			return result;
		}
		private TypeDataLayout ReadTypeDataLayout(Type t)
		{
			return this.ReadTypeDataLayout(t.GetSerializeType().TypeString);
		}
		private TypeDataLayout ReadTypeDataLayout(string t)
		{
			long backRef = this.reader.ReadInt64();

			TypeDataLayout result;
			if (this.typeDataLayout.TryGetValue(t, out result) && backRef != -1L) return result;

			long lastPos = this.reader.BaseStream.Position;
			if (backRef != -1L) this.reader.BaseStream.Seek(backRef, SeekOrigin.Begin);
			result = result ?? new TypeDataLayout(this.reader);
			if (backRef != -1L) this.reader.BaseStream.Seek(lastPos, SeekOrigin.Begin);

			this.typeDataLayout[t] = result;
			return result;
		}
		private void ReadFormatterHeader()
		{
			long initialPos = this.reader.BaseStream.Position;
			try
			{
				string headerId = this.reader.ReadString();
				if (headerId != HeaderId) throw new ApplicationException("Header ID does not match.");
				this.dataVersion = this.reader.ReadUInt16();

				// Create "Safe zone" for additional data
				long lastPos = this.reader.BaseStream.Position;
				long offset = this.reader.ReadInt64();
				try
				{
					// --[ Insert reading additional data here ]--

					// If we read the object properly and aren't where we're supposed to be, something went wrong
					if (this.reader.BaseStream.Position != lastPos + offset) throw new ApplicationException(string.Format("Wrong dataset offset: '{0}' instead of expected value '{1}'.", offset, this.reader.BaseStream.Position - lastPos));
				}
				catch (Exception e)
				{
					// If anything goes wrong, assure the stream position is valid and points to the next data entry
					this.reader.BaseStream.Seek(lastPos + offset, SeekOrigin.Begin);
					this.LocalLog.WriteError("Error reading header at '{0:X8}'-'{1:X8}': {2}", lastPos, lastPos + offset, Log.Exception(e));
				}
			}
			catch (Exception e) 
			{
				this.reader.BaseStream.Seek(initialPos, SeekOrigin.Begin);
				this.LocalLog.WriteError("Error reading header: {0}", Log.Exception(e));
			}
		}
		private object ReadPrimitive(DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Bool:			return this.reader.ReadBoolean();
				case DataType.Byte:			return this.reader.ReadByte();
				case DataType.SByte:		return this.reader.ReadSByte();
				case DataType.Short:		return this.reader.ReadInt16();
				case DataType.UShort:		return this.reader.ReadUInt16();
				case DataType.Int:			return this.reader.ReadInt32();
				case DataType.UInt:			return this.reader.ReadUInt32();
				case DataType.Long:			return this.reader.ReadInt64();
				case DataType.ULong:		return this.reader.ReadUInt64();
				case DataType.Float:		return this.reader.ReadSingle();
				case DataType.Double:		return this.reader.ReadDouble();
				case DataType.Decimal:		return this.reader.ReadDecimal();
				case DataType.Char:			return this.reader.ReadChar();
				case DataType.String:		return this.reader.ReadString();
				default:
					throw new ArgumentException(string.Format("DataType '{0}' is not a primitive.", dataType));
			}
		}
		private string ReadString()
		{
			return this.reader.ReadString();
		}

		private void ReadArrayData(bool[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadBoolean();
		}
		private void ReadArrayData(byte[] obj)
		{
			this.reader.Read(obj, 0, obj.Length);
		}
		private void ReadArrayData(sbyte[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadSByte();
		}
		private void ReadArrayData(short[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadInt16();
		}
		private void ReadArrayData(ushort[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadUInt16();
		}
		private void ReadArrayData(int[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadInt32();
		}
		private void ReadArrayData(uint[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadUInt32();
		}
		private void ReadArrayData(long[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadInt64();
		}
		private void ReadArrayData(ulong[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadUInt64();
		}
		private void ReadArrayData(float[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadSingle();
		}
		private void ReadArrayData(double[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadDouble();
		}
		private void ReadArrayData(decimal[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadDecimal();
		}
		private void ReadArrayData(char[] obj)
		{
			for (int l = 0; l < obj.Length; l++) obj[l] = this.reader.ReadChar();
		}
		private void ReadArrayData(string[] obj)
		{
			for (int l = 0; l < obj.Length; l++)
			{
				bool isNotNull = this.reader.ReadBoolean();
				if (isNotNull)
					obj[l] = this.reader.ReadString();
				else
					obj[l] = null;
			}
		}
	}
}
