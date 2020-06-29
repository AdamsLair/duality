using System;
using Duality.Serialization;

namespace Duality
{
	/// <summary>
	/// A container class to abstract all serialization logic away from settings classes.
	/// </summary>
	/// <typeparam name="TSettings"></typeparam>
	public class SettingsContainer<TSettings> : ISettingsContainer
		where TSettings : class, new()
	{
		/// <summary>
		/// Fired when <see cref="Value"/> has changed.
		/// </summary>
		public event EventHandler Changed;

		/// <summary>
		/// The settings data. This is null till <see cref="Load"/> is called after which it can never be null again.
		/// </summary>
		public TSettings Value { get; private set; }

		private readonly string path;

		/// <summary>
		/// Creates a new settings container where the data for the settings will be saved and loaded from <paramref name="path"/>.
		/// </summary>
		/// <param name="path"></param>
		public SettingsContainer(string path)
		{
			this.path = path;
		}

		/// <summary>
		/// Loads the data of <typeparamref name="TSettings"/> from file.
		/// </summary>
		public void Load()
		{
			this.Value = Serializer.TryReadObject<TSettings>(this.path) ?? new TSettings();
			Changed?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Saves the data of <typeparamref name="TSettings"/> to file.
		/// </summary>
		public void Save()
		{
			Serializer.WriteObject(this.Value, this.path, typeof(XmlSerializer));
		}
	}
}
