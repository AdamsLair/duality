using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.Serialization.Surrogates
{
	public class HashSetSurrogate : SerializeSurrogate<IEnumerable>
	{
		private class ReflectedHashsetData
		{
			public MethodInfo WriteDataMethod { get; private set; }
			public MethodInfo ReadDataMethod { get; private set; }

			public ReflectedHashsetData(Type hashSetType)
			{
				MethodInfo writeData = typeof(HashSetSurrogate).GetRuntimeMethods().FirstOrDefault(m => m.IsGenericMethod && m.Name == "WriteData");
				this.WriteDataMethod = writeData.MakeGenericMethod(hashSetType.GenericTypeArguments);

				MethodInfo readData = typeof(HashSetSurrogate).GetRuntimeMethods().FirstOrDefault(m => m.IsGenericMethod && m.Name == "ReadData");
				this.ReadDataMethod = readData.MakeGenericMethod(hashSetType.GenericTypeArguments);
			}
		}

		private readonly Dictionary<Type, ReflectedHashsetData> _reflectedData = new Dictionary<Type, ReflectedHashsetData>();

		public override bool MatchesType(TypeInfo t)
		{
			return
				t.IsGenericType &&
				t.GetGenericTypeDefinition() == typeof(HashSet<>);
		}
		public override void WriteData(IDataWriter writer)
		{
			IEnumerable hashSet = this.RealObject;
			if (hashSet != null)
			{
				ReflectedHashsetData reflectedHashsetData = GetReflectedHashsetData(hashSet.GetType());
				reflectedHashsetData.WriteDataMethod.Invoke(null, new object[] { hashSet, writer });
			}
		}

		public override void ReadData(IDataReader reader)
		{
			IEnumerable hashSet = this.RealObject;			
			if (hashSet != null)
			{
				ReflectedHashsetData reflectedHashsetData = GetReflectedHashsetData(hashSet.GetType());
				reflectedHashsetData.ReadDataMethod.Invoke(null, new object[] { hashSet, reader });
			}
		}

		private static void WriteData<T>(HashSet<T> hashSet, IDataWriter writer)
		{
			T[] values = new T[hashSet.Count];
			int i = 0;
			foreach (T item in hashSet)
			{
				values[i] = item;
				i++;
			}
			writer.WriteValue("values", values);
		}

		private static void ReadData<T>(HashSet<T> hashSet, IDataReader reader)
		{			
			T[] values;
			reader.ReadValue("values", out values);
			if (values != null)
			{
				hashSet.Clear();
				var keyTypeInfo = typeof(T).GetTypeInfo();
				foreach (var obj in values)
				{
					if (!CheckValueType(keyTypeInfo, obj)) continue;
					hashSet.Add(obj);
				}
			}
		}

		private ReflectedHashsetData GetReflectedHashsetData(Type hashSetType)
		{
			ReflectedHashsetData reflectedHashsetData;
			if (!this._reflectedData.TryGetValue(hashSetType, out reflectedHashsetData))
			{
				reflectedHashsetData = new ReflectedHashsetData(hashSetType);
				this._reflectedData.Add(hashSetType, reflectedHashsetData);
			}

			return reflectedHashsetData;
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
