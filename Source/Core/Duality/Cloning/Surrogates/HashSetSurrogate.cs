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

			// Determine unwrapping behavior to provide faster / more optimized loops.
			bool isValuePlainOld = firstGenArgInfo.IsPlainOldData();

			dynamic dynamicHashset = (dynamic)target;
			// Copy all values.
			dynamicHashset.Clear();

			if (!isValuePlainOld)
			{
				foreach (object value in source)
				{
					dynamic dynamicValue = null;
					if (!operation.HandleObject(value, ref dynamicValue)) continue;

					dynamicHashset.Add(dynamicValue); //Have to use dynamic here to allow for runtime resolving of the Add method
				}
			}
			else
			{
				foreach (dynamic dynamicValue in source)
				{
					dynamicHashset.Add(dynamicValue); //Have to use dynamic here to allow for runtime resolving of the Add method
				}
			}
		}
	}
}
