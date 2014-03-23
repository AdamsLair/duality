using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using AdamsLair.PropertyGrid.Renderer;
using ButtonState = AdamsLair.PropertyGrid.Renderer.ButtonState;

namespace AdamsLair.PropertyGrid.EditorTemplates
{
	public class MultiComboBoxEditorTemplate : EditorTemplate, IPopupControlHost
	{
		private const string ClipboardDataFormat = "MultiComboBoxEditorTemplateData";

		private	List<object>			selectedObjects	= new List<object>();
		private	string					selectedObjStr	= "";
		private	bool					pressed			= false;
		private	DateTime				mouseClosed		= DateTime.MinValue;
		private	int						dropdownHeight	= 100;
		private	MultiComboBoxDropDown	dropdown		= null;
		private	List<object>			dropdownItems	= new List<object>();
        private PopupControl			popupControl	= new PopupControl();

		public IEnumerable<object> SelectedObjects
		{
			get { return this.selectedObjects; }
			set
			{
				if (this.IsDropDownOpened) return; // Don't override while the user is selecting
				string lastObjStr = this.selectedObjStr;

				this.selectedObjects = value.ToList();
				this.selectedObjStr = this.DefaultValueStringGenerator(this.selectedObjects);
				if (this.dropdown != null)
				{
					for (int i = 0; i < this.dropdown.Items.Count; i++)
						this.dropdown.SetItemChecked(i, value.Contains(this.dropdown.Items[i]));
				}

				if (lastObjStr != this.selectedObjStr) this.EmitInvalidate();
			}
		}
		public bool IsDropDownOpened
		{
			get { return this.dropdown != null; }
		}
		public object DropDownHoveredObject
		{
			get
			{
				if (this.dropdown.HoveredIndex == -1) return null;
				return this.dropdown.Items[this.dropdown.HoveredIndex];
			}
		}
		public int DropDownHeight
		{
			get { return this.dropdownHeight; }
			set { this.dropdownHeight = value; }
		}
		public IEnumerable<object> DropDownItems
		{
			get { return this.dropdownItems; }
			set
			{
				this.dropdownItems = value.ToList();
				if (this.dropdown != null)
				{
					this.dropdown.Items.Clear();
					this.dropdown.Items.AddRange(this.dropdownItems.ToArray());
				}
			}
		}
		
		public MultiComboBoxEditorTemplate(PropertyEditor parent) : base(parent)
		{
			this.popupControl.PopupControlHost = this;
			this.popupControl.Closed += this.popupControl_Closed;
		}

		public void OnPaint(PaintEventArgs e, bool enabled, bool multiple)
		{
			ButtonState comboState = ButtonState.Normal;
			if (!enabled || this.ReadOnly)
				comboState = ButtonState.Disabled;
			else if (this.pressed || this.IsDropDownOpened || (this.focused && (Control.ModifierKeys & Keys.Control) == Keys.Control))
				comboState = ButtonState.Pressed;
			else if (this.hovered || this.focused)
				comboState = ButtonState.Hot;

			ControlRenderer.DrawComboButton(e.Graphics, this.rect, comboState, this.selectedObjStr);
		}
		public override void OnLostFocus(EventArgs e)
		{
			if (this.focused) this.EmitEditingFinished(this.selectedObjects, FinishReason.LostFocus);			
			base.OnLostFocus(e);
		}
		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.ReadOnly) this.hovered = false;
		}
		public void OnMouseDown(MouseEventArgs e)
		{
			if (this.rect.Contains(e.Location))
			{
				if (this.hovered && (e.Button & MouseButtons.Left) != MouseButtons.None && (DateTime.Now - this.mouseClosed).TotalMilliseconds > 200)
				{
					this.pressed = true;
					this.EmitInvalidate();
				}
			}
		}
		public void OnMouseUp(MouseEventArgs e)
		{
			if (this.pressed && (e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				if (this.hovered) this.ShowDropDown();
				this.pressed = false;
				this.EmitInvalidate();
			}
		}
		public void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ControlKey)
				this.EmitInvalidate();
		}
		public void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ControlKey)
				this.EmitInvalidate();

			if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Right || (e.KeyCode == Keys.Down && e.Control))
			{
				this.ShowDropDown();
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.C)
			{
				DataObject data = new DataObject();
				data.SetText(this.selectedObjStr);
				data.SetData(ClipboardDataFormat, this.selectedObjects);
				Clipboard.SetDataObject(data);
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.V)
			{
				bool success = false;
				List<object> pasteObjProxy = null; 
				if (Clipboard.ContainsData(ClipboardDataFormat))
				{
					List<object> pasteObj = Clipboard.GetData(ClipboardDataFormat) as List<object>;
					pasteObjProxy = pasteObj.Select(p => this.dropdownItems.FirstOrDefault(obj => object.Equals(obj, p))).Where(o => o != null).ToList(); 
				}
				else if (Clipboard.ContainsText())
				{
					string[] pasteObj = Clipboard.GetText().Split(new [] { ", " }, StringSplitOptions.RemoveEmptyEntries);
					pasteObjProxy = pasteObj.Select(p => this.dropdownItems.FirstOrDefault(obj => obj != null && p == obj.ToString())).ToList(); 
				}
				if (pasteObjProxy != null && pasteObjProxy.Count > 0)
				{
					this.selectedObjects = pasteObjProxy;
					this.selectedObjStr = this.DefaultValueStringGenerator(this.selectedObjects);
					this.EmitInvalidate();
					this.EmitEdited(this.selectedObjects);
					success = true;
				}
				if (!success) System.Media.SystemSounds.Beep.Play();
				e.Handled = true;
			}
		}

		public void ShowDropDown()
		{
			if (this.dropdown != null) return;
			if (this.ReadOnly) return;
			PropertyGrid parentGrid = this.Parent.ParentGrid;

			this.dropdown = new MultiComboBoxDropDown(this.dropdownItems);
			for (int i = 0; i < this.dropdown.Items.Count; i++)
				this.dropdown.SetItemChecked(i, this.selectedObjects.Contains(this.dropdown.Items[i]));

			Size dropDownSize = new Size(
				this.rect.Width, 
				Math.Min(this.dropdownHeight, this.dropdown.PreferredHeight));

			Point dropDownLoc = parentGrid.GetEditorLocation(this.Parent, true);
			//dropDownLoc = parentGrid.PointToScreen(dropDownLoc);
			dropDownLoc.Y += this.rect.Height + 1;
			dropDownLoc.X += this.Parent.Width - this.rect.Width;

			this.dropdown.Location = dropDownLoc;
			this.dropdown.Size = dropDownSize;
			this.dropdown.RequestClose += this.dropdown_RequestClose;
			this.dropdown.AcceptSelection += this.dropdown_AcceptSelection;

			this.popupControl.Show(parentGrid, this.dropdown, dropDownLoc.X, dropDownLoc.Y, dropDownSize.Width, dropDownSize.Height, PopupResizeMode.None);

			this.EmitInvalidate();
		}
		public void HideDropDown()
		{
			if (this.dropdown == null) return;
			if (!this.dropdown.Disposing && !this.dropdown.IsDisposed)
				this.dropdown.Dispose();
			this.dropdown = null;

			this.EmitInvalidate();
		}
		private void dropdown_RequestClose(object sender, EventArgs e)
		{
			this.HideDropDown();
		}
		private void dropdown_AcceptSelection(object sender, EventArgs e)
		{
			if (this.selectedObjects.Any(o => !this.dropdown.CheckedItems.Contains(o)) ||
				this.dropdown.CheckedItems.Cast<object>().Any(o => !this.selectedObjects.Contains(o)))
			{
				this.selectedObjects = this.dropdown.CheckedItems.Cast<object>().ToList();
				this.selectedObjStr = this.DefaultValueStringGenerator(this.selectedObjects);
				this.EmitInvalidate();
				this.EmitEdited(this.selectedObjects);
			}
		}
		private void popupControl_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this.HideDropDown();
			this.mouseClosed = DateTime.Now;
		}

		protected string DefaultValueStringGenerator(IEnumerable<object> objEnum)
		{
			string valueString = "";
			foreach (object obj in objEnum)
				valueString += (valueString.Length > 0 ? ", " : "") + obj.ToString();
			return valueString;
		}
	}
}
