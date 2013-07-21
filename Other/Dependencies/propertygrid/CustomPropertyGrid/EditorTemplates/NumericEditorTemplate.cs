using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

using AdamsLair.PropertyGrid.Renderer;
using AdamsLair.PropertyGrid.EmbeddedResources;
using ButtonState = AdamsLair.PropertyGrid.Renderer.ButtonState;

namespace AdamsLair.PropertyGrid.EditorTemplates
{
	public class NumericEditorTemplate : EditorTemplate
	{
		public const int GripSize = 11;

		private static IconImage gripIcon = new IconImage(Resources.NumberGripIcon);

		private	bool					isTextValid		= false;
		private	bool					isValueClamped	= false;
		private	decimal					value			= decimal.MinValue;
		private	decimal					min;
		private	decimal					max;
		private	decimal					increment;
		private	int						decimalPlaces;
		private	StringEditorTemplate	stringEditor	= null;
		private	Rectangle				gripRect		= Rectangle.Empty;
		private	bool					gripHovered		= false;
		private	bool					gripPressed		= false;
		private	Point					gripDragPos		= Point.Empty;
		private	decimal					gripDragVal		= 0m;

		public override Rectangle Rect
		{
			get { return base.Rect; }
			set
			{
				if (this.rect != value)
				{
					base.Rect = value;
					this.gripRect = new Rectangle(
						this.rect.Right - GripSize + 2,
						this.rect.Y,
						GripSize,
						this.rect.Height);
					this.stringEditor.Rect = new Rectangle(
						this.rect.X, 
						this.rect.Y, 
						this.rect.Width - GripSize + 2, 
						this.rect.Height);
				}
			}
		}
		public override bool ReadOnly
		{
			get { return base.ReadOnly; }
			set
			{
				base.ReadOnly = value;
				this.stringEditor.ReadOnly = value;
			}
		}
		public int DecimalPlaces
		{
			get { return this.decimalPlaces; }
			set
			{
				value = Math.Max(Math.Min(value, 10), 0);
				if (this.decimalPlaces != value)
				{
					this.decimalPlaces = value;
					this.SetTextFromValue();
				}
			}
		}
		public decimal Maximum
		{
			get { return this.max; }
			set
			{
				if (this.max != value)
				{
					this.max = value;
					if (this.value > this.max) this.Value = this.max;
				}
			}
		}
		public decimal Minimum
		{
			get { return this.min; }
			set
			{
				if (this.min != value)
				{
					this.min = value;
					if (this.value > this.min) this.Value = this.min;
				}
			}
		}
		public decimal Increment
		{
			get { return this.increment; }
			set { this.increment = value; }
		}
		public decimal Value
		{
			get { return this.value; }
			set
			{
				if (this.stringEditor.Focused && !this.stringEditor.SelectedAll) return; // Don't override while the user is typing
				value = Math.Max(Math.Min(value, this.max), this.min);
				if (this.value != value)
				{
					this.value = value;
					this.SetTextFromValue();
				}
			}
		}


		public NumericEditorTemplate(PropertyEditor parent) : base(parent)
		{
			this.stringEditor = new StringEditorTemplate(parent);
			this.stringEditor.Invalidate += this.ForwardInvalidate;
			this.stringEditor.Edited += this.stringEditor_Edited;
			this.stringEditor.EditingFinished += this.stringEditor_EditingFinished;

			this.ResetProperties();
			this.Value = 0;
		}

		
		public void ResetProperties()
		{
			this.min = decimal.MinValue;
			this.max = decimal.MaxValue;
			this.increment = 1;
			this.decimalPlaces = 0;

			this.Value = this.value;
		}
		public void Select()
		{
			this.stringEditor.Select();
			this.stringEditor.UpdateScroll();
		}

		public void OnPaint(PaintEventArgs e, bool enabled, bool multiple)
		{
			this.stringEditor.OnPaint(e, enabled, multiple);

			ButtonState gripState = ButtonState.Normal;
			if (!enabled || this.ReadOnly)
				gripState = ButtonState.Disabled;
			else if (this.gripPressed || (this.focused && Control.ModifierKeys.HasFlag(Keys.Control)))
				gripState = ButtonState.Pressed;
			else if (this.gripHovered || this.stringEditor.Focused)
				gripState = ButtonState.Hot;
			Rectangle gfxGripRect = new Rectangle(this.gripRect.X - 1, this.gripRect.Y, this.gripRect.Width, this.gripRect.Height);
			this.parent.ControlRenderer.DrawButton(e.Graphics, gfxGripRect, gripState, null, (enabled && !this.ReadOnly) ? gripIcon.Normal : gripIcon.Disabled);
		}

		public override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.stringEditor.OnGotFocus(e);
		}
		public override void OnLostFocus(EventArgs e)
		{
			if (this.focused && !this.stringEditor.Focused) this.EmitEditingFinished(this.value, FinishReason.LostFocus);			
			base.OnLostFocus(e);
			this.stringEditor.OnLostFocus(e);
		}
		public void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ') return; // Don't handle spaces in pure numeric editor

			KeyPressEventArgs subE = e;
			if (e.KeyChar == ',') subE = new KeyPressEventArgs('.'); // Use dots instead of commas
			this.stringEditor.OnKeyPress(subE);
			if (subE.Handled) e.Handled = true;
		}
		public void OnKeyDown(KeyEventArgs e)
		{
			this.stringEditor.OnKeyDown(e);
			if (!this.ReadOnly)
			{
				if (e.KeyCode == Keys.ControlKey)
				{
					this.EmitInvalidate();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					this.Value += this.increment;
					this.EmitEdited(this.value);
					e.Handled = true;
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					this.Value -= this.increment;
					this.EmitEdited(this.value);
					e.Handled = true;
				}
			}
		}
		public void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ControlKey)
			{
				this.EmitInvalidate();
				this.EmitEditingFinished(this.value, FinishReason.LeapValue);
			}
		}
		public void OnMouseDown(MouseEventArgs e)
		{
			if (!this.rect.Contains(e.Location)) return;
			this.stringEditor.OnMouseDown(e);
			if (this.gripHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.gripPressed = true;
				this.gripDragPos = e.Location;
				this.gripDragVal = this.value;
				this.stringEditor.Select();
				this.stringEditor.UpdateScroll();
				this.EmitInvalidate();
			}
		}
		public void OnMouseUp(MouseEventArgs e)
		{
			this.stringEditor.OnMouseUp(e);
			if (this.gripPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.gripPressed = false;
				this.gripDragPos = Point.Empty;
				this.gripDragVal = 0m;
				this.EmitInvalidate();
				this.EmitEditingFinished(this.value, FinishReason.LeapValue);
			}
		}
		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.stringEditor.OnMouseMove(e);

			bool lastGripHovered = this.gripHovered;
			this.gripHovered = !this.ReadOnly && this.gripRect.Contains(e.Location);
			if (lastGripHovered != this.gripHovered) this.EmitInvalidate();

			if (this.gripPressed)
			{
				decimal lastVal = this.value;

				this.Value = this.gripDragVal - this.increment * Math.Round((e.Location.Y - this.gripDragPos.Y) / 3m);

				if (lastVal != this.value) this.EmitEdited(this.value);
			}
		}
		public override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.stringEditor.OnMouseLeave(e);

			if (this.gripHovered) this.EmitInvalidate();
			this.gripHovered = false;
		}

		protected void SetTextFromValue()
		{
			if (this.decimalPlaces > 0)
			{
				decimal beforeSep = this.value >= 0m ? Math.Floor(this.value) : Math.Ceiling(this.value);
				decimal afterSep = Math.Abs(this.value - beforeSep);
				decimal placesMult = (decimal)Math.Pow(10.0d, this.decimalPlaces);
				beforeSep = Math.Abs(beforeSep);
				afterSep = Math.Round(afterSep * placesMult);
				while (afterSep >= placesMult)
				{
					beforeSep++;
					afterSep -= placesMult;
				}
				this.stringEditor.Text = (this.value < 0m ? "-" : "") + beforeSep.ToString() + "." + afterSep.ToString().PadLeft(this.decimalPlaces, '0');
			}
			else
			{
				this.stringEditor.Text = Math.Round(this.value).ToString();
			}
			this.isTextValid = true;
			this.isValueClamped = false;
		}
		protected void SetValueFromText()
		{
			decimal valResult;
			this.isTextValid = decimal.TryParse(this.stringEditor.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out valResult);
			if (!this.isTextValid && string.IsNullOrWhiteSpace(this.stringEditor.Text))
			{
				this.isTextValid = true;
				valResult = 0m;
			}

			if (this.isTextValid)
			{
				this.value = Math.Max(Math.Min(valResult, this.max), this.min);
				this.isValueClamped = this.value != valResult;
			}
			else
			{
				this.isValueClamped = false;
			}
		}
		
		private void ForwardInvalidate(object sender, EventArgs e)
		{
			this.EmitInvalidate();
		}
		private void stringEditor_Edited(object sender, EventArgs e)
		{
			this.SetValueFromText();
			if (this.isTextValid)
				this.EmitEdited(this.value);
			else
				this.EmitInvalidate();
		}
		void stringEditor_EditingFinished(object sender, PropertyEditingFinishedEventArgs e)
		{
			this.Select();
			if (!this.isTextValid || this.isValueClamped) System.Media.SystemSounds.Beep.Play();
			if (this.isTextValid) this.EmitEditingFinished(this.value, e.Reason);
			this.SetTextFromValue();
			this.EmitInvalidate();
		}
	}
}
