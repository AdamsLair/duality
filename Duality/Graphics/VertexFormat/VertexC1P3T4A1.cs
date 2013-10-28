using System;
using System.Runtime.InteropServices;

using Duality.ColorFormat;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Duality.VertexFormat
{
	/// <summary>
	/// Vertex data providing each vertex a position (3x4 byte), color (1x4 byte), two texture coordinates (4x4 byte)
	/// and a custom float vertex attribute (1x4 byte).
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexC1P3T4A1 : IVertexData
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
		/// The vertices texture coordinates (two of them).
		/// </summary>
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
			GL.TexCoordPointer(4, TexCoordPointerType.Float, Size, (IntPtr)OffsetTex0);

			if (mat.Technique.Res.Shader.IsAvailable)
			{
				Resources.ShaderVarInfo[] varInfo = mat.Technique.Res.Shader.Res.VarInfo;
				for (int i = 0; i < varInfo.Length; i++)
				{
					if (varInfo[i].glVarLoc == -1) continue;
					if (varInfo[i].scope != Resources.ShaderVarScope.Attribute) continue;
					if (varInfo[i].type != Resources.ShaderVarType.Float) continue;
				
					GL.EnableVertexAttribArray(varInfo[i].glVarLoc);
					GL.VertexAttribPointer(varInfo[i].glVarLoc, 1, VertexAttribPointerType.Float, false, Size, (IntPtr)OffsetAttrib);
					break;
				}
			}

			// --- From VertexP3T2A2 ---
			//Resources.ShaderVarInfo[] varInfo = mat.Technique.Res.Shader.Res.VarInfo;
			//for (int i = 0; i < varInfo.Length; i++)
			//{
			//    if (varInfo[i].glVarLoc == -1) continue;
			//    if (varInfo[i].scope != Resources.ShaderVarScope.Attribute) continue;
				
			//    GL.EnableVertexAttribArray(varInfo[i].glVarLoc);

			//    int curIndex = 0;
			//    switch (varInfo[i].type)
			//    {
			//        case Resources.ShaderVarType.Float:
			//            GL.VertexAttribPointer(varInfo[i].glVarLoc, varInfo[i].arraySize, VertexAttribPointerType.Float, false, Size, (IntPtr)(OffsetAttrib + sizeof(float) * curIndex));
			//            curIndex += varInfo[i].arraySize;
			//            break;
			//        case Resources.ShaderVarType.Vec2:
			//            GL.VertexAttribPointer(varInfo[i].glVarLoc, 2, VertexAttribPointerType.Float, false, Size, (IntPtr)(OffsetAttrib + sizeof(float) * curIndex));
			//            curIndex += 2;
			//            break;
			//    }
			//}
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

			if (mat.Technique.Res.Shader.IsAvailable)
			{
				Resources.ShaderVarInfo[] varInfo = mat.Technique.Res.Shader.Res.VarInfo;
				for (int i = 0; i < varInfo.Length; i++)
				{
					if (varInfo[i].glVarLoc == -1) continue;
					if (varInfo[i].scope != Resources.ShaderVarScope.Attribute) continue;
					if (varInfo[i].type != Resources.ShaderVarType.Float) continue;
				
					GL.DisableVertexAttribArray(varInfo[i].glVarLoc);
					break;
				}
			}

			// --- From VertexP3T2A2 ---
			//Resources.ShaderVarInfo[] varInfo = mat.Technique.Res.Shader.Res.VarInfo;
			//for (int i = 0; i < varInfo.Length; i++)
			//{
			//    if (varInfo[i].glVarLoc == -1) continue;
			//    if (varInfo[i].scope != Resources.ShaderVarScope.Attribute) continue;
				
			//    GL.DisableVertexAttribArray(varInfo[i].glVarLoc);
			//}
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
		/// Byte offset for the (double) texture coordinate.
		/// </summary>
		public const int OffsetTex0			= OffsetPos + 3 * sizeof(float);
		/// <summary>
		/// Byte offset for the custom attribute.
		/// </summary>
		public const int OffsetAttrib		= OffsetTex0 + 4 * sizeof(float);
		/// <summary>
		/// Total size in bytes.
		/// </summary>
		public const int Size				= OffsetAttrib + 1 * sizeof(float);
		public const int VertexTypeIndex	= Resources.DrawTechnique.VertexType_C1P3T4A1;

		public VertexC1P3T4A1(float x, float y, float z, float s, float t, float p, float q, float attrib, byte r = 255, byte g = 255, byte b = 255, byte a = 255)
		{
			this.Pos.X = x;
			this.Pos.Y = y;
			this.Pos.Z = z;
			this.TexCoord.X = s;
			this.TexCoord.Y = t;
			this.TexCoord.Z = p;
			this.TexCoord.W = q;
			this.Attrib = attrib;
			this.Color.R = r;
			this.Color.G = g;
			this.Color.B = b;
			this.Color.A = a;
		}
		public VertexC1P3T4A1(float x, float y, float z, float s, float t, float p, float q, float attrib, ColorRgba clr)
		{
			this.Pos.X = x;
			this.Pos.Y = y;
			this.Pos.Z = z;
			this.TexCoord.X = s;
			this.TexCoord.Y = t;
			this.TexCoord.Z = p;
			this.TexCoord.W = q;
			this.Attrib = attrib;
			this.Color = clr;
		}
		public VertexC1P3T4A1(Vector3 pos, Vector4 stuv, float attrib, byte r = 255, byte g = 255, byte b = 255, byte a = 255)
		{
			this.Pos = pos;
			this.TexCoord = stuv;
			this.Attrib = attrib;
			this.Color.R = r;
			this.Color.G = g;
			this.Color.B = b;
			this.Color.A = a;
		}
		public VertexC1P3T4A1(Vector3 pos, Vector4 stuv, float attrib, ColorRgba clr)
		{
			this.Pos = pos;
			this.TexCoord = stuv;
			this.Attrib = attrib;
			this.Color = clr;
		}
	}
}
