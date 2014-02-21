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
	public class SetComponentParentAction : ComponentAction
	{
		private	GameObject[]	backupParentObj	= null;
		private	GameObject		targetValue		= null;
		
		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_SetComponentParent; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_SetComponentParentMulti; }
		}

		public SetComponentParentAction(GameObject value, IEnumerable<Component> obj)
			: base(obj
			.Where(c => c != null)
			.Distinct(ComponentTypeComparer.Default)
			.OrderBy(c => c.GetRequiredComponents().Count()))
		{
			this.targetValue = value;
		}

		public override void Do()
		{
			if (this.backupParentObj == null)
			{
				this.backupParentObj = new GameObject[this.targetObj.Count];
				for (int i = 0; i < this.backupParentObj.Length; i++)
					this.backupParentObj[i] = this.targetObj[i].GameObj;
			}
			
			for (int i = this.targetObj.Count - 1; i >= 0; i--)
				this.targetObj[i].GameObj = null;
			for (int i = 0; i < this.targetObj.Count; i++)
			{
				this.targetObj[i].GameObj = this.targetValue;
				DebugCheckParent(this.targetObj[i], this.targetValue);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj), ReflectionInfo.Property_Component_GameObj);
		}
		public override void Undo()
		{
			if (this.backupParentObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = this.backupParentObj.Length - 1; i >= 0; i--)
			{
				this.targetObj[i].GameObj = this.backupParentObj[i];
				DebugCheckParent(this.targetObj[i], this.backupParentObj[i]);
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetObj), ReflectionInfo.Property_Component_GameObj);
		}
	}
}
