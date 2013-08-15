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
			const string VirtualContentPath			= ContentProvider.VirtualContentPath + "VertexShader:";
			const string ContentPath_Minimal		= VirtualContentPath + "Minimal";
			const string ContentPath_SmoothAnim		= VirtualContentPath + "SmoothAnim";

			ContentProvider.AddContent(ContentPath_Minimal, new VertexShader(DefaultRes.MinimalVert));
			ContentProvider.AddContent(ContentPath_SmoothAnim, new VertexShader(DefaultRes.SmoothAnimVert));

			Minimal		= ContentProvider.RequestContent<VertexShader>(ContentPath_Minimal);
			SmoothAnim	= ContentProvider.RequestContent<VertexShader>(ContentPath_SmoothAnim);
		}


		protected override ShaderType OglShaderType
		{
			get { return ShaderType.VertexShader; }
		}

		public VertexShader() : base(DefaultRes.MinimalVert) {}
		public VertexShader(string sourceCode) : base(sourceCode) {}
	}
}
