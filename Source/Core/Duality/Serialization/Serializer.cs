using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality.IO;

namespace Duality.Serialization
{
	/// <summary>
	/// Base class for Dualitys serializers.
	/// </summary>
	[DontSerialize]
	public abstract class Serializer : IDisposable
	{
		/// <summary>
		/// Declares a <see cref="System.Reflection.FieldInfo">field</see> blocker. If a field blocker
		/// returns true upon serializing a specific field, a default value is assumed instead.
		/// </summary>
		/// <param name="field"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public delegate bool FieldBlocker(FieldInfo field, object obj);

		/// <summary>
		/// Buffer object for <see cref="Duality.Serialization.ISerializeExplicit">custom de/serialization</see>, 
		/// providing read and write functionality.
		/// </summary>
		protected abstract class CustomSerialIOBase<T> : IDataReader, IDataWriter where T : Serializer
		{
			protected	Dictionary<string,object>	data;
			
			/// <summary>
			/// [GET] Enumerates all available keys.
			/// </summary>
			public IEnumerable<string> Keys
			{
				get { return this.data.Keys; }
			}
			/// <summary>
			/// [GET] Enumerates all currently stored <see cref="System.Collections.Generic.KeyValuePair{T,U}">KeyValuePairs</see>.
			/// </summary>
			public IEnumerable<KeyValuePair<string,object>> Data
			{
				get { return this.data; }
			}

			protected CustomSerialIOBase()
			{
				this.data = new Dictionary<string,object>();
			}

			/// <summary>
			/// Clears all contained data.
			/// </summary>
			public void Clear()
			{
				this.data.Clear();
			}
			/// <summary>
			/// Writes the specified name and value.
			/// </summary>
			/// <param name="name">
			/// The name to which the written value is mapped. 
			/// May, for example, be the name of a <see cref="System.Reflection.FieldInfo">Field</see>
			/// to which the written value belongs, but there are no naming restrictions, except that one name can't be used twice.
			/// </param>
			/// <param name="value">The value to write.</param>
			/// <seealso cref="IDataWriter"/>
			public void WriteValue(string name, object value)
			{
				this.data[name] = value;
			}
			/// <summary>
			/// Reads the value that is associated with the specified name.
			/// </summary>
			/// <param name="name">The name that is used for retrieving the value.</param>
			/// <returns>The value that has been read using the given name.</returns>
			/// <seealso cref="IDataReader"/>
			/// <seealso cref="ReadValue{T}(string)"/>
			/// <seealso cref="ReadValue{T}(string, out T)"/>
			public object ReadValue(string name)
			{
				object result;
				if (this.data.TryGetValue(name, out result))
					return result;
				else
					return null;
			}
			/// <summary>
			/// Reads the value that is associated with the specified name.
			/// </summary>
			/// <typeparam name="U">The expected value type.</typeparam>
			/// <param name="name">The name that is used for retrieving the value.</param>
			/// <returns>The value that has been read and cast using the given name and type.</returns>
			/// <seealso cref="IDataReader"/>
			/// <seealso cref="ReadValue(string)"/>
			/// <seealso cref="ReadValue{U}(string, out U)"/>
			public U ReadValue<U>(string name)
			{
				object read = this.ReadValue(name);
				if (read is U)
					return (U)read;
				else if (read == null)
					return default(U);
				else
				{
					try { return (U)Convert.ChangeType(read, typeof(U), System.Globalization.CultureInfo.InvariantCulture); }
					catch (Exception) { return default(U); }
				}
			}
			/// <summary>
			/// Reads the value that is associated with the specified name.
			/// </summary>
			/// <typeparam name="U">The expected value type.</typeparam>
			/// <param name="name">The name that is used for retrieving the value.</param>
			/// <param name="value">The value that has been read and cast using the given name and type.</param>
			/// <seealso cref="IDataReader"/>
			/// <seealso cref="ReadValue(string)"/>
			/// <seealso cref="ReadValue{U}(string)"/>
			public void ReadValue<U>(string name, out U value)
			{
				value = this.ReadValue<U>(name);
			}
		}

		/// <summary>
		/// Describes the serialization header of an object that is being de/serialized.
		/// </summary>
		protected class ObjectHeader
		{
			private	uint			objectId;
			private	DataType		dataType;
			private	SerializeType	serializeType;
			private	string			typeString;

			/// <summary>
			/// [GET] The objects unique ID. May be zero for non-referenced object types.
			/// </summary>
			public uint ObjectId
			{
				get { return this.objectId; }
			}
			/// <summary>
			/// [GET] The objects data type.
			/// </summary>
			public DataType DataType
			{
				get { return this.dataType; }
			}
			/// <summary>
			/// [GET] The objects resolved serialization type information. May be unavailable / null when loading objects.
			/// </summary>
			public SerializeType SerializeType
			{
				get { return this.serializeType; }
			}
			/// <summary>
			/// [GET] The objects resolved type information. May be unavailable / null when loading objects.
			/// </summary>
			public TypeInfo ObjectType
			{
				get { return (this.serializeType != null) ? this.serializeType.Type : null; }
			}
			/// <summary>
			/// [GET] The string representing this objects type in the serialized data stream.
			/// </summary>
			public string TypeString
			{
				get { return this.typeString; }
			}
			/// <summary>
			/// [GET] Whether or not the object is considered a primitive value according to its <see cref="DataType"/>.
			/// </summary>
			public bool IsPrimitive
			{
				get { return this.dataType.IsPrimitiveType(); }
			}
			/// <summary>
			/// [GET] Returns whether this kind of object requires an explicit <see cref="ObjectType"/> to be fully described described during serialization.
			/// </summary>
			public bool IsObjectTypeRequired
			{
				get { return this.dataType.HasTypeName(); }
			}
			/// <summary>
			/// [GET] Returns whether this kind of object requires an <see cref="ObjectId"/> to be fully described during serialization.
			/// </summary>
			public bool IsObjectIdRequired
			{
				get { return this.dataType.HasObjectId(); }
			}
			
			public ObjectHeader(uint id, DataType dataType, SerializeType serializeType)
			{
				this.objectId = id;
				this.dataType = dataType;
				this.serializeType = serializeType;
				this.typeString = (serializeType != null) ? serializeType.TypeString : null;
			}
			public ObjectHeader(uint id, DataType dataType, string unresolvedTypeString)
			{
				this.objectId = id;
				this.dataType = dataType;
				this.serializeType = null;
				this.typeString = unresolvedTypeString;
			}
		}


		/// <summary>
		/// The de/serialization <see cref="Duality.Log"/>.
		/// </summary>
		/// <summary>
		/// A list of <see cref="System.Reflection.FieldInfo">field</see> blockers. If any registered field blocker
		/// returns true upon serializing a specific field, a default value is assumed instead.
		/// </summary>
		protected	List<FieldBlocker>	fieldBlockers	= new List<FieldBlocker>();
		/// <summary>
		/// Manages object IDs during de/serialization.
		/// </summary>
		protected	ObjectIdManager		idManager		= new ObjectIdManager();

		private	Stream	stream			= null;
		private	bool	opInProgress	= false;
		private	bool	disposed		= false;
		private	Log		log				= Log.Core;

		
		/// <summary>
		/// [GET] Can this <see cref="Serializer"/> read data?
		/// </summary>
		public virtual bool CanRead
		{
			get { return this.stream != null && this.stream.CanRead; }
		}
		/// <summary>
		/// [GET] Can this <see cref="Serializer"/> write data?
		/// </summary>
		public virtual bool CanWrite
		{
			get { return this.stream != null && this.stream.CanWrite; }
		}
		/// <summary>
		/// [GET / SET] The target <see cref="Stream"/> this <see cref="Serializer"/> operates on (i.e. reads from and writes to).
		/// </summary>
		public Stream TargetStream
		{
			get { return this.stream; }
			set
			{
				if (this.opInProgress) throw new InvalidOperationException("Can't change the target Stream while an I/O operation is in progress.");
				if (this.stream != value)
				{
					Stream oldValue = this.stream;
					this.stream = value;
					this.OnTargetStreamChanged(oldValue, this.stream);
				}
			}
		}
		/// <summary>
		/// [GET / SET] The local de/serialization <see cref="Duality.Log"/>.
		/// </summary>
		public Log LocalLog
		{
			get { return this.log; }
			set { this.log = value ?? new Log("Serialize"); }
		}
		/// <summary>
		/// [GET] Enumerates registered <see cref="System.Reflection.FieldInfo">field</see> blockers. If any registered field blocker
		/// returns true upon serializing a specific field, a default value is assumed instead.
		/// </summary>
		public IEnumerable<FieldBlocker> FieldBlockers
		{
			get { return this.fieldBlockers; }
		}
		/// <summary>
		/// [GET] Whether this formatter has been disposed. A disposed object cannot be used anymore.
		/// </summary>
		public bool Disposed
		{
			get { return this.disposed; }
		}


		protected Serializer() {}
		~Serializer()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose(true);
		}
		private void Dispose(bool manually)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.OnDisposed(manually);
			}
		}
		protected virtual void OnDisposed(bool manually)
		{
			this.TargetStream = null;
		}

		
		/// <summary>
		/// Reads a single object and returns it.
		/// </summary>
		/// <returns></returns>
		public object ReadObject()
		{
			if (!this.CanRead) throw new InvalidOperationException("Can't read object from a write-only serializer!");
			this.BeginReadOperation();
			object result = this.ReadObjectData();
			this.EndReadOperation();
			return result;
		}
		/// <summary>
		/// Reads a single object, casts it to the specified Type and returns it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T ReadObject<T>()
		{
			object result = this.ReadObject();
			return result is T ? (T)result : default(T);
		}
		/// <summary>
		/// Reads a single object, casts it to the specified Type and returns it via output parameter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		public void ReadObject<T>(out T obj)
		{
			object result = this.ReadObject();
			obj = result is T ? (T)result : default(T);
		}

		/// <summary>
		/// Writes a single object.
		/// </summary>
		/// <param name="obj"></param>
		public void WriteObject(object obj)
		{
			if (!this.CanWrite) throw new InvalidOperationException("Can't write object to a read-only serializer!");
			this.BeginWriteOperation();
			this.WriteObjectData(obj);
			this.EndWriteOperation();
		}
		/// <summary>
		/// Writes a single object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		public void WriteObject<T>(T obj)
		{
			this.WriteObject((object)obj);
		}

		/// <summary>
		/// Unregisters all <see cref="FieldBlockers"/>.
		/// </summary>
		public void ClearFieldBlockers()
		{
			this.fieldBlockers.Clear();
		}
		/// <summary>
		/// Registers a new <see cref="FieldBlockers">FieldBlocker</see>.
		/// </summary>
		/// <param name="blocker"></param>
		public void AddFieldBlocker(FieldBlocker blocker)
		{
			if (this.fieldBlockers.Contains(blocker)) return;
			this.fieldBlockers.Add(blocker);
		}
		/// <summary>
		/// Unregisters an existing <see cref="FieldBlockers">FieldBlocker</see>.
		/// </summary>
		/// <param name="blocker"></param>
		public void RemoveFieldBlocker(FieldBlocker blocker)
		{
			this.fieldBlockers.Remove(blocker);
		}
		/// <summary>
		/// Determines whether a specific <see cref="System.Reflection.FieldInfo">field</see> is blocked.
		/// Blocked fields, despite being generally flagged as being serializable, are omitted during de/serialization and retain their default value.
		/// </summary>
		/// <param name="field">The <see cref="System.Reflection.FieldInfo">field</see> in question.</param>
		/// <param name="obj">The object where this field originates from.</param>
		/// <returns>True, if the <see cref="System.Reflection.FieldInfo">field</see> is blocked, false if not.</returns>
		public bool IsFieldBlocked(FieldInfo field, object obj)
		{
			return this.fieldBlockers.Any(blocker => blocker(field, obj));
		}

		/// <summary>
		/// Determines whether or not the specified <see cref="Stream"/> matches the required format by
		/// this <see cref="Serializer"/>. This is used to determine which <see cref="Serializer"/> can be
		/// used for any given input <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		protected abstract bool MatchesStreamFormat(Stream stream);
		/// <summary>
		/// Writes the specified object including all referenced objects.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		protected abstract object ReadObjectData();
		/// <summary>
		/// Reads an object including all referenced objects.
		/// </summary>
		/// <returns>The object that has been read.</returns>
		protected abstract void WriteObjectData(object obj);
		/// <summary>
		/// Called when the target stream this <see cref="Serializer"/> operates on has changed.
		/// </summary>
		/// <param name="oldStream"></param>
		/// <param name="newStream"></param>
		protected virtual void OnTargetStreamChanged(Stream oldStream, Stream newStream) { }
		/// <summary>
		/// Signals the beginning of an atomic ReadObject operation.
		/// </summary>
		protected virtual void OnBeginReadOperation() { }
		/// <summary>
		/// Signals the beginning of an atomic WriteObject operation.
		/// </summary>
		protected virtual void OnBeginWriteOperation() { }
		/// <summary>
		/// Signals the end of an atomic ReadObject operation.
		/// </summary>
		protected virtual void OnEndReadOperation() { }
		/// <summary>
		/// Signals the end of an atomic WriteObject operation.
		/// </summary>
		protected virtual void OnEndWriteOperation() { }
		
		/// <summary>
		/// Prepares an object for serialization and generates its header information.
		/// </summary>
		/// <param name="obj">The object to write</param>
		protected ObjectHeader PrepareWriteObject(object obj)
		{
			Type objType = obj.GetType();
			SerializeType objSerializeType = GetSerializeType(objType);
			DataType dataType = objSerializeType.DataType;
			uint objId = 0;
			
			// Check whether it's going to be an ObjectRef or not
			if (objSerializeType.CanBeReferenced)
			{
				bool newId;
				objId = this.idManager.Request(obj, out newId);

				// If its not a new id, write a reference
				if (!newId) dataType = DataType.ObjectRef;
			}

			// Check whether the object is expected to be serialized
			if (dataType != DataType.ObjectRef &&
				!objSerializeType.IsSerializable && 
				!typeof(ISerializeExplicit).GetTypeInfo().IsAssignableFrom(objSerializeType.Type) &&
				objSerializeType.Surrogate == null) 
			{
				this.LocalLog.WriteWarning("Ignoring object of Type '{0}' which is flagged with the {1}.", 
					Log.Type(objSerializeType.Type),
					typeof(DontSerializeAttribute).Name);
				return null;
			}

			// Generate object header information
			return new ObjectHeader(objId, dataType, objSerializeType);
		}


		/// <summary>
		/// Logs an error that occurred during <see cref="Duality.Serialization.ISerializeExplicit">custom serialization</see>.
		/// </summary>
		/// <param name="objId">The object id of the affected object.</param>
		/// <param name="serializeType">The <see cref="System.Type"/> of the affected object.</param>
		/// <param name="e">The <see cref="System.Exception"/> that occurred.</param>
		protected void LogCustomSerializationError(uint objId, TypeInfo serializeType, Exception e)
		{
			this.log.WriteError(
				"An error occurred in custom serialization in object Id {0} of type '{1}': {2}",
				objId,
				Log.Type(serializeType),
				Log.Exception(e));
		}
		/// <summary>
		/// Logs an error that occurred during <see cref="Duality.Serialization.ISerializeExplicit">custom deserialization</see>.
		/// </summary>
		/// <param name="objId">The object id of the affected object.</param>
		/// <param name="serializeType">The <see cref="System.Type"/> of the affected object.</param>
		/// <param name="e">The <see cref="System.Exception"/> that occurred.</param>
		protected void LogCustomDeserializationError(uint objId, TypeInfo serializeType, Exception e)
		{
			this.log.WriteError(
				"An error occurred in custom deserialization in object Id {0} of type '{1}': {2}",
				objId,
				Log.Type(serializeType),
				Log.Exception(e));
		}
		
		/// <summary>
		/// Assigns the specified value to an objects field.
		/// </summary>
		/// <param name="objSerializeType"></param>
		/// <param name="obj"></param>
		/// <param name="fieldName"></param>
		/// <param name="fieldValue"></param>
		protected void AssignValueToField(SerializeType objSerializeType, object obj, string fieldName, object fieldValue)
		{
			if (obj == null) return;

			// Retrieve field
			FieldInfo field = null;
			if (objSerializeType != null)
			{
				field = objSerializeType.Fields.FirstOrDefault(f => f.Name == fieldName);
				if (field == null)
				{
					field = ReflectionHelper.ResolveMember("F:" + objSerializeType.TypeString + ":" + fieldName) as FieldInfo;
				}
			}

			if (field == null)
			{
				this.HandleAssignValueToField(objSerializeType, obj, fieldName, fieldValue);
				return;
			}

			if (field.HasAttributeCached<DontSerializeAttribute>())
			{
				this.HandleAssignValueToField(objSerializeType, obj, fieldName, fieldValue);
				return;
			}

			TypeInfo fieldTypeInfo = field.FieldType.GetTypeInfo();
			if (fieldValue != null && !fieldTypeInfo.IsInstanceOfType(fieldValue))
			{
				if (!this.HandleAssignValueToField(objSerializeType, obj, fieldName, fieldValue))
				{
					this.LocalLog.WriteWarning("Actual Type '{0}' of object value in field '{1}' does not match reflected FieldType '{2}'. Trying to convert...'", 
						fieldValue != null ? Log.Type(fieldValue.GetType()) : "unknown", 
						fieldName, 
						Log.Type(field.FieldType));
					this.LocalLog.PushIndent();
					object castVal;
					try
					{
						if (fieldTypeInfo.IsEnum)
						{
							castVal = Convert.ChangeType(fieldValue, Enum.GetUnderlyingType(field.FieldType), System.Globalization.CultureInfo.InvariantCulture);
							castVal = Enum.ToObject(field.FieldType, castVal);
						}
						else
						{
							castVal = Convert.ChangeType(fieldValue, field.FieldType, System.Globalization.CultureInfo.InvariantCulture);
						}
						this.LocalLog.Write("...succeeded! Assigning value '{0}'", castVal);
						field.SetValue(obj, castVal);
					}
					catch (Exception)
					{
						this.LocalLog.WriteWarning("...failed! Discarding value '{0}'", fieldValue);
					}
					this.LocalLog.PopIndent();
				}
				return;
			}

			if (fieldValue == null && fieldTypeInfo.IsValueType) fieldValue = fieldTypeInfo.CreateInstanceOf();
			field.SetValue(obj, fieldValue);
		}
		/// <summary>
		/// Assigns the specified value to a specific array index.
		/// </summary>
		/// <param name="objSerializeType"></param>
		/// <param name="obj"></param>
		/// <param name="fieldName"></param>
		/// <param name="fieldValue"></param>
		protected void AssignValueToArray(SerializeType elementSerializeType, Array array, int index, object value)
		{
			if (array == null) return;
			if (value != null && !elementSerializeType.Type.IsInstanceOfType(value))
			{
				this.LocalLog.WriteWarning(
					"Actual Type '{0}' of array element value at index {1} does not match reflected array element type '{2}'. Skipping item.", 
					value != null ? Log.Type(value.GetType()) : "unknown", 
					index, 
					Log.Type(elementSerializeType.Type));
				return;
			}

			array.SetValue(value, index);
		}
		/// <summary>
		/// Resolves the specified Type.
		/// </summary>
		/// <param name="typeId"></param>
		/// <param name="objId"></param>
		/// <returns></returns>
		protected Type ResolveType(string typeId, uint objId = uint.MaxValue)
		{
			Type result = ReflectionHelper.ResolveType(typeId);
			if (result == null)
			{
				if (objId != uint.MaxValue)
					this.log.WriteError("Can't resolve Type '{0}' in object Id {1}. Type not found.", typeId, objId);
				else
					this.log.WriteError("Can't resolve Type '{0}'. Type not found.", typeId);
			}
			return result;
		}
		/// <summary>
		/// Resolves the specified Member.
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="objId"></param>
		/// <returns></returns>
		protected MemberInfo ResolveMember(string memberId, uint objId = uint.MaxValue)
		{
			MemberInfo result = ReflectionHelper.ResolveMember(memberId);
			if (result == null)
			{
				if (objId != uint.MaxValue)
					this.log.WriteError("Can't resolve Member '{0}' in object Id {1}. Member not found.", memberId, objId);
				else
					this.log.WriteError("Can't resolve Member '{0}'. Member not found.", memberId);
			}
			return result;
		}
		/// <summary>
		/// Resolves the specified Enum value.
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="enumField"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected Enum ResolveEnumValue(Type enumType, string enumField, long value)
		{
			try
			{
				object result = Enum.Parse(enumType, enumField);
				if (result != null) return (Enum)result;
			}
			catch (Exception) {}

			string memberId = "F:" + enumType.GetTypeId() + ":" + enumField;
			MemberInfo member = ReflectionHelper.ResolveMember(memberId);
			if (member != null)
			{
				try
				{
					object result = Enum.Parse(enumType, member.Name);
					if (result != null) return (Enum)result;
				}
				catch (Exception) {}
			}
			
			this.log.WriteWarning("Can't parse enum value '{0}' of Type '{1}'. Using numerical value '{2}' instead.", enumField, Log.Type(enumType), value);
			return (Enum)Enum.ToObject(enumType, value);
		}

		private void BeginReadOperation()
		{
			if (this.opInProgress) throw new InvalidOperationException("Can't begin a new operation before ending the previous one.");
			if (this.stream == null) throw new InvalidOperationException("Can't read data, because no target Stream was defined.");
			if (!this.CanRead) throw new InvalidOperationException("Can't read data, because the Serializer doesn't support it.");

			this.opInProgress = true;

			this.OnBeginReadOperation();
		}
		private void BeginWriteOperation()
		{
			if (this.opInProgress) throw new InvalidOperationException("Can't begin a new operation before ending the previous one.");
			if (this.stream == null) throw new InvalidOperationException("Can't write data, because no target Stream was defined.");
			if (!this.CanWrite) throw new InvalidOperationException("Can't write data, because the Serializer doesn't support it.");

			this.opInProgress = true;

			this.OnBeginWriteOperation();
		}
		private void EndReadOperation()
		{
			if (!this.opInProgress) throw new InvalidOperationException("Can't end the current operation, because no operation is in progress.");

			this.idManager.Clear();
			this.opInProgress = false;

			this.OnEndReadOperation();
		}
		private void EndWriteOperation()
		{
			if (!this.opInProgress) throw new InvalidOperationException("Can't end the current operation, because no operation is in progress.");

			this.idManager.Clear();
			this.opInProgress = false;

			this.OnEndWriteOperation();
		}
		private bool HandleAssignValueToField(SerializeType objSerializeType, object obj, string fieldName, object fieldValue)
		{
			AssignFieldError error = new AssignFieldError(objSerializeType, obj, fieldName, fieldValue);
			return HandleSerializeError(error);
		}


		private	static List<Type>						availableSerializerTypes	= new List<Type>();
		private	static List<Serializer>					tempCheckSerializers		= new List<Serializer>();
		private	static Dictionary<Type,SerializeType>	serializeTypeCache			= new Dictionary<Type,SerializeType>();
		private	static List<SerializeErrorHandler>		serializeHandlerCache		= new List<SerializeErrorHandler>();
		private	static List<ISerializeSurrogate>		surrogates					= null;
		private static Type								defaultSerializer			= null;

		/// <summary>
		/// [GET / SET] The default <see cref="Serializer"/> type to use, if no other is specified.
		/// </summary>
		public static Type DefaultType
		{
			get
			{
				// If we don't know yet, determine the default serialization method to use
				if (defaultSerializer == null)
				{
					InitDefaultMethod();
				}

				// If we still don't know, assume XML serialization, because we really need a default.
				return defaultSerializer ?? typeof(XmlSerializer);
			}
			set { defaultSerializer = value; }
		}
		/// <summary>
		/// [GET] Enumerates all available <see cref="Serializer"/> types.
		/// </summary>
		public static IEnumerable<Type> AvailableTypes
		{
			get
			{
				if (availableSerializerTypes.Count == 0)
				{
					availableSerializerTypes = new List<Type>();
					foreach (TypeInfo typeInfo in DualityApp.GetAvailDualityTypes(typeof(Serializer)))
					{
						if (typeInfo.IsAbstract) continue;
						availableSerializerTypes.Add(typeInfo.AsType());
					}
				}
				return availableSerializerTypes;
			}
		}
		/// <summary>
		/// [GET] A list of internal, temporary <see cref="Serializer"/> instances to check for Stream compatibility.
		/// </summary>
		private static IList<Serializer> TempCheckSerializers
		{
			get
			{
				if (tempCheckSerializers.Count == 0)
				{
					tempCheckSerializers = new List<Serializer>();
					foreach (TypeInfo typeInfo in DualityApp.GetAvailDualityTypes(typeof(Serializer)))
					{
						if (typeInfo.IsAbstract) continue;

						Serializer instance = typeInfo.CreateInstanceOf() as Serializer;
						if (instance == null) continue;

						tempCheckSerializers.Add(instance);
					}
				}
				return tempCheckSerializers;
			}
		}
		
		static Serializer()
		{
			ReflectionHelper.MemberResolve	+= new EventHandler<ResolveMemberEventArgs>(ReflectionHelper_MemberResolve);
			ReflectionHelper.TypeResolve	+= new EventHandler<ResolveMemberEventArgs>(ReflectionHelper_TypeResolve);
		}

		/// <summary>
		/// Uses a (seekable, random access) Stream to detect the serializer that can handle it.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static Type Detect(Stream stream)
		{
			if (!stream.CanRead || !stream.CanSeek) throw new ArgumentException("The specified Stream needs to be readable, seekable and provide random-access functionality.");
			if (stream.Length == 0) throw new InvalidOperationException("The specified stream must not be empty.");

			IList<Serializer> tempSerializers = TempCheckSerializers;
			for (int i = tempSerializers.Count - 1; i >= 0; i--)
			{
				Serializer serializer = tempSerializers[i];
				long oldPos = stream.Position;
				try
				{
					if (serializer.MatchesStreamFormat(stream))
					{
						return serializer.GetType();
					}
				}
				catch (Exception e)
				{
					Log.Core.WriteError(
						"An error occurred while asking {0} whether it matched the format of a certain Stream: {1}",
						Log.Type(serializer.GetType()),
						Log.Exception(e));
				}
				finally
				{
					stream.Seek(oldPos, SeekOrigin.Begin);
				}
			}

			return null;
		}
		/// <summary>
		/// Creates a new <see cref="Serializer"/> using the specified stream for I/O.
		/// </summary>
		/// <param name="stream">The stream to use.</param>
		/// <param name="preferredSerializer">
		/// The serialization method to prefer. Auto-detection is used when not specified explicitly
		/// and the underlying stream supports seeking / random access. Otherwise, the <see cref="DefaultType"/> is used.
		/// </param>
		/// <returns>A newly created <see cref="Serializer"/> meeting the specified criteria.</returns>
		public static Serializer Create(Stream stream, Type preferredSerializer = null)
		{
			// If no preferred serializer is specified, try to detect which one to use
			if (preferredSerializer == null && stream.CanRead && stream.CanSeek && stream.Length > 0 && stream.Position < stream.Length)
				preferredSerializer = Detect(stream);

			// If detection wasn't possible, assume the default serializer.
			if (preferredSerializer == null)
				preferredSerializer = DefaultType;

			// Do a consistency check on the serializer type we got passed - is it really a Serializer?
			TypeInfo baseTypeInfo = typeof(Serializer).GetTypeInfo();
			TypeInfo serializerTypeInfo = preferredSerializer.GetTypeInfo();
			if (!baseTypeInfo.IsAssignableFrom(serializerTypeInfo))
				throw new ArgumentException("Can't use a non-{0} Type as a {0}.", baseTypeInfo.Name);

			// Create an instance of the Serializer, configure and return it
			Serializer serializer = serializerTypeInfo.CreateInstanceOf() as Serializer;
			serializer.TargetStream = stream;
			return serializer;
		}
		/// <summary>
		/// Reads an object of the specified Type from an existing data file, expecting that it might fail.
		/// This method does not throw an Exception when the file does not exist or another
		/// error occurred during the read operation. Instead, it will simply return null in these cases.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="file"></param>
		/// <param name="preferredSerializer"></param>
		/// <returns></returns>
		public static T TryReadObject<T>(string file, Type preferredSerializer = null)
		{
			try
			{
				if (!FileOp.Exists(file)) return default(T);
				using (Stream str = FileOp.Open(file, FileAccessMode.Read))
				{
					return Serializer.TryReadObject<T>(str, preferredSerializer);
				}
			}
			catch (Exception)
			{
				return default(T);
			}
		}
		/// <summary>
		/// Reads an object of the specified Type from an existing data Stream, expecting that it might fail.
		/// This method does not throw an Exception when the file does not exist or an expected
		/// error occurred during the read operation. Instead, it will simply return null in these cases.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="preferredSerializer"></param>
		/// <returns></returns>
		public static T TryReadObject<T>(Stream stream, Type preferredSerializer = null)
		{
			try
			{
				using (Serializer formatter = Serializer.Create(stream, preferredSerializer))
				{
					return formatter.ReadObject<T>();
				}
			}
			catch (Exception)
			{
				return default(T);
			}
		}
		/// <summary>
		/// Reads an object of the specified Type from an existing data file. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="file"></param>
		/// <param name="preferredSerializer"></param>
		/// <returns></returns>
		public static T ReadObject<T>(string file, Type preferredSerializer = null)
		{
			using (Stream str = FileOp.Open(file, FileAccessMode.Read))
			{
				return Serializer.ReadObject<T>(str, preferredSerializer);
			}
		}
		/// <summary>
		/// Reads an object of the specified Type from an existing data Stream.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="preferredSerializer"></param>
		/// <returns></returns>
		public static T ReadObject<T>(Stream stream, Type preferredSerializer = null)
		{
			using (Serializer formatter = Serializer.Create(stream, preferredSerializer))
			{
				return formatter.ReadObject<T>();
			}
		}
		/// <summary>
		/// Saves an object to the specified data file. If it already exists, the file will be overwritten.
		/// Automatically creates the appropriate directory structure, if it doesn't exist yet.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="file"></param>
		/// <param name="preferredSerializer"></param>
		public static void WriteObject<T>(T obj, string file, Type preferredSerializer = null)
		{
			string dirName = PathOp.GetDirectoryName(file);
			if (!string.IsNullOrEmpty(dirName) && !DirectoryOp.Exists(dirName)) DirectoryOp.Create(dirName);
			using (Stream str = FileOp.Create(file))
			{
				Serializer.WriteObject<T>(obj, str, preferredSerializer);
			}
		}
		/// <summary>
		/// Saves an object to the specified data stream.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="stream"></param>
		/// <param name="preferredSerializer"></param>
		public static void WriteObject<T>(T obj, Stream stream, Type preferredSerializer = null)
		{
			using (Serializer formatter = Serializer.Create(stream, preferredSerializer ?? DefaultType))
			{
				formatter.WriteObject(obj);
			}
		}

		/// <summary>
		/// Returns the <see cref="SerializeType"/> of a Type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected static SerializeType GetSerializeType(Type type)
		{
			if (type == null) return null;

			SerializeType result;
			if (serializeTypeCache.TryGetValue(type, out result)) return result;

			result = new SerializeType(type);
			serializeTypeCache[type] = result;
			return result;
		}
		/// <summary>
		/// Retrieves a matching <see cref="Duality.Serialization.ISerializeSurrogate"/> for the specified <see cref="System.Type"/>.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> to retrieve a <see cref="Duality.Serialization.ISerializeSurrogate"/> for.</param>
		/// <returns></returns>
		internal static ISerializeSurrogate GetSurrogateFor(TypeInfo type)
		{
			if (surrogates == null)
			{
				surrogates = DualityApp.GetAvailDualityTypes(typeof(ISerializeSurrogate))
					.Where(t => !t.IsAbstract && !t.IsInterface)
					.Select(t => t.CreateInstanceOf())
					.OfType<ISerializeSurrogate>()
					.NotNull()
					.ToList();
				surrogates.StableSort((s1, s2) => s1.Priority - s2.Priority);
			}
			return surrogates.FirstOrDefault(s => s.MatchesType(type));
		}
		/// <summary>
		/// Attempts to handle a serialization error dynamically by invoking available <see cref="SerializeErrorHandler">SerializeErrorHandlers</see>.
		/// </summary>
		/// <param name="error"></param>
		/// <returns>Returns true, if the error has been handled successfully.</returns>
		protected static bool HandleSerializeError(SerializeError error)
		{
			if (error.Handled) return true;
			if (serializeHandlerCache.Count == 0)
			{
				IEnumerable<TypeInfo> handlerTypes = DualityApp.GetAvailDualityTypes(typeof(SerializeErrorHandler));
				foreach (TypeInfo handlerType in handlerTypes)
				{
					if (handlerType.IsAbstract) continue;
					try
					{
						SerializeErrorHandler handler = handlerType.CreateInstanceOf() as SerializeErrorHandler;
						if (handler != null)
						{
							serializeHandlerCache.Add(handler);
						}
					}
					catch (Exception) {}
				}
				serializeHandlerCache.StableSort((a, b) => b.Priority - a.Priority);
			}
			foreach (SerializeErrorHandler handler in serializeHandlerCache)
			{
				try
				{
					handler.HandleError(error);
					if (error.Handled) return true;
				}
				catch (Exception e)
				{
					Log.Core.WriteError("An error occurred while trying to perform a serialization fallback: {0}", Log.Exception(e));
				}
			}
			return false;
		}
		
		internal static void InitDefaultMethod()
		{
			// By default, assume one of the builtin serializers, depending on execution environment
			if (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor)
				defaultSerializer = typeof(XmlSerializer);
			else
				defaultSerializer = typeof(BinarySerializer);

			// Search for actual Resource files and detect their serialization format
			if (DirectoryOp.Exists(DualityApp.DataDirectory))
			{
				foreach (string resFile in DirectoryOp.GetFiles(DualityApp.DataDirectory, true))
				{
					if (!resFile.EndsWith(Resource.FileExt, StringComparison.OrdinalIgnoreCase)) continue;
					using (Stream stream = FileOp.Open(resFile, FileAccessMode.Read))
					{
						try
						{
							Type matchingSerializer = Detect(stream);
							if (matchingSerializer != null)
							{
								defaultSerializer = matchingSerializer;
								break;
							}
						}
						catch (Exception) {}
					}
				}
			}
		}
		internal static void ClearTypeCache()
		{
			foreach (Serializer tempSerializer in tempCheckSerializers)
			{
				tempSerializer.Dispose();
			}
			tempCheckSerializers.Clear();
			availableSerializerTypes.Clear();
			surrogates = null;
			serializeTypeCache.Clear();
			serializeHandlerCache.Clear();

			// If our default serializer isn't defined in the main Assembly, forget about it
			if (defaultSerializer != null)
			{
				bool isMainAssembly = defaultSerializer.GetTypeInfo().Assembly == typeof(DualityApp).GetTypeInfo().Assembly;
				if (!isMainAssembly)
				{
					defaultSerializer = null;
				}
			}
		}

		private static void ReflectionHelper_MemberResolve(object sender, ResolveMemberEventArgs e)
		{
			ResolveMemberError error = new ResolveMemberError(e.MemberId);
			if (HandleSerializeError(error))
				e.ResolvedMember = error.ResolvedMember;
		}
		private static void ReflectionHelper_TypeResolve(object sender, ResolveMemberEventArgs e)
		{
			ResolveTypeError error = new ResolveTypeError(e.MemberId);
			if (HandleSerializeError(error))
				e.ResolvedMember = error.ResolvedType != null ? error.ResolvedType.GetTypeInfo() : null;
		}
	}
}
