using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdamsLair.PropertyGrid.EditorTemplates
{
	public partial class ComboBoxDropDown : ListBox
	{
		private bool openedWithCtrl	= false;
		private int hoveredIndex = -1;

		public event EventHandler AcceptSelection = null;
		public event EventHandler RequestClose = null;

		public int HoveredIndex
		{
			get { return this.hoveredIndex; }
		}

		public ComboBoxDropDown()
		{
			this.IntegralHeight = false;
			this.Font = this.Font; // Prevents parent PopupControl from interfering on resize.
		}
		public ComboBoxDropDown(IEnumerable<object> items) : this()
		{
			this.Items.AddRange(items.ToArray());
		}

		protected void Accept()
		{
			if (this.AcceptSelection != null) this.AcceptSelection(this, EventArgs.Empty);
		}
		protected void Close()
		{
			if (this.RequestClose != null) this.RequestClose(this, EventArgs.Empty);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.openedWithCtrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
		}
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (this.SelectionMode == SelectionMode.One)
			{
				this.Accept();
				this.Close();
			}
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Left)
			{
				this.Accept();
				this.Close();
			}
			else if (e.KeyCode == Keys.C && e.Control)
			{
				if (this.SelectedItem != null)
					Clipboard.SetText(this.SelectedItem.ToString());
			}
			else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.KeyCode == Keys.ControlKey && this.openedWithCtrl)
			{
				this.Accept();
				this.Close();
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			int clientY = e.Y - this.ClientRectangle.Y;
			this.hoveredIndex = (clientY / this.ItemHeight) + this.TopIndex;
			if (this.hoveredIndex < 0 || this.hoveredIndex >= this.Items.Count) this.hoveredIndex = -1;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.hoveredIndex = -1;
		}
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			if (this.openedWithCtrl) this.Accept();
		}
	}
}
