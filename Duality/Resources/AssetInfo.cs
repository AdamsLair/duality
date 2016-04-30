using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;


namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Provides information on the way a <see cref="Duality.Resource"/> should be treated during
	/// Asset import operations in the editor.
	/// 
	/// This information should not be considered to be available at runtime or after deploying a
	/// game without editor support.
	/// </summary>
	public class AssetInfo
	{
		internal static readonly string FileHintNameVariable = "{Name}";

		private string importerId = null;
		private string[] sourceFileHint = null;
		[DontSerialize] private string nameHint = null;

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
		/// [GET / SET] When set, this array provides a hint to the AssetManagement system
		/// which source files were used to create this <see cref="Resource"/> during the most recent 
		/// import operation. The paths are relative to the mapped media source directory of the
		/// <see cref="Resource"/> and can use the {Name} variable to keep paths invariant to
		/// move and rename operations.
		/// </summary>
		public string[] SourceFileHint
		{
			get { return this.sourceFileHint; }
			set { this.sourceFileHint = value; }
		}
		/// <summary>
		/// [GET / SET] For runtime-only Resources that haven't been saved or located anywhere,
		/// this property provides a hint on how to name the Resource later. This property is considered
		/// temporary and won't be serialized.
		/// </summary>
		public string NameHint
		{
			get { return this.nameHint; }
			set { this.nameHint = value; }
		}
	}
}
