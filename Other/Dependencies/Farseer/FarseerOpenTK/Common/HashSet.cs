
#if WINDOWS_PHONE || XBOX

//TODO: FIX

using System;
using System.Collections;
using System.Collections.Generic;

namespace FarseerPhysics.Common
{
    
    public class HashSet<T> : ICollection<T>
    {
        private Dictionary<T, short> _dict;

        public HashSet(int capacity)
        {
            _dict = new Dictionary<T, short>(capacity);
        }

        public HashSet()
        {
            _dict = new Dictionary<T, short>();
        }

        // Methods

#region ICollection<T> Members

        public void Add(T item)
        {
            // We don't care for the value in dictionary, Keys matter.
            _dict.Add(item, 0);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(T item)
        {
            return _dict.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            return _dict.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

        // Properties
        public int Count
        {
            get { return _dict.Keys.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    }
}
#endif