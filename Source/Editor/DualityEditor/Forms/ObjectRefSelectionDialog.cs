using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using Duality.Resources;

namespace Duality.Editor.Forms
{
	public partial class ObjectRefSelectionDialog : Form
	{
		public class ReferenceNode : Node
		{
			public string Name { get; set; }
			public string Path { get; set; }
			public IContentRef ResourceReference { get; set; }
			public GameObject GameObjectReference { get; set; }
			public Component ComponentReference { get; set; }

			public ReferenceNode(IContentRef resource) : base(resource.Name)
			{
				this.Name = resource.Name;
				this.Path = resource.FullName;

				this.ResourceReference = resource;
			}

			public ReferenceNode(GameObject gameObject) : base(gameObject.Name)
			{
				this.Name = gameObject.Name;
				this.Path = gameObject.FullName;

				this.GameObjectReference = gameObject;
			}

			public ReferenceNode(Component component) : base(component.GetType().GetTypeCSCodeName())
			{
				this.Name = component.GameObj.FullName;
				this.Path = component.GetType().GetTypeCSCodeName();

				this.ComponentReference = component;
			}
		}

		public Color PathColor { get; set; }
		public Type FilteredType { get; set; }

		public string ResourcePath { get; set; }

		public IContentRef ResourceReference { get; set; }
		public GameObject GameObjectReference { get; set; }
		public Component ComponentReference { get; set; }

		private TreeModel Model { get; set; }

		public ObjectRefSelectionDialog()
		{
			InitializeComponent();

			this.Model = new TreeModel();

			this.PathColor = Color.Gray;

			this.objectReferenceListing.SelectionChanged += this.ResourceListingOnSelectedIndexChanged;
			this.objectReferenceListing.DoubleClick += this.ResourceListingOnDoubleClick;

			this.txtFilterInput.KeyDown += this.TxtFilterInputOnKeyDown;

			this.nodeName.DrawText += this.NodeName_OnDrawText;
			this.nodePath.DrawText += this.NodePath_OnDrawText;
		}

		private void TxtFilterInputOnKeyDown(object sender, KeyEventArgs keyEventArgs)
		{
			TreeNodeAdv tmp = null;

			if (keyEventArgs.KeyCode == Keys.Down)
			{
				tmp = this.objectReferenceListing.SelectedNode.NextNode;
			}
			else if (keyEventArgs.KeyCode == Keys.Up)
			{
				tmp = this.objectReferenceListing.SelectedNode.PreviousNode;
			}

			if (tmp != null)
			{
				this.objectReferenceListing.SelectedNode = tmp;
			}
		}

		private void NodePath_OnDrawText(object sender, DrawTextEventArgs drawTextEventArgs)
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

			drawTextEventArgs.TextColor = this.objectReferenceListing.ForeColor;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.objectReferenceListing.ClearSelection();
			this.objectReferenceListing.Model = this.Model;

			this.objectReferenceListing.BeginUpdate();
			this.Model.Nodes.Clear();

			if (this.FilteredType.IsSubclassOf(typeof(GameObject)) || this.FilteredType == typeof(GameObject))
			{
				this.Text = "Select a GameObject";

				foreach (GameObject currentObject in Scene.Current.AllObjects)
				{
					ReferenceNode tmpNode = new ReferenceNode(currentObject);

					this.Model.Nodes.Add(tmpNode);
				}
			}
			else if (this.FilteredType.IsSubclassOf(typeof(Component)) || this.FilteredType == typeof(Component))
			{
				this.Text = string.Format("Select a {0} Component", this.FilteredType.GetTypeCSCodeName());

				foreach (Component currentComponent in Scene.Current.FindComponents(this.FilteredType))
				{
					ReferenceNode tmpNode = new ReferenceNode(currentComponent);

					this.Model.Nodes.Add(tmpNode);
				}
			}
			else
			{
				Type tmpFilteredType = typeof(Resource);
				if (this.FilteredType.IsGenericType)
				{
					tmpFilteredType = this.FilteredType.GetGenericArguments()[0];
					this.Text = string.Format("Select a {0} Resource", tmpFilteredType.GetTypeCSCodeName(true));
				}
				else
				{
					this.Text = "Select a Resource";
				}

				foreach (IContentRef contentRef in ContentProvider.GetAvailableContent(tmpFilteredType))
				{
					ReferenceNode tmpNode = new ReferenceNode(contentRef);

					this.Model.Nodes.Add(tmpNode);
				}
			}

			this.objectReferenceListing.NodeFilter += this.NodeFilter;

			foreach (var treeNodeAdv in this.objectReferenceListing.AllNodes)
			{
				ReferenceNode tmpNode = treeNodeAdv.Tag as ReferenceNode;

				treeNodeAdv.IsExpanded = true;

				if (tmpNode != null && tmpNode.Path == this.ResourcePath)
				{
					this.objectReferenceListing.SelectedNode = treeNodeAdv;
				}
			}

			this.objectReferenceListing.EndUpdate();
			this.txtFilterInput.Focus();
		}

		private void ResourceListingOnDoubleClick(object sender, EventArgs eventArgs)
		{
			if (this.objectReferenceListing.SelectedNode == null)
			{
				this.ResourceReference = null;
				this.GameObjectReference = null;
				this.ComponentReference = null;

				return;
			}

			this.ResourceListingOnSelectedIndexChanged(sender, eventArgs);
			this.AcceptButton.PerformClick();
		}

		private void ResourceListingOnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			if (this.objectReferenceListing.SelectedNode == null)
			{
				this.ResourceReference = null;
				this.GameObjectReference = null;
				this.ComponentReference = null;

				return;
			}

			ReferenceNode node = this.objectReferenceListing.SelectedNode.Tag as ReferenceNode;

			if (node == null)
			{
				return;
			}

			this.ResourceReference = node.ResourceReference;
			this.GameObjectReference = node.GameObjectReference;
			this.ComponentReference = node.ComponentReference;
		}

		private void txtFilterInput_TextChanged(object sender, EventArgs e)
		{
			this.objectReferenceListing.UpdateNodeFilter();

			this.objectReferenceListing.SelectedNode = this.objectReferenceListing.AllNodes
				.Where((node, index) => { return node.IsHidden == false; }).First();
		}

		private bool NodeFilter(TreeNodeAdv nodeAdv)
		{
			ReferenceNode tmpNode = nodeAdv.Tag as ReferenceNode;
			string tmpFilterValue = this.txtFilterInput.Text.ToLowerInvariant();

			return tmpNode != null &&
					(
						tmpNode.Name.ToLowerInvariant().Contains(tmpFilterValue) ||
						tmpNode.Path.ToLowerInvariant().Contains(tmpFilterValue)
					);
		}
	}
}
