using System;

using Duality.Components;

namespace Duality
{
	public class TransformChangedEventArgs : ComponentEventArgs
	{
		private	Transform.DirtyFlags dirtyFlags;
		/// <summary>
		/// [GET] The changes that have been made since the last update.
		/// </summary>
		public Transform.DirtyFlags Changes
		{
			get { return this.dirtyFlags; }
		}
		public TransformChangedEventArgs(Component transform, Transform.DirtyFlags dirtyFlags) : base(transform)
		{
			this.dirtyFlags = dirtyFlags;
		}
	}
}
