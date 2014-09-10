using System;
using System.Reflection;

namespace Duality.Cloning
{
	public interface ICloneOperation
	{
		CloneProviderContext Context { get; }

		bool GetTarget<T>(T source, ref T target) where T : class;
		bool IsTarget<T>(T target) where T : class;

		bool HandleObject<T>(T source, ref T target);
	}

	public static class ExtMethodsICloneOperation
	{
		public static bool HandleObject<T>(this ICloneOperation operation, T source, T target = null) where T : class
		{
			T targetObj = target;
			return operation.HandleObject(source, ref targetObj);
		}
		public static bool HandleObject<T>(this ICloneOperation operation, T source, ref T target, bool dontNullifyExternal) where T : class
		{
			if (object.ReferenceEquals(source, null) && dontNullifyExternal && !operation.IsTarget(target))
				return false;

			return operation.HandleObject(source, ref target);
		}
	}
}
