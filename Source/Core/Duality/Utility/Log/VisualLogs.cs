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
		private static List<VisualLog> logs = new List<VisualLog>();
		private static VisualLog defaultLog = new VisualLog("Default");
		private static VisualLog[] syncLogs = new VisualLog[0];
		private static object syncObj = new object();

		/// <summary>
		/// [GET] Enumerates all currently existing logs.
		/// </summary>
		public static IEnumerable<VisualLog> All
		{
			get { return syncLogs; }
		}
		/// <summary>
		/// [GET] Returns the default log, which can be used for quick output hacks and miscellaneous data.
		/// To set up a distinct layer for specific kinds of data, use <see cref="VisualLogs.Get">a different</see>
		/// log.
		/// </summary>
		public static VisualLog Default
		{
			get { return defaultLog; }
		}
		
		/// <summary>
		/// Returns a custom global log channel that is defined by a <see cref="CustomVisualLogInfo"/>
		/// implementation, as provided via generic type parameter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static VisualLog Get<T>() where T : CustomVisualLogInfo, new()
		{
			return CustomLogHolder<T>.Instance;
		}

		internal static void PrepareRenderLogEntries()
		{
			if (syncLogs.Any(log => log.Entries.Count > 0) && DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
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
			foreach (VisualLog log in syncLogs)
				log.Update();
		}
		internal static void ClearAll()
		{
			foreach (VisualLog log in syncLogs)
				log.Clear();
		}

		private static class CustomLogHolder<T> where T : CustomVisualLogInfo, new()
		{
			public static VisualLog Instance;

			static CustomLogHolder()
			{
				T logInfo = new T();
				Instance = new VisualLog(logInfo.Name);
				lock (syncObj)
				{
					logs.Add(Instance);
					syncLogs = logs.ToArray();
				}
			}
		}
	}
}
