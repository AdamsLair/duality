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
	public class SetGameObjectNameAction : GameObjectAction
	{
		private	string[]	backupParentObj	= null;
		private	string		targetValue		= null;

		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_SetGameObjectName; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_SetGameObjectNameMulti; }
		}

		public SetGameObjectNameAction(string value, IEnumerable<GameObject> obj) : base(obj)
		{
			this.targetValue = value;
		}

		public override void Do()
		{
			if (this.backupParentObj == null)
			{
				this.backupParentObj = new string[this.targetObj.Length];
				for (int i = 0; i < this.backupParentObj.Length; i++)
					this.backupParentObj[i] = this.targetObj[i].Name;
			}

			foreach (GameObject obj in this.targetObj)
			{
				obj.Name = this.targetValue;
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj), ReflectionInfo.Property_GameObject_Name);
		}
		public override void Undo()
		{
			if (this.backupParentObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = 0; i < this.backupParentObj.Length; i++)
			{
				this.targetObj[i].Name = this.backupParentObj[i];
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj), ReflectionInfo.Property_GameObject_Name);
		}
	}
}
