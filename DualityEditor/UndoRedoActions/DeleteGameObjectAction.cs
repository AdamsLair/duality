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
	public class DeleteGameObjectAction : GameObjectAction
	{
		private	GameObject[]	backupParentObj	= null;
		private	GameObject[]	backupObj		= null;

		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_DeleteGameObject; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_DeleteGameObjectMulti; }
		}

		public DeleteGameObjectAction(IEnumerable<GameObject> obj) : base(obj) {}

		public override void Do()
		{
			if (this.backupObj == null)
			{
				this.backupObj = new GameObject[this.targetObj.Length];
				this.backupParentObj = new GameObject[this.targetObj.Length];
				for (int i = 0; i < this.backupObj.Length; i++)
				{
					this.backupObj[i] = CloneProvider.DeepClone(this.targetObj[i], BackupCloneContext);
					this.backupParentObj[i] = this.targetObj[i].Parent;
				}
			}

			Scene.Current.RemoveObject(this.targetObj);
			foreach (GameObject obj in this.targetObj)
				obj.Dispose();

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
		public override void Undo()
		{
			if (this.backupObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = 0; i < this.backupObj.Length; i++)
			{
				CloneProvider.DeepCopyTo(this.backupObj[i], this.targetObj[i], BackupCloneContext);
				Scene.Current.AddObject(this.targetObj[i]);
				this.targetObj[i].Parent = this.backupParentObj[i];
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
	}
}
