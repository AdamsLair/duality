using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace VersionUpdater
{
	public sealed class ConfigFile
	{
		public List<string> GitSearchPaths { get; set; }
		public string NuSpecRootDir { get; set; }
		public string SolutionPath { get; set; }
		public string InstallerPackageConfigPath { get; set; }
		public bool ApplyGlobalMajorUpdate { get; set; }

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
