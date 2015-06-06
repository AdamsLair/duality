using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Editor
{
	/// <summary>
	/// Some general flags for Type members that indicate preferred editor behaviour.
	/// </summary>
	[Flags]
	public enum MemberFlags
	{
		/// <summary>
		/// No flags set.
		/// </summary>
		None			= 0x00,

		/// <summary>
		/// When editing the Properties or Fields value, a final set operation is requested to finish editing.
		/// </summary>
		ForceWriteback	= 0x01,
		/// <summary>
		/// The member is considered invisible. Will override visibility rules derived from reflection.
		/// </summary>
		Invisible		= 0x02,
		/// <summary>
		/// The member is considered read-only, even if writing is possible via reflection.
		/// </summary>
		ReadOnly		= 0x04,
		/// <summary>
		/// Indicates that editing the member may have an effect on any other member of the current object.
		/// </summary>
		AffectsOthers	= 0x08,
		/// <summary>
		/// The member is considered visible. Will override visibility rules derived from reflection.
		/// </summary>
		Visible			= 0x10
	}
}
