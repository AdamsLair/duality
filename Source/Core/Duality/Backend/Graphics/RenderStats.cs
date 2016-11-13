using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public class RenderStats
	{
		private int drawcalls = 0;

		public int DrawCalls
		{
			get { return this.drawcalls; }
			set { this.drawcalls = value; }
		}
	}
}
