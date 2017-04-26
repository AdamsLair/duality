using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Provides information about a finished export operation.
	/// </summary>
	public class AssetExportFinishedEventArgs : EventArgs
	{
		private bool	 success = false;
		private Resource input   = null;
		private string[] output  = null;

		/// <summary>
		/// [GET] Whether the operation was successful.
		/// </summary>
		public bool IsSuccessful
		{
			get { return this.success; }
		}
		/// <summary>
		/// [GET] The <see cref="Resource"/> that was exported.
		/// </summary>
		public Resource Input
		{
			get { return this.input; }
		}
		/// <summary>
		/// [GET] The list of output source files that were exported.
		/// </summary>
		public IReadOnlyList<string> Output
		{
			get { return this.output; }
		}

		public AssetExportFinishedEventArgs(bool isSuccess, Resource input, IEnumerable<string> output)
		{
			this.success = isSuccess;
			this.input = input;
			this.output = output.ToArray();
		}
	}
}
