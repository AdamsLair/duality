using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Resources;
using Duality.Backend;

namespace Duality.Drawing
{
	public struct VertexDrawRange
	{
		public int Index;
		public int Count;

		public override string ToString()
		{
			return string.Format(
				"[{0} - {1}]", 
				this.Index, 
				this.Index + this.Count - 1);
		}
	}
}
