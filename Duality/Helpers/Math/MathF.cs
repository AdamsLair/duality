using System;
using System.Linq;
using OpenTK;

namespace Duality
{
	/// <summary>
	/// Provides math utility methods and float versions of <see cref="System.Math"/> to fit
	/// Duality <see cref="System.Single"/> arithmetics. 
	/// </summary>
	public static class MathF
	{
		/// <summary>
		/// Euler's number, base of the natural logarithm. Approximately 2.7182818284.
		/// </summary>
		public const float E = (float)System.Math.E;
		/// <summary>
		/// Mmmhh... pie!
		/// </summary>
		public const float Pi = (float)System.Math.PI;

		/// <summary>
		/// Equals <see cref="Pi"/> / 2.
		/// </summary>
		public const float PiOver2 = Pi / 2.0f;
		/// <summary>
		/// Equals <see cref="Pi"/> / 3.
		/// </summary>
		public const float PiOver3 = Pi / 3.0f;
		/// <summary>
		/// Equals <see cref="Pi"/> / 4.
		/// </summary>
		public const float PiOver4 = Pi / 4.0f;
		/// <summary>
		/// Equals <see cref="Pi"/> / 6.
		/// </summary>
		public const float PiOver6 = Pi / 6.0f;
		/// <summary>
		/// Equals 2 * <see cref="Pi"/>.
		/// </summary>
		public const float TwoPi = Pi * 2.0f;
		
		/// <summary>
		/// A one degree angle in radians.
		/// </summary>
		public const float RadAngle1 = TwoPi / 360.0f;
		/// <summary>
		/// A 30 degree angle in radians. Equals <see cref="PiOver6"/>.
		/// </summary>
		public const float RadAngle30 = PiOver6;
		/// <summary>
		/// A 45 degree angle in radians. Equals <see cref="PiOver4"/>.
		/// </summary>
		public const float RadAngle45 = PiOver4;
		/// <summary>
		/// A 90 degree angle in radians. Equals <see cref="PiOver2"/>.
		/// </summary>
		public const float RadAngle90 = PiOver2;
		/// <summary>
		/// A 180 degree angle in radians. Equals <see cref="Pi"/>.
		/// </summary>
		public const float RadAngle180 = Pi;
		/// <summary>
		/// A 270 degree angle in radians. Equals <see cref="Pi"/>.
		/// </summary>
		public const float RadAngle270 = Pi + PiOver2;
		/// <summary>
		/// A 360 degree angle in radians. Equals <see cref="TwoPi"/>.
		/// </summary>
		public const float RadAngle360 = TwoPi;

		private static Random rnd = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
		/// <summary>
		/// [GET / SET] Global random number generator. Is never null.
		/// </summary>
		public static Random Rnd
		{
			get { return rnd; }
			set { rnd = value ?? new Random(); }
		}


		/// <summary>
		/// Converts the specified float value to decimal and clamps it if necessary.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static decimal SafeToDecimal(float v)
		{
			if (float.IsNaN(v))
				return decimal.Zero;
			else if (v <= (float)decimal.MinValue || float.IsNegativeInfinity(v))
				return decimal.MinValue;
			else if (v >= (float)decimal.MaxValue || float.IsPositiveInfinity(v))
				return decimal.MaxValue;
			else
				return (decimal)v;
		}

		/// <summary>
		/// Returns the absolute value of a <see cref="System.Single"/>.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>The absolute value of the number.</returns>
		public static float Abs(float v)
		{
			return v < 0 ? -v : v;
		}
		/// <summary>
		/// Returns the absolute value of a <see cref="System.Int32"/>.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>The absolute value of the number.</returns>
		public static int Abs(int v)
		{
			return v < 0 ? -v : v;
		}

		/// <summary>
		/// Returns the lowest whole-number bigger than the specified one. (Rounds up)
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>The rounded number.</returns>
		/// <seealso cref="Floor"/>
		public static float Ceiling(float v)
		{
			return (float)System.Math.Ceiling(v);
		}
		/// <summary>
		/// Returns the highest whole-number smaller than the specified one. (Rounds down)
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>The rounded number.</returns>
		/// <seealso cref="Ceiling"/>
		public static float Floor(float v)
		{
			return (float)System.Math.Floor(v);
		}

		/// <summary>
		/// Rounds the specified value.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>The rounded number.</returns>
		public static float Round(float v)
		{
			return (float)System.Math.Round(v);
		}
		/// <summary>
		/// Rounds the specified value to a certain number of fraction digits.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <param name="digits">The number of fraction digits to round to.</param>
		/// <returns>The rounded number.</returns>
		public static float Round(float v, int digits)
		{
			return (float)System.Math.Round(v, digits);
		}
		/// <summary>
		/// Rounds the specified value.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <param name="mode">Specifies what happens if the value is exactly inbetween two numbers.</param>
		/// <returns>The rounded number.</returns>
		public static float Round(float v, MidpointRounding mode)
		{
			return (float)System.Math.Round(v, mode);
		}
		/// <summary>
		/// Rounds the specified value to a certain number of fraction digits.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <param name="digits">The number of fraction digits to round to.</param>
		/// <param name="mode">Specifies what happens if the value is exactly inbetween two numbers.</param>
		/// <returns>The rounded number.</returns>
		public static float Round(float v, int digits, MidpointRounding mode)
		{
			return (float)System.Math.Round(v, digits, mode);
		}

		/// <summary>
		/// Rounds the specified value to an integer value.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>The rounded number as <see cref="System.Int32"/>.</returns>
		/// <seealso cref="Round(float)"/>
		public static int RoundToInt(float v)
		{
			return (int)System.Math.Round(v);
		}
		/// <summary>
		/// Rounds the specified value to an integer value.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <param name="mode">Specifies what happens if the value is exactly inbetween two numbers.</param>
		/// <returns>The rounded number as <see cref="System.Int32"/>.</returns>
		/// <seealso cref="Round(float, MidpointRounding)"/>
		public static int RoundToInt(float v, MidpointRounding mode)
		{
			return (int)System.Math.Round(v, mode);
		}

		/// <summary>
		/// Returns the sign of a value.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>-1 if negative, 1 if positive and 0 if zero.</returns>
		public static float Sign(float v)
		{
			return v < 0.0f ? -1.0f : (v > 0.0f ? 1.0f : 0.0f);
		}
		/// <summary>
		/// Returns the sign of a value.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>-1 if negative, 1 if positive and 0 if zero.</returns>
		public static int Sign(int v)
		{
			return v < 0 ? -1 : (v > 0 ? 1 : 0);
		}

		/// <summary>
		/// Returns a numbers square root.
		/// </summary>
		/// <param name="v">A number.</param>
		/// <returns>The numbers square root.</returns>
		public static float Sqrt(float v)
		{
			return (float)System.Math.Sqrt(v);
		}

		/// <summary>
		/// Returns the factorial of an integer value.
		/// </summary>
		/// <param name="n">A number.</param>
		/// <returns>The factorial of the number.</returns>
		public static int Factorial(int n)
		{
			int r = 1;
			for (; n > 1; n--) r *= n;
			return Math.Abs(r);
		}

		/// <summary>
		/// Returns the lower of two values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns>The lowest value.</returns>
		public static float Min(float v1, float v2)
		{
			return v1 < v2 ? v1 : v2;
		}
		/// <summary>
		/// Returns the lowest of three values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="v3"></param>
		/// <returns>The lowest value.</returns>
		public static float Min(float v1, float v2, float v3)
		{
			float min = v1;
			if (v2 < min) min = v2;
			if (v3 < min) min = v3;
			return min;
		}
		/// <summary>
		/// Returns the lowest of four values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="v3"></param>
		/// <param name="v4"></param>
		/// <returns>The lowest value.</returns>
		public static float Min(float v1, float v2, float v3, float v4)
		{
			float min = v1;
			if (v2 < min) min = v2;
			if (v3 < min) min = v3;
			if (v4 < min) min = v4;
			return min;
		}
		/// <summary>
		/// Returns the lowest of any number of values.
		/// </summary>
		/// <param name="v"></param>
		/// <returns>The lowest value.</returns>
		public static float Min(params float[] v)
		{
			float min = v[0];
			for (int i = 1; i < v.Length; i++)
			{
				if (v[i] < min) min = v[i];
			}
			return min;
		}
		/// <summary>
		/// Returns the lower of two values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns>The lowest value.</returns>
		public static int Min(int v1, int v2)
		{
			return v1 < v2 ? v1 : v2;
		}
		/// <summary>
		/// Returns the lowest of three values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="v3"></param>
		/// <returns>The lowest value.</returns>
		public static int Min(int v1, int v2, int v3)
		{
			int min = v1;
			if (v2 < min) min = v2;
			if (v3 < min) min = v3;
			return min;
		}
		/// <summary>
		/// Returns the lowest of four values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="v3"></param>
		/// <param name="v4"></param>
		/// <returns>The lowest value.</returns>
		public static int Min(int v1, int v2, int v3, int v4)
		{
			int min = v1;
			if (v2 < min) min = v2;
			if (v3 < min) min = v3;
			if (v4 < min) min = v4;
			return min;
		}
		/// <summary>
		/// Returns the lowest of any number of values.
		/// </summary>
		/// <param name="v"></param>
		/// <returns>The lowest value.</returns>
		public static int Min(params int[] v)
		{
			int min = v[0];
			for (int i = 1; i < v.Length; i++)
			{
				if (v[i] < min) min = v[i];
			}
			return min;
		}
		
		/// <summary>
		/// Returns the higher of two values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns>The highest value.</returns>
		public static float Max(float v1, float v2)
		{
			return v1 > v2 ? v1 : v2;
		}
		/// <summary>
		/// Returns the highest of three values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="v3"></param>
		/// <returns>The highest value.</returns>
		public static float Max(float v1, float v2, float v3)
		{
			float max = v1;
			if (v2 > max) max = v2;
			if (v3 > max) max = v3;
			return max;
		}
		/// <summary>
		/// Returns the highest of four values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="v3"></param>
		/// <param name="v4"></param>
		/// <returns>The highest value.</returns>
		public static float Max(float v1, float v2, float v3, float v4)
		{
			float max = v1;
			if (v2 > max) max = v2;
			if (v3 > max) max = v3;
			if (v4 > max) max = v4;
			return max;
		}
		/// <summary>
		/// Returns the highest of any number of values.
		/// </summary>
		/// <param name="v"></param>
		/// <returns>The highest value.</returns>
		public static float Max(params float[] v)
		{
			float max = v[0];
			for (int i = 1; i < v.Length; i++)
			{
				if (v[i] > max) max = v[i];
			}
			return max;
		}
		/// <summary>
		/// Returns the higher of two values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns>The highest value.</returns>
		public static int Max(int v1, int v2)
		{
			return v1 > v2 ? v1 : v2;
		}
		/// <summary>
		/// Returns the highest of three values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="v3"></param>
		/// <returns>The highest value.</returns>
		public static int Max(int v1, int v2, int v3)
		{
			int max = v1;
			if (v2 > max) max = v2;
			if (v3 > max) max = v3;
			return max;
		}
		/// <summary>
		/// Returns the highest of four values.
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="v3"></param>
		/// <param name="v4"></param>
		/// <returns>The highest value.</returns>
		public static int Max(int v1, int v2, int v3, int v4)
		{
			int max = v1;
			if (v2 > max) max = v2;
			if (v3 > max) max = v3;
			if (v4 > max) max = v4;
			return max;
		}
		/// <summary>
		/// Returns the highest of any number of values.
		/// </summary>
		/// <param name="v"></param>
		/// <returns>The highest value.</returns>
		public static int Max(params int[] v)
		{
			int max = v[0];
			for (int i = 1; i < v.Length; i++)
			{
				if (v[i] > max) max = v[i];
			}
			return max;
		}

		/// <summary>
		/// Clamps a value between minimum and maximum.
		/// </summary>
		/// <param name="v">The value to clamp.</param>
		/// <param name="min">The minimum value that can't be deceeded.</param>
		/// <param name="max">The maximum value that can't be exceeded.</param>
		/// <returns>The clamped value.</returns>
		public static float Clamp(float v, float min, float max)
		{
			return v < min ? min : (v > max ? max : v);
		}
		/// <summary>
		/// Clamps a value between minimum and maximum.
		/// </summary>
		/// <param name="v">The value to clamp.</param>
		/// <param name="min">The minimum value that can't be deceeded.</param>
		/// <param name="max">The maximum value that can't be exceeded.</param>
		/// <returns>The clamped value.</returns>
		public static int Clamp(int v, int min, int max)
		{
			return v < min ? min : (v > max ? max : v);
		}

		/// <summary>
		/// Performs linear interpolation between two values.
		/// </summary>
		/// <param name="a">The first anchor value.</param>
		/// <param name="b">The second anchor value.</param>
		/// <param name="ratio">Ratio between first and second anchor. Zero will result in anchor a, one will result in anchor b.</param>
		/// <returns></returns>
		public static float Lerp(float a, float b, float ratio)
		{
			return a + ratio * (b - a);
		}
		/// <summary>
		/// Performs linear interpolation between two values.
		/// </summary>
		/// <param name="a">The first anchor value.</param>
		/// <param name="b">The second anchor value.</param>
		/// <param name="ratio">Ratio between first and second anchor. Zero will result in anchor a, one will result in anchor b.</param>
		/// <returns></returns>
		public static int Lerp(int a, int b, float ratio)
		{
			return MathF.RoundToInt(a + ratio * (float)(b - a));
		}

		/// <summary>
		/// Returns the specified power of <see cref="E"/>.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static float Exp(float v)
		{
			return (float)System.Math.Exp(v);
		}
		/// <summary>
		/// Returns the natural logarithm of a value.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static float Log(float v)
		{
			return (float)System.Math.Log(v);
		}
		/// <summary>
		/// Returns the specified power of a value.
		/// </summary>
		/// <param name="v">The base value.</param>
		/// <param name="e">Specifies the power to return.</param>
		/// <returns></returns>
		public static float Pow(float v, float e)
		{
			return (float)System.Math.Pow(v, e);
		}
		/// <summary>
		/// Returns the logarithm of a value.
		/// </summary>
		/// <param name="v">The value whichs logarithm is to be calculated.</param>
		/// <param name="newBase">The base of the logarithm.</param>
		/// <returns></returns>
		public static float Log(float v, float newBase)
		{
			return (float)System.Math.Log(v, newBase);
		}

		/// <summary>
		/// Returns the sine value of the specified (radian) angle.
		/// </summary>
		/// <param name="angle">A radian angle.</param>
		/// <returns></returns>
		public static float Sin(float angle)
		{
			return (float)System.Math.Sin(angle);
		}
		/// <summary>
		/// Returns the cosine value of the specified (radian) angle.
		/// </summary>
		/// <param name="angle">A radian angle.</param>
		/// <returns></returns>
		public static float Cos(float angle)
		{
			return (float)System.Math.Cos(angle);
		}
		/// <summary>
		/// Returns the tangent value of the specified (radian) angle.
		/// </summary>
		/// <param name="angle">A radian angle.</param>
		/// <returns></returns>
		public static float Tan(float angle)
		{
			return (float)System.Math.Tan(angle);
		}
		/// <summary>
		/// Returns the inverse sine value of the specified (radian) angle.
		/// </summary>
		/// <param name="angle">A radian angle.</param>
		/// <returns></returns>
		public static float Asin(float sin)
		{
			return (float)System.Math.Asin(sin);
		}
		/// <summary>
		/// Returns the inverse cosine value of the specified (radian) angle.
		/// </summary>
		/// <param name="angle">A radian angle.</param>
		/// <returns></returns>
		public static float Acos(float cos)
		{
			return (float)System.Math.Acos(cos);
		}
		/// <summary>
		/// Returns the inverse tangent value of the specified (radian) angle.
		/// </summary>
		/// <param name="angle">A radian angle.</param>
		/// <returns></returns>
		public static float Atan(float tan)
		{
			return (float)System.Math.Atan(tan);
		}
		/// <summary>
		/// Returns the (radian) angle whose tangent is the quotient of two specified numbers.
		/// </summary>
		/// <param name="y">The y coordinate of a point. </param>
		/// <param name="x">The x coordinate of a point. </param>
		/// <returns></returns>
		public static float Atan2(float y, float x)
		{
			return (float)System.Math.Atan2(y, x);
		}

		/// <summary>
		/// Converts degrees  to radians.
		/// </summary>
		/// <param name="deg"></param>
		/// <returns></returns>
		public static float DegToRad(float deg)
		{
			const float factor = (float)System.Math.PI / 180.0f;
			return deg * factor;
		}
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="rad"></param>
		/// <returns></returns>
		public static float RadToDeg(float rad)
		{
			const float factor = 180.0f / (float)System.Math.PI;
			return rad * factor;
		}

		/// <summary>
		/// Normalizes a value to the given circular area.
		/// </summary>
		/// <returns>The normalized value between min (inclusive) and max (exclusive).</returns>
		/// <example>
		/// <c>NormalizeVar(480, 0, 360)</c> will return 120.
		/// </example>
		public static float NormalizeVar(float var, float min, float max)
		{
			if (var >= min && var < max) return var;

			if (var < min)
				var = max + ((var - min) % max);
			else
				var = min + var % (max - min);

			return var;
		}
		/// <summary>
		/// Normalizes a value to the given circular area.
		/// </summary>
		/// <returns>The normalized value between min (inclusive) and max (exclusive).</returns>
		/// <example>
		/// <c>NormalizeVar(480, 0, 360)</c> will return 120.
		/// </example>
		public static int NormalizeVar(int var, int min, int max)
		{
			if (var >= min && var < max) return var;

			if (var < min)
				var = max + ((var - min) % max);
			else
				var = min + var % (max - min);

			return var;
		}
		/// <summary>
		/// Normalizes a radian angle to values between zero and <see cref="TwoPi"/>.
		/// </summary>
		/// <returns>The normalized value between zero and <see cref="TwoPi"/>.</returns>
		/// <example>
		/// <c>NormalizeAngle(<see cref="TwoPi"/> + <see cref="Pi"/>)</c> will return <see cref="Pi"/>.
		/// </example>
		public static float NormalizeAngle(float var)
		{
			if (var >= 0.0f && var < RadAngle360) return var;

			if (var < 0.0f)
				var = RadAngle360 + (var % RadAngle360);
			else
				var = var % RadAngle360;

			return var;
		}

		/// <summary>
		/// Returns the distance between two points in 2d space.
		/// </summary>
		/// <param name="x1">The x-Coordinate of the first point.</param>
		/// <param name="y1">The y-Coordinate of the first point.</param>
		/// <param name="x2">The x-Coordinate of the second point.</param>
		/// <param name="y2">The y-Coordinate of the second point.</param>
		/// <returns>The distance between both points.</returns>
		public static float Distance(float x1, float y1, float x2, float y2)
		{
			return ((float)System.Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
		}
		/// <summary>
		/// Returns the distance between a point and [0,0] in 2d space.
		/// </summary>
		/// <param name="x">The x-Coordinate of the point.</param>
		/// <param name="y">The y-Coordinate of the point.</param>
		/// <returns>The distance between the point and [0,0].</returns>
		public static float Distance(float x, float y)
		{
			return ((float)System.Math.Sqrt(x * x + y * y));
		}
		/// <summary>
		/// Returns the squared distance between two points in 2d space.
		/// </summary>
		/// <param name="x1">The x-Coordinate of the first point.</param>
		/// <param name="y1">The y-Coordinate of the first point.</param>
		/// <param name="x2">The x-Coordinate of the second point.</param>
		/// <param name="y2">The y-Coordinate of the second point.</param>
		/// <returns>The distance between both points.</returns>
		/// <remarks>
		/// This method is faster than <see cref="Distance(float,float,float,float)"/>. 
		/// If sufficient, such as for distance comparison, consider using this method instead.
		/// </remarks>
		public static float DistanceQuad(float x1, float y1, float x2, float y2)
		{
			return (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
		}
		/// <summary>
		/// Returns the squared distance between a point and [0,0] in 2d space.
		/// </summary>
		/// <param name="x">The x-Coordinate of the point.</param>
		/// <param name="y">The y-Coordinate of the point.</param>
		/// <returns>The distance between the point and [0,0].</returns>
		/// <remarks>
		/// This method is faster than <see cref="Distance(float,float)"/>. 
		/// If sufficient, such as for distance comparison, consider using this method instead.
		/// </remarks>
		public static float DistanceQuad(float x, float y)
		{
			return x * x + y * y;
		}

		/// <summary>
		/// Calculates the angle between two points in 2D space.
		/// </summary>
		/// <param name="x1">The x-Coordinate of the first point.</param>
		/// <param name="y1">The y-Coordinate of the first point.</param>
		/// <param name="x2">The x-Coordinate of the second point.</param>
		/// <param name="y2">The y-Coordinate of the second point.</param>
		/// <returns>The angle between [x1,y1] and [x2,y2] in radians.</returns>
		public static float Angle(float x1, float y1, float x2, float y2)
		{
			return (float)((System.Math.Atan2((y2 - y1), (x2 - x1)) + PiOver2 + TwoPi) % TwoPi);
		}
		/// <summary>
		/// Calculates the angle from [0,0] to a specified point in 2D space.
		/// </summary>
		/// <param name="x">The x-Coordinate of the point.</param>
		/// <param name="y">The y-Coordinate of the point.</param>
		/// <returns>The angle between [0,0] and [x,y] in radians.</returns>
		public static float Angle(float x, float y)
		{
			return (float)((System.Math.Atan2(y, x) + PiOver2 + TwoPi) % TwoPi);
		}

		/// <summary>
		/// Assuming a circular value area, this method returns the direction to "turn" value 1 to
		/// when it comes to take the shortest way to value 2.
		/// </summary>
		/// <param name="val1">The first (source) value.</param>
		/// <param name="val2">The second (destination) value.</param>
		/// <param name="minVal">Minimum value.</param>
		/// <param name="maxVal">Maximum value.</param>
		/// <returns>-1 for "left" / lower, 1 for "right" / higher and 0 for "stay" / equal</returns>
		public static float TurnDir(float val1, float val2, float minVal, float maxVal)
		{
			val1 = MathF.NormalizeVar(val1, minVal, maxVal);
			val2 = MathF.NormalizeVar(val2, minVal, maxVal);
			if (val1 == val2) return 0.0f;

			if (Math.Abs(val1 - val2) > (maxVal - minVal) * 0.5f)
			{
				if (val1 > val2) return 1.0f;
				else return -1.0f;
			}
			else
			{
				if (val1 > val2) return -1.0f;
				else return 1.0f;
			}
		}
		/// <summary>
		/// Assuming a circular value area, this method returns the direction to "turn" value 1 to
		/// when it comes to take the shortest way to value 2.
		/// </summary>
		/// <param name="val1">The first (source) value.</param>
		/// <param name="val2">The second (destination) value.</param>
		/// <param name="minVal">Minimum value.</param>
		/// <param name="maxVal">Maximum value.</param>
		/// <returns>-1 for "left" / lower, 1 for "right" / higher and 0 for "stay" / equal</returns>
		public static int TurnDir(int val1, int val2, int minVal, int maxVal)
		{
			val1 = MathF.NormalizeVar(val1, minVal, maxVal);
			val2 = MathF.NormalizeVar(val2, minVal, maxVal);
			if (val1 == val2) return 0;

			if (Math.Abs(val1 - val2) > (maxVal - minVal) * 0.5f)
			{
				if (val1 > val2) return 1;
				else return -1;
			}
			else
			{
				if (val1 > val2) return -1;
				else return 1;
			}
		}
		/// <summary>
		/// Assuming an angular (radian) value area, this method returns the direction to "turn" value 1 to
		/// when it comes to take the shortest way to value 2.
		/// </summary>
		/// <param name="val1">The first (source) value.</param>
		/// <param name="val2">The second (destination) value.</param>
		/// <returns>-1 for "left" / lower, 1 for "right" / higher and 0 for "stay" / equal</returns>
		public static float TurnDir(float val1, float val2)
		{
			val1 = MathF.NormalizeAngle(val1);
			val2 = MathF.NormalizeAngle(val2);
			if (val1 == val2) return 0.0f;

			if (Math.Abs(val1 - val2) > RadAngle180)
			{
				if (val1 > val2) return 1.0f;
				else return -1.0f;
			}
			else
			{
				if (val1 > val2) return -1.0f;
				else return 1.0f;
			}
		}

		/// <summary>
		/// Calculates the distance between two values assuming a circular value area.
		/// </summary>
		/// <param name="v1">Value 1</param>
		/// <param name="v2">Value 2</param>
		/// <param name="vMin">Value area minimum</param>
		/// <param name="vMax">Value area maximum</param>
		/// <returns>Value distance</returns>
		public static float CircularDist(float v1, float v2, float vMin, float vMax)
		{
			float vTemp = System.Math.Abs(NormalizeVar(v1, vMin, vMax) - NormalizeVar(v2, vMin, vMax));
			if (vTemp * 2.0f <= vMax - vMin)
				return vTemp;
			else
				return (vMax - vMin) - vTemp;
		}
		/// <summary>
		/// Calculates the distance between two values assuming a circular value area.
		/// </summary>
		/// <param name="v1">Value 1</param>
		/// <param name="v2">Value 2</param>
		/// <param name="vMin">Value area minimum</param>
		/// <param name="vMax">Value area maximum</param>
		/// <returns>Value distance</returns>
		public static int CircularDist(int v1, int v2, int vMin, int vMax)
		{
			int vTemp = System.Math.Abs(NormalizeVar(v1, vMin, vMax) - NormalizeVar(v2, vMin, vMax));
			if (vTemp * 2 <= vMax - vMin)
				return vTemp;
			else
				return (vMax - vMin) - vTemp;
		}
		/// <summary>
		/// Calculates the distance between two angular (radian) values.
		/// </summary>
		/// <param name="v1">The first (radian) angle.</param>
		/// <param name="v2">The second (radian) angle.</param>
		/// <returns>The angular distance in radians between both angles.</returns>
		public static float CircularDist(float v1, float v2)
		{
			float vTemp = System.Math.Abs(NormalizeAngle(v1) - NormalizeAngle(v2));
			if (vTemp * 2.0f <= RadAngle360)
				return vTemp;
			else
				return RadAngle360 - vTemp;
		}

		/// <summary>
		/// Turns and scales a specific coordinate around the specified center point.
		/// </summary>
		/// <param name="xCoord">The x-Coordinate to transform.</param>
		/// <param name="yCoord">The y-Coordinate to transform.</param>
		/// <param name="rot">The rotation to apply in radians.</param>
		/// <param name="scale">The scale factor to apply.</param>
		/// <param name="xCenter">The x-Coordinate of the transformations origin.</param>
		/// <param name="yCenter">The y-Coordinate of the transformations origin.</param>
		public static void TransformCoord(ref float xCoord, ref float yCoord, float rot, float scale, float xCenter, float yCenter)
		{
			float sin = (float)System.Math.Sin(rot);
			float cos = (float)System.Math.Cos(rot);
			float lastX = xCoord;
			xCoord = xCenter + ((xCoord - xCenter) * cos - (yCoord - yCenter) * sin) * scale;
			yCoord = yCenter + ((lastX - xCenter) * sin + (yCoord - yCenter) * cos) * scale;
		}
		/// <summary>
		/// Turns and scales a specific coordinate around [0,0].
		/// </summary>
		/// <param name="xCoord">The x-Coordinate to transform.</param>
		/// <param name="yCoord">The y-Coordinate to transform.</param>
		/// <param name="rot">The rotation to apply in radians.</param>
		/// <param name="scale">The scale factor to apply.</param>
		public static void TransformCoord(ref float xCoord, ref float yCoord, float rot, float scale)
		{
			float sin = (float)System.Math.Sin(rot);
			float cos = (float)System.Math.Cos(rot);
			float lastX = xCoord;
			xCoord = (xCoord * cos - yCoord * sin) * scale;
			yCoord = (lastX * sin + yCoord * cos) * scale;
		}
		/// <summary>
		/// Turns a specific coordinate around [0,0].
		/// </summary>
		/// <param name="xCoord">The x-Coordinate to transform.</param>
		/// <param name="yCoord">The y-Coordinate to transform.</param>
		/// <param name="rot">The rotation to apply in radians.</param>
		public static void TransformCoord(ref float xCoord, ref float yCoord, float rot)
		{
			float sin = (float)System.Math.Sin(rot);
			float cos = (float)System.Math.Cos(rot);
			float lastX = xCoord;
			xCoord = xCoord * cos - yCoord * sin;
			yCoord = lastX * sin + yCoord * cos;
		}

		/// <summary>
		/// Prepares a 2d transformation (rotation & scale).
		/// </summary>
		/// <param name="rot">The rotation to apply in radians.</param>
		/// <param name="scale">The scale factor to apply.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <seealso cref="TransformDotVec"/>
		public static void GetTransformDotVec(float rot, Vector2 scale, out Vector2 xDot, out Vector2 yDot)
		{
			float sin = (float)System.Math.Sin(rot);
			float cos = (float)System.Math.Cos(rot);
			xDot = new Vector2(cos * scale.X, -sin * scale.X);
			yDot = new Vector2(sin * scale.Y, cos * scale.Y);
		}
		/// <summary>
		/// Prepares a 2d transformation (rotation & scale).
		/// </summary>
		/// <param name="rot">The rotation to apply in radians.</param>
		/// <param name="scale">The scale factor to apply.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <seealso cref="TransformDotVec"/>
		public static void GetTransformDotVec(float rot, float scale, out Vector2 xDot, out Vector2 yDot)
		{
			float sin = (float)System.Math.Sin(rot);
			float cos = (float)System.Math.Cos(rot);
			xDot = new Vector2(cos * scale, -sin * scale);
			yDot = new Vector2(sin * scale, cos * scale);
		}
		/// <summary>
		/// Prepares a 2d transformation (rotation).
		/// </summary>
		/// <param name="rot">The rotation to apply in radians.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <seealso cref="TransformDotVec(ref Vector2, ref Vector2, ref Vector2)"/>
		public static void GetTransformDotVec(float rot, out Vector2 xDot, out Vector2 yDot)
		{
			float sin = (float)System.Math.Sin(rot);
			float cos = (float)System.Math.Cos(rot);
			xDot = new Vector2(cos, -sin);
			yDot = new Vector2(sin, cos);
		}

		/// <summary>
		/// Performs a 2d transformation
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <seealso cref="GetTransformDotVec(float, out Vector2, out Vector2)"/>
		public static void TransformDotVec(ref Vector2 vec, ref Vector2 xDot, ref Vector2 yDot)
		{
			float oldX = vec.X;
			vec.X = vec.X * xDot.X + vec.Y * xDot.Y;
			vec.Y = oldX * yDot.X + vec.Y * yDot.Y;
		}
		/// <summary>
		/// Performs a 2d transformation
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <seealso cref="GetTransformDotVec(float, out Vector2, out Vector2)"/>
		public static void TransformDotVec(ref Vector2 vec, Vector2 xDot, Vector2 yDot)
		{
			float oldX = vec.X;
			vec.X = vec.X * xDot.X + vec.Y * xDot.Y;
			vec.Y = oldX * yDot.X + vec.Y * yDot.Y;
		}
		/// <summary>
		/// Performs a 2d transformation
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <returns>The transformed vector.</returns>
		/// <seealso cref="GetTransformDotVec(float, out Vector2, out Vector2)"/>
		public static Vector2 TransformDotVec(Vector2 vec, Vector2 xDot, Vector2 yDot)
		{
			return new Vector2(
				vec.X * xDot.X + vec.Y * xDot.Y,
				vec.X * yDot.X + vec.Y * yDot.Y);
		}
		/// <summary>
		/// Performs a 2d transformation
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <seealso cref="GetTransformDotVec(float, out Vector2, out Vector2)"/>
		public static void TransformDotVec(ref Vector3 vec, ref Vector2 xDot, ref Vector2 yDot)
		{
			float oldX = vec.X;
			vec.X = vec.X * xDot.X + vec.Y * xDot.Y;
			vec.Y = oldX * yDot.X + vec.Y * yDot.Y;
		}
		/// <summary>
		/// Performs a 2d transformation
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <seealso cref="GetTransformDotVec(float, out Vector2, out Vector2)"/>
		public static void TransformDotVec(ref Vector3 vec, Vector2 xDot, Vector2 yDot)
		{
			float oldX = vec.X;
			vec.X = vec.X * xDot.X + vec.Y * xDot.Y;
			vec.Y = oldX * yDot.X + vec.Y * yDot.Y;
		}
		/// <summary>
		/// Performs a 2d transformation
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="xDot">Dot product base for the transformed x value.</param>
		/// <param name="yDot">Dot product base for the transformed y value.</param>
		/// <returns>The transformed vector.</returns>
		/// <seealso cref="GetTransformDotVec(float, out Vector2, out Vector2)"/>
		public static Vector3 TransformDotVec(Vector3 vec,Vector2 xDot, Vector2 yDot)
		{
			return new Vector3(
				vec.X * xDot.X + vec.Y * xDot.Y,
				vec.X * yDot.X + vec.Y * yDot.Y,
				0);
		}

		/// <summary>
		/// Checks, if two line segments (or infinite lines) cross and determines their mutual point.
		/// </summary>
		/// <param name="startX1">x-Coordinate of the first lines start.</param>
		/// <param name="startY1">y-Coordinate of the first lines start.</param>
		/// <param name="endX1">x-Coordinate of the first lines end.</param>
		/// <param name="endY1">y-Coordinate of the first lines end.</param>
		/// <param name="startX2">x-Coordinate of the second lines start.</param>
		/// <param name="startY2">y-Coordinate of the second lines start.</param>
		/// <param name="endX2">x-Coordinate of the second lines end.</param>
		/// <param name="endY2">y-Coordinate of the second lines end.</param>
		/// <param name="infinite">Whether the lines are considered infinite.</param>
		/// <param name="crossX">x-Coordiante at which both lines cross.</param>
		/// <param name="crossY">y-Coordinate at which both lines cross.</param>
		/// <returns>True, if the lines cross, false if not.</returns>
		public static bool LinesCross(
			float startX1, float startY1, float endX1, float endY1,
			float startX2, float startY2, float endX2, float endY2,
			out float crossX, out float crossY,
			bool infinite = false)
		{
			float n = (startY1 - startY2) * (endX2 - startX2) - (startX1 - startX2) * (endY2 - startY2);
			float d = (endX1 - startX1) * (endY2 - startY2) - (endY1 - startY1) * (endX2 - startX2);

			crossX = 0.0f;
			crossY = 0.0f;

			if (Math.Abs(d) < 0.0001)
				return false;
			else
			{
				float sn = (startY1 - startY2) * (endX1 - startX1) - (startX1 - startX2) * (endY1 - startY1);
				float ab = n / d;
				if (infinite)
				{
					crossX = startX1 + ab * (endX1 - startX1);
					crossY = startY1 + ab * (endY1 - startY1);
					return true;
				}
				else if (ab > 0.0 && ab < 1.0)
				{
					float cd = sn / d;
					if (cd > 0.0 && cd < 1.0)
					{
						crossX = startX1 + ab * (endX1 - startX1);
						crossY = startY1 + ab * (endY1 - startY1);
						return true;
					}
				}
			}

			return false;
		}
		/// <summary>
		/// Checks, if two line segments (or infinite lines) cross and determines their mutual point.
		/// </summary>
		/// <param name="startX1">x-Coordinate of the first lines start.</param>
		/// <param name="startY1">y-Coordinate of the first lines start.</param>
		/// <param name="endX1">x-Coordinate of the first lines end.</param>
		/// <param name="endY1">y-Coordinate of the first lines end.</param>
		/// <param name="startX2">x-Coordinate of the second lines start.</param>
		/// <param name="startY2">y-Coordinate of the second lines start.</param>
		/// <param name="endX2">x-Coordinate of the second lines end.</param>
		/// <param name="endY2">y-Coordinate of the second lines end.</param>
		/// <param name="infinite">Whether the lines are considered infinite.</param>
		/// <returns>True, if the lines cross, false if not.</returns>
		public static bool LinesCross(
			float startX1, float startY1, float endX1, float endY1,
			float startX2, float startY2, float endX2, float endY2,
			bool infinite = false)
		{
			float crossX;
			float crossY;
			return LinesCross(
				startX1, startY1, endX1, endY1,
				startX2, startY2, endX2, endY2,
				out crossX, out crossY,
				infinite);
		}

		/// <summary>
		/// Calculates the point on a line segment (or infinite line) that has the lowest possible
		/// distance to a point.
		/// </summary>
		/// <param name="pX">x-Coordinate of the point.</param>
		/// <param name="pY">y-Coordinate of the point.</param>
		/// <param name="lX1">x-Coordinate of the lines start.</param>
		/// <param name="lY1">y-Coordinate of the lines start.</param>
		/// <param name="lX2">x-Coordinate of the lines end.</param>
		/// <param name="lY2">y-Coordinate of the lines end.</param>
		/// <param name="infinite">Whether the line is considered infinite.</param>
		/// <returns>A point located on the specified line that is as close as possible to the specified point.</returns>
		public static Vector2 PointLineNearestPoint(
			float pX, float pY,
			float lX1, float lY1, float lX2, float lY2,
			bool infinite = false)
		{
			if (lX1 == lX2 && lY1 == lY2) return new Vector2(lX1, lY1);
			double sX = lX2 - lX1;
			double sY = lY2 - lY1;
			double q = ((pX - lX1) * sX + (pY - lY1) * sY) / (sX * sX + sY * sY);

			if (!infinite)
			{
				if (q < 0.0) q = 0.0f;
				if (q > 1.0) q = 1.0f;
			}

			return new Vector2((float)((1.0d - q) * lX1 + q * lX2), (float)((1.0d - q) * lY1 + q * lY2));
		}

		/// <summary>
		/// Calculates the distance between a point and a line segment (or infinite line).
		/// </summary>
		/// <param name="pX">x-Coordinate of the point.</param>
		/// <param name="pY">y-Coordinate of the point.</param>
		/// <param name="lX1">x-Coordinate of the lines start.</param>
		/// <param name="lY1">y-Coordinate of the lines start.</param>
		/// <param name="lX2">x-Coordinate of the lines end.</param>
		/// <param name="lY2">y-Coordinate of the lines end.</param>
		/// <param name="infinite">Whether the line is considered infinite.</param>
		/// <returns>The distance between point and line.</returns>
		public static float PointLineDistance(
			float pX, float pY,
			float lX1, float lY1, float lX2, float lY2,
			bool infinite = false)
		{
			Vector2 n = PointLineNearestPoint(pX, pY, lX1, lY1, lX2, lY2, infinite);
			return Distance(pX, pY, n.X, n.Y);
		}

		/// <summary>
		/// Assuming two objects travelling at a linear course with constant speed and angle, this method
		/// calculates at which point they may collide if the angle of object 1 is not defined by a specific
		/// (but constant!) value.
		/// In other words: If object 1 tries to hit object 2, let object 1 move towards the calculated point.
		/// </summary>
		/// <param name="obj1X">x-Coordinate of the first object.</param>
		/// <param name="obj1Y">y-Coordinate of the first object.</param>
		/// <param name="obj1Speed">Speed of the first object.</param>
		/// <param name="obj2X">x-Coordinate of the second object.</param>
		/// <param name="obj2Y">y-Coordinate of the second object.</param>
		/// <param name="obj2Speed">Speed of the second object.</param>
		/// <param name="obj2Angle">Angle (in radians) of the second object.</param>
		/// <param name="colX">x-Coordinate of the predicted impact.</param>
		/// <param name="colY">y-Coordinate of the predicted impact.</param>
		/// <returns>
		/// False if it is not possible for object 1 to collide with object 2 at any course of object 1.
		/// This is, for example, the case if object 1 and to move to the same direction but object 2 is faster.
		/// A "collision point" is calculated either way, though it is not a collision point but only a 
		/// "directional idea" if false is returned.
		/// </returns>
		public static bool GetLinearPrediction(
			float obj1X, float obj1Y, float obj1Speed,
			float obj2X, float obj2Y, float obj2Speed, float obj2Angle,
			out float colX, out float colY)
		{
			if (obj2Speed <= float.Epsilon)
			{
				colX = obj2X;
				colY = obj2Y;
				return true;
			}
			else if (obj1Speed <= float.Epsilon)
			{
				colX = obj2X;
				colY = obj2Y;
				return false;
			}

			obj2Angle = NormalizeAngle(obj2Angle);
			float targetDist = Distance(obj2X, obj2Y, obj1X, obj1Y);
			float targetAngle = Angle(obj2X, obj2Y, obj1X, obj1Y);
			float tmpAngle1 = targetAngle - obj2Angle;
			float tmpAngle2;

			if (tmpAngle1 < 0.0d) tmpAngle1 = RadAngle360 - tmpAngle1;
			if (Math.Abs(tmpAngle1) > Pi) tmpAngle1 = RadAngle360 - Math.Abs(tmpAngle1);
			tmpAngle1 = Math.Abs(tmpAngle1);
			tmpAngle2 = Math.Abs(Asin(Sin(tmpAngle1) * obj2Speed / obj1Speed));

			float tmp;
			if (double.IsNaN(tmpAngle2) || tmpAngle1 + tmpAngle2 >= Pi)
			{
				tmp = targetDist * obj2Speed * obj2Speed / obj1Speed;
				colX = obj2X + (Sin(obj2Angle) * tmp);
				colY = obj2Y + (-Cos(obj2Angle) * tmp);
				return false;
			}
			else
			{
				tmp = targetDist * Math.Abs(Sin(tmpAngle2) / Sin(Pi - tmpAngle1 - tmpAngle2));
				colX = obj2X + (Sin(obj2Angle) * tmp);
				colY = obj2Y + (-Cos(obj2Angle) * tmp);
				return true;
			}
		}

		/// <summary>
		/// Assuming two objects travelling at a linear course with constant speed and angle, this method
		/// calculates the time from now at which the distance between the two objects will be minimal. If
		/// this has already passed, the returned time is negative.
		/// </summary>
		/// <param name="obj1X">x-Coordinate of the first object.</param>
		/// <param name="obj1Y">y-Coordinate of the first object.</param>
		/// <param name="obj1XSpeed">x-Speed of the first object.</param>
		/// <param name="obj1YSpeed">y-Speed of the first object.</param>
		/// <param name="obj2X">x-Coordinate of the second object.</param>
		/// <param name="obj2Y">y-Coordinate of the second object.</param>
		/// <param name="obj2XSpeed">x-Speed of the second object.</param>
		/// <param name="obj2YSpeed">y-Speed of the second object.</param>
		/// <returns>Time of minimum distance.</returns>
		public static float GetLinearPrediction2(
			float obj1X, float obj1Y, float obj1XSpeed, float obj1YSpeed,
			float obj2X, float obj2Y, float obj2XSpeed, float obj2YSpeed)
		{
			float timeTemp;
			timeTemp = -((obj1Y - obj2Y) * (obj1YSpeed - obj2YSpeed)) - ((obj1X - obj2X) * (obj1XSpeed - obj2XSpeed));
			timeTemp /= (((obj1XSpeed - obj2XSpeed) * (obj1XSpeed - obj2XSpeed)) + ((obj1YSpeed - obj2YSpeed) * (obj1YSpeed - obj2YSpeed)));
			return timeTemp;
		}

		/// <summary>
		/// Returns whether or not the specified polygon is convex.
		/// </summary>
		/// <param name="vertices"></param>
		/// <returns></returns>
		public static bool IsPolygonConvex(params Vector2[] vertices)
		{
			bool neg = false;
			bool pos = false;

			for (int a = 0; a < vertices.Length; a++)
			{
				int b = (a + 1) % vertices.Length;
				int c = (b + 1) % vertices.Length;
				Vector2 ab = vertices[b] - vertices[a];
				Vector2 bc = vertices[c] - vertices[b];

				if (ab == Vector2.Zero) return false;
				if (bc == Vector2.Zero) return false;

				float dot_product = Vector2.Dot(ab.PerpendicularLeft, bc);
				if (dot_product > 0.0f) pos = true;
				else if (dot_product < 0.0f) neg = true;

				if (neg && pos) return false;
			}

			return true;
		}
		
        /// <summary>
        /// Returns the next power of two that is larger than the specified number.
        /// </summary>
        /// <param name="n">The specified number.</param>
        /// <returns>The next power of two.</returns>
        public static int NextPowerOfTwo(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException("n", "Must be positive.");
            return (int)System.Math.Pow(2, System.Math.Ceiling(System.Math.Log((double)n, 2)));
        }

		/// <summary>
		/// Swaps the values of two variables.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		public static void Swap<T>(ref T first, ref T second)
		{
			T temp = first;
			first = second;
			second = temp;
		}

		/// <summary>
		/// Combines two hash codes.
		/// </summary>
		/// <param name="baseHash"></param>
		/// <param name="otherHash"></param>
		/// <returns></returns>
		public static void CombineHashCode(ref int baseHash, int otherHash)
		{
			unchecked { baseHash = baseHash * 23 + otherHash; }
		}
		/// <summary>
		/// Combines any number of hash codes.
		/// </summary>
		/// <param name="hashes"></param>
		/// <returns></returns>
		public static int CombineHashCode(params int[] hashes)
		{
			int result = hashes[0];
			unchecked
			{
				for (int i = 1; i < hashes.Length; i++)
				{
					result = result * 23 + hashes[i];
				}
			}
			return result;
		}

		/// <summary>
		/// Throws an ArgumentOutOfRangeException, if the specified value is NaN or Infinity.
		/// </summary>
		/// <param name="value"></param>
		public static void CheckValidValue(float value)
		{
			if (float.IsNaN(value) || float.IsInfinity(value))
			{
				throw new ArgumentOutOfRangeException("value", string.Format("Invalid float value detected: {0}", value));
			}
		}
		/// <summary>
		/// Throws an ArgumentOutOfRangeException, if the specified value is NaN or Infinity.
		/// </summary>
		/// <param name="value"></param>
		public static void CheckValidValue(Vector2 value)
		{
			if (float.IsNaN(value.X) || float.IsInfinity(value.X) ||
				float.IsNaN(value.Y) || float.IsInfinity(value.Y))
			{
				throw new ArgumentOutOfRangeException("value", string.Format("Invalid float value detected: {0}", value));
			}
		}
		/// <summary>
		/// Throws an ArgumentOutOfRangeException, if the specified value is NaN or Infinity.
		/// </summary>
		/// <param name="value"></param>
		public static void CheckValidValue(Vector3 value)
		{
			if (float.IsNaN(value.X) || float.IsInfinity(value.X) ||
				float.IsNaN(value.Y) || float.IsInfinity(value.Y) ||
				float.IsNaN(value.Z) || float.IsInfinity(value.Z))
			{
				throw new ArgumentOutOfRangeException("value", string.Format("Invalid float value detected: {0}", value));
			}
		}
	}
}
