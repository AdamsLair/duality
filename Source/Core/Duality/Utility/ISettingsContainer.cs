using System;

namespace Duality
{
	public interface ISettingsContainer
	{
		event EventHandler Changed;
		void Load();
		void Save();
	}
}
