using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using Duality.Resources;

namespace Duality.Editor.Forms
{
	public partial class SelectionDialog : Form
	{
		public class ReferenceNode : Node
		{
			public string Name { get; set; }
			public string Origin { get; set; }
			public IContentRef ResourceReference { get; private set; }
			public GameObject GameObjectReference { get; private set; }
			public Component ComponentReference { get; private set; }
			public Type TypeReference { get; private set; }

			public ReferenceNode(IContentRef resource)
			{
				this.Name = resource.Name;
				this.Origin = resource.FullName;
				this.Text = this.Name;

				this.ResourceReference = resource;
				this.Image = resource.ResType.GetEditorImage();
			}
			public ReferenceNode(GameObject gameObject)
			{
				this.Name = gameObject.Name;
				this.Origin = gameObject.FullName;
				this.Text = this.Name;

				this.GameObjectReference = gameObject;
				this.Image = typeof(GameObject).GetEditorImage();
			}
			public ReferenceNode(Component component)
			{
				this.Name = string.Format("{0} ({1})", 
					component.GameObj.Name, 
					component.GetType().GetTypeCSCodeName(true));
				this.Origin = component.GameObj.FullName;
				this.Text = this.Name;

				this.ComponentReference = component;
				this.Image = component.GetType().GetEditorImage();
			}
			public ReferenceNode(Type type)
			{
				this.Name = type.Name;
				this.Origin = type.GetTypeCSCodeName();
				this.Text = this.Name;

				this.TypeReference = type;
				this.Image = type.GetEditorImage();
			}
		}

		private Size oldItemListingSize = Size.Empty;

		public Color PathColor { get; set; }
		public Type FilteredType { get; set; }
		public bool SelectType { get; set; }

		public string ResourcePath { get; set; }

		public IContentRef ResourceReference { get; set; }
		public GameObject GameObjectReference { get; set; }
		public Component ComponentReference { get; set; }
		public Type TypeReference { get; set; }

		private TreeModel Model { get; set; }


		public SelectionDialog()
		{
			InitializeComponent();

			this.Model = new TreeModel();

			this.PathColor = Color.Gray;

			this.oldItemListingSize = this.itemListing.Size;
			this.itemListing.Resize += this.itemListing_Resize;
			this.itemListing.SelectionChanged += this.ResourceListingOnSelectedIndexChanged;
			this.itemListing.DoubleClick += this.ResourceListingOnDoubleClick;
			this.itemListing.NodeFilter += this.NodeFilter;

			this.txtFilterInput.KeyDown += this.TxtFilterInputOnKeyDown;

			this.nodeName.DrawText += this.NodeName_OnDrawText;
			this.nodeOrigin.DrawText += this.NodeOrigin_OnDrawText;

			this.columnName.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.columnOrigin.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.itemListing.ClearSelection();
			this.itemListing.Model = this.Model;

			this.itemListing.BeginUpdate();
			this.Model.Nodes.Clear();

			if (this.SelectType)
			{
				this.Text = "Select a Type";

				foreach (TypeInfo type in DualityApp.GetAvailDualityTypes(this.FilteredType).Where(t => !t.IsAbstract && !t.IsInterface))
				{
					this.Model.Nodes.Add(new ReferenceNode(type));
				}
			}
			else if (typeof(GameObject).IsAssignableFrom(this.FilteredType))
			{
				this.Text = "Select a GameObject";

				foreach (GameObject currentObject in Scene.Current.AllObjects)
				{
					this.Model.Nodes.Add(new ReferenceNode(currentObject));
				}
			}
			else if (typeof(Component).IsAssignableFrom(this.FilteredType))
			{
				this.Text = string.Format("Select a {0} Component", this.FilteredType.GetTypeCSCodeName());

				foreach (Component currentComponent in Scene.Current.FindComponents(this.FilteredType))
				{
					this.Model.Nodes.Add(new ReferenceNode(currentComponent));
				}
			}
			else
			{
				Type filteredType = typeof(Resource);

				if (this.FilteredType.IsGenericType)
				{
					filteredType = this.FilteredType.GetGenericArguments()[0];
					this.Text = string.Format("Select a {0} Resource", filteredType.GetTypeCSCodeName(true));
				}
				else
				{
					this.Text = "Select a Resource";
				}

				foreach (IContentRef contentRef in ContentProvider.GetAvailableContent(filteredType))
				{
					this.Model.Nodes.Add(new ReferenceNode(contentRef));
				}
			}

			foreach (var treeNodeAdv in this.itemListing.AllNodes)
			{
				ReferenceNode node = treeNodeAdv.Tag as ReferenceNode;

				treeNodeAdv.IsExpanded = true;

				if (node != null && node.Origin == this.ResourcePath)
				{
					this.itemListing.SelectedNode = treeNodeAdv;
				}
			}

			this.itemListing.EndUpdate();
			this.txtFilterInput.Focus();
		}
		
		private void TxtFilterInputOnKeyDown(object sender, KeyEventArgs keyEventArgs)
		{
			TreeNodeAdv targetNode = null;

			if (keyEventArgs.KeyCode == Keys.Down)
			{
				if (this.itemListing.SelectedNode == null)
					targetNode = this.itemListing.FirstVisibleNode();
				else
					targetNode = this.itemListing.SelectedNode.NextVisibleNode();
				keyEventArgs.Handled = true;
			}
			else if (keyEventArgs.KeyCode == Keys.Up)
			{
				if (this.itemListing.SelectedNode == null)
					targetNode = this.itemListing.LastVisibleNode();
				else
					targetNode = this.itemListing.SelectedNode.PreviousVisibleNode();
				keyEventArgs.Handled = true;
			}

			if (targetNode != null)
			{
				this.itemListing.SelectedNode = targetNode;
			}
		}
		private void ResourceListingOnDoubleClick(object sender, EventArgs eventArgs)
		{
			if (this.itemListing.SelectedNode == null)
			{
				this.ResourceReference = null;
				this.GameObjectReference = null;
				this.ComponentReference = null;
				this.TypeReference = null;

				return;
			}

			this.ResourceListingOnSelectedIndexChanged(sender, eventArgs);
			this.AcceptButton.PerformClick();
		}
		private void ResourceListingOnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			if (this.itemListing.SelectedNode == null)
			{
				this.ResourceReference = null;
				this.GameObjectReference = null;
				this.ComponentReference = null;
				this.TypeReference = null;

				return;
			}

			ReferenceNode node = this.itemListing.SelectedNode.Tag as ReferenceNode;

			if (node == null)
			{
				return;
			}

			this.ResourceReference = node.ResourceReference;
			this.GameObjectReference = node.GameObjectReference;
			this.ComponentReference = node.ComponentReference;
			this.TypeReference = node.TypeReference;
		}
		
		private void NodeOrigin_OnDrawText(object sender, DrawTextEventArgs drawTextEventArgs)
		{
			ReferenceNode node = drawTextEventArgs.Node.Tag as ReferenceNode;

			if (node == null)
			{
				return;
			}

			drawTextEventArgs.TextColor = this.PathColor;
		}
		private void NodeName_OnDrawText(object sender, DrawTextEventArgs drawTextEventArgs)
		{
			ReferenceNode node = drawTextEventArgs.Node.Tag as ReferenceNode;

			if (node == null)
			{
				return;
			}

			drawTextEventArgs.TextColor = this.itemListing.ForeColor;
		}
		private void treeColumn_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 212, 212, 212)), e.Bounds);
			e.Handled = true;
		}
		private void txtFilterInput_TextChanged(object sender, EventArgs e)
		{
			this.itemListing.UpdateNodeFilter();

			if (this.itemListing.Root.Children.Count > 0)
			{
				this.itemListing.SelectedNode = this.itemListing.FirstVisibleNode();
			}
		}
		private void itemListing_Resize(object sender, EventArgs e)
		{
			Size sizeChange = new Size(
				this.itemListing.Width - this.oldItemListingSize.Width,
				this.itemListing.Height - this.oldItemListingSize.Height);

			this.columnOrigin.Width += sizeChange.Width;

			this.oldItemListingSize = this.itemListing.Size;
		}
		private bool NodeFilter(TreeNodeAdv nodeAdv)
		{
			ReferenceNode node = nodeAdv.Tag as ReferenceNode;
			string filterValue = this.txtFilterInput.Text.ToLowerInvariant();

			return node != null &&
				(
					node.Name.ToLowerInvariant().Contains(filterValue) ||
					node.Origin.ToLowerInvariant().Contains(filterValue)
				);
		}
	}
}
