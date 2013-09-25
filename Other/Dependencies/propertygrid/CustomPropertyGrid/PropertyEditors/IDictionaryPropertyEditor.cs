using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

using IDictionary = System.Collections.IDictionary;

using AdamsLair.PropertyGrid.Renderer;
using AdamsLair.PropertyGrid.EditorTemplates;

namespace AdamsLair.PropertyGrid.PropertyEditors
{
	public class IDictionaryPropertyEditor : GroupedPropertyEditor
	{
		public delegate void KeyValueSetter(PropertyInfo property, IEnumerable<object> targetObjects, IEnumerable<object> values, object key);

		private	bool					buttonIsCreate		= false;
		private	PropertyEditor			addKeyEditor		= null;
		private	object					addKey				= null;
		private	NumericPropertyEditor	offsetEditor		= null;
		private	int						offset				= 0;
		private	int						internalEditors		= 0;
		private	KeyValueSetter			dictKeySetter		= null;
		
		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}
		public KeyValueSetter DictionaryKeySetter
		{
			get { return this.dictKeySetter; }
			set
			{
				if (value == null) value = DefaultPropertySetter;
				this.dictKeySetter = value;
			}
		}

		public IDictionaryPropertyEditor()
		{
			this.Hints |= HintFlags.HasButton | HintFlags.ButtonEnabled;
			this.dictKeySetter = DefaultPropertySetter;

			this.offsetEditor = new NumericPropertyEditor();
			this.offsetEditor.EditedType = typeof(uint);
			this.offsetEditor.PropertyName = "Offset";
			this.offsetEditor.Getter = this.OffsetValueGetter;
			this.offsetEditor.Setter = this.OffsetValueSetter;
		}

		public override void InitContent()
		{
			base.InitContent();

			if (this.EditedType != null)
			{
				Type keyType = this.GetKeyType();
				this.addKeyEditor = this.ParentGrid.CreateEditor(keyType, this);
				this.addKeyEditor.Hints |= HintFlags.HasButton | HintFlags.ButtonEnabled;
				this.addKeyEditor.ButtonIcon = EmbeddedResources.Resources.ImageAdd;
				this.addKeyEditor.EditedType = keyType;
				this.addKeyEditor.PropertyName = "Add Entry";
				this.addKeyEditor.Getter = this.AddKeyValueGetter;
				this.addKeyEditor.Setter = this.AddKeyValueSetter;
				this.addKeyEditor.ButtonPressed += this.AddKeyButtonPressed;
				this.addKeyEditor.EditingFinished += this.AddKeyEditingFinished;

				this.PerformGetValue();
			}
			else
				this.ClearContent();
		}
		public override void ClearContent()
		{
			base.ClearContent();
			if (this.addKeyEditor != null)
			{
				this.addKeyEditor.ButtonPressed -= this.AddKeyButtonPressed;
				this.addKeyEditor.EditingFinished -= this.AddKeyEditingFinished;
				this.addKeyEditor = null;
			}
			this.offset = 0;
		}

		public override void PerformGetValue()
		{
			base.PerformGetValue();
			IDictionary[] values = this.GetValue().Cast<IDictionary>().ToArray();
			Type valueType = this.GetValueType();

			if (values == null)
			{
				this.HeaderValueText = null;
				return;
			}

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
				if (this.ContentInitialized)
				{
					if (this.Expanded)
						this.UpdateElementEditors(values, valueType);
					else
						this.ClearContent();
				}
				
				this.Hints |= HintFlags.ExpandEnabled;
				if (!this.CanExpand) this.Expanded = false;
				this.ButtonIcon = AdamsLair.PropertyGrid.EmbeddedResources.Resources.ImageDelete;
				this.buttonIsCreate = false;

				valString = values.Count() == 1 ? 
					string.Format("{0}, Count = {1}", this.EditedType.GetTypeCSCodeName(true), values.First().Count) :
					string.Format(AdamsLair.PropertyGrid.EmbeddedResources.Resources.PropertyGrid_N_Objects, values.Count());
			}

			this.HeaderValueText = valString;

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
		
		protected Type GetValueType()
		{
			if (this.EditedType.IsGenericType && this.EditedType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
				return this.EditedType.GetGenericArguments()[1];
			else
				return typeof(object);
		}
		protected Type GetKeyType()
		{
			if (this.EditedType.IsGenericType && this.EditedType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
				return this.EditedType.GetGenericArguments()[0];
			else
				return typeof(object);
		}
		protected void UpdateElementEditors(IDictionary[] values, Type valueType)
		{
			PropertyInfo indexer = typeof(IDictionary).GetProperty("Item");
			int visibleElementCount = values.Where(o => o != null).Min(o => (int)o.Count);
			bool showOffset = false;
			if (visibleElementCount > 10)
			{
				this.offset = Math.Min(this.offset, visibleElementCount - 10);
				this.offsetEditor.Maximum = visibleElementCount - 10;
				visibleElementCount = 10;
				showOffset = true;
			}
			else
			{
				this.offset = 0;
			}

			if (this.addKeyEditor.ParentEditor == null) this.AddPropertyEditor(this.addKeyEditor, 0);
			if (showOffset && this.offsetEditor.ParentEditor == null) this.AddPropertyEditor(this.offsetEditor, 1);
			else if (!showOffset && this.offsetEditor.ParentEditor != null) this.RemovePropertyEditor(this.offsetEditor);

			this.internalEditors = showOffset ? 2 : 1;
			
			// Determine which keys are currently displayed
			int elemIndex = 0;
			object[] keys = new object[visibleElementCount];
			foreach (object key in values.Where(o => o != null).First().Keys)
			{
				if (elemIndex >= this.offset)
				{
					keys[elemIndex - this.offset] = key;
				}
				elemIndex++;
				if (elemIndex >= this.offset + visibleElementCount) break;
			}

			this.BeginUpdate();

			// Add missing editors
			for (int i = this.internalEditors; i < visibleElementCount + this.internalEditors; i++)
			{
				PropertyEditor elementEditor;
				if (i < this.Children.Count())
				{
					elementEditor = this.Children.ElementAt(i);
				}
				else
				{
					elementEditor = this.ParentGrid.CreateEditor(valueType, this);
					this.AddPropertyEditor(elementEditor);
					this.ParentGrid.ConfigureEditor(elementEditor);
				}
				elementEditor.Getter = this.CreateElementValueGetter(indexer, keys[i - this.internalEditors]);
				elementEditor.Setter = this.CreateElementValueSetter(indexer, keys[i - this.internalEditors]);
				elementEditor.PropertyName = "[" + keys[i - this.internalEditors].ToString() + "]";
			}
			// Remove overflowing editors
			for (int i = this.Children.Count() - (this.internalEditors + 1); i >= visibleElementCount; i--)
			{
				PropertyEditor child = this.Children.Last();
				this.RemovePropertyEditor(child);
			}
			this.EndUpdate();
		}
		protected override bool IsChildReadOnly(PropertyEditor childEditor)
		{
			if (childEditor == this.offsetEditor) return false;
			return base.IsChildReadOnly(childEditor);
		}
		
		protected IEnumerable<object> AddKeyValueGetter()
		{
			//if (this.addKey == null) this.addKey = ReflectionHelper.CreateInstanceOf(this.GetKeyType());
			return new object[] { this.addKey };
		}
		protected void AddKeyValueSetter(IEnumerable<object> keys)
		{
			this.addKey = keys.FirstOrDefault();
		}
		protected void AddKeyButtonPressed(object sender, EventArgs e)
		{
			if (this.addKey == null) return;

			IDictionary[] targetArray = this.GetValue().Cast<IDictionary>().ToArray();
			Type valueType = this.GetValueType();

			for (int t = 0; t < targetArray.Length; t++)
			{
				IDictionary target = targetArray[t];
				if (target != null)
				{
					if (!target.IsFixedSize && !target.IsReadOnly)
					{
						if (!target.Contains(this.addKey))
						{
							// Add a new key value pair
							target.Add(this.addKey, valueType.IsValueType ? ReflectionHelper.CreateInstanceOf(valueType) : null);
						}
					}
					else
					{
						// Just some read-only container? Well, can't do anything here.
					}
				}
			}

			// Focus-Unfocus to trigger some kind of "select all" / "reset" behaivor in the "Add Key" field.
			this.ParentGrid.Focus(this);
			this.addKeyEditor.Focus();

			// Reset add key editor and update sub-editors
			this.addKey = null;
			this.PerformGetValue();
		}
		protected void AddKeyEditingFinished(object sender, PropertyEditingFinishedEventArgs e)
		{
			if (e.Reason == FinishReason.UserAccept)
				AddKeyButtonPressed(sender, e);
		}
		protected IEnumerable<object> OffsetValueGetter()
		{
			yield return (uint)this.offset;
		}
		protected void OffsetValueSetter(IEnumerable<object> values)
		{
			this.offset = (int)Convert.ChangeType(values.First(), typeof(int));
			this.PerformGetValue();
		}
		protected Func<IEnumerable<object>> CreateElementValueGetter(PropertyInfo indexer, object key)
		{
			return () => this.GetValue().Select(o => o != null ? indexer.GetValue(o, new object[] {key}) : null);
		}
		protected Action<IEnumerable<object>> CreateElementValueSetter(PropertyInfo indexer, object key)
		{
			return delegate(IEnumerable<object> values)
			{
				object[] targetArray = this.GetValue().ToArray();
				this.dictKeySetter(indexer, targetArray, values, key);
				if (this.ForceWriteBack) this.SetValues(targetArray);
			};
		}

		protected override void OnEditedTypeChanged()
		{
			base.OnEditedTypeChanged();
			this.Expanded = false;
			this.ClearContent();
		}
		protected override void OnButtonPressed()
		{
			base.OnButtonPressed();

			if (this.buttonIsCreate)
			{
				IDictionary newIList = null;
				newIList = (IDictionary)ReflectionHelper.CreateInstanceOf(this.EditedType);

				this.SetValue(newIList);
				this.Expanded = true;
			}
			else
			{
				this.SetValue(null);
			}

			this.PerformGetValue();
		}

		protected static void DefaultPropertySetter(PropertyInfo property, IEnumerable<object> targetObjects, IEnumerable<object> values, object key)
		{
			IEnumerator<object> valuesEnum = values.GetEnumerator();
			object curValue = null;

			if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
			foreach (object target in targetObjects)
			{
				if (target != null) property.SetValue(target, curValue, new object[] {key});
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
			}
		}
	}
}
