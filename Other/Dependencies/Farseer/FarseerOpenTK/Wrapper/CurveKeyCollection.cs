using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FarseerPhysics.Wrapper
{
	[Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
	public class CurveKeyCollection : ICollection<CurveKey>, IEnumerable<CurveKey>, IEnumerable
	{
		// Fields
		internal float InvTimeRange;
		internal bool IsCacheAvailable = true;
		private List<CurveKey> Keys = new List<CurveKey>();
		internal float TimeRange;

		// Methods
		public void Add(CurveKey item)
		{
			if (item == null)
			{
				throw new ArgumentNullException();
			}
			int index = this.Keys.BinarySearch(item);
			if (index >= 0)
			{
				while ((index < this.Keys.Count) && (item.Position == this.Keys[index].Position))
				{
					index++;
				}
			}
			else
			{
				index = ~index;
			}
			this.Keys.Insert(index, item);
			this.IsCacheAvailable = false;
		}

		public void Clear()
		{
			this.Keys.Clear();
			this.TimeRange = this.InvTimeRange = 0f;
			this.IsCacheAvailable = false;
		}

		public CurveKeyCollection Clone()
		{
			CurveKeyCollection keys = new CurveKeyCollection();
			keys.Keys = new List<CurveKey>(this.Keys);
			keys.InvTimeRange = this.InvTimeRange;
			keys.TimeRange = this.TimeRange;
			keys.IsCacheAvailable = true;
			return keys;
		}

		internal void ComputeCacheValues()
		{
			this.TimeRange = this.InvTimeRange = 0f;
			if (this.Keys.Count > 1)
			{
				this.TimeRange = this.Keys[this.Keys.Count - 1].Position - this.Keys[0].Position;
				if (this.TimeRange > float.Epsilon)
				{
					this.InvTimeRange = 1f / this.TimeRange;
				}
			}
			this.IsCacheAvailable = true;
		}

		public bool Contains(CurveKey item)
		{
			return this.Keys.Contains(item);
		}

		public void CopyTo(CurveKey[] array, int arrayIndex)
		{
			this.Keys.CopyTo(array, arrayIndex);
			this.IsCacheAvailable = false;
		}

		public IEnumerator<CurveKey> GetEnumerator()
		{
			return this.Keys.GetEnumerator();
		}

		public int IndexOf(CurveKey item)
		{
			return this.Keys.IndexOf(item);
		}

		public bool Remove(CurveKey item)
		{
			this.IsCacheAvailable = false;
			return this.Keys.Remove(item);
		}

		public void RemoveAt(int index)
		{
			this.Keys.RemoveAt(index);
			this.IsCacheAvailable = false;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Keys.GetEnumerator();
		}

		// Properties
		public int Count
		{
			get
			{
				return this.Keys.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public CurveKey this[int index]
		{
			get
			{
				return this.Keys[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (this.Keys[index].Position == value.Position)
				{
					this.Keys[index] = value;
				}
				else
				{
					this.Keys.RemoveAt(index);
					this.Add(value);
				}
			}
		}
	}
}
