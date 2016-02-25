using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	public abstract class TilesetEditorMode
	{
		private TilesetEditor editor = null;


		protected ContentRef<Tileset> SelectedTileset
		{
			get { return this.editor.SelectedTileset; }
		}

		public abstract string Id { get; }
		public abstract string Name { get; }
		public abstract Image Icon { get; }
		public abstract ITreeModel LayerModel { get; }

		public virtual string Description
		{
			get { return null; }
		}
		public virtual int SortOrder
		{
			get { return 0; }
		}


		internal void Init(TilesetEditor editor)
		{
			this.editor = editor;
		}
		internal void RaiseOnEnter()
		{
			this.OnEnter();
		}
		internal void RaiseOnLeave()
		{
			this.OnLeave();
		}
		internal void RaiseOnTilesetSelectionChanged()
		{
			this.OnTilesetSelectionChanged();
		}
		internal void RaiseOnTilesetModified(ObjectPropertyChangedEventArgs args)
		{
			this.OnTilesetModified(args);
		}

		protected virtual void OnEnter() { }
		protected virtual void OnLeave() { }
		protected virtual void OnTilesetSelectionChanged() { }
		protected virtual void OnTilesetModified(ObjectPropertyChangedEventArgs args) { }
	}
}
