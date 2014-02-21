using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Drawing;

using OpenTK;

namespace Duality
{
	public static class ExtMethodsVisualLogEntry
	{
		/// <summary>
		/// Keeps the log entry alive for a certain amount of time. It will fade out smoothly upon nearing its end.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entry"></param>
		/// <param name="lifetime">The time in milliseconds that will be added to the log entries lifetime.</param>
		/// <param name="lifetimeAsAlpha">Whether the lifetime of this entry should be used as alpha-value of the specified color. If set to true the entry will fade out over time.</param>
		/// <returns></returns>
		public static T KeepAlive<T>(this T entry, float lifetime, bool lifetimeAsAlpha = true) where T : VisualLogEntry
		{
			entry.Lifetime += lifetime;
			entry.LifetimeAsAlpha = lifetimeAsAlpha;
			return entry;
		}
		/// <summary>
		/// Anchors the log entry to the specified <see cref="GameObject"/>. All coordinates and sizes will be
		/// interpreted relative to it. Upon destruction, all anchored log entries will be removed as well.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entry"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static T AnchorAt<T>(this T entry, GameObject obj) where T : VisualLogEntry
		{
			entry.AnchorObj = obj;
			return entry;
		}
		/// <summary>
		/// Colors the log entry with the specified value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entry"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static T WithColor<T>(this T entry, ColorRgba color) where T : VisualLogEntry
		{
			entry.Color = color;
			return entry;
		}
		/// <summary>
		/// Aligns the text of this log entry as specified.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="blockAlign"></param>
		/// <returns></returns>
		public static VisualLogTextEntry Align(this VisualLogTextEntry entry, Alignment blockAlign)
		{
			entry.BlockAlignment = blockAlign;
			return entry;
		}
		/// <summary>
		/// Instead of a full circle, only a certain angular segment of the circle will be displayed.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="minAngle"></param>
		/// <param name="maxAngle"></param>
		/// <returns></returns>
		public static VisualLogCircleEntry Segment(this VisualLogCircleEntry entry, float minAngle, float maxAngle)
		{
			entry.MinAngle = minAngle;
			entry.MaxAngle = maxAngle;
			return entry;
		}
		/// <summary>
		/// Prohibits scale changes due to perspective transformation.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		public static VisualLogCircleEntry DontScale(this VisualLogCircleEntry entry)
		{
			entry.InvariantScale = true;
			return entry;
		}
		/// <summary>
		/// Prohibits scale changes due to perspective transformation.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		public static VisualLogPolygonEntry DontScale(this VisualLogPolygonEntry entry)
		{
			entry.InvariantScale = true;
			return entry;
		}
		/// <summary>
		/// Prohibits scale changes due to perspective transformation.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		public static VisualLogVectorEntry DontScale(this VisualLogVectorEntry entry)
		{
			entry.InvariantScale = true;
			return entry;
		}
	}
}
