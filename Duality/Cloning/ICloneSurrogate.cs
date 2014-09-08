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
		/// [GET] Specifies whether the surrogates client object requires a manual merge between source and target
		/// objects, e.g. whether its manual object handling methods will be called even when the source object is null.
		/// </summary>
		bool RequireMerge { get; }

		/// <summary>
		/// Checks whether this surrogate is able to clone the specified object type.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> of the object in question.</param>
		/// <returns>True, if this surrogate is able to clone such object, false if not.</returns>
		bool MatchesType(Type t);

		void SetupCloneTargets(object source, object target, out bool requireLateSetup, ICloneTargetSetup setup);
		void LateSetup(object source, ref object target, ICloneOperation operation);
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
	public abstract class CloneSurrogate<T> : ICloneSurrogate where T : class
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
		/// [GET] Returns whether the surrogates client object is considered to be immutable, e.g. whether
		/// it will always be required to create a target object, even if an existing one is provided by the
		/// target object graph.
		/// </summary>
		protected virtual bool IsImmutableTarget
		{
			get { return false; }
		}
		/// <summary>
		/// [GET] Specifies whether the surrogates client object requires a manual merge between source and target
		/// objects, e.g. whether its manual object handling methods will be called even when the source object is null.
		/// </summary>
		public virtual bool RequireMerge
		{
			get { return false; }
		}
		
		/// <summary>
		/// Checks whether this surrogate is able to clone the specified object type.
		/// </summary>
		/// <param name="t">The <see cref="System.Type"/> of the object in question.</param>
		/// <returns>True, if this surrogate is able to clone such object, false if not.</returns>
		public virtual bool MatchesType(Type t)
		{
			return typeof(T).IsAssignableFrom(t);
		}
		
		public virtual void CreateTargetObject(T source, ref T target, ICloneTargetSetup setup)
		{
			Type objType = source.GetType();
			target = objType.CreateInstanceOf() as T;
		}
		public virtual void CreateTargetObjectLate(T source, ref T target, ICloneOperation operation)
		{
			Type objType = source.GetType();
			target = objType.CreateInstanceOf() as T;
		}
		public abstract void SetupCloneTargets(T source, T target, ICloneTargetSetup setup);
		public abstract void CopyDataTo(T source, T target, ICloneOperation operation);
		
		void ICloneSurrogate.SetupCloneTargets(object source, object target, out bool requireLateSetup, ICloneTargetSetup setup)
		{
			requireLateSetup = false;

			T targetCast = target as T;
			if (object.ReferenceEquals(targetCast, null) || this.IsImmutableTarget)
			{
				this.CreateTargetObject(source as T, ref targetCast, setup);
				if (!object.ReferenceEquals(targetCast, null))
				{
					// If the source is null, map the old target to the new
					setup.AddTarget(source ?? target, targetCast);
				}
				else
				{
					requireLateSetup = true;
				}
			}
			else if (!object.ReferenceEquals(targetCast, null))
			{
				// If the source is null, map the old target to the new
				setup.AddTarget(source ?? target, target);
			}
			this.SetupCloneTargets(source as T, targetCast, setup);
		}
		void ICloneSurrogate.LateSetup(object source, ref object target, ICloneOperation operation)
		{
			T targetObj = target as T;
			this.CreateTargetObjectLate(source as T, ref targetObj, operation);
			target = targetObj;
		}
		void ICloneSurrogate.CopyDataTo(object source, object target, ICloneOperation operation)
		{
			this.CopyDataTo(source as T, target as T, operation);
		}
	}
}
