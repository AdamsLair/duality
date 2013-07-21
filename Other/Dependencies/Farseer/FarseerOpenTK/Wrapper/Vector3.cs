using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;

namespace OpenTK
{
	// ### MATCH ### OpenTK ### MATCH ###

	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Vector3 : IEquatable<Vector3>
	{
		public float X;
		public float Y;
		public float Z;
		private static Vector3 _zero;
		private static Vector3 _one;
		private static Vector3 _unitX;
		private static Vector3 _unitY;
		private static Vector3 _unitZ;
		private static Vector3 _up;
		private static Vector3 _down;
		private static Vector3 _right;
		private static Vector3 _left;
		private static Vector3 _forward;
		private static Vector3 _backward;
		public static Vector3 Zero
		{
			get
			{
				return _zero;
			}
		}
		public static Vector3 One
		{
			get
			{
				return _one;
			}
		}
		public static Vector3 UnitX
		{
			get
			{
				return _unitX;
			}
		}
		public static Vector3 UnitY
		{
			get
			{
				return _unitY;
			}
		}
		public static Vector3 UnitZ
		{
			get
			{
				return _unitZ;
			}
		}
		public static Vector3 Up
		{
			get
			{
				return _up;
			}
		}
		public static Vector3 Down
		{
			get
			{
				return _down;
			}
		}
		public static Vector3 Right
		{
			get
			{
				return _right;
			}
		}
		public static Vector3 Left
		{
			get
			{
				return _left;
			}
		}
		public static Vector3 Forward
		{
			get
			{
				return _forward;
			}
		}
		public static Vector3 Backward
		{
			get
			{
				return _backward;
			}
		}
		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public Vector3(float value)
		{
			this.X = this.Y = this.Z = value;
		}

		public Vector3(Vector2 value, float z)
		{
			this.X = value.X;
			this.Y = value.Y;
			this.Z = z;
		}

		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture), this.Z.ToString(currentCulture) });
		}

		public bool Equals(Vector3 other)
		{
			return (((this.X == other.X) && (this.Y == other.Y)) && (this.Z == other.Z));
		}

		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is Vector3)
			{
				flag = this.Equals((Vector3) obj);
			}
			return flag;
		}

		public override int GetHashCode()
		{
			return ((this.X.GetHashCode() + this.Y.GetHashCode()) + this.Z.GetHashCode());
		}

		public float Length
		{
			get
			{
				float num = ((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z);
				return (float) Math.Sqrt((double) num);
			}
		}

		public float LengthSquared
		{
			get
			{
				return (((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z));
			}
		}

		public static float Dot(Vector3 vector1, Vector3 vector2)
		{
			return (((vector1.X * vector2.X) + (vector1.Y * vector2.Y)) + (vector1.Z * vector2.Z));
		}

		public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
		{
			result = ((vector1.X * vector2.X) + (vector1.Y * vector2.Y)) + (vector1.Z * vector2.Z);
		}

		public void Normalize()
		{
			float num2 = ((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z);
			float num = 1f / ((float) Math.Sqrt((double) num2));
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
		}
		public Vector3 Normalized
		{
			get
			{
				Vector3 n = this;
				n.Normalize();
				return n;
			}
		}

		public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
		{
			Vector3 vector;
			vector.X = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
			vector.Y = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
			vector.Z = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
			return vector;
		}
		public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
		{
			float num3 = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
			float num2 = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
			float num = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
			result.X = num3;
			result.Y = num2;
			result.Z = num;
		}

		public static Vector3 ComponentMin(Vector3 value1, Vector3 value2)
		{
			Vector3 vector;
			vector.X = (value1.X < value2.X) ? value1.X : value2.X;
			vector.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
			vector.Z = (value1.Z < value2.Z) ? value1.Z : value2.Z;
			return vector;
		}
		public static void ComponentMin(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = (value1.X < value2.X) ? value1.X : value2.X;
			result.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
			result.Z = (value1.Z < value2.Z) ? value1.Z : value2.Z;
		}
		public static Vector3 ComponentMax(Vector3 value1, Vector3 value2)
		{
			Vector3 vector;
			vector.X = (value1.X > value2.X) ? value1.X : value2.X;
			vector.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			vector.Z = (value1.Z > value2.Z) ? value1.Z : value2.Z;
			return vector;
		}
		public static void ComponentMax(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = (value1.X > value2.X) ? value1.X : value2.X;
			result.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
			result.Z = (value1.Z > value2.Z) ? value1.Z : value2.Z;
		}

		public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
		{
			Vector3 vector;
			vector.X = value1.X + ((value2.X - value1.X) * amount);
			vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
			vector.Z = value1.Z + ((value2.Z - value1.Z) * amount);
			return vector;
		}
		public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
		{
			result.X = value1.X + ((value2.X - value1.X) * amount);
			result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
			result.Z = value1.Z + ((value2.Z - value1.Z) * amount);
		}

		public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
		{
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
		}


		public static Vector3 operator -(Vector3 value)
		{
			Vector3 vector;
			vector.X = -value.X;
			vector.Y = -value.Y;
			vector.Z = -value.Z;
			return vector;
		}

		public static bool operator ==(Vector3 value1, Vector3 value2)
		{
			return (((value1.X == value2.X) && (value1.Y == value2.Y)) && (value1.Z == value2.Z));
		}

		public static bool operator !=(Vector3 value1, Vector3 value2)
		{
			if ((value1.X == value2.X) && (value1.Y == value2.Y))
			{
				return !(value1.Z == value2.Z);
			}
			return true;
		}

		public static Vector3 operator +(Vector3 value1, Vector3 value2)
		{
			Vector3 vector;
			vector.X = value1.X + value2.X;
			vector.Y = value1.Y + value2.Y;
			vector.Z = value1.Z + value2.Z;
			return vector;
		}

		public static Vector3 operator -(Vector3 value1, Vector3 value2)
		{
			Vector3 vector;
			vector.X = value1.X - value2.X;
			vector.Y = value1.Y - value2.Y;
			vector.Z = value1.Z - value2.Z;
			return vector;
		}

		public static Vector3 operator *(Vector3 value1, Vector3 value2)
		{
			Vector3 vector;
			vector.X = value1.X * value2.X;
			vector.Y = value1.Y * value2.Y;
			vector.Z = value1.Z * value2.Z;
			return vector;
		}

		public static Vector3 operator *(Vector3 value, float scaleFactor)
		{
			Vector3 vector;
			vector.X = value.X * scaleFactor;
			vector.Y = value.Y * scaleFactor;
			vector.Z = value.Z * scaleFactor;
			return vector;
		}

		public static Vector3 operator *(float scaleFactor, Vector3 value)
		{
			Vector3 vector;
			vector.X = value.X * scaleFactor;
			vector.Y = value.Y * scaleFactor;
			vector.Z = value.Z * scaleFactor;
			return vector;
		}

		public static Vector3 operator /(Vector3 value1, Vector3 value2)
		{
			Vector3 vector;
			vector.X = value1.X / value2.X;
			vector.Y = value1.Y / value2.Y;
			vector.Z = value1.Z / value2.Z;
			return vector;
		}

		public static Vector3 operator /(Vector3 value, float divider)
		{
			Vector3 vector;
			float num = 1f / divider;
			vector.X = value.X * num;
			vector.Y = value.Y * num;
			vector.Z = value.Z * num;
			return vector;
		}
		
		public static implicit operator OpenTK.Vector3(Vector3 vec)
		{
			return new OpenTK.Vector3(vec.X, vec.Y, vec.Z);
		}
		public static implicit operator Vector3(OpenTK.Vector3 vec)
		{
			return new Vector3(vec.X, vec.Y, vec.Z);
		}

		static Vector3()
		{
			_zero = new Vector3();
			_one = new Vector3(1f, 1f, 1f);
			_unitX = new Vector3(1f, 0f, 0f);
			_unitY = new Vector3(0f, 1f, 0f);
			_unitZ = new Vector3(0f, 0f, 1f);
			_up = new Vector3(0f, 1f, 0f);
			_down = new Vector3(0f, -1f, 0f);
			_right = new Vector3(1f, 0f, 0f);
			_left = new Vector3(-1f, 0f, 0f);
			_forward = new Vector3(0f, 0f, -1f);
			_backward = new Vector3(0f, 0f, 1f);
		}
	}
}
