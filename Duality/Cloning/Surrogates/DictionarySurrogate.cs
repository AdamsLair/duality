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
		private static readonly MethodInfo copyMethodTemplate = typeof(DictionarySurrogate).GetMethod("CopyDataSpecific", ReflectionHelper.BindInstanceAll);

		public override bool MatchesType(Type t)
		{
			return typeof(IDictionary).IsAssignableFrom(t);
		}
		public override void CopyDataTo(IDictionary targetObj, CloneProvider provider)
		{
			Type dictType = this.RealObject.GetType();
			Type[] genArgs = dictType.GetGenericArguments();
			MethodInfo cast = copyMethodTemplate.MakeGenericMethod(genArgs);
			cast.Invoke(this, new object[] { targetObj, provider });
		}

		private void CopyDataSpecific<T,U>(IDictionary<T,U> targetObj, CloneProvider provider)
		{
			IDictionary<T,U> dict = this.RealObject as IDictionary<T,U>;
			targetObj.Clear();

			// Determine unwrapping behavior to provide faster / more optimized loops.
			bool isReferenceTypeKey = !typeof(T).IsDeepByValueType();
			bool isReferenceTypeValue = !typeof(U).IsDeepByValueType();

			// Copy all pairs. Don't check each pair, if the Type won't be unwrapped anyway.
			if (isReferenceTypeKey && isReferenceTypeValue)
			{
				foreach (var pair in dict)
					targetObj.Add(provider.RequestObjectClone(pair.Key), provider.RequestObjectClone(pair.Value));
			}
			else if (isReferenceTypeKey)
			{
				foreach (var pair in dict)
					targetObj.Add(provider.RequestObjectClone(pair.Key), pair.Value);
			}
			else if (isReferenceTypeValue)
			{
				foreach (var pair in dict)
					targetObj.Add(pair.Key, provider.RequestObjectClone(pair.Value));
			}
			else
			{
				foreach (var pair in dict)
					targetObj.Add(pair.Key, pair.Value);
			}
		}
	}
}
