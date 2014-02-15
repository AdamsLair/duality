using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

using Duality.Resources;
using Duality.Components.Diagnostics;

namespace Duality
{
	/// <summary>
	/// Describes the reference point by which a <see cref="VisualLogEntry"/> will be transformed.
	/// </summary>
	public enum VisualLogAnchor
	{
		/// <summary>
		/// All coordinates and sizes are interpreted as screen space values.
		/// </summary>
		Screen,
		/// <summary>
		/// All coordinates and sizes are interpreted as world space values.
		/// </summary>
		World,
		/// <summary>
		/// All coordinates and sizes are interpreted as object space values.
		/// A reference object needs to be specified for this option to be functional.
		/// </summary>
		Object
	}
}
