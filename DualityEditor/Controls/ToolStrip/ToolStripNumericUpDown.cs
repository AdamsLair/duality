using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;
using System.ComponentModel;

namespace Duality.Editor.Controls.ToolStrip
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripNumericUpDown : ToolStripControlHost
	{
		private FlowLayoutPanel	controlPanel	= null;
		private NumericUpDown	num				= null;
		private Label			txt				= null;

		public event EventHandler ValueChanged
		{
			add { this.num.ValueChanged += value; }
			remove { this.num.ValueChanged -= value; }
		}

		[DefaultValue(true)]
		public bool TextVisible
		{
			get { return this.txt.Visible; }
			set 
			{ 
				this.txt.Visible = value;
				this.UpdateAutoSize();
			}
		}
		public override string Text
		{
			get { return this.txt.Text; }
			set 
			{ 
				this.txt.Text = value;
				this.UpdateAutoSize();
			}
		}
		public override Size Size
		{
			get { return base.Size; }
			set
			{
				if (base.Size != value && !this.AutoSize)
				{
					base.Size = value;
					this.OnSizeChanged();
				}
			}
		}
		[DefaultValue(50)]
		public int NumericWidth
		{
			get { return this.num.Width; }
			set 
			{ 
				this.num.Width = value;
				this.UpdateAutoSize();
			}
		}
		[DefaultValue(typeof(decimal), "100")]
		public decimal Maximum
		{
			get { return this.num.Maximum; }
			set { this.num.Maximum = value; }
		}
		[DefaultValue(typeof(decimal), "0")]
		public decimal Minimum
		{
			get { return this.num.Minimum; }
			set { this.num.Minimum = value; }
		}
		[DefaultValue(typeof(decimal), "0")]
		public decimal Value
		{
			get { return this.num.Value; }
			set { this.num.Value = value; }
		}
		[DefaultValue(1)]
		public int DecimalPlaces
		{
			get { return this.num.DecimalPlaces; }
			set { this.num.DecimalPlaces = value; }
		}
		[DefaultValue(typeof(decimal), "1")]
		public decimal Increment
		{
			get { return this.num.Increment; }
			set { this.num.Increment = value; }
		}
		[DefaultValue(false)]
		public bool Hexadecimal
		{
			get { return this.num.Hexadecimal; }
			set { this.num.Hexadecimal = value; }
		}
		[DefaultValue(typeof(HorizontalAlignment), "Center")]
		public HorizontalAlignment NumericTextAlign
		{
			get { return this.num.TextAlign; }
			set { this.num.TextAlign = value; }
		}
		public Color NumBackColor
		{
			get { return this.num.BackColor; }
			set { this.num.BackColor = value; }
		}

		public ToolStripNumericUpDown() : base(new FlowLayoutPanel())
		{
			// Set up the FlowLayouPanel.
			this.controlPanel = (FlowLayoutPanel)Control;
			this.controlPanel.BackColor = Color.Transparent;
			this.controlPanel.WrapContents = false;
			this.controlPanel.AutoSize = true;
			this.controlPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
 
			// Add child controls.
			this.num = new NumericUpDown();
			this.num.Width = 50;
			this.num.Height = this.num.PreferredHeight;
			this.num.Margin = new Padding(0, 1, 3, 1);
			this.num.Value = 0;
			this.num.Minimum = 0;
			this.num.Maximum = 100;
			this.num.DecimalPlaces = 0;
			this.num.Increment = 1;
			this.num.Hexadecimal = false;
			this.num.TextAlign = HorizontalAlignment.Center;

			this.txt = new Label();
			this.txt.Text = "NumericUpDown";
			this.txt.TextAlign = ContentAlignment.MiddleRight;
			this.txt.AutoSize = true;
			this.txt.Dock = DockStyle.Left;

			this.controlPanel.Controls.Add(this.txt);
			this.controlPanel.Controls.Add(this.num);
		}

		protected void UpdateAutoSize()
		{
			if (!this.AutoSize) return;
			this.AutoSize = false;
			this.AutoSize = true;
		}
		protected void OnSizeChanged()
		{
			if (this.num != null && this.controlPanel != null && this.txt != null)
			{
				this.num.Width = this.controlPanel.ClientSize.Width - this.txt.Width - this.controlPanel.Margin.Horizontal - this.controlPanel.Margin.Horizontal;
			}
		}
	}
}
