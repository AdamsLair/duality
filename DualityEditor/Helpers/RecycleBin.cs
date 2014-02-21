// David Amenta - dave@DaveAmenta.com
// 05/05/08
// Updated 04/18/2010 for x64

namespace Duality.Editor
{
    /// <summary>
    /// Send files directly to the recycle bin.
    /// </summary>
    public class RecycleBin
    {
        /// <summary>
        /// Send file to recycle bin
        /// </summary>
        /// <param name="path">Location of directory or file to recycle</param>
        /// <param name="flags">FileOperationFlags to add in addition to FOF_ALLOWUNDO</param>
        public static bool Send(string path, NativeMethods.FileOperationFlags flags)
        {
            try
            {
                if (NativeMethods.IsWOW64Process())
                {
                    NativeMethods.SHFILEOPSTRUCT_x64 fs = new NativeMethods.SHFILEOPSTRUCT_x64();
                    fs.wFunc = NativeMethods.FileOperationType.FO_DELETE;
                    // important to double-terminate the string.
                    fs.pFrom = path + '\0' + '\0';
                    fs.fFlags = NativeMethods.FileOperationFlags.FOF_ALLOWUNDO | flags;
                    NativeMethods.SHFileOperation_x64(ref fs);
                }
                else
                {
                    NativeMethods.SHFILEOPSTRUCT_x86 fs = new NativeMethods.SHFILEOPSTRUCT_x86();
                    fs.wFunc = NativeMethods.FileOperationType.FO_DELETE;
                    // important to double-terminate the string.
                    fs.pFrom = path + '\0' + '\0';
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
        /// Send file to recycle bin.  Display dialog, display warning if files are too big to fit (FOF_WANTNUKEWARNING)
        /// </summary>
        /// <param name="path">Location of directory or file to recycle</param>
        public static bool Send(string path) 
        {
            return Send(path, NativeMethods.FileOperationFlags.FOF_NOCONFIRMATION | NativeMethods.FileOperationFlags.FOF_WANTNUKEWARNING);
        }
        /// <summary>
        /// Send file silently to recycle bin.  Surpress dialog, surpress errors, delete if too large.
        /// </summary>
        /// <param name="path">Location of directory or file to recycle</param>
        public static bool SendSilent(string path)
        {
            return Send(path, NativeMethods.FileOperationFlags.FOF_NOCONFIRMATION | NativeMethods.FileOperationFlags.FOF_NOERRORUI | NativeMethods.FileOperationFlags.FOF_SILENT);
        }
    }
}
