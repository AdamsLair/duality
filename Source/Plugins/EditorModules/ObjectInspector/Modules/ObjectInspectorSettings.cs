using System.Collections.Generic;
using System.Xml.Linq;

namespace Duality.Editor.Plugins.ObjectInspector
{
	/// <summary>
	/// Contains all user settings for a single object inspector window.
	/// </summary>
	public class ObjectInspectorSettings
	{
		private bool     autoRefresh = true;
		private bool     locked      = false;
		private string   titleText   = "Object Inspector";
		private bool     debugMode   = false;
		private bool     sortByName  = true;
		private XElement expandState = null;

		/// <summary>
		/// Whether the inspector will automatically refresh its values periodically while the 
		/// Sandbox mode is active.
		/// </summary>
		public bool AutoRefresh
		{
			get { return this.autoRefresh; }
			set { this.autoRefresh = value; }
		}
		/// <summary>
		/// Whether the inspector window is locked to its current target objects. While true, the
		/// inspector will not change targets as global editor selection changes.
		/// </summary>
		public bool Locked
		{
			get { return this.locked; }
			set { this.locked = value; }
		}
		/// <summary>
		/// The window title of this inspector window.
		/// </summary>
		public string TitleText
		{
			get { return this.titleText; }
			set { this.titleText = value; }
		}
		/// <summary>
		/// Whether the inspector will show internal and private values.
		/// </summary>
		public bool DebugMode
		{
			get { return this.debugMode; }
			set { this.debugMode = value; }
		}
		/// <summary>
		/// Whether all displayed members will be sorted by name.
		/// </summary>
		public bool SortByName
		{
			get { return this.sortByName; }
			set { this.sortByName = value; }
		}
		/// <summary>
		/// Stores the last used expand / collapse state of all UI nodes, so it can be preserved
		/// between selection changes and editor restarts.
		/// </summary>
		public XElement ExpandState
		{
			get { return this.expandState; }
			set { this.expandState = value; }
		}
	}
}
