using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using Duality.Editor.Controls.TreeModels.TypeHierarchy;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;


namespace Duality.Editor.Forms
{
	public partial class CreateObjectDialog : Form, IHelpProvider
	{
		private TypeBrowserTreeModel model = new TypeBrowserTreeModel();
		private Type selectedType = null;

		public Type BaseType
		{
			get { return this.model.BaseType; }
			set { this.model.BaseType = value; }
		}
		public Type SelectedType
		{
			get { return this.selectedType; }
		}
		public bool ShowNamespaces
		{
			get { return this.model.ShowNamespaces; }
			set { this.model.ShowNamespaces = value; }
		}

		public CreateObjectDialog()
		{
			this.InitializeComponent();

			this.model.ShowNamespaces = false;
			this.model.Filter = (t => !t.IsGenericTypeDefinition);
			this.objectTypeView.Model = this.model;

			this.treeNodeName.DrawText += this.treeNodeName_DrawText;
		}

		private bool CanInstantiateType(Type type)
		{
			return !type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition;
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			TypeItem item = (this.objectTypeView.SelectedNode != null ? this.objectTypeView.SelectedNode.Tag as TypeItem : null);
			this.selectedType = item.TypeInfo;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.selectedType = null;
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void treeNodeName_DrawText(object sender, DrawTextEventArgs e)
		{
			TypeItem item = e.Node.Tag as TypeItem;
			if (item != null && this.CanInstantiateType(item.TypeInfo))
			{
				e.TextColor = this.objectTypeView.ForeColor;
			}
			else
			{
				e.TextColor = Color.FromArgb(128, this.objectTypeView.ForeColor);
			}
		}
		private void objectTypeView_SelectionChanged(object sender, EventArgs e)
		{
			TypeItem item = (this.objectTypeView.SelectedNode != null ? this.objectTypeView.SelectedNode.Tag as TypeItem : null);
			this.buttonOk.Enabled = (item != null ? this.CanInstantiateType(item.TypeInfo) : false);
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			Point globalPos = this.PointToScreen(localPos);
			Point treePos = this.objectTypeView.PointToClient(globalPos);
			TreeNodeAdv node = this.objectTypeView.GetNodeAt(treePos);
			if (node == null) return null;

			TypeItem typeItem = node.Tag as TypeItem;
			if (typeItem == null) return null;

			return HelpInfo.FromMember(typeItem.TypeInfo);
		}
	}
}
