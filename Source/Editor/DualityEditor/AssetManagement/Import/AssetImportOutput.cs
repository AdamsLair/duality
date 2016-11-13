using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Represents a single result item in an Asset import or re-import operation.
	/// </summary>
	public struct AssetImportOutput
	{
		private ContentRef<Resource> resource;
		private string[] inputPaths;

		/// <summary>
		/// [GET] A reference to the generated output <see cref="Duality.Resource"/>.
		/// </summary>
		public ContentRef<Resource> Resource
		{
			get { return this.resource; }
		}
		/// <summary>
		/// [GET] The set of input paths that was used to generate this output.
		/// </summary>
		public IReadOnlyList<string> InputPaths
		{
			get { return this.inputPaths; }
		}

		public AssetImportOutput(ContentRef<Resource> resource, IEnumerable<string> inputPaths)
		{
			this.resource = resource;
			this.inputPaths = inputPaths.ToArray();
		}

		public override string ToString()
		{
			return string.Format("{0} [{1} input files]", this.resource, this.inputPaths.Length);
		}
	}
}
