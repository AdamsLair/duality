using System.Collections;
using System.Collections.Generic;

namespace Duality.Drawing
{
	public struct ScreenResolution
	{
		private int width;
		private int height;
		private float refreshRate;

		public int Width
		{
			get { return this.width; }
		}
		public int Height
		{
			get { return this.height; }
		}
		public float RefreshRate
		{
			get { return this.refreshRate; }
		}

		public ScreenResolution(int width, int height, float refreshRate)
		{
			this.width = width;
			this.height = height;
			this.refreshRate = refreshRate;
		}
	}
}
