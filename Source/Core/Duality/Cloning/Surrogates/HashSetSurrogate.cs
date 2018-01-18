using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class HashSetSurrogate : CloneSurrogate<IEnumerable>
	{
		private class ReflectedHashsetData
		{
			public bool IsPlainOldData { get; private set; }
			public MethodInfo CopyDataToMethod { get; private set; }

			public ReflectedHashsetData(Type hashSetType)
			{
				this.IsPlainOldData = hashSetType.GenericTypeArguments[0].GetTypeInfo().IsPlainOldData();
				string methodName = this.IsPlainOldData ? "CopyDataTo_PlainOldData" : "CopyDataTo_NotPlainOldData";
				MethodInfo method = typeof(HashSetSurrogate).GetRuntimeMethods().FirstOrDefault(m => m.Name == methodName);
				this.CopyDataToMethod = method.MakeGenericMethod(hashSetType.GenericTypeArguments);
			}
		}

		private readonly Dictionary<Type, ReflectedHashsetData> _reflectedData = new Dictionary<Type, ReflectedHashsetData>();
		public override bool MatchesType(TypeInfo t)
		{
			return
				t.IsGenericType &&
				t.GetGenericTypeDefinition() == typeof(HashSet<>);
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
			reflectedHashsetData.CopyDataToMethod.Invoke(null, new object[] { source, target, operation });
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

		private static void CopyDataTo_PlainOldData<TItem>(HashSet<TItem> source, HashSet<TItem> target, ICloneOperation operation)
		{
			target.Clear();
			foreach (TItem value in source)
			{
				target.Add(value);
			}
		}

		private static void CopyDataTo_NotPlainOldData<TItem>(HashSet<TItem> source, HashSet<TItem> target, ICloneOperation operation)
			where TItem : class
		{
			target.Clear();
			foreach (TItem value in source)
			{
				TItem handledObject = null;
				if (!operation.HandleObject(value, ref handledObject)) continue;
				target.Add(handledObject);
			}
		}
	}
}
