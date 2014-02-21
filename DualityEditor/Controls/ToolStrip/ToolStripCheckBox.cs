using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Duality.Editor.Controls.ToolStrip
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripCheckBox : ToolStripControlHost
	{
		private CheckBox checkBox = null;
		
		public event EventHandler CheckedChanged
		{
			add { this.checkBox.CheckedChanged += value; }
			remove { this.checkBox.CheckedChanged -= value; }
		}
		public event EventHandler CheckStateChanged
		{
			add { this.checkBox.CheckStateChanged += value; }
			remove { this.checkBox.CheckStateChanged -= value; }
		}

		public override string Text
		{
			get { return this.checkBox.Text; }
			set 
			{ 
				this.checkBox.Text = value;
				this.UpdateAutoSize();
			}
		}
		public bool Checked
		{
			get { return this.checkBox.Checked; }
			set { this.checkBox.Checked = value; }
		}
		public CheckState CheckState
		{
			get { return this.checkBox.CheckState; }
			set { this.checkBox.CheckState = value; }
		}

		public ToolStripCheckBox() : base(new CheckBox())
		{
			// Set up the FlowLayouPanel.
			this.checkBox = (CheckBox)base.Control;
			this.checkBox.AutoSize = true;
		}

		protected void UpdateAutoSize()
		{
			if (!this.AutoSize) return;
			this.AutoSize = false;
			this.AutoSize = true;
		}
	}
}
