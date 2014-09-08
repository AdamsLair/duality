using System;
using System.Reflection;

namespace Duality.Cloning
{
	public interface ICloneOperation
	{
		CloneProviderContext Context { get; }

		bool GetTarget<T>(T source, out T target) where T : class;
		bool IsTarget<T>(T target) where T : class;

		bool HandleObject(object source, ref object target);
	}

	public static class ExtMethodsICloneOperation
	{
		public static bool HandleObject<T>(this ICloneOperation operation, T source, T target) where T : class
		{
			T targetObj = target;
			return operation.HandleObject(source, ref targetObj);
		}
		public static bool HandleObject<T>(this ICloneOperation operation, T source, ref T target) where T : class
		{
			object targetObj = target;
			bool result = operation.HandleObject((object)source, ref targetObj);
			if (result) target = targetObj as T;
			return result;
		}
		public static bool HandleObject(this ICloneOperation operation, object source)
		{
			object targetObj = null;
			return operation.HandleObject(source, ref targetObj);
		}
	}
}
