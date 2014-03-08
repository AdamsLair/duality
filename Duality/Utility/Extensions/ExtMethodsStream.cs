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
		/// Wraps the Stream inside a proxy that won't close the underlying stream when being closed.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static Stream NonClosing(this Stream stream)
		{
			return new NonClosingStreamWrapper(stream);
		}
	}
}
