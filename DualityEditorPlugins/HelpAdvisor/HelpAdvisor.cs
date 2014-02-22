using System;
using System.Drawing;
using System.Windows.Forms;

using Duality.Editor;
using Duality.Editor.Plugins.HelpAdvisor.Properties;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.HelpAdvisor
{
	public partial class HelpAdvisor : DockContent
	{
		private static	HelpInfo	advisorHelp = HelpInfo.FromText(HelpAdvisorRes.HelpInfo_Advisor_Topic, HelpAdvisorRes.HelpInfo_Advisor_Desc);

		private	IHelpInfoReader	newHelp		= null;
		private	IHelpInfoReader	currentHelp	= null;
		private	IHelpInfoReader	lastHelp	= null;
		private	DateTime	changeTime	= DateTime.Now;

		private float AnimProgress
		{
			get { return Math.Min(Math.Max((float)(DateTime.Now - this.changeTime).TotalMilliseconds / 100.0f, 0.0f), 1.0f); }
		}

		public HelpAdvisor()
		{
			this.InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			HelpSystem.ActiveHelpChanged += this.Help_ActiveHelpChanged;
			this.UpdateHelp();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			HelpSystem.ActiveHelpChanged -= this.Help_ActiveHelpChanged;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			RectangleF topicRect = new RectangleF(this.labelTopic.Location.X, this.labelTopic.Location.Y, this.labelTopic.Width, this.labelTopic.Height);
			RectangleF descRect = new RectangleF(this.labelDescription.Location.X, this.labelDescription.Location.Y, this.labelDescription.Width, this.labelDescription.Height);

			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			Color lastColorBase = this.lastHelp == advisorHelp ? SystemColors.GrayText : SystemColors.ControlText;
			Color curColorBase = this.currentHelp == advisorHelp ? SystemColors.GrayText : SystemColors.ControlText;
			Color lastColor = Color.FromArgb((int)((1.0f - this.AnimProgress) * 255.0f), lastColorBase);
			Color curColor = Color.FromArgb((int)(this.AnimProgress * 255.0f), curColorBase);

			StringFormat format = StringFormat.GenericDefault;
			format.Alignment = StringAlignment.Near;
			format.LineAlignment = StringAlignment.Center;
			format.Trimming = StringTrimming.EllipsisCharacter;
			format.FormatFlags |= StringFormatFlags.NoWrap;

			if (this.lastHelp != null)
				e.Graphics.DrawString(this.lastHelp.Topic, this.labelTopic.Font, new SolidBrush(lastColor), topicRect, format);
			if (this.currentHelp != null)
				e.Graphics.DrawString(this.currentHelp.Topic, this.labelTopic.Font, new SolidBrush(curColor), topicRect, format);

			format.LineAlignment = StringAlignment.Near;
			format.FormatFlags = 0;

			if (this.lastHelp != null)
				e.Graphics.DrawString(this.lastHelp.Description, this.labelDescription.Font, new SolidBrush(lastColor), descRect, format);
			if (this.currentHelp != null)
				e.Graphics.DrawString(this.currentHelp.Description, this.labelDescription.Font, new SolidBrush(curColor), descRect, format);
		}
		private void animTimer_Tick(object sender, EventArgs e)
		{
			this.Invalidate();
			if (this.AnimProgress >= 1.0f) this.animTimer.Enabled = false;
		}
		private void commitTimer_Tick(object sender, EventArgs e)
		{
			this.CommitHelp();
			this.commitTimer.Enabled = false;
		}

		public void UpdateHelp()
		{
			this.newHelp = HelpSystem.ActiveHelp ?? advisorHelp;

			if (this.newHelp == null || this.currentHelp == null || 
				this.newHelp.Topic != this.currentHelp.Topic || 
				this.newHelp.Description != this.currentHelp.Description)
			{
				this.commitTimer.Stop();
				this.commitTimer.Start();
			}

			if (this.currentHelp == null) this.currentHelp = this.newHelp;
			if (this.lastHelp == null) this.lastHelp = this.currentHelp;
		}
		private void CommitHelp()
		{
			if (this.newHelp.Topic != this.currentHelp.Topic || this.newHelp.Description != this.currentHelp.Description)
			{
				this.lastHelp = this.currentHelp;
				this.currentHelp = newHelp;
				this.changeTime = DateTime.Now;
				this.animTimer.Enabled = true;
			}
		}

		private void Help_ActiveHelpChanged(object sender, HelpStackChangedEventArgs e)
		{
			this.UpdateHelp();
		}
	}
}
