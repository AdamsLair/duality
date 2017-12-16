using System;
using System.Runtime.InteropServices;

using Duality;
using Duality.Drawing;

namespace SmoothAnimation
{
	/// <summary>
	/// Extended vertex data format that provides an additional field for smooth 
	/// blending between animation frames.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexSmoothAnim : IVertexData
	{
		public static readonly VertexDeclaration Declaration = VertexDeclaration.Get<VertexSmoothAnim>();

		[VertexElement(VertexElementRole.Color)]
		public ColorRgba Color;
		[VertexElement(VertexElementRole.Position)]
		public Vector3 Pos;
		public float DepthOffset;
		[VertexElement(VertexElementRole.TexCoord)]
		public Vector4 TexCoord;
		public float AnimBlend;

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
	}
}
