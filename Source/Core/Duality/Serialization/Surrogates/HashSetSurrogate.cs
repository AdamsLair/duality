using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.Serialization.Surrogates
{
	public class HashSetSurrogate : SerializeSurrogate<dynamic>
	{
		public override bool MatchesType(TypeInfo t)
		{
			return
				t.IsGenericType &&
				t.GetGenericTypeDefinition() == typeof(HashSet<>);
		}
		public override void WriteData(IDataWriter writer)
		{
			dynamic hashSet = this.RealObject;
			object[] values = new object[hashSet.Count];
			int i = 0;
			foreach (var item in hashSet)
			{
				values[i] = item;
				i++;
			}
			writer.WriteValue("values", values);
		}
		public override void ReadData(IDataReader reader)
		{
			dynamic hashSet = this.RealObject;
			if (hashSet != null)
			{
				Type hashSetType = hashSet.GetType();
				Type[] genArgs = hashSetType.GenericTypeArguments;
				TypeInfo keyTypeInfo = genArgs[0].GetTypeInfo();

				hashSet.Clear();

				object[] values;
				reader.ReadValue("values", out values);
				if (values != null)
					foreach (dynamic obj in values)
					{
						if (!CheckValueType(keyTypeInfo, obj)) continue;
						hashSet.Add(obj);
					}
			}
		}

		private static bool CheckValueType(TypeInfo valueTypeInfo, object value)
		{
			if (!valueTypeInfo.IsInstanceOfType(value))
			{
				Log.Core.WriteWarning(
					"Actual Type '{0}' of value in hashset field '{1}' does not match reflected hashset field type '{2}'. Skipping value.",
					value != null ? Log.Type(value.GetType()) : "unknown",
					value,
					Log.Type(valueTypeInfo));
				return false;
			}
			return true;
		}
	}
}
