using System;
using System.Linq;

namespace Duality.Launcher
{
	public class LauncherArgs
	{
		public const string CmdArgDebug = "debug";
		public const string CmdArgEditor = "editor";
		public const string CmdArgProfiling = "profile";

		public LauncherArgs() { }

		public LauncherArgs(string[] args)
		{
			this.IsDebugging = args.Contains(CmdArgDebug);
			this.IsRunFromEditor = args.Contains(CmdArgEditor);
			this.IsProfiling = args.Contains(CmdArgProfiling);
			this.Args = args;
		}

		public bool IsDebugging { get; }
		public bool IsProfiling { get; }
		public bool IsRunFromEditor { get; }

		private string[] Args { get; }

		public override string ToString()
		{
			return this.Args != null ? this.Args.ToString(", ") : "null";
		}
	}
}
