using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using OpenTK.Input;

namespace Duality
{
	/// <summary>
	/// Provides access to a named set of <see cref="IUserInput">user input devices</see>.
	/// </summary>
	public abstract class UserInputCollection<TInput,TSource> : IList<TInput>, IList 
		where TInput : class, IUserInput
		where TSource : class, IUserInputSource
	{
		private List<TInput> input = new List<TInput>();
		private TInput dummyInput = null;
		

		/// <summary>
		/// [GET] Returns how many user inputs are known. Not all of them are necessarily available.
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
		public TInput this[int index]
		{
			get { return (index >= 0 && index < this.input.Count) ? this.input[index] : this.dummyInput; }
		}
		/// <summary>
		/// [GET] Returns a specific input by its <see cref="IUserInput.Description"/>.
		/// </summary>
		/// <param name="desc"></param>
		/// <returns></returns>
		public TInput this[string desc]
		{
			get { return this.input.FirstOrDefault(j => j.Description == desc) as TInput ?? this.dummyInput as TInput; }
		}


		public UserInputCollection()
		{
			this.dummyInput = this.CreateDummyInput();
		}

		/// <summary>
		/// Removes all user input sources.
		/// </summary>
		public void ClearSources()
		{
			foreach (TInput registeredInput in this.input)
			{
				registeredInput.Source = null;
			}
		}
		/// <summary>
		/// Adds a new user input source.
		/// </summary>
		/// <param name="source"></param>
		public void AddSource(TSource source)
		{
			foreach (TInput registeredInput in input)
			{
				if (registeredInput.Description == source.Description &&
					registeredInput.Source == null)
				{
					registeredInput.Source = source;
					return;
				}
			}

			TInput newInput = this.CreateInput(source);
			input.Add(newInput);
		}
		/// <summary>
		/// Adds a set of new user input sources.
		/// </summary>
		/// <param name="source"></param>
		public void AddSource(IEnumerable<TSource> source)
		{
			foreach (TSource s in source)
				AddSource(s);
		}
		/// <summary>
		/// Removes a previously registered user input source. 
		/// Note that the <see cref="JoystickInput"/> will still exist, 
		/// but become <see cref="JoystickInput.IsAvailable">unavailable</see> until
		/// a new matching input source is registered.
		/// </summary>
		/// <param name="source"></param>
		/// <returns>Returns true, if the source was successfully removed.</returns>
		public bool RemoveSource(TSource source)
		{
			foreach (TInput registeredInput in this.input)
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
		/// Removes a set of previously registered user input sources. 
		/// Note that each <see cref="JoystickInput"/> will still exist, 
		/// but become <see cref="JoystickInput.IsAvailable">unavailable</see> until
		/// a new matching input source is registered.
		/// </summary>
		/// <param name="source"></param>
		/// <returns>Returns true, if all sources were successfully removed.</returns>
		public bool RemoveSource(IEnumerable<TSource> source)
		{
			bool allTrue = true;
			foreach (TSource s in source)
			{
				if (!RemoveSource(s))
					allTrue = false;
			}
			return allTrue;
		}

		/// <summary>
		/// Returns the index of a specific user input.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(TInput item)
		{
			return input.IndexOf(item);
		}
		/// <summary>
		/// Returns whether a certain user input is known.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(TInput item)
		{
			return input.Contains(item);
		}
		/// <summary>
		/// Copies all known user inputs to the specified array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(TInput[] array, int arrayIndex)
		{
			this.input.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Returns an enumerator to iterate over all known user inputs.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<TInput> GetEnumerator()
		{
			return this.input.GetEnumerator();
		}

		protected abstract TInput CreateDummyInput();
		protected abstract TInput CreateInput(TSource source);

		internal void Update()
		{
			foreach (TInput j in this.input)
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool ICollection<TInput>.IsReadOnly
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] TInput IList<TInput>.this[int index]
		{
			get { return input[index]; }
			set { throw new NotSupportedException("Collection is read-only"); }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] object IList.this[int index]
		{
			get { return input[index]; }
			set { throw new NotSupportedException("Collection is read-only"); }
		}

		void IList<TInput>.Insert(int index, TInput item)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void IList<TInput>.RemoveAt(int index)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void ICollection<TInput>.Add(TInput item)
		{
			throw new NotSupportedException("Collection is read-only");
		}
		void ICollection<TInput>.Clear()
		{
			throw new NotSupportedException("Collection is read-only");
		}
		bool ICollection<TInput>.Remove(TInput item)
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
