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
		private List<object> plugins = new List<object>();

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
		public T GetSettings<T>() where T : class, new()
		{
			T setting = this.plugins.OfType<T>().FirstOrDefault();
			if (setting == null)
			{
				setting = new T();
				this.plugins.Add(setting);
			}

			return setting;
		}
		/// <summary>
		/// Clears all editor plugin settings from this container, and resets it to an empty state.
		/// </summary>
		internal void Clear()
		{
			this.plugins.Clear();
			this.oldStyleSettings = XElement.Parse("<Plugins></Plugins>");
		}
	}
}
