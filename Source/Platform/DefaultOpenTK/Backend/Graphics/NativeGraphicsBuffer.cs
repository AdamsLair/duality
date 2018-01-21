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
		
		public static void Bind(NativeGraphicsBuffer buffer)
		{
			if (GetBound(buffer.BufferType) == buffer) return;
			SetBound(buffer.BufferType, buffer);

			BufferTarget target = ToOpenTKBufferType(buffer.BufferType);
			GL.BindBuffer(target, buffer.Handle);
		}
		public static void ResetBinding(GraphicsBufferType type)
		{
			if (GetBound(type) == null) return;
			SetBound(type, null);

			BufferTarget target = ToOpenTKBufferType(type);
			GL.BindBuffer(target, 0);
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

		void INativeGraphicsBuffer.LoadData(IntPtr data, IntPtr size)
		{
			BufferTarget target = ToOpenTKBufferType(this.type);

			NativeGraphicsBuffer prevBound = GetBound(this.type);
			Bind(this);

			GL.BufferData(target, size, IntPtr.Zero, BufferUsageHint.StreamDraw);
			GL.BufferData(target, size, data, BufferUsageHint.StreamDraw);

			Bind(prevBound);
		}
		void IDisposable.Dispose()
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
