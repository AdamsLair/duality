using System;

namespace Duality.Serialization
{
	/// <summary>
	/// De/Serializes an object instead of letting it de/serialize itsself or using a Reflection-driven approach.
	/// </summary>
	/// <seealso cref="Duality.Serialization.Surrogate{T}"/>
	public interface ISerializeSurrogate
	{
		/// <summary>
		/// [GET / SET] The object that is de/serialized
		/// </summary>
		object RealObject { get; set; }
		/// <summary>
		/// [GET] Returns a serializable object that represents the <see cref="RealObject"/>.
		/// </summary>
		ISerializeExplicit SurrogateObject { get; }
		/// <summary>
		/// [GET] If more than one registered ISurrogate is capable of de/serializing a given object type, the one
		/// with the highest priority is picked.
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// Checks whether this surrogate is able to de/serialize the specified object type.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> of the object in question.</param>
		/// <returns>True, if this surrogate is able to de/serialize such object, false if not.</returns>
		bool MatchesType(Type t);
		/// <summary>
		/// Writes constructor data for the replaced object. This will be used in a deserialization pre-pass 
		/// for constructing the object. Note that constructor data may not contain any object references to
		/// itsself, since it the object doesn't exist yet at this deserialization stage.
		/// </summary>
		/// <param name="writer">The <see cref="IDataWriter"/> to serialize constructor data to.</param>
		void WriteConstructorData(IDataWriter writer);
		/// <summary>
		/// Constructs an object in deserialization based on the constructor data that has been written in
		/// serialization using <see cref="WriteConstructorData"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to deserialize constructor data from.</param>
		/// <param name="objType">The <see cref="System.Type"/> of the object to create.</param>
		/// <returns>An instance of the specified <see cref="System.Type"/> that has been constructed using the provided data.</returns>
		object ConstructObject(IDataReader reader, Type objType);
	}
	/// <summary>
	/// Default base class for <see cref="ISerializeSurrogate">Serialization Surrogates</see>. It implements both
	/// <see cref="ISerializeSurrogate"/> and <see cref="ISerializeExplicit"/>, thus being able to fully perform de/serialization
	/// of a designated object type.
	/// </summary>
	/// <typeparam name="T">
	/// The base <see cref="System.Type"/> of objects this surrogate can replace.
	/// </typeparam>
	public abstract class Surrogate<T> : ISerializeSurrogate, ISerializeExplicit
	{
		private T realObj;
		
		object ISerializeSurrogate.RealObject
		{
			get { return this.realObj; }
			set { this.realObj = (T)value; }
		}
		ISerializeExplicit ISerializeSurrogate.SurrogateObject
		{
			get { return this; }
		}
		
		/// <summary>
		/// [GET] The object that is de/serialized
		/// </summary>
		protected T RealObject
		{
			get { return this.realObj; }
		}
		/// <summary>
		/// [GET] If more than one registered surrogate is capable of de/serializing a given object type, the one
		/// with the highest priority is picked.
		/// </summary>
		public virtual int Priority
		{
			get { return 0; }
		}


		/// <summary>
		/// Checks whether this surrogate is able to de/serialize the specified object type.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> of the object in question.</param>
		/// <returns>True, if this surrogate is able to de/serialize such object, false if not.</returns>
		public virtual bool MatchesType(Type t)
		{
			return typeof(T) == t;
		}
		
		/// <summary>
		/// Writes constructor data for the replaced object. This will be used in a deserialization pre-pass 
		/// for constructing the object. Note that constructor data may not contain any object references to
		/// itsself, since it the object doesn't exist yet at this deserialization stage.
		/// </summary>
		/// <param name="writer">The <see cref="IDataWriter"/> to serialize constructor data to.</param>
		public virtual void WriteConstructorData(IDataWriter writer) {}
		/// <summary>
		/// Writes the object data to the specified <see cref="IDataWriter"/>.
		/// </summary>
		/// <param name="writer"></param>
		public abstract void WriteData(IDataWriter writer);
		
		/// <summary>
		/// Constructs an object in deserialization based on the constructor data that has been written in
		/// serialization using <see cref="WriteConstructorData"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to deserialize constructor data from.</param>
		/// <param name="objType">The <see cref="System.Type"/> of the object to create.</param>
		/// <returns>An instance of the specified <see cref="System.Type"/> that has been constructed using the provided data.</returns>
		public virtual object ConstructObject(IDataReader reader, Type objType)
		{
			return objType.CreateInstanceOf() ?? objType.CreateInstanceOf(true);
		}
		/// <summary>
		/// Reads and applies the object data to the specified <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader"></param>
		public abstract void ReadData(IDataReader reader);
	}
}
