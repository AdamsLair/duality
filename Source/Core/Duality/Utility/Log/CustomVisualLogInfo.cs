using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality
{
	public abstract class CustomVisualLogInfo
	{
		private string name;
		private ColorRgba baseColor;

		public string Name
		{
			get { return this.name; }
		}
		public ColorRgba BaseColor
		{
			get { return this.baseColor; }
		}

		public CustomVisualLogInfo(string name, ColorRgba baseColor)
		{
			this.name = name;
			this.baseColor = baseColor;
		}
	}
}
