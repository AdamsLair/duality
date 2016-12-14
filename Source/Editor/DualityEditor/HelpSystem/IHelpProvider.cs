using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality;

namespace Duality.Editor
{
	public interface IHelpProvider
	{
		HelpInfo ProvideHoverHelp(Point localPos, ref bool captured);
	}
}
