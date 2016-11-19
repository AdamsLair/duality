using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;

using Duality.Editor;
using Duality.Editor.Properties;

namespace DynamicLighting
{
	public class DynamicLightingPlugin : EditorPlugin
	{
		public override string Id
		{
			get { return "DynamicLighting"; }
		}
	}
}
