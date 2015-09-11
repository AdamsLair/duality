using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public class AssetExportEnvironment : IAssetExportEnvironment
	{
		private bool isPrepareStep = false;
		private string sourceDir = null;
		private Resource input = null;
		private bool isHandled = false;
		private List<string> outputPaths = new List<string>();

		public bool IsPrepareStep
		{
			get { return this.isPrepareStep; }
			set { this.isPrepareStep = value; }
		}
		public string SourceDirectory
		{
			get { return this.sourceDir; }
		}
		public Resource Input
		{
			get { return this.input; }
		}
		public bool IsHandled
		{
			get { return this.isHandled; }
		}
		public IEnumerable<string> OutputPaths
		{
			get { return this.outputPaths; }
		}
		
		public AssetExportEnvironment(string sourceDir, Resource input)
		{
			this.sourceDir = sourceDir;
			this.input = input;
		}

		public void ResetAcquiredData()
		{
			this.outputPaths.Clear();
			this.isHandled = false;
		}
		public string AddOutputPath(string localFilePath)
		{
			string filePath = Path.Combine(this.sourceDir, localFilePath);

			// If we're doing actual work, make sure the directory exists
			if (!this.isPrepareStep)
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			}

			this.outputPaths.Add(filePath);
			this.isHandled = true;
			return filePath;
		}
	}
}
