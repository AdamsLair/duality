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
		private Func<TSettings> loadDelegate = null;
		private Action<TSettings> saveDelegate = null;


		/// <summary>
		/// The path of the file where the settings are loaded from and saved persistently.
		/// </summary>
		public string Path { get; }
		/// <summary>
		/// The settings data. Will equal null until <see cref="Load"/> is called for the first time, 
		/// after which it can never be null again.
		/// </summary>
		public TSettings Instance { get; private set; }


		/// <summary>
		/// Fired when the settings <see cref="Instance"/> is being applied after loading or modification.
		/// </summary>
		public event EventHandler Applying;
		/// <summary>
		/// Fired when the settings <see cref="Instance"/> is about to be saved, allowing subscribers to
		/// extend it with previously missing or updated information.
		/// </summary>
		public event EventHandler Saving;


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
		/// Creates a new settings container with specialized load and save routines.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="loadDelegate"></param>
		/// <param name="saveDelegate"></param>
		public SettingsContainer(string path, Func<TSettings> loadDelegate, Action<TSettings> saveDelegate)
		{
			this.Path = path;
			this.loadDelegate = loadDelegate;
			this.saveDelegate = saveDelegate;
		}

		/// <summary>
		/// Loads the data of <typeparamref name="TSettings"/> from file.
		/// </summary>
		public void Load()
		{
			if (this.loadDelegate != null)
			{
				this.Instance = this.loadDelegate() ?? new TSettings();
			}
			else
			{
				this.Instance = Serializer.TryReadObject<TSettings>(this.Path, typeof(XmlSerializer)) ?? new TSettings();
			}
			this.Applying?.Invoke(this, EventArgs.Empty);
		}
		/// <summary>
		/// Saves the data of <typeparamref name="TSettings"/> to file.
		/// </summary>
		public void Save()
		{
			this.Saving?.Invoke(this, EventArgs.Empty);
			if (this.saveDelegate != null)
			{
				this.saveDelegate(this.Instance);
			}
			else
			{
				Serializer.WriteObject(this.Instance, this.Path, typeof(XmlSerializer));
			}
		}
		/// <summary>
		/// Ensures that any modifications to the settings <see cref="Instance"/> are applied to 
		/// all systems relying on them. 
		/// 
		/// Note that trivial settings values such as audio volume or similar may be applied directly
		/// as values are changed even before <see cref="Apply"/> is called.
		/// </summary>
		public void Apply()
		{
			this.Applying?.Invoke(this, EventArgs.Empty);
		}
	}
}
