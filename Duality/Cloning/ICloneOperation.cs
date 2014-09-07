using System;
using System.Reflection;

namespace Duality.Cloning
{
	public interface ICloneOperation
	{
		CloneProviderContext Context { get; }

		bool GetTarget<T>(T source, out T target) where T : class;

		void HandleObject(object source, object target);
	}

	public static class ExtMethodsICloneOperation
	{
		public static bool HandleObject<T>(this ICloneOperation operation, T source, out T target) where T : class
		{
			if (operation.GetTarget(source, out target))
			{
				operation.HandleObject(source, target);
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool HandleObject(this ICloneOperation operation, object source)
		{
			object target;
			if (operation.GetTarget(source, out target))
			{
				operation.HandleObject(source, target);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
