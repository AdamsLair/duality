using System;
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

			this.nodeName.DrawText += this.NodeName_OnDrawText;
			this.nodePath.DrawText += this.NodeName_OnDrawText;
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
			this.objectReferenceListing.Model = this.model;

			this.objectReferenceListing.BeginUpdate();
			this.model.Nodes.Clear();
			
			if (this.FilteredType.IsSubclassOf(typeof(GameObject)) || this.FilteredType == typeof(GameObject))
			{
				this.Text = "Select a GameObject";

				foreach (GameObject currentObject in Scene.Current.AllObjects)
				{
					ReferenceNode tmpNode = new ReferenceNode(currentObject);

					this.model.Nodes.Add(tmpNode);
				}
			}
			else if (this.FilteredType.IsSubclassOf(typeof(Component)) || this.FilteredType == typeof(Component))
			{
				this.Text = string.Format("Select a {0} Component", this.FilteredType.GetTypeCSCodeName());

				foreach (Component currentComponent in Scene.Current.FindComponents(this.FilteredType))
				{
					ReferenceNode tmpNode = new ReferenceNode(currentComponent);

					this.model.Nodes.Add(tmpNode);
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

					this.model.Nodes.Add(tmpNode);
				}
			}
			
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
