using System;
using System.Linq;

using Duality.Editor;
using Duality.Drawing;
using Duality.Properties;
using Duality.Backend;


namespace Duality.Resources
{
	/// <summary>
	/// Describes the position of a newly added rendering step in a sequence of pre-existing rendering steps.
	/// </summary>
	public enum RenderStepPosition
	{
		/// <summary>
		/// Before the rendering step to which the new one is anchored.
		/// </summary>
		Before,
		/// <summary>
		/// After the rendering step to which the new one is anchored.
		/// </summary>
		After,
		/// <summary>
		/// At the beginning of the sequence.
		/// </summary>
		First,
		/// <summary>
		/// At the end of the sequence.
		/// </summary>
		Last
	}
}
