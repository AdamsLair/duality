using System;
using System.Linq;

using OpenTK;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Connects two bodies using a gear. The gear type is determined by the joints that are attached to
	/// each body. Supported joint types are (Fixed)Prismatic- and (Fixed)Revolutejoints. Those joints
	/// are required to be either fixed or attached to a static body.
	/// </summary>
	[Serializable]
	public sealed class GearJointInfo : JointInfo
	{
		private	float	ratio	= 1.0f;


		public override bool DualJoint
		{
			get { return true; }
		}
		/// <summary>
		/// [GET / SET] The gear ratio by which body movement is connected.
		/// </summary>
		public float Ratio
		{
			get { return this.ratio; }
			set
			{
				this.ratio = value;
 				this.DestroyJoint();
				this.UpdateJoint();
			}
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			if (bodyA == null || bodyB == null) return null;

			RigidBody bodyCmpA = bodyA.UserData as RigidBody;
			RigidBody bodyCmpB = bodyB.UserData as RigidBody;
			if (bodyCmpA == null || bodyCmpB == null) return null;

			JointInfo jointA = bodyCmpA.Joints.FirstOrDefault(j => j is FixedPrismaticJointInfo || j is FixedRevoluteJointInfo || j is PrismaticJointInfo || j is RevoluteJointInfo);
			JointInfo jointB = bodyCmpB.Joints.FirstOrDefault(j => j is FixedPrismaticJointInfo || j is FixedRevoluteJointInfo || j is PrismaticJointInfo || j is RevoluteJointInfo);
			if (jointA == null || jointB == null) return null;

			if (jointA.joint == null) jointA.UpdateJoint();
			if (jointB.joint == null) jointB.UpdateJoint();
			if (jointA.joint == null || jointB.joint == null) return null;

			return JointFactory.CreateGearJoint(Scene.PhysicsWorld, jointA.joint, jointB.joint, this.ratio);
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			GearJoint j = this.joint as GearJoint;
			// j.Ratio = this.ratio; Can only be assigned during construction.
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			GearJointInfo c = target as GearJointInfo;
			c.ratio = this.ratio;
		}
	}
}
