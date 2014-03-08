using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Remoting;


namespace Duality
{
	/// <summary>
	/// Wraps a <see cref="Stream"/> inside a proxy that allows accessing only a certain portion of the Stream,
	/// beginning a its current Position. Using internal buffering, the SubStream allows seeking and rewinding 
	/// back to its original Position, even if the underlying Stream doesn't. Closing the SubStream will not
	/// close its underlying Stream.
	/// </summary>
	public class SubStreamWrapper : NonClosingStreamWrapper
	{
		private	long			baseStreamOrigin	= 0;
		private	long			maxLength			= -1;
		private	long			originalMaxLength	= -1;
		private MemoryStream	subStream			= new MemoryStream();
		private	byte[]			advanceBuffer		= null;

		public override bool CanSeek
		{
			get { return this.subStream != null; }
		}
		public override bool CanWrite
		{
			get { return this.subStream != null && base.CanSeek && base.CanWrite; }
		}
		public override long Length
		{
			get
			{
				if (this.maxLength == -1)
					return base.Length - this.baseStreamOrigin;
				else if (this.subStream.Length >= this.maxLength)
					return this.maxLength;
				else
					return Math.Min(this.maxLength, base.Length - this.baseStreamOrigin);
			}
		}
		public override long Position
		{
			get { return this.subStream.Position; }
			set 
			{
				if (value < 0) throw new ArgumentOutOfRangeException("Position");
				if (this.maxLength != -1 && value > this.maxLength) throw new ArgumentOutOfRangeException("Position");

				this.RequirePosition(value);
				this.subStream.Position = value;
			}
		}

		public SubStreamWrapper(Stream stream, long maxLength = -1) : base(stream)
		{
			this.baseStreamOrigin = stream.Position;
			this.maxLength = maxLength;
			this.originalMaxLength = maxLength;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.subStream.Dispose();
			this.advanceBuffer = null;
			this.subStream = null;
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this.subStream == null) throw new ObjectDisposedException("Can't operate on closed Stream.");

			switch (origin)
			{
				default:
				case SeekOrigin.Begin:
					this.RequirePosition(offset);
					break;
				case SeekOrigin.Current:
					this.RequirePosition(this.subStream.Position + offset);
					break;
				case SeekOrigin.End:
					this.RequirePosition(this.Length + offset);
					break;
			}
			return this.subStream.Seek(offset, origin);
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.subStream == null) throw new ObjectDisposedException("Can't operate on closed Stream.");
			if (!this.CanRead) throw new InvalidOperationException("This Stream does not support reading.");

			if (this.maxLength != -1)
			{
				count = Math.Min(count, (int)(this.maxLength - this.subStream.Position));
			}

			this.RequirePosition(this.subStream.Position + count);
			return this.subStream.Read(buffer, offset, count);
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.subStream == null) throw new ObjectDisposedException("Can't operate on closed Stream.");
			if (!this.CanWrite) throw new InvalidOperationException("This Stream does not support writing.");
			
			if (this.maxLength != -1)
			{
				count = Math.Min(count, (int)(this.maxLength - this.subStream.Position));
			}

			long oldPos = this.subStream.Position;
			this.subStream.Write(buffer, offset, count);

			long oldBasePos = base.Position;
			long newBasePos = this.baseStreamOrigin + oldPos;
			if (oldBasePos != newBasePos) base.Position = newBasePos;
			base.Write(buffer, offset, count);
			if (oldBasePos != newBasePos) base.Position = oldBasePos;
		}
		public override void SetLength(long value)
		{
			this.maxLength = Math.Min(this.originalMaxLength, value);
			this.subStream.SetLength(this.maxLength);
		}

		private void RequirePosition(long pos)
		{
			pos = Math.Max(pos, 0);
			if (this.maxLength != -1) pos = Math.Min(pos, this.maxLength);

			long requiredBytes = pos - this.subStream.Length;
			if (requiredBytes > 0)
			{
				if (this.advanceBuffer == null) this.advanceBuffer = new byte[1024 * 4];

				long oldSubStreamPos = this.subStream.Position;
				while (requiredBytes > 0)
				{
					int bytesToAdvance = Math.Min(this.advanceBuffer.Length, (int)requiredBytes);
					int bytesRead = base.Read(this.advanceBuffer, 0, bytesToAdvance);

					this.subStream.Write(this.advanceBuffer, 0, bytesRead);
					requiredBytes -= bytesRead;

					if (bytesRead < bytesToAdvance) break;
				}
				this.subStream.Position = oldSubStreamPos;
			}
		}
	}
}
