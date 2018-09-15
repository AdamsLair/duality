using System;
using System.Linq;
using System.Collections.Generic;

namespace Duality.IO
{
	/// <summary>
	/// A specialized data structure for queueing <see cref="FileEvent"/> data and
	/// normalizing it by aggregating event groups and duplicate events.
	/// </summary>
	public class FileEventQueue
	{
		private List<FileEvent> items = new List<FileEvent>();

		/// <summary>
		/// Whether there are any events in the queue.
		/// </summary>
		public bool IsEmpty
		{
			get { return this.items.Count == 0; }
		}
		/// <summary>
		/// A list of normalized events in this queue.
		/// </summary>
		public IReadOnlyList<FileEvent> Items
		{
			get { return this.items; }
		}


		/// <summary>
		/// Adds a new <see cref="FileEvent"/> at the end of the queue.
		/// </summary>
		/// <param name="fileEvent"></param>
		public void Add(FileEvent fileEvent)
		{
			this.items.Add(fileEvent);
			this.AggregateNewlyAdded(this.items.Count - 1);
		}
		/// <summary>
		/// Clears the queue of all events.
		/// </summary>
		public void Clear()
		{
			this.items.Clear();
		}
		/// <summary>
		/// Removes all events from the queue that match the specified predicate.
		/// </summary>
		/// <param name="predicate"></param>
		public void ApplyFilter(Predicate<FileEvent> predicate)
		{
			this.items.RemoveAll(predicate);
		}

		/// <summary>
		/// Performs an aggregate operation on all new events, starting at the specified index.
		/// Each event will include all previous events in its aggregate check.
		/// </summary>
		/// <param name="addedStartingIndex"></param>
		private void AggregateNewlyAdded(int addedStartingIndex)
		{
			// Iterate backwards over newly added events, and run an aggregation step
			// with all previous events for each of them.
			for (int currentIndex = this.items.Count - 1; currentIndex >= addedStartingIndex; currentIndex--)
			{
				FileEvent current = this.items[currentIndex];
				string currentOldFileName = PathOp.GetFileName(current.OldPath);
				string currentFileName = PathOp.GetFileName(current.Path);
				bool discardedCurrent = false;

				// Aggregate with previous events, so the latest event
				// in an aggregate chain is the one that defines event order.
				for (int prevIndex = currentIndex - 1; prevIndex >= 0; prevIndex--)
				{
					FileEvent prev = this.items[prevIndex];
					string prevFileName = PathOp.GetFileName(prev.Path);
					bool consumedPrev = false;

					// Aggregate identical events
					if (current.Equals(prev))
					{
						consumedPrev = true;
					}
					// Aggregate "delete Foo/A, create Bar/A" to "rename Foo/A to Bar/A" events.
					// Aggregate "delete Foo/A, create Foo/A" to "change Foo/A" events.
					else if (
						current.Type == FileEventType.Created &&
						prev.Type == FileEventType.Deleted &&
						currentFileName == prevFileName)
					{
						current.Type = (current.Path == prev.Path) ? 
							FileEventType.Changed : 
							FileEventType.Renamed;
						current.OldPath = prev.Path;
						consumedPrev = true;
					}
					// Aggregate sequential renames / moves of the same file
					else if (
						current.Type == FileEventType.Renamed &&
						prev.Type == FileEventType.Renamed &&
						currentOldFileName == prevFileName)
					{
						current.OldPath = prev.OldPath;
						consumedPrev = true;
					}
					// Aggregate "delete A, then rename B to A" into "delete B, changed A" events.
					// Some applications (like Photoshop) do stuff like that when saving files.
					else if (
						current.Type == FileEventType.Renamed &&
						prev.Type == FileEventType.Deleted &&
						current.Path == prev.Path)
					{
						FileEvent deleted = current;
						deleted.Type = FileEventType.Deleted;
						deleted.Path = current.OldPath;
						deleted.OldPath = current.OldPath;
						this.items.Insert(currentIndex, deleted);
						currentIndex++;

						current.Type = FileEventType.Changed;
						current.OldPath = current.Path;
						consumedPrev = true;
					}
					// Aggregate anything before a delete into just the delete
					else if (
						current.Type == FileEventType.Deleted &&
						prev.Path == current.Path)
					{
						// Special case for previous renames: Translate the delete back to the old path
						if (prev.Type == FileEventType.Renamed)
						{
							current.Path = prev.OldPath;
							current.OldPath = prev.OldPath;
						}
						// Special case for previous creates: Discard both the delete and the create.
						else if (prev.Type == FileEventType.Created)
						{
							discardedCurrent = true;
						}
						consumedPrev = true;
					}
					// Aggregate anything after a create into just the create
					else if (
						prev.Type == FileEventType.Created &&
						(prev.Path == current.Path || prev.Path == current.OldPath))
					{
						current.Type = FileEventType.Created;
						current.OldPath = current.Path;
						consumedPrev = true;
					}

					// Remove the previous item if it was aggregated with the current
					if (consumedPrev)
					{
						this.items.RemoveAt(prevIndex);

						// Update other indices to account for removed item
						currentIndex--;
						if (addedStartingIndex > prevIndex)
							addedStartingIndex--;
					}
				}

				// Filter out no-op events
				if (current.Type == FileEventType.Renamed &&
					current.OldPath == current.Path)
				{
					discardedCurrent = true;
				}

				// Discard or update the current event
				if (discardedCurrent)
					this.items.RemoveAt(currentIndex);
				else
					this.items[currentIndex] = current;
			}
		}
	}
}
