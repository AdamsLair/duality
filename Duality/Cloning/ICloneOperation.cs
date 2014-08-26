using System;
using System.Reflection;

namespace Duality.Cloning
{
	public interface ICloneOperation
	{
		CloneProviderContext Context { get; }

		bool GetTarget(object source, out object target);

		object AutoHandleObject(object source);
		void AutoHandleObject(object source, object target);
		void AutoHandleField(object source, object target, FieldInfo field);
	}

	public static class ExtMethodsICloneOperation
	{
		public static bool GetTarget<T>(this ICloneOperation operation, T source, out T target) where T : class
		{
			object targetObj;
			if (!operation.GetTarget(source, out targetObj))
			{
				target = default(T);
				return false;
			}
			else
			{
				target = (T)targetObj;
				return true;
			}
		}
		public static T AutoHandleObject<T>(this ICloneOperation operation, T source)
		{
			return (T)operation.AutoHandleObject(source);
		}
	}
}
