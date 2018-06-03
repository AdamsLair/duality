using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	[Flags]
	public enum PixmapNumberingStyle
	{
		None = 0x1,
		Hovered = 0x2,
		All = 0x4
	}
}
