using System;
using System.Collections.Generic;
using System.Text;

// Based on (with modifications):
// David Amenta - dave@DaveAmenta.com
// 05/05/08
// Updated 04/18/2010 for x64

namespace Duality.Editor
{
    /// <summary>
    /// Provides an API to send files and directories to the recycle bin. 
	/// 
	/// Note that each operation comes with an overhead, so it is usually a 
	/// lot better to call an API method once for N files than N times for each 
	/// file individually.
    /// </summary>
    public static class RecycleBin
    {
        /// <summary>
        /// Send file to recycle bin
        /// </summary>
        /// <param name="paths">Location of directory or file to recycle</param>
        /// <param name="flags">FileOperationFlags to add in addition to FOF_ALLOWUNDO</param>
        private static bool Send(IEnumerable<string> paths, NativeMethods.FileOperationFlags flags)
        {
            try
            {
				// Generate a single buffer containing all the file names
				StringBuilder builder = new StringBuilder();
				foreach (string path in paths)
				{
					builder.Append(path);
					builder.Append('\0');
				}
				builder.Append('\0'); // Double-termination signals the end of the buffer
				string buffer = builder.ToString();

				if (NativeMethods.IsWOW64Process())
				{
					NativeMethods.SHFILEOPSTRUCT_x64 fs = new NativeMethods.SHFILEOPSTRUCT_x64();
					fs.wFunc = NativeMethods.FileOperationType.FO_DELETE;
					fs.pFrom = buffer;
					fs.fFlags = NativeMethods.FileOperationFlags.FOF_ALLOWUNDO | flags;
					NativeMethods.SHFileOperation_x64(ref fs);
				}
				else
				{
					NativeMethods.SHFILEOPSTRUCT_x86 fs = new NativeMethods.SHFILEOPSTRUCT_x86();
					fs.wFunc = NativeMethods.FileOperationType.FO_DELETE;
					fs.pFrom = buffer;
					fs.fFlags = NativeMethods.FileOperationFlags.FOF_ALLOWUNDO | flags;
					NativeMethods.SHFileOperation_x86(ref fs);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Send file to the recycle bin. Display dialog, display warning if files are too big to fit (FOF_WANTNUKEWARNING)
		/// </summary>
		/// <param name="path">Location of directory or file to recycle</param>
		public static bool Send(string path) 
		{
			return Send(new string[] { path });
		}
		/// <summary>
		/// Send a list of files to the recycle bin.  Display dialog, display warning if files are too big to fit (FOF_WANTNUKEWARNING)
		/// </summary>
		/// <param name="paths">Location of directories or files to recycle</param>
		public static bool Send(IEnumerable<string> paths) 
		{
			return Send(paths, NativeMethods.FileOperationFlags.FOF_NOCONFIRMATION | NativeMethods.FileOperationFlags.FOF_WANTNUKEWARNING);
		}
		/// <summary>
		/// Send file silently to recycle bin. Suppress dialog, suppress errors, delete if too large.
		/// </summary>
		/// <param name="path">Location of directory or file to recycle</param>
		public static bool SendSilent(string path)
        {
            return SendSilent(new string[] { path });
        }
		/// <summary>
		/// Send a list of files silently to the recycle bin. Suppress dialog, suppress errors, delete if too large.
		/// </summary>
		/// <param name="paths">Location of directories or files to recycle</param>
		public static bool SendSilent(IEnumerable<string> paths) 
		{
			return Send(paths, NativeMethods.FileOperationFlags.FOF_NOCONFIRMATION | NativeMethods.FileOperationFlags.FOF_NOERRORUI | NativeMethods.FileOperationFlags.FOF_SILENT);
		}
    }
}
