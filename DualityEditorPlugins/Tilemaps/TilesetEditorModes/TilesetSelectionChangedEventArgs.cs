using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	/// <summary>
	/// <see cref="EventArgs"/> used for <see cref="Tileset"/> selection changes.
	/// </summary>
	public class TilesetSelectionChangedEventArgs : EventArgs
	{
		private ContentRef<Tileset>   prev   = null;
		private ContentRef<Tileset>   next   = null;

		/// <summary>
		/// [GET] The previously selected <see cref="Tileset"/>.
		/// </summary>
		public ContentRef<Tileset> Previous
		{
			get { return this.prev; }
		}
		/// <summary>
		/// [GET] The <see cref="Tileset"/> that will be selected next / is being selected.
		/// </summary>
		public ContentRef<Tileset> Next
		{
			get { return this.next; }
		}

		public TilesetSelectionChangedEventArgs(ContentRef<Tileset> prev, ContentRef<Tileset> next) : base()
		{
			this.prev = prev;
			this.next = next;
		}
	}
}
