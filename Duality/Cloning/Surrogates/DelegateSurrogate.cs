using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class DelegateSurrogate : Surrogate<Delegate>
	{
		public override bool MatchesType(Type t)
		{
			return typeof(Delegate).IsAssignableFrom(t);
		}
		public override Delegate CreateTargetObject(CloneProvider provider)
		{
			#warning TODO CLONING

			Type delType = this.RealObject.GetType();
			Delegate[] baseInvokeList = this.RealObject.GetInvocationList();
			Delegate[] cloneInvokeList = new Delegate[baseInvokeList.Length];

			for (int i = 0; i < baseInvokeList.Length; i++)
			{
				// Shallow-copy the delegate first
				cloneInvokeList[i] = baseInvokeList[i].Clone() as Delegate;

				// Register the new delegate
				//provider.RegisterObjectClone(baseInvokeList[i], cloneInvokeList[i]);

				// Adjust target to reference a clone.
				if (cloneInvokeList[i] != null)
				{
					FieldInfo targetField = delType.GetField("_target", ReflectionHelper.BindInstanceAll);
					targetField.SetValue(cloneInvokeList[i], provider.CloneObject(cloneInvokeList[i].Target));
				}
			}

			Delegate result = Delegate.Combine(cloneInvokeList);
			//provider.RegisterObjectClone(this.RealObject, result);
			return result;
		}
		public override void CopyDataTo(Delegate targetObj, CloneProvider provider)
		{
			return;
		}
	}
}
