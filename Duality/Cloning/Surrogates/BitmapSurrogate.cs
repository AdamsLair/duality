using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Drawing;

namespace Duality.Cloning.Surrogates
{
	public class BitmapSurrogate : CloneSurrogate<Bitmap>
	{
		public override void CreateTargetObject(Bitmap source, out Bitmap target, ICloneTargetSetup setup)
		{
			target = new Bitmap(source.Width, source.Height);
		}
		public override void SetupCloneTargets(Bitmap source, ICloneTargetSetup setup) {}
		public override void CopyDataTo(Bitmap source, Bitmap target, ICloneOperation operation)
		{
			target.SetPixelDataIntArgb(source.GetPixelDataIntArgb());
		}
	}
}
