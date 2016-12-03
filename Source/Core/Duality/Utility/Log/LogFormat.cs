using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Duality
{
	/// <summary>
	/// Formatting methods that help you transform data and objects into text that reads well
	/// in a log file.
	/// </summary>
	public static class LogFormat
	{
		/// <summary>
		/// Returns a string that can be used for representing the current line and method within a source code file.
		/// This method uses caller information attributes on its parameters - omit them in order to let the compiler do its work.
		/// </summary>
		/// <param name="callerInfoMember"></param>
		/// <param name="callerInfoFile"></param>
		/// <param name="callerInfoLine"></param>
		/// <returns></returns>
		public static string CurrentMethod([CallerMemberName] string callerInfoMember = null, [CallerFilePath] string callerInfoFile = null, [CallerLineNumber] int callerInfoLine = -1)
		{
			return string.Format("{0} in '{1}', line {2}.",
				callerInfoMember,
				callerInfoFile,
				callerInfoLine);
		}

		/// <summary>
		/// Returns a string that can be used for representing a <see cref="System.Reflection.Assembly"/> in log entries.
		/// </summary>
		/// <param name="asm"></param>
		/// <returns></returns>
		public static string Assembly(Assembly asm)
		{
			if (asm == null) return "null";
			string shortName = asm.GetShortAssemblyName();
			try
			{
				Version version = asm.GetName().Version;
				return string.Format("{0} {1}", shortName, version);
			}
			catch (Exception)
			{
				return string.Format("{0} {1}", shortName, "[Error retrieving Version]");
			}
		}
		/// <summary>
		/// Returns a string that can be used for representing a <see cref="System.Type"/> in log entries.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string Type(Type type)
		{
			if (type == null) return "null";
			return type.GetTypeCSCodeName(true);
		}
		/// <summary>
		/// Returns a string that can be used for representing a <see cref="System.Reflection.TypeInfo"/> in log entries.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string Type(TypeInfo type)
		{
			if (type == null) return "null";
			return LogFormat.Type(type.AsType());
		}
		/// <summary>
		/// Returns a string that can be used for representing a method in log entries.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="includeDeclaringType">If true, the methods declaring type is included in the returned name.</param>
		/// <returns></returns>
		public static string MethodInfo(MethodInfo info, bool includeDeclaringType = true)
		{
			if (info == null) return "null";
			string declTypeName = Type(info.DeclaringType);
			string returnTypeName = Type(info.ReturnType);
			string[] paramNames = info.GetParameters().Select(p => Type(p.ParameterType)).ToArray();
			string[] genArgNames = info.GetGenericArguments().Select(Type).ToArray();
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, 
				"{4} {0}{1}{3}({2})",
				includeDeclaringType ? declTypeName + "." : "",
				info.Name,
				paramNames.ToString(", "),
				genArgNames.Length > 0 ? "<" + genArgNames.ToString(", ") + ">" : "",
				returnTypeName);
		}
		/// <summary>
		/// Returns a string that can be used for representing a method or constructor in log entries.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="includeDeclaringType">If true, the methods or constructors declaring type is included in the returned name.</param>
		/// <returns></returns>
		public static string MethodInfo(MethodBase info, bool includeDeclaringType = true)
		{
			if (info is MethodInfo)
				return MethodInfo(info as MethodInfo);
			else if (info is ConstructorInfo)
				return ConstructorInfo(info as ConstructorInfo);
			else if (info != null)
				return info.ToString();
			else
				return "null";
		}
		/// <summary>
		/// Returns a string that can be used for representing a constructor in log entries.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="includeDeclaringType">If true, the constructors declaring type is included in the returned name.</param>
		/// <returns></returns>
		public static string ConstructorInfo(ConstructorInfo info, bool includeDeclaringType = true)
		{
			if (info == null) return "null";
			string declTypeName = Type(info.DeclaringType);
			string[] paramNames = info.GetParameters().Select(p => Type(p.ParameterType)).ToArray();
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, 
				"{0}{1}({2})",
				includeDeclaringType ? declTypeName + "." : "",
				info.DeclaringType.Name,
				paramNames.ToString(", "));
		}
		/// <summary>
		/// Returns a string that can be used for representing a property in log entries.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="includeDeclaringType">If true, the properties declaring type is included in the returned name.</param>
		/// <returns></returns>
		public static string PropertyInfo(PropertyInfo info, bool includeDeclaringType = true)
		{
			if (info == null) return "null";
			string declTypeName = Type(info.DeclaringType);
			string propTypeName = Type(info.PropertyType);
			string[] paramNames = info.GetIndexParameters().Select(p => Type(p.ParameterType)).ToArray();
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, 
				"{0} {1}{2}{3}",
				propTypeName,
				includeDeclaringType ? declTypeName + "." : "",
				info.Name,
				paramNames.Any() ? "[" + paramNames.ToString(", ") + "]" : "");
		}
		/// <summary>
		/// Returns a string that can be used for representing a field in log entries.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="includeDeclaringType">If true, the fields declaring type is included in the returned name.</param>
		/// <returns></returns>
		public static string FieldInfo(FieldInfo info, bool includeDeclaringType = true)
		{
			if (info == null) return "null";
			string declTypeName = Type(info.DeclaringType);
			string fieldTypeName = Type(info.FieldType);
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, 
				"{0} {1}{2}",
				fieldTypeName,
				includeDeclaringType ? declTypeName + "." : "",
				info.Name);
		}
		/// <summary>
		/// Returns a string that can be used for representing an event in log entries.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="includeDeclaringType">If true, the events declaring type is included in the returned name.</param>
		/// <returns></returns>
		public static string EventInfo(EventInfo info, bool includeDeclaringType = true)
		{
			if (info == null) return "null";
			string declTypeName = Type(info.DeclaringType);
			string fieldTypeName = Type(info.EventHandlerType);
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, 
				"{0} {1}{2}",
				fieldTypeName,
				includeDeclaringType ? declTypeName + "." : "",
				info.Name);
		}
		/// <summary>
		/// Returns a string that can be used for representing a(ny) member in log entries.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="includeDeclaringType">If true, the members declaring type is included in the returned name.</param>
		/// <returns></returns>
		public static string MemberInfo(MemberInfo info, bool includeDeclaringType = true)
		{
			if (info is MethodInfo)
				return MethodInfo(info as MethodInfo, includeDeclaringType);
			else if (info is ConstructorInfo)
				return ConstructorInfo(info as ConstructorInfo, includeDeclaringType);
			else if (info is PropertyInfo)
				return PropertyInfo(info as PropertyInfo, includeDeclaringType);
			else if (info is FieldInfo)
				return FieldInfo(info as FieldInfo, includeDeclaringType);
			else if (info is EventInfo)
				return EventInfo(info as EventInfo, includeDeclaringType);
			else if (info is TypeInfo)
				return Type(info as TypeInfo);
			else if (info != null)
				return info.ToString();
			else
				return "null";
		}
		
		/// <summary>
		/// Returns a string that can be used for representing an exception in log entries.
		/// It usually does not include the full call stack and is significantly shorter than
		/// an <see cref="System.Exception">Exceptions</see> ToString method.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static string Exception(Exception e, bool callStack = true)
		{
			if (e == null) return "null";

			string eName = Type(e.GetType());

			return string.Format(System.Globalization.CultureInfo.InvariantCulture, 
				"{0}: {1}{3}CallStack:{3}{2}",
				eName,
				e.Message,
				e.StackTrace,
				Environment.NewLine);
		}
	}
}
