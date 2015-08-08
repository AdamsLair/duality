using System;

namespace Duality
{
	/// <summary>
	/// Provides event arguments for a <see cref="GameObject">GameObjects</see> "<see cref="GameObject.Parent"/> changed" events.
	/// </summary>
	public class GameObjectParentChangedEventArgs : GameObjectEventArgs
	{
		private	GameObject	oldParent;
		private	GameObject	newParent;

		/// <summary>
		/// [GET] The GameObjects old parent.
		/// </summary>
		public GameObject OldParent
		{
			get { return this.oldParent; }
		}
		/// <summary>
		/// [GET] The GameObjects new parent.
		/// </summary>
		public GameObject NewParent
		{
			get { return this.newParent; }
		}

		public GameObjectParentChangedEventArgs(GameObject obj, GameObject oldParent, GameObject newParent) : base(obj)
		{
			this.oldParent = oldParent;
			this.newParent = newParent;
		}
	}
}
