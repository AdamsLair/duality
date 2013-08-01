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
		/// Buffer object for <see cref="Duality.Serialization.ISerializable">custom de/serialization</see>, 
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
			/// Writes the contained data to the specified serializer.
			/// </summary>
			/// <param name="formatter">The serializer to write data to.</param>
			public abstract void Serialize(T formatter);
			/// <summary>
			/// Reads data from the specified serializer
			/// </summary>
			/// <param name="formatter">The serializer to read data from.</param>
			public abstract void Deserialize(T formatter);
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
		/// The de/serialization <see cref="Duality.Log"/>.
		/// </summary>
		/// <summary>
		/// A list of <see cref="System.Reflection.FieldInfo">field</see> blockers. If any registered field blocker
		/// returns true upon serializing a specific field, a default value is assumed instead.
		/// </summary>
		protected	List<FieldBlocker>	fieldBlockers	= new List<FieldBlocker>();
		/// <summary>
		/// A list of <see cref="Duality.Serialization.ISurrogate">Serialization Surrogates</see>. If any of them
		/// matches the <see cref="System.Type"/> of an object that is to be serialized, instead of letting it
		/// serialize itsself, the <see cref="Duality.Serialization.ISurrogate"/> with the highest <see cref="Duality.Serialization.ISurrogate.Priority"/>
		/// is used instead.
		/// </summary>
		protected	List<ISurrogate>	surrogates		= new List<ISurrogate>();
		/// <summary>
		/// Manages object IDs during de/serialization.
		/// </summary>
		protected	ObjectIdManager		idManager		= new ObjectIdManager();

		private	bool	disposed	= false;
		private	Log		log			= Log.Core;


		/// <summary>
		/// [GET / SET] The de/serialization <see cref="Duality.Log"/>.
		/// </summary>
		public Log SerializationLog
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
		/// [GET] Enumerates registered <see cref="Duality.Serialization.ISurrogate">Serialization Surrogates</see>. If any of them
		/// matches the <see cref="System.Type"/> of an object that is to be serialized, instead of letting it
		/// serialize itsself, the <see cref="Duality.Serialization.ISurrogate"/> with the highest <see cref="Duality.Serialization.ISurrogate.Priority"/>
		/// is used instead.
		/// </summary>
		public IEnumerable<ISurrogate> Surrogates
		{
			get { return this.surrogates; }
		}
		/// <summary>
		/// [GET] Whether this binary serializer has been disposed. A disposed object cannot be used anymore.
		/// </summary>
		public bool Disposed
		{
			get { return this.disposed; }
		}


		protected Formatter()
		{
			this.AddSurrogate(new Surrogates.BitmapSurrogate());
			this.AddSurrogate(new Surrogates.DictionarySurrogate());
			this.AddSurrogate(new Surrogates.GuidSurrogate());
		}
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
		/// Writes the specified object including all referenced objects.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		public abstract object ReadObject();
		/// <summary>
		/// Reads an object including all referenced objects.
		/// </summary>
		/// <returns>The object that has been read.</returns>
		public abstract void WriteObject(object obj);
		
		/// <summary>
		/// Returns an object indicating a "null" value.
		/// </summary>
		/// <returns></returns>
		protected virtual object GetNullObject() 
		{
			return null;
		}
		/// <summary>
		/// Determines internal data for writing a given object.
		/// </summary>
		/// <param name="obj">The object to write</param>
		/// <param name="objSerializeType">The <see cref="Duality.Serialization.SerializeType"/> that describes the specified object.</param>
		/// <param name="dataType">The <see cref="Duality.Serialization.DataType"/> that is used for writing the specified object.</param>
		/// <param name="objId">An object id that is assigned to the specified object.</param>
		protected virtual void GetWriteObjectData(object obj, out SerializeType objSerializeType, out DataType dataType, out uint objId)
		{
			Type objType = obj.GetType();
			objSerializeType = objType.GetSerializeType();
			objId = 0;
			dataType = objSerializeType.DataType;
			
			// Check whether it's going to be an ObjectRef or not
			if (dataType == DataType.Array || dataType == DataType.Class || dataType == DataType.Delegate || dataType.IsMemberInfoType())
			{
				bool newId;
				objId = this.idManager.Request(obj, out newId);

				// If its not a new id, write a reference
				if (!newId) dataType = DataType.ObjectRef;
			}

			if (dataType != DataType.ObjectRef &&
				!objSerializeType.Type.IsSerializable && 
				!typeof(ISerializable).IsAssignableFrom(objSerializeType.Type) &&
				this.GetSurrogateFor(objSerializeType.Type) == null) 
			{
				this.SerializationLog.WriteWarning("Serializing object of Type '{0}' which isn't [Serializable]", Log.Type(objSerializeType.Type));
			}
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
		/// Unregisters all <see cref="Duality.Serialization.ISurrogate">Surrogates</see>.
		/// </summary>
		public void ClearSurrogates()
		{
			this.surrogates.Clear();
		}
		/// <summary>
		/// Registers a new <see cref="Duality.Serialization.ISurrogate">Surrogate</see>.
		/// </summary>
		/// <param name="surrogate"></param>
		public void AddSurrogate(ISurrogate surrogate)
		{
			if (this.surrogates.Contains(surrogate)) return;
			this.surrogates.Add(surrogate);
			this.surrogates.StableSort((s1, s2) => s1.Priority - s2.Priority);
		}
		/// <summary>
		/// Unregisters an existing <see cref="Duality.Serialization.ISurrogate">Surrogate</see>.
		/// </summary>
		/// <param name="surrogate"></param>
		public void RemoveSurrogate(ISurrogate surrogate)
		{
			this.surrogates.Remove(surrogate);
		}
		/// <summary>
		/// Retrieves a matching <see cref="Duality.Serialization.ISurrogate"/> for the specified <see cref="System.Type"/>.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> to retrieve a <see cref="Duality.Serialization.ISurrogate"/> for.</param>
		/// <returns></returns>
		public ISurrogate GetSurrogateFor(Type t)
		{
			return this.surrogates.FirstOrDefault(s => s.MatchesType(t));
		}

		/// <summary>
		/// Logs an error that occurred during <see cref="Duality.Serialization.ISerializable">custom serialization</see>.
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
		/// Logs an error that occurred during <see cref="Duality.Serialization.ISerializable">custom deserialization</see>.
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
				if (!this.HandleAssignValueToField(objSerializeType, obj, fieldName, fieldValue))
				{
					this.SerializationLog.WriteWarning("Field '{0}' not found. Discarding value '{1}'", fieldName, fieldValue);
				}
				return;
			}

			if (field.IsNotSerialized)
			{
				if (!this.HandleAssignValueToField(objSerializeType, obj, fieldName, fieldValue))
				{
					this.SerializationLog.WriteWarning("Field '{0}' flagged as [NonSerialized]. Discarding value '{1}'", fieldName, fieldValue);
				}
				return;
			}

			if (fieldValue != null && !field.FieldType.IsInstanceOfType(fieldValue))
			{
				if (!this.HandleAssignValueToField(objSerializeType, obj, fieldName, fieldValue))
				{
					this.SerializationLog.WriteWarning("Actual Type '{0}' of object value in field '{1}' does not match reflected FieldType '{2}'. Trying to convert...'", 
						fieldValue != null ? Log.Type(fieldValue.GetType()) : "unknown", 
						fieldName, 
						Log.Type(field.FieldType));
					this.SerializationLog.PushIndent();
					object castVal;
					try
					{
						castVal = Convert.ChangeType(fieldValue, field.FieldType, System.Globalization.CultureInfo.InvariantCulture);
						this.SerializationLog.Write("...succeeded! Assigning value '{0}'", castVal);
						field.SetValue(obj, castVal);
					}
					catch (Exception)
					{
						this.SerializationLog.WriteWarning("...failed! Discarding value '{0}'", fieldValue);
					}
					this.SerializationLog.PopIndent();
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


		private static FormattingMethod defaultMethod = FormattingMethod.Xml;
		/// <summary>
		/// [GET / SET] The default formatting method to use, if no other is specified.
		/// </summary>
		public static FormattingMethod DefaultMethod
		{
			get { return defaultMethod; }
			set { defaultMethod = value; }
		}
		
		internal static void InitDefaultMethod()
		{
			if (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor)
				defaultMethod = FormattingMethod.Xml;
			else
				defaultMethod = FormattingMethod.Binary;

			foreach (string anyResource in Directory.EnumerateFiles(DualityApp.DataDirectory, "*" + Resource.FileExt, SearchOption.AllDirectories))
			{
				using (FileStream stream = File.OpenRead(anyResource))
				{
					try
					{
						defaultMethod = XmlFormatterBase.IsXmlStream(stream) ? FormattingMethod.Xml : FormattingMethod.Binary;
						break;
					}
					catch (Exception) {}
				}
			}
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
					if (XmlFormatterBase.IsXmlStream(stream))
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
		/// Creates a new MetaFormat Formatter using the specified stream for I/O.
		/// </summary>
		/// <param name="stream">The stream to use.</param>
		/// <param name="method">
		/// The formatting method to prefer. If <see cref="FormattingMethod.Unknown"/> is specified, if the stream
		/// is read- and seekable, auto-detection is used. Otherwise, the <see cref="DefaultMethod">default formatting method</see> is used.
		/// </param>
		/// <returns>A newly created MetaFormat Formatter meeting the specified criteria.</returns>
		public static Formatter CreateMeta(Stream stream, FormattingMethod method = FormattingMethod.Unknown)
		{
			if (method == FormattingMethod.Unknown)
			{
				if (stream.CanRead && stream.CanSeek && stream.Length > 0)
				{
					if (XmlFormatterBase.IsXmlStream(stream))
						method = FormattingMethod.Xml;
					else
						method = FormattingMethod.Binary;
				}
				else
					method = defaultMethod;
			}

			if (method == FormattingMethod.Xml)
				return new XmlMetaFormatter(stream);
			else
				return new BinaryMetaFormatter(stream);
		}
	}
}
