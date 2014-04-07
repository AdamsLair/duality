using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;

using Duality;
using Duality.Components.Physics;
using Duality.Editor;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.Base.UndoRedoActions;


namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(RigidBodyPropertyEditor), "MatchToProperty")]
	public class RigidBodyPropertyEditor : ComponentPropertyEditor
	{
		private RigidBodyJointAddNewPropertyEditor	addJointEditor	= null;
		private	List<RigidBodyJointPropertyEditor>	jointEditors	= new List<RigidBodyJointPropertyEditor>();
		
		public override void ClearContent()
		{
			base.ClearContent();
			this.jointEditors.Clear();
			this.addJointEditor = null;
		}
		public override MemberInfo MapEditorToMember(PropertyEditor editor)
		{
			if (this.jointEditors.Contains(editor))
				return ReflectionInfo.Property_RigidBody_Joints;
			else
				return base.MapEditorToMember(editor);
		}

		protected override void BeforeAutoCreateEditors()
		{
			base.BeforeAutoCreateEditors();
			this.UpdateJointEditors(this.GetValue().Cast<RigidBody>());
		}
		protected override void OnUpdateFromObjects(object[] values)
		{
			base.OnUpdateFromObjects(values);
			this.UpdateJointEditors(values.Cast<RigidBody>());
		}
		protected override void OnPropertySet(PropertyInfo property, IEnumerable<object> targets)
		{
			base.OnPropertySet(property, targets);
			foreach (RigidBody c in targets.OfType<RigidBody>())
				c.AwakeBody();
		}

		protected void UpdateJointEditors(IEnumerable<RigidBody> values)
		{
			RigidBody[] valArray = values.ToArray();
			int visibleElementCount = valArray.NotNull().Min(o => o.Joints == null ? 0 : o.Joints.Count());

			// Add missing editors
			for (int i = 0; i < visibleElementCount; i++)
			{
				JointInfo joint = valArray.NotNull().First().Joints.ElementAtOrDefault(i);
				Type jointType = joint.GetType();
				bool matchesAll = valArray.NotNull().All(r => jointType.IsInstanceOfType(r.Joints.ElementAtOrDefault(i)));
				if (matchesAll)
				{
					RigidBodyJointPropertyEditor elementEditor;
					if (i < this.jointEditors.Count)
					{
						elementEditor = this.jointEditors[i];
						if (elementEditor.EditedType != jointType)
						{
							elementEditor.EditedType = jointType;
							this.ParentGrid.ConfigureEditor(elementEditor);
						}
					}
					else
					{
						elementEditor = new RigidBodyJointPropertyEditor();
						elementEditor.EditedType = jointType;
						this.ParentGrid.ConfigureEditor(elementEditor);
						this.jointEditors.Add(elementEditor);
					}
					elementEditor.PropertyName = string.Format("Joints[{0}]", i);
					elementEditor.Getter = this.CreateJointValueGetter(i);
					elementEditor.Setter = this.CreateJointValueSetter(i);
					elementEditor.ParentGetter = this.GetValue;
					if (!this.HasPropertyEditor(this.jointEditors[i])) this.AddPropertyEditor(this.jointEditors[i], i);
				}
				else if (this.jointEditors.Count > i)
				{
					this.RemovePropertyEditor(this.jointEditors[i]);
				}
			}
			// Remove overflowing editors
			for (int i = this.jointEditors.Count - 1; i >= visibleElementCount; i--)
			{
				this.RemovePropertyEditor(this.jointEditors[i]);
				this.jointEditors.RemoveAt(i);
			}

			// Add "Add joint" editor
			if (this.addJointEditor == null)
			{
				this.addJointEditor = new RigidBodyJointAddNewPropertyEditor();
				this.addJointEditor.Getter = this.CreateAddNewJointValueGetter();
				this.addJointEditor.Setter = v => {};
				this.ParentGrid.ConfigureEditor(this.addJointEditor);
				this.AddPropertyEditor(this.addJointEditor);
			}
		}

		protected Func<IEnumerable<object>> CreateJointValueGetter(int index)
		{
			return () => this.GetValue().Cast<RigidBody>().Select(o => o != null ? o.Joints.ElementAtOrDefault(index) : null);
		}
		protected Action<IEnumerable<object>> CreateJointValueSetter(int index)
		{
			return delegate(IEnumerable<object> values)
			{
				IEnumerable<JointInfo> valuesCast = values.Cast<JointInfo>();
				RigidBody[] targetArray = this.GetValue().Cast<RigidBody>().ToArray();

				// Explicitly setting a value to null: Remove corresponding joint
				if (valuesCast.All(v => v == null))
				{
					UndoRedoManager.Do(new DeleteRigidBodyJointAction(targetArray.Select(body => body.Joints.ElementAt(index))));
				}
			};
		}
		
		protected Func<IEnumerable<object>> CreateAddNewJointValueGetter()
		{
			return () => 
			{
				this.addJointEditor.TargetColliders = this.GetValue().Cast<RigidBody>().ToArray();
				return new object[] { this.addJointEditor.DisplayedValue };
			};
		}

		private static int MatchToProperty(Type propertyType, ProviderContext context)
		{
			bool compRef = !(context.ParentEditor is GameObjectOverviewPropertyEditor);
			if (typeof(RigidBody).IsAssignableFrom(propertyType) && !compRef)
				return PropertyEditorAssignmentAttribute.PriorityGeneral + 1;
			else
				return PropertyEditorAssignmentAttribute.PriorityNone;
		}
	}

	public class RigidBodyJointPropertyEditor : MemberwisePropertyEditor
	{
		private Func<IEnumerable<object>> parentGetter = null;
		private PropertyEditor otherColEditor = null;

		internal Func<IEnumerable<object>> ParentGetter
		{
			get { return this.parentGetter; }
			set { this.parentGetter = value; }
		}

		public RigidBodyJointPropertyEditor()
		{
			this.EditedType = typeof(JointInfo);
			this.HeaderStyle = GroupHeaderStyle.SmoothSunken;
			this.HeaderHeight = 30;
		}

		public override void ClearContent()
		{
			base.ClearContent();
			this.otherColEditor = null;
		}
		protected override void BeforeAutoCreateEditors()
		{
			base.BeforeAutoCreateEditors();
			JointInfo joint = this.GetValue().Cast<JointInfo>().FirstOrDefault();

			if (joint != null && joint.DualJoint)
			{
				if (this.otherColEditor == null)
				{
					this.otherColEditor = this.ParentGrid.CreateEditor(typeof(RigidBody), this);
					this.otherColEditor.Getter = this.CreateOtherColValueGetter();
					this.otherColEditor.Setter = this.CreateOtherColValueSetter();
					this.otherColEditor.PropertyName = Properties.EditorBaseRes.PropertyName_OtherCollider;
					this.otherColEditor.PropertyDesc = Properties.EditorBaseRes.PropertyDesc_OtherCollider;
					this.ParentGrid.ConfigureEditor(this.otherColEditor);
					this.AddPropertyEditor(this.otherColEditor);
				}
			}
			else if (this.otherColEditor != null)
			{
				this.RemovePropertyEditor(this.otherColEditor);
				this.otherColEditor = null;
			}
		}
		protected override void OnUpdateFromObjects(object[] values)
		{
			base.OnUpdateFromObjects(values);
			IEnumerable<JointInfo> joints = values.Cast<JointInfo>().NotNull();

			this.HeaderValueText = null;
			if (joints.Any())
				this.HeaderValueText = joints.First().GetType().Name;
			else
				this.HeaderValueText = "null";
		}
		protected override void OnPropertySet(PropertyInfo property, IEnumerable<object> targets)
		{
			base.OnPropertySet(property, targets);

			var colJoints = targets.OfType<JointInfo>().ToArray();
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, property, colJoints, null));

			var colliders = 
				colJoints.Select(c => c.BodyA).Concat(
				colJoints.Select(c => c.BodyB))
				.Distinct().NotNull().ToArray();
			foreach (var c in colliders) c.AwakeBody();
			UndoRedoManager.Do(new EditPropertyAction(this.ParentGrid, ReflectionInfo.Property_RigidBody_Joints, colliders, null));
		}

		protected Func<IEnumerable<object>> CreateOtherColValueGetter()
		{
			return () => 
			{
				JointInfo[] targetArray = this.GetValue().Cast<JointInfo>().ToArray();
				RigidBody[] otherCollider = new RigidBody[targetArray.Length];
				RigidBody[] parentCollider = this.parentGetter().Cast<RigidBody>().ToArray();
				for (int i = 0; i < targetArray.Length; i++)
				{
					if (targetArray[i] != null)
						otherCollider[i] = targetArray[i].BodyA == parentCollider[i] ? targetArray[i].BodyB : targetArray[i].BodyA;
					else
						otherCollider[i] = null;
				}
				return otherCollider;
			};
		}
		protected Action<IEnumerable<object>> CreateOtherColValueSetter()
		{
			return delegate(IEnumerable<object> values)
			{
				UndoRedoManager.Do(new ReparentRigidBodyJointAction(
					this.GetValue().Cast<JointInfo>(),
					this.parentGetter().Cast<RigidBody>(),
					values.Cast<RigidBody>()));
			};
		}
	}

	public class RigidBodyJointAddNewPropertyEditor : ObjectSelectorPropertyEditor
	{
		private RigidBody[]	targetArray	= null;

		public RigidBody[] TargetColliders
		{
			get { return this.targetArray; }
			set { this.targetArray = value; }
		}

		public RigidBodyJointAddNewPropertyEditor()
		{
			this.EditedType = typeof(Type);
			this.ButtonIcon = AdamsLair.WinForms.Properties.ResourcesCache.ImageAdd;
			this.Hints = HintFlags.Default | HintFlags.HasButton | HintFlags.ButtonEnabled;
			this.PropertyName = Properties.EditorBaseRes.PropertyName_AddJoint;
			this.PropertyDesc = Properties.EditorBaseRes.PropertyDesc_AddJoint;

			this.Items = 
				from t in DualityApp.GetAvailDualityTypes(typeof(JointInfo))
				where !t.IsAbstract && !t.GetCustomAttributes<EditorHintFlagsAttribute>().Any(f => f.Flags.HasFlag(MemberFlags.Invisible))
				select new ObjectItem(t, t.Name.Replace("JointInfo", "Joint"));
		}
		protected override void OnReadOnlyChanged()
		{
			base.OnReadOnlyChanged();
			if (this.ReadOnly)
				this.Hints &= ~HintFlags.ButtonEnabled;
			else
				this.Hints |= HintFlags.ButtonEnabled;
		}
		protected override void OnButtonPressed()
		{
			base.OnButtonPressed();
			Type jointType = this.DisplayedValue as Type;
			if (jointType == null) return;

			IEnumerable<JointInfo> newJoints = Enumerable.Repeat(jointType, this.targetArray.Length).Select(t => (t.CreateInstanceOf() ?? t.CreateInstanceOf(true)) as JointInfo);
			UndoRedoManager.Do(new CreateRigidBodyJointAction(this.targetArray, newJoints));
		}
	}
}
