using System;

using Duality.Components;

namespace Duality
{
	public class TransformChangedEventArgs : ComponentEventArgs
	{
		private	Transform.ChangeFlags changes;
		/// <summary>
		/// [GET] The changes that have been made since the last update.
		/// </summary>
		public Transform.ChangeFlags Changes
		{
			get { return this.changes; }
		}
		public TransformChangedEventArgs(Component transform, Transform.ChangeFlags dirtyFlags) : base(transform)
		{
			this.changes = dirtyFlags;
		}
	}
}
