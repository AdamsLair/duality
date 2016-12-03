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
			TypeInfo keyTypeInfo = genArgs[0].GetTypeInfo();
			TypeInfo valueTypeInfo = genArgs[1].GetTypeInfo();

			dict.Clear();
			if (genArgs[0] == typeof(string))
			{
				foreach (string key in reader.Keys)
				{
					object value = reader.ReadValue(key);

					if (!CheckKeyType(keyTypeInfo, key)) continue;
					if (!CheckValueType(valueTypeInfo, value)) continue;

					dict.Add(key, value);
				}
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
					if (!CheckKeyType(keyTypeInfo, keys[i])) continue;
					if (!CheckValueType(valueTypeInfo, values[i])) continue;

					dict.Add(keys[i], values[i]);
				}
			}
		}

		private static bool CheckKeyType(TypeInfo keyTypeInfo, object key)
		{
			if (!keyTypeInfo.IsInstanceOfType(key))
			{
				Logs.Core.WriteWarning(
					"Actual Type '{0}' of dictionary key '{1}' does not match reflected dictionary key type '{2}'. Skipping value.", 
					key != null ? LogFormat.Type(key.GetType()) : "unknown", 
					key, 
					LogFormat.Type(keyTypeInfo));
				return false;
			}
			return true;
		}
		private static bool CheckValueType(TypeInfo valueTypeInfo, object value)
		{
			if (!valueTypeInfo.IsInstanceOfType(value))
			{
				Logs.Core.WriteWarning(
					"Actual Type '{0}' of value in dictionary field '{1}' does not match reflected dictionary field type '{2}'. Skipping value.", 
					value != null ? LogFormat.Type(value.GetType()) : "unknown", 
					value, 
					LogFormat.Type(valueTypeInfo));
				return false;
			}
			return true;
		}
	}
}
