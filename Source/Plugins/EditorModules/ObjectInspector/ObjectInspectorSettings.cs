using System.Collections.Generic;
using System.Xml.Linq;

namespace Duality.Editor.Plugins.ObjectInspector
{
	public class ObjectInspectorSettings
	{
		public List<ObjectInspectorState> ObjectInspectors { get; set; } = new List<ObjectInspectorState>();
	}

	public class ObjectInspectorState
	{
		private int id;
		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		private bool autoRefresh = true;
		public bool AutoRefresh
		{
			get { return this.autoRefresh; }
			set { this.autoRefresh = value; }
		}

		private bool locked;
		public bool Locked
		{
			get { return this.locked; }
			set { this.locked = value; }
		}

		private string titleText = "Object Inspector";
		public string TitleText
		{
			get { return this.titleText; }
			set { this.titleText = value; }
		}

		private bool debugMode;
		public bool DebugMode
		{
			get { return this.debugMode; }
			set { this.debugMode = value; }
		}

		private bool sortByName;
		public bool SortByName
		{
			get { return this.sortByName; }
			set { this.sortByName = value; }
		}

		private XElement expandState;
		public XElement ExpandState
		{
			get { return this.expandState; }
			set { this.expandState = value; }
		}
	}
}
