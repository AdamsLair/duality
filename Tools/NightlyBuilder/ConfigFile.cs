using System.IO;
using System.Xml.Serialization;

namespace NightlyBuilder
{
	public sealed class ConfigFile
	{
		public string SolutionPath { get; set; }
		public string BuildResultDir { get; set; }
		public string DocSolutionPath { get; set; }
		public string DocBuildResultDir { get; set; }
		public string DocBuildResultFile { get; set; }
		public string NUnitBinDir { get; set; }
		public string UnitTestProjectDir { get; set; }
		public string PackageDir { get; set; }
		public string PackageName { get; set; }
		public string CopyPackageTo { get; set; }
		public string NuGetPath { get; set; }
		public string NuGetPackageSpecsDir { get; set; }
		public bool NoCleanNugetPackageTargetDir { get; set; }
		public string NuGetPackageTargetDir { get; set; }
		public bool NoBuild { get; set; }
		public bool NoTests { get; set; }
		public bool NoDocs { get; set; }
		public bool NonInteractive { get; set; }

		public void Save(string filePath)
		{
			using (FileStream stream = File.Open(filePath, FileMode.Create))
			{
				this.Save(stream);
			}
		}
		public void Save(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ConfigFile));
			serializer.Serialize(stream, this);
		}

		public static ConfigFile Load(string filePath)
		{
			ConfigFile result;
			using (FileStream stream = File.OpenRead(filePath))
			{
				result = Load(stream);
			}
			return result;
		}
		public static ConfigFile Load(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ConfigFile));
			return serializer.Deserialize(stream) as ConfigFile;
		}
	}
}
