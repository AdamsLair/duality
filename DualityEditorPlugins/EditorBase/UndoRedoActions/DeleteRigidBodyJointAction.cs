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
	public class DeleteRigidBodyJointAction : RigidBodyJointAction
	{
		private	RigidBody[]	backupParentObjA	= null;
		private	RigidBody[]	backupParentObjB	= null;

		protected override string NameBase
		{
			get { return EditorBaseRes.UndoRedo_DeleteRigidBodyJoint; }
		}
		protected override string NameBaseMulti
		{
			get { return EditorBaseRes.UndoRedo_DeleteRigidBodyJointMulti; }
		}

		public DeleteRigidBodyJointAction(IEnumerable<JointInfo> obj) : base(obj) {}

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
			
			foreach (JointInfo obj in this.targetObj)
			{
				if (obj.BodyA != null)
					obj.BodyA.RemoveJoint(obj);
				if (obj.BodyB != null)
					obj.BodyB.RemoveJoint(obj);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.backupParentObjA.Concat(this.backupParentObjB).Distinct()), ReflectionInfo.Property_RigidBody_Joints);
		}
		public override void Undo()
		{
			if (this.backupParentObjA == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = 0; i < this.backupParentObjA.Length; i++)
			{
				RigidBody bodyA = this.backupParentObjA[i];
				RigidBody bodyB = this.backupParentObjB[i];
				if (bodyA != null)
					bodyA.AddJoint(this.targetObj[i], bodyB);
				else if (bodyB != null)
					bodyB.AddJoint(this.targetObj[i], bodyA);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.backupParentObjA.Concat(this.backupParentObjB).Distinct()), ReflectionInfo.Property_RigidBody_Joints);
		}
	}
}
