namespace Duality.Editor.Forms
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Controls.TreeModels.FileSystem;

	public partial class ContentSelectionDialog : Form
	{
		public Type FilteredType { get; set; }

		public string ResourcePath { get; set; }
		public IContentRef ContentReference { get; set; }

		public ContentSelectionDialog()
		{
			InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.Text = string.Format("Select a {0} Resource", this.FilteredType.Name);
			Log.Editor.Write(string.Format("PATH: {0}", this.ResourcePath));

			this.imageListResourceListing.Images.Clear();
			this.resourceListing.Items.Clear();

			foreach (var contentRef in ContentProvider.GetAvailableContent(this.FilteredType))
			{
				Log.Editor.Write(contentRef.FullName);

				var tmpLVI = new ListViewItem(new string[] { contentRef.Name, contentRef.FullName }, contentRef.FullName ?? contentRef.ResType.ToString())
				{
					Tag = contentRef
				};

				Log.Editor.Write("{0} - {1}", contentRef.Name, contentRef.ResType.ToString());

				this.resourceListing.Items.Add(tmpLVI);
			}
		}

		private void resourceListing_SelectedIndexChanged(object sender, EventArgs e)
		{
			foreach (ListViewItem resourceListingSelectedItem in this.resourceListing.SelectedItems)
			{
				var contentRef = resourceListingSelectedItem.Tag as IContentRef;

				if (contentRef != null)
				{
					Log.Editor.Write("Selected {0}, updated to reference {1}", contentRef.Name, contentRef.FullName);

					this.ContentReference = contentRef;
					this.ResourcePath = this.ContentReference.FullName;
				}
			}
		}

		private void resourceListing_Resize(object sender, EventArgs e)
		{
			this.resourceListing.TileSize = new Size(this.resourceListing.ClientSize.Width, this.resourceListing.TileSize.Height);
		}

		public void SerializeToData(DataObject data)
		{
			data.SetContentRefs(new[] { this.ContentReference });
		}
	}
}
