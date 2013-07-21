using System;
using System.Globalization;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;

using Duality;
using Duality.EditorHints;

namespace DualityEditor.Controls
{
	public class EnumComboBox : ComboBox
	{
		private Type	enumType				= null;

		public event EventHandler EnumValueChanged = null;

		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		public Enum EnumValue
		{
			get
			{
				return this.SelectedItem != null ? (Enum)Enum.Parse(this.enumType, (string)this.SelectedItem) : (Enum)this.enumType.GetDefaultInstanceOf();
			}
			set
			{
				Type valEnumType = value.GetType();
                if (this.enumType != valEnumType)
				{
					this.Items.Clear();
					this.enumType = valEnumType; // Store enum type
					this.FillEnumMembers(); // Add items for enum members
				}
				this.SelectedItem = Enum.GetName(enumType, value);
				this.OnEnumValueChanged();
			}
		}
		
		public EnumComboBox()
		{
			this.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		/// <summary>
		/// Adds items to the checklistbox based on the members of the enum
		/// </summary>
		private void FillEnumMembers()
		{
			foreach (string name in Enum.GetNames(enumType))
			{
				var field = enumType.GetField(name, ReflectionHelper.BindAll);
				EditorHintFlagsAttribute flagsAttrib = field.GetCustomAttributes(typeof(EditorHintFlagsAttribute), true).FirstOrDefault() as EditorHintFlagsAttribute;
				if (flagsAttrib != null && (flagsAttrib.Flags & MemberFlags.Invisible) != MemberFlags.None) continue;

				this.Items.Add(name);
			}
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			this.OnEnumValueChanged();
		}
		protected void OnEnumValueChanged()
		{
			if (this.EnumValueChanged != null)
				this.EnumValueChanged(this, null);
		}
	}
}
