using Duality.Serialization;

namespace Duality.Editor
{
	public class DualityProjectSettings
	{
		/// <summary>
		/// [GET] Returns the path where this DualityApp's <see cref="DualityProjectSettings">application data</see> is located at.
		/// </summary>
		public static string ProjectSettingsPath
		{
			get { return "ProjectSettings.dat"; }
		}

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
