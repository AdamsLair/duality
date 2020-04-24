using System;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Duality.Tests.Utility
{
	/// <summary>
	/// Contains utilities to help test garbage collection related code.
	/// </summary>
	public class GarbageCollectionUtils
	{
		/// <summary>
		/// Asserts if the <see cref="WeakReference"/> in <see cref="CleanupTestSet"/> are collected after a garbage collection.
		/// </summary>
		/// <param name="func"></param>
		// No inlining is important here to defeat any optimizations the compiler can make that could cause instances to live longer.
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void CheckIfCleanedUp(Func<CleanupTestSet> func)
		{
			CleanupTestSet cleanupTestSet = func();

			// In some cases unloading something happens async and can take some time such as with AssemblyLoadContext. Workaround this by retrying a couple of times..
			for (int i = 0; cleanupTestSet.WeakReferences.Any(x => x.IsAlive) && i < 10; i++)
			{
				GC.Collect(2, GCCollectionMode.Forced, true);
				GC.WaitForPendingFinalizers();
			}
			foreach (var item in cleanupTestSet.WeakReferences)
			{
				Assert.False(item.IsAlive);
			}
		}
	}

	/// <summary>
	/// Represents the data used to test if instances are properly collected afer a garbage collection.
	/// </summary>
	public class CleanupTestSet
	{
		/// <summary>
		/// Creates a new <see cref="CleanupTestSet"/> without instances to keep alive.
		/// </summary>
		/// <param name="weakReference"></param>
		/// <param name="keepAliveInstances"></param>
		public CleanupTestSet(WeakReference weakReference, object[] keepAliveInstances = null) : this(new[] { weakReference }, keepAliveInstances) { }

		/// <summary>
		/// Creates a new <see cref="CleanupTestSet"/> with instances to keep alive.
		/// </summary>
		/// <param name="weakReference"></param>
		/// <param name="keepAliveInstances"></param>
		public CleanupTestSet(WeakReference[] weakReferences, object[] keepAliveInstances = null)
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
