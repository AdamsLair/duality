using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class DictionarySurrogate : CloneSurrogate<IDictionary>
	{
		public override bool MatchesType(TypeInfo t)
		{
			return 
				t.IsGenericType && 
				t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
		}

		public override void SetupCloneTargets(IDictionary source, IDictionary target, ICloneTargetSetup setup)
		{
			Type dictType = source.GetType();
			Type[] genArgs = dictType.GenericTypeArguments;
			TypeInfo firstGenArgInfo = genArgs[0].GetTypeInfo();
			TypeInfo secondGenArgInfo = genArgs[1].GetTypeInfo();

			// Handle all keys and values. Reuse existing objects wherever possible.
			if (!firstGenArgInfo.IsPlainOldData())
			{
				if (source == target)
				{
					foreach (object key in source.Keys)
						setup.HandleObject(key, key);
				}
				else
				{
					foreach (var key in source.Keys)
						setup.HandleObject(key, null);
				}
			}
			if (!secondGenArgInfo.IsPlainOldData())
			{
				if (source == target)
				{
					foreach (object val in source.Values)
						setup.HandleObject(val, val);
				}
				else
				{
					foreach (DictionaryEntry entry in source)
						setup.HandleObject(entry.Value, target[entry.Key]);
				}
			}
		}
		public override void CopyDataTo(IDictionary source, IDictionary target, ICloneOperation operation)
		{
			Type dictType = source.GetType();
			Type[] genArgs = dictType.GenericTypeArguments;
			TypeInfo firstGenArgInfo = genArgs[0].GetTypeInfo();
			TypeInfo secondGenArgInfo = genArgs[1].GetTypeInfo();

			// Determine unwrapping behavior to provide faster / more optimized loops.
			bool isKeyPlainOld = firstGenArgInfo.IsPlainOldData();
			bool isValuePlainOld = secondGenArgInfo.IsPlainOldData();

			// Copy all pairs. Don't check each pair, if the Type won't be unwrapped anyway.
			target.Clear();
			if (!isKeyPlainOld && !isValuePlainOld)
			{
				foreach (DictionaryEntry pair in source)
				{
					object keyTarget = null;
					object valueTarget = null;
					if (!operation.HandleObject(pair.Key, ref keyTarget)) continue;
					if (!operation.HandleObject(pair.Value, ref valueTarget)) continue;

					target.Add(keyTarget, valueTarget);
				}
			}
			else if (!isKeyPlainOld)
			{
				foreach (DictionaryEntry pair in source)
				{
					object keyTarget = null;
					if (!operation.HandleObject(pair.Key, ref keyTarget)) continue;

					target.Add(keyTarget, pair.Value);
				}
			}
			else if (!isValuePlainOld)
			{
				foreach (DictionaryEntry pair in source)
				{
					object valueTarget = null;
					if (!operation.HandleObject(pair.Value, ref valueTarget)) continue;

					target.Add(pair.Key, valueTarget);
				}
			}
			else
			{
				foreach (DictionaryEntry pair in source)
				{
					target.Add(pair.Key, pair.Value);
				}
			}
		}
	}
}
