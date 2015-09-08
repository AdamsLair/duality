using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;

namespace Duality.Editor
{
	/// <summary>
	/// Represents a single work item in an Asset import or re-import operation.
	/// </summary>
	public struct AssetImportInput
	{
		private string path;
		private string relativePath;
		private string fullAssetName;

		/// <summary>
		/// [GET] The path where this input file is located.
		/// </summary>
		public string Path
		{
			get { return this.path; }
		}
		/// <summary>
		/// [GET] The input path, relative to the input base directory of the import operation.
		/// </summary>
		public string RelativePath
		{
			get { return this.relativePath; }
		}
		/// <summary>
		/// [GET] The input's full name. Use this to derive Resource names from input items during import operations.
		/// </summary>
		public string FullAssetName
		{
			get { return this.fullAssetName; }
		}

		public AssetImportInput(string path, string baseDir)
		{
			this.path = path;
			this.relativePath = PathHelper.MakeFilePathRelative(this.path, baseDir);
			this.fullAssetName = System.IO.Path.Combine(
				System.IO.Path.GetDirectoryName(this.relativePath), 
				System.IO.Path.GetFileNameWithoutExtension(this.path));
		}
		public AssetImportInput(string path, string relativePath, string fullAssetName)
		{
			this.path = path;
			this.relativePath = relativePath;
			this.fullAssetName = fullAssetName;
		}

		public override string ToString()
		{
			return string.Format("{0} [{1}]", this.path, this.relativePath);
		}
	}
}
