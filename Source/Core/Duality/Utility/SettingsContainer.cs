using System;
using Duality.Serialization;

namespace Duality
{
	/// <summary>
	/// A container class to abstract all serialization logic away from settings classes.
	/// </summary>
	/// <typeparam name="TSettings"></typeparam>
	public class SettingsContainer<TSettings> : ISettingsContainer where TSettings : class, new()
	{
		public string Path { get; }
		/// <summary>
		/// The settings data. This is null until <see cref="Load"/> is called for the first time, 
		/// after which it can never be null again.
		/// </summary>
		public TSettings Instance { get; private set; }


		/// <summary>
		/// Fired when <see cref="Instance"/> has changed.
		/// </summary>
		public event EventHandler Changed;


		/// <summary>
		/// Creates a new settings container where the data for the settings will be saved and loaded 
		/// from <paramref name="path"/>.
		/// </summary>
		/// <param name="path"></param>
		public SettingsContainer(string path)
		{
			this.Path = path;
		}

		/// <summary>
		/// Loads the data of <typeparamref name="TSettings"/> from file.
		/// </summary>
		public void Load()
		{
			this.Instance = Serializer.TryReadObject<TSettings>(this.Path, typeof(XmlSerializer)) ?? new TSettings();
			this.Changed?.Invoke(this, EventArgs.Empty);
		}
		/// <summary>
		/// Saves the data of <typeparamref name="TSettings"/> to file.
		/// </summary>
		public void Save()
		{
			Serializer.WriteObject(this.Instance, this.Path, typeof(XmlSerializer));
		}
	}
}
