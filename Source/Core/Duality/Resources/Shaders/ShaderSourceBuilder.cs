using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Duality.Resources
{
	public class ShaderSourceBuilder
	{
		private string mainChunk = string.Empty;
		private List<string> sharedChunk = new List<string>();

		private StringBuilder textBuilder = new StringBuilder();


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
			this.textBuilder.Clear();

			// ToDo: Automatically comment out redefined uniforms
			// ToDo: Make sure the #version directive is moved to the top of the result

			for (int i = 0; i < this.sharedChunk.Count; i++)
			{
				this.textBuilder.AppendFormat("#line {0}", (i + 1) * 10000);
				this.textBuilder.AppendLine();
				this.textBuilder.Append(this.sharedChunk[i]);
				this.textBuilder.AppendLine();
			}

			this.textBuilder.AppendLine("#line 1");
			this.textBuilder.Append(this.mainChunk);

			return this.textBuilder.ToString();
		}
	}
}
