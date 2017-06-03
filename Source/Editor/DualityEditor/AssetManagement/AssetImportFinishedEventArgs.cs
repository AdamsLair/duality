using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Provides information about a finished import operation.
	/// </summary>
	public class AssetImportFinishedEventArgs : EventArgs
	{
		private bool                success  = false;
		private bool                reImport = false;
		private AssetImportInput[]  input    = null;
		private AssetImportOutput[] output   = null;

		/// <summary>
		/// [GET] Whether the operation was successful.
		/// </summary>
		public bool IsSuccessful
		{
			get { return this.success; }
		}
		/// <summary>
		/// [GET] Whether this was a re-import of an existing <see cref="Resource"/>.
		/// </summary>
		public bool IsReImport
		{
			get { return this.reImport; }
		}
		/// <summary>
		/// [GET] The list of input source files that were used as a basis for the operation.
		/// </summary>
		public IReadOnlyList<AssetImportInput> Input
		{
			get { return this.input; }
		}
		/// <summary>
		/// [GET] The list of output Resources that were imported, along with the source files that each of them used.
		/// </summary>
		public IReadOnlyList<AssetImportOutput> Output
		{
			get { return this.output; }
		}

		public AssetImportFinishedEventArgs(bool isSuccess, bool isReImport, IEnumerable<AssetImportInput> input, IEnumerable<AssetImportOutput> output)
		{
			this.success = isSuccess;
			this.reImport = isReImport;
			this.input = input.ToArray();
			this.output = output.ToArray();
		}
	}
}
