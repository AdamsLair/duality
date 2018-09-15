using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Duality.IO;
using Duality.Resources;

namespace Duality
{
	/// <summary>
	/// Utility class for managing embedded default content in Duality.
	/// 
	/// There's usually no reason to use this class in game code, and it can in fact be somewhat dangerous
	/// when used the wrong way. If you consider invoking any of its API, be careful about it.
	/// </summary>
	public static class DefaultContent
	{
		/// <summary>
		/// Initializes all embedded default content in Duality and registers it in the <see cref="ContentProvider"/>.
		/// </summary>
		public static void Init()
		{
			Logs.Core.Write("Initializing default content...");
			Logs.Core.PushIndent();

			VertexShader.InitDefaultContent();
			FragmentShader.InitDefaultContent();
			DrawTechnique.InitDefaultContent();
			Pixmap.InitDefaultContent();
			Texture.InitDefaultContent();
			Material.InitDefaultContent();
			RenderSetup.InitDefaultContent();
			Font.InitDefaultContent();
			AudioData.InitDefaultContent();
			Sound.InitDefaultContent();

			Logs.Core.Write("...done!");
			Logs.Core.PopIndent();
		}

		/// <summary>
		/// Opens a <see cref="Stream"/> to an embedded Duality core resource with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Stream GetEmbeddedResourceStream(string name)
		{
			string embeddedNameBase = "Duality.EmbeddedResources.";
			Assembly embeddingAssembly = typeof(Resource).GetTypeInfo().Assembly;
			return embeddingAssembly.GetManifestResourceStream(embeddedNameBase + name);
		}
		/// <summary>
		/// Initializes static default content properties of type <typeparamref name="T"/> by loading
		/// embedded Duality core resources and transforming them into one <see cref="Resource"/> instance
		/// each.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="embeddedNameExt">
		/// The file extension to be added to the property name in order to retrieve a matching embedded resource stream.
		/// </param>
		/// <param name="resourceCreator">A method that can create a new <see cref="Resource"/> from a specified stream.</param>
		public static void InitType<T>(string embeddedNameExt, Func<Stream, T> resourceCreator) where T : Resource
		{
			InitType<T>(name =>
			{
				using (Stream stream = GetEmbeddedResourceStream(name + embeddedNameExt))
				{
					if (stream == null) return null;
					return resourceCreator(stream);
				}
			});
		}
		/// <summary>
		/// Initializes static default content properties of type <typeparamref name="T"/> using a predefined
		/// mapping from property names to <see cref="Resource"/> instances.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dictionary"></param>
		public static void InitType<T>(IDictionary<string, T> dictionary) where T : Resource
		{
			InitType<T>(name =>
			{
				T res;
				if (dictionary.TryGetValue(name, out res))
					return res;
				else
					return null;
			});
		}
		/// <summary>
		/// Initializes static default content properties of type <typeparamref name="T"/> using an init method
		/// that can create or retrieve a matching <see cref="Resource"/> instance for each property name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resourceCreator"></param>
		public static void InitType<T>(Func<string, T> resourceCreator) where T : Resource
		{
			string contentPathBase = Resource.DefaultContentBasePath + typeof(T).Name + ":";

			TypeInfo resourceType = typeof(T).GetTypeInfo();
			PropertyInfo[] defaultResProps = resourceType
				.DeclaredPropertiesDeep()
				.Where(p =>
					p.IsPublic() &&
					p.IsStatic() &&
					typeof(IContentRef).GetTypeInfo().IsAssignableFrom(p.PropertyType.GetTypeInfo()))
				.ToArray();

			for (int i = 0; i < defaultResProps.Length; i++)
			{
				string name = defaultResProps[i].Name;
				string contentPath = contentPathBase + name.Replace('_', ':');

				T resource = resourceCreator(name);
				if (resource != null)
				{
					// Register the newly created default content globally
					ContentProvider.AddContent(contentPath, resource);
					defaultResProps[i].SetValue(null, ContentProvider.RequestContent<T>(contentPath));
				}
			}
		}
	}
}
