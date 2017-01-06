using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Duality.Serialization.Surrogates
{
	/// <summary>
	/// De/Serializes a <see cref="Regex"/>.
	/// </summary>
	public class RegexSurrogate : SerializeSurrogate<Regex>
	{
		public override void WriteConstructorData(IDataWriter writer)
		{
			writer.WriteValue("pattern", this.RealObject.ToString());
			writer.WriteValue("options", this.RealObject.Options);
			writer.WriteValue("timeout", this.RealObject.MatchTimeout);
		}
		public override void WriteData(IDataWriter writer) {}
		public override object ConstructObject(IDataReader reader, TypeInfo objType)
		{
			string pattern;
			RegexOptions options;
			TimeSpan timeout;

			reader.ReadValue("pattern", out pattern);
			reader.ReadValue("options", out options);
			reader.ReadValue("timeout", out timeout);

			return new Regex(pattern, options, timeout);
		}
		public override void ReadData(IDataReader reader) {}
	}
}
