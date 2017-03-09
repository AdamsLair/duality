using Aga.Controls.Tree;

namespace Duality.Editor
{
	public static class ExtMethodsTreeNodeAdv
	{
		public static bool IsParentOf(this TreeNodeAdv parent, TreeNodeAdv child)
		{
			if (child.Parent == null || child.Parent == child.Tree.Root)
			{
				return false;
			}
			else if (child.Parent == parent)
			{
				return true;
			}
			else
			{
				return IsParentOf(child.Parent, parent);
			}
		}
	}
}
