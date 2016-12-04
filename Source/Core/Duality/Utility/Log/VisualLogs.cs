using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

using Duality.Resources;
using Duality.Drawing;
using Duality.Components.Diagnostics;

namespace Duality
{
	/// <summary>
	/// Static global registry of default and custom <see cref="VisualLog"/> instances.
	/// </summary>
	public static class VisualLogs
	{
		private	static	VisualLog						logDefault	= new VisualLog("Default");
		private static	Dictionary<string,VisualLog>	logRegistry	= new Dictionary<string,VisualLog> { { logDefault.Name, logDefault } };

		/// <summary>
		/// [GET] Enumerates all currently existing logs.
		/// </summary>
		public static IEnumerable<VisualLog> All
		{
			get { return logRegistry.Values; }
		}
		/// <summary>
		/// [GET] Returns the default log, which can be used for quick output hacks and miscellaneous data.
		/// To set up a distinct layer for specific kinds of data, use <see cref="VisualLogs.Get">a different</see>
		/// log.
		/// </summary>
		public static VisualLog Default
		{
			get { return logDefault; }
		}
		/// <summary>
		/// [GET] Creates or retrieves a named log. Once a log has been created, it will remain available until
		/// explicitly removed.
		/// </summary>
		/// <param name="logName"></param>
		/// <returns></returns>
		public static VisualLog Get(string logName)
		{
			VisualLog log;
			if (!logRegistry.TryGetValue(logName, out log))
			{
				log = new VisualLog(logName);
				logRegistry[logName] = log;
			}
			return log;
		}

		internal static void PrepareRenderLogEntries()
		{
			if (logRegistry.Values.Any(log => log.Entries.Count > 0) && DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				VisualLogRenderer renderer = Scene.Current.FindComponent<VisualLogRenderer>(false);
				if (renderer == null)
				{
					GameObject obj = new GameObject("VisualLogRenderer");
					obj.AddComponent<VisualLogRenderer>();
					Scene.Current.AddObject(obj);
				}
			}
		}
		internal static void UpdateLogEntries()
		{
			foreach (VisualLog log in logRegistry.Values)
				log.Update();
		}
		internal static void ClearAll()
		{
			foreach (VisualLog log in logRegistry.Values)
				log.Clear();
		}
	}
}
