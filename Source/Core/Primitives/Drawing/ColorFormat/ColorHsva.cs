using System;
using System.Runtime.InteropServices;

namespace Duality.Drawing
{
	/// <summary>
	/// Represents a 16-byte Hsva color value.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ColorHsva : IColorData, IEquatable<ColorHsva>
	{
		/// <summary>
		/// White.
		/// </summary>
		public static readonly ColorHsva White				= ColorRgba.White.ToHsva();
		/// <summary>
		/// Black.
		/// </summary>
		public static readonly ColorHsva Black				= ColorRgba.Black.ToHsva();
		
		/// <summary>
		/// Fully saturated and max-brightness red.
		/// </summary>
		public static readonly ColorHsva Red				= ColorRgba.Red.ToHsva();
		/// <summary>
		/// Fully saturated and max-brightness green.
		/// </summary>
		public static readonly ColorHsva Green				= ColorRgba.Green.ToHsva();
		/// <summary>
		/// Fully saturated and max-brightness blue.
		/// </summary>
		public static readonly ColorHsva Blue				= ColorRgba.Blue.ToHsva();
		
		/// <summary>
		/// A very light grey.
		/// </summary>
		public static readonly ColorHsva VeryLightGrey		= ColorRgba.VeryLightGrey.ToHsva();
		/// <summary>
		/// A light grey.
		/// </summary>
		public static readonly ColorHsva LightGrey			= ColorRgba.LightGrey.ToHsva();
		/// <summary>
		/// Medium grey.
		/// </summary>
		public static readonly ColorHsva Grey				= ColorRgba.Grey.ToHsva();
		/// <summary>
		/// A dark grey.
		/// </summary>
		public static readonly ColorHsva DarkGrey			= ColorRgba.DarkGrey.ToHsva();
		/// <summary>
		/// A very dark grey.
		/// </summary>
		public static readonly ColorHsva VeryDarkGrey		= ColorRgba.VeryDarkGrey.ToHsva();
		
		/// <summary>
		/// Transparent white. Completely invisible, when drawn, but might make a difference as
		/// a background color.
		/// </summary>
		public static readonly ColorHsva TransparentWhite	= ColorRgba.TransparentWhite.ToHsva();
		/// <summary>
		/// Transparent black. Completely invisible, when drawn, but might make a difference as
		/// a background color.
		/// </summary>
		public static readonly ColorHsva TransparentBlack	= ColorRgba.TransparentBlack.ToHsva();

		/// <summary>
		/// Hue component as float [0.0f - 1.0f].
		/// </summary>
		public	float	H;
		/// <summary>
		/// Saturation component as float [0.0f - 1.0f].
		/// </summary>
		public	float	S;
		/// <summary>
		/// Value component as float [0.0f - 1.0f].
		/// </summary>
		public	float	V;
		/// <summary>
		/// Alpha component as float [0.0f - 1.0f].
		/// </summary>
		public	float	A;

		/// <summary>
		/// Creates a new color based on an existing one. This is basically a copy-constructor.
		/// </summary>
		/// <param name="clr"></param>
		public ColorHsva(ColorHsva clr)
		{
			this.H = clr.H;
			this.S = clr.S;
			this.V = clr.V;
			this.A = clr.A;
		}
		/// <summary>
		/// Creates a new color.
		/// </summary>
		/// <param name="h">Hue as float [0.0f - 1.0f].</param>
		/// <param name="s">Saturation as float [0.0f - 1.0f].</param>
		/// <param name="v">Value as float [0.0f - 1.0f].</param>
		/// <param name="a">Alpha as float [0.0f - 1.0f].</param>
		public ColorHsva(float h, float s, float v, float a = 1.0f)
		{
			this.H = h;
			this.S = s;
			this.V = v;
			this.A = a;
		}
		
		/// <summary>
		/// Calculates the colors luminance. It is an approximation on how bright the color actually looks to
		/// the human eye, weighting each (Rgba) color component differently.
		/// </summary>
		/// <returns>The colors luminance as float [0.0f - 1.0f].</returns>
		public float GetLuminance()
		{
			return this.ToRgba().GetLuminance();
		}
		
		/// <summary>
		/// Returns a new version of the color with an adjusted hue component.
		/// </summary>
		/// <param name="h">The new hue component as float [0.0f - 1.0f].</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorHsva WithHue(float h)
		{
			return new ColorHsva(h, this.S, this.V, this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted saturation component.
		/// </summary>
		/// <param name="s">The new saturation component as float [0.0f - 1.0f].</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorHsva WithSaturation(float s)
		{
			return new ColorHsva(this.H, s, this.V, this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted value component.
		/// </summary>
		/// <param name="v">The new value component as float [0.0f - 1.0f].</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorHsva WithValue(float v)
		{
			return new ColorHsva(this.H, this.S, v, this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted alpha component.
		/// </summary>
		/// <param name="a">The new alpha component as float [0.0f - 1.0f].</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorHsva WithAlpha(float a)
		{
			return new ColorHsva(this.H, this.S, this.V, a);
		}

		/// <summary>
		/// Converts the color to int-Rgba.
		/// </summary>
		public int ToIntRgba()
		{
			return this.ToRgba().ToIntRgba();
		}
		/// <summary>
		/// Converts the color to int-Argb.
		/// </summary>
		public int ToIntArgb()
		{
			return this.ToRgba().ToIntArgb();
		}
		/// <summary>
		/// Converts the color to Rgba.
		/// </summary>
		public ColorRgba ToRgba()
		{
			float hTemp = NormalizeHue(this.H) * 360.0f / 60.0f;
			int hi = (int)Math.Floor(hTemp) % 6;
			float f = hTemp - (float)Math.Floor(hTemp);

			float vTemp = ClampToUnit(this.V) * 255.0f;
			float sTemp = ClampToUnit(this.S);
			byte v = (byte)vTemp;
			byte p = (byte)(vTemp * (1.0f - sTemp));
			byte q = (byte)(vTemp * (1.0f - f * sTemp));
			byte t = (byte)(vTemp * (1.0f - (1.0f - f) * sTemp));

			if (hi == 0)		return new ColorRgba(v, t, p, ColorRgba.ClampToByte(this.A * 255.0f));
			else if (hi == 1)	return new ColorRgba(q, v, p, ColorRgba.ClampToByte(this.A * 255.0f));
			else if (hi == 2)	return new ColorRgba(p, v, t, ColorRgba.ClampToByte(this.A * 255.0f));
			else if (hi == 3)	return new ColorRgba(p, q, v, ColorRgba.ClampToByte(this.A * 255.0f));
			else if (hi == 4)	return new ColorRgba(t, p, v, ColorRgba.ClampToByte(this.A * 255.0f));
			else				return new ColorRgba(v, p, q, ColorRgba.ClampToByte(this.A * 255.0f));
		}
		
		/// <summary>
		/// Adjusts the color to match the specified int-Rgba color.
		/// </summary>
		/// <param name="rgba"></param>
		public void SetIntRgba(int rgba)
		{
			this.SetRgba(ColorRgba.FromIntRgba(rgba));
		}
		/// <summary>
		/// Adjusts the color to match the specified int-Argb color.
		/// </summary>
		/// <param name="argb"></param>
		public void SetIntArgb(int argb)
		{
			this.SetRgba(ColorRgba.FromIntArgb(argb));			
		}
		/// <summary>
		/// Adjusts the color to match the specified Rgba color.
		/// </summary>
		/// <param name="rgba"></param>
		public void SetRgba(ColorRgba rgba)
		{
			float	min		= Math.Min(Math.Min(rgba.R, rgba.G), rgba.B);
			float	max		= Math.Max(Math.Max(rgba.R, rgba.G), rgba.B);
			float	delta	= max - min;
			
			if (max > 0.0f)
			{
				this.S = delta / max;
				this.V = max / 255.0f;

				int maxInt = (int)Math.Round(max);
				if (delta != 0.0f)
				{
					if ((int)Math.Round((float)rgba.R) == maxInt)
					{
						this.H = (float)(rgba.G - rgba.B) / delta;
					}
					else if ((int)Math.Round((float)rgba.G) == maxInt)
					{
						this.H = 2.0f + (float)(rgba.B - rgba.R) / delta;
					}
					else
					{
						this.H = 4.0f + (float)(rgba.R - rgba.G) / delta;
					}
					this.H *= 60.0f;
					if (this.H < 0.0f) this.H += 360.0f;
				}
				else
				{
					this.H = 0.0f;
				}
			}
			else
			{
				this.H = 0.0f;
				this.S = 0.0f;
				this.V = 0.0f;
			}

			this.H /= 360.0f;
			this.A = (float)rgba.A / 255.0f;
		}
		
		/// <summary>
		/// Returns whether this color equals the specified one.
		/// </summary>
		/// <param name="other"></param>
		public bool Equals(ColorHsva other)
		{
			return this.H == other.H && this.S == other.S && this.V == other.V && this.A == other.A;
		}
		public override bool Equals(object obj)
		{
			if (!(obj is ColorHsva))
				return false;
			else
				return this.Equals((ColorHsva)obj);
		}
		public override int GetHashCode()
		{
			return (int)this.ToIntRgba();
		}
		public override string ToString()
		{
			return string.Format("HSVA ({0:F}, {1:F}, {2:F}, {3:F})", this.H, this.S, this.V, this.A);
		}
		
		/// <summary>
		/// Creates a new color based on an int-Rgba value.
		/// </summary>
		/// <param name="rgba"></param>
		public static ColorHsva FromIntRgba(int rgba)
		{
			ColorHsva temp = new ColorHsva();
			temp.SetIntRgba(rgba);
			return temp;
		}
		/// <summary>
		/// Creates a new color based on an int-Argb value.
		/// </summary>
		/// <param name="argb"></param>
		public static ColorHsva FromIntArgb(int argb)
		{
			ColorHsva temp = new ColorHsva();
			temp.SetIntArgb(argb);
			return temp;
		}
		/// <summary>
		/// Creates a new color based on a Rgba value.
		/// </summary>
		/// <param name="rgba"></param>
		public static ColorHsva FromRgba(ColorRgba rgba)
		{
			ColorHsva temp = new ColorHsva();
			temp.SetRgba(rgba);
			return temp;
		}
		
		/// <summary>
		/// Returns whether two colors are equal.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		public static bool operator ==(ColorHsva left, ColorHsva right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Returns whether two colors are unequal.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		public static bool operator !=(ColorHsva left, ColorHsva right)
		{
			return !left.Equals(right);
		}

		public static explicit operator ColorHsva(int c)
		{
			return FromIntRgba(c);
		}
		public static explicit operator ColorHsva(ColorRgba c)
		{
			return FromRgba(c);
		}
		public static explicit operator int(ColorHsva c)
		{
			return c.ToIntRgba();
		}
		public static explicit operator ColorRgba(ColorHsva c)
		{
			return c.ToRgba();
		}

		internal static float ClampToUnit(float value)
		{
			return (float)Math.Min(Math.Max(value, 0.0f), 1.0f);
		}
		private static float NormalizeHue(float var)
		{
			if (var >= 0.0f && var < 1.0f) return var;

			if (var < 0.0f)
				var = 1.0f + (var % 1.0f);
			else
				var = var % 1.0f;

			return var;
		}
	}
}
