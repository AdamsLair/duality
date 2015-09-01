using System;
using System.Collections.Generic;
using System.Linq;

using Duality.IO;

namespace Duality.Backend
{
	public interface ISystemBackend : IDualityBackend
	{
		IFileSystem FileSystem { get; }
	}
}
