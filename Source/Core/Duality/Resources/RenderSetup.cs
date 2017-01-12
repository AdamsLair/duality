using System;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality.Editor;
using Duality.Cloning;
using Duality.Properties;
using Duality.Backend;


namespace Duality.Resources
{
	[ExplicitResourceReference(typeof(RenderTarget), typeof(Texture), typeof(Material))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageRenderSetup)]
	public class RenderSetup : Resource
	{
		internal static void InitDefaultContent() { }
	}
}
