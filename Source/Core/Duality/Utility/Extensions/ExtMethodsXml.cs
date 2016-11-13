using System;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
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
		public static bool TryGetAttributeValue<T>(this XElement element, XName name, ref T target)
		{
			string val = GetAttributeValue(element, name);
			return TryConvertFromXml<T>(val, ref target);
		}
		public static bool GetAttributeValue<T>(this XElement element, XName name, out T target)
		{
			target = default(T);
			return TryGetAttributeValue<T>(element, name, ref target);
		}
		public static T GetAttributeValue<T>(this XElement element, XName name, T defaultValue)
		{
			T target = defaultValue;
			TryGetAttributeValue<T>(element, name, ref target);
			return target;
		}

		public static string GetElementValue(this XElement element, XName name)
		{
			XElement childElement = element.Element(name);
			return childElement != null ? childElement.Value : null;
		}
		public static bool TryGetElementValue<T>(this XElement element, XName name, ref T target)
		{
			string val = GetElementValue(element, name);
			return TryConvertFromXml<T>(val, ref target);
		}
		public static bool GetElementValue<T>(this XElement element, XName name, out T target)
		{
			target = default(T);
			return TryGetElementValue<T>(element, name, ref target);
		}
		public static T GetElementValue<T>(this XElement element, XName name, T defaultValue)
		{
			T target = defaultValue;
			TryGetElementValue<T>(element, name, ref target);
			return target;
		}

		public static string GetInnerXml(this XElement element)
		{
			var reader = element.CreateReader();
			reader.MoveToContent();
			return reader.ReadInnerXml();
		}

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
		public static IEnumerable<XElement> Elements(this XContainer node, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return node.Elements().Where(e => e.Name.LocalName == name.LocalName);
			else
				return node.Elements(name);
		}
		public static IEnumerable<XElement> Elements(this IEnumerable<XContainer> node, XName name, bool ignoreNamespace)
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
		public static IEnumerable<XElement> Ancestors(this IEnumerable<XContainer> nodes, XName name, bool ignoreNamespace)
		{
			if (ignoreNamespace)
				return nodes.Ancestors().Where(e => e.Name.LocalName == name.LocalName);
			else
				return nodes.Ancestors(name);
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

		private static bool TryConvertFromXml<T>(string valueString, ref T value)
		{
			if (valueString == null)
				return false;

			if (typeof(T) == typeof(string))
			{
				value = (T)(object)valueString;
				return true;
			}

			if (string.IsNullOrWhiteSpace(valueString))
				return false;

			try
			{
				if      (typeof(T) == typeof(decimal)) { value = (T)(object)XmlConvert.ToDecimal(valueString);	return true; }
				else if (typeof(T) == typeof(double))  { value = (T)(object)XmlConvert.ToDouble(valueString);	return true; }
				else if (typeof(T) == typeof(float))   { value = (T)(object)XmlConvert.ToSingle(valueString);	return true; }
				else if (typeof(T) == typeof(long))    { value = (T)(object)XmlConvert.ToInt64(valueString);	return true; }
				else if (typeof(T) == typeof(int))     { value = (T)(object)XmlConvert.ToInt32(valueString);	return true; }
				else if (typeof(T) == typeof(short))   { value = (T)(object)XmlConvert.ToInt16(valueString);	return true; }
				else if (typeof(T) == typeof(sbyte))   { value = (T)(object)XmlConvert.ToSByte(valueString);	return true; }
				else if (typeof(T) == typeof(bool))    { value = (T)(object)XmlConvert.ToBoolean(valueString);	return true; }
				else if (typeof(T) == typeof(ulong))   { value = (T)(object)XmlConvert.ToUInt64(valueString);	return true; }
				else if (typeof(T) == typeof(uint))    { value = (T)(object)XmlConvert.ToUInt32(valueString);	return true; }
				else if (typeof(T) == typeof(ushort))  { value = (T)(object)XmlConvert.ToUInt16(valueString);	return true; }
				else if (typeof(T) == typeof(byte))    { value = (T)(object)XmlConvert.ToByte(valueString);		return true; }

				TypeInfo typeInfo = typeof(T).GetTypeInfo();
				if (typeInfo.IsEnum)
				{
					value = (T)Enum.Parse(typeof(T), valueString);
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}

			throw new NotImplementedException();
		}
	}
}
