using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Duality.Resources;

namespace Duality.Editor
{
	/// <summary>
	/// Container class for <see cref="DualityEditorUserData"/> that is defined by editor plugins.
	/// </summary>
	public class PluginSettings
	{
		private XElement oldStyleSettings = XElement.Parse("<Plugins></Plugins>");
		private List<object> data = new List<object>();

		/// <summary>
		/// Serialized XML blob that provides access to user data that is still serialized manually, as opposed to
		/// the new way via Duality serializers.
		/// </summary>
		[Obsolete("Use the model based api")]
		public XElement OldStyleSettings
		{
			get { return this.oldStyleSettings; }
			set { this.oldStyleSettings = value; }
		}

		/// <summary>
		/// Returns existing editor plugin settings of type <typeparamref name="T"/>, or creates and registers a new 
		/// instance if none existed yet.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Get<T>() where T : class, new()
		{
			T setting = this.data.OfType<T>().FirstOrDefault();
			if (setting == null)
			{
				setting = new T();
				this.data.Add(setting);
			}

			return setting;
		}
		/// <summary>
		/// Adds or replaces the stored editor plugin settings instance of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="settings"></param>
		public void Set<T>(T settings) where T : class
		{
			this.data.RemoveAll(obj => obj is T);
			this.data.Add(settings);
		}
		/// <summary>
		/// Clears all editor plugin settings from this container, and resets it to an empty state.
		/// </summary>
		internal void Clear()
		{
			this.data.Clear();
			this.oldStyleSettings = XElement.Parse("<Plugins></Plugins>");
		}
	}
}
