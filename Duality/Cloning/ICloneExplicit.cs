namespace Duality.Cloning
{
	/// <summary>
	/// Provides a general interface for an object type that will provide and explicit method for cloning
	/// rather than falling back to automated cloning behavior.
	/// </summary>
	public interface ICloneExplicit
	{
		void CopyDataTo(object targetObj, CloneProvider provider);
	}
}
