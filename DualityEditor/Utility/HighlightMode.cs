using System;

namespace Duality.Editor
{
	[Flags]
	public enum HighlightMode
	{
		None		= 0x0,

		/// <summary>
		/// Highlights an objects conceptual representation, e.g. flashing its entry in an object overview.
		/// </summary>
		Conceptual	= 0x1,
		/// <summary>
		/// Highlights an objects spatial location, e.g. focusing it spatially in a scene view.
		/// </summary>
		Spatial		= 0x2,

		All			= Conceptual | Spatial
	}
}
