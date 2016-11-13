using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Components.Physics;

using Duality.Editor;

using Duality.Editor.Plugins.Base.Properties;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class ReparentRigidBodyJointAction : RigidBodyJointAction
	{
		private	RigidBody[]	targetParentBody	= null;
		private	RigidBody[]	targetOtherBody		= null;
		private	RigidBody[]	backupParentBody	= null;
		private	RigidBody[]	backupOtherBody		= null;

		protected override string NameBase
		{
			get { return EditorBaseRes.UndoRedo_ReparentRigidBodyJoint; }
		}
		protected override string NameBaseMulti
		{
			get { return EditorBaseRes.UndoRedo_ReparentRigidBodyJointMulti; }
		}

		public ReparentRigidBodyJointAction(IEnumerable<JointInfo> joints, IEnumerable<RigidBody> parentBodies, IEnumerable<RigidBody> otherBodies) : base(joints)
		{
			RigidBody[] parentBodyArray = parentBodies.ToArray();
			RigidBody[] otherBodyArray = otherBodies.ToArray();
			this.targetParentBody = new RigidBody[this.targetObj.Length];
			this.targetOtherBody = new RigidBody[this.targetObj.Length];
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				this.targetParentBody[i] = parentBodyArray[Math.Min(i, parentBodyArray.Length - 1)];
				this.targetOtherBody[i] = otherBodyArray[Math.Min(i, otherBodyArray.Length - 1)];
			}
		}

		public override void Do()
		{
			if (this.backupParentBody == null)
			{
				this.backupParentBody = new RigidBody[this.targetObj.Length];
				this.backupOtherBody = new RigidBody[this.targetObj.Length];
				for (int i = 0; i < this.backupParentBody.Length; i++)
				{
					this.backupParentBody[i] = this.targetObj[i].ParentBody;
					this.backupOtherBody[i] = this.targetObj[i].OtherBody;
				}
			}

			for (int i = 0; i < this.targetObj.Length; i++)
			{
				JointInfo joint = this.targetObj[i];
				RigidBody parentBody = this.targetParentBody[i];
				RigidBody otherBody = this.targetOtherBody[i];
				if (joint == null) continue;

				if (parentBody != null)
					parentBody.AddJoint(joint, otherBody);
				else if (otherBody != null)
					otherBody.AddJoint(joint, parentBody);
			}

			var affectedBodies = this.targetParentBody
				.Concat(this.targetOtherBody)
				.Concat(this.backupParentBody)
				.Concat(this.backupOtherBody)
				.Distinct();
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(affectedBodies), ReflectionInfo.Property_RigidBody_Joints);
		}
		public override void Undo()
		{
			if (this.backupParentBody == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				JointInfo joint = this.targetObj[i];
				RigidBody parentA = this.backupParentBody[i];
				RigidBody parentB = this.backupOtherBody[i];
				if (joint == null) continue;

				if (parentA != null)
					parentA.AddJoint(joint, parentB);
				else if (parentB != null)
					parentB.AddJoint(joint, parentA);
			}
			var affectedBodies = this.targetParentBody
				.Concat(this.targetOtherBody)
				.Concat(this.backupParentBody)
				.Concat(this.backupOtherBody)
				.Distinct();
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(affectedBodies), ReflectionInfo.Property_RigidBody_Joints);
		}
	}
}
