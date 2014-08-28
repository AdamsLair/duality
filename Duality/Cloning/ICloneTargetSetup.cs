namespace Duality.Cloning
{
	public interface ICloneTargetSetup
	{
		CloneProviderContext Context { get; }

		bool AddTarget<T>(T source, T target) where T : class;
		void MakeWeakReference(object source);
		void AutoHandleObject(object source);
	}
}
