using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Cloning
{
	[Flags]
	public enum CloneFieldFlags
	{
		/// <summary>
		/// No flags are set at all.
		/// </summary>
		None				= 0x0,
		/// <summary>
		/// States that the Field or Object in question is relevant to its parents identity
		/// and thus should not be cloned in an identity-preserving context.
		/// </summary>
		IdentityRelevant	= 0x1,
		/// <summary>
		/// The Field or Object in question will always be skipped during cloning. No value
		/// will be assigned at all.
		/// </summary>
		Skip				= 0x2,
		/// <summary>
		/// The Field or Object in question won't be skipped during cloning due to secondary
		/// hints such as a <see cref="Duality.DontSerializeAttribute"/> attribute on the same field.
		/// </summary>
		DontSkip			= 0x4
	}
}
