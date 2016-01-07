using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;

using Duality.Editor.Properties;

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
				this.backupObj = this.targetObj.Select(o => o.DeepClone(BackupCloneContext)).ToList();
			}
			else
			{
				for (int i = 0; i < this.backupObj.Count; i++)
					this.backupObj[i].DeepCopyTo(this.targetObj[i], BackupCloneContext);
			}

			for (int i = 0; i < this.targetObj.Count; i++)
			{
				Component obj = this.targetObj[i];

				foreach (Type required in obj.GetRequiredComponents().Reverse())
				{
					if (this.targetParentObj.GetComponent(required) != null) continue;
					obj = required.GetTypeInfo().CreateInstanceOf() as Component;
					this.backupObj.Insert(i, obj.DeepClone(BackupCloneContext));
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

			//By this point, all Component dependencies are satisfied, so we can initialize the Components if needed
			foreach (Component t in this.targetObj)
			{
				InitializeComponent(t);
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
	        private bool InitializeComponent(Component cmp)
	        {
			IEditorAction initializer = this.GetInitializeComponentAction(cmp);
			if (initializer == null) return false;
			
			initializer.Perform(cmp);
			return true;
		}
		private IEditorAction GetInitializeComponentAction(Component cmp)
		{
			if (cmp == null) return null;
			
			var actions = DualityEditorApp.GetEditorActions(cmp.GetType(), new[] { cmp },
				DualityEditorApp.ActionContextInitializeComponent);
			
			return actions == null ? null : actions.FirstOrDefault();
		}
	}
}
