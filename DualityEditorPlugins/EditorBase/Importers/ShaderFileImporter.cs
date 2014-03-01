using System.IO;

using Duality;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class ShaderFileImporter : IFileImporter
	{
		public bool CanImportFile(string srcFile)
		{
			string ext = Path.GetExtension(srcFile).ToLower();
			return ext == ".vert" || ext == ".frag";
		}
		public void ImportFile(string srcFile, string targetName, string targetDir)
		{
			string ext = Path.GetExtension(srcFile).ToLower();
			string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
			if (ext == ".vert")
			{
				VertexShader res = new VertexShader();
				res.LoadSource(srcFile);
				res.Compile();
				res.Save(output[0]);
			}
			else
			{
				FragmentShader res = new FragmentShader();
				res.LoadSource(srcFile);
				res.Compile();
				res.Save(output[0]);
			}
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string ext = Path.GetExtension(srcFile).ToLower();
			string targetResPath;
			if (ext == ".vert")
				targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), VertexShader.FileExt);
			else
				targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), FragmentShader.FileExt);
			return new string[] { targetResPath };
		}


		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<AbstractShader> s = r.As<AbstractShader>();
			return s != null && s.Res.SourcePath == srcFile;
		}
		public void ReimportFile(ContentRef<Resource> r, string srcFile)
		{
			AbstractShader s = r.Res as AbstractShader;
			s.LoadSource(srcFile);
			s.Compile();
		}
	}
}
