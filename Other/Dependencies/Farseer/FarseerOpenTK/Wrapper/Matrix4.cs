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
	public struct Matrix4 : IEquatable<Matrix4>
	{
		public float M11;
		public float M12;
		public float M13;
		public float M14;
		public float M21;
		public float M22;
		public float M23;
		public float M24;
		public float M31;
		public float M32;
		public float M33;
		public float M34;
		public float M41;
		public float M42;
		public float M43;
		public float M44;
		private static Matrix4 _identity;
		public static Matrix4 Identity
		{
			get
			{
				return _identity;
			}
		}
		public Vector3 Up
		{
			get
			{
				Vector3 vector;
				vector.X = this.M21;
				vector.Y = this.M22;
				vector.Z = this.M23;
				return vector;
			}
			set
			{
				this.M21 = value.X;
				this.M22 = value.Y;
				this.M23 = value.Z;
			}
		}
		public Vector3 Down
		{
			get
			{
				Vector3 vector;
				vector.X = -this.M21;
				vector.Y = -this.M22;
				vector.Z = -this.M23;
				return vector;
			}
			set
			{
				this.M21 = -value.X;
				this.M22 = -value.Y;
				this.M23 = -value.Z;
			}
		}
		public Vector3 Right
		{
			get
			{
				Vector3 vector;
				vector.X = this.M11;
				vector.Y = this.M12;
				vector.Z = this.M13;
				return vector;
			}
			set
			{
				this.M11 = value.X;
				this.M12 = value.Y;
				this.M13 = value.Z;
			}
		}
		public Vector3 Left
		{
			get
			{
				Vector3 vector;
				vector.X = -this.M11;
				vector.Y = -this.M12;
				vector.Z = -this.M13;
				return vector;
			}
			set
			{
				this.M11 = -value.X;
				this.M12 = -value.Y;
				this.M13 = -value.Z;
			}
		}
		public Vector3 Forward
		{
			get
			{
				Vector3 vector;
				vector.X = -this.M31;
				vector.Y = -this.M32;
				vector.Z = -this.M33;
				return vector;
			}
			set
			{
				this.M31 = -value.X;
				this.M32 = -value.Y;
				this.M33 = -value.Z;
			}
		}
		public Vector3 Backward
		{
			get
			{
				Vector3 vector;
				vector.X = this.M31;
				vector.Y = this.M32;
				vector.Z = this.M33;
				return vector;
			}
			set
			{
				this.M31 = value.X;
				this.M32 = value.Y;
				this.M33 = value.Z;
			}
		}
		public Vector3 Translation
		{
			get
			{
				Vector3 vector;
				vector.X = this.M41;
				vector.Y = this.M42;
				vector.Z = this.M43;
				return vector;
			}
			set
			{
				this.M41 = value.X;
				this.M42 = value.Y;
				this.M43 = value.Z;
			}
		}
		public Matrix4(
			float m11, float m12, float m13, float m14, 
			float m21, float m22, float m23, float m24, 
			float m31, float m32, float m33, float m34, 
			float m41, float m42, float m43, float m44)
		{
			this.M11 = m11;
			this.M12 = m12;
			this.M13 = m13;
			this.M14 = m14;
			this.M21 = m21;
			this.M22 = m22;
			this.M23 = m23;
			this.M24 = m24;
			this.M31 = m31;
			this.M32 = m32;
			this.M33 = m33;
			this.M34 = m34;
			this.M41 = m41;
			this.M42 = m42;
			this.M43 = m43;
			this.M44 = m44;
		}


		public static void CreateRotationZ(float radians, out Matrix4 result)
		{
			float num2 = (float) Math.Cos((double) radians);
			float num = (float) Math.Sin((double) radians);
			result.M11 = num2;
			result.M12 = num;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = -num;
			result.M22 = num2;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return ("{ " + string.Format(currentCulture, "{{M11:{0} M12:{1} M13:{2} M14:{3}}} ", new object[] { this.M11.ToString(currentCulture), this.M12.ToString(currentCulture), this.M13.ToString(currentCulture), this.M14.ToString(currentCulture) }) + string.Format(currentCulture, "{{M21:{0} M22:{1} M23:{2} M24:{3}}} ", new object[] { this.M21.ToString(currentCulture), this.M22.ToString(currentCulture), this.M23.ToString(currentCulture), this.M24.ToString(currentCulture) }) + string.Format(currentCulture, "{{M31:{0} M32:{1} M33:{2} M34:{3}}} ", new object[] { this.M31.ToString(currentCulture), this.M32.ToString(currentCulture), this.M33.ToString(currentCulture), this.M34.ToString(currentCulture) }) + string.Format(currentCulture, "{{M41:{0} M42:{1} M43:{2} M44:{3}}} ", new object[] { this.M41.ToString(currentCulture), this.M42.ToString(currentCulture), this.M43.ToString(currentCulture), this.M44.ToString(currentCulture) }) + "}");
		}

		public bool Equals(Matrix4 other)
		{
			return ((((((this.M11 == other.M11) && (this.M22 == other.M22)) && ((this.M33 == other.M33) && (this.M44 == other.M44))) && (((this.M12 == other.M12) && (this.M13 == other.M13)) && ((this.M14 == other.M14) && (this.M21 == other.M21)))) && ((((this.M23 == other.M23) && (this.M24 == other.M24)) && ((this.M31 == other.M31) && (this.M32 == other.M32))) && (((this.M34 == other.M34) && (this.M41 == other.M41)) && (this.M42 == other.M42)))) && (this.M43 == other.M43));
		}

		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is Matrix4)
			{
				flag = this.Equals((Matrix4) obj);
			}
			return flag;
		}

		public override int GetHashCode()
		{
			return (((((((((((((((this.M11.GetHashCode() + this.M12.GetHashCode()) + this.M13.GetHashCode()) + this.M14.GetHashCode()) + this.M21.GetHashCode()) + this.M22.GetHashCode()) + this.M23.GetHashCode()) + this.M24.GetHashCode()) + this.M31.GetHashCode()) + this.M32.GetHashCode()) + this.M33.GetHashCode()) + this.M34.GetHashCode()) + this.M41.GetHashCode()) + this.M42.GetHashCode()) + this.M43.GetHashCode()) + this.M44.GetHashCode());
		}


		public static Matrix4 operator -(Matrix4 matrix1)
		{
			Matrix4 matrix;
			matrix.M11 = -matrix1.M11;
			matrix.M12 = -matrix1.M12;
			matrix.M13 = -matrix1.M13;
			matrix.M14 = -matrix1.M14;
			matrix.M21 = -matrix1.M21;
			matrix.M22 = -matrix1.M22;
			matrix.M23 = -matrix1.M23;
			matrix.M24 = -matrix1.M24;
			matrix.M31 = -matrix1.M31;
			matrix.M32 = -matrix1.M32;
			matrix.M33 = -matrix1.M33;
			matrix.M34 = -matrix1.M34;
			matrix.M41 = -matrix1.M41;
			matrix.M42 = -matrix1.M42;
			matrix.M43 = -matrix1.M43;
			matrix.M44 = -matrix1.M44;
			return matrix;
		}

		public static bool operator ==(Matrix4 matrix1, Matrix4 matrix2)
		{
			return ((((((matrix1.M11 == matrix2.M11) && (matrix1.M22 == matrix2.M22)) && ((matrix1.M33 == matrix2.M33) && (matrix1.M44 == matrix2.M44))) && (((matrix1.M12 == matrix2.M12) && (matrix1.M13 == matrix2.M13)) && ((matrix1.M14 == matrix2.M14) && (matrix1.M21 == matrix2.M21)))) && ((((matrix1.M23 == matrix2.M23) && (matrix1.M24 == matrix2.M24)) && ((matrix1.M31 == matrix2.M31) && (matrix1.M32 == matrix2.M32))) && (((matrix1.M34 == matrix2.M34) && (matrix1.M41 == matrix2.M41)) && (matrix1.M42 == matrix2.M42)))) && (matrix1.M43 == matrix2.M43));
		}

		public static bool operator !=(Matrix4 matrix1, Matrix4 matrix2)
		{
			if (((((matrix1.M11 == matrix2.M11) && (matrix1.M12 == matrix2.M12)) && ((matrix1.M13 == matrix2.M13) && (matrix1.M14 == matrix2.M14))) && (((matrix1.M21 == matrix2.M21) && (matrix1.M22 == matrix2.M22)) && ((matrix1.M23 == matrix2.M23) && (matrix1.M24 == matrix2.M24)))) && ((((matrix1.M31 == matrix2.M31) && (matrix1.M32 == matrix2.M32)) && ((matrix1.M33 == matrix2.M33) && (matrix1.M34 == matrix2.M34))) && (((matrix1.M41 == matrix2.M41) && (matrix1.M42 == matrix2.M42)) && (matrix1.M43 == matrix2.M43))))
			{
				return !(matrix1.M44 == matrix2.M44);
			}
			return true;
		}

		public static implicit operator OpenTK.Matrix4(Matrix4 mat)
		{
			return new OpenTK.Matrix4(mat.M11, mat.M12, mat.M13, mat.M14, mat.M21, mat.M22, mat.M23, mat.M24, mat.M31, mat.M32, mat.M33, mat.M34, mat.M41, mat.M42, mat.M43, mat.M44);
		}
		public static implicit operator Matrix4(OpenTK.Matrix4 mat)
		{
			return new Matrix4(mat.M11, mat.M12, mat.M13, mat.M14, mat.M21, mat.M22, mat.M23, mat.M24, mat.M31, mat.M32, mat.M33, mat.M34, mat.M41, mat.M42, mat.M43, mat.M44);
		}

		static Matrix4()
		{
			_identity = new Matrix4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
		}
	}
}
