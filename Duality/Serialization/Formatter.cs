using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace Duality.Serialization
{
	/// <summary>
	/// Represents a set of data formatting methods.
	/// </summary>
	public enum FormattingMethod
	{
		/// <summary>
		/// An unknown formatting method
		/// </summary>
		Unknown,

		/// <summary>
		/// Text-based XML formatting
		/// </summary>
		Xml,
		/// <summary>
		/// Binary formatting
		/// </summary>
		Binary
	}

	/// <summary>
	/// Base class for Dualitys serializers.
	/// </summary>
	public abstract class Formatter : IDisposable
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
		protected abstract class CustomSerialIOBase<T> : IDataReader, IDataWriter where T : Formatter
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
			public Type ObjectType
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
				this.typeString = serializeType != null ? serializeType.TypeString : null;
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

		private	bool	disposed	= false;
		private	Log		log			= Log.Core;

		
		/// <summary>
		/// [GET] Can this serializer read data?
		/// </summary>
		public abstract bool CanRead { get; }
		/// <summary>
		/// [GET] Can this serializer write data?
		/// </summary>
		public abstract bool CanWrite { get; }
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


		protected Formatter() {}
		~Formatter()
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
		protected virtual void OnDisposed(bool manually) {}

		
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
		/// Instead of writing the value of a blocked field, the matching <see cref="System.Type">Types</see>
		/// defautl value is assumed.
		/// </summary>
		/// <param name="field">The <see cref="System.Reflection.FieldInfo">field</see> in question.</param>
		/// <param name="obj">The object where this field originates from.</param>
		/// <returns>True, if the <see cref="System.Reflection.FieldInfo">field</see> is blocked, false if not.</returns>
		public bool IsFieldBlocked(FieldInfo field, object obj)
		{
			return this.fieldBlockers.Any(blocker => blocker(field, obj));
		}


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
		/// Signals the beginning of an atomic ReadObject operation.
		/// </summary>
		protected virtual void BeginReadOperation() {}
		/// <summary>
		/// Signals the beginning of an atomic WriteObject operation.
		/// </summary>
		protected virtual void BeginWriteOperation() {}
		/// <summary>
		/// Signals the end of an atomic ReadObject operation.
		/// </summary>
		protected virtual void EndReadOperation()
		{
			this.idManager.Clear();
		}
		/// <summary>
		/// Signals the end of an atomic WriteObject operation.
		/// </summary>
		protected virtual void EndWriteOperation()
		{
			this.idManager.Clear();
		}
		
		/// <summary>
		/// Prepares an object for serialization and generates its header information.
		/// </summary>
		/// <param name="obj">The object to write</param>
		protected ObjectHeader PrepareWriteObject(object obj)
		{
			Type objType = obj.GetType();
			SerializeType objSerializeType = objType.GetSerializeType();
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
				!objSerializeType.Type.IsSerializable && 
				!typeof(ISerializeExplicit).IsAssignableFrom(objSerializeType.Type) &&
				GetSurrogateFor(objSerializeType.Type) == null) 
			{
				this.LocalLog.WriteWarning("Ignoring object of Type '{0}' which isn't [Serializable]", Log.Type(objSerializeType.Type));
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
		protected void LogCustomSerializationError(uint objId, Type serializeType, Exception e)
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
		protected void LogCustomDeserializationError(uint objId, Type serializeType, Exception e)
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
					field = ReflectionHelper.ResolveMember("F:" + objSerializeType.TypeString + ":" + fieldName, false) as FieldInfo;
				}
			}

			if (field == null)
			{
				this.HandleAssignValueToField(objSerializeType, obj, fieldName, fieldValue);
				return;
			}

			if (field.IsNotSerialized)
			{
				this.HandleAssignValueToField(objSerializeType, obj, fieldName, fieldValue);
				return;
			}

			if (fieldValue != null && !field.FieldType.IsInstanceOfType(fieldValue))
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
						castVal = Convert.ChangeType(fieldValue, field.FieldType, System.Globalization.CultureInfo.InvariantCulture);
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

			if (fieldValue == null && field.FieldType.IsValueType) fieldValue = field.FieldType.CreateInstanceOf();
			field.SetValue(obj, fieldValue);
		}
		/// <summary>
		/// Resolves the specified Type.
		/// </summary>
		/// <param name="typeId"></param>
		/// <param name="objId"></param>
		/// <returns></returns>
		protected Type ResolveType(string typeId, uint objId = uint.MaxValue)
		{
			Type result = ReflectionHelper.ResolveType(typeId, false);
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
			MemberInfo result = ReflectionHelper.ResolveMember(memberId, false);
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
			MemberInfo member = ReflectionHelper.ResolveMember(memberId, false);
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

		private bool HandleAssignValueToField(SerializeType objSerializeType, object obj, string fieldName, object fieldValue)
		{
			AssignFieldError error = new AssignFieldError(objSerializeType, obj, fieldName, fieldValue);
			return ReflectionHelper.HandleSerializeError(error);
		}


		private	static List<ISerializeSurrogate>	surrogates		= null;
		private static FormattingMethod defaultMethod	= FormattingMethod.Xml;

		/// <summary>
		/// [GET / SET] The default formatting method to use, if no other is specified.
		/// </summary>
		public static FormattingMethod DefaultMethod
		{
			get { return defaultMethod; }
			set { defaultMethod = value; }
		}

		/// <summary>
		/// Retrieves a matching <see cref="Duality.Serialization.ISerializeSurrogate"/> for the specified <see cref="System.Type"/>.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> to retrieve a <see cref="Duality.Serialization.ISerializeSurrogate"/> for.</param>
		/// <returns></returns>
		protected static ISerializeSurrogate GetSurrogateFor(Type type)
		{
			if (surrogates == null)
			{
				surrogates = 
					DualityApp.GetAvailDualityTypes(typeof(ISerializeSurrogate))
					.Select(t => t.CreateInstanceOf())
					.OfType<ISerializeSurrogate>()
					.NotNull()
					.ToList();
				surrogates.StableSort((s1, s2) => s1.Priority - s2.Priority);
			}
			return surrogates.FirstOrDefault(s => s.MatchesType(type));
		}
		
		internal static void InitDefaultMethod()
		{
			if (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor)
				defaultMethod = FormattingMethod.Xml;
			else
				defaultMethod = FormattingMethod.Binary;

			if (Directory.Exists(DualityApp.DataDirectory))
			{
				foreach (string anyResource in Directory.EnumerateFiles(DualityApp.DataDirectory, "*" + Resource.FileExt, SearchOption.AllDirectories))
				{
					using (FileStream stream = File.OpenRead(anyResource))
					{
						try
						{
							defaultMethod = XmlFormatter.IsXmlStream(stream) ? FormattingMethod.Xml : FormattingMethod.Binary;
							break;
						}
						catch (Exception) {}
					}
				}
			}
		}
		internal static void ClearTypeCache()
		{
			surrogates = null;
		}

		/// <summary>
		/// Creates a new Formatter using the specified stream for I/O.
		/// </summary>
		/// <param name="stream">The stream to use.</param>
		/// <param name="method">
		/// The formatting method to prefer. If <see cref="FormattingMethod.Unknown"/> is specified, if the stream
		/// is read- and seekable, auto-detection is used. Otherwise, the <see cref="DefaultMethod">default formatting method</see> is used.
		/// </param>
		/// <returns>A newly created Formatter meeting the specified criteria.</returns>
		public static Formatter Create(Stream stream, FormattingMethod method = FormattingMethod.Unknown)
		{
			if (method == FormattingMethod.Unknown)
			{
				if (stream.CanRead && stream.CanSeek && stream.Length > 0)
				{
					if (XmlFormatter.IsXmlStream(stream))
						method = FormattingMethod.Xml;
					else
						method = FormattingMethod.Binary;
				}
				else
					method = defaultMethod;
			}

			if (method == FormattingMethod.Xml)
				return new XmlFormatter(stream);
			else
				return new BinaryFormatter(stream);
		}
		/// <summary>
		/// Reads an object of the specified Type from an existing data file, expecting that it might fail.
		/// This method does not throw an Exception when the file does not exist or another
		/// error occurred during the read operation. Instead, it will simply return null in these cases.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="file"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public static T TryReadObject<T>(string file, FormattingMethod method = FormattingMethod.Unknown)
		{
			try
			{
				if (!File.Exists(file)) return default(T);
				using (FileStream str = File.OpenRead(file))
				{
					return Formatter.TryReadObject<T>(str, method);
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
		/// <param name="method"></param>
		/// <returns></returns>
		public static T TryReadObject<T>(Stream stream, FormattingMethod method = FormattingMethod.Unknown)
		{
			try
			{
				using (Formatter formatter = Formatter.Create(stream, method))
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
		/// <param name="method"></param>
		/// <returns></returns>
		public static T ReadObject<T>(string file, FormattingMethod method = FormattingMethod.Unknown)
		{
			using (FileStream str = File.OpenRead(file))
			{
				return Formatter.ReadObject<T>(str, method);
			}
		}
		/// <summary>
		/// Reads an object of the specified Type from an existing data Stream.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public static T ReadObject<T>(Stream stream, FormattingMethod method = FormattingMethod.Unknown)
		{
			using (Formatter formatter = Formatter.Create(stream, method))
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
		/// <param name="method"></param>
		public static void WriteObject<T>(T obj, string file, FormattingMethod method = FormattingMethod.Unknown)
		{
			string dirName = Path.GetDirectoryName(file);
			if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
			using (FileStream str = File.Open(file, FileMode.Create))
			{
				Formatter.WriteObject<T>(obj, str, method);
			}
		}
		/// <summary>
		/// Saves an object to the specified data stream.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="stream"></param>
		/// <param name="method"></param>
		public static void WriteObject<T>(T obj, Stream stream, FormattingMethod method = FormattingMethod.Unknown)
		{
			using (Formatter formatter = Formatter.Create(stream, method))
			{
				formatter.WriteObject(obj);
			}
		}
	}
}
