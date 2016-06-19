using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ItemModels;

using Duality;
using Duality.Resources;
using Duality.Plugins.Tilemaps;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps
{
	public class TilemapsEditorPlugin : EditorPlugin
	{
		private static readonly string          ElementNameTilePalette   = "TilePalette";
		private static readonly string          ElementNameTilesetEditor = "TilesetEditor";
		public  static readonly ITileDrawSource EmptyTileDrawingSource   = new DummyTileDrawSource();
		public  static readonly string          ActionTilemapEditor      = "TilemapEditor";

		private static TilemapsEditorPlugin instance = null;
		public static TilemapsEditorPlugin Instance
		{
			get { return instance; }
		}


		private bool                     isLoading                = false;
		private TilesetEditor            tilesetEditor            = null;
		private TilemapToolSourcePalette tilePalette              = null;
		private int                      pendingLocalTilePalettes = 0;
		private XElement                 tilePaletteSettings      = null;
		private XElement                 tilesetEditorSettings    = null;
		private ITileDrawSource          tileDrawingSource        = EmptyTileDrawingSource;

		/// <summary>
		/// An event that is fired when the <see cref="TileDrawingSource"/> is assigned a new value.
		/// </summary>
		public event EventHandler TileDrawingSourceChanged = null;
		

		public override string Id
		{
			get { return "Tilemaps"; }
		}
		/// <summary>
		/// [GET / SET] The data source that is used for retrieving tile patterns while using a tile drawing tool.
		/// Can be thought of as the "tile brush" that is currently used in user editing operations.
		/// </summary>
		public ITileDrawSource TileDrawingSource
		{
			get { return this.tileDrawingSource; }
			set
			{
				this.tileDrawingSource = value ?? EmptyTileDrawingSource;
				if (this.TileDrawingSourceChanged != null)
					this.TileDrawingSourceChanged(this, EventArgs.Empty);
			}
		}

		
		public TilemapsEditorPlugin()
		{
			instance = this;
		}

		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(TilemapToolSourcePalette))
				result = this.RequestTilePalette();
			else if (dockContentType == typeof(TilesetEditor))
				result = this.RequestTilesetEditor();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menus
			MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_View);
			viewItem.AddItem(new MenuModelItem
			{
				Name = TilemapsRes.MenuItemName_TilePalette,
				Icon = TilemapsResCache.IconTilePalette,
				ActionHandler = this.menuItemTilePalette_Click
			});
			viewItem.AddItem(new MenuModelItem
			{
				Name = TilemapsRes.MenuItemName_TilesetEditor,
				Icon = TilemapsResCache.IconTilesetEditor,
				ActionHandler = this.menuItemTilesetEditor_Click
			});

			// Register events
			FileEventManager.ResourceModified += this.FileEventManager_ResourceModified;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
		}
		protected override void SaveUserData(XElement node)
		{
			// Save editor settings to local cache node
			if (this.tilePalette != null)
			{
				this.tilePaletteSettings = new XElement(ElementNameTilePalette);
				this.tilePalette.SaveUserData(this.tilePaletteSettings);
			}
			if (this.tilesetEditor != null)
			{
				this.tilesetEditorSettings = new XElement(ElementNameTilesetEditor);
				this.tilesetEditor.SaveUserData(this.tilesetEditorSettings);
			}

			// Save settings from the local cache node persistently.
			if (this.tilePaletteSettings != null && !this.tilePaletteSettings.IsEmpty)
				node.Add(new XElement(this.tilePaletteSettings));
			if (this.tilesetEditorSettings != null && !this.tilesetEditorSettings.IsEmpty)
				node.Add(new XElement(this.tilesetEditorSettings));
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;

			// Retrieve settings from persistent editor data and put them into the local cache node
			foreach (XElement tilePaletteElem in node.Elements(ElementNameTilePalette))
			{
				int i = tilePaletteElem.GetAttributeValue("id", 0);
				if (i < 0 || i >= 1) continue;

				this.tilePaletteSettings = new XElement(tilePaletteElem);
				break;
			}
			foreach (XElement tilesetEditorElem in node.Elements(ElementNameTilesetEditor))
			{
				int i = tilesetEditorElem.GetAttributeValue("id", 0);
				if (i < 0 || i >= 1) continue;

				this.tilesetEditorSettings = new XElement(tilesetEditorElem);
				break;
			}

			// If we have an active matching editors, apply the settings directly
			if (this.tilePalette != null && this.tilePaletteSettings != null)
				this.tilePalette.LoadUserData(this.tilePaletteSettings);
			if (this.tilesetEditor != null && this.tilesetEditorSettings != null)
				this.tilesetEditor.LoadUserData(this.tilesetEditorSettings);

			this.isLoading = false;
		}

		/// <summary>
		/// Informs the system that a <see cref="TilemapToolSourcePalette"/> is required and creates one, if none is present yet.
		/// </summary>
		/// <returns></returns>
		public TilemapToolSourcePalette PushTilePalette()
		{
			this.pendingLocalTilePalettes++;
			if (this.pendingLocalTilePalettes == 1)
			{
				this.RequestTilePalette();
			}
			return this.tilePalette;
		}
		/// <summary>
		/// Informs the system that a <see cref="TilemapToolSourcePalette"/> is no longer required and closes it, if no
		/// other claims are pending.
		/// </summary>
		public void PopTilePalette()
		{
			if (this.pendingLocalTilePalettes == 0)
				return;
			else if (this.pendingLocalTilePalettes == 1 && this.tilePalette != null)
				this.tilePalette.Hide();
			else
				this.pendingLocalTilePalettes--;
		}
		
		public TilesetEditor RequestTilesetEditor()
		{
			// Create a new tileset editor, if no is available right now
			if (this.tilesetEditor == null || this.tilesetEditor.IsDisposed)
			{
				this.tilesetEditor = new TilesetEditor();
				this.tilesetEditor.FormClosed += this.tilesetEditor_FormClosed;
			
				// If there are cached settings available, apply them to the new editor
				if (this.tilePaletteSettings != null)
					this.tilesetEditor.LoadUserData(this.tilePaletteSettings);
			}

			// If we're not creating it as part of the loading procedure, add it to the main docking layout directly
			if (!this.isLoading)
			{
				this.tilesetEditor.Show(DualityEditorApp.MainForm.MainDockPanel);
			}

			return this.tilesetEditor;
		}
		private TilemapToolSourcePalette RequestTilePalette()
		{
			// Create a new tile palette, if none is available right now
			if (this.tilePalette == null || this.tilePalette.IsDisposed)
			{
				this.tilePalette = new TilemapToolSourcePalette();
				this.tilePalette.VisibleChanged += this.tilePalette_VisibleChanged;
				this.tilePalette.HideOnClose = true;
			
				// If there are cached settings available, apply them to the new palette
				if (this.tilePaletteSettings != null)
					this.tilePalette.LoadUserData(this.tilePaletteSettings);
			}

			// If we're not creating it as part of the loading procedure, add it to the main docking layout directly
			if (!this.isLoading)
			{
				this.tilePalette.Show(DualityEditorApp.MainForm.MainDockPanel);
			}

			return this.tilePalette;
		}
		
		private void tilesetEditor_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.tilesetEditorSettings = new XElement(ElementNameTilesetEditor);
			this.tilesetEditor.SaveUserData(this.tilesetEditorSettings);

			this.tilesetEditor.FormClosed -= this.tilesetEditor_FormClosed;
			this.tilesetEditor.Dispose();
			this.tilesetEditor = null;
		}
		private void tilePalette_VisibleChanged(object sender, EventArgs e)
		{
			if (this.tilePalette.IsHidden)
			{
				this.pendingLocalTilePalettes--;
			}
		}
		private void menuItemTilesetEditor_Click(object sender, EventArgs e)
		{
			TilesetEditor editor = this.RequestTilesetEditor();
			if (editor.Pane != null)
			{
				editor.Pane.Activate();
				editor.Focus();
			}
		}
		private void menuItemTilePalette_Click(object sender, EventArgs e)
		{
			TilemapToolSourcePalette palette = this.PushTilePalette();
			if (palette.Pane != null)
			{
				palette.Pane.Activate();
				palette.Focus();
			}
		}
		
		private void FileEventManager_ResourceModified(object sender, ResourceEventArgs e)
		{
			if (e.IsResource) this.OnResourceModified(e.Content);
		}
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.Objects.ResourceCount > 0)
			{
				foreach (var r in e.Objects.Resources)
					this.OnResourceModified(r);
			}
		}
		private void OnResourceModified(ContentRef<Resource> resRef)
		{
			List<object> changedObj = null;

			// If a pixmap has been modified, rebuild the tilesets that are based on it.
			if (resRef.Is<Pixmap>())
			{
				ContentRef<Pixmap> pixRef = resRef.As<Pixmap>();
				foreach (ContentRef<Tileset> tilesetRef in ContentProvider.GetLoadedContent<Tileset>())
				{
					Tileset tileset = tilesetRef.Res;

					// Early-out, if the tileset is unavailable, or we didn't compile it yet anyway
					if (tileset == null) continue;
					if (!tileset.Compiled) continue;

					// Determine whether this tileset uses the modified pixmap
					bool usesModifiedPixmap = false;
					foreach (TilesetRenderInput input in tileset.RenderConfig)
					{
						if (input.SourceData == pixRef)
						{
							usesModifiedPixmap = true;
							break;
						}
					}
					if (!usesModifiedPixmap) continue;

					// Recompile the tileset
					tileset.Compile();

					if (changedObj == null) changedObj = new List<object>();
					changedObj.Add(tilesetRef.Res);
				}
			}
			// If a Tileset has been modified, we'll need to give local Components a chance to update.
			else if (resRef.Is<Tileset>())
			{
				ContentRef<Tileset> tilesetRef = resRef.As<Tileset>();
				Tileset tileset = tilesetRef.Res;

				// Since we're able to edit tilesets without applying changes yet,
				// we'll check whether there are new compiled changes. Don't update
				// stuff unnecessarily if the changes aren't compiled yet anyway.
				bool appliedTilesetChanges = 
					tileset != null && 
					tileset.Compiled && 
					!tileset.HasChangedSinceCompile;
				
				if (appliedTilesetChanges)
				{
					foreach (Tilemap tilemap in Scene.Current.FindComponents<Tilemap>())
					{
						// Early-out for unaffected tilemaps
						if (tilemap.Tileset != tilesetRef) continue;

						// Notify every Component that is interested about changes using
						// a trick: Since they will almost certainly be subscribed to
						// tilemap changes anyway, pretend to change the entire tilemap.
						tilemap.BeginUpdateTiles();
						tilemap.EndUpdateTiles();
					}
				}
			}

			// Notify a change that isn't critical regarding persistence (don't flag stuff unsaved)
			if (changedObj != null)
				DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(changedObj as IEnumerable<object>), false);
		}
	}
}
