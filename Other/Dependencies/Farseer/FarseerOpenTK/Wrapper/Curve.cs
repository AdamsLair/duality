using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FarseerPhysics.Wrapper
{
	[Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
	public class Curve
	{
		// Fields
		private CurveKeyCollection keys = new CurveKeyCollection();
		private CurveLoopType postLoop;
		private CurveLoopType preLoop;

		// Methods
		private float CalcCycle(float t)
		{
			float num = (t - this.keys[0].position) * this.keys.InvTimeRange;
			if (num < 0f)
			{
				num--;
			}
			int num2 = (int) num;
			return (float) num2;
		}

		public Curve Clone()
		{
			Curve curve = new Curve();
			curve.preLoop = this.preLoop;
			curve.postLoop = this.postLoop;
			curve.keys = this.keys.Clone();
			return curve;
		}

		public void ComputeTangent(int keyIndex, CurveTangent tangentType)
		{
			this.ComputeTangent(keyIndex, tangentType, tangentType);
		}

		public void ComputeTangent(int keyIndex, CurveTangent tangentInType, CurveTangent tangentOutType)
		{
			float num2;
			float num4;
			float num7;
			float num8;
			if ((this.keys.Count <= keyIndex) || (keyIndex < 0))
			{
				throw new ArgumentOutOfRangeException("keyIndex");
			}
			CurveKey key = this.Keys[keyIndex];
			float position = num8 = num4 = key.Position;
			float num = num7 = num2 = key.Value;
			if (keyIndex > 0)
			{
				position = this.Keys[keyIndex - 1].Position;
				num = this.Keys[keyIndex - 1].Value;
			}
			if ((keyIndex + 1) < this.keys.Count)
			{
				num4 = this.Keys[keyIndex + 1].Position;
				num2 = this.Keys[keyIndex + 1].Value;
			}
			if (tangentInType == CurveTangent.Smooth)
			{
				float num10 = num4 - position;
				float num6 = num2 - num;
				if (Math.Abs(num6) < 1.192093E-07f)
				{
					key.TangentIn = 0f;
				}
				else
				{
					key.TangentIn = (num6 * Math.Abs((float) (position - num8))) / num10;
				}
			}
			else if (tangentInType == CurveTangent.Linear)
			{
				key.TangentIn = num7 - num;
			}
			else
			{
				key.TangentIn = 0f;
			}
			if (tangentOutType == CurveTangent.Smooth)
			{
				float num9 = num4 - position;
				float num5 = num2 - num;
				if (Math.Abs(num5) < 1.192093E-07f)
				{
					key.TangentOut = 0f;
				}
				else
				{
					key.TangentOut = (num5 * Math.Abs((float) (num4 - num8))) / num9;
				}
			}
			else if (tangentOutType == CurveTangent.Linear)
			{
				key.TangentOut = num2 - num7;
			}
			else
			{
				key.TangentOut = 0f;
			}
		}

		public void ComputeTangents(CurveTangent tangentType)
		{
			this.ComputeTangents(tangentType, tangentType);
		}

		public void ComputeTangents(CurveTangent tangentInType, CurveTangent tangentOutType)
		{
			for (int i = 0; i < this.Keys.Count; i++)
			{
				this.ComputeTangent(i, tangentInType, tangentOutType);
			}
		}

		public float Evaluate(float position)
		{
			if (this.keys.Count == 0)
			{
				return 0f;
			}
			if (this.keys.Count == 1)
			{
				return this.keys[0].internalValue;
			}
			CurveKey key = this.keys[0];
			CurveKey key2 = this.keys[this.keys.Count - 1];
			float t = position;
			float num6 = 0f;
			if (t < key.position)
			{
				if (this.preLoop == CurveLoopType.Constant)
				{
					return key.internalValue;
				}
				if (this.preLoop == CurveLoopType.Linear)
				{
					return (key.internalValue - (key.tangentIn * (key.position - t)));
				}
				if (!this.keys.IsCacheAvailable)
				{
					this.keys.ComputeCacheValues();
				}
				float num5 = this.CalcCycle(t);
				float num3 = t - (key.position + (num5 * this.keys.TimeRange));
				if (this.preLoop == CurveLoopType.Cycle)
				{
					t = key.position + num3;
				}
				else if (this.preLoop == CurveLoopType.CycleOffset)
				{
					t = key.position + num3;
					num6 = (key2.internalValue - key.internalValue) * num5;
				}
				else
				{
					t = ((((int) num5) & 1) != 0) ? (key2.position - num3) : (key.position + num3);
				}
			}
			else if (key2.position < t)
			{
				if (this.postLoop == CurveLoopType.Constant)
				{
					return key2.internalValue;
				}
				if (this.postLoop == CurveLoopType.Linear)
				{
					return (key2.internalValue - (key2.tangentOut * (key2.position - t)));
				}
				if (!this.keys.IsCacheAvailable)
				{
					this.keys.ComputeCacheValues();
				}
				float num4 = this.CalcCycle(t);
				float num2 = t - (key.position + (num4 * this.keys.TimeRange));
				if (this.postLoop == CurveLoopType.Cycle)
				{
					t = key.position + num2;
				}
				else if (this.postLoop == CurveLoopType.CycleOffset)
				{
					t = key.position + num2;
					num6 = (key2.internalValue - key.internalValue) * num4;
				}
				else
				{
					t = ((((int) num4) & 1) != 0) ? (key2.position - num2) : (key.position + num2);
				}
			}
			CurveKey key4 = null;
			CurveKey key3 = null;
			t = this.FindSegment(t, ref key4, ref key3);
			return (num6 + Hermite(key4, key3, t));
		}

		private float FindSegment(float t, ref CurveKey k0, ref CurveKey k1)
		{
			float num2 = t;
			k0 = this.keys[0];
			for (int i = 1; i < this.keys.Count; i++)
			{
				k1 = this.keys[i];
				if (k1.position >= t)
				{
					double position = k0.position;
					double num6 = k1.position;
					double num5 = t;
					double num3 = num6 - position;
					num2 = 0f;
					if (num3 > 1E-10)
					{
						num2 = (float) ((num5 - position) / num3);
					}
					return num2;
				}
				k0 = k1;
			}
			return num2;
		}

		private static float Hermite(CurveKey k0, CurveKey k1, float t)
		{
			if (k0.Continuity == CurveContinuity.Step)
			{
				if (t >= 1f)
				{
					return k1.internalValue;
				}
				return k0.internalValue;
			}
			float num = t * t;
			float num2 = num * t;
			float internalValue = k0.internalValue;
			float num5 = k1.internalValue;
			float tangentOut = k0.tangentOut;
			float tangentIn = k1.tangentIn;
			return ((((internalValue * (((2f * num2) - (3f * num)) + 1f)) + (num5 * ((-2f * num2) + (3f * num)))) + (tangentOut * ((num2 - (2f * num)) + t))) + (tangentIn * (num2 - num)));
		}

		// Properties
		public bool IsConstant
		{
			get
			{
				return (this.keys.Count <= 1);
			}
		}

		public CurveKeyCollection Keys
		{
			get
			{
				return this.keys;
			}
		}

		public CurveLoopType PostLoop
		{
			get
			{
				return this.postLoop;
			}
			set
			{
				this.postLoop = value;
			}
		}

		public CurveLoopType PreLoop
		{
			get
			{
				return this.preLoop;
			}
			set
			{
				this.preLoop = value;
			}
		}
	}

	public enum CurveContinuity
	{
		Smooth,
		Step
	}
	public enum CurveLoopType
	{
		Constant,
		Cycle,
		CycleOffset,
		Oscillate,
		Linear
	}
	public enum CurveTangent
	{
		Flat,
		Linear,
		Smooth
	}
}
