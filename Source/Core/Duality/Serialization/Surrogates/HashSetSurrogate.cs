using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.Serialization.Surrogates
{
	public class HashSetSurrogate : SerializeSurrogate<IEnumerable>
	{
		public override bool MatchesType(TypeInfo t)
		{
			return
				t.IsGenericType &&
				t.GetGenericTypeDefinition() == typeof(HashSet<>);
		}
		public override void WriteData(IDataWriter writer)
		{
			IEnumerable hashSet = this.RealObject;
			dynamic dynamicHashSet = this.RealObject;
			object[] values = new object[dynamicHashSet.Count];
			int i = 0;
			foreach (object item in hashSet)
			{
				values[i] = item;
				i++;
			}
			writer.WriteValue("values", values);
		}
		public override void ReadData(IDataReader reader)
		{
			IEnumerable hashSet = this.RealObject;			
			if (hashSet != null)
			{				
				Type hashSetType = hashSet.GetType();
				Type[] genArgs = hashSetType.GenericTypeArguments;
				TypeInfo keyTypeInfo = genArgs[0].GetTypeInfo();
				dynamic dynamicHashset = hashSet;
				dynamicHashset.Clear();

				object[] values;
				reader.ReadValue("values", out values);
				if (values != null)
					foreach (dynamic obj in values)
					{
						if (!CheckValueType(keyTypeInfo, obj)) continue;
						dynamicHashset.Add(obj); //Have to use dynamic here to allow for runtime resolving of the Add method
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
