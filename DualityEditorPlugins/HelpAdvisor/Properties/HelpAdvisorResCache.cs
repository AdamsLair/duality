using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Plugins.HelpAdvisor.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class HelpAdvisorResCache
	{
		public static readonly Bitmap	IconHelp	= HelpAdvisorRes.IconHelp;
	}
}
