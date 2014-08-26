namespace Duality.Cloning
{
	public interface ICloneTargetSetup
	{
		CloneProviderContext Context { get; }

		bool AddTarget(object source, object target);

		void AutoHandleObject(object source);
	}
}
