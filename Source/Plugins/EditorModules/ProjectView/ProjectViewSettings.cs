using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Editor.Plugins.ProjectView
{
	public class ProjectViewSettings
	{
		private string importSourcePath;

		public string ImportSourcePath
		{
			get { return this.importSourcePath; }
			set { this.importSourcePath = value; }
		}
	}
}
