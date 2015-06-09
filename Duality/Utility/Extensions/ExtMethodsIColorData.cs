using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	public static class ExtMethodsIColorData
	{
		/// <summary>
		/// Converts the color to a different color data format. If there is also a
		/// specific method doing the desired conversion, use that instead - it might be faster.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static T ConvertTo<T>(this IColorData source) where T : IColorData
		{
			T clr = default(T);
			if (clr == null)
			{
				if (typeof(T) == typeof(IColorData))
					return (T)source;
				else
					clr = (T)typeof(T).CreateInstanceOf();
			}
			clr.SetIntArgb(source.ToIntArgb());
			return clr;
		}
		/// <summary>
		/// Converts the color to a different color data format. If there is also a
		/// specific method doing the desired conversion, use that instead - it might be faster.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static IColorData ConvertTo(this IColorData source, Type type)
		{
			if (!typeof(IColorData).IsAssignableFrom(type)) throw new ArgumentException("Target type must implement IColorData.", "type");
			if (type == typeof(IColorData)) return source;

			IColorData clr = type.GetDefaultOf() as IColorData;
			if (clr == null) clr = type.CreateInstanceOf() as IColorData;
			clr.SetIntArgb(source.ToIntArgb());
			return clr;
		}
	}
}
