using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Components.Physics;

using Duality.Editor;

using Duality.Editor.Plugins.CamView.Properties;

namespace Duality.Editor.Plugins.CamView.UndoRedoActions
{
	public class DeleteRigidBodyShapeAction : RigidBodyShapeAction
	{
		private	RigidBody[]	backupParentObj	= null;

		protected override string NameBase
		{
			get { return CamViewRes.UndoRedo_DeleteRigidBodyShape; }
		}
		protected override string NameBaseMulti
		{
			get { return CamViewRes.UndoRedo_DeleteRigidBodyShapeMulti; }
		}

		public DeleteRigidBodyShapeAction(IEnumerable<ShapeInfo> obj) : base(obj) {}

		public override void Do()
		{
			if (this.backupParentObj == null)
			{
				this.backupParentObj = new RigidBody[this.targetObj.Length];
				for (int i = 0; i < this.backupParentObj.Length; i++)
					this.backupParentObj[i] = this.targetObj[i].Parent;
			}
			
			foreach (ShapeInfo obj in this.targetObj)
			{
				obj.Parent.RemoveShape(obj);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.backupParentObj.Distinct()), ReflectionInfo.Property_RigidBody_Shapes);
		}
		public override void Undo()
		{
			if (this.backupParentObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = 0; i < this.backupParentObj.Length; i++)
			{
				this.backupParentObj[i].AddShape(this.targetObj[i]);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.backupParentObj.Distinct()), ReflectionInfo.Property_RigidBody_Shapes);
		}
	}
}
