using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class DictionarySurrogate : Surrogate<IDictionary>
	{
		private static readonly MethodInfo setupMethodTemplate = typeof(DictionarySurrogate).GetMethod("SetupCloneTargetsGeneric", ReflectionHelper.BindInstanceAll);
		private static readonly MethodInfo copyMethodTemplate = typeof(DictionarySurrogate).GetMethod("CopyDataGeneric", ReflectionHelper.BindInstanceAll);

		public override bool MatchesType(Type t)
		{
			return typeof(IDictionary).IsAssignableFrom(t);
		}

		public override void SetupCloneTargets(IDictionary source, ICloneTargetSetup setup)
		{
			Type dictType = source.GetType();
			Type[] genArgs = dictType.GetGenericArguments();
			MethodInfo cast = setupMethodTemplate.MakeGenericMethod(genArgs);
			cast.Invoke(this, new object[] { source, setup });
		}
		public override void CopyDataTo(IDictionary source, IDictionary target, ICloneOperation operation)
		{
			Type dictType = source.GetType();
			Type[] genArgs = dictType.GetGenericArguments();
			MethodInfo cast = copyMethodTemplate.MakeGenericMethod(genArgs);
			cast.Invoke(this, new object[] { source, target, operation });
		}

		private void SetupCloneTargetsGeneric<T,U>(IDictionary<T,U> source, ICloneTargetSetup setup)
		{
			// Handle all keys and values
			if (!typeof(T).IsPlainOldData())
			{
				foreach (var key in source.Keys)
					setup.AutoHandleObject(key);
			}
			if (!typeof(U).IsPlainOldData())
			{
				foreach (var val in source.Values)
					setup.AutoHandleObject(val);
			}
		}
		private void CopyDataGeneric<T,U>(IDictionary<T,U> source, IDictionary<T,U> target, ICloneOperation operation)
		{
			target.Clear();

			// Determine unwrapping behavior to provide faster / more optimized loops.
			bool isKeyPlainOld = typeof(T).IsPlainOldData();
			bool isValuePlainOld = typeof(U).IsPlainOldData();

			// Copy all pairs. Don't check each pair, if the Type won't be unwrapped anyway.
			if (!isKeyPlainOld && !isValuePlainOld)
			{
				foreach (var pair in source)
				{
					object keyTarget;
					object valueTarget;
					if (!operation.AutoHandleObject(pair.Key, out keyTarget)) continue;
					if (!operation.AutoHandleObject(pair.Value, out valueTarget)) continue;

					target.Add((T)keyTarget, (U)valueTarget);
				}
			}
			else if (!isKeyPlainOld)
			{
				foreach (var pair in source)
				{
					object keyTarget;
					if (!operation.AutoHandleObject(pair.Key, out keyTarget)) continue;

					target.Add((T)keyTarget, pair.Value);
				}
			}
			else if (!isValuePlainOld)
			{
				foreach (var pair in source)
				{
					object valueTarget;
					if (!operation.AutoHandleObject(pair.Value, out valueTarget)) continue;

					target.Add(pair.Key, (U)valueTarget);
				}
			}
			else
			{
				foreach (var pair in source)
				{
					target.Add(pair.Key, pair.Value);
				}
			}
		}
	}
}
