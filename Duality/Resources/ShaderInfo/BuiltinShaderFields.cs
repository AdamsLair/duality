using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;


namespace Duality.Resources
{
	/// <summary>
	/// Manages Duality's builtin shader variables.
	/// </summary>
	public static class BuiltinShaderFields
	{
		public static readonly int InvalidIndex = -1;

		/// <summary>
		/// Determines whether a certain <see cref="ShaderFieldInfo"/> name refers to a builtin shader variable
		/// and returns a unique index to retrieve that variables value.
		/// </summary>
		public static int GetIndex(string fieldName)
		{
			switch (fieldName)
			{
				case "RealTime":   return 0;
				case "GameTime":   return 1;
				case "FrameCount": return 2;
			}

			return InvalidIndex;
		}
		/// <summary>
		/// Retrieves the value of a builtin shader variable using the index retrieved by <see cref="GetIndex"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool TryGetValue(int index, out float value)
		{
			switch (index)
			{
				case 0: value = (float)Time.MainTimer.TotalSeconds; return true;
				case 1: value = (float)Time.GameTimer.TotalSeconds; return true;
				case 2: value = (float)Time.FrameCount;             return true;
			}

			value = 0.0f;
			return false;
		}
	}
}
