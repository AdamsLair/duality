using System;

namespace Duality
{
	public class CollisionEventArgs : EventArgs
	{
		private	GameObject		colWith;
		private	CollisionData	colData;

		public GameObject CollideWith
		{
			get { return this.colWith; }
		}
		public CollisionData CollisionData
		{
			get { return this.colData; }
		}

		public CollisionEventArgs(GameObject obj, CollisionData data)
		{
			this.colWith = obj;
			this.colData = data;
		}
	}
}
