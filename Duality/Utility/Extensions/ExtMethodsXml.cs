using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Duality
{
	public static class ExtMethodsXml
	{
		public static string GetAttributeValue(this XElement element, XName name)
		{
			XAttribute attribute = element.Attribute(name);
			return attribute != null ? attribute.Value : null;
		}

		public static IEnumerable<XElement> Descendants(this XContainer container, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return container.Descendants().Where(e => e.Name.LocalName == name.LocalName);
			else
				return container.Descendants(name);
		}
		public static IEnumerable<XElement> Elements(this XContainer node, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return node.Elements().Where(e => e.Name.LocalName == name.LocalName);
			else
				return node.Elements(name);
		}
		public static IEnumerable<XElement> ElementsBeforeSelf(this XContainer node, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return node.ElementsBeforeSelf().Where(e => e.Name.LocalName == name.LocalName);
			else
				return node.ElementsBeforeSelf(name);
		}
		public static IEnumerable<XElement> ElementsAfterSelf(this XContainer node, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return node.ElementsAfterSelf().Where(e => e.Name.LocalName == name.LocalName);
			else
				return node.ElementsAfterSelf(name);
		}
		public static XElement Element(this XContainer node, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return node.Elements().FirstOrDefault(e => e.Name.LocalName == name.LocalName);
			else
				return node.Element(name);
		}

		public static IEnumerable<XElement> Ancestors(this XNode node, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return node.Ancestors().Where(e => e.Name.LocalName == name.LocalName);
			else
				return node.Ancestors(name);
		}
	}
}
