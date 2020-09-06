using System.Collections.Generic;
using System.Xml.Linq;

namespace Duality.Editor.Plugins.ObjectInspector
{
	/// <summary>
	/// Contains all user settings for the object inspector plugin.
	/// </summary>
	public class ObjectInspectorPluginSettings
	{
		private Dictionary<int, ObjectInspectorSettings> inspectorSettingsById = new Dictionary<int, ObjectInspectorSettings>();

		/// <summary>
		/// Contains user settings that are defined per inspector window.
		/// </summary>
		public Dictionary<int, ObjectInspectorSettings> InspectorSettingsById
		{
			get { return this.inspectorSettingsById; }
			set { this.inspectorSettingsById = value; }
		}
	}
}
