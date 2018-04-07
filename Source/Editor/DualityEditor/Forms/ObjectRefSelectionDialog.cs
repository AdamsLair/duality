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
			public IContentRef ResourceReference { get; private set; }
			public GameObject GameObjectReference { get; private set; }
			public Component ComponentReference { get; private set; }

			public ReferenceNode(IContentRef resource)
			{
				this.Name = resource.Name;
				this.Path = resource.FullName;
				this.Text = this.Name;

				this.ResourceReference = resource;
				this.Image = resource.ResType.GetEditorImage();
			}
			public ReferenceNode(GameObject gameObject)
			{
				this.Name = gameObject.Name;
				this.Path = gameObject.FullName;
				this.Text = this.Name;

				this.GameObjectReference = gameObject;
				this.Image = typeof(GameObject).GetEditorImage();
			}
			public ReferenceNode(Component component)
			{
				this.Name = string.Format("{0} ({1})", 
					component.GameObj.Name, 
					component.GetType().GetTypeCSCodeName(true));
				this.Path = component.GameObj.FullName;
				this.Text = this.Name;

				this.ComponentReference = component;
				this.Image = component.GetType().GetEditorImage();
			}
		}

		private Size oldObjectReferenceListingSize = Size.Empty;

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

			this.oldObjectReferenceListingSize = this.objectReferenceListing.Size;
			this.objectReferenceListing.Resize += this.objectReferenceListing_Resize;
			this.objectReferenceListing.SelectionChanged += this.ResourceListingOnSelectedIndexChanged;
			this.objectReferenceListing.DoubleClick += this.ResourceListingOnDoubleClick;
			this.objectReferenceListing.NodeFilter += this.NodeFilter;

			this.txtFilterInput.KeyDown += this.TxtFilterInputOnKeyDown;

			this.nodeName.DrawText += this.NodeName_OnDrawText;
			this.nodePath.DrawText += this.NodePath_OnDrawText;

			this.columnName.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.columnPath.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.objectReferenceListing.ClearSelection();
			this.objectReferenceListing.Model = this.Model;

			this.objectReferenceListing.BeginUpdate();
			this.Model.Nodes.Clear();

			if (typeof(GameObject).IsAssignableFrom(this.FilteredType))
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

			foreach (var treeNodeAdv in this.objectReferenceListing.AllNodes)
			{
				ReferenceNode node = treeNodeAdv.Tag as ReferenceNode;

				treeNodeAdv.IsExpanded = true;

				if (node != null && node.Path == this.ResourcePath)
				{
					this.objectReferenceListing.SelectedNode = treeNodeAdv;
				}
			}

			this.objectReferenceListing.EndUpdate();
			this.txtFilterInput.Focus();
		}
		
		private void TxtFilterInputOnKeyDown(object sender, KeyEventArgs keyEventArgs)
		{
			TreeNodeAdv targetNode = null;

			if (keyEventArgs.KeyCode == Keys.Down)
			{
				if (this.objectReferenceListing.SelectedNode == null)
					targetNode = this.objectReferenceListing.FirstVisibleNode();
				else
					targetNode = this.objectReferenceListing.SelectedNode.NextVisibleNode();
				keyEventArgs.Handled = true;
			}
			else if (keyEventArgs.KeyCode == Keys.Up)
			{
				if (this.objectReferenceListing.SelectedNode == null)
					targetNode = this.objectReferenceListing.LastVisibleNode();
				else
					targetNode = this.objectReferenceListing.SelectedNode.PreviousVisibleNode();
				keyEventArgs.Handled = true;
			}

			if (targetNode != null)
			{
				this.objectReferenceListing.SelectedNode = targetNode;
			}
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
		private void treeColumn_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 212, 212, 212)), e.Bounds);
			e.Handled = true;
		}
		private void txtFilterInput_TextChanged(object sender, EventArgs e)
		{
			this.objectReferenceListing.UpdateNodeFilter();

			if (this.objectReferenceListing.Root.Children.Count > 0)
			{
				this.objectReferenceListing.SelectedNode = this.objectReferenceListing.FirstVisibleNode();
			}
		}
		private void objectReferenceListing_Resize(object sender, EventArgs e)
		{
			Size sizeChange = new Size(
				this.objectReferenceListing.Width - this.oldObjectReferenceListingSize.Width,
				this.objectReferenceListing.Height - this.oldObjectReferenceListingSize.Height);

			this.columnPath.Width += sizeChange.Width;

			this.oldObjectReferenceListingSize = this.objectReferenceListing.Size;
		}
		private bool NodeFilter(TreeNodeAdv nodeAdv)
		{
			ReferenceNode node = nodeAdv.Tag as ReferenceNode;
			string filterValue = this.txtFilterInput.Text.ToLowerInvariant();

			return node != null &&
				(
					node.Name.ToLowerInvariant().Contains(filterValue) ||
					node.Path.ToLowerInvariant().Contains(filterValue)
				);
		}
	}
}
