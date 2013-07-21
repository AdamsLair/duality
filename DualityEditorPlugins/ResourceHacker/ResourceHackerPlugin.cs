using System;
using System.Windows.Forms;
using System.IO;
using Duality.Serialization.MetaFormat;

using DualityEditor;
using DualityEditor.Forms;
using DualityEditor.EditorRes;
using DualityEditor.CorePluginInterface;

using WeifenLuo.WinFormsUI.Docking;

using ResourceHacker.PluginRes;

namespace ResourceHacker
{
	public class ResourceHackerPlugin : EditorPlugin
	{
		private	static	ResourceHackerPlugin	instance	= null;
		internal static ResourceHackerPlugin Instance
		{
			get { return instance; }
		}


		private	bool				isLoading			= false;
		private	ResourceHacker		resHacker			= null;
		private	ToolStripMenuItem	menuItemResHacker	= null;

		public override string Id
		{
			get { return "ResourceHacker"; }
		}


		public ResourceHackerPlugin()
		{
			instance = this;
		}
		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(ResourceHacker))
				result = this.RequestResourceHacker();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void LoadPlugin()
		{
			base.LoadPlugin();
			
			CorePluginRegistry.RegisterTypeImage(typeof(DataNode),				PluginRes.ResourceHackerResCache.IconPrimitive);
			CorePluginRegistry.RegisterTypeImage(typeof(ArrayNode),				PluginRes.ResourceHackerResCache.IconArray);
			CorePluginRegistry.RegisterTypeImage(typeof(StructNode),			PluginRes.ResourceHackerResCache.IconObject);
			CorePluginRegistry.RegisterTypeImage(typeof(ObjectRefNode),			PluginRes.ResourceHackerResCache.IconObjectRef);
			CorePluginRegistry.RegisterTypeImage(typeof(TypeDataLayoutNode),	PluginRes.ResourceHackerResCache.IconClass);
			CorePluginRegistry.RegisterTypeImage(typeof(MemberInfoNode),		PluginRes.ResourceHackerResCache.IconMethod);
			CorePluginRegistry.RegisterTypeImage(typeof(DelegateNode),			PluginRes.ResourceHackerResCache.IconDelegate);

			// Register PropertyEditor provider
			CorePluginRegistry.RegisterPropertyEditorProvider(new PropertyEditors.PropertyEditorProvider());
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menus
			this.menuItemResHacker = main.RequestMenu(Path.Combine(GeneralRes.MenuName_Tools, ResourceHackerRes.MenuItemName_ResourceHacker));

			// Configure menus
			this.menuItemResHacker.Image = ResourceHackerResCache.IconResourceHacker;
			this.menuItemResHacker.Click += new EventHandler(this.menuItemResHacker_Click);
		}

		public ResourceHacker RequestResourceHacker()
		{
			if (this.resHacker == null || this.resHacker.IsDisposed)
			{
				this.resHacker = new ResourceHacker();
				this.resHacker.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.resHacker = null; };
			}

			if (!this.isLoading)
			{
				this.resHacker.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (this.resHacker.Pane != null)
				{
					this.resHacker.Pane.Activate();
					this.resHacker.Focus();
				}
			}

			return this.resHacker;
		}

		private void menuItemResHacker_Click(object sender, EventArgs e)
		{
			this.RequestResourceHacker();
		}
	}
}
