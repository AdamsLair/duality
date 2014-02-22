using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Represents an object that can be de/activated and explicitly released / disposed
	/// </summary>
	public interface IManageableObject
	{
		/// <summary>
		/// [GET] Returns whether the object is considered disposed.
		/// </summary>
		bool Disposed { get; }
		/// <summary>
		/// [GET] Returns whether the object is currently active.
		/// </summary>
		bool Active { get; }

		/// <summary>
		/// Disposes the object.
		/// </summary>
		void Dispose();
	}

	public static class ExtMethodsIManageableObject
	{
		/// <summary>
		/// Schedules this object for disposal at the end of the current update cycle.
		/// </summary>
		/// <param name="obj"></param>
		public static void DisposeLater(this IManageableObject obj)
		{
			DualityApp.DisposeLater(obj);
		}
	}
}
