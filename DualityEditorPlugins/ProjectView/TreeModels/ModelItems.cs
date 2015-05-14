using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using Aga.Controls.Tree;

namespace Duality.Editor.Plugins.ProjectView.TreeModels
{
	public abstract class NodeBase : Node
	{
		public static string GetNodePathId(string nodePath)
		{
			if (ContentProvider.IsDefaultContentPath(nodePath))
				return nodePath.ToUpper();
			else
				return Path.GetFullPath(nodePath).ToUpper();
		}

		private	string	nodePath		= null;
		private	bool	readOnly		= false;

		public IEnumerable<NodeBase> NodesDeep
		{
			get
			{
				foreach (NodeBase n in this.Nodes.OfType<NodeBase>())
				{
					yield return n;
					foreach (NodeBase c in n.NodesDeep)
						yield return c;
				}
			}
		}
		public string NodePath
		{
			get { return this.nodePath; }
			set
			{
				if (this.nodePath != value)
				{
					string oldPath = this.nodePath;
					this.nodePath = value;
					this.OnNodePathChanged(oldPath);
				}
			}
		}
		public string NodePathId
		{
			get { return GetNodePathId(this.NodePath); }
		}
		public bool ReadOnly
		{
			get { return this.readOnly; }
		}
		public virtual string TypeName
		{
			get { return null; }
		}

		protected NodeBase(string path, string name, bool readOnly = false) : base(name)
		{
			this.nodePath = path;
			this.readOnly = readOnly;
		}
			
		public void NotifyVisible()
		{
			this.OnFirstVisible();
		}
		public void ApplyPathToName()
		{
			this.Text = this.GetNameFromPath(this.nodePath);
		}
		public bool ApplyNameToPath()
		{
			string outVar;
			return this.ApplyNameToPath(out outVar);
		}
		public virtual bool ApplyNameToPath(out string conflictingPath)
		{
			conflictingPath = null;
			return false;
		}
		public virtual string GetNameFromPath(string path)
		{
			if (ContentProvider.IsDefaultContentPath(path))
			{
				string[] pathSplit = path.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
				return pathSplit[pathSplit.Length - 1];
			}
			else
			{
				return path;
			}
		}

		protected virtual void OnNodePathChanged(string oldPath)
		{

		}
		protected virtual void OnFirstVisible() {}
	}
	public class DirectoryNode : NodeBase
	{
		public DirectoryNode(string path, bool readOnly = false) : base(path, null, readOnly)
		{
			this.ApplyPathToName();
		}
			
		public override bool ApplyNameToPath(out string conflictingPath)
		{
			conflictingPath = null;
 			if (this.ReadOnly) return false;

			string oldPath = this.NodePath;
			string oldDirName = Path.GetFileName(oldPath);
			string newPathBase = oldPath.Remove(oldPath.Length - oldDirName.Length, oldDirName.Length);
			string newPath = newPathBase + this.Text;
			bool equalsCaseInsensitive = newPath.ToUpper() == oldPath.ToUpper();

			if (Directory.Exists(newPath) && !equalsCaseInsensitive)
			{
				conflictingPath = newPath;
				return false;
			}

			if (equalsCaseInsensitive)
			{
				string tempPath = newPath + "_sSJencn83rhfSHhfn3ns456omvmvs28fndDN84ns";
				Directory.Move(oldPath, tempPath);
				Directory.Move(tempPath, newPath);
			}
			else
				Directory.Move(oldPath, newPath);

			// Between performing the move event and it being received by the FileEventManager there will be a
			// short window of inconsistency where the existing Resource is still registered under its old name
			// but the file is already renamed to the new name. To prevent loading the Resource twice, we'll pre-register
			// it under its new name.
			foreach (ResourceNode resNode in this.NodesDeep.OfType<ResourceNode>())
			{
				if (resNode.ResLink.ResWeak != null)
					ContentProvider.AddContent(resNode.NodePath.Replace(oldPath, newPath), resNode.ResLink.ResWeak);
			}

			this.NodePath = newPath;
			return true;
		}
		public override string GetNameFromPath(string path)
		{
			if (!ContentProvider.IsDefaultContentPath(path))
				return Path.GetFileName(path);
			else
				return base.GetNameFromPath(path);
		}

		protected override void OnNodePathChanged(string oldPath)
		{
			base.OnNodePathChanged(oldPath);
			foreach (NodeBase node in this.Nodes)
			{
				node.NodePath = this.NodePath + node.NodePath.Remove(0, oldPath.Length);
			}
		}
	}
	public class ResourceNode : NodeBase
	{
		private	IContentRef	res		= null;
		private	Type		resType	= null;

		public IContentRef ResLink
		{
			get { return this.res; }
		}
		public Type ResType
		{
			get { return this.resType; }
		}
		public override string TypeName
		{
			get { return this.resType != null ? this.resType.GetTypeKeyword() : null; }
		}

		public ResourceNode(string path) : base(path, null, false)
		{
			string[] fileNameSplit = Path.GetFileNameWithoutExtension(path).Split('.');

			this.res = new ContentRef<Resource>(null, path);
			this.resType = Resource.GetTypeByFileName(path);
			this.ApplyPathToName();
		}
		public ResourceNode(IContentRef res) : base(res.Path, null, res.IsDefaultContent)
		{
			this.res = res;
			this.resType = res.ResType;
			this.ApplyPathToName();
		}

		public void UpdateImage()
		{
			this.Image = GetTypeImage(this.resType, this.res);				
		}

		public override bool ApplyNameToPath(out string conflictingPath)
		{
			conflictingPath = null;
			if (this.ReadOnly) return false;

			string oldPath = this.NodePath;
			string oldFileName = Path.GetFileName(oldPath);
			string newPathBase = oldPath.Remove(oldPath.Length - oldFileName.Length, oldFileName.Length);
			string newPath = newPathBase + this.Text + Resource.GetFileExtByType(this.resType);
			bool equalsCaseInsensitive = newPath.ToUpper() == oldPath.ToUpper();

			if (File.Exists(newPath) && !equalsCaseInsensitive)
			{
				conflictingPath = newPath;
				return false;
			}

			if (equalsCaseInsensitive)
			{
				string tempPath = newPath + "_sSJencn83rhfSHhfn3ns456omvmvs28fndDN84ns";
				File.Move(oldPath, tempPath);
				File.Move(tempPath, newPath);
			}
			else
				File.Move(oldPath, newPath);

			// Between performing the move event and it being received by the FileEventManager there will be a
			// short window of inconsistency where the existing Resource is still registered under its old name
			// but the file is already renamed to the new name. To prevent loading the Resource twice, we'll pre-register
			// it under its new name.
			if (this.res.ResWeak != null) ContentProvider.AddContent(newPath, this.res.ResWeak);

			this.NodePath = newPath;
			return true;
		}
		public override string GetNameFromPath(string path)
		{
			if (!ContentProvider.IsDefaultContentPath(path))
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
				string[] fileNameSplit = fileName.Split('.');
				return fileNameSplit[0];
			}
			else
				return base.GetNameFromPath(path);
		}

		protected override void OnNodePathChanged(string oldPath)
		{
			base.OnNodePathChanged(oldPath);
			this.res.Path = this.NodePath;
		}
		protected override void OnFirstVisible()
		{
			base.OnFirstVisible();
			this.UpdateImage();
		}

		public static Image GetTypeImage(Type type, IContentRef resLink = null)
		{
			return (type ?? typeof(Resource)).GetEditorImage();
		}
	}
}
