using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Backend;

namespace Duality.Editor.Backend.Dummy
{
	public class DummyEditorGraphicsBackend : IEditorGraphicsBackend
	{
		string IDualityBackend.Id
		{
			get { return "DummyEditorGraphicsBackend"; }
		}
		string IDualityBackend.Name
		{
			get { return "No Editor Graphics"; }
		}
		int IDualityBackend.Priority
		{
			get { return int.MinValue; }
		}

		bool IDualityBackend.CheckAvailable()
		{
			return true;
		}
		void IDualityBackend.Init() { }
		void IDualityBackend.Shutdown() { }

		INativeEditorGraphicsContext IEditorGraphicsBackend.CreateContext()
		{
			return new DummyNativeEditorGraphicsContext();
		}
	}
}
