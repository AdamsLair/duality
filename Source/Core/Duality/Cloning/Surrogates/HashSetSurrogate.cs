using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class HashSetSurrogate : CloneSurrogate<IEnumerable>
	{
		public Dictionary<Type, ReflectedHashsetData> ReflectedData = new Dictionary<Type, ReflectedHashsetData>();
		public override bool MatchesType(TypeInfo t)
		{
			return
				t.IsGenericType &&
				t.GetGenericTypeDefinition() == typeof(HashSet<>);
		}

		public ReflectedHashsetData GetReflectedHashsetData(Type hashSetType)
		{			
			ReflectedHashsetData reflectedHashsetData;
			if (!this.ReflectedData.TryGetValue(hashSetType, out reflectedHashsetData))
			{
				reflectedHashsetData = new ReflectedHashsetData(hashSetType);
				this.ReflectedData.Add(hashSetType, reflectedHashsetData);
			}

			return reflectedHashsetData;
		}

		public override void SetupCloneTargets(IEnumerable source, IEnumerable target, ICloneTargetSetup setup)
		{
			ReflectedHashsetData reflectedHashsetData = GetReflectedHashsetData(source.GetType());
			if (!reflectedHashsetData.IsPlainOldData)
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
			ReflectedHashsetData reflectedHashsetData = GetReflectedHashsetData(source.GetType());

			// Copy all values.
			reflectedHashsetData.ClearMethod.Invoke(target, new object[] { });

			var parameterBuffer = new object[1];
			// Determine unwrapping behavior to provide faster / more optimized loops.
			if (!reflectedHashsetData.IsPlainOldData)
			{
				foreach (object value in source)
				{
					object handledObject = null;
					if (!operation.HandleObject(value, ref handledObject)) continue;
					parameterBuffer[0] = handledObject;
					reflectedHashsetData.AddMethod.Invoke(target, parameterBuffer);
				}
			}
			else
			{
				foreach (object value in source)
				{
					parameterBuffer[0] = value;
					reflectedHashsetData.AddMethod.Invoke(target, parameterBuffer);
				}
			}
		}

		public class ReflectedHashsetData
		{
			public MethodInfo ClearMethod { get; private set; }
			public MethodInfo AddMethod { get; private set; }
			public bool IsPlainOldData { get; private set; }

			public ReflectedHashsetData(Type hashSetType)
			{
				Type[] genArgs = hashSetType.GenericTypeArguments;
				this.ClearMethod = hashSetType.GetRuntimeMethod("Clear", new Type[] { });
				this.AddMethod = hashSetType.GetRuntimeMethod("Add", genArgs);
				this.IsPlainOldData = genArgs[0].GetTypeInfo().IsPlainOldData();
			}
		}
	}
}
