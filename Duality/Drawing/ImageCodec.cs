using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.Drawing
{
	public static class ImageCodec
	{
		public const string FormatJpeg = "image/jpeg";
		public const string FormatPng  = "image/png";
		public const string FormatTiff = "image/tiff";
		public const string FormatBmp  = "image/bmp";

		private static List<IImageCodec> availableCodecs = null;
		public static IEnumerable<IImageCodec> AvailableCodecs
		{
			get
			{
				if (availableCodecs == null)
					GatherAvailable();
				return availableCodecs;
			}
		}

		private static void GatherAvailable()
		{
			availableCodecs = new List<IImageCodec>();
			foreach (TypeInfo imageCodecType in DualityApp.GetAvailDualityTypes(typeof(IImageCodec)))
			{
				TypeInfo imageCodecTypeInfo = imageCodecType.GetTypeInfo();
 				if (imageCodecTypeInfo.IsAbstract) continue;
 				if (imageCodecTypeInfo.IsInterface) continue;

				IImageCodec codec = imageCodecTypeInfo.CreateInstanceOf() as IImageCodec;
				if (codec != null)
				{
					availableCodecs.Add(codec);
				}
			}
			availableCodecs.StableSort((a, b) => b.Priority > a.Priority ? 1 : -1);
		}
		internal static void ClearTypeCache()
		{
			availableCodecs = null;
		}

		public static IImageCodec GetRead(string formatId)
		{
			if (availableCodecs == null)
				GatherAvailable();

			foreach (IImageCodec codec in availableCodecs)
			{
				if (codec.CanReadFormat(formatId))
					return codec;
			}

			return null;
		}
		public static IImageCodec GetWrite(string formatId)
		{
			if (availableCodecs == null)
				GatherAvailable();

			foreach (IImageCodec codec in availableCodecs)
			{
				if (codec.CanWriteFormat(formatId))
					return codec;
			}

			return null;
		}
		public static T Get<T>() where T : class, IImageCodec
		{
			if (availableCodecs == null)
				GatherAvailable();

			foreach (IImageCodec codec in availableCodecs)
			{
				if (codec is T)
					return codec as T;
			}

			return null;
		}
	}
}
