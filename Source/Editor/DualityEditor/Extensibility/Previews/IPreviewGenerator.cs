using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	/// <summary>
	/// Describes an object that can generate previews of an object
	/// </summary>
	/// <seealso cref="PreviewGenerator{T}"/>
	public interface IPreviewGenerator
	{
		/// <summary>
		/// The priority to assign to this generator. Generators
		/// with higher priorty will be checked first and the first
		/// preview successfully created will be used.
		/// </summary>
		int Priority { get; }
		/// <summary>
		/// The type of object that this generator can provide previews for.
		/// </summary>
		Type ObjectType { get; }

		/// <summary>
		/// Generates a preview according to the given query
		/// </summary>
		/// <param name="settings">The query to perform</param>
		void Perform(IPreviewQuery settings);
	}
}
