﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using AdamsLair.WinForms.ItemModels;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Diagnostics;
using Duality.Components.Physics;
using Duality.Resources;
using Duality.Properties;
using TextRenderer = Duality.Components.Renderers.TextRenderer;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.Base.Properties;


namespace Duality.Editor.Plugins.Base
{
	public class EditorBasePlugin : EditorPlugin
	{
		public override string Id
		{
			get { return "EditorBase"; }
		}


		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menus
			MenuModelItem settingsItem = main.MainMenu.RequestItem(GeneralRes.MenuName_Settings);
			settingsItem.SortValue = MenuModelItem.SortValue_OverBottom;
			settingsItem.AddItems(new[]
			{
				new MenuModelItem
				{
					Name = EditorBaseRes.MenuItemName_AppData,
					ActionHandler = this.menuItemAppData_Click
				},
				new MenuModelItem
				{
					Name = EditorBaseRes.MenuItemName_UserData,
					ActionHandler = this.menuItemUserData_Click
				}
			});

			FileEventManager.ResourceModified += this.FileEventManager_ResourceChanged;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
		}
		
		private void menuItemAppData_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new [] { DualityApp.AppData }));
		}
		private void menuItemUserData_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new [] { DualityApp.UserData }));
		}

		private void FileEventManager_ResourceChanged(object sender, ResourceEventArgs e)
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

			// If a font has been modified, reload it and update all TextRenderers
			if (resRef.Is<Font>())
			{
				foreach (TextRenderer r in Scene.Current.AllObjects.GetComponents<TextRenderer>())
				{
					r.Text.ApplySource();

					if (changedObj == null) changedObj = new List<object>();
					changedObj.Add(r);
				}
			}
			// If its a Pixmap, reload all associated Textures
			else if (resRef.Is<Pixmap>())
			{
				ContentRef<Pixmap> pixRef = resRef.As<Pixmap>();
				foreach (ContentRef<Texture> tex in ContentProvider.GetLoadedContent<Texture>())
				{
					if (!tex.IsAvailable) continue;
					if (tex.Res.BasePixmap == pixRef)
					{
						tex.Res.ReloadData();

						if (changedObj == null) changedObj = new List<object>();
						changedObj.Add(tex.Res);
					}
				}
			}
			// If its a Texture, update all associated RenderTargets
			else if (resRef.Is<Texture>())
			{
				if (resRef.IsLoaded)
				{
					Texture tex = resRef.As<Texture>().Res;
					if (tex.NeedsReload)
						tex.ReloadData();
				}

				ContentRef<Texture> texRef = resRef.As<Texture>();
				foreach (ContentRef<RenderTarget> rt in ContentProvider.GetLoadedContent<RenderTarget>())
				{
					if (!rt.IsAvailable) continue;
					if (rt.Res.Targets.Contains(texRef))
					{
						rt.Res.SetupTarget();

						if (changedObj == null) changedObj = new List<object>();
						changedObj.Add(rt.Res);
					}
				}
			}
			// If its some kind of shader, update all associated ShaderPrograms
			else if (resRef.Is<AbstractShader>())
			{
				ContentRef<FragmentShader> fragRef = resRef.As<FragmentShader>();
				ContentRef<VertexShader> vertRef = resRef.As<VertexShader>();
				foreach (ContentRef<ShaderProgram> sp in ContentProvider.GetLoadedContent<ShaderProgram>())
				{
					if (!sp.IsAvailable) continue;
					if (sp.Res.Fragment == fragRef || sp.Res.Vertex == vertRef)
					{
						if (sp.Res.Compiled) sp.Res.Compile();

						if (changedObj == null) changedObj = new List<object>();
						changedObj.Add(sp.Res);
					}
				}
			}

			// Notify a change that isn't critical regarding persistence (don't flag stuff unsaved)
			if (changedObj != null)
				DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(changedObj as IEnumerable<object>), false);
		}
	}
}
