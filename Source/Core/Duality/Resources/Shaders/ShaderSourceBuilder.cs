using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Duality.Resources
{
	public class ShaderSourceBuilder
	{
		private string mainChunk = string.Empty;
		private List<string> sharedChunk = new List<string>();

		public void Clear()
		{
			this.mainChunk = string.Empty;
			this.sharedChunk.Clear();
		}
		public void SetMainChunk(string sourceCode)
		{
			this.mainChunk = sourceCode ?? string.Empty;
		}
		public void AddSharedChunk(string sourceCode)
		{
			this.sharedChunk.Add(sourceCode);
		}

		public string Build()
		{
			// ToDo
			throw new NotImplementedException();
		}
	}
}
