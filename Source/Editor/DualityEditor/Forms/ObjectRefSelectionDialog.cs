using System;
using System.Windows.Forms;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace Duality.Editor.Forms
{
	using System.Drawing;

	public partial class ObjectRefSelectionDialog : Form
	{
		public class ReferenceNode : Node
		{
			public ReferenceNode(IContentRef resource) : base(resource.Name)
			{
				this.Name = resource.Name;
				this.Path = resource.FullName;

				this.ResourceReference = resource;
			}

			private string name;

			public string Name
			{
				get { return this.name; }
				set { this.name = value; }
			}

			private string path;

			public string Path
			{
				get { return this.path; }
				set { this.path = value; }
			}

			private IContentRef resourceReference;

			public IContentRef ResourceReference
			{
				get { return this.resourceReference; }
				set { this.resourceReference = value; }
			}
		}

		public Type FilteredType { get; set; }

		public string ResourcePath { get; set; }
		public IContentRef ResourceReference { get; set; }

		public TreeModel model { get; set; }

		public ObjectRefSelectionDialog()
		{
			InitializeComponent();

			this.model = new TreeModel();

			this.objectReferenceListing.Click += this.ResourceListingOnSelectedIndexChanged;
			this.objectReferenceListing.DoubleClick += this.ResourceListingOnDoubleClick;

			this.nodeName.DrawText += this.NodeNameOnDrawText;
			this.nodePath.DrawText += this.NodePathOnDrawText;
		}

		private void NodePathOnDrawText(object sender, DrawTextEventArgs drawTextEventArgs)
		{
			// drawTextEventArgs.TextColor = Color.Red;
		}

		private void NodeNameOnDrawText(object sender, DrawTextEventArgs drawTextEventArgs)
		{
			// drawTextEventArgs.TextColor = Color.Red;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.Text = string.Format("Select a {0} Resource", this.FilteredType.Name);

			this.objectReferenceListing.ClearSelection();
			this.objectReferenceListing.Model = this.model;

			this.objectReferenceListing.BeginUpdate();
			this.model.Nodes.Clear();
			
			foreach (IContentRef contentRef in ContentProvider.GetAvailableContent(this.FilteredType))
			{
				ReferenceNode tmpNode = new ReferenceNode(contentRef);

				tmpNode.Text = contentRef.Name;

				this.model.Nodes.Add(tmpNode);
			}

			foreach (var treeNodeAdv in this.objectReferenceListing.AllNodes)
			{
				ReferenceNode tmpNode = treeNodeAdv.Tag as ReferenceNode;

				if (tmpNode.Path == this.ResourcePath)
				{
					this.objectReferenceListing.SelectedNode = treeNodeAdv;
					break;
				}
			}

			this.objectReferenceListing.EndUpdate();
			this.objectReferenceListing.Focus();
		}

		private void ResourceListingOnDoubleClick(object sender, EventArgs eventArgs)
		{
			this.ResourceListingOnSelectedIndexChanged(sender, eventArgs);
			this.AcceptButton.PerformClick();
		}

		private void ResourceListingOnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			ReferenceNode node = this.objectReferenceListing.SelectedNode.Tag as ReferenceNode;

			this.ResourceReference = node.ResourceReference;
		}
	}
}
