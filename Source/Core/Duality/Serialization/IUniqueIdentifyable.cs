namespace Duality.Serialization
{
	/// <summary>
	/// An object that implements this interface is able to choose its own, fixed object id during serialization.
	/// This can under some circumstances help to minimize versioning conflicts due to a modified object id naming scheme.
	/// </summary>
	public interface IUniqueIdentifyable
	{
		/// <summary>
		/// [GET] The object id that will be picked preferrably for this object.
		/// </summary>
		uint PreferredId { get; }
	}
}
