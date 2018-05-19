using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Duality;
using Duality.Cloning;
using Duality.Editor.Forms;
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

				// Create dependency Components where required. This will extend the current loop.
				// (Reversed, so repeated injection at the same index will yield the original order)
				List<Type> createRequirements = Component.RequireMap
					.GetRequirementsToCreate(this.targetParentObj, obj.GetType())
					.Reverse().ToList();
				for (int j = 0; j < createRequirements.Count; j++)
				{
					Type required = createRequirements[j];

					// If the type can't be instantiated, ask the user for a concrete type to use
					if (required.IsInterface || required.IsAbstract)
					{
						required = GetRequiredConcreteType(required, obj.GetType());
						if (required == null)
						{
							Log.Editor.WriteWarning("Failed to add {0} because its requirements could not be resolved", obj.GetType().GetTypeCSCodeName(true));
							this.targetObj.Clear();
							return;
						}

						// Get additional requirements for the concrete type
						IEnumerable<Type> additionalRequirements =
							Component.RequireMap.GetRequirementsToCreate(this.targetParentObj, required)
								// Except types that are already covered by a previously identified requirement
								.Where(newReq => !createRequirements.Any(newReq.IsAssignableFrom));
						createRequirements.AddRange(additionalRequirements);
					}

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

		/// <summary>
		/// Gets a concrete type from the user that extends from the given type.
		/// </summary>
		/// <param name="type">The abstract type to get a concrete type for</param>
		/// <param name="typeWithRequirement">The type that needs this requirement satisfied</param>
		private static Type GetRequiredConcreteType(Type type, Type typeWithRequirement)
		{
			ListSelectionDialog typeDialog = new ListSelectionDialog
			{
				FilteredType = type,
				SelectType = true,
				HeaderText = string.Format("{0} requires one of the following components. Please select one.", typeWithRequirement.GetTypeCSCodeName(true))
			};
			DialogResult result = typeDialog.ShowDialog();
			if (result == DialogResult.OK && typeDialog.TypeReference != null)
			{
				return typeDialog.TypeReference;
			}
			return null;
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
				.OrderBy(c => Component.RequireMap.GetRequirements(c.GetType()).Count());
		}
	}
}
