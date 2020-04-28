using System;
using System.Collections.Generic;

namespace Duality
{
	/// <summary>
	/// Provides event arguments events related to groups of <see cref="Duality.GameObject"/> instances.
	/// </summary>
	public class GameObjectGroupEventArgs : EventArgs
	{
		private List<GameObject> objects;

		/// <summary>
		/// [GET] Enumerates all objects in this event.
		/// </summary>
		public IEnumerable<GameObject> Objects
		{
			get { return this.objects; }
		}

		/// <summary>
		/// Creates a new <see cref="GameObjectGroupEventArgs"/> instance with the given list of objects.
		/// Note that the list is not copied for performance reasons. However, the event also does not
		/// provide any write access to that internal list either.
		/// </summary>
		public GameObjectGroupEventArgs(List<GameObject> objects)
		{
			this.objects = objects;
		}
	}
}
