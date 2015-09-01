using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.Serialization.Surrogates
{
	/// <summary>
	/// De/Serializes a <see cref="System.Collections.Generic.Dictionary{T,U}"/>.
	/// </summary>
	public class DictionarySurrogate : SerializeSurrogate<IDictionary>
	{
		public override bool MatchesType(TypeInfo t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
		}
		public override void WriteData(IDataWriter writer)
		{
			IDictionary dict = this.RealObject;
			TypeInfo dictType = dict.GetType().GetTypeInfo();
			Type[] genArgs = dictType.GenericTypeArguments;

			if (genArgs[0] == typeof(string))
			{
				foreach (DictionaryEntry entry in dict)
					writer.WriteValue(entry.Key as string, entry.Value);
			}
			else
			{
				object[] keys = new object[dict.Keys.Count];
				object[] values = new object[dict.Values.Count];

				dict.Keys.CopyTo(keys, 0);
				dict.Values.CopyTo(values, 0);

				writer.WriteValue("keys", keys);
				writer.WriteValue("values", values);
			}
		}
		public override void ReadData(IDataReader reader)
		{
			IDictionary dict = this.RealObject;
			TypeInfo dictType = dict.GetType().GetTypeInfo();
			Type[] genArgs = dictType.GenericTypeArguments;

			dict.Clear();
			if (genArgs[0] == typeof(string))
			{
				foreach (var key in reader.Keys)
					dict.Add(key, reader.ReadValue(key));
			}
			else
			{
				object[] keys;
				object[] values;
				reader.ReadValue("keys", out keys);
				reader.ReadValue("values", out values);

				for (int i = 0; i < keys.Length; i++)
				{
					if (keys[i] == null) continue;
					dict.Add(keys[i], values[i]);
				}
			}
		}
	}
}
