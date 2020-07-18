using System;

namespace Duality
{
	public interface ISettingsContainer
	{
		/// <summary>
		/// Loads the data of the settings.
		/// </summary>
		void Load();
		/// <summary>
		/// Saves the data of the settings.
		/// </summary>
		void Save();
	}
}
