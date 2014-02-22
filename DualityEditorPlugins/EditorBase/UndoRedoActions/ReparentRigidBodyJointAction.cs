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
		private	RigidBody[]	targetParentObjA	= null;
		private	RigidBody[]	targetParentObjB	= null;
		private	RigidBody[]	backupParentObjA	= null;
		private	RigidBody[]	backupParentObjB	= null;

		protected override string NameBase
		{
			get { return EditorBaseRes.UndoRedo_ReparentRigidBodyJoint; }
		}
		protected override string NameBaseMulti
		{
			get { return EditorBaseRes.UndoRedo_ReparentRigidBodyJointMulti; }
		}

		public ReparentRigidBodyJointAction(IEnumerable<JointInfo> obj, IEnumerable<RigidBody> parentA, IEnumerable<RigidBody> parentB) : base(obj)
		{
			RigidBody[] parentAArray = parentA.ToArray();
			RigidBody[] parentBArray = parentB.ToArray();
			this.targetParentObjA = new RigidBody[this.targetObj.Length];
			this.targetParentObjB = new RigidBody[this.targetObj.Length];
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				this.targetParentObjA[i] = parentAArray[Math.Min(i, parentAArray.Length - 1)];
				this.targetParentObjB[i] = parentBArray[Math.Min(i, parentBArray.Length - 1)];
			}
		}

		public override void Do()
		{
			if (this.backupParentObjA == null)
			{
				this.backupParentObjA = new RigidBody[this.targetObj.Length];
				this.backupParentObjB = new RigidBody[this.targetObj.Length];
				for (int i = 0; i < this.backupParentObjA.Length; i++)
				{
					this.backupParentObjA[i] = this.targetObj[i].BodyA;
					this.backupParentObjB[i] = this.targetObj[i].BodyB;
				}
			}

			for (int i = 0; i < this.targetObj.Length; i++)
			{
				JointInfo joint = this.targetObj[i];
				RigidBody parentA = this.targetParentObjA[i];
				RigidBody parentB = this.targetParentObjB[i];
				if (joint == null) continue;

				if (parentA != null)
					parentA.AddJoint(joint, parentB);
				else if (parentB != null)
					parentB.AddJoint(joint, parentA);
			}

			var affectedBodies = this.targetParentObjA
				.Concat(this.targetParentObjB)
				.Concat(this.backupParentObjA)
				.Concat(this.backupParentObjB)
				.Distinct();
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(affectedBodies), ReflectionInfo.Property_RigidBody_Joints);
		}
		public override void Undo()
		{
			if (this.backupParentObjA == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				JointInfo joint = this.targetObj[i];
				RigidBody parentA = this.backupParentObjA[i];
				RigidBody parentB = this.backupParentObjB[i];
				if (joint == null) continue;

				if (parentA != null)
					parentA.AddJoint(joint, parentB);
				else if (parentB != null)
					parentB.AddJoint(joint, parentA);
			}
			var affectedBodies = this.targetParentObjA
				.Concat(this.targetParentObjB)
				.Concat(this.backupParentObjA)
				.Concat(this.backupParentObjB)
				.Distinct();
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(affectedBodies), ReflectionInfo.Property_RigidBody_Joints);
		}
	}
}
