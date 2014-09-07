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
		public override bool MatchesType(Type t)
		{
			return typeof(IDictionary).IsAssignableFrom(t) && t.IsGenericType && t.GetGenericArguments().Length >= 2;
		}

		public override void SetupCloneTargets(IDictionary source, IDictionary target, ICloneTargetSetup setup)
		{
			Type dictType = source.GetType();
			Type[] genArgs = dictType.GetGenericArguments();

			// Handle all keys and values. Reuse existing objects wherever possible.
			if (!genArgs[0].IsPlainOldData())
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
			if (!genArgs[1].IsPlainOldData())
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
			Type[] genArgs = dictType.GetGenericArguments();

			// Determine unwrapping behavior to provide faster / more optimized loops.
			bool isKeyPlainOld = genArgs[0].IsPlainOldData();
			bool isValuePlainOld = genArgs[1].IsPlainOldData();

			// Copy all pairs. Don't check each pair, if the Type won't be unwrapped anyway.
			target.Clear();
			if (!isKeyPlainOld && !isValuePlainOld)
			{
				foreach (DictionaryEntry pair in source)
				{
					object keyTarget;
					object valueTarget;
					if (!operation.AutoHandleObject(pair.Key, out keyTarget)) continue;
					if (!operation.AutoHandleObject(pair.Value, out valueTarget)) continue;

					target.Add(keyTarget, valueTarget);
				}
			}
			else if (!isKeyPlainOld)
			{
				foreach (DictionaryEntry pair in source)
				{
					object keyTarget;
					if (!operation.AutoHandleObject(pair.Key, out keyTarget)) continue;

					target.Add(keyTarget, pair.Value);
				}
			}
			else if (!isValuePlainOld)
			{
				foreach (DictionaryEntry pair in source)
				{
					object valueTarget;
					if (!operation.AutoHandleObject(pair.Value, out valueTarget)) continue;

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
