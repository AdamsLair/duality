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
			foreach (var pair in dict) targetObj.Add(pair.Key, pair.Value);
		}
	}
}
