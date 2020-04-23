using System;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Duality.Tests.Utility
{
	public class GarbageCollectionUtils
	{
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

	public class CleanupTestSet
	{
		public CleanupTestSet(WeakReference weakReference, object[] keepAliveInstances = null) : this(new[] { weakReference }, keepAliveInstances) { }

		public CleanupTestSet(WeakReference[] weakReferences, object[] keepAliveInstances = null)
		{
			this.WeakReferences = weakReferences;
			this.KeepAliveInstances = keepAliveInstances;
		}

		public WeakReference[] WeakReferences { get; }
		public object[] KeepAliveInstances { get; }
	}
}
