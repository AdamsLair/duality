using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Backend;

using OpenTK;
using OpenTK.Graphics;

namespace Duality.Editor.Backend.DefaultOpenTK
{
	public class EditorGraphicsBackend : IEditorGraphicsBackend
	{
		string IDualityBackend.Id
		{
			get { return "DefaultOpenTKEditorGraphicsBackend"; }
		}
		string IDualityBackend.Name
		{
			get { return "GLControl (OpenTK)"; }
		}
		int IDualityBackend.Priority
		{
			get { return 0; }
		}

		bool IDualityBackend.CheckAvailable()
		{
			return true;
		}
		void IDualityBackend.Init()
		{
			// Since we'll be using only one context, we don't need sharing
			OpenTK.Graphics.GraphicsContext.ShareContexts = false;
		}
		void IDualityBackend.Shutdown() { }

		INativeEditorGraphicsContext IEditorGraphicsBackend.CreateContext()
		{
			return new NativeEditorGraphicsContext();
		}
	}
}
