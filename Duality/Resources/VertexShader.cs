using System;
using System.IO;

using Duality.Properties;
using Duality.Editor;


namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL VertexShader.
	/// </summary>
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageVertexShader)]
	public class VertexShader : AbstractShader
	{
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
			InitDefaultContentFromEmbedded<VertexShader>(".vert", stream =>
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					string code = reader.ReadToEnd();
					return new VertexShader(code);
				}
			});
		}


		protected override ShaderType Type
		{
			get { return ShaderType.Vertex; }
		}

		public VertexShader() : base(string.Empty) {}
		public VertexShader(string sourceCode) : base(sourceCode) {}
	}
}
