using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.UndoRedoActions;
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
		protected override void SaveUserData(XElement node)
		{
			for (int i = 0; i < this.objViews.Count; i++)
			{
				XElement objViewElem = new XElement("ObjInspector_" + i);
				node.Add(objViewElem);
				this.objViews[i].SaveUserData(objViewElem);
			}
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;
			for (int i = 0; i < this.objViews.Count; i++)
			{
				XElement objViewElem = node.Element("ObjInspector_" + i);
				if (objViewElem == null) continue;
				this.objViews[i].LoadUserData(objViewElem);
			}
			this.isLoading = false;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);
			
			// Request menu
			MenuModelItem viewItem = main.RequestMainMenu(GeneralRes.MenuName_View);
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
