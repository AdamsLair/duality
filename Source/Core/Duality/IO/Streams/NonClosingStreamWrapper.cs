﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Duality.IO
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
			if (!this.pretendClosed)
			{
				// If the base stream is still open, flush it.
				if (base.CanWrite)
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
