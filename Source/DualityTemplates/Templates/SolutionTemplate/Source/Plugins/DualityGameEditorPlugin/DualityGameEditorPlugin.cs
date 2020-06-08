﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Editor;

namespace DualityGameEditorPlugin
{
	/// <summary>
	/// Defines a Duality editor plugin.
	/// </summary>
	public class DualityGameEditorPlugin : EditorPlugin
	{
		public override string Id
		{
			get { return "Duality_EditorPlugin"; }
		}
	}
}