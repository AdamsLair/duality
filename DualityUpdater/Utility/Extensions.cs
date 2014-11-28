using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Duality.Updater
{
	public static class Extensions
	{
		public static IEnumerable<XElement> Descendants(this XContainer container, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return container.Descendants().Where(e => e.Name.LocalName == name.LocalName);
			else
				return container.Descendants(name);
		}
		public static IEnumerable<XElement> Descendants(this IEnumerable<XContainer> container, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return container.Descendants().Where(e => e.Name.LocalName == name.LocalName);
			else
				return container.Descendants(name);
		}
		public static IEnumerable<XElement> Elements(this XContainer container, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return container.Elements().Where(e => e.Name.LocalName == name.LocalName);
			else
				return container.Elements(name);
		}
		public static IEnumerable<XElement> Elements(this IEnumerable<XContainer> container, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return container.Elements().Where(e => e.Name.LocalName == name.LocalName);
			else
				return container.Elements(name);
		}
		public static XAttribute Attribute(this XElement element, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return element.Attributes().Where(a => a.Name.LocalName == name.LocalName).FirstOrDefault();
			else
				return element.Attribute(name);
		}
		public static IEnumerable<XAttribute> Attributes(this XElement element, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return element.Attributes().Where(a => a.Name.LocalName == name.LocalName);
			else
				return element.Attributes(name);
		}
		public static IEnumerable<XAttribute> Attributes(this IEnumerable<XElement> elements, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return elements.Attributes().Where(a => a.Name.LocalName == name.LocalName);
			else
				return elements.Attributes(name);
		}

		public static void RemoveUpwards(this XElement element)
		{
			XElement parent = element.Parent;
			element.Remove();
			if (parent != null && !parent.Nodes().Any())
				parent.RemoveUpwards();
		}

		public static string ToString<T>(this IEnumerable<T> collection, Func<T, string> toString, string separator)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var item in collection)
			{
				sb.Append(toString(item));
				sb.Append(separator);
			}
			return sb.ToString(0, Math.Max(0, sb.Length - separator.Length));  // Remove at the end is faster
		}
	}
}
