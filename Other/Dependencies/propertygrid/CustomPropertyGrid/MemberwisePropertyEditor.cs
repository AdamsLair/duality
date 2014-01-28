using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace AdamsLair.PropertyGrid
{
	public class MemberwisePropertyEditor : GroupedPropertyEditor
	{
		public delegate bool AutoMemberPredicate(MemberInfo member, bool showNonPublic);
		public delegate void PropertyValueSetter(PropertyInfo property, IEnumerable<object> targetObjects, IEnumerable<object> values);
		public delegate void FieldValueSetter(FieldInfo field, IEnumerable<object> targetObjects, IEnumerable<object> values);

		private	bool	buttonIsCreate	= false;
		private	AutoMemberPredicate				memberPredicate			= null;
		private	Predicate<MemberInfo>			memberAffectsOthers		= null;
		private	Func<MemberInfo,PropertyEditor>	memberEditorCreator		= null;
		private	PropertyValueSetter				memberPropertySetter	= null;
		private	FieldValueSetter				memberFieldSetter		= null;

		public override object DisplayedValue
		{
			get { return this.GetValue().FirstOrDefault(); }
		}
		public AutoMemberPredicate MemberPredicate
		{
			get { return this.memberPredicate; }
			set
			{
				if (value == null) value = DefaultMemberPredicate;
				if (this.memberPredicate != value)
				{
					this.memberPredicate = value;
					if (this.ContentInitialized) this.InitContent();
				}
			}
		}
		public Predicate<MemberInfo> MemberAffectsOthers
		{
			get { return this.memberAffectsOthers; }
			set
			{
				if (value == null) value = DefaultMemberAffectsOthers;
				if (this.memberAffectsOthers != value)
				{
					this.memberAffectsOthers = value;
					if (this.ContentInitialized) this.InitContent();
				}
			}
		}
		public Func<MemberInfo,PropertyEditor> MemberEditorCreator
		{
			get { return this.memberEditorCreator; }
			set
			{
				if (value == null) value = DefaultMemberEditorCreator;
				if (this.memberEditorCreator != value)
				{
					this.memberEditorCreator = value;
					if (this.ContentInitialized) this.InitContent();
				}
			}
		}
		public PropertyValueSetter MemberPropertySetter
		{
			get { return this.memberPropertySetter; }
			set
			{
				if (value == null) value = DefaultPropertySetter;
				this.memberPropertySetter = value;
			}
		}
		public FieldValueSetter MemberFieldSetter
		{
			get { return this.memberFieldSetter; }
			set
			{
				if (value == null) value = DefaultFieldSetter;
				this.memberFieldSetter = value;
			}
		}


		public MemberwisePropertyEditor()
		{
			this.Hints |= HintFlags.HasButton | HintFlags.ButtonEnabled;
			this.memberEditorCreator = DefaultMemberEditorCreator;
			this.memberPredicate = DefaultMemberPredicate;
			this.memberAffectsOthers = DefaultMemberAffectsOthers;
			this.memberPropertySetter = DefaultPropertySetter;
			this.memberFieldSetter = DefaultFieldSetter;
		}

		public override void InitContent()
		{
			this.ClearContent();
			if (this.EditedType != null)
			{
				base.InitContent();

				// Look for all the properties, even nonpublic - they'll get sorted out by the predicate, if unwanted.
				BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

				// Generate and add property editors for the current type
				this.BeginUpdate();
				this.BeforeAutoCreateEditors();
				// Properties
				{
					PropertyInfo[] propArr = this.EditedType.GetProperties(flags);
					var propQuery = 
						from p in propArr
						where p.CanRead && p.GetIndexParameters().Length == 0 && this.IsAutoCreateMember(p)
						orderby GetTypeHierarchyLevel(p.DeclaringType) ascending, p.Name
						select p;
					foreach (PropertyInfo prop in propQuery)
					{
						this.AddEditorForProperty(prop);
					}
				}
				// Fields
				{
					FieldInfo[] fieldArr = this.EditedType.GetFields(flags);
					var fieldQuery =
						from f in fieldArr
						where this.IsAutoCreateMember(f)
						orderby GetTypeHierarchyLevel(f.DeclaringType) ascending, f.Name
						select f;
					foreach (FieldInfo field in fieldQuery)
					{
						this.AddEditorForField(field);
					}
				}
				this.EndUpdate();
				this.PerformGetValue();
			}
		}

		public PropertyEditor AddEditorForProperty(PropertyInfo prop)
		{
			PropertyEditor e = this.AutoCreateMemberEditor(prop);
			if (e == null) e = this.ParentGrid.CreateEditor(prop.PropertyType, this);
			if (e == null) return null;
			e.BeginUpdate();
			e.Getter = this.CreatePropertyValueGetter(prop);
			e.Setter = prop.CanWrite ? this.CreatePropertyValueSetter(prop) : null;
			e.PropertyName = prop.Name;
			e.EditedMember = prop;
			e.NonPublic = !this.memberPredicate(prop, false);
			this.AddPropertyEditor(e);
			this.ParentGrid.ConfigureEditor(e);
			e.EndUpdate();
			return e;
		}
		public PropertyEditor AddEditorForField(FieldInfo field)
		{
			PropertyEditor e = this.AutoCreateMemberEditor(field);
			if (e == null) e = this.ParentGrid.CreateEditor(field.FieldType, this);
			if (e == null) return null;
			e.BeginUpdate();
			e.Getter = this.CreateFieldValueGetter(field);
			e.Setter = this.CreateFieldValueSetter(field);
			e.PropertyName = field.Name;
			e.EditedMember = field;
			e.NonPublic = !this.memberPredicate(field, false);
			this.AddPropertyEditor(e);
			this.ParentGrid.ConfigureEditor(e);
			e.EndUpdate();
			return e;
		}

		public override void PerformGetValue()
		{
			base.PerformGetValue();
			object[] curObjects = this.GetValue().ToArray();

			this.BeginUpdate();
			if (curObjects == null)
			{
				this.HeaderValueText = null;
				return;
			}
			this.OnUpdateFromObjects(curObjects);
			this.EndUpdate();

			foreach (PropertyEditor e in this.Children)
				e.PerformGetValue();
		}
		public override void PerformSetValue()
		{
			if (this.ReadOnly) return;
			if (!this.Children.Any()) return;
			base.PerformSetValue();

			foreach (PropertyEditor e in this.Children)
				e.PerformSetValue();
		}
		protected virtual void OnUpdateFromObjects(object[] values)
		{
			string valString = null;

			if (!values.Any() || values.All(o => o == null))
			{
				this.ClearContent();

				this.Hints &= ~HintFlags.ExpandEnabled;
				this.ButtonIcon = AdamsLair.PropertyGrid.EmbeddedResources.Resources.ImageAdd;
				this.buttonIsCreate = true;
				this.Expanded = false;
					
				valString = "null";
			}
			else
			{
				this.Hints |= HintFlags.ExpandEnabled;
				if (!this.CanExpand) this.Expanded = false;
				this.ButtonIcon = AdamsLair.PropertyGrid.EmbeddedResources.Resources.ImageDelete;
				this.buttonIsCreate = false;

				valString = values.Count() == 1 ? 
					values.First().ToString() :
					string.Format(AdamsLair.PropertyGrid.EmbeddedResources.Resources.PropertyGrid_N_Objects, values.Count());
			}

			this.HeaderValueText = valString;
		}
		protected override void OnEditedTypeChanged()
		{
			base.OnEditedTypeChanged();
			if (this.EditedType.IsValueType)
				this.Hints &= ~HintFlags.HasButton;
			else
				this.Hints |= HintFlags.HasButton;
			if (this.ContentInitialized) this.InitContent();
		}
		protected override void OnEditedMemberChanged()
		{
			base.OnEditedTypeChanged();
			if (this.ContentInitialized) this.InitContent();
		}
		protected override void OnButtonPressed()
		{
			base.OnButtonPressed();
			if (this.EditedType.IsValueType)
			{
				this.SetValue(this.ParentGrid.CreateObjectInstance(this.EditedType));
			}
			else
			{
				if (this.buttonIsCreate)
				{
					this.SetValue(this.ParentGrid.CreateObjectInstance(this.EditedType));
					this.Expanded = true;
				}
				else
				{
					this.SetValue(null);
				}
			}

			this.PerformGetValue();
		}

		protected virtual void BeforeAutoCreateEditors() {}
		protected virtual bool IsAutoCreateMember(MemberInfo info)
		{
			return this.memberPredicate(info, this.ParentGrid.ShowNonPublic);
		}
		protected virtual PropertyEditor AutoCreateMemberEditor(MemberInfo info)
		{
			return this.memberEditorCreator(info);
		}

		protected internal override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!this.buttonIsCreate && e.KeyCode == Keys.Delete)
			{
				this.OnButtonPressed();
				e.Handled = true;
			}
			else if (this.buttonIsCreate && e.KeyCode == Keys.Return)
			{
				this.OnButtonPressed();
				e.Handled = true;
			}
		}

		protected Func<IEnumerable<object>> CreatePropertyValueGetter(PropertyInfo property)
		{
			return () => this.GetValue().Select(o => o != null ? property.GetValue(o, null) : null);
		}
		protected Func<IEnumerable<object>> CreateFieldValueGetter(FieldInfo field)
		{
			return () => this.GetValue().Select(o => o != null ? field.GetValue(o) : null);
		}
		protected Action<IEnumerable<object>> CreatePropertyValueSetter(PropertyInfo property)
		{
			bool affectsOthers = this.ParentGrid.ShowNonPublic || this.memberAffectsOthers(property);
			return delegate(IEnumerable<object> values)
			{
				object[] targetArray = this.GetValue().ToArray();

				// Set value
				this.memberPropertySetter(property, targetArray, values);

				// Fixup struct values by assigning the modified struct copy to its original member
				if (this.EditedType.IsValueType || this.ForceWriteBack) this.SetValues((IEnumerable<object>)targetArray);

				this.OnPropertySet(property, targetArray);
				if (affectsOthers)
					this.PerformGetValue();
				else
					this.OnUpdateFromObjects(this.GetValue().ToArray());
			};
		}
		protected Action<IEnumerable<object>> CreateFieldValueSetter(FieldInfo field)
		{
			bool affectsOthers = this.ParentGrid.ShowNonPublic || this.memberAffectsOthers(field);
			return delegate(IEnumerable<object> values)
			{
				object[] targetArray = this.GetValue().ToArray();

				// Set value
				this.memberFieldSetter(field, targetArray, values);

				// Fixup struct values by assigning the modified struct copy to its original member
				if (this.EditedType.IsValueType || this.ForceWriteBack) this.SetValues((IEnumerable<object>)targetArray);

				this.OnFieldSet(field, targetArray);
				if (affectsOthers)
					this.PerformGetValue();
				else
					this.OnUpdateFromObjects(this.GetValue().ToArray());
			};
		}

		protected virtual void OnPropertySet(PropertyInfo property, IEnumerable<object> targets) {}
		protected virtual void OnFieldSet(FieldInfo property, IEnumerable<object> targets) {}

		protected static bool DefaultMemberPredicate(MemberInfo info, bool showNonPublic)
		{
			if (showNonPublic)
			{
				return true;
			}
			else if (info is PropertyInfo)
			{
				PropertyInfo property = info as PropertyInfo;
				MethodInfo getter = property.GetGetMethod(true);
				return getter != null && getter.IsPublic;
			}
			else if (info is FieldInfo)
			{
				FieldInfo field = info as FieldInfo;
				return field.IsPublic;
			}
			else
			{
				return false;
			}
		}
		protected static bool DefaultMemberAffectsOthers(MemberInfo info)
		{
			return false;
		}
		protected static PropertyEditor DefaultMemberEditorCreator(MemberInfo info)
		{
			return null;
		}
		protected static void DefaultPropertySetter(PropertyInfo property, IEnumerable<object> targetObjects, IEnumerable<object> values)
		{
			IEnumerator<object> valuesEnum = values.GetEnumerator();
			object curValue = null;

			if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
			foreach (object target in targetObjects)
			{
				if (target != null) property.SetValue(target, curValue, null);
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
			}
		}
		protected static void DefaultFieldSetter(FieldInfo field, IEnumerable<object> targetObjects, IEnumerable<object> values)
		{
			IEnumerator<object> valuesEnum = values.GetEnumerator();
			object curValue = null;

			if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
			foreach (object target in targetObjects)
			{
				if (target != null) field.SetValue(target, curValue);
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
			}
		}

		private static int GetTypeHierarchyLevel(Type t)
		{
			int level = 0;
			while (t.BaseType != null) { t = t.BaseType; level++; }
			return level;
		}
	}
}
