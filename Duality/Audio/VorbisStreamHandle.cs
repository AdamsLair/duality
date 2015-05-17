using System;
using System.IO;
using System.Collections.Generic;

using NVorbis;

namespace Duality.Audio
{
	public class VorbisStreamHandle : IDisposable
	{
		private bool disposed;
		private VorbisReader ovStream;

		public bool Disposed
		{
			get { return this.disposed; }
		}
		internal VorbisReader VorbisInstance
		{
			get { return this.ovStream; }
		}
		
		internal VorbisStreamHandle(byte[] memory)
		{
			this.ovStream = new VorbisReader(new MemoryStream(memory), true);
		}

		~VorbisStreamHandle()
		{
			this.Dispose(false);
		}
		private void Dispose(bool manually)
		{
			if (this.disposed) return;
			
			if (this.ovStream != null)
			{
				this.ovStream.Dispose();
				this.ovStream = null;
			}

			this.disposed = true;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
