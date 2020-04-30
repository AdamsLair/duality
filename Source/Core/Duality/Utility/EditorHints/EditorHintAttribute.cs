using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Duality.Editor
{
	/// <summary>
	/// An attribute that provides information about a Types or Members preferred editor behaviour.
	/// </summary>
	public abstract class EditorHintAttribute : Attribute
	{
		/// <summary>
		/// Retrieves the specified editor hint attribute from a member, if existing.
		/// </summary>
		/// <typeparam name="T">The Type of editor hint to retrieve.</typeparam>
		/// <param name="member">The member to extract the hint from. May be null.</param>
		/// <param name="overrideHints">An optional override hint, that will be preferred, if applicable.</param>
		public static T Get<T>(MemberInfo member, IEnumerable<EditorHintAttribute> overrideHints = null) where T : EditorHintAttribute
		{
			T hint = member != null ? member.GetAttributesCached<T>().FirstOrDefault() : null;
			if (overrideHints == null)
				return hint;
			else if (hint == null)
				return overrideHints.OfType<T>().FirstOrDefault();
			else
				return overrideHints.OfType<T>().Where(o => hint.GetType().GetTypeInfo().IsInstanceOfType(o)).FirstOrDefault() ?? hint;
		}
		/// <summary>
		/// Retrieves the specified editor hint attributes from a member, if existing.
		/// </summary>
		/// <typeparam name="T">The Type of editor hints to retrieve.</typeparam>
		/// <param name="member">The member to extract the hints from. May be null.</param>
		/// <param name="overrideHints">An optional collection of override hints, that will be preferred, if applicable.</param>
		public static IEnumerable<T> GetAll<T>(MemberInfo member, IEnumerable<EditorHintAttribute> overrideHints = null) where T : EditorHintAttribute
		{
			IEnumerable<T> hints = member != null ? member.GetAttributesCached<T>() : null;
			if (overrideHints == null)
				return hints;
			else if (hints == null)
				return overrideHints.OfType<T>();
			else
				return hints.Select(original => overrideHints.OfType<T>().Where(o => original.GetType().GetTypeInfo().IsInstanceOfType(o)).FirstOrDefault() ?? original);
		}
	}
}
