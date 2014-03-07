using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Remoting;


namespace Duality.Serialization
{
	/// <summary>
	/// Wraps a <see cref="Stream"/>, but only pretends to close it without actually doing so.
	/// </summary>
	public class NonClosingStreamWrapper : Stream
	{
		private Stream	baseStream		= null;
		private	bool	pretendClosed	= false;


		public Stream BaseStream
		{
			get { return this.baseStream; }
		}
		public override bool CanRead
		{
			get { return !this.pretendClosed && this.baseStream.CanRead; }
		}
		public override bool CanSeek
		{
			get { return !this.pretendClosed && this.baseStream.CanSeek; }
		}
		public override bool CanWrite
		{
			get { return !this.pretendClosed && this.baseStream.CanWrite; }
		}
		public override bool CanTimeout
		{
			get { return !this.pretendClosed && this.baseStream.CanTimeout; }
		}
		public override long Length
		{
			get
			{
				this.ThrowIfClosed();
				return this.baseStream.Length;
			}
		}
		public override long Position
		{
			get
			{
				this.ThrowIfClosed();
				return this.baseStream.Position;
			}
			set
			{
				this.ThrowIfClosed();
				this.baseStream.Position = value;
			}
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


		public NonClosingStreamWrapper(Stream stream)
		{
			if (stream == null) throw new ArgumentNullException("stream");
			this.baseStream = stream;
		}
		
		private void ThrowIfClosed()
		{
			if (this.pretendClosed)
			{
				throw new InvalidOperationException("The Stream has been closed or disposed.");
			}
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.pretendClosed = true;
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.ThrowIfClosed();
			return this.baseStream.BeginRead(buffer, offset, count, callback, state);
		}
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.ThrowIfClosed();
			return this.baseStream.BeginWrite(buffer, offset, count, callback, state);
		}
		public override int EndRead(IAsyncResult asyncResult)
		{
			this.ThrowIfClosed();
			return this.baseStream.EndRead(asyncResult);
		}
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.ThrowIfClosed();
			this.baseStream.EndWrite(asyncResult);
		}

		public override void Close()
		{
			if (!this.pretendClosed)
			{
				this.baseStream.Flush();
			}
			this.pretendClosed = true;			
		}
		public override void Flush()
		{
			this.ThrowIfClosed();
			this.baseStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			this.ThrowIfClosed();
			return this.baseStream.Read(buffer, offset, count);
		}
		public override int ReadByte()
		{
			this.ThrowIfClosed();
			return this.baseStream.ReadByte();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			this.ThrowIfClosed();
			return this.baseStream.Seek(offset, origin);
		}
		public override void SetLength(long value)
		{
			this.ThrowIfClosed();
			this.baseStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.ThrowIfClosed();
			this.baseStream.Write(buffer, offset, count);
		}
		public override void WriteByte(byte value)
		{
			this.ThrowIfClosed();
			this.baseStream.WriteByte(value);
		}
	}
}
