using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;

namespace Duality.Editor.AssetManagement
{
	public class AssetExportEnvironment : IAssetExportEnvironment
	{
		private bool isPrepareStep = false;
		private string exportDir = null;
		private Resource input = null;
		private bool isHandled = false;
		private List<string> outputPaths = new List<string>();

		public bool IsPrepareStep
		{
			get { return this.isPrepareStep; }
			set { this.isPrepareStep = value; }
		}
		public string ExportDirectory
		{
			get { return this.exportDir; }
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
		
		public AssetExportEnvironment(string exportDir, Resource input)
		{
			this.exportDir = exportDir;
			this.input = input;
		}

		public void ResetAcquiredData()
		{
			this.outputPaths.Clear();
			this.isHandled = false;
		}
		public string AddOutputPath(string localFilePath)
		{
			string filePath = Path.Combine(this.exportDir, localFilePath);

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
