using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

namespace Duality.Editor
{
	internal class NativeMethods
	{
		public const int GW_HWNDNEXT = 2; // The next window is below the specified window
		public const int GW_HWNDPREV = 3; // The previous window is above

		[StructLayout(LayoutKind.Sequential)]
		public struct IconInfo
		{
		  public bool fIcon;
		  public int xHotspot;
		  public int yHotspot;
		  public IntPtr hbmMask;
		  public IntPtr hbmColor;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Message
		{
			public IntPtr hWnd;
			public uint msg;
			public IntPtr wParam;
			public IntPtr lParam;
			public uint time;
			public Point p;
		}
		
		/// <summary>
		/// Possible flags for the SHFileOperation method.
		/// </summary>
		[Flags]
		public enum FileOperationFlags: ushort
		{
			/// <summary>
			/// Do not show a dialog during the process
			/// </summary>
			FOF_SILENT =                0x0004,
			/// <summary>
			/// Do not ask the user to confirm selection
			/// </summary>
			FOF_NOCONFIRMATION =        0x0010,
			/// <summary>
			/// Delete the file to the recycle bin.  (Required flag to send a file to the bin
			/// </summary>
			FOF_ALLOWUNDO =             0x0040,
			/// <summary>
			/// Do not show the names of the files or folders that are being recycled.
			/// </summary>
			FOF_SIMPLEPROGRESS =        0x0100,
			/// <summary>
			/// Surpress errors, if any occur during the process.
			/// </summary>
			FOF_NOERRORUI =             0x0400,
			/// <summary>
			/// Warn if files are too big to fit in the recycle bin and will need
			/// to be deleted completely.
			/// </summary>
			FOF_WANTNUKEWARNING =       0x4000,
		}

		/// <summary>
		/// File Operation Function Type for SHFileOperation
		/// </summary>
		public enum FileOperationType: uint
		{
			/// <summary>
			/// Move the objects
			/// </summary>
			FO_MOVE =                   0x0001,
			/// <summary>
			/// Copy the objects
			/// </summary>
			FO_COPY =                   0x0002,
			/// <summary>
			/// Delete (or recycle) the objects
			/// </summary>
			FO_DELETE =                 0x0003,
			/// <summary>
			/// Rename the object(s)
			/// </summary>
			FO_RENAME =                 0x0004,
		}

		public enum KeyMapType : uint 
		{
			MAPVK_VK_TO_VSC    = 0x0,
			MAPVK_VSC_TO_VK    = 0x1,
			MAPVK_VK_TO_CHAR   = 0x2,
			MAPVK_VSC_TO_VK_EX = 0x3,
			MAPVK_VK_TO_VSC_EX = 0x4
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public struct SHFILEOPSTRUCT_x86
		{
			public IntPtr hwnd;
			[MarshalAs(UnmanagedType.U4)]
			public FileOperationType wFunc;
			public string pFrom;
			public string pTo;
			public FileOperationFlags fFlags;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fAnyOperationsAborted;
			public IntPtr hNameMappings;
			public string lpszProgressTitle;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SHFILEOPSTRUCT_x64
		{
			public IntPtr hwnd;
			[MarshalAs(UnmanagedType.U4)]
			public FileOperationType wFunc;
			public string pFrom;
			public string pTo;
			public FileOperationFlags fFlags;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fAnyOperationsAborted;
			public IntPtr hNameMappings;
			public string lpszProgressTitle;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Point
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LowLevelHookStruct
		{
			public Point pt;
			public uint mouseData;
			public uint flags;
			public uint time;
			public IntPtr dwExtraInfo;
		}

		public static bool IsWOW64Process()
		{
			return IntPtr.Size == 8;
		}

		[DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "SHFileOperation")]
		public static extern int SHFileOperation_x86(ref SHFILEOPSTRUCT_x86 FileOp);
		[DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint="SHFileOperation")]
		public static extern int SHFileOperation_x64(ref SHFILEOPSTRUCT_x64 FileOp);

		[System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
		[DllImport("user32.dll")]
		public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

		[DllImport("user32.dll", CharSet=CharSet.Auto, CallingConvention=CallingConvention.Winapi)]
		public static extern IntPtr GetFocus();

		[DllImport("user32.dll")]
		public static extern IntPtr GetTopWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(IntPtr hWnd);
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindow", SetLastError = true)]
		public static extern IntPtr GetNextWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int wFlag);

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint code, KeyMapType mapType);

		// Copy from screen
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

		#region Global Hooks
		public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644988(v=vs.85).aspx see Return value
		public static IntPtr SUPPRESS_OTHER_HOOKS = new IntPtr(1);
		private const int WH_MOUSE_LL = 14;

		public enum MouseMessages
		{
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_MOUSEMOVE = 0x0200,
			WM_MOUSEWHEEL = 0x020A,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);
		public static IntPtr SetWindowsMouseHookEx(LowLevelMouseProc proc)
		{
			using (Process curProcess = Process.GetCurrentProcess())
			{
				using (ProcessModule curModule = curProcess.MainModule)
				{
					return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
				}
			}
		}
		#endregion

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);
	}
}
