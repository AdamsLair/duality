using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Resources;

namespace Duality.Backend
{
	public class WindowOptions
	{
		private int				width			= 800;
		private int				height			= 600;
		private ScreenMode		screenMode		= ScreenMode.Window;
		private RefreshMode		refreshMode		= RefreshMode.VSync;
		private string			title			= "Duality Window";
		private bool			systemCursor	= true;

		public int Width
		{
			get { return this.width; }
			set { this.width = value; }
		}
		public int Height
		{
			get { return this.height; }
			set { this.height = value; }
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
