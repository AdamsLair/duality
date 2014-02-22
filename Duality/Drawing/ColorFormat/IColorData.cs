using System;

namespace Duality.Drawing
{
	/// <summary>
	/// A general interface for different types of color data.
	/// </summary>
	public interface IColorData
	{
		/// <summary>
		/// Converts the color to a <see cref="System.UInt32"/>-Rgba value.
		/// </summary>
		/// <returns></returns>
		int ToIntRgba();
		/// <summary>
		/// Sets the color base ond a <see cref="System.UInt32"/>-Rgba value.
		/// </summary>
		/// <param name="rgba"></param>
		void SetIntRgba(int rgba);
		
		/// <summary>
		/// Converts the color to a <see cref="System.UInt32"/>-Argb value.
		/// </summary>
		/// <returns></returns>
		int ToIntArgb();
		/// <summary>
		/// Sets the color base ond a <see cref="System.UInt32"/>-Argb value.
		/// </summary>
		/// <param name="rgba"></param>
		void SetIntArgb(int argb);
	}

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
					clr = (T)typeof(T).CreateInstanceOf(true);
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

			IColorData clr = type.GetDefaultInstanceOf() as IColorData;
			if (clr == null) clr = type.CreateInstanceOf(true) as IColorData;
			clr.SetIntArgb(source.ToIntArgb());
			return clr;
		}
	}
}
