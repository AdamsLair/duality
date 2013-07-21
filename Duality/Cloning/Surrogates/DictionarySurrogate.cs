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
		public override bool MatchesType(Type t)
		{
			return typeof(IDictionary).IsAssignableFrom(t);
		}
		public override void CopyDataTo(IDictionary targetObj, CloneProvider provider)
		{
			Type dictType = this.RealObject.GetType();
			Type[] genArgs = dictType.GetGenericArguments();
			MethodInfo cast = typeof(DictionarySurrogate).GetMethod("CopyDataSpecific", ReflectionHelper.BindInstanceAll).MakeGenericMethod(genArgs);
			cast.Invoke(this, new object[] { targetObj, provider });
		}

		private void CopyDataSpecific<T,U>(Dictionary<T,U> targetObj, CloneProvider provider)
		{
			Dictionary<T,U> dict = this.RealObject as Dictionary<T,U>;
			targetObj.Clear();
			foreach (var pair in dict) targetObj.Add(pair.Key, pair.Value);
		}
	}
}
