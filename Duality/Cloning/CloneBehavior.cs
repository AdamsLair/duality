using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Cloning
{
	public enum CloneBehavior
	{
		/// <summary>
		/// The object will be assigned by-reference, because external ownership is assumed.
		/// </summary>
		Reference,
		/// <summary>
		/// The object will be cloned deeply, because local ownership is assumed.
		/// </summary>
		ChildObject,
		/// <summary>
		/// If the referenced object is part of the cloned object graph, it will be assigned by-reference
		/// similar to the <see cref="Reference"/> setting. Otherwise, it will be skipped without assigning
		/// any value.
		/// </summary>
		WeakReference
	}
}
