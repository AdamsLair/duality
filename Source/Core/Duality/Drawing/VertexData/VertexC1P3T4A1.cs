using System;
using System.Runtime.InteropServices;

namespace Duality.Drawing
{
	/// <summary>
	/// Vertex data providing each vertex a position (3x4 byte), color (1x4 byte), two texture coordinates (4x4 byte)
	/// and a custom float vertex attribute (1x4 byte).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexC1P3T4A1 : IVertexData
	{
		public static readonly VertexDeclaration Declaration = VertexDeclaration.Get<VertexC1P3T4A1>();

		/// <summary>
		/// The vertices color.
		/// </summary>
		[VertexElement(VertexElementRole.Color)]
		public ColorRgba Color;
		/// <summary>
		/// The vertices position.
		/// </summary>
		[VertexElement(VertexElementRole.Position)]
		public Vector3 Pos;
		/// <summary>
		/// The vertices texture coordinates (two of them).
		/// </summary>
		[VertexElement(VertexElementRole.TexCoord)]
		public Vector4 TexCoord;
		/// <summary>
		/// The vertices custom attribute.
		/// </summary>
		public float Attrib;

		Vector3 IVertexData.Pos
		{
			get { return this.Pos; }
			set { this.Pos = value; }
		}
		ColorRgba IVertexData.Color
		{
			get { return this.Color; }
			set { this.Color = value; }
		}
		VertexDeclaration IVertexData.Declaration
		{
			get { return Declaration; }
		}
	}
}
