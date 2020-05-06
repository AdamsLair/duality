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
	/// Represents a 3D vector using three single-precision floating-point numbers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3 : IEquatable<Vector3>
	{
		/// <summary>
		/// Defines a unit-length Vector3 that points towards the X-axis.
		/// </summary>
		public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
		/// <summary>
		/// Defines a unit-length Vector3 that points towards the Y-axis.
		/// </summary>
		public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
		/// <summary>
		/// /// Defines a unit-length Vector3 that points towards the Z-axis.
		/// </summary>
		public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
		/// <summary>
		/// Defines a zero-length Vector3.
		/// </summary>
		public static readonly Vector3 Zero = new Vector3(0, 0, 0);
		/// <summary>
		/// Defines an instance with all components set to 1.
		/// </summary>
		public static readonly Vector3 One = new Vector3(1, 1, 1);


		/// <summary>
		/// The X component of the Vector3.
		/// </summary>
		public float X;
		/// <summary>
		/// The Y component of the Vector3.
		/// </summary>
		public float Y;
		/// <summary>
		/// The Z component of the Vector3.
		/// </summary>
		public float Z;

		/// <summary>
		/// Gets or sets an OpenTK.Vector2 with the X and Y components of this instance.
		/// </summary>
		public Vector2 Xy { get { return new Vector2(this.X, this.Y); } set { this.X = value.X; this.Y = value.Y; } }

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value that will initialize this instance.</param>
		public Vector3(float value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
		}
		/// <summary>
		/// Constructs a new Vector3.
		/// </summary>
		/// <param name="x">The x component of the Vector3.</param>
		/// <param name="y">The y component of the Vector3.</param>
		/// <param name="z">The z component of the Vector3.</param>
		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		/// <summary>
		/// Constructs a new Vector3 from the given Vector2.
		/// </summary>
		/// <param name="v">The Vector2 to copy components from.</param>
		public Vector3(Vector2 v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = 0.0f;
		}
		/// <summary>
		/// Constructs a new Vector3 from the given Vector2.
		/// </summary>
		/// <param name="v">The Vector2 to copy components from.</param>
		/// <param name="z"></param>
		public Vector3(Vector2 v, float z)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = z;
		}

		/// <summary>
		/// Gets the length (magnitude) of the vector.
		/// </summary>
		/// <seealso cref="LengthSquared"/>
		public float Length
		{
			get
			{
				return (float)System.Math.Sqrt(
					this.X * this.X + 
					this.Y * this.Y + 
					this.Z * this.Z);
			}
		}
		/// <summary>
		/// Gets the square of the vector length (magnitude).
		/// </summary>
		/// <remarks>
		/// This property avoids the costly square root operation required by the Length property. This makes it more suitable
		/// for comparisons.
		/// </remarks>
		/// <seealso cref="Length"/>
		public float LengthSquared
		{
			get
			{
				return 
					this.X * this.X + 
					this.Y * this.Y + 
					this.Z * this.Z;
			}
		}
		/// <summary>
		/// Returns a normalized version of this vector.
		/// </summary>
		public Vector3 Normalized
		{
			get
			{
				float length = this.Length;
				if (length < 1e-15f) return Vector3.Zero;

				float scale = 1.0f / length;
				return new Vector3(
					this.X * scale,
					this.Y * scale,
					this.Z * scale);
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
					case 2: return this.Z;
					default: throw new IndexOutOfRangeException("Vector3 access at index: " + index);
				}
			}
			set
			{
				switch (index)
				{
					case 0: this.X = value; return;
					case 1: this.Y = value; return;
					case 2: this.Z = value; return;
					default: throw new IndexOutOfRangeException("Vector3 access at index: " + index);
				}
			}
		}

		/// <summary>
		/// Scales the Vector3 to unit length.
		/// </summary>
		public void Normalize()
		{
			float length = this.Length;
			if (length < 1e-15f)
			{
				this = Vector3.Zero;
		}
			else
			{
				float scale = 1.0f / length;
				this.X *= scale;
				this.Y *= scale;
				this.Z *= scale;
			}
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a">Left operand.</param>
		/// <param name="b">Right operand.</param>
		/// <param name="result">Result of operation.</param>
		public static void Add(ref Vector3 a, ref Vector3 b, out Vector3 result)
		{
			result = new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}
		/// <summary>
		/// Subtract one Vector from another
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">Result of subtraction</param>
		public static void Subtract(ref Vector3 a, ref Vector3 b, out Vector3 result)
		{
			result = new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}
		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector3 vector, float scale, out Vector3 result)
		{
			result = new Vector3(vector.X * scale, vector.Y * scale, vector.Z * scale);
		}
		/// <summary>
		/// Multiplies a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Multiply(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
		{
			result = new Vector3(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
		}
		/// <summary>
		/// Divides a vector by a scalar.
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector3 vector, float scale, out Vector3 result)
		{
			Multiply(ref vector, 1 / scale, out result);
		}
		/// <summary>
		/// Divide a vector by the components of a vector (scale).
		/// </summary>
		/// <param name="vector">Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <param name="result">Result of the operation.</param>
		public static void Divide(ref Vector3 vector, ref Vector3 scale, out Vector3 result)
		{
			result = new Vector3(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z);
		}

		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise minimum</returns>
		public static Vector3 Min(Vector3 a, Vector3 b)
		{
			a.X = a.X < b.X ? a.X : b.X;
			a.Y = a.Y < b.Y ? a.Y : b.Y;
			a.Z = a.Z < b.Z ? a.Z : b.Z;
			return a;
		}
		/// <summary>
		/// Calculate the component-wise minimum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise minimum</param>
		public static void Min(ref Vector3 a, ref Vector3 b, out Vector3 result)
		{
			result.X = a.X < b.X ? a.X : b.X;
			result.Y = a.Y < b.Y ? a.Y : b.Y;
			result.Z = a.Z < b.Z ? a.Z : b.Z;
		}

		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <returns>The component-wise maximum</returns>
		public static Vector3 Max(Vector3 a, Vector3 b)
		{
			a.X = a.X > b.X ? a.X : b.X;
			a.Y = a.Y > b.Y ? a.Y : b.Y;
			a.Z = a.Z > b.Z ? a.Z : b.Z;
			return a;
		}
		/// <summary>
		/// Calculate the component-wise maximum of two vectors
		/// </summary>
		/// <param name="a">First operand</param>
		/// <param name="b">Second operand</param>
		/// <param name="result">The component-wise maximum</param>
		public static void Max(ref Vector3 a, ref Vector3 b, out Vector3 result)
		{
			result.X = a.X > b.X ? a.X : b.X;
			result.Y = a.Y > b.Y ? a.Y : b.Y;
			result.Z = a.Z > b.Z ? a.Z : b.Z;
		}

		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The dot product of the two inputs</returns>
		public static float Dot(Vector3 left, Vector3 right)
		{
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}
		/// <summary>
		/// Calculate the dot (scalar) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <param name="result">The dot product of the two inputs</param>
		public static void Dot(ref Vector3 left, ref Vector3 right, out float result)
		{
			result = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}

		/// <summary>
		/// Caclulate the cross (vector) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The cross product of the two inputs</returns>
		public static Vector3 Cross(Vector3 left, Vector3 right)
		{
			Vector3 result;
			Cross(ref left, ref right, out result);
			return result;
		}
		/// <summary>
		/// Caclulate the cross (vector) product of two vectors
		/// </summary>
		/// <param name="left">First operand</param>
		/// <param name="right">Second operand</param>
		/// <returns>The cross product of the two inputs</returns>
		/// <param name="result">The cross product of the two inputs</param>
		public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
		{
			result = new Vector3(left.Y * right.Z - left.Z * right.Y,
				left.Z * right.X - left.X * right.Z,
				left.X * right.Y - left.Y * right.X);
		}

		/// <summary>
		/// Calculates the distance between two points described by two vectors. 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		public static float Distance(ref Vector3 left, ref Vector3 right)
		{
			Vector3 diff;
			diff.X = left.X - right.X;
			diff.Y = left.Y - right.Y;
			diff.Z = left.Z - right.Z;
			return (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y + diff.Z * diff.Z);
		}

		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
		public static Vector3 Lerp(Vector3 a, Vector3 b, float blend)
		{
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			a.Z = blend * (b.Z - a.Z) + a.Z;
			return a;
		}
		/// <summary>
		/// Returns a new Vector that is the linear blend of the 2 given Vectors
		/// </summary>
		/// <param name="a">First input vector</param>
		/// <param name="b">Second input vector</param>
		/// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
		/// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
		public static void Lerp(ref Vector3 a, ref Vector3 b, float blend, out Vector3 result)
		{
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
			result.Z = blend * (b.Z - a.Z) + a.Z;
		}

		/// <summary>
		/// Calculates the angle (in radians) between two vectors.
		/// </summary>
		/// <param name="first">The first vector.</param>
		/// <param name="second">The second vector.</param>
		/// <returns>Angle (in radians) between the vectors.</returns>
		/// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
		public static float AngleBetween(Vector3 first, Vector3 second)
		{
			return (float)System.Math.Acos((Vector3.Dot(first, second)) / (first.Length * second.Length));
		}
		/// <summary>
		/// Calculates the angle (in radians) between two vectors.
		/// </summary>
		/// <param name="first">The first vector.</param>
		/// <param name="second">The second vector.</param>
		/// <param name="result">Angle (in radians) between the vectors.</param>
		/// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
		public static void AngleBetween(ref Vector3 first, ref Vector3 second, out float result)
		{
			float temp;
			Vector3.Dot(ref first, ref second, out temp);
			result = (float)System.Math.Acos(temp / (first.Length * second.Length));
		}

		/// <summary>
		/// Transform a Vector by the given Matrix</summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <returns>The transformed vector</returns>
		public static Vector3 Transform(Vector3 vec, Matrix4 mat)
		{
			Vector3 result;
			Transform(ref vec, ref mat, out result);
			return result;
		}
		/// <summary>
		/// Transform a Vector by the given Matrix</summary>
		/// <param name="vec">The vector to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <param name="result">The transformed vector</param>
		public static void Transform(ref Vector3 vec, ref Matrix4 mat, out Vector3 result)
		{
			Vector4 row0 = mat.Row0;
			Vector4 row1 = mat.Row1;
			Vector4 row2 = mat.Row2;
			Vector4 row3 = mat.Row3;
			result.X = vec.X * row0.X + vec.Y * row1.X + vec.Z * row2.X + row3.X;
			result.Y = vec.X * row0.Y + vec.Y * row1.Y + vec.Z * row2.Y + row3.Y;
			result.Z = vec.X * row0.Z + vec.Y * row1.Z + vec.Z * row2.Z + row3.Z;
		}
		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <returns>The result of the operation.</returns>
		public static Vector3 Transform(Vector3 vec, Quaternion quat)
		{
			Vector3 result;
			Transform(ref vec, ref quat, out result);
			return result;
		}
		/// <summary>
		/// Transforms a vector by a quaternion rotation.
		/// </summary>
		/// <param name="vec">The vector to transform.</param>
		/// <param name="quat">The quaternion to rotate the vector by.</param>
		/// <param name="result">The result of the operation.</param>
		public static void Transform(ref Vector3 vec, ref Quaternion quat, out Vector3 result)
		{
			// Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
			// vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
			Vector3 xyz = quat.Xyz, temp, temp2;
			Vector3.Cross(ref xyz, ref vec, out temp);
			Vector3.Multiply(ref vec, quat.W, out temp2);
			Vector3.Add(ref temp, ref temp2, out temp);
			Vector3.Cross(ref xyz, ref temp, out temp);
			Vector3.Multiply(ref temp, 2, out temp);
			Vector3.Add(ref vec, ref temp, out result);
		}

		/// <summary>
		/// Adds two instances.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3 operator +(Vector3 left, Vector3 right)
		{
			return new Vector3(
				left.X + right.X, 
				left.Y + right.Y, 
				left.Z + right.Z);
		}
		/// <summary>
		/// Subtracts two instances.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3 operator -(Vector3 left, Vector3 right)
		{
			return new Vector3(
				left.X - right.X, 
				left.Y - right.Y, 
				left.Z - right.Z);
		}
		/// <summary>
		/// Negates an instance.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3 operator -(Vector3 vec)
		{
			return new Vector3(
				-vec.X, 
				-vec.Y, 
				-vec.Z);
		}
		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3 operator *(Vector3 vec, float scale)
		{
			return new Vector3(
				vec.X * scale, 
				vec.Y * scale, 
				vec.Z * scale);
		}
		/// <summary>
		/// Multiplies an instance by a scalar.
		/// </summary>
		/// <param name="scale">The scalar.</param>
		/// <param name="vec">The instance.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3 operator *(float scale, Vector3 vec)
		{
			return vec * scale;
		}
		/// <summary>
		/// Scales an instance by a vector.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="scale">The scale.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3 operator *(Vector3 vec, Vector3 scale)
		{
			return new Vector3(
				vec.X * scale.X, 
				vec.Y * scale.Y, 
				vec.Z * scale.Z);
		}
		/// <summary>
		/// Divides an instance by a scalar.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3 operator /(Vector3 vec, float scale)
		{
			return vec * (1.0f / scale);
		}
		/// <summary>
		/// Divides an instance by a vector.
		/// </summary>
		/// <param name="vec">The instance.</param>
		/// <param name="scale">The scalar.</param>
		/// <returns>The result of the calculation.</returns>
		public static Vector3 operator /(Vector3 vec, Vector3 scale)
		{
			return new Vector3(
				vec.X / scale.X, 
				vec.Y / scale.Y, 
				vec.Z / scale.Z);
		}
		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left equals right; false otherwise.</returns>
		public static bool operator ==(Vector3 left, Vector3 right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True, if left does not equa lright; false otherwise.</returns>
		public static bool operator !=(Vector3 left, Vector3 right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a System.String that represents the current Vector3.
		/// </summary>
		public override string ToString()
		{
			return string.Format("({0}, {1}, {2})", this.X, this.Y, this.Z);
		}
		/// <summary>
		/// Returns the hashcode for this instance.
		/// </summary>
		/// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}
		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>True if the instances are equal; false otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Vector3))
				return false;

			return this.Equals((Vector3)obj);
		}

		/// <summary>
		/// Indicates whether the current vector is equal to another vector.
		/// </summary>
		/// <param name="other">A vector to compare with this vector.</param>
		/// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
		public bool Equals(Vector3 other)
		{
			return
				this.X == other.X &&
				this.Y == other.Y &&
				this.Z == other.Z;
		}
	}
}
