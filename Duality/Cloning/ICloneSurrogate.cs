using System;

namespace Duality.Cloning
{
	/// <summary>
	/// Clones an object instead of letting it clone itsself or using a Reflection-driven approach.
	/// </summary>
	/// <seealso cref="Duality.Cloning.Surrogate{T}"/>
	public interface ICloneSurrogate
	{
		/// <summary>
		/// [GET / SET] The object that is cloned
		/// </summary>
		object RealObject { get; set; }
		/// <summary>
		/// [GET] If more than one registered ISurrogate is capable of cloning a given object type, the one
		/// with the highest priority is picked.
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// Checks whether this surrogate is able to clone the specified object type.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> of the object in question.</param>
		/// <returns>True, if this surrogate is able to clone such object, false if not.</returns>
		bool MatchesType(Type t);

		object CreateTargetObject(CloneProvider provider);
		void CopyDataTo(object targetObj, CloneProvider provider);
	}
	/// <summary>
	/// Default base class for <see cref="ICloneSurrogate">Serialization Surrogates</see>. It implements both
	/// <see cref="ICloneSurrogate"/> and <see cref="ICloneExplicit"/>, thus being able to fully perform de/serialization
	/// of a designated object type.
	/// </summary>
	/// <typeparam name="T">
	/// The base <see cref="System.Type"/> of objects this surrogate can replace.
	/// </typeparam>
	public abstract class Surrogate<T> : ICloneSurrogate
	{
		private T realObj;
		
		object ICloneSurrogate.RealObject
		{
			get { return this.realObj; }
			set { this.realObj = (T)value; }
		}
		
		/// <summary>
		/// [GET / SET] The object that is cloned
		/// </summary>
		protected T RealObject
		{
			get { return this.realObj; }
		}
		/// <summary>
		/// [GET] If more than one registered ISurrogate is capable of cloning a given object type, the one
		/// with the highest priority is picked.
		/// </summary>
		public virtual int Priority
		{
			get { return 0; }
		}

		
		/// <summary>
		/// Checks whether this surrogate is able to clone the specified object type.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> of the object in question.</param>
		/// <returns>True, if this surrogate is able to clone such object, false if not.</returns>
		public virtual bool MatchesType(Type t)
		{
			return typeof(T) == t;
		}
		
		public virtual T CreateTargetObject(CloneProvider provider)
		{
			Type objType = this.RealObject.GetType();
			return (T)(objType.CreateInstanceOf() ?? objType.CreateInstanceOf(true));
		}
		public abstract void CopyDataTo(T targetObj, CloneProvider provider);

		object ICloneSurrogate.CreateTargetObject(CloneProvider provider)
		{
			return this.CreateTargetObject(provider);
		}
		void ICloneSurrogate.CopyDataTo(object targetObj, CloneProvider provider)
		{
			this.CopyDataTo((T)targetObj, provider);
		}
	}
}
