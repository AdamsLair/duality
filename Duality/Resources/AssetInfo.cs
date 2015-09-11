using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;


namespace Duality.Editor
{
	/// <summary>
	/// Provides information on the way a <see cref="Duality.Resource"/> should be treated during
	/// Asset import operations in the editor.
	/// This information should not be considered to be available at runtime.
	/// </summary>
	public class AssetInfo
	{
		private string importerId = null;
		private string nameHint = null;

		/// <summary>
		/// [GET / SET] The ID of the Asset Importer that was used to import this Asset. 
		/// If set, it will be automatically used for Re-Import operations.
		/// </summary>
		public string ImporterId
		{
			get { return this.importerId; }
			set { this.importerId = value; }
		}
		/// <summary>
		/// [GET / SET] For runtime-only Resources that haven't been saved or located anywhere,
		/// this property provides a hint on how to name the Resource later.
		/// </summary>
		public string NameHint
		{
			get { return this.nameHint; }
			set { this.nameHint = value; }
		}
	}
}
