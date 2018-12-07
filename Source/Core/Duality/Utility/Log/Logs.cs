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
		private static List<Log>        logs         = new List<Log>();
		private static List<ILogOutput> globalOutput = new List<ILogOutput>();

		private static Log[]        syncLogs   = new Log[0];
		private static ILogOutput[] syncOutput = new ILogOutput[0];
		private static object       syncObj    = new object();

		private static Log logGame   = null;
		private static Log logCore   = null;
		private static Log logEditor = null;
		
		/// <summary>
		/// [GET] Enumerates the log output instances to which all global logs write.
		/// </summary>
		public static IEnumerable<ILogOutput> GlobalOutput
		{
			get { return syncOutput; }
		}
		/// <summary>
		/// [GET] Enumerates all global logs.
		/// </summary>
		public static IEnumerable<Log> All
		{
			get { return syncLogs; }
		}
		/// <summary>
		/// [GET] A global log channel for game-related entries. Use this for logging data from game plugins.
		/// </summary>
		public static Log Game
		{
			get { return logGame; }
		}
		/// <summary>
		/// [GET] A global log channel for core-related entries. This is normally only used by Duality itsself.
		/// </summary>
		public static Log Core
		{
			get { return logCore; }
		}
		/// <summary>
		/// [GET] A global log channel for editor-related entries. This is used by the Duality editor and its plugins.
		/// </summary>
		public static Log Editor
		{
			get { return logEditor; }
		}

		[System.Diagnostics.DebuggerNonUserCode]
		static Logs()
		{
			logGame   = new Log("Game", "Game");
			logCore   = new Log("Core", "Core");
			logEditor = new Log("Editor", "Editor");
			logs.Add(logGame);
			logs.Add(logCore);
			logs.Add(logEditor);
			syncLogs = logs.ToArray();
		}

		/// <summary>
		/// Adds the specified <see cref="ILogOutput"/> to every log global channel.
		/// </summary>
		/// <param name="output"></param>
		public static void AddGlobalOutput(ILogOutput output)
		{
			lock (syncObj)
			{
				globalOutput.Add(output);
				syncOutput = globalOutput.ToArray();
				foreach (Log log in logs)
				{
					log.AddOutput(output);
				}
			}
		}
		/// <summary>
		/// Removes the specified <see cref="ILogOutput"/> from every log global channel.
		/// </summary>
		/// <param name="output"></param>
		public static void RemoveGlobalOutput(ILogOutput output)
		{
			lock (syncObj)
			{
				globalOutput.Remove(output);
				syncOutput = globalOutput.ToArray();
				foreach (Log log in logs)
				{
					log.RemoveOutput(output);
				}
			}
		}

		/// <summary>
		/// Returns a custom global log channel that is defined by a <see cref="CustomLogInfo"/>
		/// implementation, as provided via generic type parameter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static Log Get<T>() where T : CustomLogInfo, new()
		{
			return CustomLogHolder<T>.Instance;
		}

		private static class CustomLogHolder<T> where T : CustomLogInfo, new()
		{
			public static Log Instance;

			static CustomLogHolder()
			{
				T logInfo = new T();
				Instance = new Log(logInfo.Name, logInfo.Id, logInfo);
				lock (syncObj)
				{
					foreach (ILogOutput output in globalOutput)
					{
						Instance.AddOutput(output);
					}
					logs.Add(Instance);
					syncLogs = logs.ToArray();
				}
			}
		}
	}
}
