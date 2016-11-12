﻿using System;
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
		/// The vertices texture coordinate.
		/// </summary>
		[VertexElement(VertexElementRole.TexCoord)]
		public Vector2 TexCoord;

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
