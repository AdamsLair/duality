using System.Linq;

namespace Duality.Launcher
{
	/// <summary>
	/// Abstracts away all the commandline arguments that can be passed to duality.
	/// </summary>
	public class LauncherArgs
	{
		public const string CmdArgDebug = "debug";
		public const string CmdArgEditor = "editor";
		public const string CmdArgProfiling = "profile";

		/// <summary>
		/// Creates a new <see cref="LauncherArgs"/> with the default values.
		/// </summary>
		public LauncherArgs() { }

		/// <summary>
		/// Creates a new <see cref="LauncherArgs"/> using the provided arguments in <paramref name="args"/>
		/// </summary>
		/// <param name="args"></param>
		public LauncherArgs(string[] args)
		{
			this.IsDebugging = args.Contains(CmdArgDebug);
			this.IsRunFromEditor = args.Contains(CmdArgEditor);
			this.IsProfiling = args.Contains(CmdArgProfiling);
			this.Args = args;
		}

		/// <summary>
		/// <c>true</c> when the debug flag is passed.
		/// </summary>
		public bool IsDebugging { get; }

		/// <summary>
		/// <c>true</c> when the editor flag is passed.
		/// </summary>
		public bool IsProfiling { get; }

		/// <summary>
		/// <c>true</c> when the profile flag is passed.
		/// </summary>
		public bool IsRunFromEditor { get; }

		private string[] Args { get; }

		public override string ToString()
		{
			return this.Args != null ? this.Args.ToString(", ") : "null";
		}
	}
}
