using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor
{
	/// <summary>
	/// Provides general information about this Duality application / game.
	/// </summary>
	public class DualityEditorAppData
	{
		private string sourcePath = "Source";
		private string importPath = "Import";

		/// <summary>
		/// [GET / SET] The path to the source code of your game.
		/// </summary>
		public string SourcePath
		{
			get { return this.sourcePath; }
			set { this.sourcePath = value; }
		}

		/// <summary>
		/// [GET / SET] The path to the import folder.
		/// </summary>
		public string ImportPath
		{
			get { return this.importPath; }
			set { this.importPath = value; }
		}
	}
}
