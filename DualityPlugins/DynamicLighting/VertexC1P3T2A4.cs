using System;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Duality.ColorFormat;
using Duality.VertexFormat;
using Duality.Resources;

namespace DynamicLighting
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexC1P3T2A4 : IVertexData
	{
		internal static int vertexTypeIndex = DrawTechnique.VertexType_Unknown;
		public static int VertexTypeIndex
		{
			get { return vertexTypeIndex; }
		}

		public ColorRgba Color;
		public Vector3 Pos;
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
		public int TypeIndex
		{
			get { return vertexTypeIndex; }
		}
		
		void IVertexData.SetupVBO(BatchInfo mat)
		{
			GL.EnableClientState(ArrayCap.ColorArray);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);

			GL.ColorPointer(4, ColorPointerType.UnsignedByte, Size, (IntPtr)OffsetColor);
			GL.VertexPointer(3, VertexPointerType.Float, Size, (IntPtr)OffsetPos);
			GL.TexCoordPointer(2, TexCoordPointerType.Float, Size, (IntPtr)OffsetTex0);

			if (mat.Technique.Res.Shader.IsAvailable)
			{
				ShaderVarInfo[] varInfo = mat.Technique.Res.Shader.Res.VarInfo;
				for (int i = 0; i < varInfo.Length; i++)
				{
					if (varInfo[i].glVarLoc == -1) continue;
					if (varInfo[i].scope != ShaderVarScope.Attribute) continue;
					if (varInfo[i].type != ShaderVarType.Vec4) continue;
				
					GL.EnableVertexAttribArray(varInfo[i].glVarLoc);
					GL.VertexAttribPointer(varInfo[i].glVarLoc, 4, VertexAttribPointerType.Float, false, Size, (IntPtr)OffsetAttrib);
					break;
				}
			}
		}
		void IVertexData.UploadToVBO<T>(T[] vertexData, int length)
		{
			GL.BufferData(BufferTarget.ArrayBuffer, IntPtr.Zero, IntPtr.Zero, BufferUsageHint.StreamDraw);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Size * length), vertexData, BufferUsageHint.StreamDraw);
		}
		void IVertexData.FinishVBO(BatchInfo mat)
		{
			GL.DisableClientState(ArrayCap.ColorArray);
			GL.DisableClientState(ArrayCap.VertexArray);
			GL.DisableClientState(ArrayCap.TextureCoordArray);

			if (mat.Technique.Res.Shader.IsAvailable)
			{
				ShaderVarInfo[] varInfo = mat.Technique.Res.Shader.Res.VarInfo;
				for (int i = 0; i < varInfo.Length; i++)
				{
					if (varInfo[i].glVarLoc == -1) continue;
					if (varInfo[i].scope != ShaderVarScope.Attribute) continue;
					if (varInfo[i].type != ShaderVarType.Vec4) continue;
				
					GL.DisableVertexAttribArray(varInfo[i].glVarLoc);
					break;
				}
			}
		}


		public const int OffsetColor	= 0;
		public const int OffsetPos		= OffsetColor + 4 * sizeof(byte);
		public const int OffsetTex0		= OffsetPos + 3 * sizeof(float);
		public const int OffsetAttrib	= OffsetTex0 + 2 * sizeof(float);
		public const int Size			= OffsetAttrib + 4 * sizeof(float);
	}
}
