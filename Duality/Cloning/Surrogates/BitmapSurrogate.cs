using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Drawing;

namespace Duality.Cloning.Surrogates
{
	public class BitmapSurrogate : Surrogate<Bitmap>
	{
		public override Bitmap CreateTargetObject(CloneProvider provider)
		{
			return new Bitmap(this.RealObject.Width, this.RealObject.Height);
		}
		public override void CopyDataTo(Bitmap targetObj, CloneProvider provider)
		{
			Bitmap target = targetObj as Bitmap;
			target.SetPixelDataIntArgb(this.RealObject.GetPixelDataIntArgb());
		}
	}
}
