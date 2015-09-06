using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor
{
	public interface IFileImporter
	{
		bool CanImportFile(string srcFile);
		void ImportFile(string srcFile, string targetName, string targetDir);

		bool CanReImportFile(ContentRef<Resource> r, string srcFile);
		void ReImportFile(ContentRef<Resource> r, string srcFile);

		bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile);
		string[] GetOutputFiles(string srcFile, string targetName, string targetDir);
	}
}
