using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Duality.Editor;

namespace Duality.Resources
{
	public class RectAtlas : IList<Rect>, IList
	{
		private readonly List<Rect> rects;
		private Dictionary<string, List<int>> tags;

		/// <summary>
		/// [GET] The number of rects in the atlas
		/// </summary>
		public int Count
		{
			get { return this.rects.Count; }
		}

		/// <summary>
		/// [GET] Whether or not the atlas is read only. Always false.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// [GET / SET] Gets or sets the rect at the given index
		/// </summary>
		public Rect this[int index]
		{
			get { return this.rects[index]; }
			set { this.rects[index] = value; }
		}

		/// <summary>
		/// [GET] Gets the first rect with the given tag
		/// </summary>
		public Rect this[string tag]
		{
			get { return this.rects[this.tags[tag][0]]; }
		}

		public RectAtlas()
		{
			this.rects = new List<Rect>();
		}

		public RectAtlas(int count)
		{
			this.rects = new List<Rect>(count);
		}

		public RectAtlas(IEnumerable<Rect> rects)
		{
			this.rects = new List<Rect>(rects);
		}

		public RectAtlas(RectAtlas other)
		{
			this.rects = other.rects.ToList();
			if (other.tags != null)
			{
				this.tags = new Dictionary<string, List<int>>();
				foreach (var kvp in other.tags)
				{
					this.tags.Add(kvp.Key, kvp.Value.ToList());
				}
			}
		}

		/// <summary>
		/// Adds the given rect to the atlas.
		/// </summary>
		/// <param name="item">The rect to add</param>
		public void Add(Rect item)
		{
			this.rects.Add(item);
		}

		/// <summary>
		/// Clears the atlas of all rects and associated information
		/// </summary>
		public void Clear()
		{
			this.rects.Clear();
			this.tags = null;
		}

		/// <summary>
		/// Determines whether or not this atlas contains the given rect
		/// </summary>
		/// <param name="item">The atlas to look for</param>
		/// <returns>Whether or not the rect is contained in the atlas</returns>
		public bool Contains(Rect item)
		{
			return this.rects.Contains(item);
		}

		/// <summary>
		/// Copies the rects in the atlas to the given array starting at the given index.
		/// </summary>
		/// <param name="array">The array to copy the atlas rects to</param>
		/// <param name="arrayIndex">The index within the given array to place the atlas rects</param>
		public void CopyTo(Rect[] array, int arrayIndex)
		{
			this.rects.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes the given rect from the atlas and returns whether
		/// or not the rect was successfully removed.
		/// </summary>
		/// <param name="item">The rect to remove</param>
		/// <returns>Whether or not the given rect was removed from the atlas</returns>
		public bool Remove(Rect item)
		{
			int index = this.IndexOf(item);

			// Rects does not exist in atlas
			if (index == -1)
				return false;

			this.RemoveAt(index);
			return true;
		}

		/// <summary>
		/// Returns the index of the given rect within the atlas, or -1 if the rect is not found.
		/// </summary>
		/// <param name="item">The rect to find the index of</param>
		/// <returns>The index of the rect within the atlas, or -1 if the rect is not in the atlas.</returns>
		public int IndexOf(Rect item)
		{
			return this.rects.IndexOf(item);
		}

		/// <summary>
		/// Inserts the given rect into the atlas at the given index.
		/// </summary>
		/// <param name="index">The index at which to insert the rect</param>
		/// <param name="item">The rect to insert</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The given index was negative or greater than the number of rects in the atlas.
		/// </exception>
		public void Insert(int index, Rect item)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index", "The insertion index cannot be less than 0");
			if (index > this.rects.Count)
				throw new ArgumentOutOfRangeException("index", "The insertion index was greater than the size number of rects in the atlas");

			if (this.tags != null)
			{
				// Adjust tagged indices to account for the insertion of this item
				foreach (List<int> indexList in this.tags.Values)
				{
					for (int i = 0; i < indexList.Count; i++)
					{
						// Index after rect being added - update it to new index
						if (indexList[i] >= index)
						{
							indexList[i]++;
						}
					}
				}
			}

			this.rects.Insert(index, item);
		}

		/// <summary>
		/// Removes the rect from that atlas at the given index
		/// </summary>
		/// <param name="index">The index of the rect to remove</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The given index was negative or greater than the number of rects in the atlas.
		/// </exception>
		public void RemoveAt(int index)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index", "The insertion index cannot be less than 0");
			if (index > this.rects.Count)
				throw new ArgumentOutOfRangeException("index", "The insertion index was greater than the size number of rects in the atlas");

			if (this.tags != null)
			{
				// Adjust tagged indices to account for the removal of this item
				foreach (List<int> indexList in this.tags.Values)
				{
						for (int i = 0; i < indexList.Count; i++)
					{
						// Index of rect being removed - remove from tag list
						if (indexList[i] == index)
						{
							indexList.RemoveAt(i);
							i--;
						}
						// Index after rect being remove - update it to new index
						else if (indexList[i] > index)
						{
							indexList[i]--;
						}
					}
				}
			}

			this.rects.RemoveAt(index);
		}

		/// <summary>
		/// Tags the rects at the given indices with the given tag. If the rect at the given index
		/// already has the given tag, it will be ignored. Note that a rect can have multiple tags.
		/// </summary>
		/// <param name="tag">The tag to apply to the rects. Cannot be null or empty.</param>
		/// <param name="indices">The indices of the rects to tag</param>
		/// <exception cref="ArgumentException">
		/// The given tag was null or empty.
		/// </exception>
		public void TagIndices(string tag, int[] indices)
		{
			if (string.IsNullOrEmpty(tag))
				throw new ArgumentException("The tag cannot be null or empty", "tag");

			if (this.tags == null)
				this.tags = new Dictionary<string, List<int>>();
			if (!this.tags.ContainsKey(tag))
				this.tags.Add(tag, new List<int>());

			List<int> taggedIndices = this.tags[tag];
			foreach (int newTagIndex in indices)
			{
				int possibleIndex = taggedIndices.BinarySearch(newTagIndex);
				// If possibleIndex >= 0, the index is already tagged,
				// else BinarySearch returns the bitwise complement of
				// the index the item should be inserted into.
				if (possibleIndex < 0)
					taggedIndices.Insert(~possibleIndex, newTagIndex);
			}
		}

		/// <summary>
		/// Returns all rects with the given tag or an empty enumerable if no rects
		/// with the given tag exist. The returned rects will be in index order.
		/// </summary>
		/// <param name="tag">The tag of the rects to look for</param>
		/// <returns>
		/// The rects with the given tag or an empty enumerable if no rects with the given tag exist.
		/// </returns>
		public IEnumerable<Rect> GetTaggedRects(string tag)
		{
			return this.GetTaggedIndices(tag).Select(i => this[i]);
		}

		/// <summary>
		/// Returns all the indices of all rects with the given tag or an empty enumerable if no rects
		/// with the given tag exist.
		/// </summary>
		/// <param name="tag">The tag of the rects to look for</param>
		/// <returns>
		/// The indices of rects with the given tag or an empty enumerable if no rects with the given tag exist.
		/// </returns>
		public IEnumerable<int> GetTaggedIndices(string tag)
		{
			List<int> indices;
			if (this.tags == null || !this.tags.TryGetValue(tag, out indices))
				return Enumerable.Empty<int>();

			return indices;
		}

		/// <summary>
		/// Removes the given tag from all rects at the given indices.
		/// </summary>
		/// <param name="tag">The tag to remove from the rects</param>
		/// <param name="taggedIndices">The indices of rects to untag</param>
		public void UntagIndices(string tag, int[] taggedIndices)
		{
			List<int> indices;
			if (this.tags == null || !this.tags.TryGetValue(tag, out indices))
				return;

			foreach (int indexToUnTag in taggedIndices)
			{
				// Find the index of the tagged index within the index listing
				int index = indices.BinarySearch(indexToUnTag);
				if (index >= 0)
					indices.RemoveAt(index);
			}
		}

		public IEnumerator<Rect> GetEnumerator()
		{
			return ((ICollection<Rect>)this.rects).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.rects.GetEnumerator();
		}

		#region IList Implementation

		private readonly object syncRoot = new object();

		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsSynchronized
		{
			get { return false; }
		}

		[EditorHintFlags(MemberFlags.Invisible)]
		public object SyncRoot
		{
			get { return this.syncRoot; }
		}

		object IList.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (Rect) value; }
		}

		[EditorHintFlags(MemberFlags.Invisible)]
		bool IList.IsFixedSize
		{
			get { return false; }
		}

		int IList.Add(object value)
		{
			if (value is Rect)
			{
				this.Add((Rect)value);
				return this.Count - 1;
			}
			return -1;
		}

		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this.rects).CopyTo(array, index);
		}

		bool IList.Contains(object value)
		{
			if (value is Rect)
			{
				return this.Contains((Rect)value);
			}
			return false;
		}

		int IList.IndexOf(object value)
		{
			if (value is Rect)
			{
				return this.IndexOf((Rect)value);
			}
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			if (value is Rect)
			{
				this.Insert(index, (Rect)value);
			}
		}

		void IList.Remove(object value)
		{
			if (value is Rect)
			{
				this.Remove((Rect)value);
			}
		}

		#endregion
	}
}
