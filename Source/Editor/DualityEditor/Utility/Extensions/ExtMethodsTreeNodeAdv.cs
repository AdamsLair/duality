using Aga.Controls.Tree;

namespace Duality.Editor
{
	public static class ExtMethodsTreeNodeAdv
	{
		public static bool IsChildOf(this TreeNodeAdv child, TreeNodeAdv parent)
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
				return IsChildOf(child.Parent, parent);
			}
		}
	}
}
