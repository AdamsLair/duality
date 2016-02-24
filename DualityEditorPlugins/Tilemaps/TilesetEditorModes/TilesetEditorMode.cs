using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	public abstract class TilesetEditorMode
	{
		public abstract string Id { get; }
		public abstract string Name { get; }
		public abstract Image Icon { get; }

		public virtual string Description
		{
			get { return null; }
		}
		public virtual int SortOrder
		{
			get { return 0; }
		}

		internal protected void OnEnter() { }
		internal protected void OnLeave() { }
	}
}
