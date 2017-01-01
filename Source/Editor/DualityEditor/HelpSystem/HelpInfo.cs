﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using Duality;

namespace Duality.Editor
{
	public class HelpInfo : IReadOnlyHelpInfo
	{
		private	string	id;
		private	string	topic;
		private	string	desc;
		private	HelpAction	helpAction	= DefaultPerformHelpAction;
		
		public string Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		public string Topic
		{
			get { return this.topic; }
			set { this.topic = value; }
		}
		public string Description
		{
			get { return this.desc; }
			set { this.desc = value; }
		}
		public HelpAction PerformHelpAction
		{
			get { return this.helpAction; }
			set
			{
				if (value == null) value = DefaultPerformHelpAction;
				this.helpAction = value;
			}
		}

		private HelpInfo() {}

		public void AppendText(string text)
		{
			this.desc += "\n\n" + text;
		}

		public static HelpInfo FromText(string topic, string desc, string id = null)
		{
			HelpInfo info = new HelpInfo();
			
			info.id = id;
			info.topic = topic;
			info.desc = desc;

			return info;
		}
		public static HelpInfo FromMember(MemberInfo member)
		{
			if (member == null) return null;
			XmlCodeDoc.Entry doc = HelpSystem.GetXmlCodeDoc(member);

			if (doc != null)
			{
				HelpInfo info = new HelpInfo();

				info.id = member.GetMemberId();
				info.topic = member.Name;
				info.desc = "";

				if (doc.Summary != null) info.desc += doc.Summary;
				if (doc.Remarks != null) info.desc += "\n\n" + doc.Remarks;

				return info;
			}

			return CreateNotAvailable(member.Name);
		}
		public static HelpInfo FromResource(IContentRef res)
		{
			Type resType = res.ResType;

			// Default content? Attempt to retrieve the associated Property description.
			if (res.IsDefaultContent)
			{
				string[] defaultPath = res.Path.Split(':');
				PropertyInfo property = null;
				for (int i = 1; i <  defaultPath.Length; i++)
				{
					string memberName = string.Join("_", defaultPath.Skip(i));
					property = resType.GetProperty(memberName, BindingFlags.Static | BindingFlags.Public);
					if (property != null) break;
				}
				if (property != null)
				{
					return FromMember(property);
				}
			}

			// Default to Type description.
			return FromMember(resType);
		}
		public static HelpInfo FromGameObject(GameObject obj)
		{
			if (obj == null) return null;
			HelpInfo info = FromMember(typeof(GameObject)) ?? new HelpInfo();

			info.topic = obj.FullName;

			return info;
		}
		public static HelpInfo FromComponent(Component cmp)
		{
			if (cmp == null) return null;
			HelpInfo info = FromMember(cmp.GetType()) ?? new HelpInfo();

			info.topic = cmp.ToString();

			return info;
		}
		public static HelpInfo FromSelection(ObjectSelection sel)
		{
			if (sel == null || sel.GameObjectCount != 1) return null;
			return FromGameObject(sel.GameObjects.First());

			// ToDo: Probably improve later
		}
		public static HelpInfo FromObject(object obj)
		{
			if (obj is MemberInfo) return FromMember(obj as MemberInfo);
			if (obj is IContentRef) return FromResource(obj as IContentRef);
			if (obj is GameObject) return FromGameObject(obj as GameObject);
			if (obj is Component) return FromComponent(obj as Component);
			if (obj is ObjectSelection) return FromSelection(obj as ObjectSelection);

			return CreateNotAvailable(obj != null ? obj.ToString() : null);
		}
		public static HelpInfo CreateNotAvailable(string topic)
		{
			return FromText(topic ?? "Unknown", Duality.Editor.Properties.GeneralRes.HelpInfo_NotAvailable_Desc);
		}

		public static bool DefaultPerformHelpAction(HelpInfo info)
		{
			MemberInfo member = !string.IsNullOrEmpty(info.Id) ? ReflectionHelper.ResolveMember(info.Id) : null;
			if (member != null)
			{
				string memberHtmlName;
				if (member is FieldInfo && member.DeclaringType.IsEnum)
					memberHtmlName = member.DeclaringType.GetMemberId();
				else
					memberHtmlName = info.Id;
				memberHtmlName = memberHtmlName.Replace('.', '_').Replace(':', '_').Replace('+', '_');
				
				string ddocPath = Path.GetFullPath("DDoc.chm");
				string cmdLine = string.Format("{0}::/html/{1}.htm", ddocPath, memberHtmlName);

				Process[] proc = Process.GetProcessesByName("hh");
				if (proc.Length > 0) proc[0].CloseMainWindow();
				Process.Start("HH.exe", cmdLine);
				return true;
			}
			
			return false;
		}
	}
}
