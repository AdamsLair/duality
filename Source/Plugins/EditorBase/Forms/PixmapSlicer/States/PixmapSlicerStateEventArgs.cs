using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Duality.Editor.Controls.ToolStrip;
using Duality.Resources;
using Font = System.Drawing.Font;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// <see cref="EventArgs"/> for general pixmap slicer
	/// state events/changes
	/// </summary>
	public class PixmapSlicerStateEventArgs : EventArgs
	{
		public Type StateType { get; private set; }

		public PixmapSlicerStateEventArgs(Type stateType)
		{
			this.StateType = stateType;
		}
	}
}
