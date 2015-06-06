using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace Duality.Editor
{
	public static class ExtMethodsMemberInfoEditorHint
	{
		/// <summary>
		/// Returns the category tree which this Type prefers to be in.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string[] GetEditorCategory(this Type type)
		{
			string[] tree = null;
			foreach (var attrib in type.GetAttributesCached<EditorHintCategoryAttribute>())
			{
				tree = attrib.CategoryTree;
				if (tree != null) break;
			}
			if (tree == null) tree = type.Namespace.Split('.');
			return tree;
		}
		/// <summary>
		/// Return the preferred icon image representation of the specified Type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Image GetEditorImage(this Type type)
		{
			Image image = null;
			foreach (var attrib in type.GetAttributesCached<EditorHintImageAttribute>())
			{
				image = attrib.IconImage;
				if (image != null) break;
			}
			return image;
		}
	}
}
