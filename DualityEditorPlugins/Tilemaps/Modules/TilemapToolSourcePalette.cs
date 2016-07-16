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
			IReadOnlyGrid<Tile> selectedTiles = this.tilesetView.SelectedTiles;
			Grid<bool> shape = new Grid<bool>(selectedTiles.Width, selectedTiles.Height);
			Grid<Tile> pattern = new Grid<Tile>(selectedTiles.Width, selectedTiles.Height);
			for (int y = 0; y < selectedTiles.Height; y++)
			{
				for (int x = 0; x < selectedTiles.Width; x++)
				{
					shape[x, y] = true;
					pattern[x, y] = selectedTiles[x, y];
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
