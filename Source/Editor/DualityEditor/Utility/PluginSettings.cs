using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Duality.Resources;

namespace Duality.Editor
{
	public class PluginSettings
	{
		private XElement oldStyleSettings = XElement.Parse("<Plugins></Plugins>");
		private List<object> plugins = new List<object>();

		[Obsolete("Use the model based api")]
		public XElement OldStyleSettings
		{
			get { return this.oldStyleSettings; }
			set { this.oldStyleSettings = value; }
		}

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
		internal void Clear()
		{
			this.plugins.Clear();
			this.oldStyleSettings = XElement.Parse("<Plugins></Plugins>");
		}
	}
}
