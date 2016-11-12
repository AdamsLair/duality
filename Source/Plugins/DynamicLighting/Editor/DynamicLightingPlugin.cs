using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Plugins.DynamicLighting;

using Duality.Editor;
using Duality.Editor.Properties;

namespace Duality.Editor.Plugins.DynamicLighting
{
	public class DynamicLightingPlugin : EditorPlugin
	{
		public override string Id
		{
			get { return "DynamicLighting"; }
		}
	}
}
