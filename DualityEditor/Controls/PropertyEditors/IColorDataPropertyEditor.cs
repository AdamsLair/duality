using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

using AdamsLair.WinForms;
using ButtonState = AdamsLair.WinForms.Renderer.ButtonState;

using Duality;
using Duality.Drawing;

using Duality.Editor.Forms;

namespace Duality.Editor.Controls.PropertyEditors
{
	[PropertyEditorAssignment(typeof(IColorData))]
	public class IColorDataPropertyEditor : PropertyEditor
	{
		protected	ColorPickerDialog	dialog	= new ColorPickerDialog { BackColor = Color.FromArgb(212, 212, 212) };
		protected	IColorData	value			= null;
		protected	Rectangle	rectPanel		= Rectangle.Empty;
		protected	Rectangle	rectButton		= Rectangle.Empty;
		protected	bool		buttonHovered	= false;
		protected	bool		buttonPressed	= false;
		protected	bool		panelHovered	= false;
		private		Point		panelDragBegin	= Point.Empty;

		public override object DisplayedValue
		{
			get { return value.ConvertTo(this.EditedType); }
		}


		public IColorDataPropertyEditor()
		{
			this.Height = 22;
			this.dialog.ColorEdited += this.dialog_ColorEdited;
		}

		protected override void OnGetValue()
		{
			base.OnGetValue();
			IColorData[] values = this.GetValue().Cast<IColorData>().ToArray();

			this.BeginUpdate();
			int oldValue = this.value != null ? this.value.ToIntRgba() : -1;
			if (!values.Any())
			{
				this.value = ColorRgba.TransparentBlack;
			}
			else
			{
				this.value = (IColorData)values.NotNull().FirstOrDefault();

				// No visual appearance of "multiple values" yet - need one?
			}
			this.EndUpdate();
			if (oldValue != (this.value != null ? this.value.ToIntRgba() : -1)) this.Invalidate();
		}

		public void ShowColorDialog()
		{
			this.dialog.OldColor = (this.value ?? ColorRgba.TransparentBlack).ToSysDrawColor();
			this.dialog.SelectedColor = this.dialog.OldColor;
			DialogResult result = this.dialog.ShowDialog(this.ParentGrid);

			this.value = (result == DialogResult.OK) ? this.dialog.SelectedColor.ToDualityRgba() : this.dialog.OldColor.ToDualityRgba();
			this.PerformSetValue();
			this.PerformGetValue();
			this.OnEditingFinished(result == DialogResult.OK ? FinishReason.UserAccept : FinishReason.LostFocus);
		}
		private void dialog_ColorEdited(object sender, EventArgs e)
		{
			this.value = this.dialog.SelectedColor.ToDualityRgba();
			this.PerformSetValue();
			this.PerformGetValue();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			Color brightChecker = Color.FromArgb(224, 224, 224);
			Color darkChecker = Color.FromArgb(192, 192, 192);
			e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker), this.rectPanel);
			if (this.value != null)
			{
				Color val = Color.FromArgb(this.value.ToIntArgb());
				Color valSolid = Color.FromArgb(255, val);
				e.Graphics.FillRectangle(
					new SolidBrush(val), 
					this.rectPanel.X,
					this.rectPanel.Y,
					this.rectPanel.Width / 2,
					this.rectPanel.Height);
				e.Graphics.FillRectangle(
					new SolidBrush(valSolid), 
					this.rectPanel.X + this.rectPanel.Width / 2,
					this.rectPanel.Y,
					this.rectPanel.Width / 2,
					this.rectPanel.Height);
			}
			e.Graphics.DrawRectangle(SystemPens.ControlLightLight, 
				this.rectPanel.X + 1,
				this.rectPanel.Y + 1,
				this.rectPanel.Width - 2,
				this.rectPanel.Height - 2);
			e.Graphics.DrawRectangle(SystemPens.ControlDark, this.rectPanel);

			ButtonState buttonState = ButtonState.Disabled;
			if (!this.ReadOnly && this.Enabled)
			{
				if (this.buttonPressed)							buttonState = ButtonState.Pressed;
				else if (this.buttonHovered || this.Focused)	buttonState = ButtonState.Hot;
				else											buttonState = ButtonState.Normal;
			}
			ControlRenderer.DrawButton(
				e.Graphics, 
				this.rectButton, 
				buttonState, 
				null, 
				Properties.GeneralResCache.ColorWheel);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.ShowColorDialog();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.C && e.Control)
			{
				DataObject data = new DataObject();
				data.SetIColorData(new[] { this.value ?? (IColorData)ColorRgba.TransparentBlack });
				Clipboard.SetDataObject(data);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.V && e.Control)
			{
				DataObject data = Clipboard.GetDataObject() as DataObject;
				if (data.ContainsIColorData())
				{
					this.value = data.GetIColorData<IColorData>().FirstOrDefault();
					this.PerformSetValue();
					this.PerformGetValue();
					this.OnEditingFinished(FinishReason.LeapValue);
				}
				else
					System.Media.SystemSounds.Beep.Play();

				e.Handled = true;
			}
			base.OnKeyDown(e);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			bool lastButtonHovered = this.buttonHovered;
			bool lastPanelHovered = this.panelHovered;
			this.buttonHovered = !this.ReadOnly && this.rectButton.Contains(e.Location);
			this.panelHovered = this.rectPanel.Contains(e.Location);
			if (lastButtonHovered != this.buttonHovered || lastPanelHovered != this.panelHovered) this.Invalidate();
			
			if (this.panelDragBegin != Point.Empty)
			{
				if (Math.Abs(this.panelDragBegin.X - e.X) > 5 || Math.Abs(this.panelDragBegin.Y - e.Y) > 5)
				{
					DataObject dragDropData = new DataObject();
					dragDropData.SetIColorData(new[] { this.value });
					this.ParentGrid.DoDragDrop(dragDropData, DragDropEffects.All | DragDropEffects.Link);
				}
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.buttonHovered || this.panelHovered) this.Invalidate();
			this.buttonHovered = false;
			this.panelHovered = false;
			this.panelDragBegin = Point.Empty;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.buttonHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.buttonPressed = true;
				this.Invalidate();
			}
			else if (this.panelHovered)
			{
				this.panelDragBegin = e.Location;
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this.buttonPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				if (this.buttonPressed && this.buttonHovered) this.ShowColorDialog();
				this.buttonPressed = false;
				this.Invalidate();
			}
		}
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			if (this.panelHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
				this.ShowColorDialog();
		}
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			DataObject data = e.Data as DataObject;
			if (data.ContainsIColorData()) e.Effect = e.AllowedEffect;
		}
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			DataObject data = e.Data as DataObject;

			if (data.ContainsIColorData())
			{
				this.value = data.GetIColorData<IColorData>().FirstOrDefault();
				this.PerformSetValue();
				this.PerformGetValue();
				this.OnEditingFinished(FinishReason.LeapValue);
				e.Effect = e.AllowedEffect;
			}
		}

		protected override void UpdateGeometry()
		{
			base.UpdateGeometry();

			this.rectButton = new Rectangle(
				this.ClientRectangle.Right - 22,
				this.ClientRectangle.Top,
				22,
				this.ClientRectangle.Height);
			this.rectPanel = new Rectangle(
				this.ClientRectangle.X,
				this.ClientRectangle.Y,
				this.ClientRectangle.Width - this.rectButton.Width - 2,
				this.ClientRectangle.Height - 1);
		}
	}
}

