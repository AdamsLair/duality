using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Resources
{
	/// <summary>
	/// The scope of a <see cref="Shader">shader</see> variable
	/// </summary>
	public enum ShaderFieldScope
	{
		/// <summary>
		/// Unknown scope
		/// </summary>
		Unknown = -1,

		/// <summary>
		/// It is a uniform variable, i.e. constant during all rendering stages
		/// and set once per <see cref="Duality.Drawing.BatchInfo">draw batch</see>.
		/// </summary>
		Uniform,
		/// <summary>
		/// It is a vertex attribute, i.e. defined for each vertex separately.
		/// </summary>
		Attribute
	}
}
