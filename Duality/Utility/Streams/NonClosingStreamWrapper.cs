using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Remoting;


namespace Duality
{
	/// <summary>
	/// Wraps a <see cref="Stream"/>, but only pretends to close it without actually doing so.
	/// </summary>
	public class NonClosingStreamWrapper : StreamWrapper
	{
		private	bool	pretendClosed	= false;


		public override bool CanRead
		{
			get { return !this.pretendClosed && base.CanRead; }
		}
		public override bool CanSeek
		{
			get { return !this.pretendClosed && base.CanSeek; }
		}
		public override bool CanWrite
		{
			get { return !this.pretendClosed && base.CanWrite; }
		}
		public override bool CanTimeout
		{
			get { return !this.pretendClosed && base.CanTimeout; }
		}
		public override long Length
		{
			get
			{
				this.ThrowIfClosed();
				return base.Length;
			}
		}
		public override long Position
		{
			get
			{
				this.ThrowIfClosed();
				return base.Position;
			}
			set
			{
				this.ThrowIfClosed();
				base.Position = value;
			}
		}


		public NonClosingStreamWrapper(Stream stream) : base(stream) {}
		
		private void ThrowIfClosed()
		{
			if (this.pretendClosed)
			{
				throw new ObjectDisposedException("The Stream has been closed or disposed.");
			}
		}
		protected override void Dispose(bool disposing)
		{
			this.Close();
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.ThrowIfClosed();
			return base.BeginRead(buffer, offset, count, callback, state);
		}
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.ThrowIfClosed();
			return base.BeginWrite(buffer, offset, count, callback, state);
		}
		public override int EndRead(IAsyncResult asyncResult)
		{
			this.ThrowIfClosed();
			return base.EndRead(asyncResult);
		}
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.ThrowIfClosed();
			base.EndWrite(asyncResult);
		}

		public override void Close()
		{
			if (!this.pretendClosed)
			{
				base.Flush();
			}
			this.pretendClosed = true;			
		}
		public override void Flush()
		{
			this.ThrowIfClosed();
			base.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			this.ThrowIfClosed();
			return base.Read(buffer, offset, count);
		}
		public override int ReadByte()
		{
			this.ThrowIfClosed();
			return base.ReadByte();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			this.ThrowIfClosed();
			return base.Seek(offset, origin);
		}
		public override void SetLength(long value)
		{
			this.ThrowIfClosed();
			base.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.ThrowIfClosed();
			base.Write(buffer, offset, count);
		}
		public override void WriteByte(byte value)
		{
			this.ThrowIfClosed();
			base.WriteByte(value);
		}
	}
}
