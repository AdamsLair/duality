using System;

namespace Duality.Tests.Utility
{
	/// <summary>
	/// Represents the data used to test if instances are properly collected afer a garbage collection.
	/// </summary>
	public class GarbageCollectionTestSet
	{
		/// <summary>
		/// Creates a new <see cref="GarbageCollectionTestSet"/> without instances to keep alive.
		/// </summary>
		/// <param name="weakReference"></param>
		/// <param name="keepAliveInstances"></param>
		public GarbageCollectionTestSet(WeakReference weakReference, object[] keepAliveInstances = null) : this(new[] { weakReference }, keepAliveInstances) { }

		/// <summary>
		/// Creates a new <see cref="GarbageCollectionTestSet"/> with instances to keep alive.
		/// </summary>
		/// <param name="weakReference"></param>
		/// <param name="keepAliveInstances"></param>
		public GarbageCollectionTestSet(WeakReference[] weakReferences, object[] keepAliveInstances = null)
		{
			this.WeakReferences = weakReferences;
			this.KeepAliveInstances = keepAliveInstances;
		}

		/// <summary>
		/// These references will be checked after a collection to verify they are properly collected
		/// </summary>
		public WeakReference[] WeakReferences { get; }

		/// <summary>
		/// Instances that will be kept alive during the test.
		/// </summary>
		public object[] KeepAliveInstances { get; }
	}
}
