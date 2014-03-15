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
		private	decimal					limitMin;
		private	decimal					limitMax;
		private	decimal					barMin			= decimal.MinValue;
		private	decimal					barMax			= decimal.MaxValue;
		private	decimal					increment;
		private	int						decimalPlaces;
		private	StringEditorTemplate	stringEditor	= null;
		private	Rectangle				gripRect		= Rectangle.Empty;
		private	Rectangle				minMaxRect		= Rectangle.Empty;
		private	bool					showMinMax		= false;
		private	bool					barHovered		= false;
		private	bool					barPressed		= false;
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
					this.UpdateGeometry();
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
		public bool ShowMinMaxBar
		{
			get { return this.showMinMax; }
			set
			{
				if (this.showMinMax != value)
				{
					this.showMinMax = value;
					this.UpdateGeometry();
				}
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
			get { return this.limitMax; }
			set
			{
				if (this.limitMax != value)
				{
					this.limitMax = value;
					if (this.barMax > this.limitMax) this.ValueBarMaximum = this.limitMax;
					if (this.value > this.limitMax) this.Value = this.limitMax;
				}
			}
		}
		public decimal Minimum
		{
			get { return this.limitMin; }
			set
			{
				if (this.limitMin != value)
				{
					this.limitMin = value;
					if (this.barMin < this.limitMin) this.ValueBarMinimum = this.limitMin;
					if (this.value < this.limitMin) this.Value = this.limitMin;
				}
			}
		}
		public decimal ValueBarMaximum
		{
			get { return this.barMax; }
			set
			{
				if (this.barMax != value)
				{
					this.barMax = value;
					if (this.showMinMax) this.EmitInvalidate();
				}
			}
		}
		public decimal ValueBarMinimum
		{
			get { return this.barMin; }
			set
			{
				if (this.barMin != value)
				{
					this.barMin = value;
					if (this.showMinMax) this.EmitInvalidate();
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
				value = Math.Max(Math.Min(value, this.limitMax), this.limitMin);
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
			this.limitMin = decimal.MinValue;
			this.limitMax = decimal.MaxValue;
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

			if (!this.minMaxRect.IsEmpty)
			{
				Color minMaxBarColor = this.parent.ControlRenderer.ColorHightlight;
				if (multiple) minMaxBarColor = minMaxBarColor.MixWith(this.parent.ControlRenderer.ColorMultiple, 0.5f, true);
				if (!this.barHovered && !this.barPressed) minMaxBarColor = Color.FromArgb(128, minMaxBarColor);
				e.Graphics.FillRectangle(new SolidBrush(minMaxBarColor),
					this.minMaxRect.X,
					this.minMaxRect.Y,
					this.minMaxRect.Width * Math.Min(Math.Max((float)((this.value - this.barMin) / (this.barMax - this.barMin)), 0.0f), 1.0f),
					this.minMaxRect.Height);
			}
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
			else if (this.barHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				decimal lastVal = this.value;
				this.Value = this.barMin + (decimal)Math.Min(Math.Max((float)(e.Location.X - this.minMaxRect.X) / (float)this.minMaxRect.Width, 0.0f), 1.0f) * (this.barMax - this.barMin);
				if (lastVal != this.value) this.EmitEdited(this.value);

				this.barPressed = true;
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
			else if (this.barPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.barPressed = false;
				this.EmitInvalidate();
				this.EmitEditingFinished(this.value, FinishReason.LeapValue);
			}
		}
		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.stringEditor.OnMouseMove(e);

			bool lastGripHovered = this.gripHovered;
			bool lastBarHovered = this.barHovered;
			this.gripHovered = !this.ReadOnly && this.gripRect.Contains(e.Location);
			this.barHovered = !this.ReadOnly && this.minMaxRect.Contains(e.Location);

			if (lastGripHovered != this.gripHovered || lastBarHovered != this.barHovered)
				this.EmitInvalidate();

			if (this.gripPressed)
			{
				decimal lastVal = this.value;
				this.Value = this.gripDragVal - this.increment * Math.Round((e.Location.Y - this.gripDragPos.Y) / 3m);
				if (lastVal != this.value) this.EmitEdited(this.value);
			}
			else if (this.barPressed)
			{
				decimal lastVal = this.value;
				this.Value = this.barMin + (decimal)Math.Min(Math.Max((float)(e.Location.X - this.minMaxRect.X) / (float)this.minMaxRect.Width, 0.0f), 1.0f) * (this.barMax - this.barMin);
				if (lastVal != this.value) this.EmitEdited(this.value);
			}
		}
		public override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.stringEditor.OnMouseLeave(e);

			if (this.gripHovered ||this.barHovered) this.EmitInvalidate();
			this.gripHovered = false;
			this.barHovered = false;
		}

		protected void UpdateGeometry()
		{
			if (this.ShowMinMaxBar)
			{
				this.minMaxRect = new Rectangle(
					this.rect.X,
					this.rect.Bottom - 3,
					this.rect.Width,
					3);
			}
			else
			{
				this.minMaxRect = Rectangle.Empty;
			}
			this.gripRect = new Rectangle(
				this.rect.Right - GripSize + 2,
				this.rect.Y,
				GripSize,
				this.rect.Height - this.minMaxRect.Height);
			this.stringEditor.Rect = new Rectangle(
				this.rect.X, 
				this.rect.Y, 
				this.rect.Width - GripSize + 2, 
				this.rect.Height - this.minMaxRect.Height);
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
				this.value = Math.Max(Math.Min(valResult, this.limitMax), this.limitMin);
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
