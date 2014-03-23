using System;
using System.Collections.Generic;
using System.Linq;
using AdamsLair.WinForms;

using Duality;

namespace Duality.Editor.Plugins.Base.PropertyEditors
{
	[PropertyEditorAssignment(typeof(GameObjectOverviewPropertyEditor), "MatchToProperty")]
	public class GameObjectOverviewPropertyEditor : GroupedPropertyEditor
	{
		private	GameObjectPropertyEditor		gameObjEditor		= null;
		private	Dictionary<Type,PropertyEditor>	componentEditors	= new Dictionary<Type,PropertyEditor>();
		
		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}
		public override bool CanGetFocus
		{
			get { return false; }
		}

		public GameObjectOverviewPropertyEditor()
		{
			this.HeaderHeight = 0;
			this.Hints = HintFlags.None;
			this.Indent = 0;
			this.gameObjEditor = new GameObjectPropertyEditor();
		}

		public override void InitContent()
		{
			if (this.ContentInitialized) this.ClearContent();
			base.InitContent();

			if (this.EditedType != null)
			{
				this.PerformGetValue();
			}
		}
		public override void ClearContent()
		{
			base.ClearContent();
			this.ClearPropertyEditors();
			this.componentEditors.Clear();
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			GameObject[] values = this.GetValue().Cast<GameObject>().ToArray();
			if (values == null) return;

			if (!values.Any() || values.All(o => o == null))
			{
				this.ClearContent();
				this.Expanded = false;
			}
			else
			{
				if (this.ContentInitialized)
				{
					this.UpdateComponentEditors(values);
					foreach (PropertyEditor e in this.Children)
						e.PerformGetValue();
				}
				this.Expanded = !this.ContentInitialized || this.Children.Any();
			}
		}
		protected override void OnSetValue()
		{
			base.OnSetValue();
			if (this.ReadOnly) return;
			if (!this.Children.Any()) return;

			foreach (PropertyEditor e in this.Children)
				e.PerformSetValue();
		}

		protected void UpdateComponentEditors(GameObject[] values)
		{
			this.BeginUpdate();

			if (!this.Children.Any())
			{
				this.gameObjEditor.Getter = this.GetValue;
				this.gameObjEditor.Setter = this.SetValue;
				this.AddPropertyEditor(this.gameObjEditor);
			}
			Type[] typesInUse = values.GetComponents<Component>().Select(c => c.GetType()).Distinct().ToArray();

			// Remove Component editors that aren't needed anymore
			var cmpEditorCopy = new Dictionary<Type,PropertyEditor>(this.componentEditors);
			foreach (var pair in cmpEditorCopy)
			{
				if (!typesInUse.Contains(pair.Key))
				{
					this.RemovePropertyEditor(pair.Value);
					this.componentEditors.Remove(pair.Key);
				}
			}

			// Create the ones that are needed now and not added yet
			foreach (Type t in typesInUse)
			{
				if (!this.componentEditors.ContainsKey(t))
				{
					PropertyEditor e = this.ParentGrid.CreateEditor(t, this);
					e.Getter = this.CreateComponentValueGetter(t);
					e.Setter = this.CreateComponentValueSetter(t);
					e.PropertyName = t.GetTypeCSCodeName(true);
					this.ParentGrid.ConfigureEditor(e);
					this.AddPropertyEditor(e);
					this.componentEditors[t] = e;
				}
			}

			this.EndUpdate();
		}

		protected Func<IEnumerable<object>> CreateComponentValueGetter(Type componentType)
		{
			return () => this.GetValue().Select(o => o != null ? (o as GameObject).GetComponent(componentType) : null);
		}
		protected Action<IEnumerable<object>> CreateComponentValueSetter(Type componentType)
		{
			// We don't need a setter. At all.
			return v => {};
		}

		private static int MatchToProperty(Type propertyType, ProviderContext context)
		{
			if (typeof(GameObject).IsAssignableFrom(propertyType) && context.ParentEditor == null)
				return PropertyEditorAssignmentAttribute.PrioritySpecialized;
			else
				return PropertyEditorAssignmentAttribute.PriorityNone;
		}
	}
}
