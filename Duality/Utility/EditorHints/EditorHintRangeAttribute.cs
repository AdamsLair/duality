using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace Duality.Editor
{
	/// <summary>
	/// Provides information about a numerical members allowed value range.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class EditorHintRangeAttribute : EditorHintAttribute
	{
		private	decimal	limitMin;
		private	decimal	limitMax;
		private	decimal	reasonableMin;
		private	decimal	reasonableMax;

		/// <summary>
		/// [GET] The members limiting minimum value.
		/// </summary>
		public decimal LimitMinimum
		{
			get { return this.limitMin; }
		}
		/// <summary>
		/// [GET] The members limiting maximum value.
		/// </summary>
		public decimal LimitMaximum
		{
			get { return this.limitMax; }
		}
		/// <summary>
		/// [GET] The members reasonable (non-limiting) minimum value.
		/// </summary>
		public decimal ReasonableMinimum
		{
			get { return this.reasonableMin; }
		}
		/// <summary>
		/// [GET] The members reasonable (non-limiting) maximum value.
		/// </summary>
		public decimal ReasonableMaximum
		{
			get { return this.reasonableMax; }
		}

		public EditorHintRangeAttribute(int min, int max) : this(min, max, min, max) {}
		public EditorHintRangeAttribute(int limitMin, int limitMax, int reasonableMin, int reasonableMax)
		{
			this.limitMin = limitMin;
			this.limitMax = limitMax;
			this.reasonableMin = Math.Max(reasonableMin, limitMin);
			this.reasonableMax = Math.Min(reasonableMax, limitMax);
		}
		public EditorHintRangeAttribute(float min, float max) : this(min, max, min, max) {}
		public EditorHintRangeAttribute(float limitMin, float limitMax, float reasonableMin, float reasonableMax)
		{
			this.limitMin = MathF.SafeToDecimal(limitMin);
			this.limitMax = MathF.SafeToDecimal(limitMax);
			this.reasonableMin = MathF.SafeToDecimal(Math.Max(reasonableMin, limitMin));
			this.reasonableMax = MathF.SafeToDecimal(Math.Min(reasonableMax, limitMax));
		}
	}
}
