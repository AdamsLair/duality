namespace Duality.Cloning
{
	/// <summary>
	/// Provides a general interface for an object type with custom cloning.
	/// </summary>
	public interface ICloneable
	{
		void CopyDataTo(object targetObj, CloneProvider provider);
	}
}
