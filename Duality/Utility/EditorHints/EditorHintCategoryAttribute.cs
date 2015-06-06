using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Duality.Editor
{
	/// <summary>
	/// Provides information about a Types editor category.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class EditorHintCategoryAttribute : EditorHintAttribute
	{
		private	string		category		= null;
		private	string[]	categoryTree	= null;

		/// <summary>
		/// [GET] The preferred category tree to fit this Type in, split into hierarchial tokens.
		/// </summary>
		public string[] CategoryTree
		{
			get { return this.categoryTree; }
		}
		/// <summary>
		/// [GET] The preferred category tree to fit this Type in.
		/// </summary>
		public string Category
		{
			get { return this.category; }
		}

		public EditorHintCategoryAttribute(string category)
		{
			this.category = category;
			this.UpdateCategoryTree();
		}
		private void UpdateCategoryTree()
		{
			if (!string.IsNullOrWhiteSpace(this.category))
			{
				this.categoryTree = this.category.Split(
					new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, 
					StringSplitOptions.RemoveEmptyEntries);
			}
			else
			{
				this.categoryTree = new string[0];
			}
		}
	}
}
