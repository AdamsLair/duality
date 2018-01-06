using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Backend;


namespace Duality.Resources
{
	/// <summary>
	/// A static list of names for Duality's builtin shader variables.
	/// </summary>
	public static class BuiltinShaderFields
	{
		public static readonly string DeltaTime = "_DeltaTime";
		public static readonly string RealTime = "_RealTime";
		public static readonly string GameTime = "_GameTime";
		public static readonly string FrameCount = "_FrameCount";

		public static readonly string CameraFocusDist = "_CameraFocusDist";
		public static readonly string CameraPosition = "_CameraPosition";
		public static readonly string CameraIsPerspective = "_CameraIsPerspective";
	}
}
