using System;
using System.Reflection;
using System.Linq;

using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;
using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// String/Rect KeyValuePair-alike used for key-based atlas lookup
	/// </summary>
	public struct AtlasKeyValuePair
	{
		private string key;
		private Rect value;

		public string Key
		{
			get { return this.key; }
			set { this.key = value; }
		}

		public Rect Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public AtlasKeyValuePair(string key, Rect value)
		{
			this.key = key;
			this.value = value;
		}
	}
}
