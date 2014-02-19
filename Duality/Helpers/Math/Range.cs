using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Represents a range of values between a specific minimum and maximum value.
	/// </summary>
	public struct Range : IEquatable<Range>
	{
		/// <summary>
		/// The minimum value of this Range.
		/// </summary>
		public float MinValue;
		/// <summary>
		/// The maximum value of this Range.
		/// </summary>
		public float MaxValue;

		/// <summary>
		/// [GET] The total width of this Range. This value may be negative for irregular ranges, 
		/// i.e. ranges with a higher minimum value than their maximum value.
		/// </summary>
		public float Width
		{
			get { return this.MaxValue - this.MinValue; }
		}
		/// <summary>
		/// [GET] The center value of this Range.
		/// </summary>
		public float Center
		{
			get { return (this.MaxValue + this.MinValue) * 0.5f; }
		}
		/// <summary>
		/// [GET] Returns a normalized version of this Range where the minimum is guaranteed to be smaller than the maximum value.
		/// </summary>
		public Range Normalized
		{
			get
			{
				Range result = this;
				result.Normalize();
				return result;
			}
		}

		/// <summary>
		/// Creates a new Range from mininmum and maximum values.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public Range(float min, float max)
		{
			this.MinValue = min;
			this.MaxValue = max;
		}
		/// <summary>
		/// Creates a new Range with zero-width from a single value.
		/// </summary>
		/// <param name="value"></param>
		public Range(float value)
		{
			this.MinValue = value;
			this.MaxValue = value;
		}

		/// <summary>
		/// Performs a linear interpolation between the Ranges minimum and maximum value using the specified blend factor.
		/// </summary>
		/// <param name="ratio"></param>
		/// <returns></returns>
		public float Lerp(float ratio)
		{
			return MathF.Lerp(this.MinValue, this.MaxValue, ratio);
		}
		/// <summary>
		/// Normalizes this Range, i.e. flips minimum and maximum value, if irregular.
		/// </summary>
		public void Normalize()
		{
			if (this.MaxValue < this.MinValue)
			{
				MathF.Swap(ref this.MinValue, ref this.MaxValue);
			}
		}
		/// <summary>
		/// Returns whether this Range contains a certain value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(float value)
		{
			return this.MinValue <= value && this.MaxValue >= value;
		}
		/// <summary>
		/// Returns whether this Range contains a certain other range.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(Range value)
		{
			return this.Contains(value.MinValue) && this.Contains(value.MaxValue);
		}

		public bool Equals(Range other)
		{
			return 
				this.MinValue.Equals(other.MinValue) &&
				this.MaxValue.Equals(other.MaxValue);
		}
		public override bool Equals(object obj)
		{
			if (obj is Range)
				return this.Equals((Range)obj);
			else
				return false;
		}
		public override string ToString()
		{
			return string.Format("[{0} to {1}]", this.MinValue, this.MaxValue);
		}
		public override int GetHashCode()
		{
			return MathF.CombineHashCode(this.MinValue.GetHashCode(), this.MaxValue.GetHashCode());
		}

		/// <summary>
		/// Returns whether two Ranges are equal.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(Range left, Range right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Returns whether two Ranges are inequal.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(Range left, Range right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Adds two Ranges by adding each of their components individually.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Range operator +(Range left, Range right)
		{
			return new Range(
				left.MinValue + right.MinValue,
				left.MaxValue + right.MaxValue);
		}
		/// <summary>
		/// Subtracts two Ranges by subtracting each of their components individually.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Range operator -(Range left, Range right)
		{
			return new Range(
				left.MinValue - right.MinValue,
				left.MaxValue - right.MaxValue);
		}
		/// <summary>
		/// Multiplies two Ranges by multiplying each of their components individually.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Range operator *(Range left, Range right)
		{
			return new Range(
				left.MinValue * right.MinValue,
				left.MaxValue * right.MaxValue);
		}
		/// <summary>
		/// Divides two Ranges by dividing each of their components individually.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Range operator /(Range left, Range right)
		{
			return new Range(
				left.MinValue / right.MinValue,
				left.MaxValue / right.MaxValue);
		}

		/// <summary>
		/// Performs an implicit conversion from a single value to a ranged value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator Range(float value)
		{
			return new Range(value);
		}
	}
}
