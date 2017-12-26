using System;
using System.Windows.Forms;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using Duality.Editor.Extensibility.DataConversion;
using Duality.Resources;


namespace Duality.Editor.Forms
{
	public partial class ObjectRefSelectionDialog : Form, IObjectRefHolder
	{
		public class ReferenceNode : Node
		{
			public ReferenceNode(IContentRef resource) : base(resource.Name)
			{
				this.Name = resource.Name;
				this.Path = resource.FullName;

				this.ResourceReference = resource;
			}

			public ReferenceNode(GameObject resource) : base(resource.Name)
			{
				this.Name = resource.Name;
				this.Path = resource.FullName;

				this.GameObjectReference = resource;
			}

			public ReferenceNode(Component resource) : base(resource.GetType().Name)
			{
				this.Name = resource.GetType().Name;
				this.Path = resource.GetType().FullName;

				this.ComponentReference = resource;
			}

			private string _name;

			public string Name
			{
				get { return this._name; }
				set { this._name = value; }
			}

			private string _path;

			public string Path
			{
				get { return this._path; }
				set { this._path = value; }
			}

			private IContentRef _resourceReference;

			public IContentRef ResourceReference
			{
				get { return this._resourceReference; }
				set { this._resourceReference = value; }
			}

			private GameObject _gameObjectReference;

			public GameObject GameObjectReference
			{
				get { return this._gameObjectReference; }
				set { this._gameObjectReference = value; }
			}

			private Component _componentReference;

			public Component ComponentReference
			{
				get { return this._componentReference; }
				set { this._componentReference = value; }
			}
		}

		public Type FilteredType { get; set; }

		public string ResourcePath { get; set; }
		public IContentRef ResourceReference { get; set; }
		public GameObject GameObjectReference { get; set; }
		public Component ComponentReference { get; set; }

		public TreeModel model { get; set; }

		public ObjectRefSelectionDialog()
		{
			InitializeComponent();

			this.model = new TreeModel();

			this.objectReferenceListing.Click += this.ResourceListingOnSelectedIndexChanged;
			this.objectReferenceListing.DoubleClick += this.ResourceListingOnDoubleClick;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.Text = string.Format("Select a {0} Resource", this.FilteredType.Name);

			this.objectReferenceListing.ClearSelection();
			this.objectReferenceListing.Model = this.model;

			this.objectReferenceListing.BeginUpdate();
			this.model.Nodes.Clear();
			
			if (this.FilteredType.IsSubclassOf(typeof(GameObject)) || this.FilteredType == typeof(GameObject))
			{
				foreach (GameObject currentObject in Scene.Current.AllObjects)
				{
					ReferenceNode tmpNode = new ReferenceNode(currentObject);

					tmpNode.Text = currentObject.Name;

					this.model.Nodes.Add(tmpNode);
				}
			}
			else if (this.FilteredType.IsSubclassOf(typeof(Component)) || this.FilteredType == typeof(Component))
			{
				foreach (Component currentComponent in Scene.Current.FindComponents(this.FilteredType))
				{
					ReferenceNode tmpNode = new ReferenceNode(currentComponent);

					tmpNode.Text = currentComponent.GetType().Name;

					this.model.Nodes.Add(tmpNode);
				}
			}
			else
			{
				foreach (IContentRef contentRef in ContentProvider.GetAvailableContent(this.FilteredType.GenericTypeArguments[0]))
				{
					ReferenceNode tmpNode = new ReferenceNode(contentRef)
					{
						Text = contentRef.Name
					};

					this.model.Nodes.Add(tmpNode);
				}
			}
			
			foreach (var treeNodeAdv in this.objectReferenceListing.AllNodes)
			{
				ReferenceNode tmpNode = treeNodeAdv.Tag as ReferenceNode;

				if (tmpNode != null && tmpNode.Path == this.ResourcePath)
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

			this.ResourceReference = node.ResourceReference;
			this.GameObjectReference = node.GameObjectReference;
			this.ComponentReference = node.ComponentReference;
		}
	}
}
