using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Plugins.SceneView.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class SceneViewResCache
	{
		public static readonly Icon		IconSceneView				= SceneViewRes.IconSceneView;
		public static readonly Bitmap	OverlayLink					= SceneViewRes.OverlayLink;
		public static readonly Bitmap	OverlayLinkBroken			= SceneViewRes.OverlayLinkBroken;
		public static readonly Bitmap	IconLock					= SceneViewRes.IconLock;
		public static readonly Bitmap	IconEyeCross				= SceneViewRes.IconEyeCross;
	}
}
