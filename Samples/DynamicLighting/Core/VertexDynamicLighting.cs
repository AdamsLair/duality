using System;
using System.Runtime.InteropServices;

using Duality;
using Duality.Drawing;

namespace DynamicLighting
{
	/// <summary>
	/// Extended vertex data format that provides an additional field for dynamic
	/// lighting calculations in the fragment or vertex shader.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexDynamicLighting : IVertexData
	{
		public static readonly VertexDeclaration Declaration = VertexDeclaration.Get<VertexDynamicLighting>();

		public ColorRgba Color;
		public Vector3 Pos;
		public float DepthOffset;
		public Vector2 TexCoord;
		public Vector4 LightingParam;
		// Add Vector3 for lighting world position, see note in Light.cs

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
