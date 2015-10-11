using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Backend;


namespace Duality.Resources
{
	/// <summary>
	/// Manages Duality's builtin shader variables.
	/// </summary>
	public static class BuiltinShaderFields
	{
		public const int InvalidIndex = -1;

		/// <summary>
		/// Determines whether a certain <see cref="ShaderFieldInfo"/> name refers to a builtin shader variable
		/// and returns a unique index to retrieve that variables value.
		/// </summary>
		public static int GetIndex(string fieldName)
		{
			switch (fieldName)
			{
				// Per-Frame Variables (Globals)
				case "RealTime":        return 0;
				case "GameTime":        return 1;
				case "FrameCount":      return 2;

				// Per-DrawDevice Variables
				case "CameraFocusDist": return 3;
				case "CameraPosition":  return 4;
				case "CameraParallax":  return 5;
			}

			return InvalidIndex;
		}
		/// <summary>
		/// Retrieves the value of a builtin shader variable using the index retrieved by <see cref="GetIndex"/>.
		/// </summary>
		/// <param name="currentDevice"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool TryGetValue(IDrawDevice currentDevice, int index, ref float[] value)
		{
			int size = 1;
			switch (index)
			{
				case InvalidIndex: return false;
				default: size = 1; break;
				case 4:  size = 3; break;
			}

			value = (value != null && value.Length == size) ? value : new float[size];
			switch (index)
			{
				case 0: value[0] = (float)Time.MainTimer.TotalSeconds; return true;
				case 1: value[0] = (float)Time.GameTimer.TotalSeconds; return true;
				case 2: value[0] = (float)Time.FrameCount;             return true;
				case 3: value[0] = currentDevice.FocusDist;            return true;
				case 4: value[0] = currentDevice.RefCoord.X;
				        value[1] = currentDevice.RefCoord.Y;
				        value[2] = currentDevice.RefCoord.Z;           return true;
				case 5: value[0] = currentDevice.Perspective == PerspectiveMode.Parallax ? 1.0f : 0.0f; return true;
			}

			return false;
		}
	}
}
