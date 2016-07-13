using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Aga.Controls.Tree;

namespace Duality.Editor.Plugins.Tilemaps
{
	/// <summary>
	/// The data model if a <see cref="TilesetEditor"/> layer view item.
	/// </summary>
	public abstract class TilesetEditorLayerNode : Node
	{
		/// <summary>
		/// [GET] The item's short title / headline that is displayed to the user.
		/// </summary>
		public abstract string Title { get; }
		/// <summary>
		/// [GET] The item's description that is displayed to the user.
		/// </summary>
		public abstract string Description { get; }
	}
}
