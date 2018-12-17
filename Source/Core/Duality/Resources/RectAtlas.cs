using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Duality.Editor;

namespace Duality.Resources
{
	public class RectAtlas : IList<Rect>, IList
	{
		public struct RectAtlasItem
		{
			public Rect Rect;
			public Vector2 Pivot;
			public string Tag;
		}

		private RawList<RectAtlasItem> items;
		private Dictionary<string, List<int>> tags;

		/// <summary>
		/// The items of the <see cref="RectAtlas"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback | MemberFlags.Visible)]
		private RawList<RectAtlasItem> Items
		{
			get { return this.items; }
			set
			{
				// Maybe even validate inspector-provided value contents here?
				this.items = value;
				this.RebuildTagLookup();
			}
		}

		/// <summary>
		/// [GET] The number of rects in the atlas
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int Count
		{
			get { return this.items.Count; }
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
			get { return this.items[index].Rect; }
			set { this.items.Data[index].Rect = value; }
		}

		/// <summary>
		/// [GET] Gets the first rect with the given tag
		/// </summary>
		public Rect this[string tag]
		{
			get { return this.items[this.tags[tag][0]].Rect; }
		}

		public RectAtlas()
		{
			this.items = new RawList<RectAtlasItem>();
		}

		public RectAtlas(int count)
		{
			this.items = new RawList<RectAtlasItem>(count);
		}

		public RectAtlas(IEnumerable<Rect> rects)
		{
			if (rects == null)
				throw new ArgumentNullException("rects");
			this.items = new RawList<RectAtlasItem>();
			foreach (var r in rects)
			{
				this.items.Add(new RectAtlasItem
				{
					Rect = r
				});
			}
		}

		public RectAtlas(RectAtlas other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			this.items = new RawList<RectAtlasItem>(other.items);
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
			this.items.Add(new RectAtlasItem{ Rect = item });
		}

		/// <summary>
		/// Clears the atlas of all rects and associated information
		/// </summary>
		public void Clear()
		{
			this.items.Clear();
			this.tags = null;
		}

		/// <summary>
		/// Determines whether or not this atlas contains the given rect
		/// </summary>
		/// <param name="item">The atlas to look for</param>
		/// <returns>Whether or not the rect is contained in the atlas</returns>
		public bool Contains(Rect item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].Rect == item)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Copies the rects in the atlas to the given array starting at the given index.
		/// </summary>
		/// <param name="array">The array to copy the atlas rects to</param>
		/// <param name="arrayIndex">The index within the given array to place the atlas rects</param>
		public void CopyTo(Rect[] array, int arrayIndex)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				array[i + arrayIndex] = this.items[i].Rect;
			}
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
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].Rect == item)
					return i;
			}
			return -1;
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
			if (index > this.items.Count)
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

			this.items.Insert(index, new RectAtlasItem { Rect = item });
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
			if (index > this.items.Count)
				throw new ArgumentOutOfRangeException("index", "The insertion index was greater than the size number of rects in the atlas");

			if (this.tags != null)
			{
				List<string> emptyKeys = new List<string>();

				// Adjust tagged indices to account for the removal of this item
				foreach (var kvp in this.tags)
				{
					List<int> indexList = kvp.Value;
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

					if (indexList.Count == 0)
						emptyKeys.Add(kvp.Key);
				}

				// Remove any list of tagged indices that are now empty
				foreach (string emptyKey in emptyKeys)
					this.tags.Remove(emptyKey);
			}

			this.items.RemoveAt(index);
		}

		/// <summary>
		/// Tags the rects at the given indices with the given tag. If the rect at the given index
		/// already has the given tag, it will be ignored.
		/// </summary>
		/// <param name="tag">The tag to apply to the rects. Cannot be null or empty.</param>
		/// <param name="indices">The indices of the rects to tag</param>
		/// <exception cref="ArgumentException">
		/// The given tag was null or empty.
		/// </exception>
		/// /// <exception cref="ArgumentNullException">
		/// The given indices array was null.
		/// </exception>
		public void TagIndices(string tag, int[] indices)
		{
			if (string.IsNullOrEmpty(tag))
				throw new ArgumentException("The tag cannot be null or empty", "tag");
			if (indices == null)
				throw new ArgumentNullException("indices");

			if (this.tags == null)
				this.tags = new Dictionary<string, List<int>>();
			if (!this.tags.ContainsKey(tag))
				this.tags.Add(tag, new List<int>());

			List<int> taggedIndices = this.tags[tag];
			foreach (int newTagIndex in indices)
			{
				// Remove this index from the index list of its current tag
				List<int> existingTagList;
				if (this.items.Data[newTagIndex].Tag != null
					&& this.tags.TryGetValue(this.items.Data[newTagIndex].Tag, out existingTagList))
				{
					existingTagList.Remove(newTagIndex);
					if (existingTagList.Count == 0)
					{
						this.tags.Remove(this.items.Data[newTagIndex].Tag);
					}
				}

				this.items.Data[newTagIndex].Tag = tag;

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
				{
					indices.RemoveAt(index);
					this.items.Data[index].Tag = null;
				}
			}
		}

		/// <summary>
		/// Gets the tag associated with the given index.
		/// </summary>
		/// <param name="index">The index within the atlas to find the tag for.</param>
		/// <param name="tag">The tag to associate with the given index.</param>
		/// <returns>The tag of the given rect atlas index</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The given index was outside the range of the number of elements in the atlas.
		/// </exception>
		public void SetTag(int index, string tag)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index", "The index cannot be less than 0");
			if (index > this.items.Count)
				throw new ArgumentOutOfRangeException("index", "The index was greater than the size number of rects in the atlas");

			this.TagIndices(tag, new []{ index });
		}

		/// <summary>
		/// Gets the tag associated with the given index.
		/// </summary>
		/// <param name="index">The index within the atlas to find the tag for.</param>
		/// <returns>The tag of the given rect atlas index</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The given index was outside the range of the number of elements in the atlas.
		/// </exception>
		public string GetTag(int index)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index", "The index cannot be less than 0");
			if (index > this.items.Count)
				throw new ArgumentOutOfRangeException("index", "The index was greater than the size number of rects in the atlas");

			return this.items[index].Tag;
		}

		/// <summary>
		/// Gets the pivot associated with the given index
		/// </summary>
		/// <param name="index">The index within the atlas to find the pivot for.</param>
		/// <returns>The pivot of the given rect atlas index</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The given index was outside the range of the number of elements in the atlas.
		/// </exception>
		public Vector2 GetPivot(int index)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index", "The index cannot be less than 0");
			if (index > this.items.Count)
				throw new ArgumentOutOfRangeException("index", "The index was greater than the size number of rects in the atlas");

			return this.items[index].Pivot;
		}

		/// <summary>
		/// Sets the pivot of the rect at the given index.
		/// </summary>
		/// <param name="index">The index of the rect to set the pivot for.</param>
		/// <param name="pivot">The new pivot of the atlas rect.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The given index was outside the range of the number of elements in the atlas.
		/// </exception>
		public void SetPivot(int index, Vector2 pivot)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException("index", "The index cannot be less than 0");
			if (index > this.items.Count)
				throw new ArgumentOutOfRangeException("index", "The index was greater than the size number of rects in the atlas");

			this.items.Data[index].Pivot = pivot;
		}

		/// <summary>
		/// Rebuilds the tag lookup dictionary from scratch based
		/// off of the data in the <see cref="items"/> field.
		/// </summary>
		private void RebuildTagLookup()
		{
			if (this.tags != null)
				this.tags.Clear();

			for (int i = 0; i < this.items.Count; i++)
			{
				if (string.IsNullOrEmpty(this.items[i].Tag))
					continue;

				if (this.tags == null)
					this.tags = new Dictionary<string, List<int>>();

				List<int> indices;
				if (!this.tags.TryGetValue(this.items[i].Tag, out indices))
				{
					indices = new List<int>();
					this.tags[this.items[i].Tag] = indices;
				}
				indices.Add(i);
			}
		}

		public IEnumerator<Rect> GetEnumerator()
		{
			return this.items.Select(i => i.Rect).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.Select(i => i.Rect).GetEnumerator();
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
			for (int i = 0; i < this.items.Count; i++)
			{
				array.SetValue(this.items[i].Rect, i + index);
			}
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
