using System;
using System.IO;

using Duality;
using Duality.IO;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class ShaderFileImporter : IFileImporter
	{
		public static readonly string SourceFileExtVertex = ".vert";
		public static readonly string SourceFileExtFragment = ".frag";

		public bool CanImportFile(string srcFile)
		{
			string ext = Path.GetExtension(srcFile);
			return 
				string.Equals(ext, SourceFileExtVertex, StringComparison.InvariantCultureIgnoreCase) || 
				string.Equals(ext, SourceFileExtFragment, StringComparison.InvariantCultureIgnoreCase); 
		}
		public void ImportFile(string srcFile, string targetName, string targetDir)
		{
			string ext = Path.GetExtension(srcFile);
			string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
			string sourceCode = File.ReadAllText(srcFile);
			if (string.Equals(ext, SourceFileExtVertex, StringComparison.InvariantCultureIgnoreCase))
			{
				VertexShader res = new VertexShader();
				res.Source = sourceCode;
				res.SourcePath = srcFile;
				res.Compile();
				res.Save(output[0]);
			}
			else
			{
				FragmentShader res = new FragmentShader();
				res.Source = sourceCode;
				res.SourcePath = srcFile;
				res.Compile();
				res.Save(output[0]);
			}
		}

		public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
		{
			string ext = Path.GetExtension(srcFile);
			if (r.Is<VertexShader>() && string.Equals(ext, SourceFileExtVertex, StringComparison.InvariantCultureIgnoreCase))
				return true;
			else if (r.Is<FragmentShader>() && string.Equals(ext, SourceFileExtFragment, StringComparison.InvariantCultureIgnoreCase))
				return true;
			else
				return false;
		}
		public void ReImportFile(ContentRef<Resource> r, string srcFile)
		{
			AbstractShader s = r.Res as AbstractShader;
			string sourceCode = File.ReadAllText(srcFile);
			s.Source = sourceCode;
			s.SourcePath = srcFile;
			s.Compile();
		}

		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<AbstractShader> s = r.As<AbstractShader>();
			return s != null && s.Res.SourcePath == srcFile;
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string ext = Path.GetExtension(srcFile).ToLower();
			string targetResPath;
			if (ext == SourceFileExtVertex)
				targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Resource.GetFileExtByType<VertexShader>());
			else
				targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Resource.GetFileExtByType<FragmentShader>());
			return new string[] { targetResPath };
		}
	}
}
