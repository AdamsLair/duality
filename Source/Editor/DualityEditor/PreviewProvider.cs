using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using Duality;
using Duality.Resources;

namespace Duality.Editor
{
	/// <summary>
	/// Provides preview images and sounds for a given object.
	/// See <seealso cref="PreviewGenerator{T}"/>.
	/// </summary>
	public static class PreviewProvider
	{
		private static List<IPreviewGenerator> previewGenerators = new List<IPreviewGenerator>();

		internal static void Init()
		{
			foreach (TypeInfo genType in DualityEditorApp.GetAvailDualityEditorTypes(typeof(IPreviewGenerator)))
			{
				if (genType.IsAbstract) continue;
				IPreviewGenerator gen = genType.CreateInstanceOf() as IPreviewGenerator;
				if (gen != null) previewGenerators.Add(gen);
			}
		}
		internal static void Terminate()
		{
			previewGenerators.Clear();
		}

		/// <summary>
		/// Provides a suitable preview image for the given object or null if none is available.
		/// </summary>
		/// <param name="obj">The object being previewed</param>
		/// <param name="desiredWidth">The desired width of the image</param>
		/// <param name="desiredHeight">The desired height of the image</param>
		/// <param name="mode">Determines how the image will be scaled or resized to match the given dimensions</param>
		/// <returns></returns>
		public static Bitmap GetPreviewImage(object obj, int desiredWidth, int desiredHeight, PreviewSizeMode mode = PreviewSizeMode.FixedNone)
		{
			if (desiredWidth <= 0) return null;
			if (desiredHeight <= 0) return null;

			PreviewImageQuery query = new PreviewImageQuery(obj, desiredWidth, desiredHeight, mode);
			GetPreview(query);
			return query.Result;
		}
		/// <summary>
		/// Provides a suitable preview sound for the given object or null if none is available.
		/// </summary>
		/// <param name="obj">The object being previewed</param>
		/// <returns></returns>
		public static Sound GetPreviewSound(object obj)
		{
			PreviewSoundQuery query = new PreviewSoundQuery(obj);
			GetPreview(query);
			return query.Result;
		}
		/// <summary>
		/// Performs the given <see cref="IPreviewQuery"/>
		/// </summary>
		/// <param name="query">The query to perform</param>
		public static void GetPreview(IPreviewQuery query)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;
			if (query == null) return;

			// Query all IPreviewGenerator instances that match the specified query
			var generators = (
				from g in previewGenerators
				orderby query.SourceFits(g.ObjectType) descending, g.Priority descending
				select g).ToArray();

			// Iterate over available generators until one can handle the preview query
			foreach (IPreviewGenerator gen in generators)
			{
				if (!query.TransformSource(gen.ObjectType)) continue;

				gen.Perform(query);

				if (query.Result != null) break;
			}
		}
	}
}
