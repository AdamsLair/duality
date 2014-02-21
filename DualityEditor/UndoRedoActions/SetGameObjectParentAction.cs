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
	public class SetGameObjectParentAction : GameObjectAction
	{
		private	GameObject[]	backupParentObj	= null;
		private	GameObject		targetValue		= null;

		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_SetGameObjectParent; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_SetGameObjectParentMulti; }
		}

		public SetGameObjectParentAction(GameObject value, IEnumerable<GameObject> obj) : base(obj)
		{
			this.targetValue = value;
		}

		public override void Do()
		{
			if (this.backupParentObj == null)
			{
				this.backupParentObj = new GameObject[this.targetObj.Length];
				for (int i = 0; i < this.backupParentObj.Length; i++)
					this.backupParentObj[i] = this.targetObj[i].Parent;
			}

			foreach (GameObject obj in this.targetObj)
			{
				obj.Parent = this.targetValue;
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj), ReflectionInfo.Property_GameObject_Parent);
		}
		public override void Undo()
		{
			if (this.backupParentObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = 0; i < this.backupParentObj.Length; i++)
			{
				this.targetObj[i].Parent = this.backupParentObj[i];
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj), ReflectionInfo.Property_GameObject_Parent);
		}
	}
}
