using System;
using System.Linq;
using System.Collections.Generic;
using Aga.Controls.Tree;

namespace Duality.Editor
{
	public static class ExtMethodsTreeViewAdv
	{
		public static void SaveNodesExpanded(this TreeViewAdv view, TreeNodeAdv node, HashSet<object> expandedNodes, Func<TreeNodeAdv,object> idFunc)
		{
			if (node.IsExpanded) expandedNodes.Add(idFunc(node));
			foreach (TreeNodeAdv child in node.Children) SaveNodesExpanded(view, child, expandedNodes, idFunc);
		}
		public static void SaveNodesExpanded(this TreeViewAdv view, TreeNodeAdv node, HashSet<object> expandedNodes)
		{
			SaveNodesExpanded(view, node, expandedNodes, n => n.Tag);
		}
		public static void RestoreNodesExpanded(this TreeViewAdv view, TreeNodeAdv node, HashSet<object> expandedNodes, Func<TreeNodeAdv,object> idFunc)
		{
			node.IsExpanded = expandedNodes.Contains(idFunc(node));
			foreach (TreeNodeAdv child in node.Children) RestoreNodesExpanded(view, child, expandedNodes, idFunc);
		}
		public static void RestoreNodesExpanded(this TreeViewAdv view, TreeNodeAdv node, HashSet<object> expandedNodes)
		{
			RestoreNodesExpanded(view, node, expandedNodes, n => n.Tag);
		}
		public static TreeNodeAdv FirstVisibleNode(this TreeViewAdv view)
		{
			return view.Root.Children.FirstOrDefault(node => !node.IsHidden);
		}
		public static TreeNodeAdv LastVisibleNode(this TreeViewAdv view)
		{
			return view.Root.Children.LastOrDefault(node => !node.IsHidden);
		}
	}
}
