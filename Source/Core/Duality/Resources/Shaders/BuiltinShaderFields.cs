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
		public static readonly string DeltaTime = "_deltaTime";
		public static readonly string RealTime = "_realTime";
		public static readonly string GameTime = "_gameTime";
		public static readonly string FrameCount = "_frameCount";

		public static readonly string CameraFocusDist = "_cameraFocusDist";
		public static readonly string CameraPosition = "_cameraPosition";
		public static readonly string CameraIsPerspective = "_cameraIsPerspective";

		public static readonly string ViewMatrix = "_viewMatrix";
		public static readonly string ProjectionMatrix = "_projectionMatrix";
		public static readonly string ViewProjectionMatrix = "_viewProjectionMatrix";

		public static readonly string MainTex = "mainTex";
		public static readonly string MainColor = "mainColor";
	}
}
