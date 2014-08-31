namespace Duality.Cloning
{
	/// <summary>
	/// Provides a general interface for an object type that will provide and explicit method for cloning
	/// rather than falling back to automated cloning behavior.
	/// </summary>
	public interface ICloneExplicit
	{
		/// <summary>
		/// Performs the cloning setup step. In this step, all reference-type instances from the target object
		/// graph are generated. Not specifying a target instance for a source instance will result in
		/// handling it as a reference or weak reference, depending on the concrete case.
		/// </summary>
		/// <param name="setup">The setup environment for the cloning operation.</param>
		void SetupCloneTargets(ICloneTargetSetup setup);
		/// <summary>
		/// Performs the cloning copy step. In this step, all data is copied from source instances to
		/// target instances. No reference-type object instances should be created in this step, as this
		/// should be part of the setup step.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="operation"></param>
		void CopyDataTo(object target, ICloneOperation operation);
	}
}
