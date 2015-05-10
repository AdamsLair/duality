using System;
using System.Runtime.InteropServices;

namespace Duality.Drawing
{
	/// <summary>
	/// Vertex data providing each vertex a position (3x4 byte) and color (1x4 byte).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexC1P3 : IVertexData
	{
		public static readonly VertexFormatDefinition FormatDefinition = new VertexFormatDefinition(typeof(VertexC1P3));

		/// <summary>
		/// The vertices position.
		/// </summary>
		[VertexField(VertexFieldRole.Position)]
		public Vector3 Pos;
		/// <summary>
		/// The vertices color.
		/// </summary>
		[VertexField(VertexFieldRole.Color)]
		public ColorRgba Color;

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
		VertexFormatDefinition IVertexData.Format
		{
			get { return FormatDefinition; }
		}
	}
}
