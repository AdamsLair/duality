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
		/// Asserts if the <see cref="WeakReference"/> in <see cref="GarbageCollectionTestSet"/> are collected after a garbage collection.
		/// </summary>
		/// <param name="func"></param>
		// No inlining is important here to defeat any optimizations the compiler can make that could cause instances to live longer.
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void CheckIfCleanedUp(Func<GarbageCollectionTestSet> func)
		{
			GarbageCollectionTestSet cleanupTestSet = func();

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
}
