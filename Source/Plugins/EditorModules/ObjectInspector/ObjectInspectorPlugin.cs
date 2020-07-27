using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.Plugins.ObjectInspector.Properties;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ItemModels;


namespace Duality.Editor.Plugins.ObjectInspector
{
	public class ObjectInspectorPlugin : EditorPlugin
	{
		private	static	ObjectInspectorPlugin	instance	= null;
		internal static ObjectInspectorPlugin Instance
		{
			get { return instance; }
		}


		private	List<ObjectInspector>	objViews		= new List<ObjectInspector>();
		private	bool					isLoading		= false;
		private ObjectInspectorSettings userSettings;

		public override string Id
		{
			get { return "ObjectInspector"; }
		}
		public IEnumerable<ObjectInspector>	ObjViews
		{
			get { return this.objViews; }
		}


		public ObjectInspectorPlugin()
		{
			instance = this;
		}
		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(ObjectInspector))
				result = this.RequestObjView();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void SaveUserData(PluginSettings pluginSettings)
		{
			foreach (ObjectInspector objectInspector in this.objViews)
			{
				objectInspector.SaveGridExpandState();
			}
			pluginSettings.Set(this.userSettings);
		}

		protected override void LoadUserData(PluginSettings pluginSettings)
		{
			this.isLoading = true;
			this.userSettings = pluginSettings.Get<ObjectInspectorSettings>();
			foreach (ObjectInspectorState inspectorState in this.userSettings.ObjectInspectors)
			{
				if (inspectorState.Id < 0 || inspectorState.Id >= this.objViews.Count) continue;
				this.objViews[inspectorState.Id].UserSettings = inspectorState;
				this.objViews[inspectorState.Id].ApplyUserSettings();
			}
			this.isLoading = false;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);
			
			// Request menu
			MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_View);
			viewItem.AddItem(new MenuModelItem
			{
				Name = ObjectInspectorRes.MenuItemName_ObjView,
				Icon = ObjectInspectorResCache.IconObjView.ToBitmap(),
				ActionHandler = this.menuItemObjView_Click
			});
		}
		
		public ObjectInspector RequestObjView(bool dontShow = false)
		{
			ObjectInspector objView = new ObjectInspector(this.objViews.Count);
			this.objViews.Add(objView);
			objView.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.objViews.Remove(sender as ObjectInspector); };

			if (!this.isLoading && !dontShow)
			{
				objView.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (objView.Pane != null)
				{
					objView.Pane.Activate();
					objView.Focus();
				}
			}
			return objView;
		}

		private void menuItemObjView_Click(object sender, EventArgs e)
		{
			ObjectInspector objView = this.RequestObjView();
		}
	}
}
