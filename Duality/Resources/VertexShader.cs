using System;
using OpenTK.Graphics.OpenGL;

namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL VertexShader.
	/// </summary>
	[Serializable]
	public class VertexShader : AbstractShader
	{
		/// <summary>
		/// A VertexShader resources file extension.
		/// </summary>
		public new const string FileExt = ".VertexShader" + Resource.FileExt;
		
		/// <summary>
		/// (Virtual) base path for Duality's embedded default VertexShaders.
		/// </summary>
		public const string VirtualContentPath = ContentProvider.VirtualContentPath + "VertexShader:";
		/// <summary>
		/// (Virtual) path of the <see cref="Minimal"/> VertexShader.
		/// </summary>
		public const string ContentPath_Minimal		= VirtualContentPath + "Minimal";
		/// <summary>
		/// (Virtual) path of the <see cref="SmoothAnim"/> VertexShader.
		/// </summary>
		public const string ContentPath_SmoothAnim	= VirtualContentPath + "SmoothAnim";

		/// <summary>
		/// [GET] A minimal VertexShader. It performs OpenGLs default transformation
		/// and forwards a single texture coordinate and color to the fragment stage.
		/// </summary>
		public static ContentRef<VertexShader> Minimal		{ get; private set; }
		/// <summary>
		/// [GET] The SmoothAnim VertexShader. In addition to the <see cref="Minimal"/>
		/// setup, it forwards the custom animBlend vertex attribute to the fragment stage.
		/// </summary>
		public static ContentRef<VertexShader> SmoothAnim	{ get; private set; }

		internal static void InitDefaultContent()
		{
			VertexShader tmp;

			tmp = new VertexShader();
			tmp.SetSource(DefaultRes.MinimalVert);
			ContentProvider.RegisterContent(ContentPath_Minimal, tmp);

			tmp = new VertexShader();
			tmp.SetSource(DefaultRes.SmoothAnimVert);
			ContentProvider.RegisterContent(ContentPath_SmoothAnim, tmp);

			Minimal		= ContentProvider.RequestContent<VertexShader>(ContentPath_Minimal);
			SmoothAnim	= ContentProvider.RequestContent<VertexShader>(ContentPath_SmoothAnim);
		}


		protected override ShaderType OglShaderType
		{
			get { return ShaderType.VertexShader; }
		}

		public VertexShader()
		{
			// By default, use minimal shader source.
			this.SetSource(DefaultRes.MinimalVert);
		}
	}
}
