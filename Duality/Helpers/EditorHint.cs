using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.EditorHints
{
	/// <summary>
	/// Some general flags for Type members that indicate preferred editor behaviour.
	/// </summary>
	[Flags]
	public enum MemberFlags
	{
		/// <summary>
		/// No flags set.
		/// </summary>
		None			= 0x0,

		/// <summary>
		/// When editing the Properties or Fields value, a final set operation is requested to finish editing.
		/// </summary>
		ForceWriteback	= 0x1,
		/// <summary>
		/// The member is considered invisible.
		/// </summary>
		Invisible		= 0x2,
		/// <summary>
		/// The member is considered read-only, even if writing is possible via reflection.
		/// </summary>
		ReadOnly		= 0x4,
		/// <summary>
		/// Indicates that editing the member may have an effect on any other member of the current object.
		/// </summary>
		AffectsOthers	= 0x8
	}

	/// <summary>
	/// An attribute that provides information about a Types or Members preferred editor behaviour.
	/// </summary>
	public abstract class EditorHintAttribute : Attribute {}

	/// <summary>
	/// Provides general information about a members preferred editor behaviour.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class EditorHintFlagsAttribute : EditorHintAttribute
	{
		private	MemberFlags	flags;
		/// <summary>
		/// [GET] Flags that indicate the members general behaviour
		/// </summary>
		public MemberFlags Flags
		{
			get { return this.flags; }
		}
		public EditorHintFlagsAttribute(MemberFlags flags)
		{
			this.flags = flags;
		}
	}

	/// <summary>
	/// Provides information about a numerical members allowed value range.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class EditorHintRangeAttribute : EditorHintAttribute
	{
		private	decimal	min;
		private	decimal	max;
		/// <summary>
		/// [GET] The members minimum value
		/// </summary>
		public decimal Min
		{
			get { return this.min; }
		}
		/// <summary>
		/// [GET] The members maximum value
		/// </summary>
		public decimal Max
		{
			get { return this.max; }
		}
		public EditorHintRangeAttribute(int min, int max)
		{
			this.min = min;
			this.max = max;
		}
		public EditorHintRangeAttribute(float min, float max)
		{
			this.min = MathF.SafeToDecimal(min);
			this.max = MathF.SafeToDecimal(max);
		}
	}

	/// <summary>
	/// Provides information about a numerical members value increment.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class EditorHintIncrementAttribute : EditorHintAttribute
	{
		private	decimal	inc;
		/// <summary>
		/// [GET] The members value increment.
		/// </summary>
		public decimal Increment
		{
			get { return this.inc; }
		}
		public EditorHintIncrementAttribute(int inc)
		{
			this.inc = inc;
		}
		public EditorHintIncrementAttribute(float inc)
		{
			this.inc = (decimal)inc;
		}
	}

	/// <summary>
	/// Provides information about a numerical members decimal accuracy
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class EditorHintDecimalPlacesAttribute : EditorHintAttribute
	{
		private	int places;
		/// <summary>
		/// [GET] The preferred number of displayed decimal places
		/// </summary>
		public int Places
		{
			get { return this.places; }
		}
		public EditorHintDecimalPlacesAttribute(int places)
		{
			this.places = places;
		}
	}

	/// <summary>
	/// Provides information about a Types editor category.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class EditorHintCategoryAttribute : EditorHintAttribute
	{
		private	string[] category	= null;
		private	string context	= null;
		/// <summary>
		/// [GET] The preferred category tree to fit this Type in.
		/// </summary>
		public string[] Category
		{
			get { return this.category; }
		}
		/// <summary>
		/// [GET] The context this category applies to.
		/// </summary>
		public string Context
		{
			get { return this.context; }
		}
		public EditorHintCategoryAttribute(string category) : this(category, null) {}
		public EditorHintCategoryAttribute(string category, string context)
		{
			if (category != null)
				this.category = category.Split(new[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
			else
				this.category = null;
			this.context = context;
		}
	}

	public static class ExtMethodsMemberInfoEditorHint
	{
		public static T GetEditorHint<T>(this MemberInfo info) where T : EditorHintAttribute
		{
			return Attribute.GetCustomAttributes(info, typeof(T), true).FirstOrDefault() as T;
		}
		public static T GetEditorHint<T>(this MemberInfo info, IEnumerable<EditorHintAttribute> hintOverride) where T : EditorHintAttribute
		{
			T attrib = null;
			if (hintOverride != null) attrib = hintOverride.OfType<T>().FirstOrDefault();
			if (attrib == null && info != null) attrib = info.GetEditorHint<T>();
			return attrib;
		}
		public static IEnumerable<T> GetEditorHints<T>(this MemberInfo info) where T : EditorHintAttribute
		{
			return Attribute.GetCustomAttributes(info, typeof(T), true).OfType<T>();
		}
		public static IEnumerable<T> GetEditorHints<T>(this MemberInfo info, IEnumerable<EditorHintAttribute> hintOverride) where T : EditorHintAttribute
		{
			if (info != null) return info.GetEditorHints<T>().OverrideEditorHintsBy(hintOverride).OfType<T>();
			return null;
		}
		public static IEnumerable<EditorHintAttribute> OverrideEditorHintsBy(this IEnumerable<EditorHintAttribute> hints, IEnumerable<EditorHintAttribute> overrideHints)
		{
			if (overrideHints == null) return hints;
			if (hints == null) return overrideHints;
			return hints.Where(h => !overrideHints.Any(o => o.GetType().IsInstanceOfType(h)));
		}
	}
}
