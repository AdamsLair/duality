using System;
using System.Windows.Forms;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace Duality.Editor.Forms
{

	public partial class ObjectRefSelectionDialog : Form
	{
		public class ReferenceNode : Node
		{
			public ReferenceNode(string name, string path) : base(name)
			{
				this.FullName = path;
			}

			public string FullName { get; set; }
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
		}

		private void NodeNameOnDrawText(object sender, DrawTextEventArgs drawTextEventArgs)
		{
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.Text = string.Format("Select a {0} Resource", this.FilteredType.Name);

			this.objectReferenceListing.ClearSelection();
			this.objectReferenceListing.Model = this.model;

			this.model.Nodes.Clear();

			foreach (IContentRef contentRef in ContentProvider.GetAvailableContent(this.FilteredType))
			{
				ReferenceNode tmpNode = new ReferenceNode(contentRef.Name, contentRef.FullName);

				this.model.Nodes.Add(tmpNode);

				if (contentRef.FullName == this.ResourcePath)
				{
				}
			}

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
