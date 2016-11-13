using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	public class PreviewImageQuery : PreviewQuery<Bitmap>
	{
		public int DesiredWidth { get; private set; }
		public int DesiredHeight { get; private set; }
		public PreviewSizeMode SizeMode { get; private set; }

		public PreviewImageQuery(object src, int desiredWidth, int desiredHeight, PreviewSizeMode mode) : base(src)
		{
			this.DesiredWidth = desiredWidth;
			this.DesiredHeight = desiredHeight;
			this.SizeMode = mode;
		}
	}
}
