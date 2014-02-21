using System;

using Duality.Properties;
using Duality.Editor;

using OpenTK.Graphics.OpenGL;


namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL FragmentShader.
	/// </summary>
	[Serializable]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageFragmentShader)]
	public class FragmentShader : AbstractShader
	{
		/// <summary>
		/// A FragmentShader resources file extension.
		/// </summary>
		public new const string FileExt = ".FragmentShader" + Resource.FileExt;
		
		/// <summary>
		/// [GET] A minimal FragmentShader. It performs a texture lookup
		/// and applies vertex-coloring.
		/// </summary>
		public static ContentRef<FragmentShader> Minimal	{ get; private set; }
		/// <summary>
		/// [GET] A FragmentShader designed for picking operations. It uses
		/// the provided texture for alpha output and forwards the incoming RGB color value.
		/// </summary>
		public static ContentRef<FragmentShader> Picking	{ get; private set; }
		/// <summary>
		/// [GET] The SmoothAnim FragmentShader. It performs two lookups
		/// on the same texture and blends the results using an incoming float value.
		/// </summary>
		public static ContentRef<FragmentShader> SmoothAnim	{ get; private set; }
		/// <summary>
		/// [GET] The SharpMask FragmentShader. It enforces an antialiazed sharp mask when upscaling linearly blended textures.
		/// </summary>
		public static ContentRef<FragmentShader> SharpAlpha	{ get; private set; }

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath		= ContentProvider.VirtualContentPath + "FragmentShader:";
			const string ContentPath_Minimal	= VirtualContentPath + "Minimal";
			const string ContentPath_Picking	= VirtualContentPath + "Picking";
			const string ContentPath_SmoothAnim	= VirtualContentPath + "SmoothAnim";
			const string ContentPath_SharpMask	= VirtualContentPath + "SharpAlpha";

			ContentProvider.AddContent(ContentPath_Minimal,	new FragmentShader(DefaultContent.MinimalFrag));
			ContentProvider.AddContent(ContentPath_Picking,	new FragmentShader(DefaultContent.PickingFrag));
			ContentProvider.AddContent(ContentPath_SmoothAnim,	new FragmentShader(DefaultContent.SmoothAnimFrag));
			ContentProvider.AddContent(ContentPath_SharpMask,	new FragmentShader(DefaultContent.SharpAlphaFrag));

			Minimal		= ContentProvider.RequestContent<FragmentShader>(ContentPath_Minimal);
			Picking		= ContentProvider.RequestContent<FragmentShader>(ContentPath_Picking);
			SmoothAnim	= ContentProvider.RequestContent<FragmentShader>(ContentPath_SmoothAnim);
			SharpAlpha	= ContentProvider.RequestContent<FragmentShader>(ContentPath_SharpMask);
		}


		protected override ShaderType OglShaderType
		{
			get { return ShaderType.FragmentShader; }
		}
		
		public FragmentShader() : base(DefaultContent.MinimalFrag) {}
		public FragmentShader(string sourceCode) : base(sourceCode) {}
	}
}
