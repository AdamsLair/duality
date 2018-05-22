using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Duality
{
	public static class CmpInitializableExtensions
	{
		/// <summary>
		/// Calls <see cref="ICmpInitializable.OnShutdown"/> on all instances in <paramref name="initializables"/>.
		/// Guarantees that all shutdowns are executed even if there are exceptions.
		/// </summary>
		/// <param name="initializables"></param>
		/// <param name="context"></param>
		/// <exception cref="AggregateException">If any exceptions occurred while executing the onshutdowns</exception>
		[DebuggerStepThrough]
		public static void ShutdownAllReversed(this IList<ICmpInitializable> initializables, Component.ShutdownContext context)
		{
			List<Exception> exceptions = null; //In the happy path we wont be needing the list so delay creating the list instance until its actually needed.
			for (int i = initializables.Count - 1; i >= 0; i--)
			{
				try
				{
					initializables[i].OnShutdown(context);
				}
				catch (Exception e)
				{
					if (exceptions == null) exceptions = new List<Exception>();
					exceptions.Add(e);
				}
			}

			if (exceptions != null)
			{
				throw new AggregateException(string.Format("The following exceptions occurred during ShutdownContext: {0}", context), exceptions);
			}
		}

		/// <summary>
		/// Calls <see cref="ICmpInitializable.OnShutdown"/> on all instances in <paramref name="initializables"/>.
		/// Guarantees that all shutdowns are executed even if there are exceptions.
		/// </summary>
		/// <param name="initializables"></param>
		/// <param name="context"></param>
		/// <exception cref="AggregateException">If any exceptions occurred while executing the onshutdowns</exception>
		[DebuggerStepThrough]
		public static void ShutdownAll(this IList<ICmpInitializable> initializables, Component.ShutdownContext context)
		{
			List<Exception> exceptions = null; //In the happy path we wont be needing the list so delay creating the list instance until its actually needed.
			for (int i = 0; i < initializables.Count; i++)
			{
				try
				{
					initializables[i].OnShutdown(context);
				}
				catch (Exception e)
				{
					if (exceptions == null) exceptions = new List<Exception>();
					exceptions.Add(e);
				}
			}

			if (exceptions != null)
			{
				throw new AggregateException(string.Format("The following exceptions occurred during ShutdownContext: {0}", context), exceptions);
			}
		}
	}
}
