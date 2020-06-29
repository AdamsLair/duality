using System;
using Duality.Serialization;

namespace Duality
{
	public class SettingsContainer<TSettings> : ISettingsContainer
		where TSettings : class, new()
	{
		public event EventHandler Changed;
		public TSettings Value { get; private set; }
		private readonly string path;

		public SettingsContainer(string path)
		{
			this.path = path;
		}

		public void Load()
		{
			this.Value = Serializer.TryReadObject<TSettings>(this.path) ?? new TSettings();
			Changed?.Invoke(this, EventArgs.Empty);
		}

		public void Save()
		{
			Serializer.WriteObject(this.Value, this.path, typeof(XmlSerializer));
		}
	}
}
