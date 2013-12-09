using System;
using System.Runtime.InteropServices;

namespace Duality.ColorFormat
{
	/// <summary>
	/// Represents a 4-byte Rgba color value.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct ColorRgba : IColorData, IEquatable<ColorRgba>
	{
		/// <summary>
		/// Size of a single color component in bytes.
		/// </summary>
		public const int CompSize	= sizeof(byte);
		/// <summary>
		/// Byte offset of the red value.
		/// </summary>
		public const int OffsetR	= 0;
		/// <summary>
		/// Byte offset of the green value.
		/// </summary>
		public const int OffsetG	= OffsetR + CompSize;
		/// <summary>
		/// Byte offset of the blue value.
		/// </summary>
		public const int OffsetB	= OffsetG + CompSize;
		/// <summary>
		/// Byte offset of the alpha value.
		/// </summary>
		public const int OffsetA	= OffsetB + CompSize;
		/// <summary>
		/// Total size of the struct in bytes.
		/// </summary>
		public const int Size		= OffsetA + CompSize;

		/// <summary>
		/// White.
		/// </summary>
		public static readonly ColorRgba White				= new ColorRgba(255,	255,	255);
		/// <summary>
		/// Black.
		/// </summary>
		public static readonly ColorRgba Black				= new ColorRgba(0,		0,		0);

		/// <summary>
		/// Fully saturated and max-brightness red. Also known as [255,0,0].
		/// </summary>
		public static readonly ColorRgba Red				= new ColorRgba(255,	0,		0);
		/// <summary>
		/// Fully saturated and max-brightness green. Also known as [0,255,0].
		/// </summary>
		public static readonly ColorRgba Green				= new ColorRgba(0,		255,	0);
		/// <summary>
		/// Fully saturated and max-brightness blue. Also known as [0,0,255].
		/// </summary>
		public static readonly ColorRgba Blue				= new ColorRgba(0,		0,		255);

		/// <summary>
		/// A very light grey. Value: 224.
		/// </summary>
		public static readonly ColorRgba VeryLightGrey		= new ColorRgba(224,	224,	224);
		/// <summary>
		/// A light grey. Value: 192.
		/// </summary>
		public static readonly ColorRgba LightGrey			= new ColorRgba(192,	192,	192);
		/// <summary>
		/// Medium grey. Value: 128.
		/// </summary>
		public static readonly ColorRgba Grey				= new ColorRgba(128,	128,	128);
		/// <summary>
		/// A dark grey. Value: 64.
		/// </summary>
		public static readonly ColorRgba DarkGrey			= new ColorRgba(64,		64,		64);
		/// <summary>
		/// A very dark grey. Value: 32.
		/// </summary>
		public static readonly ColorRgba VeryDarkGrey		= new ColorRgba(32,		32,		32);

		/// <summary>
		/// Transparent white. Completely invisible, when drawn, but might make a difference as
		/// a background color.
		/// </summary>
		public static readonly ColorRgba TransparentWhite	= new ColorRgba(255,	255,	255,	0);
		/// <summary>
		/// Transparent black. Completely invisible, when drawn, but might make a difference as
		/// a background color.
		/// </summary>
		public static readonly ColorRgba TransparentBlack	= new ColorRgba(0,		0,		0,		0);

		/// <summary>
		/// Red color component.
		/// </summary>
		public	byte	R;
		/// <summary>
		/// Green color component.
		/// </summary>
		public	byte	G;
		/// <summary>
		/// Blue color component.
		/// </summary>
		public	byte	B;
		/// <summary>
		/// Alpha color component. Usually treated as opacity.
		/// </summary>
		public	byte	A;

		/// <summary>
		/// Creates a new color based on an existing one. This is basically a copy-constructor.
		/// </summary>
		/// <param name="clr"></param>
		public ColorRgba(ColorRgba clr)
		{
			this.R = clr.R;
			this.G = clr.G;
			this.B = clr.B;
			this.A = clr.A;
		}
		/// <summary>
		/// Creates a new color based on an int-Rgba value.
		/// </summary>
		/// <param name="rgba"></param>
		public ColorRgba(int rgba)
		{
			this.R = (byte)((rgba & 0xFF000000) >> 24);
			this.G = (byte)((rgba & 0x00FF0000) >> 16);
			this.B = (byte)((rgba & 0x0000FF00) >> 8);
			this.A = (byte)(rgba & 0x000000FF);
		}
		/// <summary>
		/// Creates a new color.
		/// </summary>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="a">The alpha component.</param>
		public ColorRgba(byte r, byte g, byte b, byte a = 255)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}
		/// <summary>
		/// Creates a new color based on value (brightness) and alpha.
		/// </summary>
		/// <param name="value">The value / brightness of the color.</param>
		/// <param name="a">The colors alpha value.</param>
		public ColorRgba(byte value, byte a = 255)
		{
			this.R = value;
			this.G = value;
			this.B = value;
			this.A = a;
		}
		/// <summary>
		/// Creates a new color.
		/// </summary>
		/// <param name="r">The red component as float [0.0f - 1.0f].</param>
		/// <param name="g">The green component as float [0.0f - 1.0f].</param>
		/// <param name="b">The blue component as float [0.0f - 1.0f].</param>
		/// <param name="a">The alpha component as float [0.0f - 1.0f].</param>
		public ColorRgba(float r, float g, float b, float a = 1.0f)
		{
			this.R = (byte)MathF.Clamp((int)(r * 255.0f), 0, 255);
			this.G = (byte)MathF.Clamp((int)(g * 255.0f), 0, 255);
			this.B = (byte)MathF.Clamp((int)(b * 255.0f), 0, 255);
			this.A = (byte)MathF.Clamp((int)(a * 255.0f), 0, 255);
		}
		/// <summary>
		/// Creates a new color based on value (brightness) and alpha.
		/// </summary>
		/// <param name="value">The value / brightness of the color as float [0.0f - 1.0f].</param>
		/// <param name="a">The colors alpha value as float [0.0f - 1.0f].</param>
		public ColorRgba(float value, float a = 1.0f)
		{
			this.R = (byte)MathF.Clamp((int)(value * 255.0f), 0, 255);
			this.G = this.R;
			this.B = this.R;
			this.A = (byte)MathF.Clamp((int)(a * 255.0f), 0, 255);
		}
		
		/// <summary>
		/// Returns a new version of the color with an adjusted red component.
		/// </summary>
		/// <param name="r">The new red component.</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorRgba WithRed(byte r)
		{
			return new ColorRgba(r, this.G, this.B, this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted green component.
		/// </summary>
		/// <param name="g">The new green component.</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorRgba WithGreen(byte g)
		{
			return new ColorRgba(this.R, g, this.B, this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted blue component.
		/// </summary>
		/// <param name="b">The new blue component.</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorRgba WithBlue(byte b)
		{
			return new ColorRgba(this.R, this.G, b, this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted alpha component.
		/// </summary>
		/// <param name="a">The new alpha component.</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorRgba WithAlpha(byte a)
		{
			return new ColorRgba(this.R, this.G, this.B, a);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted red component.
		/// </summary>
		/// <param name="r">The new red component as float [0.0f - 1.0f].</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorRgba WithRed(float r)
		{
			return new ColorRgba((byte)MathF.Clamp((int)(r * 255.0f), 0, 255), this.G, this.B, this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted green component.
		/// </summary>
		/// <param name="g">The new green component as float [0.0f - 1.0f].</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorRgba WithGreen(float g)
		{
			return new ColorRgba(this.R, (byte)MathF.Clamp((int)(g * 255.0f), 0, 255), this.B, this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted blue component.
		/// </summary>
		/// <param name="b">The new blue component as float [0.0f - 1.0f].</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorRgba WithBlue(float b)
		{
			return new ColorRgba(this.R, this.G, (byte)MathF.Clamp((int)(b * 255.0f), 0, 255), this.A);
		}
		/// <summary>
		/// Returns a new version of the color with an adjusted alpha component.
		/// </summary>
		/// <param name="a">The new alpha component as float [0.0f - 1.0f].</param>
		/// <returns>A new color with the specified adjustments.</returns>
		public ColorRgba WithAlpha(float a)
		{
			return new ColorRgba(this.R, this.G, this.B, (byte)MathF.Clamp((int)(a * 255.0f), 0, 255));
		}

		/// <summary>
		/// Calculates the colors luminance. It is an approximation on how bright the color actually looks to
		/// the human eye, weighting each color component differently.
		/// </summary>
		/// <returns>The colors luminance as float [0.0f - 1.0f].</returns>
		public float GetLuminance()
		{
			return (0.2126f * this.R + 0.7152f * this.G + 0.0722f * this.B) / 255.0f;
		}

		/// <summary>
		/// Converts the color to int-Rgba.
		/// </summary>
		/// <returns></returns>
		public int ToIntRgba()
		{
			return ((int)this.R << 24) | ((int)this.G << 16) | ((int)this.B << 8) | ((int)this.A);
		}
		/// <summary>
		/// Converts the color to int-Argb.
		/// </summary>
		/// <returns></returns>
		public int ToIntArgb()
		{
			return ((int)this.A << 24) | ((int)this.R << 16) | ((int)this.G << 8) | ((int)this.B);
		}
		/// <summary>
		/// Converts the color to Hsva.
		/// </summary>
		/// <returns></returns>
		public ColorHsva ToHsva()
		{
			return ColorHsva.FromRgba(this);
		}

		/// <summary>
		/// Adjusts the color to match the specified int-Argb color.
		/// </summary>
		/// <param name="argb"></param>
		public void SetIntArgb(int argb)
		{
			this.A = (byte)((argb & 0xFF000000) >> 24);
			this.R = (byte)((argb & 0x00FF0000) >> 16);
			this.G = (byte)((argb & 0x0000FF00) >> 8);
			this.B = (byte)(argb & 0x000000FF);
		}
		/// <summary>
		/// Adjusts the color to match the specified int-Rgba color.
		/// </summary>
		/// <param name="rgba"></param>
		public void SetIntRgba(int rgba)
		{
			this.R = (byte)((rgba & 0xFF000000) >> 24);
			this.G = (byte)((rgba & 0x00FF0000) >> 16);
			this.B = (byte)((rgba & 0x0000FF00) >> 8);
			this.A = (byte)(rgba & 0x000000FF);
		}
		/// <summary>
		/// Adjusts the color to match the specified Hsva color.
		/// </summary>
		/// <param name="hsva"></param>
		public void SetHsva(ColorHsva hsva)
		{
			this = hsva.ToRgba();
		}

		/// <summary>
		/// Returns whether this color equals the specified one.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ColorRgba other)
		{
			return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
		}
		public override bool Equals(object obj)
		{
			if (!(obj is ColorRgba))
				return false;
			else
				return this.Equals((ColorRgba)obj);
		}
		public override int GetHashCode()
		{
			return this.ToIntRgba();
		}
		public override string ToString()
		{
			return string.Format("ColorRGBA ({0}, {1}, {2}, {3} / #{4:X8})", this.R, this.G, this.B, this.A, this.ToIntRgba());
		}

		/// <summary>
		/// Creates a new color based on an int-Rgba value.
		/// </summary>
		/// <param name="rgba"></param>
		/// <returns></returns>
		public static ColorRgba FromIntRgba(int rgba)
		{
			ColorRgba temp = new ColorRgba();
			temp.SetIntRgba(rgba);
			return temp;
		}
		/// <summary>
		/// Creates a new color based on an int-Argb value.
		/// </summary>
		/// <param name="argb"></param>
		/// <returns></returns>
		public static ColorRgba FromIntArgb(int argb)
		{
			ColorRgba temp = new ColorRgba();
			temp.SetIntArgb(argb);
			return temp;
		}
		/// <summary>
		/// Creates a new color based on a Hsva value.
		/// </summary>
		/// <param name="hsva"></param>
		/// <returns></returns>
		public static ColorRgba FromHsva(ColorHsva hsva)
		{
			return hsva.ToRgba();
		}

		/// <summary>
		/// Mixes two colors by performing a linear interpolation between both.
		/// </summary>
		/// <param name="first">The first color.</param>
		/// <param name="second">The second color.</param>
		/// <param name="factor">The linear interpolation value. Zero equals the first color, one equals the second color.</param>
		/// <returns>The interpolated / mixed color.</returns>
		public static ColorRgba Lerp(ColorRgba first, ColorRgba second, float factor)
		{
			float invFactor = 1.0f - factor;
			return new ColorRgba(
				(byte)MathF.Clamp(MathF.Round(first.R * invFactor + second.R * factor), 0.0f, 255.0f),
				(byte)MathF.Clamp(MathF.Round(first.G * invFactor + second.G * factor), 0.0f, 255.0f),
				(byte)MathF.Clamp(MathF.Round(first.B * invFactor + second.B * factor), 0.0f, 255.0f),
				(byte)MathF.Clamp(MathF.Round(first.A * invFactor + second.A * factor), 0.0f, 255.0f));
		}

		/// <summary>
		/// Returns whether two colors are equal.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		/// <returns></returns>
		public static bool operator ==(ColorRgba left, ColorRgba right)
        {
            return left.Equals(right);
        }
		/// <summary>
		/// Returns whether two colors are unequal.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		/// <returns></returns>
		public static bool operator !=(ColorRgba left, ColorRgba right)
        {
            return !left.Equals(right);
        }
		/// <summary>
		/// Adds two colors component-wise.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		/// <returns></returns>
		public static ColorRgba operator +(ColorRgba left, ColorRgba right)
        {
            return new ColorRgba(
				(byte)Math.Min(255, left.R + right.R), 
				(byte)Math.Min(255, left.G + right.G), 
				(byte)Math.Min(255, left.B + right.B), 
				(byte)Math.Min(255, left.A + right.A));
        }
		/// <summary>
		/// Subtracts the second color from the first component-wise.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		/// <returns></returns>
		public static ColorRgba operator -(ColorRgba left, ColorRgba right)
        {
            return new ColorRgba(
				(byte)Math.Max(0, left.R - right.R), 
				(byte)Math.Max(0, left.G - right.G), 
				(byte)Math.Max(0, left.B - right.B), 
				(byte)Math.Max(0, left.A - right.A));
        }
		/// <summary>
		/// Multiplies two colors component-wise.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		/// <returns></returns>
		public static ColorRgba operator *(ColorRgba left, ColorRgba right)
        {
            return new ColorRgba(
				(byte)MathF.Clamp(MathF.Round(left.R * right.R / 255), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.G * right.G / 255), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.B * right.B / 255), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.A * right.A / 255), 0.0f, 255.0f));
        }
		/// <summary>
		/// Scales a color by the specified factor. This affects color and alpha equally.
		/// </summary>
		/// <param name="left">The color to scale.</param>
		/// <param name="right">The scaling factor.</param>
		/// <returns></returns>
		public static ColorRgba operator *(ColorRgba left, float right)
        {
            return new ColorRgba(
				(byte)MathF.Clamp(MathF.Round(left.R * right), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.G * right), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.B * right), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.A * right), 0.0f, 255.0f));
        }
		
		/// <summary>
		/// Adds two colors component-wise.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		/// <param name="result"></param>
		public static void Add(ref ColorRgba left, ref ColorRgba right, out ColorRgba result)
		{
			result = new ColorRgba(
				(byte)Math.Min(255, left.R + right.R), 
				(byte)Math.Min(255, left.G + right.G), 
				(byte)Math.Min(255, left.B + right.B), 
				(byte)Math.Min(255, left.A + right.A));
		}
		/// <summary>
		/// Subtracts two colors component-wise.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		/// <param name="result"></param>
		public static void Subtract(ref ColorRgba left, ref ColorRgba right, out ColorRgba result)
		{
			result = new ColorRgba(
				(byte)Math.Max(0, left.R - right.R), 
				(byte)Math.Max(0, left.G - right.G), 
				(byte)Math.Max(0, left.B - right.B), 
				(byte)Math.Max(0, left.A - right.A));
		}
		/// <summary>
		/// Multiplies two colors component-wise.
		/// </summary>
		/// <param name="left">The first color.</param>
		/// <param name="right">The second color.</param>
		/// <param name="result"></param>
		public static void Multiply(ref ColorRgba left, ref ColorRgba right, out ColorRgba result)
		{
			result = new ColorRgba(
				(byte)MathF.Clamp(MathF.Round(left.R * right.R / 255), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.G * right.G / 255), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.B * right.B / 255), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.A * right.A / 255), 0.0f, 255.0f));
		}
		/// <summary>
		/// Scales a color by the specified factor. This affects color and alpha equally.
		/// </summary>
		/// <param name="left">The color that is to be scaled.</param>
		/// <param name="right">The scaling factor.</param>
		/// <param name="result"></param>
		public static void Scale(ref ColorRgba left, float right, out ColorRgba result)
		{
			result = new ColorRgba(
				(byte)MathF.Clamp(MathF.Round(left.R * right), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.G * right), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.B * right), 0.0f, 255.0f), 
				(byte)MathF.Clamp(MathF.Round(left.A * right), 0.0f, 255.0f));
		}
		
		public static explicit operator ColorRgba(int c)
		{
			return new ColorRgba(c);
		}
		public static explicit operator ColorRgba(ColorHsva c)
		{
			return c.ToRgba();
		}
		public static explicit operator ColorRgba(OpenTK.Graphics.Color4 c)
		{
			return new ColorRgba(
				(byte)Math.Max(0, Math.Min(255, 255 * c.R)),
				(byte)Math.Max(0, Math.Min(255, 255 * c.G)),
				(byte)Math.Max(0, Math.Min(255, 255 * c.B)),
				(byte)Math.Max(0, Math.Min(255, 255 * c.A)));
		}
		public static explicit operator int(ColorRgba c)
		{
			return c.ToIntRgba();
		}
		public static explicit operator ColorHsva(ColorRgba c)
		{
			return ColorHsva.FromRgba(c);
		}
		public static explicit operator OpenTK.Graphics.Color4(ColorRgba c)
		{
			return new OpenTK.Graphics.Color4(
				c.R,
				c.G,
				c.B,
				c.A);
		}
	}
}
