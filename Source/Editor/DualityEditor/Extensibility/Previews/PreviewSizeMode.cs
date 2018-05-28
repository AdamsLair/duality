using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	public enum PreviewSizeMode
	{
		FixedNone,
		/// <summary>
		/// The preview can be stretched along its Y-axis to fit the desired dimensions
		/// </summary>
		FixedWidth,
		/// <summary>
		/// The preview can be stretched along its X-axis to fit the desired dimensions
		/// </summary>
		FixedHeight,
		/// <summary>
		/// The preview will be scaled equally in both directions to fit the desired dimensions
		/// </summary>
		FixedBoth
	}
}
