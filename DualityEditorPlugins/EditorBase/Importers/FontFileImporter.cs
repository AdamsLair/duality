using System.IO;

using Duality;
using Duality.IO;
using Duality.Resources;
using Duality.Editor;

namespace Duality.Editor.Plugins.Base
{
	public class FontFileImporter : IFileImporter
	{
		public bool CanImportFile(string srcFile)
		{
			string ext = Path.GetExtension(srcFile).ToLower();
			return ext == ".ttf";
		}
		public void ImportFile(string srcFile, string targetName, string targetDir)
		{
			string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
			Font font = new Font();

			font.SourcePath = srcFile;
			font.EmbeddedTrueTypeFont = File.ReadAllBytes(srcFile);
			font.RenderGlyphs();
			font.Save(output[0]);
		}

		public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
		{
			return r.Is<Font>();
		}
		public void ReImportFile(ContentRef<Resource> r, string srcFile)
		{
			Font font = r.Res as Font;
			font.SourcePath = srcFile;
			font.EmbeddedTrueTypeFont = File.ReadAllBytes(srcFile);
			font.RenderGlyphs();
		}

		public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
		{
			ContentRef<Font> f = r.As<Font>();
			return f != null && f.Res.EmbeddedTrueTypeFont != null && f.Res.SourcePath == srcFile;
		}
		public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
		{
			string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Resource.GetFileExtByType<Font>());
			return new string[] { targetResPath };
		}
	}
}
