using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Reflection;

using Duality;

namespace DualityEditor
{
	public class XmlCodeDoc
	{
		private static readonly char[] MemberNameSep = "{[(".ToCharArray();

		public enum EntryType
		{
			Unknown,

			Type,
			Field,
			Property,
			Event,
			Method,
			Constructor
		}
		public class Entry
		{
			private	EntryType	type;
			private	string		typeName;
			private	string		memberName;
			private	string		summary;
			private	string		remarks;

			public EntryType EntryType
			{
				get { return this.type; }
			}
			public string TypeName
			{
				get { return this.typeName; }
			}
			public string MemberName
			{
				get { return this.memberName; }
			}
			public Type Type
			{
				get { return ReflectionHelper.ResolveType(this.typeName); }
			}
			public MemberInfo Member
			{
				get { return ReflectionHelper.ResolveMember(this.memberName); }
			}

			public string Summary
			{
				get { return this.summary; }
				set { this.summary = value; }
			}
			public string Remarks
			{
				get { return this.remarks; }
				set { this.remarks = value; }
			}

			private Entry(EntryType type, string typeName, string memberName)
			{
				this.type = type;
				this.typeName = typeName;
				this.memberName = memberName;
			}
			public Entry(Entry other)
			{
				this.type = other.type;
				this.typeName = other.typeName;
				this.memberName = other.memberName;
				this.summary = other.summary;
				this.remarks = other.remarks;
			}

			public static Entry Create(MemberInfo member)
			{
				EntryType entryType;
				if (member is Type) entryType = EntryType.Type;
				else if (member is FieldInfo) entryType = EntryType.Field;
				else if (member is PropertyInfo) entryType = EntryType.Property;
				else if (member is MethodInfo) entryType = EntryType.Method;
				else if (member is ConstructorInfo) entryType = EntryType.Constructor;
				else if (member is EventInfo) entryType = EntryType.Event;
				else entryType = EntryType.Unknown;

				if (member is Type)
					return new Entry(entryType, (member as Type).GetTypeId(), member.GetMemberId());
				else if (member != null)
					return new Entry(entryType, member.DeclaringType.GetTypeId(), member.GetMemberId());
				else
					return null;
			}
		}

		private	Dictionary<string,Entry>	entries	= new Dictionary<string,Entry>();

		public XmlCodeDoc()
		{

		}
		public XmlCodeDoc(Stream str)
		{
			this.LoadFromStream(str);
		}
		public XmlCodeDoc(string file)
		{
			this.LoadFromFile(file);
		}

		public void LoadFromFile(string file)
		{
			using (FileStream str = File.OpenRead(file))
			{
				this.LoadFromStream(str);
			}
		}
		public void LoadFromStream(Stream str)
		{
			StreamReader reader = new StreamReader(str);
			this.LoadFromXml(reader.ReadToEnd());
		}
		public void LoadFromXml(string xml)
		{
			this.Clear();

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
			
			XmlNode assemblyNode = xmlDoc.DocumentElement["assembly"];
			XmlNode assemblyNameNode = assemblyNode != null ? assemblyNode["name"] : null;
			string assemblyName = assemblyNameNode.InnerText;

			XmlNode memberNode = xmlDoc.DocumentElement["members"];
			foreach (XmlNode child in memberNode)
			{
				if (child is XmlComment) continue;
				XmlAttribute memberNameAttrib = child.Attributes["name"];
				if (memberNameAttrib == null) continue;

				// Create a member entry based on the determined data.
				MemberInfo member = ResolveDocStyleMember(memberNameAttrib.Value);
				if (member != null)
				{
					Entry memberEntry = Entry.Create(member);

					XmlNode summaryNode = child["summary"];
					XmlNode remarksNode = child["remarks"];
					
					if (summaryNode != null)
					{
						string summary = summaryNode.InnerXml;
						summary = Regex.Replace(summary, "[\n\r\t\\s]+", " ", RegexOptions.Multiline);
						summary = Regex.Replace(summary, "<c>(.*?)<\\/c>", m => m.Groups[1].Value.Trim('"'));
						summary = Regex.Replace(summary, "<code>(.*?)<\\/code>", m => m.Groups[1].Value.Trim('"'));
						summary = Regex.Replace(summary, "<see cref=(\"[^\"]*\")>(.*?)<\\/see>", m => m.Groups[2].Value.Trim('"'));
						summary = Regex.Replace(summary, "<see cref=(\"[^\"]*\")\\s/>", delegate(Match m)
						{
							MemberInfo info = ResolveDocStyleMember(m.Groups[1].Value.Trim('"'));
							return info != null ? info.Name : m.Groups[1].Value; 
						});
						summary = Regex.Replace(summary, "[ ]{2,}", " ");
						memberEntry.Summary = summary.Trim();
					}
					if (remarksNode != null)
					{
						string remarks = remarksNode.InnerXml;
						remarks = Regex.Replace(remarks, "[\n\r\t\\s]+", " ", RegexOptions.Multiline);
						remarks = Regex.Replace(remarks, "<c>(.*?)<\\/c>", m => m.Groups[1].Value.Trim('"'));
						remarks = Regex.Replace(remarks, "<code>(.*?)<\\/code>", m => m.Groups[1].Value.Trim('"'));
						remarks = Regex.Replace(remarks, "<see cref=(\"[^\"]*\")>(.*?)<\\/see>", m => m.Groups[2].Value.Trim('"'));
						remarks = Regex.Replace(remarks, "<see cref=(\"[^\"]*\")\\s/>", delegate(Match m)
						{
							MemberInfo info = ResolveDocStyleMember(m.Groups[1].Value.Trim('"'));
							return info != null ? info.Name : m.Groups[1].Value; 
						});
						remarks = Regex.Replace(remarks, "[ ]{2,}", " ");
						memberEntry.Remarks = remarks.Trim();
					}

					this.AddEntry(memberEntry);
				}
			}
		}

		public void Clear()
		{
			this.entries.Clear();
		}
		public void AddEntry(Entry entry)
		{
			this.entries[entry.MemberName] = entry;
		}
		public void AppendDoc(XmlCodeDoc other)
		{
			foreach (var pair in other.entries)
				this.AddEntry(new Entry(pair.Value));
		}

		public Entry GetMemberDoc(MemberInfo info)
		{
			if (info == null) return null;
			Entry result;
			if (!this.entries.TryGetValue(info.GetMemberId(), out result)) return null;
			return result;
		}

		private static MemberInfo ResolveDocStyleMember(string docId)
		{
			// Determine entry type
			EntryType memberEntryType;
			switch (docId[0])
			{
				case 'M':	memberEntryType = EntryType.Method;		break;
				case 'T':	memberEntryType = EntryType.Type;		break;
				case 'F':	memberEntryType = EntryType.Field;		break;
				case 'P':	memberEntryType = EntryType.Property;	break;
				case 'E':	memberEntryType = EntryType.Event;		break;
				default:	memberEntryType = EntryType.Unknown;	return null;
			}

			// Determine member name and its (Declaring) type name
			string memberName;
			string memberTypeName;
			if (memberEntryType == EntryType.Type)
			{
				memberName = memberTypeName = docId.Remove(0, 2);
			}
			else
			{
				int memberNameSepIndex = docId.IndexOfAny(MemberNameSep);
				int lastDotIndex = memberNameSepIndex != -1 ? 
					docId.LastIndexOf('.', memberNameSepIndex) :
					docId.LastIndexOf('.');
				memberTypeName = docId.Substring(2, lastDotIndex - 2);
				memberName = docId.Substring(lastDotIndex + 1, docId.Length - lastDotIndex - 1);
			}

			// Determine the members (declaring) type
			Type memberType = ResolveDocStyleType(memberTypeName);
			if (memberType == null) return null;

			// Determine the member info
			MemberInfo member = null;
			if (memberEntryType == EntryType.Type)
				member = memberType;
			else if (memberEntryType == EntryType.Field)
				member = memberType.GetField(memberName, ReflectionHelper.BindAll);
			else if (memberEntryType == EntryType.Event)
				member = memberType.GetEvent(memberName, ReflectionHelper.BindAll);
			else if (memberEntryType == EntryType.Property)
			{
				string methodName;
				Type[] paramTypes;
				int paramIndex = memberName.IndexOf('(');
				if (paramIndex != -1)
				{
					methodName = memberName.Substring(0, paramIndex);
					string paramList = memberName.Substring(paramIndex + 1, memberName.Length - paramIndex - 2);
					string[] paramTypeNames = paramList.Split(',');
					paramTypes = new Type[paramTypeNames.Length];
					for (int i = 0; i < paramTypeNames.Length; i++)
					{
						bool isByRef = false;
						if (paramTypeNames[i].EndsWith("@"))
						{
							// ref / out parameter
							paramTypeNames[i] = paramTypeNames[i].Remove(paramTypeNames[i].Length - 1);
							isByRef = true;
						}

						paramTypes[i] = ResolveDocStyleType(paramTypeNames[i]);

						if (isByRef && paramTypes[i] != null) paramTypes[i] = paramTypes[i].MakeByRefType();
					}
				}
				else
				{
					methodName = memberName;
					paramTypes = Type.EmptyTypes;
				}
				member = memberType.GetProperty(methodName, ReflectionHelper.BindAll, null, null, paramTypes, null);
			}
			else if (memberEntryType == EntryType.Method)
			{
				string methodName;
				string[] paramTypeNames;
				int paramIndex = memberName.IndexOf('(');
				if (paramIndex != -1)
				{
					methodName = memberName.Substring(0, paramIndex);
					string paramList = memberName.Substring(paramIndex + 1, memberName.Length - paramIndex - 2);
					paramTypeNames = SplitGenArgs(paramList);
				}
				else
				{
					methodName = memberName;
					paramTypeNames = new string[0];
				}
				
				// Determine parameter types
				Type[] paramTypes = new Type[paramTypeNames.Length];
				bool[] paramTypeByRef = new bool[paramTypeNames.Length];
				for (int i = 0; i < paramTypeNames.Length; i++)
				{
					paramTypeByRef[i] = false;
					if (paramTypeNames[i].EndsWith("@"))
					{
						// ref / out parameter
						paramTypeNames[i] = paramTypeNames[i].Remove(paramTypeNames[i].Length - 1);
						paramTypeByRef[i] = true;
					}

					if (paramTypeNames[i][0] == '`' && paramTypeNames[i][1] != '`')
					{
						int typeGenArgIndex = 0;
						int.TryParse(paramTypeNames[i].Substring(1, paramTypeNames[i].Length - 1), out typeGenArgIndex);
						paramTypes[i] = memberType.GetGenericArguments()[typeGenArgIndex];
					}
					else if (paramTypeNames[i].StartsWith("``"))
						paramTypes[i] = null;
					else
						paramTypes[i] = ResolveDocStyleType(paramTypeNames[i]);

					if (paramTypeByRef[i] && paramTypes[i] != null) paramTypes[i] = paramTypes[i].MakeByRefType();
				}

				if (methodName == "#ctor") memberEntryType = EntryType.Constructor;
				if (memberEntryType == EntryType.Constructor)
				{
					member = memberType.GetConstructor(paramTypes);
				}
				else
				{
					int genMethodArgDeclIndex = methodName.IndexOf("``", System.StringComparison.Ordinal);
					int genMethodArgs = 0;
					if (genMethodArgDeclIndex != -1)
					{
						genMethodArgs = int.Parse(methodName.Substring(genMethodArgDeclIndex + 2, methodName.Length - genMethodArgDeclIndex - 2));
						methodName = methodName.Remove(genMethodArgDeclIndex);
					}

					MethodInfo[] availMethods = memberType.GetMethods(ReflectionHelper.BindAll).Where(
						m => m.Name == methodName && 
						m.GetGenericArguments().Length == genMethodArgs &&
						m.GetParameters().Length == paramTypes.Length).ToArray();

					// Select the method that fits
					foreach (MethodInfo method in availMethods)
					{
						bool possibleMatch = true;
						ParameterInfo[] methodParams = method.GetParameters();
						for (int i = 0; i < methodParams.Length; i++)
						{
							// Generic method param
							if (paramTypes[i] == null)
							{
								Type genMethodParam = ResolveDocStyleType(paramTypeNames[i], method);
								if (paramTypeByRef[i] && genMethodParam != null) genMethodParam = genMethodParam.MakeByRefType();

								if (genMethodParam != methodParams[i].ParameterType)
								{
									possibleMatch = false;
									break;
								}
							}
							// Some other param
							else if (methodParams[i].ParameterType != paramTypes[i])
							{
								possibleMatch = false;
								break;
							}
						}
						if (possibleMatch)
						{
							member = method;
							break;
						}
					}
				}
			}

			return member;
		}
		private static Type ResolveDocStyleType(string typeString, MethodInfo declaringMethod = null)
		{
			return ReflectionHelper.ResolveType(ConvertFromDocStyleType(typeString), false, declaringMethod);
		}
		private static string ConvertFromDocStyleType(string typeString)
		{
			int genArgStartIndex = typeString.IndexOf('{');
			int genArgEndIndex = typeString.LastIndexOf('}');
			if (genArgStartIndex != -1)
			{
				string genArgList = typeString.Substring(genArgStartIndex + 1, genArgEndIndex - genArgStartIndex - 1);
				string[] genArgStrings = SplitGenArgs(genArgList);
				typeString = 
					typeString.Substring(0, genArgStartIndex) + "`" + genArgStrings.Length + "[" + 
					genArgStrings.ToString(s => "[" + ConvertFromDocStyleType(s) + "]", ",") + 
					"]" + typeString.Substring(genArgEndIndex + 1, typeString.Length - genArgEndIndex - 1);
			}

			return typeString;
		}
		private static string[] SplitGenArgs(string paramList)
		{
			return ReflectionHelper.SplitArgs(paramList, '{', '}', ',', 0);
		}
	}
}
