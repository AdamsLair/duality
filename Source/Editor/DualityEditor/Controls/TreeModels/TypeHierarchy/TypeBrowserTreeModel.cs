﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Aga.Controls.Tree;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using System.Reflection;

namespace Duality.Editor.Controls.TreeModels.TypeHierarchy
{
	public class TypeBrowserTreeModel : ITreeModel
	{
		private	Assembly[]			assemblies		= null;
		private	string[]			namespaces		= null;
		private	Predicate<Type>		filter			= null;
		private	Type				baseType		= typeof(object);
		private	bool				showNamespaces	= true;

		public Predicate<Type> Filter
		{
			get { return this.filter; }
			set { this.filter = value; }
		}
		public Type BaseType
		{
			get { return this.baseType; }
			set
			{
				this.baseType = value ?? typeof(object);
				if (this.StructureChanged != null)
					this.StructureChanged(this, new TreePathEventArgs());
			}
		}
		public bool ShowNamespaces
		{
			get { return this.showNamespaces; }
			set
			{
				this.showNamespaces = value;
				if (this.StructureChanged != null)
					this.StructureChanged(this, new TreePathEventArgs());
			}
		}

		public TypeBrowserTreeModel(Type baseType = null)
		{
			this.baseType = baseType ?? typeof(object);
		}

		private void InitAssemblyCache()
		{
			if (this.assemblies != null) return;

			// Retrieve a list of all loaded, non-disposed Assemblies
			Assembly[] loadedAssemblies = 
				DualityApp.AssemblyLoader.LoadedAssemblies
				.Where(a => !DualityApp.PluginManager.DisposedPlugins.Contains(a))
				.ToArray();

			// Aggregate selectable assemblies based on Duality core Assemblies and their dependencies
			HashSet<Assembly> selectableAssemblies = new HashSet<Assembly>();
			foreach (Assembly coreAssembly in DualityApp.GetDualityAssemblies())
			{
				selectableAssemblies.Add(coreAssembly);

				AssemblyName[] referencedAssemblies = coreAssembly.GetReferencedAssemblies();
				foreach (AssemblyName reference in referencedAssemblies)
				{
					string shortName = reference.GetShortAssemblyName();
					Assembly dependency = loadedAssemblies.FirstOrDefault(a => a.GetShortAssemblyName() == shortName);
					if (dependency != null)
						selectableAssemblies.Add(dependency);
				}
			}

			this.assemblies = selectableAssemblies.ToArray();
			this.namespaces = this.assemblies
				.SelectMany(a => { try { return a.GetExportedTypes(); } catch (Exception) { return new Type[0]; } })
				.Select(t => t.Namespace)
				.Distinct()
				.Where(n => !string.IsNullOrEmpty(n))
				.ToArray();
		}

		private static string GetRootNamespace(string name)
		{
			int firstDot = name.IndexOf('.');
			if (firstDot == -1)
				return name;
			else
				return name.Substring(0, firstDot);
		}
		private static string GetParentNamespace(string name)
		{
			int lastDot = name.LastIndexOf('.');
			if (lastDot == -1)
				return null;
			else
				return name.Substring(0, lastDot);
		}
		private IEnumerable<string> GetSubNamespaces(string parentNamespace)
		{
			if (string.IsNullOrEmpty(parentNamespace))
			{
				return this.namespaces.Select(n => GetRootNamespace(n)).Distinct();
			}
			else
			{
				return this.namespaces.Where(n => GetParentNamespace(n) == parentNamespace);
			}
		}

		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
			this.InitAssemblyCache();
			List<BaseItem> items = new List<BaseItem>();

			BaseItem parentItem = treePath.LastNode as BaseItem;
			TypeItem parentTypeItem = parentItem as TypeItem;
			NamespaceItem parentNamespaceItem = parentItem as NamespaceItem;

			Type parentType = parentTypeItem != null ? parentTypeItem.TypeInfo : this.baseType;
			string parentName = parentNamespaceItem != null ? parentNamespaceItem.Name : null;

			if (this.showNamespaces && parentTypeItem == null)
			{
				foreach (string subName in this.GetSubNamespaces(parentName))
				{
					items.Add(new NamespaceItem(subName, parentItem));
				}
			}
			if (!this.showNamespaces || parentName != null)
			{
				foreach (Assembly assembly in this.assemblies)
				{
					foreach (Type exportedType in assembly.GetExportedTypes())
					{
						if (this.showNamespaces && exportedType.Namespace != parentName) continue;
						if (!parentType.IsAssignableFrom(exportedType)) continue;
						if (parentType == exportedType) continue;
						if (this.filter != null && !this.filter(exportedType)) continue;
						items.Add(new TypeItem(exportedType, parentItem));
					}
				}
			}

			return items;
		}
		public bool IsLeaf(TreePath treePath)
		{
			TypeItem item = treePath.LastNode as TypeItem;
			return item != null && item.TypeInfo != null && (item.TypeInfo.IsSealed || item.TypeInfo.IsValueType);
		}

		#pragma warning disable 67  // Event never used
		public event EventHandler<TreeModelEventArgs> NodesChanged;
		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;
	}
}
