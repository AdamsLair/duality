using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	public class LayerSelectionEventArgs : EventArgs
	{
		private object selectedNodeTag = null;

		/// <summary>
		/// [GET] The tag value of the selected view node. Usually, this is a reference to
		/// the model node that corresponds to it.
		/// </summary>
		public object SelectedNodeTag
		{
			get { return this.selectedNodeTag; }
		}

		public LayerSelectionEventArgs(TreeNodeAdv viewNode) : base()
		{
			this.selectedNodeTag = (viewNode != null) ? viewNode.Tag : null;
		}
	}
}
