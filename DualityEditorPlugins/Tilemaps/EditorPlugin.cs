using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ItemModels;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps
{
	public class TilemapsEditorPlugin : EditorPlugin
	{
		private	static TilemapsEditorPlugin instance = null;
		internal static TilemapsEditorPlugin Instance
		{
			get { return instance; }
		}


		private	bool                     isLoading                = false;
		private	TilemapToolSourcePalette tilePalette              = null;
		private int                      pendingLocalTilePalettes = 0;
		

		public override string Id
		{
			get { return "Tilemaps"; }
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
		}
		protected override void SaveUserData(XElement node)
		{
			if (this.tilePalette != null)
			{
				XElement tilePaletteElem = new XElement("TilePalette");
				this.tilePalette.SaveUserData(tilePaletteElem);
				if (!tilePaletteElem.IsEmpty)
					node.Add(tilePaletteElem);
			}
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;
			if (this.tilePalette != null)
			{
				foreach (XElement tilePaletteElem in node.Elements("TilePalette"))
				{
					int i = tilePaletteElem.GetAttributeValue("id", 0);
					if (i < 0 || i >= 1) continue;

					this.tilePalette.LoadUserData(tilePaletteElem);
				}
			}
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
				this.tilePalette.Close();
			else
				this.pendingLocalTilePalettes--;
		}

		private TilemapToolSourcePalette RequestTilePalette()
		{
			if (this.tilePalette == null || this.tilePalette.IsDisposed)
			{
				this.tilePalette = new TilemapToolSourcePalette();
				this.tilePalette.FormClosed += delegate(object sender, FormClosedEventArgs e)
				{
					this.tilePalette = null;
					if (e.CloseReason == CloseReason.UserClosing)
					{
						this.pendingLocalTilePalettes--;
					}
				};
			}

			if (!this.isLoading)
			{
				this.tilePalette.Show(DualityEditorApp.MainForm.MainDockPanel);
			}

			return this.tilePalette;
		}

		private void menuItemTilePalette_Click(object sender, EventArgs e)
		{
			TilemapToolSourcePalette palette = this.tilePalette ?? this.PushTilePalette();
			if (palette.Pane != null)
			{
				palette.Pane.Activate();
				palette.Focus();
			}
		}
	}
}
