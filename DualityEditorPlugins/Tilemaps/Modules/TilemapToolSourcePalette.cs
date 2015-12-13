using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps
{
	public partial class TilemapToolSourcePalette : DockContent
	{
		public TilemapToolSourcePalette()
		{
			this.InitializeComponent();
			this.mainToolStrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}

		private void ApplySelectedTileset()
		{
			Tileset tileset = TilemapsEditorSelectionParser.QuerySelectedTileset();

			this.tilesetView.Tileset = tileset;
			this.labelTileset.Text = (tileset != null) ? 
				string.Format(TilemapsRes.TilePalette_SelectedTileset, tileset.Name) : 
				TilemapsRes.TilePalette_NoTilesetSelected;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			DualityEditorApp.SelectionChanged += this.DualityEditorApp_SelectionChanged;
			this.ApplySelectedTileset();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			DualityEditorApp.SelectionChanged -= this.DualityEditorApp_SelectionChanged;
		}

		private void DualityEditorApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.SameObjects) return;
			this.ApplySelectedTileset();
		}
	}
}
