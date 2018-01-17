using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class HashSetSurrogate : CloneSurrogate<IEnumerable>
	{
		public override bool MatchesType(TypeInfo t)
		{
			return
				t.IsGenericType &&
				t.GetGenericTypeDefinition() == typeof(HashSet<>);
		}

		public override void SetupCloneTargets(IEnumerable source, IEnumerable target, ICloneTargetSetup setup)
		{
			Type hashSetType = source.GetType();
			Type[] genArgs = hashSetType.GenericTypeArguments;
			TypeInfo firstGenArgInfo = genArgs[0].GetTypeInfo();

			if (!firstGenArgInfo.IsPlainOldData())
			{
				if (source == target)
				{
					foreach (object value in source)
						setup.HandleObject(value, value);
				}
				else
				{
					foreach (object value in source)
						setup.HandleObject(value, null);
				}
			}
		}
		public override void CopyDataTo(IEnumerable source, IEnumerable target, ICloneOperation operation)
		{
			Type hashSetType = source.GetType();
			Type[] genArgs = hashSetType.GenericTypeArguments;
			TypeInfo firstGenArgInfo = genArgs[0].GetTypeInfo();
			var clearMethodInfo = hashSetType.GetRuntimeMethod("Clear", new Type[] { });
			var addMethodInfo = hashSetType.GetRuntimeMethod("Add", genArgs);
			// Determine unwrapping behavior to provide faster / more optimized loops.
			bool isValuePlainOld = firstGenArgInfo.IsPlainOldData();

			// Copy all values.
			clearMethodInfo.Invoke(target, new object[]{});

			if (!isValuePlainOld)
			{
				foreach (object value in source)
				{
					object handledObject = null;
					if (!operation.HandleObject(value, ref handledObject)) continue;
					addMethodInfo.Invoke(target, new[] { handledObject } );
				}
			}
			else
			{
				foreach (object dynamicValue in source)
				{
					addMethodInfo.Invoke(target, new[] { dynamicValue });
				}
			}
		}
	}
}
