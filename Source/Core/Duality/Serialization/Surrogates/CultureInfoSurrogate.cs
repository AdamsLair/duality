using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;

namespace Duality.Serialization.Surrogates
{
	/// <summary>
	/// De/Serializes a <see cref="CultureInfo"/> instance.
	/// </summary>
	public class CultureInfoSurrogate : SerializeSurrogate<CultureInfo>
	{
		public override void WriteConstructorData(IDataWriter writer)
		{
			writer.WriteValue("name", this.RealObject.Name);
		}
		public override void WriteData(IDataWriter writer) {}
		public override object ConstructObject(IDataReader reader, TypeInfo objType)
		{
			string name;
			reader.ReadValue("name", out name);
			try
			{
				return new CultureInfo(name);
			}
			catch (Exception)
			{
				Log.Core.WriteError("Unable to resolve CultureInfo '{0}'. Falling back to invariant culture instead.", name);
				return CultureInfo.InvariantCulture;
			}
		}
		public override void ReadData(IDataReader reader) {}
	}
}
