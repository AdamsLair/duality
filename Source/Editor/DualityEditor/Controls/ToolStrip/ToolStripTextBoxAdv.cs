using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;
using System.ComponentModel;

namespace Duality.Editor.Controls.ToolStrip
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripTextBoxAdv : ToolStripControlHost
	{
		private Panel container = null;
		private TextBox textBox = null;
		private bool acceptsOnlyNumbers = false;
		private bool selectAllOnClick = false;

		public event EventHandler EditingFinished = null;
		public event EventHandler ProceedRequested = null;
		public event EventHandler ValueChanged
		{
			add { this.textBox.TextChanged += value; }
			remove { this.textBox.TextChanged -= value; }
		}

		public override string Text
		{
			get { return this.textBox.Text; }
			set
			{
				bool selectAllAfterChange = 
					this.textBox.SelectionStart != 0 || 
					this.textBox.SelectionLength > 0;

				this.textBox.Text = value;

				if (selectAllAfterChange)
					this.textBox.SelectAll();
			}
		}
		public override Color BackColor
		{
			get { return this.container.BackColor; }
			set
			{
				this.container.BackColor = value;
				this.textBox.BackColor = value;
			}
		}
		public override Color ForeColor
		{
			get { return this.textBox.ForeColor; }
			set { this.textBox.ForeColor = value; }
		}
		public override bool Focused
		{
			get { return this.textBox.Focused; }
		}
		public int MaxLength
		{
			get { return this.textBox.MaxLength; }
			set { this.textBox.MaxLength = value; }
		}
		public bool AcceptsOnlyNumbers
		{
			get { return this.acceptsOnlyNumbers; }
			set { this.acceptsOnlyNumbers = value; }
		}

		public ToolStripTextBoxAdv(string name) : base(new Panel(), name)
		{
			this.container = (Panel)this.Control;
			this.container.Padding = new Padding(0, 2, 0, 1);
			this.container.Height = 20;
			this.container.BorderStyle = BorderStyle.FixedSingle;
			this.container.BackColor = this.BackColor;

			this.textBox = new TextBox();
			this.textBox.Dock = DockStyle.Fill;
			this.textBox.BorderStyle = BorderStyle.None;
			this.textBox.TextAlign = HorizontalAlignment.Center;
			this.textBox.BackColor = this.BackColor;
			this.textBox.Multiline = false;
			this.textBox.GotFocus += this.textBox_GotFocus;
			this.textBox.LostFocus += this.textBox_LostFocus;
			this.textBox.MouseClick += this.textBox_MouseClick;
			this.textBox.KeyUp += this.textBox_KeyUp;
			this.textBox.KeyDown += this.textBox_KeyDown;
			this.textBox.KeyPress += this.textBox_KeyPress;

			this.container.Controls.Add(this.textBox);
		}

		private bool IsFilteredChar(char input)
		{
			if (this.acceptsOnlyNumbers && !char.IsNumber(input) && !char.IsPunctuation(input))
				return true;

			return false;
		}
		private bool IsProceedKey(Keys keyCode)
		{
			return
				keyCode == Keys.Return || 
				keyCode == Keys.Enter ||
				keyCode == Keys.Tab;
		}
		private bool IsKeyInputExceedingLength()
		{
			return 
				this.textBox.MaxLength > 0 &&
				this.textBox.SelectionLength == 0 &&
				this.textBox.TextLength >= this.textBox.MaxLength;
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.textBox.Focus();
		}
		private void OnEditingFinished()
		{
			if (this.EditingFinished != null)
				this.EditingFinished(this, EventArgs.Empty);
		}
		private void OnProceedRequested()
		{
			if (this.ProceedRequested != null)
				this.ProceedRequested(this, EventArgs.Empty);
		}

		private void textBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (this.selectAllOnClick)
			{
				this.textBox.SelectAll();
				this.selectAllOnClick = false;
			}
		}
		private void textBox_GotFocus(object sender, EventArgs e)
		{
			this.textBox.SelectAll();
			this.selectAllOnClick = true;
		}
		private void textBox_LostFocus(object sender, EventArgs e)
		{
			this.OnEditingFinished();
		}
		private void textBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.IsProceedKey(e.KeyCode) || this.IsKeyInputExceedingLength())
			{
				e.Handled = true;
				e.SuppressKeyPress = true;

				this.OnEditingFinished();
				this.OnProceedRequested();
			}
		}
		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.IsProceedKey(e.KeyCode) || this.IsKeyInputExceedingLength())
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}
		private void textBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.IsFilteredChar(e.KeyChar))
				e.Handled = true;
		}

	}
}
