using System;
using System.Runtime.InteropServices;

namespace Duality.Drawing
{
	/// <summary>
	/// Vertex data providing each vertex a position (3x4 byte), color (1x4 byte) and texture coordinate (2x4 byte)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexC1P3T2 : IVertexData
	{
		public static readonly VertexDeclaration Declaration = VertexDeclaration.Get<VertexC1P3T2>();

		/// <summary>
		/// The vertices position.
		/// </summary>
		public Vector3 Pos;
		/// <summary>
		/// A depth offset that is applied after the vertex has been transformed.
		/// Used for adjusting rendering order of objects without affecting projection.
		/// </summary>
		public float DepthOffset;
		/// <summary>
		/// The vertices color.
		/// </summary>
		public ColorRgba Color;
		/// <summary>
		/// The vertices texture coordinate.
		/// </summary>
		public Vector2 TexCoord;

		Vector3 IVertexData.Pos
		{
			get { return this.Pos; }
			set { this.Pos = value; }
		}
		float IVertexData.DepthOffset
		{
			get { return this.DepthOffset; }
			set { this.DepthOffset = value; }
		}
		ColorRgba IVertexData.Color
		{
			get { return this.Color; }
			set { this.Color = value; }
		}

		public override string ToString()
		{
			return string.Format(
				"Pos {0}, Color {1}, TexCoord {2}", 
				this.Pos, 
				this.Color, 
				this.TexCoord);
		}
	}
}
