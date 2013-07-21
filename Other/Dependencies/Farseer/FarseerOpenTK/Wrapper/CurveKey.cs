using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FarseerPhysics.Wrapper
{
	[Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
	public class CurveKey : IEquatable<CurveKey>, IComparable<CurveKey>
	{
		// Fields
		internal CurveContinuity continuity;
		internal float internalValue;
		internal float position;
		internal float tangentIn;
		internal float tangentOut;

		// Methods
		public CurveKey(float position, float value)
		{
			this.position = position;
			this.internalValue = value;
		}

		public CurveKey(float position, float value, float tangentIn, float tangentOut)
		{
			this.position = position;
			this.internalValue = value;
			this.tangentIn = tangentIn;
			this.tangentOut = tangentOut;
		}

		public CurveKey(float position, float value, float tangentIn, float tangentOut, CurveContinuity continuity)
		{
			this.position = position;
			this.internalValue = value;
			this.tangentIn = tangentIn;
			this.tangentOut = tangentOut;
			this.continuity = continuity;
		}

		public CurveKey Clone()
		{
			return new CurveKey(this.position, this.internalValue, this.tangentIn, this.tangentOut, this.continuity);
		}

		public int CompareTo(CurveKey other)
		{
			if (this.position == other.position)
			{
				return 0;
			}
			if (this.position >= other.position)
			{
				return 1;
			}
			return -1;
		}

		public bool Equals(CurveKey other)
		{
			return (((((other != null) && (other.position == this.position)) && ((other.internalValue == this.internalValue) && (other.tangentIn == this.tangentIn))) && (other.tangentOut == this.tangentOut)) && (other.continuity == this.continuity));
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as CurveKey);
		}

		public override int GetHashCode()
		{
			return ((((this.position.GetHashCode() + this.internalValue.GetHashCode()) + this.tangentIn.GetHashCode()) + this.tangentOut.GetHashCode()) + this.continuity.GetHashCode());
		}

		public static bool operator ==(CurveKey a, CurveKey b)
		{
			bool flag3 = null == a;
			bool flag2 = null == b;
			if (flag3 || flag2)
			{
				return (flag3 == flag2);
			}
			return a.Equals(b);
		}

		public static bool operator !=(CurveKey a, CurveKey b)
		{
			bool flag3 = a == null;
			bool flag2 = b == null;
			if (flag3 || flag2)
			{
				return (flag3 != flag2);
			}
			return ((((a.position != b.position) || (a.internalValue != b.internalValue)) || ((a.tangentIn != b.tangentIn) || (a.tangentOut != b.tangentOut))) || (a.continuity != b.continuity));
		}

		// Properties
		public CurveContinuity Continuity
		{
			get
			{
				return this.continuity;
			}
			set
			{
				this.continuity = value;
			}
		}

		public float Position
		{
			get
			{
				return this.position;
			}
		}

		public float TangentIn
		{
			get
			{
				return this.tangentIn;
			}
			set
			{
				this.tangentIn = value;
			}
		}

		public float TangentOut
		{
			get
			{
				return this.tangentOut;
			}
			set
			{
				this.tangentOut = value;
			}
		}

		public float Value
		{
			get
			{
				return this.internalValue;
			}
			set
			{
				this.internalValue = value;
			}
		}
	}
}
