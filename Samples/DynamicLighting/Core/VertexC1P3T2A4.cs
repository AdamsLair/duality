using System;
using System.Runtime.InteropServices;

using Duality;
using Duality.Drawing;
using Duality.Resources;

namespace DynamicLighting
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexC1P3T2A4 : IVertexData
	{
		public static readonly VertexDeclaration Declaration = VertexDeclaration.Get<VertexC1P3T2A4>();

		[VertexElement(VertexElementRole.Color)]
		public ColorRgba Color;
		[VertexElement(VertexElementRole.Position)]
		public Vector3 Pos;
		[VertexElement(VertexElementRole.TexCoord)]
		public Vector2 TexCoord;
		public Vector4 Attrib;
		// Add Vector3 for lighting world position, see note in Light.cs

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
