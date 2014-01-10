using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Duality;
using Duality.Serialization;

using OpenTK;
using NUnit.Framework;

namespace DualityTests.Serialization
{
	[TestFixture]
	public class XmlFormatterTest : FormatterTest
	{
		protected override Formatter CreateFormatter(Stream stream)
		{
			return Formatter.Create(stream, FormattingMethod.Xml);
		}
		protected override Formatter CreateMetaFormatter(Stream stream)
		{
			return Formatter.CreateMeta(stream, FormattingMethod.Xml);
		}
	}
}
