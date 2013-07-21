using System;
using OpenTK.Graphics.OpenGL;

namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL FragmentShader.
	/// </summary>
	[Serializable]
	public class FragmentShader : AbstractShader
	{
		/// <summary>
		/// A FragmentShader resources file extension.
		/// </summary>
		public new const string FileExt = ".FragmentShader" + Resource.FileExt;
		
		/// <summary>
		/// (Virtual) base path for Duality's embedded default FragmentShaders.
		/// </summary>
		public const string VirtualContentPath = ContentProvider.VirtualContentPath + "FragmentShader:";
		/// <summary>
		/// (Virtual) path of the <see cref="Minimal"/> FragmentShader.
		/// </summary>
		public const string ContentPath_Minimal		= VirtualContentPath + "Minimal";
		/// <summary>
		/// (Virtual) path of the <see cref="Picking"/> FragmentShader.
		/// </summary>
		public const string ContentPath_Picking		= VirtualContentPath + "Picking";
		/// <summary>
		/// (Virtual) path of the <see cref="SmoothAnim"/> FragmentShader.
		/// </summary>
		public const string ContentPath_SmoothAnim	= VirtualContentPath + "SmoothAnim";
		
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

		internal static void InitDefaultContent()
		{
			FragmentShader tmp;

			tmp = new FragmentShader();
			tmp.SetSource(DefaultRes.MinimalFrag);
			ContentProvider.RegisterContent(ContentPath_Minimal, tmp);

			tmp = new FragmentShader();
			tmp.SetSource(DefaultRes.PickingFrag);
			ContentProvider.RegisterContent(ContentPath_Picking, tmp);

			tmp = new FragmentShader();
			tmp.SetSource(DefaultRes.SmoothAnimFrag);
			ContentProvider.RegisterContent(ContentPath_SmoothAnim, tmp);

			Minimal		= ContentProvider.RequestContent<FragmentShader>(ContentPath_Minimal);
			Picking		= ContentProvider.RequestContent<FragmentShader>(ContentPath_Picking);
			SmoothAnim	= ContentProvider.RequestContent<FragmentShader>(ContentPath_SmoothAnim);
		}


		protected override ShaderType OglShaderType
		{
			get { return ShaderType.FragmentShader; }
		}
		
		public FragmentShader()
		{
			// By default, use minimal shader source.
			this.SetSource(DefaultRes.MinimalFrag);
		}
	}
}
