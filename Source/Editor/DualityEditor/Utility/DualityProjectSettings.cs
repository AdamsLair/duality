using Duality.Serialization;

namespace Duality.Editor
{
	public class DualityProjectSettings
	{
		private static readonly string ProjectSettingsPath = "ProjectSettings.dat";

		private string launcherPath = "DualityGame.exe";
		public string LauncherPath
		{
			get { return this.launcherPath; }
			set { this.launcherPath = value; }
		}

		public void Save()
		{
			Serializer.WriteObject(this, ProjectSettingsPath, typeof(XmlSerializer));
		}

		public static DualityProjectSettings Load()
		{
			return Serializer.TryReadObject<DualityProjectSettings>(ProjectSettingsPath) ?? new DualityProjectSettings();
		}
	}
}
