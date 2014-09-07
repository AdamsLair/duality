namespace Duality.Cloning
{
	public interface ICloneTargetSetup
	{
		CloneProviderContext Context { get; }

		void AddTarget(object source, object target);
		void HandleObject(object source, object target, CloneBehavior behavior);
	}

	public static class ExtMethodsICloneTargetSetup
	{
		public static void HandleObject<T>(this ICloneTargetSetup setup, T source) where T : class
		{
			setup.HandleObject(source, null, CloneBehavior.Default);
		}
		public static void HandleObject<T>(this ICloneTargetSetup setup, T source, CloneBehavior behavior) where T : class
		{
			setup.HandleObject(source, null, behavior);
		}
		public static void HandleObject<T>(this ICloneTargetSetup setup, T source, T target) where T : class
		{
			setup.HandleObject(source, target, CloneBehavior.Default);
		}
		public static void HandleObject<T>(this ICloneTargetSetup setup, T source, T target, CloneBehavior behavior) where T : class
		{
			setup.HandleObject(source, target, behavior);
		}
	}
}
