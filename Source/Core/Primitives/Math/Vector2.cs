#region --- License ---
/*
Copyright (c) 2006 - 2008 The Open Toolkit library.

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Note: This code has been heavily modified for the Duality framework.

	*/
#endregion

using System;
using System.Runtime.InteropServices;

namespace Duality
{
	/// <summary>
	/// Represents a 2D vector using two single-precision floating-point numbers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2 : IEquatable<Vector2>
	{
		/// <summary>
		/// Defines a unit-length Vector2 that points along the X-axis.
		/// </summary>
		public static readonly Vector2 UnitX = new Vector2(1, 0);
		/// <summary>
		/// Defines a unit-length Vector2 that points along the Y-axis.
		/// </summary>
		public static readonly Vector2 UnitY = new Vector2(0, 1);
		/// <summary>
		/// Defines a zero-length Vector2.
		/// </summary>
		public static readonly Vector2 Zero = new Vector2(0, 0);
		/// <summary>
		/// Defines an instance with all components set to 1.
		/// </summary>
		public static readonly Vector2 One = new Vector2(1, 1);

		/// <summary>
		/// The X component of the Vector2.
		/// </summary>
		public float X;
		/// <summary>
		/// The Y component of the Vector2.
		/// </summary>
		public float Y;

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector2(float value)
		{
			X = value;
			Y = value;
		}
		/// <summary>
		/// Constructs a new Vector2.
		/// </summary>
		/// <param name="x">The x coordinate of the net Vector2.</param>
		/// <param name="y">The y coordinate of the net Vector2.</param>
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}
		/// <summary>
		/// Constructs a new vector from angle and length.
		/// </summary>
		/// <param name="angle"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static Vector2 FromAngleLength(float angle, float length)
		{
			return new Vector2((float)Math.Sin(angle) * length, (float)Math.Cos(angle) * -length);
		}

		/// <summary>
		/// Gets the length (magnitude) of the vector.
		/// </summary>
		/// <seealso cref="LengthSquared"/>
		public float Length
		{
			get
			{
				return (float)System.Math.Sqrt(X * X + Y * Y);
			}
		}
		/// <summary>
		/// Gets the square of the vector length (magnitude).
		/// </summary>
		/// <remarks>
		/// This property avoids the costly square root operation required by the Length property. This makes it more suitable
		/// for comparisons.
		/// </remarks>
		/// <see cref="Length"/>
		public float LengthSquared
		{
			get
			{
				return X * X + Y * Y;
			}
		}
		/// <summary>
		/// Returns the vectors angle
		/// </summary>
		public float Angle
		{
			get
			{
				return (float)((Math.Atan2(Y, X) + Math.PI * 2.5) % (Math.PI * 2));
			}
		}

		/// <summary>
		/// Gets the perpendicular vector on the right side of this vector.
		/// </summary>
		public Vector2 PerpendicularRight
		{
			get
			{
				return new Vector2(-Y, X);
			}
		}
		/// <summary>
		/// Gets the perpendicular vector on the left side of this vector.
		/// </summary>
		public Vector2 PerpendicularLeft
		{
			get
			{
				return new Vector2(Y, -X);
			}
		}
		/// <summary>
		/// Returns a normalized version of this vector.
		/// </summary>
		public Vector2 Normalized
		{
			get
			{
				float length = this.Length;
				if (length < 1e-15f) return Vector2.Zero;

				float scale = 1.0f / length;
				return new Vector2(
					this.X * scale, 
					this.Y * scale);
			}
		}

		/// <summary>
		/// Gets or sets the value at the index of the Vector.
		/// </summary>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return this.X;
					case 1: return this.Y;
					default: throw new IndexOutOfRangeException("Vector2 access at index: " + index);
				}
			}
			set
			{
				switch (index)
				{
					case 0: this.X = value; return;
					case 1: this.Y = value; return;
					default: throw new IndexOutOfRangeException("Vector2 access at index: " + index);
				}
			}
		}


		/// <summary>
		/// Scales the Vector2 to unit length.
		/// </summary>
		public void Normalize()
		{
			float length = this.Length;
			if (length < 1e-15f)
			{
				this = Vector2.Zero;
			}
			else
			{
				float scale = 1.0f / length;
				this.X *= scale;
				this.Y *= scale;
			}
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <param name="result">Result of operation.</param>
		public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result)
		{
			result = new Vector2(a.X + b.X, a.Y + b.Y);
		}
		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">Result of subtraction</param>
		public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result)
		{
			result = new Vector2(a.X - b.X, a.Y - b.Y);
		}
		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector2 vector, float scale, out Vector2 result)
		{
			result = new Vector2(vector.X * scale, vector.Y * scale);
		}
		/// <summary>
		/// Multiplies a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
		{
			result = new Vector2(vector.X * scale.X, vector.Y * scale.Y);
		}
		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector2 vector, float scale, out Vector2 result)
		{
			Multiply(ref vector, 1 / scale, out result);
		}
		/// <summary>
		/// Divide a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result)
		{
			result = new Vector2(vector.X / scale.X, vector.Y / scale.Y);
		}

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise minimum</returns>
		public static Vector2 Min(Vector2 a, Vector2 b)
		{
			a.X = a.X < b.X ? a.X : b.X;
			a.Y = a.Y < b.Y ? a.Y : b.Y;
			return a;
		}
		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise minimum</param>
		public static void Min(ref Vector2 a, ref Vector2 b, out Vector2 result)
		{
			result.X = a.X < b.X ? a.X : b.X;
			result.Y = a.Y < b.Y ? a.Y : b.Y;
		}

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise maximum</returns>
		public static Vector2 Max(Vector2 a, Vector2 b)
		{
			a.X = a.X > b.X ? a.X : b.X;
			a.Y = a.Y > b.Y ? a.Y : b.Y;
			return a;
		}
		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise maximum</param>
		public static void Max(ref Vector2 a, ref Vector2 b, out Vector2 result)
		{
			result.X = a.X > b.X ? a.X : b.X;
			result.Y = a.Y > b.Y ? a.Y : b.Y;
		}

		/// <summary>
		/// Calculates the cross product of the vectors
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static float Cross(Vector2 left, Vector2 right)
		{
			return Cross(left.X, left.Y, right.X, right.Y);
		}

		/// <summary>
		/// Calculates the cross product
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <returns></returns>
		public static float Cross(float x1, float y1, float x2, float y2)
		{
			return x1 * y2 - y1 * x2;
		}

		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The dot product of the two inputs</returns>
		public static float Dot(Vector2 left, Vector2 right)
		{
			return left.X * right.X + left.Y * right.Y;
		}
		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <param name="result">The dot product of the two inputs</param>
		public static void Dot(ref Vector2 left, ref Vector2 right, out float result)
		{
			result = left.X * right.X + left.Y * right.Y;
		}

		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
		public static Vector2 Lerp(Vector2 a, Vector2 b, float blend)
		{
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			return a;
		}
		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
		public static void Lerp(ref Vector2 a, ref Vector2 b, float blend, out Vector2 result)
		{
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
		}

		/// <summary>
		/// Calculates the angle (in radians) between two vectors.
		/// </summary>
		/// <param name="first">The first vector.</param>
		/// <param name="second">The second vector.</param>
		/// <returns>Angle (in radians) between the vectors.</returns>
		/// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
		public static float AngleBetween(Vector2 first, Vector2 second)
		{
			return (float)System.Math.Acos((Vector2.Dot(first, second)) / (first.Length * second.Length));
		}
		/// <summary>
		/// Calculates the angle (in radians) between two vectors.
		/// </summary>
		/// <param name="first">The first vector.</param>
		/// <param name="second">The second vector.</param>
		/// <param name="result">Angle (in radians) between the vectors.</param>
		/// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
		public static void AngleBetween(ref Vector2 first, ref Vector2 second, out float result)
		{
			float temp;
			Vector2.Dot(ref first, ref second, out temp);
			result = (float)System.Math.Acos(temp / (first.Length * second.Length));
		}
		
		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector2 Transform(Vector2 vec, Quaternion quat)
		{
			Vector2 result;
			Transform(ref vec, ref quat, out result);
			return result;
		}
		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <param name="result">The result of the operation.</param>
		public static void Transform(ref Vector2 vec, ref Quaternion quat, out Vector2 result)
		{
			Quaternion v = new Quaternion(vec.X, vec.Y, 0, 0), i, t;
			Quaternion.Invert(ref quat, out i);
			Quaternion.Multiply(ref quat, ref v, out t);
			Quaternion.Multiply(ref t, ref i, out v);

			result = new Vector2(v.X, v.Y);
		}
		/// <summary>
		/// Transforms the vector
		/// </summary>
		/// <param name="vec"></param>
		/// <param name="mat"></param>
		/// <returns></returns>
		public static Vector2 Transform(Vector2 vec, Matrix4 mat)
		{
			Vector2 result;
			Transform(ref vec, ref mat, out result);
			return result;
		}
		/// <summary>
		/// Transforms the vector
		/// </summary>
		/// <param name="vec"></param>
		/// <param name="mat"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static void Transform(ref Vector2 vec, ref Matrix4 mat, out Vector2 result)
		{
			Vector4 row0 = mat.Row0;
			Vector4 row1 = mat.Row1;
			Vector4 row3 = mat.Row3;
			result.X = vec.X * row0.X + vec.Y * row1.X + row3.X;
			result.Y = vec.X * row0.Y + vec.Y * row1.Y + row3.Y;
		}

		/// <summary>
		/// Adds the specified instances.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		public static Vector2 operator +(Vector2 left, Vector2 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}
		/// <summary>
		/// Subtracts the specified instances.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		public static Vector2 operator -(Vector2 left, Vector2 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}
		/// <summary>
		/// Negates the specified instance.
		/// </summary>
		/// <param name="vec">Operand.</param>
		/// <returns>Result of negation.</returns>
		public static Vector2 operator -(Vector2 vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			return vec;
		}
		/// <summary>
		/// Multiplies the specified instance by a scalar.
		/// </summary>
		/// <param name="vec">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Vector2 operator *(Vector2 vec, float scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}
		/// <summary>
		/// Multiplies the specified instance by a scalar.
		/// </summary>
		/// <param name="scale">Left operand.</param>
		/// <param name="vec">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Vector2 operator *(float scale, Vector2 vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}
		/// <summary>
		/// Scales the specified instance by a vector.
		/// </summary>
		/// <param name="vec">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Vector2 operator *(Vector2 vec, Vector2 scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			return vec;
		}
		/// <summary>
		/// Divides the specified instance by a scalar.
		/// </summary>
		/// <param name="vec">Left operand</param>
		/// <param name="scale">Right operand</param>
		/// <returns>Result of the division.</returns>
		public static Vector2 operator /(Vector2 vec, float scale)
		{
			float mult = 1.0f / scale;
			vec.X *= mult;
			vec.Y *= mult;
			return vec;
		}
		/// <summary>
		/// Divides the specified instance by a vector.
		/// </summary>
		/// <param name="vec">Left operand</param>
		/// <param name="scale">Right operand</param>
		/// <returns>Result of the division.</returns>
		public static Vector2 operator /(Vector2 vec, Vector2 scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			return vec;
		}
		/// <summary>
		/// Compares the specified instances for equality.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True if both instances are equal; false otherwise.</returns>
		public static bool operator ==(Vector2 left, Vector2 right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Compares the specified instances for inequality.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True if both instances are not equal; false otherwise.</returns>
		public static bool operator !=(Vector2 left, Vector2 right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a System.String that represents the current Vector2.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("({0:F}, {1:F})", X, Y);
		}
		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}
		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Vector2))
				return false;

			return this.Equals((Vector2)obj);
		}

		/// <summary>
		/// Indicates whether the current vector is equal to another vector.
		/// </summary>
		/// <param name="other">A vector to compare with this vector.</param>
		/// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
		public bool Equals(Vector2 other)
		{
			return
				X == other.X &&
				Y == other.Y;
		}
	}
}
