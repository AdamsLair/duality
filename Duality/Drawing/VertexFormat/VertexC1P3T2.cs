using System;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Duality.Drawing
{
	/// <summary>
	/// Vertex data providing each vertex a position (3x4 byte), color (1x4 byte) and texture coordinate (2x4 byte)
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexC1P3T2 : IVertexData
	{
		/// <summary>
		/// The vertices color.
		/// </summary>
		public ColorRgba Color;
		/// <summary>
		/// The vertices position.
		/// </summary>
		public Vector3 Pos;
		/// <summary>
		/// The vertices texture coordinate.
		/// </summary>
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
		int IVertexData.TypeIndex
		{
			get { return VertexTypeIndex; }
		}
		
		void IVertexData.SetupVBO(Resources.BatchInfo mat)
		{
			GL.EnableClientState(ArrayCap.ColorArray);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);

			GL.ColorPointer(4, ColorPointerType.UnsignedByte, Size, (IntPtr)OffsetColor);
			GL.VertexPointer(3, VertexPointerType.Float, Size, (IntPtr)OffsetPos);
			GL.TexCoordPointer(2, TexCoordPointerType.Float, Size, (IntPtr)OffsetTex0);
		}
		void IVertexData.UploadToVBO<T>(T[] vertexData, int vertexCount)
		{
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Size * vertexCount), IntPtr.Zero, BufferUsageHint.StreamDraw);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Size * vertexCount), vertexData, BufferUsageHint.StreamDraw);
		}
		void IVertexData.FinishVBO(Resources.BatchInfo mat)
		{
			GL.DisableClientState(ArrayCap.ColorArray);
			GL.DisableClientState(ArrayCap.VertexArray);
			GL.DisableClientState(ArrayCap.TextureCoordArray);
		}
		
		/// <summary>
		/// Byte offset for the color.
		/// </summary>
		public const int OffsetColor		= 0;
		/// <summary>
		/// Byte offset for the position.
		/// </summary>
		public const int OffsetPos			= OffsetColor + 4 * sizeof(byte);
		/// <summary>
		/// Byte offset for the texture coordinate.
		/// </summary>
		public const int OffsetTex0			= OffsetPos + 3 * sizeof(float);
		/// <summary>
		/// Total size in bytes.
		/// </summary>
		public const int Size				= OffsetTex0 + 2 * sizeof(float);
		public const int VertexTypeIndex	= Resources.DrawTechnique.VertexType_C1P3T2;

		public VertexC1P3T2(float x, float y, float z, float u, float v, byte r = 255, byte g = 255, byte b = 255, byte a = 255)
		{
			this.Pos.X = x;
			this.Pos.Y = y;
			this.Pos.Z = z;
			this.TexCoord.X = u;
			this.TexCoord.Y = v;
			this.Color.R = r;
			this.Color.G = g;
			this.Color.B = b;
			this.Color.A = a;
		}
		public VertexC1P3T2(float x, float y, float z, float u, float v, ColorRgba clr)
		{
			this.Pos.X = x;
			this.Pos.Y = y;
			this.Pos.Z = z;
			this.TexCoord.X = u;
			this.TexCoord.Y = v;
			this.Color = clr;
		}
		public VertexC1P3T2(Vector3 pos, Vector2 uv, byte r = 255, byte g = 255, byte b = 255, byte a = 255)
		{
			this.Pos = pos;
			this.TexCoord = uv;
			this.Color.R = r;
			this.Color.G = g;
			this.Color.B = b;
			this.Color.A = a;
		}
		public VertexC1P3T2(Vector3 pos, Vector2 uv, ColorRgba clr)
		{
			this.Pos = pos;
			this.TexCoord = uv;
			this.Color = clr;
		}
	}
}
