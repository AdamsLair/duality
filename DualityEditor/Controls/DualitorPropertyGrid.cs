using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using AdamsLair.WinForms.PropertyEditing;
using AdamsLair.WinForms.PropertyEditing.Editors;
using PropertyGrid = AdamsLair.WinForms.PropertyEditing.PropertyGrid;

using Duality;
using Duality.Editor;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Forms;

namespace Duality.Editor.Controls
{
	public class DualitorPropertyGrid : PropertyGrid, IHelpProvider
	{
		public DualitorPropertyGrid()
		{
			//this.ControlRenderer.FocusBrightnessScale = 1.15f;
			this.ControlRenderer.ColorMultiple = Color.FromArgb(242, 212, 170);
			this.ControlRenderer.ColorGrayText = Color.FromArgb(96, 96, 96);
			this.ControlRenderer.ColorVeryLightBackground = Color.FromArgb(224, 224, 224);
			this.ControlRenderer.ColorLightBackground = Color.FromArgb(212, 212, 212);
			this.ControlRenderer.ColorBackground = Color.FromArgb(196, 196, 196);
			this.RegisterEditorProvider(new PropertyEditors.DualityPropertyEditorProvider());
		}

		public override void ConfigureEditor(PropertyEditor editor, object configureData = null)
		{
			IEnumerable<EditorHintAttribute> hintOverride = configureData as IEnumerable<EditorHintAttribute>;
			IEnumerable<EditorHintAttribute> parentHint = null;
			if (editor.ParentEditor != null)
			{
				IEnumerable<EditorHintAttribute> parentHintOverride = editor.ParentEditor.ConfigureData as IEnumerable<EditorHintAttribute>;
				if (editor.ParentEditor.EditedMember != null)
					parentHint = EditorHintAttribute.GetAll<EditorHintAttribute>(editor.ParentEditor.EditedMember, parentHintOverride);
				else
					parentHint = parentHintOverride;
			}

			if (hintOverride == null && parentHint != null)
			{
				// No configuration data available? Allow to derive certain types from parent list or dictionary.
				if (editor.ParentEditor is IListPropertyEditor || editor.ParentEditor is IDictionaryPropertyEditor)
				{
					hintOverride = parentHint.Where(a => 
						a is EditorHintDecimalPlacesAttribute ||
						a is EditorHintIncrementAttribute ||
						a is EditorHintRangeAttribute);
				}
				// That way we can specify the decimal places of an array of Vector2-structs and actually change those Vector2 editors.
			}

			// Invoke the PropertyEditor's configure method
			base.ConfigureEditor(editor, hintOverride);

			// Do some final configuration for editors that do not behave as intended by default.
			if (editor is MemberwisePropertyEditor)
			{
				MemberwisePropertyEditor memberEditor = editor as MemberwisePropertyEditor;
				memberEditor.MemberPredicate = this.EditorMemberPredicate;
				memberEditor.MemberAffectsOthers = this.EditorMemberAffectsOthers;
				memberEditor.MemberPropertySetter = this.EditorMemberPropertySetter;
				memberEditor.MemberFieldSetter = this.EditorMemberFieldSetter;
			}
			if (editor is IListPropertyEditor)
			{
				IListPropertyEditor listEditor = editor as IListPropertyEditor;
				listEditor.ListIndexSetter = this.EditorListIndexSetter;
			}
			if (editor is IDictionaryPropertyEditor)
			{
				IDictionaryPropertyEditor dictEditor = editor as IDictionaryPropertyEditor;
				dictEditor.DictionaryKeySetter = this.EditorDictionaryKeySetter;
			}

			var flagsAttrib = EditorHintAttribute.Get<EditorHintFlagsAttribute>(editor.EditedMember, hintOverride);
			if (flagsAttrib != null)
			{
				editor.ForceWriteBack = (flagsAttrib.Flags & MemberFlags.ForceWriteback) == MemberFlags.ForceWriteback;
				if ((flagsAttrib.Flags & MemberFlags.ReadOnly) == MemberFlags.ReadOnly)
					editor.Setter = null;
			}

			if (editor is NumericPropertyEditor)
			{
				var rangeAttrib = EditorHintAttribute.Get<EditorHintRangeAttribute>(editor.EditedMember, hintOverride);
				var incAttrib = EditorHintAttribute.Get<EditorHintIncrementAttribute>(editor.EditedMember, hintOverride);
				var placesAttrib = EditorHintAttribute.Get<EditorHintDecimalPlacesAttribute>(editor.EditedMember, hintOverride);
				NumericPropertyEditor numEditor = editor as NumericPropertyEditor;
				if (rangeAttrib != null)
				{
					numEditor.ValueBarMaximum = rangeAttrib.ReasonableMaximum;
					numEditor.ValueBarMinimum = rangeAttrib.ReasonableMinimum;
					numEditor.Maximum = rangeAttrib.LimitMaximum;
					numEditor.Minimum = rangeAttrib.LimitMinimum;
				}
				if (incAttrib != null) numEditor.Increment = incAttrib.Increment;
				if (placesAttrib != null) numEditor.DecimalPlaces = placesAttrib.Places;
			}
		}
		protected override void PrepareSetValue()
		{
			base.PrepareSetValue();
			UndoRedoManager.BeginMacro();
		}
		protected override void PostSetValue()
		{
			base.PostSetValue();
			UndoRedoManager.EndMacro(UndoRedoManager.MacroDeriveName.FromFirst);
		}
		protected override void OnEditingFinished(PropertyEditorValueEventArgs e)
		{
			base.OnEditingFinished(e);
			UndoRedoManager.Finish();
		}
		public override object CreateObjectInstance(Type objectType)
		{
			object obj = null;

			if (objectType.IsAbstract || objectType.IsInterface || objectType == typeof(object))
			{
				CreateObjectDialog createDialog = new CreateObjectDialog();
				createDialog.BaseType = objectType;
				createDialog.ShowNamespaces = objectType == typeof(object);
				DialogResult result = createDialog.ShowDialog();
				if (result == DialogResult.OK)
				{
					obj = createDialog.SelectedType.CreateInstanceOf();
				}
			}

			return obj ?? base.CreateObjectInstance(objectType);
		}

		private bool EditorMemberPredicate(MemberInfo info, bool showNonPublic)
		{
			PropertyInfo property = info as PropertyInfo;
			FieldInfo field = info as FieldInfo;
			EditorHintFlagsAttribute flagsAttrib = info.GetCustomAttributes<EditorHintFlagsAttribute>().FirstOrDefault();

			// Accept all members in "Debug Mode", if not declared inside Duality itself
			if (showNonPublic && info.DeclaringType.Assembly != typeof(DualityApp).Assembly) return true;

			// Accept explicitly visible members
			if (flagsAttrib != null && (flagsAttrib.Flags & MemberFlags.Visible) != MemberFlags.None) return true;
			
			// Reject properties with non-public getters
			if (property != null)
			{
				MethodInfo getter = property.GetGetMethod(true);
				if (getter == null) return false;
				if (!getter.IsPublic) return false;
			}

			// Reject non-public fields
			if (field != null && !field.IsPublic) return false;

			// Reject even public fields of a Component - we don't want to encourage using them, since Duality basically works with Properties.
			if (field != null && typeof(Component).IsAssignableFrom(info.DeclaringType)) return false;

			// Reject explicitly invisible members
			if (flagsAttrib != null && (flagsAttrib.Flags & MemberFlags.Invisible) != MemberFlags.None) return false;

			return true;
		}
		private bool EditorMemberAffectsOthers(MemberInfo info)
		{
			EditorHintFlagsAttribute flagsAttrib = info.GetCustomAttributes<EditorHintFlagsAttribute>().FirstOrDefault();
			return this.ShowNonPublic || (flagsAttrib != null && (flagsAttrib.Flags & MemberFlags.AffectsOthers) != MemberFlags.None);
		}
		private void EditorMemberPropertySetter(PropertyInfo property, IEnumerable<object> targetObjects, IEnumerable<object> values)
		{
			UndoRedoManager.Do(new EditPropertyAction(this, property, targetObjects, values));
		}
		private void EditorMemberFieldSetter(FieldInfo field, IEnumerable<object> targetObjects, IEnumerable<object> values)
		{
			UndoRedoManager.Do(new EditFieldAction(this, field, targetObjects, values));
		}
		private void EditorListIndexSetter(PropertyInfo indexer, IEnumerable<object> targetObjects, IEnumerable<object> values, int index)
		{
			UndoRedoManager.Do(new EditPropertyAction(this, indexer, targetObjects, values, new object[] {index}));
		}
		private void EditorDictionaryKeySetter(PropertyInfo indexer, IEnumerable<object> targetObjects, IEnumerable<object> values, object key)
		{
			UndoRedoManager.Do(new EditPropertyAction(this, indexer, targetObjects, values, new object[] {key}));
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			// A dropdown is opened. Provide dropdown help.
			IPopupControlHost dropdownEdit = this.FocusEditor as IPopupControlHost;
			if (dropdownEdit != null && dropdownEdit.IsDropDownOpened)
			{
				EnumPropertyEditor enumEdit = dropdownEdit as EnumPropertyEditor;
				FlaggedEnumPropertyEditor enumFlagEdit = dropdownEdit as FlaggedEnumPropertyEditor;
				ObjectSelectorPropertyEditor objectSelectorEdit = dropdownEdit as ObjectSelectorPropertyEditor;

				// Its able to provide help. Redirect.
				if (dropdownEdit is IHelpProvider)
				{
					captured = true;
					Point dropdownEditorPos = this.GetEditorLocation(dropdownEdit as PropertyEditor, true);
					return (dropdownEdit as IHelpProvider).ProvideHoverHelp(new Point(localPos.X - dropdownEditorPos.X, localPos.Y - dropdownEditorPos.Y), ref captured);
				}
				// Special case: Its a known basic dropdown.
				else if (enumEdit != null)
				{
					captured = true;
					if (enumEdit.DropDownHoveredName != null)
						return HelpInfo.FromMember(enumEdit.EditedType.GetField(enumEdit.DropDownHoveredName, ReflectionHelper.BindAll));
					else
					{
						FieldInfo field = enumEdit.EditedType.GetField(enumEdit.DisplayedValue.ToString(), ReflectionHelper.BindAll);
						if (field != null) return HelpInfo.FromMember(field);
					}
				}
				else if (enumFlagEdit != null)
				{
					captured = true;
					if (enumFlagEdit.DropDownHoveredItem != null)
						return HelpInfo.FromMember(enumFlagEdit.EditedType.GetField(enumFlagEdit.DropDownHoveredItem.Caption, ReflectionHelper.BindAll));
					else
					{
						FieldInfo field = enumFlagEdit.EditedType.GetField(enumFlagEdit.DisplayedValue.ToString(), ReflectionHelper.BindAll);
						if (field != null) return HelpInfo.FromMember(field);
					}
				}
				else if (objectSelectorEdit != null)
				{
					captured = true;
					if (objectSelectorEdit.DropDownHoveredObject != null)
						return HelpInfo.FromObject(objectSelectorEdit.DropDownHoveredObject.Value);
					else
						return HelpInfo.FromObject(objectSelectorEdit.DisplayedValue);
				}

				// No help available.
				return null;
			}
			captured = false;

			// Pick an editor and see if it has access to an actual IHelpProvider
			PropertyEditor pickedEditor = this.PickEditorAt(localPos.X, localPos.Y, true);
			PropertyEditor helpEditor = pickedEditor;
			while (helpEditor != null)
			{
				Point helpEditorPos = this.GetEditorLocation(helpEditor, true);
				if (helpEditor is IHelpProvider)
				{
					IHelpProvider localProvider = helpEditor as IHelpProvider;
					HelpInfo localHelp = localProvider.ProvideHoverHelp(new Point(localPos.X - helpEditorPos.X, localPos.Y - helpEditorPos.Y), ref captured);
					if (localHelp != null)
						return localHelp;
				}
				helpEditor = helpEditor.ParentEditor;
			}

			// If not, default to member or type information
			if (pickedEditor != null)
			{
				if (!string.IsNullOrEmpty(pickedEditor.PropertyDesc))
					return HelpInfo.FromText(pickedEditor.PropertyName, pickedEditor.PropertyDesc);
				else if (pickedEditor.EditedMember != null)
					return HelpInfo.FromMember(pickedEditor.EditedMember);
				else if (pickedEditor.EditedType != null)
					return HelpInfo.FromMember(pickedEditor.EditedType);
			}

			return null;
		}
	}
}
