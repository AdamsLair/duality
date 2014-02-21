using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor.Properties;

namespace Duality.Editor.UndoRedoActions
{
	public class DeleteComponentAction : ComponentAction
	{
		private	GameObject[]	backupParentObj	= null;
		private Component[]		backupObj		= null;
		
		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_DeleteComponent; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_DeleteComponentMulti; }
		}

		public DeleteComponentAction(IEnumerable<Component> obj) : base(obj) {}

		public override void Do()
		{
			if (this.backupObj == null)
			{
				this.backupObj = new Component[this.targetObj.Count];
				this.backupParentObj = new GameObject[this.targetObj.Count];
				for (int i = 0; i < this.backupObj.Length; i++)
				{
					this.backupObj[i] = CloneProvider.DeepClone(this.targetObj[i], BackupCloneContext);
					this.backupParentObj[i] = this.targetObj[i].GameObj;
				}
			}

			foreach (Component obj in this.targetObj)
			{
				obj.Dispose();
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.backupParentObj));
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
		public override void Undo()
		{
			if (this.backupObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = this.backupObj.Length - 1; i >= 0; i--)
			{
				CloneProvider.DeepCopyTo(this.backupObj[i], this.targetObj[i], BackupCloneContext);
				this.targetObj[i].GameObj = this.backupParentObj[i];
				DebugCheckParent(this.targetObj[i], this.backupParentObj[i]);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.backupParentObj));
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
	}
}
