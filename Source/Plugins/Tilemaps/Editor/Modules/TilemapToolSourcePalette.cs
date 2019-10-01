using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using WeifenLuo.WinFormsUI.Docking;

using Duality.Resources;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps
{
	public partial class TilemapToolSourcePalette : DockContent
	{
		private PatternTileDrawSource paletteSource = new PatternTileDrawSource();
		private bool globalEventsSubscribed = false;
		private TilesetView.TileIndexDrawMode tileIndexDrawMode = TilesetView.TileIndexDrawMode.Never;


		private ContentRef<Tileset> SelectedTileset
		{
			get { return this.tilesetView.TargetTileset; }
			set
			{
				// Apply the selection to the palette's controls
				this.tilesetView.TargetTileset = value;
				this.labelTileset.Text = (value != null) ? 
					string.Format(TilemapsRes.TilePalette_SelectedTileset, value.Name) : 
					TilemapsRes.TilePalette_NoTilesetSelected;
			}
		}


		public TilemapToolSourcePalette()
		{
			this.InitializeComponent();
			this.mainToolStrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
			this.ApplyTileIndexDrawMode();
		}

		internal void SaveUserData(XElement node)
		{
			node.SetElementValue("DarkBackground", this.buttonBrightness.Checked);
			node.SetElementValue("DisplayTileIndices", this.tileIndexDrawMode);
		}
		internal void LoadUserData(XElement node)
		{
			bool tryParseBool;
			TilesetView.TileIndexDrawMode tryParseTileIndices;

			if (node.GetElementValue("DarkBackground", out tryParseBool)) this.buttonBrightness.Checked = tryParseBool;
			if (node.GetElementValue("DisplayTileIndices", out tryParseTileIndices)) this.tileIndexDrawMode = tryParseTileIndices;

			this.ApplyBrightness();
			this.ApplyTileIndexDrawMode();
		}

		private void SelectTilesetFromCurrentScene()
		{
			// Search the newly entered Scene for available tilesets
			IEnumerable<ContentRef<Tileset>> tilesets = TilemapsEditorSelectionParser.GetTilesetsInScene(Scene.Current);

			// If the currently selected tileset is not available, select one of the available ones.
			if (this.SelectedTileset == null || !tilesets.Contains(this.SelectedTileset))
			{
				ContentRef<Tileset> availableTileset = tilesets.FirstOrDefault();

				if (availableTileset == null)
					availableTileset = TilemapsEditorSelectionParser.QuerySelectedTileset();

				this.SelectedTileset = availableTileset;
			}
		}

		private void ApplySelectedTileset()
		{
			// Query the selected tileset...
			ContentRef<Tileset> lastTileset = this.SelectedTileset;
			ContentRef<Tileset> tileset = TilemapsEditorSelectionParser.QuerySelectedTileset();

			// ...or just use the last one (for editing continuity reasons) if none is selected.
			if (tileset == null && TilemapsEditorSelectionParser.SceneContainsTileset(Scene.Current, lastTileset))
				tileset = lastTileset;

			// If the selected tileset is no longer available, deselect it.
			if (!tileset.IsAvailable)
				tileset = null;

			this.SelectedTileset = tileset;
		}
		private void ApplyBrightness()
		{
			bool darkMode = this.buttonBrightness.Checked;
			this.tilesetView.BackColor = darkMode ? Color.FromArgb(64, 64, 64) : Color.FromArgb(192, 192, 192);
			this.tilesetView.ForeColor = darkMode ? Color.FromArgb(255, 255, 255) : Color.FromArgb(0, 0, 0);
		}
		private void ApplyTileIndexDrawMode()
		{
			this.tilesetView.DrawTileIndices = this.tileIndexDrawMode;
			switch (this.tileIndexDrawMode)
			{
				case TilesetView.TileIndexDrawMode.Never:
					this.buttonDrawTileIndices.Image = TilemapsResCache.IconHideIndices;
					break;
				case TilesetView.TileIndexDrawMode.Hovering:
					this.buttonDrawTileIndices.Image = TilemapsResCache.IconRevealIndices;
					break;
				case TilesetView.TileIndexDrawMode.Always:
					this.buttonDrawTileIndices.Image = TilemapsResCache.IconShowIndices;
					break;
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			// Since this is a module that will only be hidden and made visible,
			// rather than being disposed and recreated, we will have to listen
			// to visibility events as well as OnShown / OnClosed.
			if (this.IsHidden)
				this.OnBecameInvisible();
			else if (!this.IsHidden)
				this.OnBecameVisible();
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.OnBecameVisible();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			this.OnBecameInvisible();
		}
		
		private void OnBecameVisible()
		{
			if (!this.globalEventsSubscribed)
			{
				this.globalEventsSubscribed = true;

				DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
				DualityEditorApp.SelectionChanged      += this.DualityEditorApp_SelectionChanged;
				Resource.ResourceDisposing             += this.Resource_ResourceDisposing;
				Scene.Entered                          += this.Scene_Entered;
				TilemapsEditorPlugin.Instance.TileDrawingSourceChanged += 
					this.TilemapsEditorPlugin_TileDrawingSourceChanged;
			}
				
			// Apply editor-global tileset selection
			this.ApplySelectedTileset();
			// If none is selected, fall back to a tileset from the current Scene
			this.SelectTilesetFromCurrentScene();
		}
		private void OnBecameInvisible()
		{
			if (this.globalEventsSubscribed)
			{
				this.globalEventsSubscribed = false;

				DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
				DualityEditorApp.SelectionChanged      -= this.DualityEditorApp_SelectionChanged;
				Resource.ResourceDisposing             -= this.Resource_ResourceDisposing;
				Scene.Entered                          -= this.Scene_Entered;
				TilemapsEditorPlugin.Instance.TileDrawingSourceChanged -= 
					this.TilemapsEditorPlugin_TileDrawingSourceChanged;
			}
		}

		private void buttonBrightness_CheckedChanged(object sender, EventArgs e)
		{
			this.ApplyBrightness();
		}
		private void buttonDrawTileIndices_Click(object sender, EventArgs e)
		{
			this.tileIndexDrawMode = (TilesetView.TileIndexDrawMode)(((int)this.tileIndexDrawMode + 1) % 3);
			this.ApplyTileIndexDrawMode();
		}
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			// If the user changed the assigned Tileset of a Tilemap present in the current Scene,
			// we'll need to update the implicit Tileset selection for the palette.
			if (e.Objects.ComponentCount > 0 &&
				e.HasProperty(TilemapsReflectionInfo.Property_Tilemap_Tileset) &&
				e.Objects.Components.OfType<Tilemap>().Any())
			{
				this.ApplySelectedTileset();
			}
			// If the Scene itself has changed and we didn't have a selected Tileset, try to
			// select one from the Scene again.
			if (e.HasObject(Scene.Current) && this.SelectedTileset == null)
			{
				this.SelectTilesetFromCurrentScene();
			}
		}
		private void DualityEditorApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.SameObjects) return;
			this.ApplySelectedTileset();
		}
		private void Resource_ResourceDisposing(object sender, ResourceEventArgs e)
		{
			if (!e.IsResource) return;

			// Deselect the current tileset, if it's being disposed.
			// This is required since we don't always react to Deselect
			// events, which the editor provides on Resource disposal.
			if (this.SelectedTileset == e.Content.As<Tileset>())
			{
				this.SelectedTileset = null;
			}
		}
		private void Scene_Entered(object sender, EventArgs e)
		{
			this.SelectTilesetFromCurrentScene();
		}
		private void tilesetView_SelectedAreaEditingFinished(object sender, EventArgs e)
		{
			// Early-out, if nothing is selected
			if (this.tilesetView.SelectedArea.IsEmpty)
			{
				// When clearing the selection, remove it as tile drawing source, if it was set before
				if (TilemapsEditorPlugin.Instance.TileDrawingSource == this.paletteSource)
					TilemapsEditorPlugin.Instance.TileDrawingSource = TilemapsEditorPlugin.EmptyTileDrawingSource;
				return;
			}

			// Retrieve selected tile data
			Tileset tileset = this.SelectedTileset.Res;
			IReadOnlyGrid<Tile> selectedTiles = this.tilesetView.SelectedTiles;
			Grid<bool> shape = new Grid<bool>(selectedTiles.Width, selectedTiles.Height);
			Grid<Tile> pattern = new Grid<Tile>(selectedTiles.Width, selectedTiles.Height);
			for (int y = 0; y < selectedTiles.Height; y++)
			{
				for (int x = 0; x < selectedTiles.Width; x++)
				{
					Tile tile = selectedTiles[x, y];

					// For standard autotile parts, only paint the autotiles base index
					// and ignore which specific part was selected. 
					//
					// Note that this doesn't ensure completely normalized autotile base indices
					// across the tilemap, only normalized new indices being painted. A full
					// normalization guarantee would be possible by changing Tile.ResolveIndex, 
					// but since it isn't actually necessary and potentially destructive, we'll
					// just make sure "most indices are generally" normalized.
					int autoTileIndex = tileset.TileData[tile.BaseIndex].AutoTileLayer - 1;
					if (autoTileIndex >= 0)
					{
						TilesetAutoTileInfo autoTile = tileset.AutoTileData[autoTileIndex];
						TileConnection connectivity = autoTile.TileInfo[tile.BaseIndex].Neighbours;
						bool isDefaultTile = autoTile.StateToTile[(int)connectivity] == tile.BaseIndex;
						if (isDefaultTile)
							tile.BaseIndex = autoTile.BaseTileIndex;
					}

					shape[x, y] = true;
					pattern[x, y] = tile;
				}
			}
			this.paletteSource.SetData(shape, pattern);
			
			// Apply the selected tiles to the palettes source for tile drawing
			TilemapsEditorPlugin.Instance.TileDrawingSource = this.paletteSource;
		}
		private void TilemapsEditorPlugin_TileDrawingSourceChanged(object sender, EventArgs e)
		{
			// If the tile drawing source has been set to something other than this palette,
			// deselect the local pattern, so it doesn't look like it's still active.
			if (TilemapsEditorPlugin.Instance.TileDrawingSource != this.paletteSource)
			{
				this.tilesetView.SelectedArea = Rectangle.Empty;
			}
		}
	}
}
