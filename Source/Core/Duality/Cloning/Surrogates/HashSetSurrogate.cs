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
			public bool IsDeepCopyByAssignment { get; private set; }
			public MethodInfo CopyDataToMethod { get; private set; }

			public ReflectedHashsetData(Type hashSetType)
			{
				this.IsDeepCopyByAssignment = hashSetType.GenericTypeArguments[0].GetTypeInfo().IsDeepCopyByAssignment();
				string methodName = this.IsDeepCopyByAssignment ? "CopyDataTo_DeepCopyByAssignment" : "CopyDataTo_DeepCopyByTraversal"; // Replace hardcoded strings with nameof in C#6
				MethodInfo method = typeof(HashSetSurrogate).GetRuntimeMethods().First(m => m.Name == methodName);
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
			if (!reflectedHashsetData.IsDeepCopyByAssignment)
			{
				if (source == target)
				{
					foreach (object value in source)
						setup.HandleObject(value, value);
				}
				else
				{
					// In a hashset, there is no defined item identity, so we cannot find an equivalent 
					// between a source item and a target item. Map the source to null, so a new target will
					// be created during the clone operation.
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

		private static void CopyDataTo_DeepCopyByAssignment<TItem>(HashSet<TItem> source, HashSet<TItem> target, ICloneOperation operation)
		{
			target.Clear();
			foreach (TItem value in source)
			{
				target.Add(value);
			}
		}
		private static void CopyDataTo_DeepCopyByTraversal<TItem>(HashSet<TItem> source, HashSet<TItem> target, ICloneOperation operation) where TItem : class
		{
			target.Clear();
			foreach (TItem value in source)
			{
				TItem handledObject = null;
				operation.HandleObject(value, ref handledObject);
				target.Add(handledObject);
			}
		}
	}
}
