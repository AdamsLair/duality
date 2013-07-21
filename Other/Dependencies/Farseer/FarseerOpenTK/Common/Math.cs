/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK;

namespace FarseerPhysics.Common
{
    public static class MathUtils
    {
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2 Cross(Vector2 a, float s)
        {
            return new Vector2(s * a.Y, -s * a.X);
        }

        public static Vector2 Cross(float s, Vector2 a)
        {
            return new Vector2(-s * a.Y, s * a.X);
        }

        public static Vector2 Abs(Vector2 v)
        {
            return new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
        }

        public static Vector2 Multiply(ref Mat22 A, Vector2 v)
        {
            return Multiply(ref A, ref v);
        }

        public static Vector2 Multiply(ref Mat22 A, ref Vector2 v)
        {
            return new Vector2(A.Col1.X * v.X + A.Col2.X * v.Y, A.Col1.Y * v.X + A.Col2.Y * v.Y);
        }

        public static Vector2 MultiplyT(ref Mat22 A, Vector2 v)
        {
            return MultiplyT(ref A, ref v);
        }

        public static Vector2 MultiplyT(ref Mat22 A, ref Vector2 v)
        {
            return new Vector2(v.X * A.Col1.X + v.Y * A.Col1.Y, v.X * A.Col2.X + v.Y * A.Col2.Y);
        }

        public static Vector2 Multiply(ref Transform T, Vector2 v)
        {
            return Multiply(ref T, ref v);
        }

        public static Vector2 Multiply(ref Transform T, ref Vector2 v)
        {
            return new Vector2(T.Position.X + T.R.Col1.X * v.X + T.R.Col2.X * v.Y,
                               T.Position.Y + T.R.Col1.Y * v.X + T.R.Col2.Y * v.Y);
        }

        public static Vector2 MultiplyT(ref Transform T, Vector2 v)
        {
            return MultiplyT(ref T, ref v);
        }

        public static Vector2 MultiplyT(ref Transform T, ref Vector2 v)
        {
            Vector2 tmp = Vector2.Zero;
            tmp.X = v.X - T.Position.X;
            tmp.Y = v.Y - T.Position.Y;
            return MultiplyT(ref T.R, ref tmp);
        }

        // A^T * B
        public static void MultiplyT(ref Mat22 A, ref Mat22 B, out Mat22 C)
        {
            C = new Mat22();
            C.Col1.X = A.Col1.X * B.Col1.X + A.Col1.Y * B.Col1.Y;
            C.Col1.Y = A.Col2.X * B.Col1.X + A.Col2.Y * B.Col1.Y;
            C.Col2.X = A.Col1.X * B.Col2.X + A.Col1.Y * B.Col2.Y;
            C.Col2.Y = A.Col2.X * B.Col2.X + A.Col2.Y * B.Col2.Y;
        }

        // v2 = A.R' * (B.R * v1 + B.p - A.p) = (A.R' * B.R) * v1 + (B.p - A.p)
        public static void MultiplyT(ref Transform A, ref Transform B, out Transform C)
        {
            C = new Transform();
            MultiplyT(ref A.R, ref B.R, out C.R);
            C.Position.X = B.Position.X - A.Position.X;
            C.Position.Y = B.Position.Y - A.Position.Y;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        /// <summary>
        /// This function is used to ensure that a floating point number is
        /// not a NaN or infinity.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>
        /// 	<c>true</c> if the specified x is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValid(float x)
        {
            if (float.IsNaN(x))
            {
                // NaN.
                return false;
            }

            return !float.IsInfinity(x);
        }

        public static bool IsValid(this Vector2 x)
        {
            return IsValid(x.X) && IsValid(x.Y);
        }

        /// <summary>
        /// This is a approximate yet fast inverse square-root.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static float InvSqrt(float x)
        {
            FloatConverter convert = new FloatConverter();
            convert.x = x;
            float xhalf = 0.5f * x;
            convert.i = 0x5f3759df - (convert.i >> 1);
            x = convert.x;
            x = x * (1.5f - xhalf * x * x);
            return x;
        }

        public static int Clamp(int a, int low, int high)
        {
            return Math.Max(low, Math.Min(a, high));
        }

        public static float Clamp(float a, float low, float high)
        {
            return Math.Max(low, Math.Min(a, high));
        }

        public static Vector2 Clamp(Vector2 a, Vector2 low, Vector2 high)
        {
            return Vector2.ComponentMax(low, Vector2.ComponentMin(a, high));
        }

        public static void Cross(ref Vector2 a, ref Vector2 b, out float c)
        {
            c = a.X * b.Y - a.Y * b.X;
        }

        /// <summary>
        /// Return the angle between two vectors on a plane
        /// The angle is from vector 1 to vector 2, positive anticlockwise
        /// The result is between -pi -> pi
        /// </summary>
        public static double VectorAngle(ref Vector2 p1, ref Vector2 p2)
        {
            double theta1 = Math.Atan2(p1.Y, p1.X);
            double theta2 = Math.Atan2(p2.Y, p2.X);
            double dtheta = theta2 - theta1;
            while (dtheta > Math.PI)
                dtheta -= (2 * Math.PI);
            while (dtheta < -Math.PI)
                dtheta += (2 * Math.PI);

            return (dtheta);
        }

        public static double VectorAngle(Vector2 p1, Vector2 p2)
        {
            return VectorAngle(ref p1, ref p2);
        }

        /// <summary>
        /// Returns a positive number if c is to the left of the line going from a to b.
        /// </summary>
        /// <returns>Positive number if point is left, negative if point is right, 
        /// and 0 if points are collinear.</returns>
        public static float Area(Vector2 a, Vector2 b, Vector2 c)
        {
            return Area(ref a, ref b, ref c);
        }

        /// <summary>
        /// Returns a positive number if c is to the left of the line going from a to b.
        /// </summary>
        /// <returns>Positive number if point is left, negative if point is right, 
        /// and 0 if points are collinear.</returns>
        public static float Area(ref Vector2 a, ref Vector2 b, ref Vector2 c)
        {
            return a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y);
        }

        /// <summary>
        /// Determines if three vertices are collinear (ie. on a straight line)
        /// </summary>
        /// <param name="a">First vertex</param>
        /// <param name="b">Second vertex</param>
        /// <param name="c">Third vertex</param>
        /// <returns></returns>
        public static bool Collinear(ref Vector2 a, ref Vector2 b, ref Vector2 c)
        {
            return Collinear(ref a, ref b, ref c, 0);
        }

        public static bool Collinear(ref Vector2 a, ref Vector2 b, ref Vector2 c, float tolerance)
        {
            return FloatInRange(Area(ref a, ref b, ref c), -tolerance, tolerance);
        }

        public static void Cross(float s, ref Vector2 a, out Vector2 b)
        {
            b = new Vector2(-s * a.Y, s * a.X);
        }

        public static bool FloatEquals(float value1, float value2)
        {
            return Math.Abs(value1 - value2) <= Settings.Epsilon;
        }

        /// <summary>
        /// Checks if a floating point Value is equal to another,
        /// within a certain tolerance.
        /// </summary>
        /// <param name="value1">The first floating point Value.</param>
        /// <param name="value2">The second floating point Value.</param>
        /// <param name="delta">The floating point tolerance.</param>
        /// <returns>True if the values are "equal", false otherwise.</returns>
        public static bool FloatEquals(float value1, float value2, float delta)
        {
            return FloatInRange(value1, value2 - delta, value2 + delta);
        }

        /// <summary>
        /// Checks if a floating point Value is within a specified
        /// range of values (inclusive).
        /// </summary>
        /// <param name="value">The Value to check.</param>
        /// <param name="min">The minimum Value.</param>
        /// <param name="max">The maximum Value.</param>
        /// <returns>True if the Value is within the range specified,
        /// false otherwise.</returns>
        public static bool FloatInRange(float value, float min, float max)
        {
            return (value >= min && value <= max);
        }

        #region Nested type: FloatConverter

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatConverter
        {
            [FieldOffset(0)]
            public float x;
            [FieldOffset(0)]
            public int i;
        }

        #endregion
    }

    /// <summary>
    /// A 2-by-2 matrix. Stored in column-major order.
    /// </summary>
    public struct Mat22
    {
        public Vector2 Col1, Col2;

        /// <summary>
        /// Construct this matrix using columns.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        public Mat22(Vector2 c1, Vector2 c2)
        {
            Col1 = c1;
            Col2 = c2;
        }

        /// <summary>
        /// Construct this matrix using scalars.
        /// </summary>
        /// <param name="a11">The a11.</param>
        /// <param name="a12">The a12.</param>
        /// <param name="a21">The a21.</param>
        /// <param name="a22">The a22.</param>
        public Mat22(float a11, float a12, float a21, float a22)
        {
            Col1 = new Vector2(a11, a21);
            Col2 = new Vector2(a12, a22);
        }

        /// <summary>
        /// Construct this matrix using an angle. This matrix becomes
        /// an orthonormal rotation matrix.
        /// </summary>
        /// <param name="angle">The angle.</param>
        public Mat22(float angle)
        {
            // TODO_ERIN compute sin+cos together.
            float c = (float)Math.Cos(angle), s = (float)Math.Sin(angle);
            Col1 = new Vector2(c, s);
            Col2 = new Vector2(-s, c);
        }

        /// <summary>
        /// Extract the angle from this matrix (assumed to be
        /// a rotation matrix).
        /// </summary>
        /// <value></value>
        public float Angle
        {
            get { return (float)Math.Atan2(Col1.Y, Col1.X); }
        }

        public Mat22 Inverse
        {
            get
            {
                float a = Col1.X, b = Col2.X, c = Col1.Y, d = Col2.Y;
                float det = a * d - b * c;
                if (det != 0.0f)
                {
                    det = 1.0f / det;
                }

                Mat22 result = new Mat22();
                result.Col1.X = det * d;
                result.Col1.Y = -det * c;

                result.Col2.X = -det * b;
                result.Col2.Y = det * a;

                return result;
            }
        }

        /// <summary>
        /// Initialize this matrix using columns.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        public void Set(Vector2 c1, Vector2 c2)
        {
            Col1 = c1;
            Col2 = c2;
        }

        /// <summary>
        /// Initialize this matrix using an angle. This matrix becomes
        /// an orthonormal rotation matrix.
        /// </summary>
        /// <param name="angle">The angle.</param>
        public void Set(float angle)
        {
            float c = (float)Math.Cos(angle), s = (float)Math.Sin(angle);
            Col1.X = c;
            Col2.X = -s;
            Col1.Y = s;
            Col2.Y = c;
        }

        /// <summary>
        /// Set this to the identity matrix.
        /// </summary>
        public void SetIdentity()
        {
            Col1.X = 1.0f;
            Col2.X = 0.0f;
            Col1.Y = 0.0f;
            Col2.Y = 1.0f;
        }

        /// <summary>
        /// Set this matrix to all zeros.
        /// </summary>
        public void SetZero()
        {
            Col1.X = 0.0f;
            Col2.X = 0.0f;
            Col1.Y = 0.0f;
            Col2.Y = 0.0f;
        }

        /// <summary>
        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public Vector2 Solve(Vector2 b)
        {
            float a11 = Col1.X, a12 = Col2.X, a21 = Col1.Y, a22 = Col2.Y;
            float det = a11 * a22 - a12 * a21;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            return new Vector2(det * (a22 * b.X - a12 * b.Y), det * (a11 * b.Y - a21 * b.X));
        }

        public static void Add(ref Mat22 A, ref Mat22 B, out Mat22 R)
        {
            R.Col1 = A.Col1 + B.Col1;
            R.Col2 = A.Col2 + B.Col2;
        }
    }

    /// <summary>
    /// A 3-by-3 matrix. Stored in column-major order.
    /// </summary>
    public struct Mat33
    {
        public Vector3 Col1, Col2, Col3;

        /// <summary>
        /// Construct this matrix using columns.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <param name="c3">The c3.</param>
        public Mat33(Vector3 c1, Vector3 c2, Vector3 c3)
        {
            Col1 = c1;
            Col2 = c2;
            Col3 = c3;
        }

        /// <summary>
        /// Set this matrix to all zeros.
        /// </summary>
        public void SetZero()
        {
            Col1 = Vector3.Zero;
            Col2 = Vector3.Zero;
            Col3 = Vector3.Zero;
        }

        /// <summary>
        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public Vector3 Solve33(Vector3 b)
        {
            float det = Vector3.Dot(Col1, Vector3.Cross(Col2, Col3));
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            return new Vector3(det * Vector3.Dot(b, Vector3.Cross(Col2, Col3)),
                               det * Vector3.Dot(Col1, Vector3.Cross(b, Col3)),
                               det * Vector3.Dot(Col1, Vector3.Cross(Col2, b)));
        }

        /// <summary>
        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases. Solve only the upper
        /// 2-by-2 matrix equation.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public Vector2 Solve22(Vector2 b)
        {
            float a11 = Col1.X, a12 = Col2.X, a21 = Col1.Y, a22 = Col2.Y;
            float det = a11 * a22 - a12 * a21;

            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            return new Vector2(det * (a22 * b.X - a12 * b.Y), det * (a11 * b.Y - a21 * b.X));
        }
    }

    /// <summary>
    /// A transform contains translation and rotation. It is used to represent
    /// the position and orientation of rigid frames.
    /// </summary>
    public struct Transform
    {
        public Vector2 Position;
        public Mat22 R;

        /// <summary>
        /// Initialize using a position vector and a rotation matrix.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="r">The r.</param>
        public Transform(ref Vector2 position, ref Mat22 r)
        {
            Position = position;
            R = r;
        }

        /// <summary>
        /// Calculate the angle that the rotation matrix represents.
        /// </summary>
        /// <value></value>
        public float Angle
        {
            get { return (float)Math.Atan2(R.Col1.Y, R.Col1.X); }
        }

        /// <summary>
        /// Set this to the identity transform.
        /// </summary>
        public void SetIdentity()
        {
            Position = Vector2.Zero;
            R.SetIdentity();
        }

        /// <summary>
        /// Set this based on the position and angle.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="angle">The angle.</param>
        public void Set(Vector2 position, float angle)
        {
            Position = position;
            R.Set(angle);
        }
    }

    /// <summary>
    /// This describes the motion of a body/shape for TOI computation.
    /// Shapes are defined with respect to the body origin, which may
    /// no coincide with the center of mass. However, to support dynamics
    /// we must interpolate the center of mass position.
    /// </summary>
    public struct Sweep
    {
        /// <summary>
        /// World angles
        /// </summary>
        public float A;

        public float A0;

        /// <summary>
        /// Fraction of the current time step in the range [0,1]
        /// c0 and a0 are the positions at alpha0.
        /// </summary>
        public float Alpha0;

        /// <summary>
        /// Center world positions
        /// </summary>
        public Vector2 C;

        public Vector2 C0;

        /// <summary>
        /// Local center of mass position
        /// </summary>
        public Vector2 LocalCenter;

        /// <summary>
        /// Get the interpolated transform at a specific time.
        /// </summary>
        /// <param name="xf">The transform.</param>
        /// <param name="beta">beta is a factor in [0,1], where 0 indicates alpha0.</param>
        public void GetTransform(out Transform xf, float beta)
        {
            xf = new Transform();
            xf.Position.X = (1.0f - beta) * C0.X + beta * C.X;
            xf.Position.Y = (1.0f - beta) * C0.Y + beta * C.Y;
            float angle = (1.0f - beta) * A0 + beta * A;
            xf.R.Set(angle);

            // Shift to origin
            xf.Position -= MathUtils.Multiply(ref xf.R, ref LocalCenter);
        }

        /// <summary>
        /// Advance the sweep forward, yielding a new initial state.
        /// </summary>
        /// <param name="alpha">new initial time..</param>
        public void Advance(float alpha)
        {
            Debug.Assert(Alpha0 < 1.0f);
            float beta = (alpha - Alpha0) / (1.0f - Alpha0);
            C0.X = (1.0f - beta) * C0.X + beta * C.X;
            C0.Y = (1.0f - beta) * C0.Y + beta * C.Y;
            A0 = (1.0f - beta) * A0 + beta * A;
            Alpha0 = alpha;
        }

        /// <summary>
        /// Normalize the angles.
        /// </summary>
        public void Normalize()
        {
            float d = MathHelper.TwoPi * (float)Math.Floor(A0 / MathHelper.TwoPi);
            A0 -= d;
            A -= d;
        }
    }
}