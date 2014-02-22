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
	public class CreateRigidBodyJointAction : RigidBodyJointAction
	{
		private	RigidBody[]	targetParentObj	= null;

		protected override string NameBase
		{
			get { return EditorBaseRes.UndoRedo_CreateRigidBodyJoint; }
		}
		protected override string NameBaseMulti
		{
			get { return EditorBaseRes.UndoRedo_CreateRigidBodyJointMulti; }
		}
		public IEnumerable<JointInfo> Result
		{
			get { return this.targetObj; }
		}

		public CreateRigidBodyJointAction(IEnumerable<RigidBody> parent, IEnumerable<JointInfo> obj) : base(obj)
		{
			this.targetParentObj = parent.ToArray();
		}

		public override void Do()
		{
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				JointInfo joint = this.targetObj[i];
				RigidBody parent = this.targetParentObj[i];
				if (joint == null) continue;
				if (parent == null) continue;
				parent.AddJoint(joint);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj.Distinct()), ReflectionInfo.Property_RigidBody_Joints);
		}
		public override void Undo()
		{
			for (int i = 0; i < this.targetObj.Length; i++)
			{
				JointInfo joint = this.targetObj[i];
				RigidBody parent = this.targetParentObj[i];
				if (joint == null) continue;
				if (parent == null) continue;
				parent.RemoveJoint(joint);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj.Distinct()), ReflectionInfo.Property_RigidBody_Joints);
		}
	}
}
