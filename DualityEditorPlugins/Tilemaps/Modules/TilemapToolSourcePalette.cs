using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using WeifenLuo.WinFormsUI.Docking;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps
{
	public partial class TilemapToolSourcePalette : DockContent
	{
		private Tileset SelectedTileset
		{
			get { return this.tilesetView.Tileset; }
			set
			{
				// Apply the selection to the palette's controls
				this.tilesetView.Tileset = value;
				this.labelTileset.Text = (value != null) ? 
					string.Format(TilemapsRes.TilePalette_SelectedTileset, value.Name) : 
					TilemapsRes.TilePalette_NoTilesetSelected;
			}
		}

		public TilemapToolSourcePalette()
		{
			this.InitializeComponent();
			this.mainToolStrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}

		internal void SaveUserData(XElement node)
		{
			node.SetElementValue("DarkBackground", this.buttonBrightness.Checked);
		}
		internal void LoadUserData(XElement node)
		{
			bool tryParseBool;

			if (node.GetElementValue("DarkBackground", out tryParseBool)) this.buttonBrightness.Checked = tryParseBool;

			this.ApplyBrightness();
		}

		private void ApplySelectedTileset()
		{
			// Query the selected tileset - or just use the last one (for editing continuity reasons) if none is selected.
			Tileset lastTileset = this.SelectedTileset;
			Tileset tileset = TilemapsEditorSelectionParser.QuerySelectedTileset() ?? lastTileset;
			if (tileset != null && tileset.Disposed)
				tileset = null;

			this.SelectedTileset = tileset;
		}
		private void ApplyBrightness()
		{
			bool darkMode = this.buttonBrightness.Checked;
			this.tilesetView.BackColor = darkMode ? Color.FromArgb(64, 64, 64) : Color.FromArgb(192, 192, 192);
			this.tilesetView.ForeColor = darkMode ? Color.FromArgb(255, 255, 255) : Color.FromArgb(0, 0, 0);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			DualityEditorApp.SelectionChanged += this.DualityEditorApp_SelectionChanged;
			Resource.ResourceDisposing        += this.Resource_ResourceDisposing;
			this.ApplySelectedTileset();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			DualityEditorApp.SelectionChanged -= this.DualityEditorApp_SelectionChanged;
			Resource.ResourceDisposing        -= this.Resource_ResourceDisposing;
		}
		
		private void buttonBrightness_CheckedChanged(object sender, EventArgs e)
		{
			this.ApplyBrightness();
		}
		private void Resource_ResourceDisposing(object sender, ResourceEventArgs e)
		{
			if (!e.IsResource) return;

			// Deselect the current tileset, if it's being disposed
			if (this.SelectedTileset == e.Content)
			{
				this.SelectedTileset = null;
			}
		}
		private void DualityEditorApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.SameObjects) return;
			this.ApplySelectedTileset();
		}
	}
}
