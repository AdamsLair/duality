using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;

using Duality;
using Duality.EditorHints;

namespace DualityEditor.Controls
{
	public class EnumFlagCheckedListBox : CustomFlagCheckedListBox
	{
		private Type	enumType				= null;
		private Enum	enumValue				= default(Enum);

		public event EventHandler EnumValueChanged = null;


		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		public Enum EnumValue
		{
			get
			{
				return this.enumValue;
			}
			set
			{
				this.enumValue = value;
				if (this.enumType != value.GetType())
				{
					this.enumType = value.GetType();
					this.Items.Clear();
					this.FillEnumMembers();
				}
				this.FlagValue = (ulong)Convert.ChangeType(this.enumValue, typeof(ulong));
			}
		}

		protected override void OnFlagValueChanged()
		{
			base.OnFlagValueChanged();
			this.enumValue = (Enum)Enum.ToObject(this.enumType, this.FlagValue);
			this.OnEnumValueChanged();
		}
		protected void OnEnumValueChanged()
		{
			if (this.EnumValueChanged != null)
				this.EnumValueChanged(this, null);
		}
		
		private void FillEnumMembers()
		{
			foreach (string name in Enum.GetNames(enumType))
			{
				var field = enumType.GetField(name, ReflectionHelper.BindAll);
				EditorHintFlagsAttribute flagsAttrib = field.GetCustomAttributes(typeof(EditorHintFlagsAttribute), true).FirstOrDefault() as EditorHintFlagsAttribute;
				if (flagsAttrib != null && (flagsAttrib.Flags & MemberFlags.Invisible) != MemberFlags.None) continue;

				object val = Enum.Parse(enumType,name);
				ulong ulongVal = (ulong)Convert.ChangeType(val, typeof(ulong));

				Add(ulongVal,name);
			}
		}
	}

	public class CustomFlagCheckedListBox : CheckedListBox
	{
		private bool	isUpdatingCheckStates	= false;
		private ulong	flagValue				= 0;

		public event EventHandler FlagValueChanged = null;


		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		public ulong FlagValue
		{
			get { return this.flagValue; }
			set
			{
				this.flagValue = value;
				this.UpdateCheckedItems();
				this.OnFlagValueChanged();
			}
		}


		public CustomFlagCheckedListBox()
		{
			this.CheckOnClick = true;
		}
		public FlagCheckedListBoxItem Add(ulong v, string c)
		{
			FlagCheckedListBoxItem item = new FlagCheckedListBoxItem(v, c);
			this.Items.Add(item);
			return item;
		}

		protected ulong GetCurrentValue()
		{
			ulong sum = 0;

			for (int i = 0; i < this.Items.Count; i++)
			{
				FlagCheckedListBoxItem item = this.Items[i] as FlagCheckedListBoxItem;

				if (this.GetItemChecked(i))
					sum |= item.Value;
			}

			return sum;
		}
		protected void UpdateCheckedItems()
		{
			this.isUpdatingCheckStates = true;

			// Iterate over all items
			for (int i = 0; i < this.Items.Count; i++)
			{
				FlagCheckedListBoxItem item = this.Items[i] as FlagCheckedListBoxItem;

				if (item.Value == 0)	this.SetItemChecked(i, flagValue == 0);
				else					this.SetItemChecked(i, (item.Value & flagValue) == item.Value && item.Value != 0);
			}

			this.isUpdatingCheckStates = false;
		}

		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			base.OnItemCheck(e);
			if (this.isUpdatingCheckStates) return;

			FlagCheckedListBoxItem item = this.Items[e.Index] as FlagCheckedListBoxItem;
			
			if (item.Value == 0)
				this.flagValue = 0;
			else if (e.NewValue == CheckState.Checked)
				this.flagValue |= item.Value;
			else if (e.NewValue == CheckState.Unchecked)
				this.flagValue &= ~item.Value;

			this.UpdateCheckedItems();
			this.OnFlagValueChanged();
		}
		protected virtual void OnFlagValueChanged()
		{
			if (this.FlagValueChanged != null)
				this.FlagValueChanged(this, null);
		}
	}

	/// <summary>
	/// Represents an item in the checklistbox
	/// </summary>
	public class FlagCheckedListBoxItem
	{
		private ulong	value	= 0;
		private string	caption	= null;

		public string Caption
		{
			get { return this.caption; }
		}
		public ulong Value
		{
			get { return this.value; }
		}

		public FlagCheckedListBoxItem(ulong v, string c)
		{
			this.value = v;
			this.caption = c;
		}

		public override string ToString()
		{
			return this.caption;
		}
	}
}
