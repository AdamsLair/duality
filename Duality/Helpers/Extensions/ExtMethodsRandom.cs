using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Duality.ColorFormat;

namespace Duality
{
	/// <summary>
	/// Provides extension methods for <see cref="System.Random">random number generators</see>.
	/// </summary>
	public static class ExtMethodsRandom
	{
		/// <summary>
		/// Returns a random byte.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <returns></returns>
		public static byte NextByte(this Random r)
		{
			return (byte)(r.Next() % 256);
		}
		/// <summary>
		/// Returns a random byte.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="max">Exclusive maximum value.</param>
		/// <returns></returns>
		public static byte NextByte(this Random r, byte max)
		{
			return (byte)(r.Next() % ((int)max + 1));
		}
		/// <summary>
		/// Returns a random byte.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="min">Inclusive minimum value.</param>
		/// <param name="max">Exclusive maximum value.</param>
		/// <returns></returns>
		public static byte NextByte(this Random r, byte min, byte max)
		{
			return (byte)(min + (r.Next() % ((int)max - min + 1)));
		}

		/// <summary>
		/// Returns a random float.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <returns></returns>
		public static float NextFloat(this Random r)
		{
			return (float)r.NextDouble();
		}
		/// <summary>
		/// Returns a random float.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="max">Exclusive maximum value.</param>
		/// <returns></returns>
		public static float NextFloat(this Random r, float max)
		{
			return max * (float)r.NextDouble();
		}
		/// <summary>
		/// Returns a random float.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="min">Inclusive minimum value.</param>
		/// <param name="max">Exclusive maximum value.</param>
		/// <returns></returns>
		public static float NextFloat(this Random r, float min, float max)
		{
			return min + (max - min) * (float)r.NextDouble();
		}

		/// <summary>
		/// Returns a random bool.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <returns></returns>
		public static bool NextBool(this Random r)
		{
			return r.NextDouble() > 0.5d;
		}
		
		/// <summary>
		/// Returns a random <see cref="Vector2"/> with length one.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <returns></returns>
		public static Vector2 NextVector2(this Random r)
		{
			float angle = r.NextFloat(0.0f, MathF.RadAngle360);
			return new Vector2(MathF.Sin(angle), -MathF.Cos(angle));
		}
		/// <summary>
		/// Returns a random <see cref="Vector2"/>.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="radius">Length of the vector.</param>
		/// <returns></returns>
		public static Vector2 NextVector2(this Random r, float radius)
		{
			float angle = r.NextFloat(0.0f, MathF.RadAngle360);
			return new Vector2(MathF.Sin(angle), -MathF.Cos(angle)) * radius;
		}
		/// <summary>
		/// Returns a random <see cref="Vector2"/>.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="minRadius">Minimum length of the vector</param>
		/// <param name="maxRadius">Maximum length of the vector</param>
		/// <returns></returns>
		public static Vector2 NextVector2(this Random r, float minRadius, float maxRadius)
		{
			return r.NextVector2(r.NextFloat(minRadius, maxRadius));
		}
		/// <summary>
		/// Returns a random <see cref="Vector2"/> pointing to a position inside the specified rect.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="x">Rectangle that contains the random vector.</param>
		/// <param name="y">Rectangle that contains the random vector.</param>
		/// <param name="w">Rectangle that contains the random vector.</param>
		/// <param name="h">Rectangle that contains the random vector.</param>
		/// <returns></returns>
		public static Vector2 NextVector2(this Random r, float x, float y, float w, float h)
		{
			return new Vector2(r.NextFloat(x, x + w), r.NextFloat(y, y + h));
		}
		/// <summary>
		/// Returns a random <see cref="Vector2"/> pointing to a position inside the specified rect.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="rect">Rectangle that contains the random vector.</param>
		/// <returns></returns>
		public static Vector2 NextVector2(this Random r, Rect rect)
		{
			return new Vector2(r.NextFloat(rect.X, rect.X + rect.W), r.NextFloat(rect.Y, rect.Y + rect.H));
		}
		
		/// <summary>
		/// Returns a random <see cref="Vector3"/> with length one.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <returns></returns>
		public static Vector3 NextVector3(this Random r)
		{
			Quaternion rot = Quaternion.Identity;
			rot *= Quaternion.FromAxisAngle(Vector3.UnitZ, r.NextFloat(MathF.RadAngle360));
			rot *= Quaternion.FromAxisAngle(Vector3.UnitX, r.NextFloat(MathF.RadAngle360));
			rot *= Quaternion.FromAxisAngle(Vector3.UnitY, r.NextFloat(MathF.RadAngle360));
			return Vector3.Transform(Vector3.UnitX, rot);
		}
		/// <summary>
		/// Returns a random <see cref="Vector3"/>.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="radius">Maximum length of the vector.</param>
		/// <returns></returns>
		public static Vector3 NextVector3(this Random r, float radius)
		{
			Quaternion rot = Quaternion.Identity;
			rot *= Quaternion.FromAxisAngle(Vector3.UnitZ, r.NextFloat(MathF.RadAngle360));
			rot *= Quaternion.FromAxisAngle(Vector3.UnitX, r.NextFloat(MathF.RadAngle360));
			rot *= Quaternion.FromAxisAngle(Vector3.UnitY, r.NextFloat(MathF.RadAngle360));
			return Vector3.Transform(new Vector3(radius, 0, 0), rot);
		}
		/// <summary>
		/// Returns a random <see cref="Vector3"/>.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="minRadius">Minimum length of the vector</param>
		/// <param name="maxRadius">Maximum length of the vector</param>
		/// <returns></returns>
		public static Vector3 NextVector3(this Random r, float minRadius, float maxRadius)
		{
			return r.NextVector3(r.NextFloat(minRadius, maxRadius));
		}
		/// <summary>
		/// Returns a random <see cref="Vector3"/> pointing to a position inside the specified cube.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <param name="x">Cube that contains the random vector.</param>
		/// <param name="y">Cube that contains the random vector.</param>
		/// <param name="w">Cube that contains the random vector.</param>
		/// <param name="h">Cube that contains the random vector.</param>
		/// <returns></returns>
		public static Vector3 NextVector3(this Random r, float x, float y, float z, float w, float h, float d)
		{
			return new Vector3(r.NextFloat(x, x + w), r.NextFloat(y, y + h), r.NextFloat(z, z + d));
		}

		/// <summary>
		/// Returns a random <see cref="ColorRgba"/>.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <returns></returns>
		public static ColorRgba NextColorRGBA(this Random r)
		{
			return new ColorRgba(
				r.NextByte(),
				r.NextByte(),
				r.NextByte(),
				255);
		}
		/// <summary>
		/// Returns a random <see cref="ColorHsva"/>.
		/// </summary>
		/// <param name="r">A random number generator.</param>
		/// <returns></returns>
		public static ColorHsva NextColorHSVA(this Random r)
		{
			return new ColorHsva(
				r.NextFloat(),
				r.NextFloat(),
				r.NextFloat(),
				1.0f);
		}

		/// <summary>
		/// Returns a random value from a weighted value pool.
		/// </summary>
		/// <typeparam name="T">Type of the random values.</typeparam>
		/// <param name="r">A random number generator.</param>
		/// <param name="values">A pool of values.</param>
		/// <param name="weights">One weight for each value in the pool.</param>
		/// <returns></returns>
		public static T OneOfWeighted<T>(this Random r, IEnumerable<T> values, IEnumerable<float> weights)
		{
			float totalWeight = weights.Sum();
			float pickedWeight = r.NextFloat(totalWeight);
			
			IEnumerator<T> valEnum = values.GetEnumerator();
			if (!valEnum.MoveNext()) return default(T);

			foreach (float w in weights)
			{
				pickedWeight -= w;
				if (pickedWeight < 0.0f) return valEnum.Current;
				valEnum.MoveNext();
			}

			return default(T);
		}
		/// <summary>
		/// Returns a random value from a weighted value pool.
		/// </summary>
		/// <typeparam name="T">Type of the random values.</typeparam>
		/// <param name="r">A random number generator.</param>
		/// <param name="values">A pool of values.</param>
		/// <param name="weights">One weight for each value in the pool.</param>
		/// <returns></returns>
		public static T OneOfWeighted<T>(this Random r, IEnumerable<T> values, params float[] weights)
		{
			return OneOfWeighted<T>(r, values, weights as IEnumerable<float>);
		}
		/// <summary>
		/// Returns a random value from a weighted value pool.
		/// </summary>
		/// <typeparam name="T">Type of the random values.</typeparam>
		/// <param name="r">A random number generator.</param>
		/// <param name="weightedValues">A weighted value pool.</param>
		/// <returns></returns>
		public static T OneOfWeighted<T>(this Random r, IEnumerable<KeyValuePair<T,float>> weightedValues)
		{
			float totalWeight = weightedValues.Sum(v => v.Value);
			float pickedWeight = r.NextFloat(totalWeight);
			
			foreach (KeyValuePair<T,float> pair in weightedValues)
			{
				pickedWeight -= pair.Value;
				if (pickedWeight < 0.0f) return pair.Key;
			}

			return default(T);
		}
		/// <summary>
		/// Returns a random value from a weighted value pool.
		/// </summary>
		/// <typeparam name="T">Type of the random values.</typeparam>
		/// <param name="r">A random number generator.</param>
		/// <param name="weightedValues">A weighted value pool.</param>
		/// <returns></returns>
		public static T OneOfWeighted<T>(this Random r, params KeyValuePair<T,float>[] weightedValues)
		{
			return OneOfWeighted<T>(r, weightedValues as IEnumerable<KeyValuePair<T,float>>);
		}
		/// <summary>
		/// Returns a random value from a weighted value pool.
		/// </summary>
		/// <typeparam name="T">Type of the random values.</typeparam>
		/// <param name="r">A random number generator.</param>
		/// <param name="values">A pool of values.</param>
		/// <param name="weightFunc">A weight function that provides a weight for each value from the pool.</param>
		/// <returns></returns>
		public static T OneOfWeighted<T>(this Random r, IEnumerable<T> values, Func<T,float> weightFunc)
		{
			return OneOfWeighted<T>(r, values, values.Select(v => weightFunc(v)));
		}

		/// <summary>
		/// Returns one randomly selected element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="r"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static T OneOf<T>(this Random r, IEnumerable<T> values)
		{
			return values.ElementAt(r.Next(values.Count()));
		}
	}
}
