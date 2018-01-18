using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			reflectedHashsetData.CloneMethod.Invoke(null, new object[] { source, target, operation });
		}

		public static void CopyDataTo_PlainOldData<TItem>(HashSet<TItem> source, HashSet<TItem> target, ICloneOperation operation)
		{
			target.Clear();
			foreach (var value in source)
			{
				target.Add(value);
			}
		}

		public static void CopyDataTo_NotPlainOldData<TItem>(HashSet<TItem> source, HashSet<TItem> target, ICloneOperation operation)
			where TItem : class
		{
			target.Clear();
			foreach (var value in source)
			{
				TItem handledObject = null;
				if (!operation.HandleObject(value, ref handledObject)) continue;
				target.Add(handledObject);
			}
		}

		public class ReflectedHashsetData
		{
			public bool IsPlainOldData { get; private set; }
			public MethodInfo CloneMethod { get; private set; }

			public Delegate CloneDelegate { get; private set; }

			public ReflectedHashsetData(Type hashSetType)
			{
				this.IsPlainOldData = hashSetType.GenericTypeArguments[0].GetTypeInfo().IsPlainOldData();
				string methodName = this.IsPlainOldData ? "CopyDataTo_PlainOldData" : "CopyDataTo_NotPlainOldData";

				MethodInfo method = (from m in typeof(HashSetSurrogate).GetRuntimeMethods()
									 where m.Name == methodName
									 select m).First();

				this.CloneMethod = method.MakeGenericMethod(hashSetType.GenericTypeArguments);
				//this.CloneDelegate = this.CloneMethod.CreateDelegate(typeof(Action<object, object, object>));
			}
		}
	}
}
