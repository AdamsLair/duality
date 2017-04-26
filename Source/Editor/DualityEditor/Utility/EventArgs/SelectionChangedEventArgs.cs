using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	public class SelectionChangedEventArgs : EventArgs
	{
		private ObjectSelection			 current;
		private ObjectSelection			 previous;
		private ObjectSelection.Category	diffCat;
		private ObjectSelection			 added;
		private ObjectSelection			 removed;
		private SelectionChangeReason	   reason;

		public ObjectSelection Current
		{
			get { return this.current; }
		}
		public ObjectSelection Previous
		{
			get { return this.previous; }
		}
		public ObjectSelection.Category AffectedCategories
		{
			get { return this.diffCat; }
		}
		public ObjectSelection Added
		{
			get { return this.added; }
		}
		public ObjectSelection Removed
		{
			get { return this.removed; }
		}
		public bool SameObjects
		{
			get { return this.added.Empty && this.removed.Empty; }
		}
		public SelectionChangeReason ChangeReason
		{
			get { return this.reason; }
		}

		public SelectionChangedEventArgs(ObjectSelection current, ObjectSelection previous, ObjectSelection.Category changedCategoryFallback, SelectionChangeReason reason)
		{
			this.current = current;
			this.previous = previous;
			this.reason = reason;

			this.diffCat = ObjectSelection.GetAffectedCategories(this.previous, this.current);
			if (this.diffCat == ObjectSelection.Category.None) this.diffCat = changedCategoryFallback;
			this.added = this.current - this.previous;
			this.removed = this.previous - this.current;
		}
	}
}
