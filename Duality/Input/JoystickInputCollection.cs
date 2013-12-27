using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using OpenTK.Input;

namespace Duality
{
	/// <summary>
	/// Provides access to a set of <see cref="JoystickInput">JoystickInputs</see>.
	/// </summary>
	public sealed class JoystickInputCollection : IList<JoystickInput>, IList
	{
		private List<JoystickInput> input = new List<JoystickInput>();
		private JoystickInput dummyInput = new JoystickInput(true);
		

		/// <summary>
		/// [GET] Returns how many joystick inputs are known. Not all of them are necessarily available.
		/// </summary>
		public int Count
		{
			get { return input.Count; }
		}
		

		/// <summary>
		/// [GET] Returns a specific input by index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public JoystickInput this[int index]
		{
			get { return (index >= 0 && index < this.input.Count) ? this.input[index] : this.dummyInput; }
		}
		/// <summary>
		/// [GET] Returns a specific input by its <see cref="JoystickInput.Description"/>.
		/// </summary>
		/// <param name="desc"></param>
		/// <returns></returns>
		public JoystickInput this[string desc]
		{
			get { return this.input.FirstOrDefault(j => j.Description == desc) ?? this.dummyInput; }
		}


		/// <summary>
		/// Removes all extended user input sources.
		/// </summary>
		public void ClearSources()
		{
			foreach (JoystickInput registeredInput in this.input)
			{
				registeredInput.Source = null;
			}
		}
		/// <summary>
		/// Adds a new extended user input source.
		/// </summary>
		/// <param name="source"></param>
		public void AddSource(IJoystickInputSource source)
		{
			foreach (JoystickInput registeredInput in input)
			{
				if (registeredInput.Description == source.Description &&
					registeredInput.Source == null)
				{
					registeredInput.Source = source;
					return;
				}
			}

			JoystickInput newInput = new JoystickInput();
			newInput.Source = source;
			input.Add(newInput);
		}
		/// <summary>
		/// Adds a set of new extended user input sources.
		/// </summary>
		/// <param name="source"></param>
		public void AddSource(IEnumerable<IJoystickInputSource> source)
		{
			foreach (IJoystickInputSource s in source)
				AddSource(s);
		}
		/// <summary>
		/// Removes a previously registered extended user input source. 
		/// Note that the <see cref="JoystickInput"/> will still exist, 
		/// but become <see cref="JoystickInput.IsAvailable">unavailable</see> until
		/// a new matching input source is registered.
		/// </summary>
		/// <param name="source"></param>
		/// <returns>Returns true, if the source was successfully removed.</returns>
		public bool RemoveSource(IJoystickInputSource source)
		{
			foreach (JoystickInput registeredInput in this.input)
			{
				if (registeredInput.Source == source)
				{
					registeredInput.Source = null;
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Removes a set of previously registered extended user input sources. 
		/// Note that each <see cref="JoystickInput"/> will still exist, 
		/// but become <see cref="JoystickInput.IsAvailable">unavailable</see> until
		/// a new matching input source is registered.
		/// </summary>
		/// <param name="source"></param>
		/// <returns>Returns true, if all sources were successfully removed.</returns>
		public bool RemoveSource(IEnumerable<IJoystickInputSource> source)
		{
			bool allTrue = true;
			foreach (IJoystickInputSource s in source)
			{
				if (!RemoveSource(s))
					allTrue = false;
			}
			return allTrue;
		}

		/// <summary>
		/// Returns the index of a specific extended user input.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(JoystickInput item)
		{
			return input.IndexOf(item);
		}
		/// <summary>
		/// Returns whether a certain extended user input is known.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(JoystickInput item)
		{
			return input.Contains(item);
		}
		/// <summary>
		/// Copies all known extended user inputs to the specified array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(JoystickInput[] array, int arrayIndex)
		{
			this.input.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Returns an enumerator to iterate over all known extended user inputs.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<JoystickInput> GetEnumerator()
		{
			return this.input.GetEnumerator();
		}

		internal void Update()
		{
			foreach (JoystickInput j in this.input)
				j.Update();
		}

		#region Explicit Interfaces
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool IList.IsReadOnly
		{
			get { return true; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool IList.IsFixedSize
		{
			get { return false; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool ICollection<JoystickInput>.IsReadOnly
		{
			get { return true; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] object ICollection.SyncRoot
		{
			get { return (this.input as ICollection).SyncRoot; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] JoystickInput IList<JoystickInput>.this[int index]
		{
			get { return input[index]; }
			set { throw new NotSupportedException("Collection is read-only"); }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] object IList.this[int index]
		{
			get { return input[index]; }
			set { throw new NotSupportedException("Collection is read-only"); }
		}

		void IList<JoystickInput>.Insert(int index, JoystickInput item)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void IList<JoystickInput>.RemoveAt(int index)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void ICollection<JoystickInput>.Add(JoystickInput item)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void ICollection<JoystickInput>.Clear()
		{
			throw new NotSupportedException("Collection is read-only");
		}
		bool ICollection<JoystickInput>.Remove(JoystickInput item)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.input.GetEnumerator();
		}
		
		int IList.Add(object value)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void IList.Clear()
		{
			throw new NotSupportedException("Collection is read-only");
		}
		bool IList.Contains(object value)
		{
			return this.input.Contains(value);
		}
		int IList.IndexOf(object value)
		{
			return (this.input as IList).IndexOf(value);
		}
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void ICollection.CopyTo(Array array, int index)
		{
			(this.input as IList).CopyTo(array, index);
		}
		#endregion
	}
}
