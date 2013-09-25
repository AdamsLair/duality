using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

using IList = System.Collections.IList;

using AdamsLair.PropertyGrid.Renderer;
using AdamsLair.PropertyGrid.EditorTemplates;

namespace AdamsLair.PropertyGrid.PropertyEditors
{
	public class IListPropertyEditor : GroupedPropertyEditor
	{
		public delegate void IndexValueSetter(PropertyInfo indexer, IEnumerable<object> targetObjects, IEnumerable<object> values, int index);

		private	bool					buttonIsCreate	= false;
		private	NumericPropertyEditor	sizeEditor		= null;
		private	NumericPropertyEditor	offsetEditor	= null;
		private	int						offset			= 0;
		private	int						internalEditors	= 0;
		private	IndexValueSetter		listIndexSetter	= null;
		
		public override object DisplayedValue
		{
			get { return this.GetValue(); }
		}
		public IndexValueSetter ListIndexSetter
		{
			get { return this.listIndexSetter; }
			set
			{
				if (value == null) value = DefaultPropertySetter;
				this.listIndexSetter = value;
			}
		}

		public IListPropertyEditor()
		{
			this.Hints |= HintFlags.HasButton | HintFlags.ButtonEnabled;

			this.listIndexSetter = DefaultPropertySetter;

			this.sizeEditor = new NumericPropertyEditor();
			this.sizeEditor.EditedType = typeof(uint);
			this.sizeEditor.PropertyName = "Size";
			this.sizeEditor.Getter = this.SizeValueGetter;
			this.sizeEditor.Setter = this.SizeValueSetter;

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
				this.PerformGetValue();
			else
				this.ClearContent();
		}
		public override void ClearContent()
		{
			base.ClearContent();
			this.offset = 0;
		}

		public override void PerformGetValue()
		{
			base.PerformGetValue();
			IList[] values = this.GetValue().Cast<IList>().ToArray();
			Type elementType = this.GetElementType();

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
						this.UpdateElementEditors(values, elementType);
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

		protected Type GetElementType()
		{
			if (this.EditedType.HasElementType)
				return this.EditedType.GetElementType();
			else if (this.EditedType.IsGenericType)
				return this.EditedType.GetGenericArguments()[0];
			else
				return typeof(object);
		}
		protected void UpdateElementEditors(IList[] values, Type elementType)
		{
			PropertyInfo indexer = typeof(IList).GetProperty("Item");
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

			if (this.sizeEditor.ParentEditor == null) this.AddPropertyEditor(this.sizeEditor, 0);
			if (showOffset && this.offsetEditor.ParentEditor == null) this.AddPropertyEditor(this.offsetEditor, 1);
			else if (!showOffset && this.offsetEditor.ParentEditor != null) this.RemovePropertyEditor(this.offsetEditor);

			this.internalEditors = showOffset ? 2 : 1;

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
					elementEditor = this.ParentGrid.CreateEditor(elementType, this);
					//elementEditor.ButtonPressed += elementEditor_ButtonPressed;
					this.AddPropertyEditor(elementEditor);
					this.ParentGrid.ConfigureEditor(elementEditor);
				}
				elementEditor.Getter = this.CreateElementValueGetter(indexer, i - this.internalEditors + this.offset);
				elementEditor.Setter = this.CreateElementValueSetter(indexer, i - this.internalEditors + this.offset);
				elementEditor.PropertyName = "[" + (i - this.internalEditors + this.offset) + "]";
				//elementEditor.Hints |= HintFlags.HasButton | HintFlags.ButtonEnabled;
			}
			// Remove overflowing editors
			for (int i = this.Children.Count() - (this.internalEditors + 1); i >= visibleElementCount; i--)
			{
				PropertyEditor child = this.Children.Last();
				//child.ButtonPressed -= elementEditor_ButtonPressed;
				this.RemovePropertyEditor(child);
			}
			this.EndUpdate();
		}
		protected override bool IsChildReadOnly(PropertyEditor childEditor)
		{
			if (childEditor == this.offsetEditor) return false;
			return base.IsChildReadOnly(childEditor);
		}

		protected void RemoveElementAt(int index)
		{
			IList[] targetArray = this.GetValue().Cast<IList>().ToArray();
			Type elementType = this.GetElementType();

			bool writeBack = false;
			for (int t = 0; t < targetArray.Length; t++)
			{
				IList target = targetArray[t];
				if (target != null)
				{
					if (!target.IsFixedSize && !target.IsReadOnly)
					{
						// Dynamically adjust IList length
						target.RemoveAt(index);
					}
					else if (target is Array)
					{
						// Create new array that replaces the old one
						Array newTarget = Array.CreateInstance(elementType, target.Count - 1);
						for (int i = 0; i < index; i++)					newTarget.SetValue(target[i], i);
						for (int i = index + 1; i < target.Count; i++)	newTarget.SetValue(target[i], i - 1);
						targetArray[t] = newTarget;
						writeBack = true;
					}
					else
					{
						// Just some read-only container? Well, can't do anything here.
					}
				}
			}
			if (writeBack || this.ForceWriteBack) this.SetValues(targetArray);
			this.PerformGetValue();
		}

		protected IEnumerable<object> SizeValueGetter()
		{
			return this.GetValue().Select(o => o != null ? (object)((IList)o).Count : null);
		}
		protected void SizeValueSetter(IEnumerable<object> values)
		{
			IEnumerator<object> valuesEnum = values.GetEnumerator();
			IList[] targetArray = this.GetValue().Cast<IList>().ToArray();
			Type elementType = this.GetElementType();

			bool writeBack = false;
			uint curValue = 0;
			if (valuesEnum.MoveNext()) curValue = (uint)valuesEnum.Current;
			for (int t = 0; t < targetArray.Length; t++)
			{
				IList target = targetArray[t];
				if (target != null)
				{
					if (!target.IsFixedSize && !target.IsReadOnly)
					{
						// Dynamically adjust IList length
						while (target.Count < curValue)
							target.Add(elementType.IsValueType ? ReflectionHelper.CreateInstanceOf(elementType) : null);
						while (target.Count > curValue)
							target.RemoveAt(target.Count - 1);
					}
					else if (target is Array)
					{
						// Create new array that replaces the old one
						Array newTarget = Array.CreateInstance(elementType, curValue);
						for (int i = 0; i < Math.Min(curValue, target.Count); i++) newTarget.SetValue(target[i], i);
						targetArray[t] = newTarget;
						writeBack = true;
					}
					else
					{
						// Just some read-only container? Well, can't do anything here.
					}
				}
				if (valuesEnum.MoveNext()) curValue = (uint)valuesEnum.Current;
			}
			if (writeBack || this.ForceWriteBack) this.SetValues(targetArray);
			this.PerformGetValue();
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
		protected Func<IEnumerable<object>> CreateElementValueGetter(PropertyInfo indexer, int index)
		{
			return () => this.GetValue().Select(o => o != null ? indexer.GetValue(o, new object[] {index}) : null);
		}
		protected Action<IEnumerable<object>> CreateElementValueSetter(PropertyInfo indexer, int index)
		{
			return delegate(IEnumerable<object> values)
			{
				object[] targetArray = this.GetValue().ToArray();
				this.listIndexSetter(indexer, targetArray, values, index);
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
				IList newCollection = null;

				newCollection = this.ParentGrid.CreateObjectInstance(this.EditedType) as IList;

				if (newCollection == null)
				{
					Type elementType = this.GetElementType();
					Type listType = elementType != null ? elementType.MakeArrayType() : null;
					if (listType != null && this.EditedType.IsAssignableFrom(listType))
						newCollection = this.ParentGrid.CreateObjectInstance(listType) as IList;
				}

				if (newCollection != null)
				{
					this.SetValue(newCollection);
					this.Expanded = true;
				}
			}
			else
			{
				this.SetValue(null);
			}

			this.PerformGetValue();
		}
		protected internal override void ConfigureEditor(object configureData)
		{
			base.ConfigureEditor(configureData);
		}

		protected static void DefaultPropertySetter(PropertyInfo property, IEnumerable<object> targetObjects, IEnumerable<object> values, int index)
		{
			IEnumerator<object> valuesEnum = values.GetEnumerator();
			object curValue = null;

			if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
			foreach (object target in targetObjects)
			{
				if (target != null) property.SetValue(target, curValue, new object[] { index });
				if (valuesEnum.MoveNext()) curValue = valuesEnum.Current;
			}
		}
	}
}
