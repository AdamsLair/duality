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
		public override bool MatchesType(Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
		}
		public override void WriteData(IDataWriter writer)
		{
			Type dictType = this.RealObject.GetType();
			Type[] genArgs = dictType.GetGenericArguments();
			MethodInfo cast = typeof(DictionarySurrogate).GetMethod("WriteDataSpecific", ReflectionHelper.BindInstanceAll).MakeGenericMethod(genArgs);
			cast.Invoke(this, new[] { writer });
		}
		public override void ReadData(IDataReader reader)
		{
			Type dictType = this.RealObject.GetType();
			Type[] genArgs = dictType.GetGenericArguments();
			MethodInfo cast = typeof(DictionarySurrogate).GetMethod("ReadDataSpecific", ReflectionHelper.BindInstanceAll).MakeGenericMethod(genArgs);
			cast.Invoke(this, new[] { reader });
		}

		protected void WriteDataSpecific<T,U>(IDataWriter writer)
		{
			Dictionary<T,U> dict = this.RealObject as Dictionary<T,U>;
			
			if (typeof(T) == typeof(string))
			{
				foreach (var pair in dict)
					writer.WriteValue(pair.Key as string, pair.Value);
			}
			else
			{
				writer.WriteValue("keys", dict.Keys.ToArray());
				writer.WriteValue("values", dict.Values.ToArray());
			}
		}
		protected void ReadDataSpecific<T,U>(IDataReader reader)
		{
			Dictionary<T,U> dict = this.RealObject as Dictionary<T,U>;
			
			dict.Clear();
			if (typeof(T) == typeof(string))
			{
				foreach (var key in reader.Keys)
					dict.Add((T)(object)key, reader.ReadValue<U>(key));
			}
			else
			{
				T[] keys;
				U[] values;
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
