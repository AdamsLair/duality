using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend
{
	public class WindowOptions
	{
		private Point2      size         = new Point2(800, 600);
		private ScreenMode  screenMode   = ScreenMode.Window;
		private RefreshMode refreshMode  = RefreshMode.VSync;
		private string      title        = "Duality Window";
		private bool        systemCursor = true;

		public Point2 Size
		{
			get { return this.size; }
			set { this.size = value; }
		}
		public ScreenMode ScreenMode
		{
			get { return this.screenMode; }
			set { this.screenMode = value; }
		}
		public RefreshMode RefreshMode
		{
			get { return this.refreshMode; }
			set { this.refreshMode = value; }
		}
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}
		public bool SystemCursorVisible
		{
			get { return this.systemCursor; }
			set { this.systemCursor = value; }
		}
	}
}
