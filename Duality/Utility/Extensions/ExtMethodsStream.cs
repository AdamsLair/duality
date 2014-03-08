using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Remoting;


namespace Duality
{
	public static class ExtMethodsStream
	{
		/// <summary>
		/// Wraps the Stream inside a <see cref="NonClosingStreamWrapper">proxy</see> that won't close the underlying stream when being closed.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static Stream NonClosing(this Stream stream)
		{
			return new NonClosingStreamWrapper(stream);
		}
		/// <summary>
		/// Wraps the Stream inside a proxy that allows accessing only a certain subsection of the Stream,
		/// beginning a its current Position. The <see cref="SubStreamWrapper"/> allows seeking and rewinding 
		/// back to its original Position, even if the underlying Stream doesn't. Closing the sub-Stream will
		/// not close the underlying base Stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="maxLength">The maximum length in bytes that is accessible using the sub-Stream. Specify -1 for not limiting it.</param>
		/// <returns></returns>
		public static Stream SubStream(this Stream stream, long maxLength = -1)
		{
			return new SubStreamWrapper(stream, maxLength);
		}
	}
}
