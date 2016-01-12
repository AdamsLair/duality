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
		private List<Component> backupObj        = null;
		private GameObject      targetParentObj  = null;


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


		public CreateComponentAction(GameObject parent, Type componentType) : this(parent, new[] { PrepareNewUserEditComponent(componentType) }) { }
		public CreateComponentAction(GameObject parent, IEnumerable<Component> cmp) : base(FilterPreparedComponents(cmp))
		{
			if (parent == null) throw new ArgumentNullException("parent");
			this.targetParentObj = parent;
		}

		public override void Do()
		{
			// Before performing the action, create a backup of the Components if not done yet
			if (this.backupObj == null)
			{
				this.backupObj = this.targetObj.Select(o => o.DeepClone(BackupCloneContext)).ToList();
			}
			// Otherwise, apply the backup to revert the cached target Components to their original state
			else
			{
				for (int i = 0; i < this.backupObj.Count; i++)
					this.backupObj[i].DeepCopyTo(this.targetObj[i], BackupCloneContext);
			}

			// Make sure each Component's dependencies are met and attach them to the target parent
			for (int i = 0; i < this.targetObj.Count; i++)
			{
				Component obj = this.targetObj[i];

				// Create dependency Components where required. This will extend the current loop
				foreach (Type required in obj.GetRequiredComponents().Reverse())
				{
					if (this.targetParentObj.GetComponent(required) != null) continue;
					obj = required.GetTypeInfo().CreateInstanceOf() as Component;

					// Setup newly created dependency Components for user editing
					SetupComponentForEditing(obj);

					// Inject them into the target and backup list
					this.backupObj.Insert(i, obj.DeepClone(BackupCloneContext));
					this.targetObj.Insert(i, obj);
				}

				// Attach the current Component to the target parent
				obj.GameObj = this.targetParentObj;
				DebugCheckParent(obj, this.targetParentObj);

				// If attaching the current Component failed, erase it from the action altogether
				if (obj.GameObj == null)
				{
					this.backupObj.RemoveAt(i);
					this.targetObj.RemoveAt(i);
					i--;
				}
			}

			// Notify the editor about changes to the Scene and parent object
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

		private static void SetupComponentForEditing(Component cmp)
		{
			// Gather all available user editing setup actions
			IEnumerable<IEditorAction> setupActions = DualityEditorApp.GetEditorActions(
				cmp.GetType(), 
				new[] { cmp },
				DualityEditorApp.ActionContextSetupObjectForEditing);

			// Invoke all of them on the specified Component
			foreach (IEditorAction setupAction in setupActions)
			{
				setupAction.Perform(cmp);
			}
		}

		private static Component PrepareNewUserEditComponent(Type cmpType)
		{
			Component cmp = cmpType.GetTypeInfo().CreateInstanceOf() as Component;
			SetupComponentForEditing(cmp);
			return cmp;
		}
		private static IEnumerable<Component> FilterPreparedComponents(IEnumerable<Component> cmp)
		{
			return cmp
				.Where(c => c != null && c.GameObj == null)
				.Distinct(ComponentTypeComparer.Default)
				.OrderBy(c => c.GetRequiredComponents().Count());
		}
	}
}
