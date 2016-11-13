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
	public class CreateRigidBodyShapeAction : RigidBodyShapeAction
	{
		private	RigidBody	targetParentObj	= null;

		protected override string NameBase
		{
			get { return CamViewRes.UndoRedo_CreateRigidBodyShape; }
		}
		protected override string NameBaseMulti
		{
			get { return CamViewRes.UndoRedo_CreateRigidBodyShapeMulti; }
		}
		public IEnumerable<ShapeInfo> Result
		{
			get { return this.targetObj; }
		}

		public CreateRigidBodyShapeAction(RigidBody parent, IEnumerable<ShapeInfo> obj) : base(obj)
		{
			this.targetParentObj = parent;
		}
		public CreateRigidBodyShapeAction(RigidBody parent, params ShapeInfo[] obj) : this(parent, obj as IEnumerable<ShapeInfo>) {}

		public override void Do()
		{
			foreach (ShapeInfo obj in this.targetObj)
			{
				this.targetParentObj.AddShape(obj);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj), ReflectionInfo.Property_RigidBody_Shapes);
		}
		public override void Undo()
		{
			foreach (ShapeInfo obj in this.targetObj)
			{
				this.targetParentObj.RemoveShape(obj);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj), ReflectionInfo.Property_RigidBody_Shapes);
		}
	}
}
