using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Remoting;


namespace Duality
{
	/// <summary>
	/// Wraps a <see cref="Stream"/> within a new one, and forwards all functionality to the underlying Stream.
	/// </summary>
	public abstract class StreamWrapper : Stream
	{
		private Stream	baseStream		= null;


		public override bool CanRead
		{
			get { return this.baseStream.CanRead; }
		}
		public override bool CanSeek
		{
			get { return this.baseStream.CanSeek; }
		}
		public override bool CanWrite
		{
			get { return this.baseStream.CanWrite; }
		}
		public override bool CanTimeout
		{
			get { return this.baseStream.CanTimeout; }
		}
		public override long Length
		{
			get { return this.baseStream.Length; }
		}
		public override long Position
		{
			get { return this.baseStream.Position; }
			set { this.baseStream.Position = value; }
		}
		public override int ReadTimeout
		{
			get { return this.baseStream.ReadTimeout; }
			set { this.baseStream.ReadTimeout = value; }
		}
		public override int WriteTimeout
		{
			get { return this.baseStream.WriteTimeout; }
			set { this.baseStream.WriteTimeout = value; }
		}
		public override ObjRef CreateObjRef(Type requestedType)
		{
			throw new NotSupportedException();
		}


		public StreamWrapper(Stream stream)
		{
			if (stream == null) throw new ArgumentNullException("stream");
			this.baseStream = stream;
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.baseStream.Dispose();
			}
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.baseStream.BeginRead(buffer, offset, count, callback, state);
		}
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.baseStream.BeginWrite(buffer, offset, count, callback, state);
		}
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.baseStream.EndRead(asyncResult);
		}
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.baseStream.EndWrite(asyncResult);
		}

		public override void Close()
		{
			this.baseStream.Close();		
		}
		public override void Flush()
		{
			this.baseStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.baseStream.Read(buffer, offset, count);
		}
		public override int ReadByte()
		{
			return this.baseStream.ReadByte();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.baseStream.Seek(offset, origin);
		}
		public override void SetLength(long value)
		{
			this.baseStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.baseStream.Write(buffer, offset, count);
		}
		public override void WriteByte(byte value)
		{
			this.baseStream.WriteByte(value);
		}
	}
}
