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
		/// [GET / SET] The name of your application / game. It will also be used as a window title by the launcher app.
		/// </summary>
		public string SourcePath
		{
			get { return this.sourcePath; }
			set { this.sourcePath = value; }
		}

		/// <summary>
		/// [GET / SET] The name of your application / game. It will also be used as a window title by the launcher app.
		/// </summary>
		public string ImportPath
		{
			get { return this.importPath; }
			set { this.importPath = value; }
		}
	}
}
