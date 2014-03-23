using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using AdamsLair.PropertyGrid.Renderer;

namespace AdamsLair.PropertyGrid.PropertyEditors
{
	public class BoolPropertyEditor : PropertyEditor
	{
		private	CheckState	state	= CheckState.Unchecked;
		private	bool		hovered	= false;
		private	bool		pressed	= false;

		public override object DisplayedValue
		{
			get { return Convert.ChangeType(this.state == CheckState.Checked, this.EditedType); }
		}

		public BoolPropertyEditor()
		{
			//this.Height = 16;
		}
		protected override void OnParentEditorChanged()
		{
			base.OnParentEditorChanged();
			this.Height = Math.Max(13, 3 + (int)Math.Round((float)this.ControlRenderer.DefaultFont.Height));
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			this.BeginUpdate();
			CheckState lastState = this.state;
			object[] values = this.GetValue().ToArray();

			// Apply values to editors
			if (!values.Any())
				this.state = CheckState.Unchecked;
			else
			{
				int trueCount = values.Count(o => o != null && (bool)Convert.ToBoolean(o));
				if (!this.ReadOnly && (values.Any(o => o == null) || (trueCount > 0 && trueCount < values.Count())))
					this.state = CheckState.Indeterminate;
				else
					this.state = trueCount > 0 ? CheckState.Checked : CheckState.Unchecked;
			}

			this.EndUpdate();
			if (this.state != lastState) this.Invalidate();
		}
		protected override void OnSetValue()
		{
			if (this.state == CheckState.Indeterminate) return;
			base.OnSetValue();
		}

		protected void ToggleState()
		{
			if (this.state == CheckState.Checked)
				this.state = CheckState.Unchecked;
			else
				this.state = CheckState.Checked;

			this.Invalidate();
			this.PerformSetValue();
			this.PerformGetValue();
			this.OnEditingFinished(FinishReason.LeapValue);
		}
		protected void SetState(bool value)
		{
			this.state = value ? CheckState.Checked : CheckState.Unchecked;

			this.Invalidate();
			this.PerformSetValue();
			this.PerformGetValue();
			this.OnEditingFinished(FinishReason.LeapValue);
		}

		protected internal override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			CheckBoxState boxState = CheckBoxState.UncheckedNormal;
			bool boxEnabled = this.Enabled && !this.ReadOnly;
			bool hot = this.hovered | this.Focused;
			if (this.state == CheckState.Checked)
			{
				if (!boxEnabled)		boxState = CheckBoxState.CheckedDisabled;
				else if (this.pressed)	boxState = CheckBoxState.CheckedPressed;
				else if (hot)			boxState = CheckBoxState.CheckedHot;
				else					boxState = CheckBoxState.CheckedNormal;				
			}
			else if (this.state == CheckState.Unchecked)
			{
				if (!boxEnabled)		boxState = CheckBoxState.UncheckedDisabled;
				else if (this.pressed)	boxState = CheckBoxState.UncheckedPressed;
				else if (hot)			boxState = CheckBoxState.UncheckedHot;
				else					boxState = CheckBoxState.UncheckedNormal;	
			}
			else if (this.state == CheckState.Indeterminate)
			{
				if (!boxEnabled)		boxState = CheckBoxState.MixedDisabled;
				else if (this.pressed)	boxState = CheckBoxState.MixedPressed;
				else if (hot)			boxState = CheckBoxState.MixedHot;
				else					boxState = CheckBoxState.MixedNormal;	
			}
			
			Size boxSize = ControlRenderer.CheckBoxSize;
			Point boxLoc = new Point(
				this.ClientRectangle.X + 2,
				this.ClientRectangle.Y + this.ClientRectangle.Height / 2 - boxSize.Height / 2);
			ControlRenderer.DrawCheckBox(e.Graphics, boxLoc, boxState);
		}
		protected internal override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			bool lastHovered = this.hovered;
			this.hovered = !this.ReadOnly && this.ClientRectangle.Contains(e.Location);
			if (lastHovered != this.hovered) this.Invalidate();
		}
		protected internal override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.hovered) this.Invalidate();
			this.hovered = false;
			this.pressed = false;
		}
		protected internal override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.hovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				if (!this.pressed) this.Invalidate();
				this.pressed = true;
			}
		}
		protected internal override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ((e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				if (this.hovered && this.pressed) this.ToggleState();
				if (this.pressed) this.Invalidate();
				this.pressed = false;
			}
		}
		protected internal override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Return)
			{
				this.ToggleState();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Delete)
			{
				this.SetState(false);
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.C)
			{
				Clipboard.SetText((this.state == CheckState.Checked).ToString());
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.V)
			{
				bool temp;
				if (bool.TryParse(Clipboard.GetText(), out temp))
					this.SetState(temp);
				e.Handled = true;
			}
		}
	}
}
