using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public class BackendException : Exception
	{
		public BackendException(string message) : base(message) { }
		public BackendException(string message, Exception inner) : base(message, inner) { }
	}
}
