using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Resources
{
	/// <summary>
	/// The type of a <see cref="AbstractShader">shader</see> variable.
	/// </summary>
	public enum ShaderFieldType
	{
		/// <summary>
		/// Unknown type.
		/// </summary>
		Unknown = -1,
		
		/// <summary>
		/// A <see cref="System.Boolean"/> variable.
		/// </summary>
		Bool,
		/// <summary>
		/// A <see cref="System.Int32"/> variable.
		/// </summary>
		Int,
		/// <summary>
		/// A <see cref="System.Single"/> variable.
		/// </summary>
		Float,

		/// <summary>
		/// A two-dimensional vector with <see cref="System.Single"/> precision.
		/// </summary>
		Vec2,
		/// <summary>
		/// A three-dimensional vector with <see cref="System.Single"/> precision.
		/// </summary>
		Vec3,
		/// <summary>
		/// A four-dimensional vector with <see cref="System.Single"/> precision.
		/// </summary>
		Vec4,
		
		/// <summary>
		/// A 2x2 matrix with <see cref="System.Single"/> precision.
		/// </summary>
		Mat2,
		/// <summary>
		/// A 3x3 matrix with <see cref="System.Single"/> precision.
		/// </summary>
		Mat3,
		/// <summary>
		/// A 4x4 matrix with <see cref="System.Single"/> precision.
		/// </summary>
		Mat4,
		
		/// <summary>
		/// Represents a texture binding and provides lookups.
		/// </summary>
		Sampler2D
	}

	public static class ExtMethodsShaderVarType
	{
		public static Type GetElementPrimitive(this ShaderFieldType type)
		{
			switch (type)
			{
				default:
					return typeof(float);
				case ShaderFieldType.Int:
					return typeof(int);
				case ShaderFieldType.Bool:
					return typeof(bool);
			}
		}
		public static int GetElementCount(this ShaderFieldType type)
		{
			switch (type)
			{
				default:
					return 1;
				case ShaderFieldType.Vec2:
					return 2;
				case ShaderFieldType.Vec3:
					return 3;
				case ShaderFieldType.Vec4:
					return 4;
				case ShaderFieldType.Mat2:
					return 4;
				case ShaderFieldType.Mat3:
					return 9;
				case ShaderFieldType.Mat4:
					return 16;
			}
		}
	}
}
