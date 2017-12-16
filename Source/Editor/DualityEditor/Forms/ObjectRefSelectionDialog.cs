using System;
using System.Drawing;
using System.Windows.Forms;

namespace Duality.Editor.Forms
{	
	public partial class ObjectRefSelectionDialog : Form
	{
		public Type FilteredType { get; set; }

		public string ResourcePath { get; set; }
		public IContentRef ContentReference { get; set; }

		public ObjectRefSelectionDialog()
		{
			InitializeComponent();

			// this.resourceListing.DoubleClick += this.ResourceListingOnDoubleClick;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.Text = string.Format("Select a {0} Resource", this.FilteredType.Name);

			// this.resourceListing.Items.Clear();

			foreach (var contentRef in ContentProvider.GetAvailableContent(this.FilteredType))
			{
				var tmpLVI = new ListViewItem(new string[] { contentRef.Name, contentRef.FullName }, contentRef.FullName ?? contentRef.ResType.ToString())
				{
					Tag = contentRef
				};

				// this.resourceListing.Items.Add(tmpLVI);

				if (contentRef.FullName == this.ResourcePath)
				{
					tmpLVI.Selected = true;
				}
			}

			// this.resourceListing.Focus();
		}

		private void ResourceListingOnDoubleClick(object sender, EventArgs eventArgs)
		{
			this.ResourceListingOnSelectedIndexChanged(sender, eventArgs);
			this.AcceptButton.PerformClick();
		}

		private void ResourceListingOnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			/*foreach (ListViewItem resourceListingSelectedItem in this.resourceListing.SelectedItems)
			{
				var contentRef = resourceListingSelectedItem.Tag as IContentRef;

				if (contentRef != null)
				{
					this.ContentReference = contentRef;
					this.ResourcePath = this.ContentReference.FullName;
				}
			}*/
		}

		private void resourceListing_Resize(object sender, EventArgs e)
		{
			// this.resourceListing.TileSize = new Size(this.resourceListing.ClientSize.Width, this.resourceListing.TileSize.Height);
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
