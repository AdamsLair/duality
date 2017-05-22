using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using Duality.IO;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

namespace Duality.Editor.PackageManagement
{
	internal class PackageManagerLogger : NuGet.ILogger
	{
		void NuGet.ILogger.Log(NuGet.MessageLevel level, string message, params object[] args)
		{
			switch (level)
			{
				case NuGet.MessageLevel.Debug:
					#if DEBUG
						Log.Editor.Write(message, args);
					#endif
					break;
				default:
				case NuGet.MessageLevel.Info:
					Log.Editor.Write(message, args);
					break;
				case NuGet.MessageLevel.Warning:
					Log.Editor.WriteWarning(message, args);
					break;
				case NuGet.MessageLevel.Error:
					Log.Editor.WriteError(message, args);
					break;
			}
		}
		NuGet.FileConflictResolution NuGet.IFileConflictResolver.ResolveFileConflict(string message)
		{
			Log.Editor.Write("Package File Conflict: {0}", message);
			return NuGet.FileConflictResolution.Overwrite;
		}
	}
}
