using System;
using System.Reflection;

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
		/// <param name="t">The <see cref="System.Reflection.TypeInfo"/> of the object in question.</param>
		/// <returns>True, if this surrogate is able to clone such object, false if not.</returns>
		bool MatchesType(TypeInfo t);
		
		/// <summary>
		/// Performs the cloning setup step, in which all reference-type instances from the target object
		/// graph are generated. 
		/// 
		/// The purpose of this method is to help the cloning system walk the entire (relevant) object graph
		/// in order to determine which objects are referenced and which are owned / deep-cloned, as well as
		/// creating instances or re-using existing instances from the target graph.
		/// 
		/// Walking the specified object's part of the source object graph and mapping instances to their target object
		/// graph correspondents is done by using the <see cref="ICloneTargetSetup"/> interface methods for 
		/// handling object instances and struct values.
		/// </summary>
		/// <param name="source">
		/// The object instance from the source graph that is being investigated right now.
		/// </param>
		/// <param name="target">
		/// The object instance from the target graph that corresponds to the object's instance in the source graph.
		/// When invoking this method, the target object will either have existed already, or been created by the
		/// cloning system.
		/// </param>
		/// <param name="requireLateSetup">
		/// Specified whether this kind of object requires a late setup step, allowing to create its instance only
		/// the entire other target object graph has already been created.
		/// </param>
		/// <param name="setup">The setup environment for the cloning operation.</param>
		void SetupCloneTargets(object source, object target, out bool requireLateSetup, ICloneTargetSetup setup);
		/// <summary>
		/// Performs a late setup for the source object. This is similar to the <see cref="SetupCloneTargets"/> step,
		/// except that all other objects from the target graph have already been created and a full source-target
		/// mapping is available.
		/// 
		/// A late-setup step is usually not required and should be avoided as long as it's not a necessity.
		/// </summary>
		/// <param name="source">
		/// The object instance from the source graph that is being set up right now.
		/// </param>
		/// <param name="target">
		/// The object instance from the target graph that corresponds to the object's instance in the source graph.
		/// In the late-setup step, there already is a preliminary target object instance, but it can be replaced
		/// by re-assigning the target reference.
		/// </param>
		/// <param name="operation"></param>
		void LateSetup(object source, ref object target, ICloneOperation operation);
		/// <summary>
		/// Performs the cloning copy step, in which all data is copied from source instances to
		/// target instances. No new object instances should be created in this step, as object creation
		/// should be part of the setup step instead.
		/// </summary>
		/// <param name="source">
		/// The object instance from the source graph that is being copied.
		/// </param>
		/// <param name="target">
		/// The object instance from the target graph that corresponds to this object's instance in the source graph.
		/// When invoking this method, the target object will either have existed already, or been created by the
		/// cloning system.
		/// </param>
		/// <param name="operation"></param>
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
		/// <param name="t">The <see cref="System.Reflection.TypeInfo"/> of the object in question.</param>
		/// <returns>True, if this surrogate is able to clone such object, false if not.</returns>
		public virtual bool MatchesType(TypeInfo t)
		{
			return typeof(T).GetTypeInfo().IsAssignableFrom(t);
		}
		
		/// <summary>
		/// In case there is no pre-existing correspondent object in the target graph, this method
		/// is invoked in order to create one based on the source object. The default implementation
		/// creates a target object of matching type using <see cref="ObjectCreator.CreateInstanceOf"/>.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="setup"></param>
		public virtual void CreateTargetObject(T source, ref T target, ICloneTargetSetup setup)
		{
			Type objType = source.GetType();
			TypeInfo objTypeInfo = objType.GetTypeInfo();
			target = objTypeInfo.CreateInstanceOf() as T;
		}
		/// <summary>
		/// If no object was created in <see cref="CreateTargetObject"/>, the surrogate implementation
		/// will assume a late-setup step for this object and invoke this method as soon as the entire
		/// target graph has been created. Otherwise, this method matches <see cref="CreateTargetObject"/>-
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="operation"></param>
		public virtual void CreateTargetObjectLate(T source, ref T target, ICloneOperation operation)
		{
			Type objType = source.GetType();
			TypeInfo objTypeInfo = objType.GetTypeInfo();
			target = objTypeInfo.CreateInstanceOf() as T;
		}
		/// <inheritdoc cref="ICloneSurrogate.SetupCloneTargets"/>
		public abstract void SetupCloneTargets(T source, T target, ICloneTargetSetup setup);
		/// <inheritdoc cref="ICloneSurrogate.CopyDataTo"/>
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
