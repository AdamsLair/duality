using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace OpenTK
{
	// ### MATCH ### OpenTK ### MATCH ###

	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Vector2 : IEquatable<Vector2>
	{
		public float X;
		public float Y;
		private static Vector2 _zero;
		private static Vector2 _one;
		private static Vector2 _unitX;
		private static Vector2 _unitY;
		public static Vector2 Zero
		{
			get
			{
				return _zero;
			}
		}
		public static Vector2 One
		{
			get
			{
				return _one;
			}
		}
		public static Vector2 UnitX
		{
			get
			{
				return _unitX;
			}
		}
		public static Vector2 UnitY
		{
			get
			{
				return _unitY;
			}
		}
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public Vector2(float value)
		{
			this.X = this.Y = value;
		}

		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture) });
		}

		public bool Equals(Vector2 other)
		{
			return ((this.X == other.X) && (this.Y == other.Y));
		}

		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is Vector2)
			{
				flag = this.Equals((Vector2) obj);
			}
			return flag;
		}

		public override int GetHashCode()
		{
			return (this.X.GetHashCode() + this.Y.GetHashCode());
		}

		public float Length
		{
			get
			{
				float num = (this.X * this.X) + (this.Y * this.Y);
				return (float) Math.Sqrt((double) num);
			}
		}

		public float LengthSquared
		{
			get
			{
				return ((this.X * this.X) + (this.Y * this.Y));
			}
		}

		public static float Dot(Vector2 value1, Vector2 value2)
		{
			return ((value1.X * value2.X) + (value1.Y * value2.Y));
		}

		public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
		{
			result = (value1.X * value2.X) + (value1.Y * value2.Y);
		}

		public void Normalize()
		{
			float num2 = (this.X * this.X) + (this.Y * this.Y);
			float num = 1f / ((float) Math.Sqrt((double) num2));
			this.X *= num;
			this.Y *= num;
		}
		public Vector2 Normalized
		{
			get
			{
				Vector2 n = this;
				n.Normalize();
				return n;
			}
		}

		public static Vector2 ComponentMin(Vector2 value1, Vector2 value2)
		{
			Vector2 vector;
			vector.X = (value1.X < value2.X) ? value1.X : value2.X;
			vector.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
			return vector;
		}
		public static void ComponentMin(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = (value1.X < value2.X) ? value1.X : value2.X;
			result.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
		}
		public static Vector2 ComponentMax(Vector2 value1, Vector2 value2)
		{
			Vector2 vector;
			vector.X = (value1.X > value2.X) ? value1.X : value2.X;
			vector.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			return vector;
		}
		public static void ComponentMax(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = (value1.X > value2.X) ? value1.X : value2.X;
			result.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
		}

		public static Vector2 Transform(Vector2 position, Matrix4 matrix)
		{
			Vector2 vector;
			float num2 = ((position.X * matrix.M11) + (position.Y * matrix.M21)) + matrix.M41;
			float num = ((position.X * matrix.M12) + (position.Y * matrix.M22)) + matrix.M42;
			vector.X = num2;
			vector.Y = num;
			return vector;
		}

		public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
		}
		public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
		}
		public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
		}
		public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
		{
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
		}
		public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
		}
		public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
		{
			float num = 1f / divider;
			result.X = value1.X * num;
			result.Y = value1.Y * num;
		}

		public static Vector2 operator -(Vector2 value)
		{
			Vector2 vector;
			vector.X = -value.X;
			vector.Y = -value.Y;
			return vector;
		}

		public static bool operator ==(Vector2 value1, Vector2 value2)
		{
			return ((value1.X == value2.X) && (value1.Y == value2.Y));
		}

		public static bool operator !=(Vector2 value1, Vector2 value2)
		{
			if (value1.X == value2.X)
			{
				return !(value1.Y == value2.Y);
			}
			return true;
		}

		public static Vector2 operator +(Vector2 value1, Vector2 value2)
		{
			Vector2 vector;
			vector.X = value1.X + value2.X;
			vector.Y = value1.Y + value2.Y;
			return vector;
		}

		public static Vector2 operator -(Vector2 value1, Vector2 value2)
		{
			Vector2 vector;
			vector.X = value1.X - value2.X;
			vector.Y = value1.Y - value2.Y;
			return vector;
		}

		public static Vector2 operator *(Vector2 value1, Vector2 value2)
		{
			Vector2 vector;
			vector.X = value1.X * value2.X;
			vector.Y = value1.Y * value2.Y;
			return vector;
		}

		public static Vector2 operator *(Vector2 value, float scaleFactor)
		{
			Vector2 vector;
			vector.X = value.X * scaleFactor;
			vector.Y = value.Y * scaleFactor;
			return vector;
		}

		public static Vector2 operator *(float scaleFactor, Vector2 value)
		{
			Vector2 vector;
			vector.X = value.X * scaleFactor;
			vector.Y = value.Y * scaleFactor;
			return vector;
		}

		public static Vector2 operator /(Vector2 value1, Vector2 value2)
		{
			Vector2 vector;
			vector.X = value1.X / value2.X;
			vector.Y = value1.Y / value2.Y;
			return vector;
		}

		public static Vector2 operator /(Vector2 value1, float divider)
		{
			Vector2 vector;
			float num = 1f / divider;
			vector.X = value1.X * num;
			vector.Y = value1.Y * num;
			return vector;
		}

		public static implicit operator OpenTK.Vector2(Vector2 vec)
		{
			return new OpenTK.Vector2(vec.X, vec.Y);
		}
		public static implicit operator Vector2(OpenTK.Vector2 vec)
		{
			return new Vector2(vec.X, vec.Y);
		}

		static Vector2()
		{
			_zero = new Vector2();
			_one = new Vector2(1f, 1f);
			_unitX = new Vector2(1f, 0f);
			_unitY = new Vector2(0f, 1f);
		}
	}
}
