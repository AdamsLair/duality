using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Duality
{
	/// <summary>
	/// Static global registry of default and custom Duality <see cref="Log"/> instances.
	/// </summary>
	public static class Logs
	{
		private static Log logGame   = null;
		private static Log logCore   = null;
		private static Log logEditor = null;

		/// <summary>
		/// [GET] A log for game-related entries. Use this for logging data from game plugins.
		/// </summary>
		public static Log Game
		{
			get { return logGame; }
		}
		/// <summary>
		/// [GET] A log for core-related entries. This is normally only used by Duality itsself.
		/// </summary>
		public static Log Core
		{
			get { return logCore; }
		}
		/// <summary>
		/// [GET] A log for editor-related entries. This is used by the Duality editor and its plugins.
		/// </summary>
		public static Log Editor
		{
			get { return logEditor; }
		}

		[System.Diagnostics.DebuggerNonUserCode]
		static Logs()
		{
			Log.SharedState state = new Log.SharedState();

			logGame   = new Log("Game", state);
			logCore   = new Log("Core", state);
			logEditor = new Log("Edit", state);
		}

		public static void AddGlobalOutput(ILogOutput output)
		{
			logGame.AddOutput(output);
			logCore.AddOutput(output);
			logEditor.AddOutput(output);
		}
		public static void RemoveGlobalOutput(ILogOutput output)
		{
			logGame.RemoveOutput(output);
			logCore.RemoveOutput(output);
			logEditor.RemoveOutput(output);
		}
	}
}
