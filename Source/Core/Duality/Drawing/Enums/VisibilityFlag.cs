using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Drawing
{
	[Flags]
	public enum VisibilityFlag : uint
	{
		None = 0U,

		// User-defined groups
		Group0 = 1U << 0,
		Group1 = 1U << 1,
		Group2 = 1U << 2,
		Group3 = 1U << 3,
		Group4 = 1U << 4,
		Group5 = 1U << 5,
		Group6 = 1U << 6,
		Group7 = 1U << 7,
		Group8 = 1U << 8,
		Group9 = 1U << 9,
		Group10 = 1U << 10,
		Group11 = 1U << 11,
		Group12 = 1U << 12,
		Group13 = 1U << 13,
		Group14 = 1U << 14,
		Group15 = 1U << 15,
		Group16 = 1U << 16,
		Group17 = 1U << 17,
		Group18 = 1U << 18,
		Group19 = 1U << 19,
		Group20 = 1U << 20,
		Group21 = 1U << 21,
		Group22 = 1U << 22,
		Group23 = 1U << 23,
		Group24 = 1U << 24,
		Group25 = 1U << 25,
		Group26 = 1U << 26,
		Group27 = 1U << 27,
		Group28 = 1U << 28,
		Group29 = 1U << 29,
		Group30 = 1U << 30,

		// Special groups (Might cause special behaviour)
		/// <summary>
		/// Special flag. This flag is set when rendering screen overlays.
		/// </summary>
		ScreenOverlay = 1U << 31,

		// Compound groups
		All = uint.MaxValue,
		AllFlags = ScreenOverlay,
		AllGroups = All & (~AllFlags)
	}
}
