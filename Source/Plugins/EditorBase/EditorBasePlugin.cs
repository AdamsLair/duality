using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using AdamsLair.WinForms.ItemModels;

using Duality;
using Duality.IO;
using Duality.Resources;
using TextRenderer = Duality.Components.Renderers.TextRenderer;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.Base.Forms;
using Duality.Editor.Properties;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.Base.Properties;
using WeifenLuo.WinFormsUI.Docking;


namespace Duality.Editor.Plugins.Base
{
	public class EditorBasePlugin : EditorPlugin
	{
		private static readonly string ElementNamePixmapSlicer = "PixmapSlicer";

		private PixmapSlicerForm	slicingForm				= null;
		private XElement			pixmapSlicerSettings	= null;

		private bool isLoading = false;

		public override string Id
		{
			get { return "EditorBase"; }
		}

		public PixmapSlicerForm RequestPixmapSlicerForm()
		{
			// Create a new slicing form, if none are available right now
			if (this.slicingForm == null || this.slicingForm.IsDisposed)
			{
				this.slicingForm = new PixmapSlicerForm();
				this.slicingForm.FormClosed += this.slicingForm_FormClosed;

				// If there are cached settings available, apply them to the new editor
				if (this.pixmapSlicerSettings != null)
					this.slicingForm.LoadUserData(this.pixmapSlicerSettings);

				if (!this.isLoading)
				{
					this.slicingForm.DockPanel = DualityEditorApp.MainForm.MainDockPanel;
				}
			}

			// If we're not creating it as part of the loading procedure,
			// add it to the main docking layout directly
			if (!this.isLoading)
			{
				this.slicingForm.Show(DualityEditorApp.MainForm.MainDockPanel);
			}

			return this.slicingForm;
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

			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
		}
		protected override void SaveUserData(XElement node)
		{
			if (this.slicingForm != null)
			{
				this.pixmapSlicerSettings = new XElement(ElementNamePixmapSlicer);
				this.slicingForm.SaveUserData(this.pixmapSlicerSettings);
			}

			if (this.slicingForm != null && !this.pixmapSlicerSettings.IsEmpty)
				node.Add(this.pixmapSlicerSettings);
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;
			foreach (XElement pixmapSlicerElem in node.Elements(ElementNamePixmapSlicer))
			{
				int i = pixmapSlicerElem.GetAttributeValue("id", 0);
				if (i < 0 || i >= 1) continue;

				this.pixmapSlicerSettings = new XElement(pixmapSlicerElem);
				break;
			}

			if (this.slicingForm != null && this.pixmapSlicerSettings != null)
				this.slicingForm.LoadUserData(this.pixmapSlicerSettings);

			this.isLoading = false;
		}
		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(PixmapSlicerForm))
				result = this.RequestPixmapSlicerForm();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		private void slicingForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.pixmapSlicerSettings = new XElement(ElementNamePixmapSlicer);
			this.slicingForm.SaveUserData(this.pixmapSlicerSettings);

			this.slicingForm.FormClosed -= this.slicingForm_FormClosed;
			this.slicingForm.Dispose();
			this.slicingForm = null;
		}
		private void menuItemAppData_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new [] { DualityApp.AppData }));
		}
		private void menuItemUserData_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new [] { DualityApp.UserData }));
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.Objects.ResourceCount > 0)
			{
				List<object> modifiedObjects = new List<object>();
				foreach (Resource resource in e.Objects.Resources)
				{
					this.PropagateDependentResourceChanges(resource, modifiedObjects);
				}

				// Notify about propagated changes, but flag them as non-persistent
				if (modifiedObjects.Count > 0)
				{
					DualityEditorApp.NotifyObjPropChanged(
						this, 
						new ObjectSelection(modifiedObjects), 
						false);
				}
			}
		}
		private void PropagateDependentResourceChanges(ContentRef<Resource> resRef, List<object> modifiedObjects)
		{
			// If a font has been modified, reload it and update all TextRenderers
			if (resRef.Is<Font>())
			{
				foreach (TextRenderer r in Scene.Current.AllObjects.GetComponents<TextRenderer>())
				{
					if (r.Text.Fonts.Contains(resRef.As<Font>()))
					{
						r.Text.ApplySource();
						modifiedObjects.Add(r);
					}
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
						// Note: Reloading texture data every time _any_ change happens to a Pixmap
						// is super wasteful. Find a way to identify relevant changes, and only reload
						// when they happen.
						// An example of an unnecessary reload is changing the Pixmap atlas, where updating
						// the atlas only would be sufficient.
						tex.Res.ReloadData();
						modifiedObjects.Add(tex.Res);
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
						modifiedObjects.Add(rt.Res);
					}
				}
			}
			// If its some kind of shader, update all associated techniques
			else if (resRef.Is<Shader>())
			{
				ContentRef<FragmentShader> fragRef = resRef.As<FragmentShader>();
				ContentRef<VertexShader> vertRef = resRef.As<VertexShader>();
				foreach (ContentRef<DrawTechnique> sp in ContentProvider.GetLoadedContent<DrawTechnique>())
				{
					if (!sp.IsAvailable) continue;
					if (sp.Res.Fragment == fragRef || sp.Res.Vertex == vertRef)
					{
						if (sp.Res.Compiled) sp.Res.Compile();
						modifiedObjects.Add(sp.Res);
					}
				}
			}
		}
	}
}
