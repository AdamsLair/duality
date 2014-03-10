using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Duality;
using Duality.Serialization;
using Duality.Serialization.MetaFormat;

using OpenTK;
using NUnit.Framework;

namespace Duality.Tests.Serialization
{
	public class BinaryFormatterTest : FormatterTest
	{
		protected override FormattingMethod PrimaryFormat
		{
			get { return FormattingMethod.Binary; }
		}
	}
}
