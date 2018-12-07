using System;
using System.Diagnostics;
using System.Collections.Generic;

using NUnit.Framework;

using Duality.IO;

namespace Duality.Tests.IO
{
	[TestFixture]
	public class FileEventQueueTest
	{
		[Test] public void Basics()
		{
			List<FileEvent> eventList = new List<FileEvent>();
			eventList.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			eventList.Add(new FileEvent(FileEventType.Created, "Foo\\B", false));
			eventList.Add(new FileEvent(FileEventType.Deleted, "Foo\\C", false));
			eventList.Add(new FileEvent(FileEventType.Renamed, "Foo\\D", "Foo\\E", false));

			FileEventQueue queue = new FileEventQueue();
			Assert.IsTrue(queue.IsEmpty);
			Assert.AreEqual(0, queue.Items.Count);

			foreach (FileEvent item in eventList)
			{
				queue.Add(item);
			}

			Assert.IsFalse(queue.IsEmpty);
			CollectionAssert.AreEqual(eventList, queue.Items);

			queue.ApplyFilter(fileEvent => fileEvent.Type == FileEventType.Created);
			eventList.RemoveAt(1);

			Assert.IsFalse(queue.IsEmpty);
			CollectionAssert.AreEqual(eventList, queue.Items);

			queue.Clear();

			Assert.IsTrue(queue.IsEmpty);
			Assert.AreEqual(0, queue.Items.Count);
		}

		[Test] public void AggregateEqualEvents()
		{
			List<FileEvent> eventList = new List<FileEvent>();
			eventList.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			eventList.Add(new FileEvent(FileEventType.Created, "Foo\\B", false));
			eventList.Add(new FileEvent(FileEventType.Deleted, "Foo\\C", false));
			eventList.Add(new FileEvent(FileEventType.Renamed, "Foo\\D", "Foo\\E", false));

			// Create a queue and add each test event multiple times
			FileEventQueue queue = new FileEventQueue();
			for (int sequenceIndex = 0; sequenceIndex < 3; sequenceIndex++)
			{
				foreach (FileEvent item in eventList)
				{
					for (int groupIndex = 0; groupIndex < 3; groupIndex++)
					{
						queue.Add(item);
					}
				}
			}

			// Assert that the queue only contains what's in the event list, in 
			// the order of latest submission
			CollectionAssert.AreEqual(eventList, queue.Items);
		}
		[Test] public void AggregateMultiRename()
		{
			List<FileEvent> inputEvents = new List<FileEvent>();
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\A", "Foo\\B", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\B", "Foo\\C", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\C", "Foo\\D", false));

			// Create a queue and add events in order
			FileEventQueue queue = new FileEventQueue();
			foreach (FileEvent item in inputEvents)
			{
				queue.Add(item);
			}

			// Assert that all rename events were aggregated into a single one
			List<FileEvent> outputEvents = new List<FileEvent>();
			outputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\A", "Foo\\D", false));
			CollectionAssert.AreEqual(outputEvents, queue.Items);
		}
		[Test] public void AggregateDeleteCreate()
		{
			List<FileEvent> inputEvents = new List<FileEvent>();
			inputEvents.Add(new FileEvent(FileEventType.Deleted, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Created, "Bar\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Deleted, "Foo\\B", false));
			inputEvents.Add(new FileEvent(FileEventType.Created, "Foo\\B", false));

			// Create a queue and add events in order
			FileEventQueue queue = new FileEventQueue();
			foreach (FileEvent item in inputEvents)
			{
				queue.Add(item);
			}

			// Assert that the queue merges a delete-create using the same file name into a rename,
			// and using the same path into a change.
			List<FileEvent> outputEvents = new List<FileEvent>();
			outputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\A", "Bar\\A", false));
			outputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\B", false));
			CollectionAssert.AreEqual(outputEvents, queue.Items);
		}
		[Test] public void AggregateDeleteRenameInto()
		{
			List<FileEvent> inputEvents = new List<FileEvent>();
			inputEvents.Add(new FileEvent(FileEventType.Deleted, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\B", "Foo\\A", false));

			// Create a queue and add events in order
			FileEventQueue queue = new FileEventQueue();
			foreach (FileEvent item in inputEvents)
			{
				queue.Add(item);
			}

			// Assert that the delete was discarded, but a change was queued after rename
			// because we essentially still have that file, just with different contents.
			List<FileEvent> outputEvents = new List<FileEvent>();
			outputEvents.Add(new FileEvent(FileEventType.Deleted, "Foo\\B", false));
			outputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			CollectionAssert.AreEqual(outputEvents, queue.Items);
		}

		[Test] public void DiscardNopRenameIntoSelf()
		{
			List<FileEvent> inputEvents = new List<FileEvent>();
			inputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\A", "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\B", "Foo\\B", false));

			// Create a queue and add events in order
			FileEventQueue queue = new FileEventQueue();
			foreach (FileEvent item in inputEvents)
			{
				queue.Add(item);
			}

			// Assert that we discard all rename events that have the same old and new path.
			List<FileEvent> outputEvents = new List<FileEvent>();
			outputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			CollectionAssert.AreEqual(outputEvents, queue.Items);
		}
		[Test] public void DiscardAnythingBeforeDelete()
		{
			List<FileEvent> inputEvents = new List<FileEvent>();
			inputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\A", "Foo\\B", false));
			inputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\B", false));
			inputEvents.Add(new FileEvent(FileEventType.Deleted, "Foo\\B", false));

			// Create a queue and add events in order
			FileEventQueue queue = new FileEventQueue();
			foreach (FileEvent item in inputEvents)
			{
				queue.Add(item);
			}

			// Assert that we discard all now-irrelevant events of a deleted file.
			// Note that we require the queue to pick up on the rename mid-sequence.
			List<FileEvent> outputEvents = new List<FileEvent>();
			outputEvents.Add(new FileEvent(FileEventType.Deleted, "Foo\\A", false));
			CollectionAssert.AreEqual(outputEvents, queue.Items);
		}
		[Test] public void DiscardAnythingAfterCreate()
		{
			List<FileEvent> inputEvents = new List<FileEvent>();
			inputEvents.Add(new FileEvent(FileEventType.Created, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\A", "Foo\\B", false));
			inputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\B", false));

			// Create a queue and add events in order
			FileEventQueue queue = new FileEventQueue();
			foreach (FileEvent item in inputEvents)
			{
				queue.Add(item);
			}

			// Assert that we discard all folluwup events of a newly created file.
			// Since we didn't know the file existed before this queue, it's pointless
			// to know what it did until now.
			// Note that we require the queue to pick up on the rename mid-sequence.
			List<FileEvent> outputEvents = new List<FileEvent>();
			outputEvents.Add(new FileEvent(FileEventType.Created, "Foo\\B", false));
			CollectionAssert.AreEqual(outputEvents, queue.Items);
		}
		[Test] public void DiscardCreateAnythingDelete()
		{
			List<FileEvent> inputEvents = new List<FileEvent>();
			inputEvents.Add(new FileEvent(FileEventType.Created, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\A", "Foo\\B", false));
			inputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\B", false));
			inputEvents.Add(new FileEvent(FileEventType.Deleted, "Foo\\B", false));

			// Create a queue and add events in order
			FileEventQueue queue = new FileEventQueue();
			foreach (FileEvent item in inputEvents)
			{
				queue.Add(item);
			}

			// Assert that the queue discards event sequences starting with file creation
			// and ending with file deletion, as they might as well have never existed.
			// Note that we require the queue to pick up on the rename mid-sequence.
			List<FileEvent> outputEvents = new List<FileEvent>();
			CollectionAssert.AreEqual(outputEvents, queue.Items);
		}

		[Test] public void SimplifyPhotoshopRename()
		{
			// Photoshop, and maybe other applications, have this peculiar trick when
			// saving output files that overwrite an existing file, in which they first
			// write to a temp file, then delete the target file, then rename the temp
			// file into the target file.
			List<FileEvent> inputEvents = new List<FileEvent>();
			inputEvents.Add(new FileEvent(FileEventType.Created, "Foo\\Temp", false));
			inputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\Temp", false));
			inputEvents.Add(new FileEvent(FileEventType.Deleted, "Foo\\A", false));
			inputEvents.Add(new FileEvent(FileEventType.Renamed, "Foo\\Temp", "Foo\\A", false));

			// Create a queue and add events in order
			FileEventQueue queue = new FileEventQueue();
			foreach (FileEvent item in inputEvents)
			{
				queue.Add(item);
			}

			// Assert that the way we see these events has been normalized so we can easily
			// see that the target file was changed.
			List<FileEvent> outputEvents = new List<FileEvent>();
			outputEvents.Add(new FileEvent(FileEventType.Changed, "Foo\\A", false));
			CollectionAssert.AreEqual(outputEvents, queue.Items);
		}
	}
}
