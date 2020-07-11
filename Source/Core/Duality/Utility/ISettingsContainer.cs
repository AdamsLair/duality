using System;

namespace Duality
{
	public interface ISettingsContainer
	{
		/// <summary>
		/// Fired when settings have changed.
		/// </summary>
		event EventHandler Changed;

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
