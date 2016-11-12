using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	internal static class GeneralResNames
	{
		private const string ManifestBaseName	= "Duality.Editor.EmbeddedResources.";

		public const string ImageAssetImporter	= ManifestBaseName + "AssetImporter.png";
	}
}
