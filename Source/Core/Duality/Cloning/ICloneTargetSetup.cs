using System;
using System.Reflection;

namespace Duality.Cloning
{
	/// <summary>
	/// Cloning system interface that allows an <see cref="ICloneExplicit"/> or <see cref="ICloneSurrogate"/>
	/// to take part in the setup step of a cloning operation. The purpose of the setup step is to walk the
	/// source object graph and create the required instances of the target graph where they do not exist yet.
	/// </summary>
	public interface ICloneTargetSetup
	{
		/// <summary>
		/// [GET] The context of this cloning operation, which can provide additional settings.
		/// </summary>
		CloneProviderContext Context { get; }

		/// <summary>
		/// Specifies an existing mapping between source and target object graph, in which references to a source
		/// object are re-mapped to the specified target object. Does not investigate the specified source object
		/// for further references or walks its object graph.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">A reference from the source graph that will be re-mapped in the target graph.</param>
		/// <param name="target">The target graph object that this reference will be re-mapped to.</param>
		void AddTarget<T>(T source, T target) where T : class;
		/// <summary>
		/// Walks the object graph of the specified instance from the source graph, while mapping it to the graph that
		/// is spanned by the specified target object. When specified, the default <see cref="CloneBehavior"/> of the 
		/// source object or a certain type of its child objects can be overridden locally.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">An object from the source graph that will be investigated by the cloning system.</param>
		/// <param name="target">The object's already existing equivalent from the target graph to which it will be mapped.</param>
		/// <param name="behavior">An optional override for the cloning behavior of this object.</param>
		/// <param name="behaviorTarget">
		/// When specified, the optional <see cref="CloneBehavior"/> override will only be active for the first level of 
		/// referenced objects of this type.
		/// </param>
		void HandleObject<T>(T source, T target, CloneBehavior behavior = CloneBehavior.Default, TypeInfo behaviorTarget = null) where T : class;
		/// <summary>
		/// Walks the object graph of the specified data struct from the source graph, while mapping it to the graph that
		/// is spanned by the specified target struct. When specified, the default <see cref="CloneBehavior"/> of the 
		/// source object or a certain type of its child objects can be overridden locally.
		/// 
		/// Note that this only makes sense if the struct contains object references and is not just plain-old data.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">A struct from the source graph that will be investigated by the cloning system.</param>
		/// <param name="target">The struct's already existing equivalent from the target graph to which it will be mapped.</param>
		/// <param name="behavior">An optional override for the cloning behavior of this struct.</param>
		/// <param name="behaviorTarget">
		/// When specified, the optional <see cref="CloneBehavior"/> override will only be active for the first level of 
		/// referenced objects of this type.
		/// </param>
		void HandleValue<T>(ref T source, ref T target, CloneBehavior behavior = CloneBehavior.Default, TypeInfo behaviorTarget = null) where T : struct;
	}
}
