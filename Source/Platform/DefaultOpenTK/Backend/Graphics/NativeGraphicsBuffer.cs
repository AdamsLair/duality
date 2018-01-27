using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

using OpenTK.Graphics.OpenGL;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeGraphicsBuffer : INativeGraphicsBuffer
	{
		private static NativeGraphicsBuffer boundVertex = null;
		private static NativeGraphicsBuffer boundIndex = null;
		
		public static void Bind(GraphicsBufferType type, NativeGraphicsBuffer buffer)
		{
			if (GetBound(type) == buffer) return;
			SetBound(type, buffer);

			BufferTarget target = ToOpenTKBufferType(type);
			GL.BindBuffer(target, buffer != null ? buffer.Handle : 0);
		}
		private static void SetBound(GraphicsBufferType type, NativeGraphicsBuffer buffer)
		{
			if (type == GraphicsBufferType.Vertex) boundVertex = buffer;
			else if (type == GraphicsBufferType.Index) boundIndex = buffer;
			else return;
		}
		private static NativeGraphicsBuffer GetBound(GraphicsBufferType type)
		{
			if (type == GraphicsBufferType.Vertex) return boundVertex;
			else if (type == GraphicsBufferType.Index) return boundIndex;
			else return null;
		}


		private int handle = 0;
		private GraphicsBufferType type = GraphicsBufferType.Vertex;

		public int Handle
		{
			get { return this.handle; }
		}
		public GraphicsBufferType BufferType
		{
			get { return this.type; }
		}

		public NativeGraphicsBuffer(GraphicsBufferType type)
		{
			this.handle = GL.GenBuffer();
			this.type = type;
		}

		public void LoadData(IntPtr data, int size)
		{
			NativeGraphicsBuffer prevBound = GetBound(this.type);
			Bind(this.type, this);

			BufferTarget target = ToOpenTKBufferType(this.type);
			GL.BufferData(target, size, IntPtr.Zero, BufferUsageHint.StreamDraw);
			GL.BufferData(target, size, data, BufferUsageHint.StreamDraw);

			Bind(this.type, prevBound);
		}
		public void Dispose()
		{
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated &&
				this.handle != 0)
			{
				DefaultOpenTKBackendPlugin.GuardSingleThreadState();
				GL.DeleteBuffer(this.handle);
				this.handle = 0;
			}
		}

		private static BufferTarget ToOpenTKBufferType(GraphicsBufferType value)
		{
			switch (value)
			{
				case GraphicsBufferType.Vertex: return BufferTarget.ArrayBuffer;
				case GraphicsBufferType.Index: return BufferTarget.ElementArrayBuffer;
			}

			return BufferTarget.ArrayBuffer;
		}
	}
}
