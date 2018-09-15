using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	/// <summary>
	/// Describes an interface for a <see cref="Component"/> that listens to 
	/// serialization events.
	/// </summary>
	public interface ICmpSerializeListener
	{
		/// <summary>
		/// Called when the <see cref="Component"/>, its <see cref="GameObject"/> and all related
		/// objects have been deserialized and are being initialized after load.
		/// </summary>
		void OnLoaded();
		/// <summary>
		/// Called when the <see cref="Component"/>, its <see cref="GameObject"/> and all related
		/// objects have been serialized. Useful for undoing pre-serialization operations that were
		/// done in <see cref="OnSaving"/>.
		/// </summary>
		void OnSaved();
		/// <summary>
		/// Called when the <see cref="Component"/>, its <see cref="GameObject"/> and all related
		/// objects are preparing to be serialized. Useful for running temporary pre-save operations
		/// that are undone in <see cref="OnSaved"/> after serialization completed.
		/// </summary>
		void OnSaving();
	}
}
