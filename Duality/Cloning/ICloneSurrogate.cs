using System;

namespace Duality.Cloning
{
	/// <summary>
	/// Clones an object instead of letting it clone itsself or using a Reflection-driven approach.
	/// </summary>
	/// <seealso cref="Duality.Cloning.CloneSurrogate{T}"/>
	public interface ICloneSurrogate
	{
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

		void SetupCloneTargets(object source, object target, out bool requireLateSetup, ICloneTargetSetup setup);
		void LateSetup(object source, out object target, ICloneOperation operation);
		void CopyDataTo(object source, object target, ICloneOperation operation);
	}
	/// <summary>
	/// Default base class for <see cref="ICloneSurrogate">Serialization Surrogates</see>. It implements both
	/// <see cref="ICloneSurrogate"/> and <see cref="ICloneExplicit"/>, thus being able to fully perform de/serialization
	/// of a designated object type.
	/// </summary>
	/// <typeparam name="T">
	/// The base <see cref="System.Type"/> of objects this surrogate can replace.
	/// </typeparam>
	public abstract class CloneSurrogate<T> : ICloneSurrogate
	{
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
		
		public virtual void CreateTargetObject(T source, out T target, ICloneTargetSetup setup)
		{
			Type objType = source.GetType();
			target = (T)objType.CreateInstanceOf();
		}
		public virtual void CreateTargetObjectLate(T source, out T target, ICloneOperation operation)
		{
			Type objType = source.GetType();
			target = (T)objType.CreateInstanceOf();
		}
		public abstract void SetupCloneTargets(T source, T target, ICloneTargetSetup setup);
		public abstract void CopyDataTo(T source, T target, ICloneOperation operation);
		
		void ICloneSurrogate.SetupCloneTargets(object source, object target, out bool requireLateSetup, ICloneTargetSetup setup)
		{
			requireLateSetup = false;

			T targetCast;
			if (object.ReferenceEquals(target, null))
			{
				this.CreateTargetObject((T)source, out targetCast, setup);
				if (!typeof(T).IsValueType && targetCast != null)
				{
					setup.AddTarget(source, (object)targetCast);
				}
				else
				{
					requireLateSetup = true;
				}
			}
			else
			{
				targetCast = (T)target;
			}
			this.SetupCloneTargets((T)source, targetCast, setup);
		}
		void ICloneSurrogate.LateSetup(object source, out object target, ICloneOperation operation)
		{
			T targetObj;
			this.CreateTargetObjectLate((T)source, out targetObj, operation);
			target = targetObj;
		}
		void ICloneSurrogate.CopyDataTo(object source, object target, ICloneOperation operation)
		{
			this.CopyDataTo((T)source, (T)target, operation);
		}
	}
}
