using System;
using System.IO;

using Duality.Properties;
using Duality.Editor;


namespace Duality.Resources
{
	/// <summary>
	/// Represents an OpenGL FragmentShader.
	/// </summary>
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageFragmentShader)]
	public class FragmentShader : AbstractShader
	{
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
			InitDefaultContentFromEmbedded<FragmentShader>(".frag", stream =>
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					string code = reader.ReadToEnd();
					return new FragmentShader(code);
				}
			});
		}


		protected override ShaderType Type
		{
			get { return ShaderType.Fragment; }
		}
		
		public FragmentShader() : base(string.Empty) {}
		public FragmentShader(string sourceCode) : base(sourceCode) {}
	}
}
