using System;
using System.Collections.Generic;
using System.Text;
using Duality.Resources;
using Duality.Utility.Coroutines;

namespace Duality.Utility.Coroutines
{
	public static class CoroutineSceneExtensions
	{
		/// <summary>
		/// Starts a coroutine and registers it in the scene's CoroutineManager
		/// </summary>
		/// <param name="scene">The Scene</param>
		/// <param name="method">The coroutine implementation</param>
		/// <param name="name">The name of the coroutine, optional</param>
		/// <returns>The created corutine</returns>
		public static Coroutine StartCoroutine(this Scene scene, IEnumerable<WaitUntil> method)
		{
			return scene.CoroutineManager.StartNew(method);
		}
	}
}
