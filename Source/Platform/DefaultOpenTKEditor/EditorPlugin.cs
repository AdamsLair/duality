using System;
using System.Reflection;
using System.Linq;

using Duality;
using Duality.Resources;

namespace Duality.Editor.Backend.DefaultOpenTK
{
    public class DefaultOpenTKEditorBackendPlugin : EditorPlugin
	{
		public override string Id
		{
			get { return "DefaultOpenTKEditorBackend"; }
		}
	}
}
