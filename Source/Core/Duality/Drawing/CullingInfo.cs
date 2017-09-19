using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Resources;
using Duality.Backend;

namespace Duality.Drawing
{
	public struct CullingInfo
	{
		public Vector3 Position;
		public float Radius;
		public VisibilityFlag Visibility;
	}
}
