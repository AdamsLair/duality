using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor.Properties;

using OpenTK;

namespace Duality.Editor.UndoRedoActions
{
	public class CreateComponentAction : ComponentAction
	{
		private List<Component>	backupObj		= null;
		private	GameObject		targetParentObj	= null;

		protected override string NameBase
		{
			get { return GeneralRes.UndoRedo_CreateComponent; }
		}
		protected override string NameBaseMulti
		{
			get { return GeneralRes.UndoRedo_CreateComponentMulti; }
		}
		public IEnumerable<Component> Result
		{
			get { return this.targetObj; }
		}

		public CreateComponentAction(GameObject parent, IEnumerable<Component> obj)
			: base(obj
			.Where(c => c != null && c.GameObj == null)
			.Distinct(ComponentTypeComparer.Default)
			.OrderBy(c => c.GetRequiredComponents().Count()))
		{
			if (parent == null) throw new ArgumentNullException("parent");
			this.targetParentObj = parent;
		}
		public CreateComponentAction(GameObject parent, params Component[] obj) : this(parent, obj as IEnumerable<Component>) {}

		public override void Do()
		{
			if (this.backupObj == null)
			{
				this.backupObj = this.targetObj.Select(o => CloneProvider.DeepClone(o, BackupCloneContext)).ToList();
			}
			else
			{
				for (int i = 0; i < this.backupObj.Count; i++)
					CloneProvider.DeepCopyTo(this.backupObj[i], this.targetObj[i], BackupCloneContext);
			}

			for (int i = 0; i < this.targetObj.Count; i++)
			{
				Component obj = this.targetObj[i];

				foreach (Type required in obj.GetRequiredComponents().Reverse())
				{
					if (this.targetParentObj.GetComponent(required) != null) continue;
					obj = required.CreateInstanceOf() as Component;
					this.backupObj.Insert(i, CloneProvider.DeepClone(obj, BackupCloneContext));
					this.targetObj.Insert(i, obj);
				}

				obj.GameObj = this.targetParentObj;
				DebugCheckParent(obj, this.targetParentObj);

				if (obj.GameObj == null)
				{
					this.backupObj.RemoveAt(i);
					this.targetObj.RemoveAt(i);
					i--;
				}
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj));
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
		public override void Undo()
		{
			if (this.backupObj == null) throw new InvalidOperationException("Can't undo what hasn't been done yet");
			for (int i = this.targetObj.Count - 1; i >= 0; i--)
			{
				Component obj = this.targetObj[i];
				obj.Dispose();
			}
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetParentObj));
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(Scene.Current));
		}
	}
}
