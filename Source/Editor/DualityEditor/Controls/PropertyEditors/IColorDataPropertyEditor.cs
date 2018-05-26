using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using AdamsLair.WinForms.ColorControls;
using AdamsLair.WinForms.PropertyEditing;
using ButtonState = AdamsLair.WinForms.Drawing.ButtonState;

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
		protected	Rectangle	rectCDiagButton	= Rectangle.Empty;
		protected	Rectangle	rectCPickButton	= Rectangle.Empty;
		protected	bool		buttonCDiagHovered	= false;
		protected	bool		buttonCDiagPressed	= false;
		protected	bool		buttonCPickHovered	= false;
		protected	bool		buttonCPickPressed	= false;
		protected	bool		panelHovered		= false;
		private		Point		panelDragBegin		= Point.Empty;
		private		IColorData	valueBeforePicking	= null;
		private		GlobalColorPickOperation	pickingOperation	= null;

		public override object DisplayedValue
		{
			get { return value.ConvertTo(this.EditedType); }
		}


		public IColorDataPropertyEditor()
		{
			this.Height = 22;
			this.dialog.ColorEdited += this.dialog_ColorEdited;
		}

		private void ShowColorDialog()
		{
			this.dialog.OldColor = (this.value ?? ColorRgba.TransparentBlack).ToSysDrawColor();
			this.dialog.SelectedColor = this.dialog.OldColor;
			DialogResult result = this.dialog.ShowDialog(this.ParentGrid);

			this.value = (result == DialogResult.OK) ? this.dialog.SelectedColor.ToDualityRgba() : this.dialog.OldColor.ToDualityRgba();
			this.PerformSetValue();
			this.PerformGetValue();
			this.OnEditingFinished(result == DialogResult.OK ? FinishReason.UserAccept : FinishReason.LostFocus);
		}
		private void StartColorPick()
		{
			this.valueBeforePicking = this.value;

			if (this.pickingOperation == null)
				this.pickingOperation = new GlobalColorPickOperation();

			this.pickingOperation.PickedColorChanged += this.pickingOperation_PickedColorChanged;
			this.pickingOperation.OperationEnded     += this.pickingOperation_OperationEnded;
			this.pickingOperation.Start();
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

				ColorRgba rgba = this.value.ConvertTo<ColorRgba>();
				Color textColor = rgba.GetLuminance() > 0.5f ? Color.Black : Color.White;
				StringFormat format = StringFormat.GenericDefault;
				format.Alignment = StringAlignment.Center;
				format.LineAlignment = StringAlignment.Center;
				format.Trimming = StringTrimming.EllipsisCharacter;
				e.Graphics.DrawString(
					string.Format("{0}, {1}, {2}, {3}", rgba.R, rgba.G, rgba.B, rgba.A),
					this.ControlRenderer.FontRegular,
					new SolidBrush(Color.FromArgb(128, textColor)),
					this.rectPanel,
					format);
			}
			e.Graphics.DrawRectangle(SystemPens.ControlLightLight, 
				this.rectPanel.X + 1,
				this.rectPanel.Y + 1,
				this.rectPanel.Width - 2,
				this.rectPanel.Height - 2);
			e.Graphics.DrawRectangle(SystemPens.ControlDark, this.rectPanel);

			ButtonState buttonCDiagState = ButtonState.Disabled;
			if (!this.ReadOnly && this.Enabled)
			{
				if (this.buttonCDiagPressed)                        buttonCDiagState = ButtonState.Pressed;
				else if (this.buttonCDiagHovered)					buttonCDiagState = ButtonState.Hot;
				else                                                buttonCDiagState = ButtonState.Normal;
			}
			ControlRenderer.DrawButton(
				e.Graphics, 
				this.rectCDiagButton,
				buttonCDiagState, 
				null, 
				Properties.GeneralResCache.ColorWheel);

			ButtonState buttonCPickState = ButtonState.Disabled;
			if (!this.ReadOnly && this.Enabled)
			{
				if (this.buttonCPickPressed)                        buttonCPickState = ButtonState.Pressed;
				else if (this.buttonCPickHovered)					buttonCPickState = ButtonState.Hot;
				else                                                buttonCPickState = ButtonState.Normal;
			}
			ControlRenderer.DrawButton(
				e.Graphics,
				this.rectCPickButton,
				buttonCPickState,
				null,
				Properties.GeneralResCache.ColorPick);
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
				IColorData[] colorData;
				if (data.TryGetIColorData(out colorData))
				{
					this.value = colorData.FirstOrDefault();
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

			bool lastCDiagButtonHovered = this.buttonCDiagHovered;
			bool lastCPickButtonHovered = this.buttonCPickHovered;
			bool lastPanelHovered = this.panelHovered;
			this.buttonCDiagHovered = !this.ReadOnly && this.rectCDiagButton.Contains(e.Location);
			this.buttonCPickHovered = !this.ReadOnly && this.rectCPickButton.Contains(e.Location);
			this.panelHovered = this.rectPanel.Contains(e.Location);

			if (lastCDiagButtonHovered != this.buttonCDiagHovered ||
				lastCPickButtonHovered != this.buttonCPickHovered || 
				lastPanelHovered != this.panelHovered) 
				this.Invalidate();
			
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
			if (this.buttonCDiagHovered || this.buttonCPickHovered || this.panelHovered) this.Invalidate();
			this.buttonCDiagHovered = false;
			this.buttonCPickHovered = false;
			this.panelHovered = false;
			this.panelDragBegin = Point.Empty;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.buttonCDiagHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.buttonCDiagPressed = true;
				this.Invalidate();
			}
			if (this.buttonCPickHovered && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				this.buttonCPickPressed = true;
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
			if (this.buttonCDiagPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				if (this.buttonCDiagPressed && this.buttonCDiagHovered) this.ShowColorDialog();
				this.buttonCDiagPressed = false;
				this.Invalidate();
			}
			if (this.buttonCPickPressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				if (this.buttonCPickPressed && this.buttonCPickHovered) this.StartColorPick();
				this.buttonCPickPressed = false;
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

			this.rectCDiagButton = new Rectangle(
				this.ClientRectangle.Right - 22,
				this.ClientRectangle.Top,
				22,
				this.ClientRectangle.Height);
			this.rectCPickButton = new Rectangle(
				this.ClientRectangle.Right - this.rectCDiagButton.Width - 22,
				this.ClientRectangle.Top,
				22,
				this.ClientRectangle.Height);
			this.rectPanel = new Rectangle(
				this.ClientRectangle.X,
				this.ClientRectangle.Y,
				this.ClientRectangle.Width - this.rectCDiagButton.Width - this.rectCPickButton.Width - 1,
				this.ClientRectangle.Height - 1);
		}
		
		private void dialog_ColorEdited(object sender, EventArgs e)
		{
			this.value = this.dialog.SelectedColor.ToDualityRgba();
			this.PerformSetValue();
			this.PerformGetValue();
		}
		private void pickingOperation_PickedColorChanged(object sender, EventArgs e)
		{
			this.value = this.pickingOperation.PickedColor.ToDualityRgba();
			this.PerformSetValue();
			this.PerformGetValue();
			this.Invalidate();
		}
		private void pickingOperation_OperationEnded(object sender, EventArgs e)
		{
			if (this.pickingOperation.IsCanceled)
			{
				this.value = this.valueBeforePicking;
				this.PerformSetValue();
				this.PerformGetValue();
			}
			this.pickingOperation.PickedColorChanged -= this.pickingOperation_PickedColorChanged;
			this.pickingOperation.OperationEnded     -= this.pickingOperation_OperationEnded;
		}
	}
}

