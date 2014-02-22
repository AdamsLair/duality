using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Provides custom information about the Duality environment in which this application / game runs.
	/// It is persistent beyond installing or deleting a specific Duality game and is shared among all Duality
	/// games. Developers can use the DualityMetaData API to share player-related game information, such as
	/// stats, player descisions, tasks, progress, etc.
	/// </summary>
	[Serializable]
	public class DualityMetaData
	{
		/// <summary>
		/// An array of valid path separators for meta data.
		/// </summary>
		public static readonly char[] Separator = "/\\".ToCharArray();

		[Serializable]
		private class Entry
		{
			public Dictionary<string,Entry> children;
			public string value;

			public Entry()
			{
				this.children = null;
				this.value = null;
			}
			public Entry(Entry cc)
			{
				this.value = cc.value;
				this.children = new Dictionary<string,Entry>();

				foreach (var pair in cc.children)
					this.children[pair.Key] = new Entry(pair.Value);
			}
			public Entry(string value)
			{
				this.value = value;
				this.children = null;
			}

			public Entry ReadValueEntry(string key)
			{
				if (String.IsNullOrEmpty(key)) return this;
				if (this.children == null || this.children.Count == 0) return null;

				int sepIndex = key.IndexOfAny(Separator);
				string singleKey;
				if (sepIndex != -1)
				{
					singleKey = key.Substring(0, sepIndex);
					key = key.Substring(sepIndex + 1, key.Length - sepIndex - 1);
				}
				else
				{
					singleKey = key;
					key = null;
				}

				Entry valEntry;
				if (this.children.TryGetValue(singleKey, out valEntry))
					return valEntry.ReadValueEntry(key);
				else
					return null;
			}
			public string ReadValue(string key)
			{
				Entry valEntry = this.ReadValueEntry(key);
				return valEntry != null ? valEntry.value : null;
			}
			public void WriteValue(string key, string value)
			{
				if (String.IsNullOrEmpty(key))
				{
					this.value = value;
					return;
				}

				int sepIndex = key.IndexOfAny(Separator);
				string singleKey;
				if (sepIndex != -1)
				{
					singleKey = key.Substring(0, sepIndex);
					key = key.Substring(sepIndex + 1, key.Length - sepIndex - 1);
				}
				else
				{
					singleKey = key;
					key = null;
				}

				Entry valEntry;
				if (this.children == null || !this.children.TryGetValue(singleKey, out valEntry))
				{
					if (this.children == null) this.children = new Dictionary<string,Entry>();
					valEntry = new Entry();
					this.children[singleKey] = valEntry;
				}
				valEntry.WriteValue(key, value);
			}
		}

		private Entry   rootEntry       = new Entry();

		/// <summary>
		/// [GET / SET] The string value that is located at the specified key (path). Keys are organized hierarchially and behave
		/// like file paths. Use the normal path separator chars to address keys in keys.
		/// </summary>
		/// <param name="key">The key that defines where to look for the value.</param>
		/// <returns>The string value associated with the specified key.</returns>
		/// <example>
		/// The following code reads and writes the value of <c>MainNode / SubNode / SomeKey</c>:
		/// <code>
		/// string value = DualityApp.MetaData["MainNode/SubNode/SomeKey"];
		/// DualityApp.MetaData["MainNode/SubNode/SomeKey"] = "Some other value";
		/// </code>
		/// </example>
		/// <seealso cref="ReadValue(string)"/>
		/// <seealso cref="ReadValueAs{T}(string, out T)"/>
		public string this[string key]
		{
			get { return this.ReadValue(key); }
			set { this.WriteValue(key, value); }
		}

		/// <summary>
		/// Reads the specified key's string value. Keys are organized hierarchially and behave
		/// like file paths. Use the normal path separator chars to address keys in keys.
		/// </summary>
		/// <param name="key">The key that defines where to look for the value.</param>
		/// <returns>The string value associated with the specified key.</returns>
		/// <example>
		/// The following code reads the value of <c>MainNode / SubNode / SomeKey</c>:
		/// <code>
		/// string value = DualityApp.MetaData.ReadValue("MainNode/SubNode/SomeKey");
		/// </code>
		/// </example>
		/// <seealso cref="ReadValueAs{T}(string, out T)"/>
		public string ReadValue(string key)
		{
			return this.rootEntry.ReadValue(key);
		}
		/// <summary>
		/// Reads the specified key's string value and tries to parse it.
		/// </summary>
		/// <typeparam name="T">The desired value type</typeparam>
		/// <param name="key">The key that defines where to look for the value.</param>
		/// <param name="value">The parsed value based on the string that is associated with the specified key.</param>
		/// <returns>True, if successful, false if not.</returns>
		/// <seealso cref="ReadValue(string)"/>
		/// <example>
		/// The following code writes and reads an int value:
		/// <code>
		/// DualityApp.MetaData.WriteValue("SomeKey", 42);
		/// int value =  DualityApp.MetaData.ReadValueAs{int}("SomeKey");
		/// </code>
		/// </example>
		public bool ReadValueAs<T>(string key, out T value)
		{
			string valStr = this.ReadValue(key);
			try
			{
				value = (T)Convert.ChangeType(valStr, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
				return true;
			}
			catch (Exception)
			{
				value = default(T);
				return false;
			}
		}
		/// <summary>
		/// Reads all the <see cref="KeyValuePair{T,U}"/>s that are children of the specified key.
		/// </summary>
		/// <param name="key">The key of which to return child values.</param>
		/// <returns>An enumeration of <see cref="KeyValuePair{T,U}"/>s.</returns>
		/// <example>
		/// The following code creates a small hierarchy and reads a part of it out again:
		/// <code>
		/// DualityApp.MetaData["MainNode/SubNode/SomeKey"] = "42";
		/// DualityApp.MetaData["MainNode/SubNode/SomeOtherKey"] = "43";
		/// DualityApp.MetaData["MainNode/SubNode/SomeOtherKey2"] = "44";
		/// DualityApp.MetaData["MainNode/SubNode2"] = "Something";
		/// 
		/// var pairs = DualityApp.MetaData.ReadSubValues("MainNode/SubNode");
		/// foreach (var pair in pairs)
		/// {
		///     Log.Core.Write("{0}: {1}", pair.Key, pair.Value);
		/// }
		/// </code>
		/// The expected output is:
		/// <code>
		/// SomeKey: 42
		/// SomeOtherKey: 43
		/// SomeOtherKey2: 44
		/// </code>
		/// </example>
		public IEnumerable<KeyValuePair<string,string>> ReadSubValues(string key)
		{
			Entry parentEntry = this.rootEntry.ReadValueEntry(key);
			if (parentEntry == null) yield break;

			foreach (var pair in parentEntry.children)
				yield return new KeyValuePair<string,string>(pair.Key, pair.Value.value);
		}
		/// <summary>
		/// Writes the specified string value to the specified key. Keys are organized hierarchially and behave
		/// like file paths. Use the normal path separator chars to address keys in keys.
		/// </summary>
		/// <param name="key">The key that defines to write the value to.</param>
		/// <param name="value">The value to write</param>
		/// <seealso cref="WriteValue{T}(string, T)"/>
		public void WriteValue(string key, string value)
		{
			this.rootEntry.WriteValue(key, value);
		}
		/// <summary>
		/// Writes the specified value to the specified key. Keys are organized hierarchially and behave
		/// like file paths. Use the normal path separator chars to address keys in keys.
		/// </summary>
		/// <typeparam name="T">The value's Type.</typeparam>
		/// <param name="key">The key that defines to write the value to.</param>
		/// <param name="value">The value to write</param>
		/// <seealso cref="WriteValue(string, string)"/>
		public void WriteValue<T>(string key, T value)
		{
			string valStr = value as string;
			if (valStr != null)
			{
				this.WriteValue(key, valStr);
				return;
			}

			IFormattable valFormattable = value as IFormattable;
			if (valFormattable != null)
			{
				this.WriteValue(key, valFormattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture));
				return;
			}

			this.WriteValue(key, value.ToString());
			return;
		}
	}
}
