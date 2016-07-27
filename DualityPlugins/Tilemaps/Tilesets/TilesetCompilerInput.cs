using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Cloning;
using Duality.Resources;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Specifies input data for the <see cref="TilesetCompiler"/> to work with.
	/// </summary>
	public struct TilesetCompilerInput
	{
		/// <summary>
		/// Input tile data. This data is not modified, but provided in
		/// mutable form for performance reasons / to allow direct array access.
		/// </summary>
		public RawList<TileInput> TileInput;
		/// <summary>
		/// Visual layer configuration. This data is not modified, only read.
		/// </summary>
		public IReadOnlyList<TilesetRenderInput> RenderConfig;

		/// <summary>
		/// (Optional) potentially existing output that was produced in previous compilations.
		/// A <see cref="TilesetCompiler"/> may choose to re-use some of the data
		/// structures provided here, so they might be returned as output in modified form.
		/// </summary>
		public TilesetCompilerOutput ExistingOutput;
	}
}
