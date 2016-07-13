using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	/// <summary>
	/// Specifies ways in which <see cref="TilesetEditorMode"/> layers can be edited.
	/// </summary>
	[Flags]
	public enum LayerEditingCaps
	{
		None = 0x0,

		/// <summary>
		/// Allow users to modify existing layers.
		/// </summary>
		Modify = 0x1,
		/// <summary>
		/// Allow users to add or remove layers.
		/// </summary>
		AddRemove = 0x2,
		
		All = Modify | AddRemove
	}
}
