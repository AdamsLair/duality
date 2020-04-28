﻿using System;

namespace Duality.Drawing
{
	/// <summary>
	/// A general interface for different types of color data.
	/// </summary>
	public interface IColorData
	{
		/// <summary>
		/// Converts the color to a <see cref="System.UInt32"/>-Rgba value.
		/// </summary>
		int ToIntRgba();
		/// <summary>
		/// Sets the color base ond a <see cref="System.UInt32"/>-Rgba value.
		/// </summary>
		/// <param name="rgba"></param>
		void SetIntRgba(int rgba);
		
		/// <summary>
		/// Converts the color to a <see cref="System.UInt32"/>-Argb value.
		/// </summary>
		int ToIntArgb();
		/// <summary>
		/// Sets the color base ond a <see cref="System.UInt32"/>-Argb value.
		/// </summary>
		/// <param name="argb"></param>
		void SetIntArgb(int argb);
	}
}
