using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Duality.IO
{
	[Flags]
	public enum FileAccessMode
	{
		None	  = 0x0,

		Read	  = 0x1,
		Write	 = 0x2,

		ReadWrite = Read | Write
	}
}
