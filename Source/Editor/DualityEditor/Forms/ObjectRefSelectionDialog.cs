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
			public ReferenceNode(string name, string path) : base(name)
			{
				this.Name = name;
				this.Path = path;
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
		}

		public Type FilteredType { get; set; }

		public string ResourcePath { get; set; }
		public IContentRef ContentReference { get; set; }

		public TreeModel model { get; set; }

		public ObjectRefSelectionDialog()
		{
			InitializeComponent();

			this.model = new TreeModel();

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
				ReferenceNode tmpNode = new ReferenceNode(contentRef.Name, contentRef.FullName);

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
			// TODO: Handle selection
		}

		/// <summary>
		/// Set the content reference to the currently selected elements value.
		/// </summary>
		/// <param name="data">a reference to a DataObject instance</param>
		public void SerializeToData(DataObject data)
		{
			data.SetContentRefs(new[] { this.ContentReference });
		}
	}
}
