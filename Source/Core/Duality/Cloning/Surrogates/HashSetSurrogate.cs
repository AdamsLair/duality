using System;
using System.Collections.Generic;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class HashSetSurrogate : CloneSurrogate<dynamic>
	{
		public override bool MatchesType(TypeInfo t)
		{
			return
				t.IsGenericType &&
				t.GetGenericTypeDefinition() == typeof(HashSet<>);
		}

		public override void SetupCloneTargets(dynamic source, dynamic target, ICloneTargetSetup setup)
		{
			Type hashSetType = source.GetType();
			Type[] genArgs = hashSetType.GenericTypeArguments;
			TypeInfo firstGenArgInfo = genArgs[0].GetTypeInfo();

			if (!firstGenArgInfo.IsPlainOldData())
			{
				if (source == target)
				{
					foreach (dynamic key in source)
						setup.HandleObject(key, key);
				}
				else
				{
					foreach (dynamic key in source)
						setup.HandleObject(key, null);
				}
			}
		}
		public override void CopyDataTo(dynamic source, dynamic target, ICloneOperation operation)
		{
			Type hashSetType = source.GetType();
			Type[] genArgs = hashSetType.GenericTypeArguments;
			TypeInfo firstGenArgInfo = genArgs[0].GetTypeInfo();

			// Determine unwrapping behavior to provide faster / more optimized loops.
			bool isKeyPlainOld = firstGenArgInfo.IsPlainOldData();

			// Copy all values.
			target.Clear();

			if (!isKeyPlainOld)
			{
				foreach (dynamic key in source)
				{
					dynamic keyTarget = null;
					if (!operation.HandleObject(key, ref keyTarget)) continue;

					target.Add(keyTarget);
				}
			}
			else
			{
				foreach (dynamic key in source)
				{
					target.Add(key);
				}
			}
		}
	}
}
