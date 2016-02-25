using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Aga.Controls.Tree;

namespace Duality.Editor.Plugins.Tilemaps
{
	public abstract class TilesetEditorLayerNode : Node
	{
		public abstract string Title { get; }
		public abstract string Description { get; }
	}
}
