namespace Duality.Cloning
{
	/// <summary>
	/// Provides a general interface for an object type that will provide and explicit method for cloning
	/// rather than falling back to automated cloning behavior.
	/// </summary>
	public interface ICloneExplicit
	{
		/// <summary>
		/// Performs the cloning setup step, in which all reference-type instances from the target object
		/// graph are generated. 
		/// 
		/// The purpose of this method is to help the cloning system walk the entire (relevant) object graph
		/// in order to determine which objects are referenced and which are owned / deep-cloned, as well as
		/// creating instances or re-using existing instances from the target graph.
		/// 
		/// Walking this object's part of the source object graph and mapping instances to their target object
		/// graph correspondents is done by using the <see cref="ICloneTargetSetup"/> interface methods for 
		/// handling object instances and struct values.
		/// </summary>
		/// <param name="target">
		/// The object instance from the target graph that corresponds to this object's instance in the source graph.
		/// When invoking this method, the target object will either have existed already, or been created by the
		/// cloning system.
		/// </param>
		/// <param name="setup">The setup environment for the cloning operation.</param>
		void SetupCloneTargets(object target, ICloneTargetSetup setup);
		/// <summary>
		/// Performs the cloning copy step, in which all data is copied from source instances to
		/// target instances. No new object instances should be created in this step, as object creation
		/// should be part of the setup step instead.
		/// </summary>
		/// <param name="target">
		/// The object instance from the target graph that corresponds to this object's instance in the source graph.
		/// When invoking this method, the target object will either have existed already, or been created by the
		/// cloning system.
		/// </param>
		/// <param name="operation"></param>
		void CopyDataTo(object target, ICloneOperation operation);
	}
}
